using Interface.Core;
using System.Web.Mvc;
using System.Web.Http;
using WebApp.Models;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Linq;
using WebApp.Extensions;
using System;
using System.Threading;
using System.Web.Http.Filters;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace WebApp.Controllers.Api
{
    public abstract class BaseApiController : ApiController
    {
        protected IHrUnitOfWork HrUnitOfWork { get; private set; }
        public BaseApiController()
        {

        }

        protected bool ServerValidationEnabled = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>

        protected BaseApiController(IHrUnitOfWork unitOfWork)
        {
            HrUnitOfWork = unitOfWork;
            ServerValidationEnabled = System.Configuration.ConfigurationManager.AppSettings["ServerValidationEnabled"] == "true";

        }

        protected Model.Domain.PersonSetup _PersonSetup
        {
            get
            {
                return HrUnitOfWork.CompanyRepository.GetPersonSetup(User.Identity.GetDefaultCompany());
            }
            set
            {
                HrUnitOfWork.CompanyRepository.SetPersonSetup(value);
            }
        }

        protected void AutoMapper(Models.AutoMapperParm parm)
        {
            Models.AutoMapper mapper = new Models.AutoMapper(parm, HrUnitOfWork, User.Identity);
            mapper.Map();
        }

        public List<Model.ViewModel.Error> SaveChanges(string Language)
        {
            List<DbEntityEntry> entries = null;
            var errors = HrUnitOfWork.SaveChanges(Language, out entries);
            if (errors.Count > 0) return errors;

            // now send notifications
            var notifications = entries.Where(e => e.Entity.ToString() == "Model.Domain.Notifications.Notification").Select(e => e.Entity);
            if (notifications.Count() > 0) // found notifications
            {
                foreach (var entity in notifications)
                {
                    var notification = (Model.Domain.Notifications.Notification)entity;
                    HangFireJobs.SendNotication(notification, notification.Condition, HrUnitOfWork.NotificationRepository);
                }
            }

            return errors;
        }

        protected string GetWhereClause(int MenuId)
        {
            //int menuId = 0;
            string RoleId = "";
            RoleId = HrUnitOfWork.MenuRepository.GetMenuRoleId(MenuId, User.Identity.Name);
            //if (Session["MenuId"] != null) menuId = int.Parse(Session["MenuId"].ToString());
            //if (Session["RoleId"] != null) RoleId = Session["RoleId"].ToString();

            if (MenuId > 0)
            {
                var menu = HrUnitOfWork.MenuRepository.Get(MenuId);
                if (menu != null && menu.WhereClause != null)
                {
                    string whereclause = menu.WhereClause.Trim();
                    if (whereclause.Length > 0)
                    {
                        if (whereclause.IndexOf("@Company") != -1) whereclause = whereclause.Replace("@Company", User.Identity.GetDefaultCompany().ToString());
                        if (whereclause.IndexOf("@User") != -1) whereclause = whereclause.Replace("@User", User.Identity.Name);
                        if (whereclause.IndexOf("@RoleId") != -1) whereclause = whereclause.Replace("@RoleId", "\"" + RoleId + "\"");
                        if (whereclause.IndexOf("@EmpId") != -1) whereclause = whereclause.Replace("@EmpId", User.Identity.GetEmpId().ToString());
                        if (whereclause.IndexOf("@IsDepManager") != -1) whereclause = whereclause.Replace("@IsDepManager", HttpContext.Current.Session["IsDepManager"] == null ? "false" : HttpContext.Current.Session["IsDepManager"].ToString());
                        if (whereclause.IndexOf("@JobId") != -1) whereclause = whereclause.Replace("@JobId", HttpContext.Current.Session["JobId"] == null ? "0" : HttpContext.Current.Session["JobId"].ToString());
                        if (whereclause.IndexOf("@LocationId") != -1) whereclause = whereclause.Replace("@LocationId", HttpContext.Current.Session["LocationId"] == null ? "0" : HttpContext.Current.Session["LocationId"].ToString());
                        if (whereclause.IndexOf("@PositionId") != -1) whereclause = whereclause.Replace("@PositionId", HttpContext.Current.Session["PositionId"] == null ? "0" : HttpContext.Current.Session["PositionId"].ToString());
                        if (whereclause.IndexOf("@DepartmentId") != -1) whereclause = whereclause.Replace("@DepartmentId", HttpContext.Current.Session["DepartmentId"] == null ? "0" : HttpContext.Current.Session["DepartmentId"].ToString());
                        if (whereclause.IndexOf("@BranchId") != -1) whereclause = whereclause.Replace("@BranchId", HttpContext.Current.Session["BranchId"] == null ? "0" : HttpContext.Current.Session["BranchId"].ToString());
                        if (whereclause.IndexOf("@SectorId") != -1) whereclause = whereclause.Replace("@SectorId", HttpContext.Current.Session["SectorId"] == null ? "0" : HttpContext.Current.Session["SectorId"].ToString());
                    }

                    return whereclause;
                }
            }

            return "";
        }

    }
}
