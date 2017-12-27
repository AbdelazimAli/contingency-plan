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
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class TasksController : BaseController
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
        public TasksController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region Manager
        // GET: Tasks
        public ActionResult Index()
        {
            string message;
            ViewBag.Periods = _hrUnitOfWork.CheckListRepository.ReadTasksPeriods(User.Identity.GetEmpId(), CompanyId, Language, out message);
            if(!string.IsNullOrEmpty(message)) ViewBag.errorMsg = message;
            
            ViewBag.SubPeriods = _hrUnitOfWork.CheckListRepository.ReadTasksSubPeriods(User.Identity.GetEmpId());
            return View();
        }

        public ActionResult GetTasksPeriods(int MenuId, int? PeriodId, int? SubPeriodId)
        {
            var query = _hrUnitOfWork.CheckListRepository.ReadManagerTasks(User.Identity.GetEmpId(), PeriodId, SubPeriodId, CompanyId, Language);
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

        public ActionResult GetTasks(int MenuId, int EmpId, int? PeriodId, int? SubPeriodId)
        {
            var query = _hrUnitOfWork.CheckListRepository.ReadManagerEmpTasks(User.Identity.GetEmpId(), EmpId, PeriodId, SubPeriodId, Language);
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

        public ActionResult GetChart(int? periodId, int? subPeriodId)
        {
            return Json(_hrUnitOfWork.CheckListRepository.EmployeeProgress(User.Identity.GetEmpId(), periodId, subPeriodId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id = 0)
        {
            int? position = null;
            if(Session["PositionId"] != null)
                position = int.Parse(Session["PositionId"]?.ToString());
            ViewBag.Emps = _hrUnitOfWork.CheckListRepository.GetManagerEmpList(User.Identity.GetEmpId(), position, CompanyId, Language);

            EmpTask oldTask = _hrUnitOfWork.Repository<EmpTask>().Where(t => t.ManagerId == User.Identity.GetEmpId()).OrderByDescending(t => t.TaskNo).FirstOrDefault();
            if (id == 0) return View(new EmpTasksViewModel() { TaskNo = (short)(oldTask != null ? (oldTask.TaskNo + 1) : 1),
                AssignedTime = DateTime.Now, Status = 1});

            EmpTasksViewModel task = _hrUnitOfWork.CheckListRepository.GetManagerEmpTask(id);
            return task == null ? (ActionResult)HttpNotFound() : View(task);
        }

        [HttpPost]
        public ActionResult Details(EmpTasksViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpTasksForm",
                        TableName = "EmpTasks",
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

            EmpTask task = _hrUnitOfWork.CheckListRepository.GetEmpTask(model.Id);

            byte version;
            byte.TryParse(Request.Form["version"], out version);
            string message = "OK,";

            //Check 1.Subperiod is defiend, 2.Done tasks is started
            if (model.Status == 1) //1-AssignTo
            {
                if (model.AssignedTime == null) model.AssignedTime = DateTime.Now;

                int? subPeriod;
                string error = _hrUnitOfWork.CheckListRepository.GetTaskSubPeriod(CompanyId, model.AssignedTime, Language, out subPeriod);
                if (error.Length > 0)
                {
                    ModelState.AddModelError("AssignedTime", error);
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                model.SubPeriodId = subPeriod;
            }
            else if (model.Status == 2) //2-Done
            {
                if (model.StartTime == null || model.StartTime > DateTime.Now)
                {
                    ModelState.AddModelError("Status", MsgUtils.Instance.Trls("TaskNotStarted"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                model.EndTime = DateTime.Now;
            }


            ///Save
            if (model.Id == 0)
            { /// New
                task = new EmpTask();
                
                AutoMapper(new AutoMapperParm() { Source = model, Destination = task, Version = version, ObjectName = "EmpTasksForm", Options = moreInfo });
                task.SubPeriodId = model.SubPeriodId;
                task.CreatedUser = UserName;
                task.CreatedTime = DateTime.Now;
                _hrUnitOfWork.CheckListRepository.Add(task);
            }
            else if(model.ChangeEmployee)
            {
                EmpTask oldTask = _hrUnitOfWork.CheckListRepository.GetEmpTask(model.Id);
                var oldEmpId = oldTask.EmpId;
                //check emp changed
                if (oldEmpId == model.EmpId)
                    return Json(MsgUtils.Instance.Trls("ChangeToSameEmp"));

                //assign to new Emp
                task = new EmpTask();
                AutoMapper(new AutoMapperParm() { Source = model, Destination = task, Version = version, ObjectName = "EmpTasksForm", Options = moreInfo });
                task.Status = 1; //1-AssignTo
                task.AssignedTime = DateTime.Now;

                int? subPeriod;
                string error = _hrUnitOfWork.CheckListRepository.GetTaskSubPeriod(CompanyId, model.AssignedTime, Language, out subPeriod);
                if (error.Length > 0)
                {
                    ModelState.AddModelError("AssignedTime", error);
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                task.SubPeriodId = subPeriod;

                task.CreatedUser = UserName;
                task.CreatedTime = DateTime.Now;

                _hrUnitOfWork.CheckListRepository.Add(task);

                //cancel from current Emp
                AutoMapper(new AutoMapperParm() { Source = model, Destination = oldTask, Version = version, ObjectName = "EmpTasksForm", Options = moreInfo });
                oldTask.EmpId = oldEmpId;
                oldTask.Status = 3; //3-cancel
                oldTask.ModifiedUser = UserName;
                oldTask.ModifiedTime = DateTime.Now;

                _hrUnitOfWork.CheckListRepository.Attach(oldTask);
                _hrUnitOfWork.CheckListRepository.Entry(oldTask).State = EntityState.Modified;
            }
            else
            { /// Edit
                if (task.EmpId != model.EmpId)
                    return Json(MsgUtils.Instance.Trls("ClickChangeEmp"));

                AutoMapper(new AutoMapperParm() { Source = model, Destination = task, Version = version, ObjectName = "EmpTasksForm", Options = moreInfo });
                task.ModifiedUser = UserName;
                task.ModifiedTime = DateTime.Now;

                _hrUnitOfWork.CheckListRepository.Attach(task);
                _hrUnitOfWork.CheckListRepository.Entry(task).State = EntityState.Modified;

                if(task.Status == 2 || task.Status == 4) //2-Done, 4-Not Done
                    _hrUnitOfWork.CheckListRepository.AssignNextTask(task);
            }

            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;
            else
                message = message + ((new JavaScriptSerializer()).Serialize(model));
            return Json(message);
        }

        public ActionResult DeleteTask(int id)
        {
            var message = "OK";
            DataSource<EmpTask> Source = new DataSource<EmpTask>();

            EmpTask empTask = _hrUnitOfWork.CheckListRepository.GetEmpTask(id);
            _hrUnitOfWork.CheckListRepository.Remove(empTask);

            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }
        #endregion

        #region Employee
        public ActionResult EmployeeTasksIndex()
        {
            return View();
        }

        public ActionResult GetEmployeeTasks(int MenuId)
        {
            var query = _hrUnitOfWork.CheckListRepository.ReadEmployeeTasksGrid(User.Identity.GetEmpId(), Language);
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

        public ActionResult EmployeeTasksDetails(int id)
        {
            EmpTasksViewModel task = _hrUnitOfWork.CheckListRepository.GetEmployeeTask(id, Language);
            return task == null ? (ActionResult)HttpNotFound() : View(task);
        }

        public ActionResult SaveEmployeeTasks (int Id, bool btn)
        {
            //var AllEmpTasks = _hrUnitOfWork.CheckListRepository.ReadEmployeeTasksGrid(User.Identity.GetEmpId(), Language).ToList();
            EmpTask oldTask = _hrUnitOfWork.CheckListRepository.GetEmpTask(Id);

            byte version;
            byte.TryParse(Request.Form["version"], out version);

            if (btn) //Start
            {
                //var lessPriorty = AllEmpTasks.FirstOrDefault(t => t.Id != task.Id && t.Priority < task.Priority && task.Status == 1);
                //if (lessPriorty != null)
                //    return Json(MsgUtils.Instance.Trls("LessPrioryNotFinishYet"));
                _hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "StartTime", ValueBefore = oldTask.StartTime.ToString(), ValueAfter = DateTime.Now.ToString(), ObjectName = "EmpTasksForm", Version = version, CompanyId = CompanyId, SourceId = oldTask.Id.ToString(), UserName = UserName });
                if (oldTask.StartTime != null)
                    return Json(MsgUtils.Instance.Trls("TaskAlreadyStarted"));

                oldTask.StartTime = DateTime.Now;
            }
            else
            {
                _hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "EndTime", ValueBefore = oldTask.EndTime.ToString(), ValueAfter = DateTime.Now.ToString(), ObjectName = "EmpTasksForm", Version = version, CompanyId = CompanyId, SourceId = oldTask.Id.ToString(), UserName = UserName });
               _hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "Status", ValueBefore = oldTask.Status.ToString(), ValueAfter = "2", ObjectName = "EmpTasksForm", Version = version, CompanyId = CompanyId, SourceId = oldTask.Id.ToString(), UserName = UserName });

                oldTask.EndTime = DateTime.Now;
                oldTask.Status = 2;
                TimeSpan duration = oldTask.EndTime.Value - oldTask.StartTime.Value;

                switch (oldTask.Unit)
                {
                    case 0://Minutes
                        oldTask.Duration = Convert.ToSByte(duration.Minutes);
                        break;
                    case 1: //Hours
                        oldTask.Duration = Convert.ToSByte(duration.Hours);
                        break;
                    case 2: //Days
                        oldTask.Duration = Convert.ToSByte(duration.Days);
                        break;
                    case 3: //Weeks
                        oldTask.Duration = Convert.ToSByte(duration.Days / 7);
                        break;
                    case 4: //Months
                        oldTask.Duration = Convert.ToSByte(duration.Days / 365.25 * 12);
                        break;
                }
                _hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "Status", ValueBefore = "", ValueAfter = oldTask.Duration.ToString(), ObjectName = "EmpTasksForm", Version = version, CompanyId = CompanyId, SourceId = oldTask.Id.ToString(), UserName = UserName });

                _hrUnitOfWork.CheckListRepository.AssignNextTask(oldTask);
            }
            oldTask.ModifiedTime = DateTime.Now;
            oldTask.ModifiedUser = UserName;

            EmpTask task = new EmpTask();
            AutoMapper(new AutoMapperParm() { Source = oldTask, Destination = task, ObjectName = "EmpTasksForm", Version = version });

            _hrUnitOfWork.CheckListRepository.Attach(oldTask);
            _hrUnitOfWork.CheckListRepository.Entry(oldTask).State = EntityState.Modified;

            string message = "OK";
            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;

            return Json(message);
        }
        #endregion
    }
}