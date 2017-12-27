using Interface.Core;
//using Microsoft.Office.Interop.Excel;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;
using System.Web.Http.ModelBinding;
using System.Web;

namespace WebApp.Controllers.Api
{
    public class DashbordController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;

        public DashbordController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        #region Employee 
        [ResponseType(typeof(ChartViewModel)), HttpGet]
        [Route("api/Dashbord/EmpMonthTask")]
        public IHttpActionResult EmpMonthTask()
        {
          
            var YearTasks = _hrUnitOfWork.CheckListRepository.EmpTasksByPerid(1027, User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
            var MonthesTasks = _hrUnitOfWork.CheckListRepository.EmpTasksBySubPeriod(1027, User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());

            var AnnualLeave = _hrUnitOfWork.LeaveRepository.LeavesCounts(1027, User.Identity.GetDefaultCompany(), User.Identity.GetCulture());

            return Ok(new { YearTasks = YearTasks, MonthesTasks = MonthesTasks, AnnualLeave = AnnualLeave });
        }

        

        [ResponseType(typeof(LeaveReqViewModel)), HttpGet]
        [Route("api/Dashbord/EmpPeriodLeavesGrid")]
        public IHttpActionResult EmpPeriodLeavesGrid(int LeaveType, int Period)
        {
            return Json(_hrUnitOfWork.LeaveRepository.PeriodLeaveGrid(1027, LeaveType, Period, User.Identity.GetDefaultCompany(), User.Identity.GetLanguage()));
        }
        #endregion
        #region Manager

        [ResponseType(typeof(ChartViewModel)), HttpGet]
        [Route("api/Dashbord/ManagerEmployeeTask")]
        public IHttpActionResult ManagerEmployeeTask()
        {
            var EmpId = User.Identity.GetEmpId();
            EmpId = 1042;
            var query = _hrUnitOfWork.CheckListRepository.ManagerEmployeeTask(EmpId, User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
            return Ok(query);
        }
        #endregion
    }
}