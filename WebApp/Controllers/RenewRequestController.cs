using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class RenewRequestController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        private string UserName { get; set; }
        private string Language { get; set; }
        private int CompanyId { get; set; }
        public int EmpId { get; set; }
        string serverMapPath = "~/Files/uploadercash/";
        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }
        
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
                UserName = requestContext.HttpContext.User.Identity.Name;
                EmpId = requestContext.HttpContext.User.Identity.GetEmpId();


            }
        }
        public RenewRequestController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;



        }
        // GET: RenewRequest
        public ActionResult Index()
        {
            //var ids = _hrUnitOfWork.PeopleRepository.GetIdsList(CompanyId, EmpId);
            //GetFileList("RenewRequest", ids);
            //_hrUnitOfWork.PeopleRepository.ReadRenewRequestAtt();
            return View();
        }
        public ActionResult GetAllRenew(int MenuId, byte Tab, int pageSize, int skip, byte? Range, DateTime? Start, DateTime? End)
        {
            var version = Request.QueryString["Version"] != null ? byte.Parse(Request.QueryString["Version"]) : 0;
            IQueryable<RenewRequestViewModel> query = _hrUnitOfWork.PeopleRepository.ReadRenewRequestTabs(CompanyId, Tab, Range ?? 10, Start, End, Language, byte.Parse(version.ToString()), EmpId);
            string filter = "";
            string Sorting = "";
            string whecls = GetWhereClause(MenuId);

            query = (IQueryable<RenewRequestViewModel>)Utils.GetFilter(query, ref filter, ref Sorting);
            if (whecls.Length > 0 || filter.Length > 0)
            {
                try
                {
                    if (whecls.Length > 0 && filter.Length == 0)
                        query = query.Where(whecls);
                    else if (filter.Length > 0 && whecls.Length == 0)
                        query = query.Where(filter);
                    else
                        query = query.Where(filter).Where(whecls);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            var total = query.Count();

            if (Sorting.Length > 0 && skip > 0)
                query = query.Skip(skip).Take(pageSize).OrderBy(Sorting);
            else if (Sorting.Length > 0)
                query = query.Take(pageSize).OrderBy(Sorting);
            else if (skip > 0)
                query = query.Skip(skip).Take(pageSize);
            else
                query = query.Take(pageSize);

            return Json(new { total = total, data = query.ToList() }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetImage(int id)
        {
            var stream = _hrUnitOfWork.Repository<CompanyDocsViews>().Where(a => a.Source == "RenewRequest" && a.SourceId == id).Select(a => a.file_stream).FirstOrDefault();
            return Json(Convert.ToBase64String(stream), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var version = byte.Parse(Request.QueryString["Version"]);
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            ViewBag.colDropDown = _hrUnitOfWork.PeopleRepository.GetEditColumn(CompanyId, "People", version, Language, RoleId).Select(g => new { id = g.ColumnName, name = g.Title });
            ViewBag.QualificationId = _hrUnitOfWork.QualificationRepository.GetAll().Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.KafeelId = _hrUnitOfWork.LookUpRepository.GetAllKafeels().Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.Status = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("MaritalStat", Language).Select(a => new { id = a.CodeId, name = a.Title }).ToList();
            if (id == 0)
            {
                return View(new RenewRequestViewModel() { EmpId = EmpId, CompanyId = CompanyId });
            }
            RenewRequestViewModel request = _hrUnitOfWork.PeopleRepository.GetRenewRequest(id);
            return View(request);


        }
        [HttpPost]
        public ActionResult Details(RenewRequestViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "RenewRequest",
                        TableName = "RenewRequests",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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
            }
            else
                return Json(Models.Utils.ParseFormErrors(ModelState));

            RenewRequest request = _hrUnitOfWork.PeopleRepository.Getrequest(model.Id);
            byte version;
            byte.TryParse(Request.Form["version"], out version);
            string message = "OK";
            if (model.Id == 0)
            {
                request = new RenewRequest();
                //if (model.ColumnName== "QualificationId")
                //{
                //    model.OldValueId = int.Parse(model.OldValue);
                //    model.NewValueId = int.Parse(model.NewValue);
                //    model.NewValue= _hrUnitOfWork.QualificationRepository.GetAll().Where(qq => qq.Id == int.Parse(model.NewValue)).FirstOrDefault().Name;
                //    model.OldValue = _hrUnitOfWork.QualificationRepository.GetAll().Where(qq => qq.Id == int.Parse(model.OldValue)).FirstOrDefault().Name;
                //}
                //if (model.ColumnName == "MaritalStat")
                //{
                //    model.OldValueId = int.Parse(model.OldValue);
                //    model.NewValueId = int.Parse(model.NewValue);
                //    model.NewValue = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("MaritalStat", Language).Where(qq => qq.CodeId == int.Parse(model.NewValue)).FirstOrDefault().Name;
                //    model.OldValue = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("MaritalStat", Language).Where(qq => qq.CodeId == int.Parse(model.OldValue)).FirstOrDefault().Name;
                //}
                request.EmpId = EmpId;
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "RenewRequest", Options = moreInfo, Transtype = TransType.Insert };
                AutoMapper(parms);

                request.ApprovalStatus = 2;
                request.CreatedUser = UserName;
                request.CreatedTime = DateTime.Now;
                _hrUnitOfWork.PeopleRepository.Add(request);
            }
            else
            {
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "RenewRequest", Options = moreInfo, Transtype = TransType.Update };
                AutoMapper(parms);
                _hrUnitOfWork.PeopleRepository.Attach(request);
                _hrUnitOfWork.PeopleRepository.Entry(request).State = EntityState.Modified;
            }

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                return Json(message);
            }
            if (message == "OK")
                message += "," + ((new JavaScriptSerializer()).Serialize(request));

            var js = Json(message);
            return Json(message);
        }
        public ActionResult DeleteRequest(int id)
        {
            List<Error> errors = new List<Error>();

            string message = "OK";
            var request = _hrUnitOfWork.PeopleRepository.Getrequest(id);
            _hrUnitOfWork.PeopleRepository.Remove(request);

            errors = SaveChanges(Language);
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }
        public ActionResult GetColumnValue(int EmpId,string ColumnName)
        {
            var colVal = _hrUnitOfWork.PeopleRepository.GetColValue(EmpId, ColumnName);

            return Json(new { colVal = colVal }, JsonRequestBehavior.AllowGet);
            
        }

        public void GetFileList(string Source, IList<int> SourceId)
        {
            var comdocs = _hrUnitOfWork.CompanyRepository.GetDocsViews(Source, SourceId);
            string fullPath = Path.Combine(StorageRoot);
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo dir = new DirectoryInfo(fullPath);
                foreach (var item in comdocs)
                {
                    string filefullpath = Server.MapPath(serverMapPath + item.name);
                    if (!System.IO.File.Exists(filefullpath))
                    {
                        System.IO.File.WriteAllBytes(filefullpath, item.file_stream);
                    }
                    
                }
            }
        }

        public ActionResult Remove(string Source, string SourceId,string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Files/uploadercash"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                         System.IO.File.Delete(physicalPath);
                        int SrcId = int.Parse(SourceId);
                        CompanyDocsViews doc = _hrUnitOfWork.Repository<CompanyDocsViews>().SingleOrDefault(d => d.Source == "RenewRequest" && d.SourceId == SrcId && d.name == fileName);
                        _hrUnitOfWork.CompanyRepository.Remove(doc);
                        SaveChanges(Language);
                       
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }


    }
}