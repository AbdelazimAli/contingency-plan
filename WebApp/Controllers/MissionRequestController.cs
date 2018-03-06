using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApp.Extensions;
using System.Web.Script.Serialization;
using WebApp.Models;
using System;
using System.Linq.Dynamic;
using System.Web.Routing;
using Model.ViewModel.MissionRequest;
using Model.ViewModel.Personnel;
using System.Web.Helpers;
using System.IO;
using Db.Persistence;
using Db.Persistence.Services;
namespace WebApp.Controllers
{
    public class MissionRequestController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
                UserName = requestContext.HttpContext.User.Identity.Name;
            }
        }
        public MissionRequestController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: MissionRequest
        public ActionResult Index(int id = 0)
        {
            if (id == 2)
                ViewBag.MangRole = true;
            else
            { ViewBag.MangRole = false; }

            return View();
        }

        public ActionResult GetMission(int MenuId, int pageSize, byte Tab, int skip, byte? Range, DateTime? Start, DateTime? End)
        {
            IQueryable<ErrandIndexRequestViewModel> query;
            if (Range >= 0 && Range < 10)

                query = _hrUnitOfWork.MissionRepository.ReadMissionRequestArchieve(CompanyId, Range ?? 10, Start, End, Language);
            else
            {
                query = _hrUnitOfWork.MissionRepository.ReadMissionRequest(CompanyId, Tab, Range ?? 10, Start, End, Language);
            }
            return ApplyFilter<ErrandIndexRequestViewModel>(query, null, MenuId, pageSize, skip);
          
        }

        public ActionResult MissionDetails(int id = 0, byte Version = 0)
        {
            if (!_hrUnitOfWork.LeaveRepository.CheckAutoCompleteColumn("ErrandRequests", CompanyId, Version, "EmpId"))
            {
                var mang = Request.QueryString["Manager"]?.ToString();
                if (mang != null)
                {
                    var MangerData = User.Identity.GetEmpId();
                    ViewBag.Mangers = _hrUnitOfWork.PeopleRepository.GetActiveMangersByMangerId(CompanyId, Language, MangerData);
                }
                else
                {
                    ViewBag.Mangers = _hrUnitOfWork.PeopleRepository.GetActiveMangers(CompanyId, Language);
                }
            }
            ViewBag.Sites = _hrUnitOfWork.SiteRepository.ReadSites(Language, CompanyId).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language);
            if (id == 0) return View(new ErrandFormRequestViewModel());
            var Mission = _hrUnitOfWork.MissionRepository.ReadMissionRequest(id);
            return Mission == null ? (ActionResult)HttpNotFound() : View(Mission);
        }

        public ActionResult GetSiteInfo(int SiteId,byte ErrandType,int EmpId)
        {
            var Info = _hrUnitOfWork.MissionRepository.GetFullSiteInfo(SiteId,ErrandType,EmpId, Language);
            return Json(new { Info = Info }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveMissionRequest(ErrandFormRequestViewModel model, OptionsViewModel moreInfo, bool clear = false)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "ErrandRequest",
                        TableName = "ErrandRequests",
                        Columns = Models.Utils.GetColumnViews(ModelState).Where(s => s.Name != "MultiDays").ToList(),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        foreach (var e in errors)
                        {
                            foreach (var errorMsg in e.errors)
                            {
                                ModelState.AddModelError(errorMsg.field, errorMsg.message);
                            }
                        }

                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
                bool IsValid = true;
                string conflictMessage = string.Empty;
                string message = "OK";
                if (model.MultiDays)
                {
                    // check Server date with Start date 
                    if (model.StartDate.Date < DateTime.Today.Date)
                    {
                        ModelState.AddModelError("StartDate", MsgUtils.Instance.Trls("Errand DateLess than Now"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }

                    IsValid = Constants.CheckDateRangAndTasksConflict.IsValid(_hrUnitOfWork, model.EmpId.ToString(), Constants.Sources.Errand, model.Id, model.StartDate.Date, model.EndDate.Date, DateTime.Today, Language,out conflictMessage);

                }
                else
                {
                    var currentime = DateTime.Now.GetUniversalDatetime(User.Identity.GetTimeZone()); //TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(User.Identity.GetTimeZone()));
                    if (Convert.ToDateTime(model.StartTime) < currentime)
                    {
                        ModelState.AddModelError("StartTime", MsgUtils.Instance.Trls("Start Less than Now"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                   
                        IsValid = Constants.CheckDateRangAndTasksConflict.IsValid(_hrUnitOfWork, model.EmpId.ToString(), Constants.Sources.Errand, model.Id, Convert.ToDateTime(model.StartTime), Convert.ToDateTime(model.EndTime), DateTime.Today, Language,out conflictMessage);
                  
                }
                if (!IsValid)
                {
                    
                    ModelState.AddModelError("", conflictMessage);
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }

                var record = _hrUnitOfWork.Repository<ErrandRequest>().FirstOrDefault(j => j.Id == model.Id);
                var isRequired = _hrUnitOfWork.Repository<Workflow>().Where(w => w.Source == "ErrandRequest" && w.CompanyId == CompanyId).Select(a => a.IsRequired).FirstOrDefault();

                if (record == null)
                {
                    record = new ErrandRequest();

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "ErrandRequest",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    record.CompanyId = CompanyId;
                    record.ApprovalStatus = (byte)(model.submit ? (isRequired ? 2 : 6) : 1);
                    if (model.MultiDays)
                    {
                        record.StartDate = model.StartDate;
                        record.EndDate = model.EndDate;
                    }
                    else
                    {
                        record.StartDate = Convert.ToDateTime(model.StartTime);
                        record.EndDate = Convert.ToDateTime(model.EndTime);
                    }

                    _hrUnitOfWork.MissionRepository.Add(record);


                }
                else
                {

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "ErrandRequest",
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });

                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.CompanyId = CompanyId;
                    record.ApprovalStatus = (byte)(model.submit ? (isRequired ? 2 : 6) : 1);

                    if (model.MultiDays)
                    {
                        record.StartDate = model.StartDate;
                        record.EndDate = model.EndDate;
                    }
                    else
                    {
                        record.StartDate = Convert.ToDateTime(model.StartTime);
                        record.EndDate = Convert.ToDateTime(model.EndTime);
                    }
                    _hrUnitOfWork.MissionRepository.Attach(record);
                    _hrUnitOfWork.MissionRepository.Entry(record).State = EntityState.Modified;

                }
                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                var trans = _hrUnitOfWork.BeginTransaction();
                var Errors = SaveChanges(Language);

                if (Errors.Count > 0)
                {
                    message = Errors.First().errors.First().message;
                    trans.Rollback();
                    trans.Dispose();
                    return Json(message);
                }
                #region workflow
                if (isRequired && model.submit)
                {
                    WfViewModel wf = new WfViewModel()
                    {
                        Source = "ErrandRequest",
                        SourceId = record.CompanyId,
                        DocumentId = record.Id,
                        RequesterEmpId = record.EmpId,
                        ApprovalStatus = 2,
                        CreatedUser = UserName,
                    };

                    var wfTrans = _hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, Language);
                    if (wfTrans == null && wf.WorkFlowStatus != "Success")
                    {
                        record.ApprovalStatus = 1;
                        message += "," + (new JavaScriptSerializer()).Serialize(new { model = record, error = wf.WorkFlowStatus });

                        _hrUnitOfWork.MissionRepository.Attach(record);
                        _hrUnitOfWork.MissionRepository.Entry(record).State = EntityState.Modified;
                    }
                    else if (wfTrans != null)
                        _hrUnitOfWork.LeaveRepository.Add(wfTrans);


                    errors = Save(Language);
                    if (Errors.Count > 0)
                    {
                        message = Errors.First().errors.First().message;
                        trans.Rollback();
                        trans.Dispose();
                        return Json(message);
                    }
                }
                #endregion

                if (message == "OK")
                {
                    if (clear)
                    {
                        model = new ErrandFormRequestViewModel();
                        message += "," + (new JavaScriptSerializer()).Serialize(model);
                    }
                    else
                    {
                        message += "," + (new JavaScriptSerializer()).Serialize(record);
                    }

                    trans.Commit();
                }

                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
       
        public ActionResult CloseMission(int Id)
        {
            var record = _hrUnitOfWork.MissionRepository.CloseMission(Id);
            CloseMissionCiewModel Model = new CloseMissionCiewModel();
            Model.ValidFileExtensions = "'.jpg','.jpeg','.bmp','.png','.gif'";/*,'.doc','.docx','.xls','.xlsx'*/
            Model.Expenses = record.Expenses;
            Model.Notes = record.Notes;
            ViewBag.Id = Id;
            Model.ErrandId = Id;
            return PartialView("_CloseMission", Model);
        }

        public ActionResult SaveCloseMission(CloseMissionCiewModel Model)
        {

            var message = "OK";
            var trans = _hrUnitOfWork.BeginTransaction();
            var errors = UploadOneFile("ErrandRequest", Model.ErrandId.ToString());
            if (errors.Count > 0)
            {
                trans.Rollback();
                trans.Dispose();
                message = errors.First().errors.First().message;
                return Json(message);
            }
            var record = _hrUnitOfWork.Repository<ErrandRequest>().Where(a => a.Id == Model.ErrandId).FirstOrDefault();
            record.Notes = Model.Notes;
            record.Expenses = Model.Expenses;
            errors = Save(Language);
            if (errors.Count > 0)
            {
                trans.Rollback();
                trans.Dispose();
                message = errors.First().errors.First().message;
                return Json(message);
            }
            trans.Commit();
            trans.Dispose();

            return Json(message);
        }

        [HttpPost]
        public List<Error> UploadOneFile(string Source, string SourceId)
        {
            List<Error> errors = new List<Error>();
            var imageData = _hrUnitOfWork.MissionRepository.GetBytes(int.Parse(SourceId));
            if (imageData != null) return errors;
            var file = HttpContext.Request.Files[0];
            int sid = Convert.ToInt32(SourceId);
            var stream = ImageProcesses.ReadFully(file.InputStream);
            WebImage fullsize = null;
            WebImage thumbs = null;
            fullsize = new WebImage(stream).Resize(1240, 1754);
            thumbs = new WebImage(stream).Resize(124, 175);

            CompanyDocsViews doc = new CompanyDocsViews()
            {
                CompanyId = CompanyId,
                name = file.FileName,
                CreatedUser = UserName,
                Source = Source,
                SourceId = sid,
                file_stream = fullsize != null ? fullsize.GetBytes() : stream,
                thumbs = thumbs != null ? thumbs.GetBytes() : null
            };

            _hrUnitOfWork.CompanyRepository.Add(doc);
            errors = Save(Language);
            return errors;
        }

        public ActionResult GetImage(int ID)
        {
            var imageData = _hrUnitOfWork.MissionRepository.GetBytes(ID);

            return File(imageData, "image/jpg");
        }

    }
}