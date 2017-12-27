using Interface.Core;
using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Extensions;

namespace WebApp.Controllers.Api
{
    public class TasksController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;
        public TasksController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region  Tasks
        [ResponseType(typeof(EmpTasksViewModel)), HttpGet]
        [Route("api/Tasks/GetEmployeeTasks")]
        public IHttpActionResult GetEmployeeTasks(int MenuId)
        {
            var query = _hrUnitOfWork.CheckListRepository.ReadEmployeeTasksGrid(1027, User.Identity.GetLanguage());
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {
                    //TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Ok("");
                }
            }
            return Ok(query);
        }

        [ResponseType(typeof(EmpTasksViewModel)), HttpGet]
        [Route("api/Tasks/EmployeeTasksDetails")]
        public IHttpActionResult EmployeeTasksDetails(int id = 0)
        {
            EmpTasksViewModel task = _hrUnitOfWork.CheckListRepository.GetEmployeeTask(id, User.Identity.GetLanguage());

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [ResponseType(typeof(EmpTasksViewModel)), HttpPost]
        [Route("api/Tasks/SaveTask")]
        public IHttpActionResult SaveTask(EmpTasksViewModel model)
        {
            EmpTask task = _hrUnitOfWork.CheckListRepository.GetEmpTask(model.Id);
            if (model.isStart) //Start
            {
                _hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "StartTime", ValueBefore = task.StartTime.ToString(), ValueAfter = DateTime.Now.ToString(), ObjectName = "EmpTasksForm", CompanyId = User.Identity.GetDefaultCompany(), SourceId = task.Id.ToString(), UserName = User.Identity.Name });
                if (task.StartTime != null)
                    return Ok(MsgUtils.Instance.Trls("TaskAlreadyStarted"));
                task.StartTime = DateTime.Now;
            }
            else
            {
                _hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "EndTime", ValueBefore = task.EndTime.ToString(), ValueAfter = DateTime.Now.ToString(), ObjectName = "EmpTasksForm", CompanyId = User.Identity.GetDefaultCompany(), SourceId = task.Id.ToString(), UserName = User.Identity.Name });
                _hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "Status", ValueBefore = task.Status.ToString(), ValueAfter = "2", ObjectName = "EmpTasksForm", CompanyId = User.Identity.GetDefaultCompany(), SourceId = task.Id.ToString(), UserName = User.Identity.Name });

                task.EndTime = DateTime.Now;
                task.Status = 2;
                TimeSpan duration = task.EndTime.Value - task.StartTime.Value;

                switch (task.Unit)
                {
                    case 0://Minutes
                        task.Duration = Convert.ToSByte(duration.Minutes);
                        break;
                    case 1: //Hours
                        task.Duration = Convert.ToSByte(duration.Hours);
                        break;
                    case 2: //Days
                        task.Duration = Convert.ToSByte(duration.Days);
                        break;
                    case 3: //Weeks
                        task.Duration = Convert.ToSByte(duration.Days / 7);
                        break;
                    case 4: //Months
                        task.Duration = Convert.ToSByte(duration.Days / 365.25 * 12);
                        break;
                }
                _hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "Status", ValueBefore = "", ValueAfter = task.Duration.ToString(), ObjectName = "EmpTasksForm", CompanyId = User.Identity.GetDefaultCompany(), SourceId = task.Id.ToString(), UserName = User.Identity.Name });
                _hrUnitOfWork.CheckListRepository.AssignNextTask(task);
            }
            task.ModifiedTime = DateTime.Now;
            task.ModifiedUser = User.Identity.Name;

            _hrUnitOfWork.CheckListRepository.Attach(task);
            _hrUnitOfWork.CheckListRepository.Entry(task).State = EntityState.Modified;

            string message = "Ok";
            var Errors = SaveChanges(User.Identity.GetLanguage());
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;

            return Json(message);
        }
        #endregion
        #region message


        [ResponseType(typeof(EmployeeMessagesViewModel)), HttpGet]
        [Route("api/Tasks/GetEmployeeMessage")]
        public IHttpActionResult GetEmployeeMessage(int MenuId)
        {
            var query = _hrUnitOfWork.MessageRepository.GetEmployeeMessages(User.Identity.GetLanguage());
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {
                    //TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Ok("");
                }
            }
            return Ok(query);
        }

        [HttpDelete]
        [Route("api/Tasks/deleteMessage")]
        public IHttpActionResult deleteMessage(int Id)
        {
            var Message = "Ok";
            var EmpMess = _hrUnitOfWork.MessageRepository.GetEmpMessage(Id);
            if (EmpMess != null)
                _hrUnitOfWork.MessageRepository.Remove(EmpMess);

            try
            {
                var err = SaveChanges(User.Identity.GetLanguage());
                if (err.Count() > 0)
                {
                    foreach (var item in err)
                    {
                        Message = item.errors.Select(a => a.message).FirstOrDefault();
                    }
                    return Ok(Message);
                }
            }
            catch (Exception ex)
            {
                var msg = _hrUnitOfWork.HandleDbExceptions(ex, User.Identity.GetLanguage());
                if (msg.Length > 0)
                    return Ok(msg);
            }
            return Ok(Message);
        }

        [ResponseType(typeof(EmployeeMessagesViewModel)), HttpGet]
        [Route("api/Tasks/GetEmpMessagesDetails")]
        public IHttpActionResult GetEmpMessagesDetails(int Id)
        {
            var query = _hrUnitOfWork.MessageRepository.GetEmployeeMessages(User.Identity.GetLanguage()).Where(a => a.Id == Id).FirstOrDefault();
            return Ok(query);
        }

        #endregion
    }
}