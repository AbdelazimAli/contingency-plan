using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Extensions;

namespace WebApp.Controllers
{
    public class PeopleTrainController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        private string UserName { get; set; }
        private string Language { get; set; }
        private int CompanyId { get; set; }
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
        public PeopleTrainController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        

        #region PeopleTraining
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetPeopleTrain(int MenuId)
        {

            var query = _hrUnitOfWork.TrainingRepository.GetPeopleTrain(Language,CompanyId);
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Details(int id = 0)
        {

            //Get People
            ViewBag.PersonId = _hrUnitOfWork.TrainingRepository.GetPeople(Language).Select(a=> new {id=a.Id,name=a.Title, PicUrl = a.PicUrl, Icon = a.EmpStatus });
            ViewBag.CourseId = _hrUnitOfWork.TrainingRepository.GetTrainCourse(Language, CompanyId).Select(p => new { id = p.Id, name = p.LocalName });
            //  ViewBag.EventId = _hrUnitOfWork.TrainingRepository.GetAllEvents().Select(p => new { id = p.Id, name = p.Name});
            ViewBag.EventId = _hrUnitOfWork.Repository<TrainEvent>().Select(p => new { id = p.Id, name = p.Name });

            ViewBag.CurrCode = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            if (id == 0)
                return View(new PeopleTrainFormViewModel());

            var peopleTrain = _hrUnitOfWork.TrainingRepository.ReadPeopleTrain(id, Language);
            return peopleTrain == null ? (ActionResult)HttpNotFound() : View(peopleTrain);
        }
        [HttpPost]
        public ActionResult Details(PeopleTrainFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
        
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LocationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PeopleTrains",
                        TableName = "PeopleTrain",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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

                var record = _hrUnitOfWork.Repository<PeopleTraining>().FirstOrDefault(a => a.Id == model.Id);
                //insert
                if (record == null)
                {
                    record = new PeopleTraining();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "PeopleTrains",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo, 
                        Transtype = TransType.Insert
                    });
                    
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    record.CompanyId = CompanyId;
                    record.ApprovalStatus = 6;
                   // record.ApprovalStatus = (byte)(model.submit == true ? 2 : 1); //1- New, 2- Submit
                    record.RequestDate = DateTime.Now;                    
                    _hrUnitOfWork.TrainingRepository.Add(record);

                }

