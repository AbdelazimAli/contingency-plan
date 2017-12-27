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
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CareerPathController : BaseController
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
        public CareerPathController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: CareerPath
        public ActionResult Index()
        {
             ViewBag.Performance = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Performance", Language).Select(a=> new { value = a.CodeId, text= a.Title});
             ViewBag.jobs = _hrUnitOfWork.JobRepository.ReadJobs(CompanyId,Language,0).Select(a => new { value = a.Id, text = a.LocalName  , isLocal=a.Islocal});
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadCareerPaths(int MenuId)   
        {
            var query = _hrUnitOfWork.JobRepository.ReadCareerPaths(CompanyId);
            string whereclause = GetWhereClause(MenuId);
            if (whereclause.Length > 0)
            {
                try
                {
                    query = query.Where(whereclause);
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
        public ActionResult ReadCareerPathJobs(int Id)
        {
            return Json(_hrUnitOfWork.JobRepository.ReadCareerJobs(Id, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateCareerPath(IEnumerable<CareerPathViewModel> models , OptionsViewModel moreInfo)
        {
            var result = new List<CareerPath>();
            var datasource = new DataSource<CareerPathViewModel>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CareerPaths",
                        TableName = "CareerPaths",
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (CareerPathViewModel c in models)
                {
                    var Path = new CareerPath();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = Path,
                        Source = c,
                        ObjectName = "CareerPaths",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });

                    if (c.StartDate > c.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndMustGrtThanStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }
                    result.Add(Path);
                    Path.CompanyId = Path.IsLocal ? CompanyId : (int?)null;
                    _hrUnitOfWork.JobRepository.Add(Path);
                    
                }
               
                datasource.Errors = SaveChanges(Language);
            }

            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from model in models
                               join r in result on model.Name equals r.Name into g
                               from r in g.DefaultIfEmpty()
                               select new CareerPathViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   Code = model.Code,
                                   Name = model.Name,
                                   IsLocal = model.IsLocal,
                                   StartDate = model.StartDate,
                                   Description = model.Description,
                                   EndDate = model.EndDate,
                               }).ToList();

            if (datasource.Errors.Count() > 0)
            {
                datasource.Data = models;
                return Json(datasource);
            }
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateCareerPath(IEnumerable<CareerPathViewModel> models, IEnumerable<OptionsViewModel> options)
        {
          
            var datasource = new DataSource<CareerPathViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CareerPaths",
                        TableName = "CareerPaths",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }

                    for (var i = 0; i < models.Count(); i++)
                    {
                        var Path = _hrUnitOfWork.JobRepository.Find(models.ElementAtOrDefault(i).Id);

                        AutoMapper(new AutoMapperParm() { ObjectName = "CareerPaths", Destination = Path, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype=TransType.Update});
                        Path.CompanyId = models.ElementAtOrDefault(i).IsLocal ? CompanyId : (int?)null;
                        Path.ModifiedTime = DateTime.Now;
                        Path.ModifiedUser = UserName;
                        if (models.ElementAtOrDefault(i).StartDate > models.ElementAtOrDefault(i).EndDate)
                        {
                            ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndMustGrtThanStart"));
                            datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                            return Json(datasource);
                        }
                       
                        _hrUnitOfWork.JobRepository.Attach(Path);
                        _hrUnitOfWork.JobRepository.Entry(Path).State = EntityState.Modified;

                    }
                    datasource.Errors = SaveChanges(Language);
                }
                else
                {
                    datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                }
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }

        public ActionResult DeleteCareerPath(int Id)
        {
            var datasource = new DataSource<CareerPathViewModel>();

            var Obj = _hrUnitOfWork.Repository<CareerPath>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "CareerPaths",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });

            _hrUnitOfWork.JobRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
  
        public ActionResult DeleteCareerPathJobs(int Id)
        {
            var datasource = new DataSource<CareerPathJobsViewModel>();

            var Obj = _hrUnitOfWork.Repository<CareerPathJobs>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "CareerPathJobs",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });

            _hrUnitOfWork.JobRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");

        }
        public ActionResult CreateCareerPathJobs(IEnumerable<CareerPathJobsViewModel> models , OptionsViewModel moreInfo)
        {
            var result = new List<CareerPathJobs>();
            var datasource = new DataSource<CareerPathJobsViewModel>();

            if (ModelState.IsValid)
            {               
                foreach (CareerPathJobsViewModel c in models)
                {
                    var PathJobs = new CareerPathJobs();
  
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = PathJobs,
                        Source = c,
                        ObjectName = "CareerPathJobs",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    result.Add(PathJobs);
                    _hrUnitOfWork.JobRepository.Add(PathJobs);
                }
                datasource.Errors = SaveChanges(Language);
            }

            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from model in models
                               join r in result on model.CareerId equals r.CareerId
                               select new CareerPathJobsViewModel
                               {
                                   Id = r.Id,
                                   FormulaId = model.FormulaId,
                                   Performance = model.Performance,
                                   MinYears = model.MinYears,
                                   JobName = model.JobName,
                                    JobId=model.JobId,
                                    Sequence=model.Sequence
                               }).ToList();


            if (datasource.Errors.Count() > 0)
            {
                datasource.Data = models;
                return Json(datasource);
            }
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateCareerPathJobs(IEnumerable<CareerPathJobsViewModel> models, OptionsViewModel moreInfo)
        {
            var datasource = new DataSource<CareerPathJobsViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {           
                    var Ids = models.Select(m => m.Id);
                    var db = _hrUnitOfWork.Repository<CareerPathJobs>().Where(j => Ids.Contains(j.Id));

                    foreach (CareerPathJobsViewModel model in models)
                    {
                        var record = db.FirstOrDefault(a => a.Id == model.Id);
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = record,
                            Source = model,
                            ObjectName = "CareerPathJobs",
                            Version = Convert.ToByte(Request.Form["Version"]),
                            Options = moreInfo,
                            Transtype = TransType.Update
                        });
                        record.ModifiedTime = System.DateTime.Now;
                        record.ModifiedUser = UserName;

                        _hrUnitOfWork.JobRepository.Attach(record);
                        _hrUnitOfWork.JobRepository.Entry(record).State = EntityState.Modified;
                    }

                    datasource.Errors = SaveChanges(Language);
                }
                else
                {
                    datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                }
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }
    }
}
