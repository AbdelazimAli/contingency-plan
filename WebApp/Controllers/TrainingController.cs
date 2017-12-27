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
using WebApp.Extensions;
using Model.Domain.Payroll;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class TrainingController : BaseController
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
        public TrainingController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region TrainCourse
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        public ActionResult GetTrainingCourse(int MenuId)
        {
            string culture = Language;

            var query = _hrUnitOfWork.TrainingRepository.GetTrainCourse(Language, CompanyId);
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
            fillViewBag(id);

            if (id == 0)
                return View(new TrainCourseFormViewModel());

            var trainCourse = _hrUnitOfWork.TrainingRepository.ReadTrainCourse(id, Language);
            return trainCourse == null ? (ActionResult)HttpNotFound() : View(trainCourse);
        }
        public void fillViewBag(int Id)
        {
            ViewBag.PrevCourses = _hrUnitOfWork.TrainingRepository.GetTrainCourse(Language, CompanyId).Where(a=>a.Id !=Id).Select(p => new { id = p.Id, name = p.LocalName });
            ViewBag.Qualification = _hrUnitOfWork.Repository<Qualification>().Select(p => new { id = p.Id, name = p.Name }).ToList();
            ViewBag.Course = _hrUnitOfWork.TrainingRepository.GetTrainCourse(Language, CompanyId).Select(p => new { value = p.Id, text = p.LocalName });
            ViewBag.Locations = _hrUnitOfWork.LocationRepository.ReadLocations(Language, CompanyId).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.Jobs = _hrUnitOfWork.JobRepository.ReadJobs(CompanyId, Language,0).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.CompanyStuctures = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId, null, Language);
            ViewBag.Payrolls = _hrUnitOfWork.Repository<Payrolls>().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.Positions = _hrUnitOfWork.PositionRepository.GetPositions(Language, CompanyId).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.PeopleGroups = _hrUnitOfWork.PeopleRepository.GetPeoples().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.PayrollGrades = _hrUnitOfWork.JobRepository.GetPayrollGrade();

        }

        public ActionResult Details(TrainCourseFormViewModel model, OptionsViewModel moreInfo)
        {

            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LocationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "TrainCourse",
                        TableName = "TrainCourses",
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

                //insert
                if (model.Id == 0)
                {
                    var record = new TrainCourse();
                    MapTrainCourse(record, model, moreInfo);
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    _hrUnitOfWork.TrainingRepository.Add(record);
                }
                //update
                else
                {
                    var record = _hrUnitOfWork.Repository<TrainCourse>().FirstOrDefault(a => a.Id == model.Id);
                    MapTrainCourse(record, model, moreInfo);
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
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
        private void MapTrainCourse(TrainCourse trainCourse, TrainCourseFormViewModel model, OptionsViewModel moreInfo)
        {
            model.Locations = model.ILocations == null ? null : string.Join(",", model.ILocations.ToArray());
            model.Jobs = model.IJobs == null ? null : string.Join(",", model.IJobs.ToArray());
            model.Employments = model.IEmployments == null ? null : string.Join(",", model.IEmployments.ToArray());
            model.PeopleGroups = model.IPeopleGroups == null ? null : string.Join(",", model.IPeopleGroups.ToArray());
            model.Payrolls = model.IPayrolls == null ? null : string.Join(",", model.IPayrolls.ToArray());
            model.PayrollGrades = model.IPayrollGrades == null ? null : string.Join(",", model.IPayrollGrades.ToArray());
            model.CompanyStuctures = model.ICompanyStuctures == null ? null : string.Join(",", model.ICompanyStuctures.ToArray());
            model.Positions = model.IPositions == null ? null : string.Join(",", model.IPositions.ToArray());
            model.PrevCourses = model.IPrevCourses == null ? null : string.Join(",", model.IPrevCourses.ToArray());
            moreInfo.VisibleColumns.Add("Locations");
            moreInfo.VisibleColumns.Add("Jobs");
            moreInfo.VisibleColumns.Add("Employments");
            moreInfo.VisibleColumns.Add("PeopleGroups");
            moreInfo.VisibleColumns.Add("Payrolls");
            moreInfo.VisibleColumns.Add("PayrollGrades");
            moreInfo.VisibleColumns.Add("CompanyStuctures");
            moreInfo.VisibleColumns.Add("Positions");
            moreInfo.VisibleColumns.Add("PrevCourses");
            _hrUnitOfWork.LeaveRepository.AddLName(Language, trainCourse.Name, model.Name, model.LocalName);
            if (trainCourse.Id == 0)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = trainCourse,
                    Source = model,
                    ObjectName = "TrainCourse",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });
            }
            else
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = trainCourse,
                    Source = model,
                    ObjectName = "TrainCourse",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Update
                });

            }
        }

        public ActionResult DeleteTrainCourse(int id)
        {

            var message = "OK";
            var Source = new DataSource<TrainCourseViewModel>();
            Source.Errors = new List<Error>();

            TrainCourse trainCourse = _hrUnitOfWork.TrainingRepository.GetTrainCourse(id);
            bool Exist = false;
            var ExistData = _hrUnitOfWork.Repository<TrainCourse>().Select(a => new { a.Id, a.PrevCourses }).ToList();
            foreach (var item in ExistData)
            {

                if (item.PrevCourses != null)
                {
                    if (item.PrevCourses.Split(',').Contains(trainCourse.Id.ToString()))
                    {
                        Exist = true;
                        break;
                    }

                }
            }
            if (!Exist)
            {
                if (trainCourse != null)
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Source = trainCourse,
                        ObjectName = "TrainCourses",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Transtype = TransType.Delete
                    });

                    _hrUnitOfWork.TrainingRepository.Remove(trainCourse);
                    _hrUnitOfWork.TrainingRepository.RemoveLName(Language, trainCourse.Name);
                }
                Source.Errors = SaveChanges(Language);
            } else
                Source.Errors.Add(new Error() { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("connectwithData") } } });

           
            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);

        }


        #endregion

        #region TrainPath
        public ActionResult IndexTrainPath()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        public ActionResult GetTrainPath(int MenuId)
        {
            var query = _hrUnitOfWork.TrainingRepository.GetTrainPath(Language, CompanyId);
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

        public ActionResult DetailsTrainPath(int id = 0)
        {
            fillViewBag(id);

            if (id == 0)
                return View(new TrainPathFormViewModel());

            var trainPath = _hrUnitOfWork.TrainingRepository.ReadTrainPath(id, Language);
           // Id == 0 ? false : Job.IsLocal
            ViewBag.Course = _hrUnitOfWork.TrainingRepository.GetTrainCourseLST(Language, CompanyId, id == 0 ? false : trainPath.IsLocal).Select(p => new { value = p.Id, text = p.LocalName });
            return trainPath == null ? (ActionResult)HttpNotFound() : View(trainPath);
        }
        [HttpPost]
        public ActionResult DetailsTrainPath(TrainPathFormViewModel model, OptionsViewModel moreInfo)
        {

            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LocationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "TrainPath",
                        TableName = "TrainPaths",
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

                var record = _hrUnitOfWork.Repository<TrainPath>().FirstOrDefault(a => a.Id == model.Id);
                //insert
                if (record == null)
                {
                    record = new TrainPath();

                    MapTrainPath(record, model, moreInfo);
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    _hrUnitOfWork.TrainingRepository.Add(record);

                }

                //update
                else
                {

                    MapTrainPath(record, model, moreInfo);
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    _hrUnitOfWork.TrainingRepository.Attach(record);
                    _hrUnitOfWork.TrainingRepository.Entry(record).State = EntityState.Modified;

                }
                var Errors = SaveChanges(Language);
                TempData["pathId"] = record.Id;
                var message = "OK";
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);

            }

            return Json(Models.Utils.ParseFormErrors(ModelState));
        }
        private void MapTrainPath(TrainPath trainPath, TrainPathFormViewModel model, OptionsViewModel moreInfo)
        {
            model.Locations = model.ILocations == null ? null : string.Join(",", model.ILocations.ToArray());
            model.Jobs = model.IJobs == null ? null : string.Join(",", model.IJobs.ToArray());
            model.Employments = model.IEmployments == null ? null : string.Join(",", model.IEmployments.ToArray());
            model.PeopleGroups = model.IPeopleGroups == null ? null : string.Join(",", model.IPeopleGroups.ToArray());
            model.Payrolls = model.IPayrolls == null ? null : string.Join(",", model.IPayrolls.ToArray());
            model.PayrollGrades = model.IPayrollGrades == null ? null : string.Join(",", model.IPayrollGrades.ToArray());
            model.CompanyStuctures = model.ICompanyStuctures == null ? null : string.Join(",", model.ICompanyStuctures.ToArray());
            model.Positions = model.IPositions == null ? null : string.Join(",", model.IPositions.ToArray());
            moreInfo.VisibleColumns.Add("Locations");
            moreInfo.VisibleColumns.Add("Jobs");
            moreInfo.VisibleColumns.Add("Employments");
            moreInfo.VisibleColumns.Add("PeopleGroups");
            moreInfo.VisibleColumns.Add("Payrolls");
            moreInfo.VisibleColumns.Add("PayrollGrades");
            moreInfo.VisibleColumns.Add("CompanyStuctures");
            moreInfo.VisibleColumns.Add("Positions");
            _hrUnitOfWork.LeaveRepository.AddLName(Language, trainPath.Name, model.Name, model.LocalName);
            if (trainPath.Id == 0)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = trainPath,
                    Source = model,
                    ObjectName = "TrainPath",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });
            }
            else
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = trainPath,
                    Source = model,
                    ObjectName = "TrainPath",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Update
                });
            }

        }

        public ActionResult DeleteTrainPath(int id)
        {
            var message = "OK";
            DataSource<PositionViewModel> Source = new DataSource<PositionViewModel>();

            TrainPath trainPath = _hrUnitOfWork.TrainingRepository.GetTrainPath(id);
            if (trainPath != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = trainPath,
                    ObjectName = "TrainPaths",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });

                _hrUnitOfWork.TrainingRepository.Remove(trainPath);
            }
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);


        }
        public ActionResult ReadTrainPathCourse(int TrainPathId)
        {
            return Json(_hrUnitOfWork.TrainingRepository.GetCours(TrainPathId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateTrainPathCourse(IEnumerable<TrainPathCourseViewModel> models)
        {
            var datasource = new DataSource<TrainPathCourseViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            int pathid = (int)TempData["pathId"];
            TempData.Keep("pathId");
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "TrainPathCourses",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                //old Courses 
                var oldvalue = _hrUnitOfWork.TrainingRepository.Courses(pathid);
                foreach (TrainPathCourseViewModel model in models)
                {
                    var course = _hrUnitOfWork.Repository<TrainCourse>().Where(a => a.Id == model.TrainCourse_Id).FirstOrDefault();
                    oldvalue.Courses.Add(course);
                }
                _hrUnitOfWork.TrainingRepository.Attach(oldvalue);
                _hrUnitOfWork.TrainingRepository.Entry(oldvalue).State = EntityState.Modified;
                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }



            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        public ActionResult DeleteTrainPathCourse(IEnumerable<TrainPathCourseViewModel> models)
        {
            var datasource = new DataSource<TrainPathCourseViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            int pathid = (int)TempData["pathId"];
            TempData.Keep("pathId");
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "TrainPathCourses",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                //old Courses 
                var oldvalue = _hrUnitOfWork.TrainingRepository.Courses(pathid);
                foreach (TrainPathCourseViewModel model in models)
                {
                    var course = _hrUnitOfWork.Repository<TrainCourse>().Where(a => a.Id == model.TrainCourse_Id).FirstOrDefault();
                    oldvalue.Courses.Remove(course);
                }
                _hrUnitOfWork.TrainingRepository.Attach(oldvalue);
                _hrUnitOfWork.TrainingRepository.Entry(oldvalue).State = EntityState.Modified;
                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }



            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        public ActionResult UpdateTrainPathCourse(IEnumerable<TrainPathCourseViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<TrainPathCourseViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            int pathid = (int)TempData["pathId"];
            TempData.Keep("pathId");
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "TrainPathCourses",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var CourseList = _hrUnitOfWork.Repository<TrainCourse>().ToList();
                //old Courses 
                var oldvalue = _hrUnitOfWork.TrainingRepository.Courses(pathid);
                foreach (TrainPathCourseViewModel model in models)
                {

                    var courseAdd = CourseList.Where(a => a.Id == model.TrainCourse_Id).FirstOrDefault();
                    var courseRemove = CourseList.Where(a => a.Id == model.OldValue).FirstOrDefault();
                    oldvalue.Courses.Add(courseAdd);
                    oldvalue.Courses.Remove(courseRemove);
                }
                _hrUnitOfWork.TrainingRepository.Attach(oldvalue);
                _hrUnitOfWork.TrainingRepository.Entry(oldvalue).State = EntityState.Modified;
                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }



            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }





        #endregion


        #region TrainPathPeople
        public ActionResult TrainPathPeople(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult ReadEmployeeTrainpath(int id)
        {
            return Json(_hrUnitOfWork.TrainingRepository.TrainPathProgress(id, Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmployeeTrainPath()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetEmpsTrainpaths()
        {
            return Json(_hrUnitOfWork.TrainingRepository.GetEmpsTrainpaths(Language, CompanyId), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetMissCourses(int[] CourseIds, int Id)
        {
            var r = _hrUnitOfWork.TrainingRepository.GetMissCourses(Id, CourseIds, Language);
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}