                //update
                else
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "PeopleTrains",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    //if (model.submit)
                    //    _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                    //    {
                    //        ColumnName = "ApprovalStatus",
                    //        CompanyId = CompanyId,
                    //        ObjectName = "PeopleTrains",
                    //        SourceId = record.Id,
                    //        UserName = UserName,
                    //        Version = 0,
                    //        ValueAfter = MsgUtils.Instance.Trls("Submit"),
                    //        ValueBefore = MsgUtils.Instance.Trls("Darft")
                    //    });

                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.CompanyId = CompanyId;
                    record.RequestDate = DateTime.Now;
                    record.ApprovalStatus = 6;
                   // record.ApprovalStatus = (byte)(model.submit == true ? 2 : model.ApprovalStatus);
                    _hrUnitOfWork.TrainingRepository.Attach(record);
                    _hrUnitOfWork.TrainingRepository.Entry(record).State = EntityState.Modified;

                }
                var Errors = SaveChanges(Language);
                var message = "OK";
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);

            }

            return Json(Models.Utils.ParseFormErrors(ModelState));
        }
        public ActionResult Delete(int id)
        {
            var message = "OK";
            DataSource<PositionViewModel> Source = new DataSource<PositionViewModel>();

            PeopleTraining PeopleTrain = _hrUnitOfWork.TrainingRepository.GetpeopleTraining(id);
            if (PeopleTrain != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = PeopleTrain,
                    ObjectName = "PeopleTrains ",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.TrainingRepository.Remove(PeopleTrain);
            }
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }

        public ActionResult GetPlanedHoure(int courseId=0)
        {
            var result = _hrUnitOfWork.Repository<TrainCourse>().Where(a => a.Id == courseId).Select(a => a.PlannedHours).FirstOrDefault();
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckEvevtMaxCount(int EventId,int? PTrainId)
        {
            return Json(_hrUnitOfWork.TrainingRepository.CheckCount(EventId, PTrainId), JsonRequestBehavior.AllowGet);
        }


        #endregion
        #region FollowUp
        public ActionResult GetMenuName(int MenuId)
        {
            var x = _hrUnitOfWork.TrainingRepository.GetMenuId(MenuId, CompanyId);
            return Json(x, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TrainFollowUpIndex()
        {
            ViewBag.CanselReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("CancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            ViewBag.Mangers = _hrUnitOfWork.EmployeeRepository.GetManagers(CompanyId, Language).Select(m => new { id = m.Id, name = m.Name });
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadTrainFollowUp(int MenuId)
        {
            //2-submit
            var query = _hrUnitOfWork.TrainingRepository.GetTrainFollowUp(CompanyId, Language);
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                }
            }

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult TrainFollowUpDetails(int Id, byte Version)
        {
            ViewBag.RejectReason = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("CancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });
            PeopleTrainFormViewModel request = _hrUnitOfWork.TrainingRepository.ReadPeopleTrain(Id,Language);
            ViewBag.PersonId = _hrUnitOfWork.TrainingRepository.GetPeople(Language).Select(a => new { id = a.Id, name = a.Title, PicUrl = a.PicUrl, Icon = a.EmpStatus });
            ViewBag.CourseId = _hrUnitOfWork.TrainingRepository.GetTrainCourse(Language, CompanyId).Select(p => new { id = p.Id, name = p.LocalName });
            // ViewBag.EventId = _hrUnitOfWork.TrainingRepository.GetAllEvents().Select(p => new { id = p.Id, name = p.Name });
            ViewBag.EventId = _hrUnitOfWork.Repository<TrainEvent>().Select(p => new { id = p.Id, name = p.Name });
            ViewBag.CurrCode = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            return View(request);
        }

        private string AddWFTrans(PeopleTraining PTrainObj, int? ManagerId, bool? backToEmp)
        {
            WfViewModel wf = new WfViewModel()
            {
                Source = "Training",
                SourceId = CompanyId,
                DocumentId = PTrainObj.Id,
                RequesterEmpId = PTrainObj.EmpId,
                ApprovalStatus = PTrainObj.ApprovalStatus,
                CreatedUser = UserName,
                
            };

            if (ManagerId != null) wf.ManagerId = ManagerId;
            else if (backToEmp != null) wf.BackToEmployee = backToEmp.Value;

            var wfTrans = _hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, Language);
            PTrainObj.WFlowId = wf.WFlowId;
            if (wfTrans == null && wf.WorkFlowStatus != "Success")
                return wf.WorkFlowStatus;
            else if (wfTrans == null && wf.WorkFlowStatus == "Success")
                PTrainObj.ApprovalStatus = 6;
            
            else if (wfTrans != null)
                _hrUnitOfWork.LeaveRepository.Add(wfTrans);

            return "";
        }
        public ActionResult SendTo(int Id, byte? Send, int? ManagerId)
        {
            PeopleTraining PTrainObj = _hrUnitOfWork.TrainingRepository.GetPeopleTraining(Id);
            string error = "";
            if (Send == 1)
            {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = CompanyId,
                    ObjectName = "PeopleTrainFollowUpForm",
                    SourceId = Id.ToString(),
                    UserName = UserName,
                    Version = Convert.ToByte(Request.Form["Version"]),
                    ValueAfter = MsgUtils.Instance.Trls("EmployeeReview"),
                    ValueBefore = MsgUtils.Instance.Trls("Submit")
                });

                PTrainObj.ApprovalStatus = 3;
                error = AddWFTrans(PTrainObj, null, true);
            }
            else if (Send == 2)
            {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = CompanyId,
                    ObjectName = "PeopleTrainFollowUpForm",
                    SourceId = Id.ToString(),
                    UserName = UserName,
                    Version = Convert.ToByte(Request.Form["Version"]),
                    ValueAfter = MsgUtils.Instance.Trls("ManagerReview"),
                    ValueBefore = MsgUtils.Instance.Trls("Submit")
                });
                PTrainObj.ApprovalStatus = 4;
                error = AddWFTrans(PTrainObj, ManagerId, null);
            }
            if (error.Length > 0)
                return Json(error);

            _hrUnitOfWork.TrainingRepository.Attach(PTrainObj);
            _hrUnitOfWork.TrainingRepository.Entry(PTrainObj).State = EntityState.Modified;

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                var message = Errors.First().errors.First().message;
                return Json(message);
            }
            return Json("Ok");
        }
     
        public ActionResult CancelReq(int Id, short? Reason, string Desc)
        {
            PeopleTraining PTrainObj = _hrUnitOfWork.TrainingRepository.GetPeopleTraining(Id);
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelReason",
                CompanyId = CompanyId,
                ObjectName = "PeopleTrainFollowUpForm",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("CancelReason", Language).Where(a => a.CodeId == Reason).Select(b => b.Title).FirstOrDefault(),
                ValueBefore = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("CancelReason", Language).Where(a => a.CodeId == PTrainObj.CancelReason).Select(b => b.Title).FirstOrDefault()
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelDesc",
                CompanyId = CompanyId,
                ObjectName = "PeopleTrainFollowUpForm",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = Desc,
                ValueBefore = PTrainObj.CancelDesc
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "ApprovalStatus",
                CompanyId = CompanyId,
                ObjectName = "PeopleTrainFollowUpForm",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = MsgUtils.Instance.Trls("Cancel before approved"),
                ValueBefore = MsgUtils.Instance.Trls("Submit")
            });
            PTrainObj.CancelDesc = Desc;
            PTrainObj.CancelReason = Reason;
            PTrainObj.ApprovalStatus = 7;

            string error = AddWFTrans(PTrainObj, null, null);
            if (error.Length > 0)
                return Json(error);

            _hrUnitOfWork.TrainingRepository.Attach(PTrainObj);
            _hrUnitOfWork.TrainingRepository.Entry(PTrainObj).State = EntityState.Modified;

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                error = Errors.First().errors.First().message;
                return Json(error);
            }

            return Json("Ok");
        }
        [HttpPost]
        public ActionResult TrainFollowUpDetails(PeopleTrainFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> Errors = new List<Error>();
            PeopleTraining request = _hrUnitOfWork.TrainingRepository.GetPeopleTraining(model.Id);
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    Errors = _hrUnitOfWork.LeaveRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PeopleTrainFollowUpForm",
                        TableName = "PeopleTrain",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        ParentColumn = "EventId",
                        Culture = Language
                    });

                    if (Errors.Count() > 0)
                    {
                        foreach (var e in Errors)
                        {
                            foreach (var errorMsg in e.errors)
                            {
                                ModelState.AddModelError(errorMsg.field, errorMsg.message);
                            }
                        }
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            AutoMapper(new Models.AutoMapperParm
            {
                Destination = request,
                Source = model,
                ObjectName = "PeopleTrainFollowUpForm",
                Version = Convert.ToByte(Request.Form["Version"]),
                Options = moreInfo
            });

            if (request.ApprovalStatus != 9)
            {
                request.RejectDesc = null;
                request.RejectReason = null;
            }

            request.ModifiedTime = DateTime.Now;
            request.ModifiedUser = UserName;

            if (request.ApprovalStatus == 5 || request.ApprovalStatus == 9) //Accepted or Rejected
            {
                string msg = AddWFTrans(request, null, null);
                if (msg.Length > 0)
                    return Json(msg);
            }
            _hrUnitOfWork.TrainingRepository.Attach(request);
            _hrUnitOfWork.TrainingRepository.Entry(request).State = EntityState.Modified;

            Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                var message = Errors.First().errors.First().message;
                return Json(message);
            }

            return Json("OK");
        }

        #endregion

    }
}