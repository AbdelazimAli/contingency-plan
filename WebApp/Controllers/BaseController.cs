using Interface.Core;
using System;
using System.Linq;
using System.Web.Mvc;
using WebApp.Extensions;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Model.ViewModel;
using Model.Domain;
using System.Data.Entity;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Gets my project unit of work.
        /// </summary>
        /// <value>
        /// My project unit of work.
        /// </value>
        
        protected bool ServerValidationEnabled = false;
        protected IHrUnitOfWork HrUnitOfWork { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        protected BaseController(IHrUnitOfWork unitOfWork)
        { 
            HrUnitOfWork = unitOfWork;
            ServerValidationEnabled = System.Configuration.ConfigurationManager.AppSettings["ServerValidationEnabled"] == "true";
        }
       
        public BaseController()
        {

        }
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

            filterContext.Result = PartialView("Error", model);
            //    new PartialViewResult()
            //{
            //    ViewName = "Error",
            //    ViewData = new ViewDataDictionary(model)
            //};


            // To display the above error in view we can use the below code
            // @Model.Exception;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (Request.QueryString["MenuId"] == null) return;
            Session["MenuId"] = Request.QueryString["MenuId"];

            if (Session["RoleId"] == null)
                Session["RoleId"] = HrUnitOfWork.MenuRepository.GetMenuRoleId(int.Parse(Session["MenuId"].ToString()), User.Identity.Name);
            else if (Request.QueryString["RoleId"] != null)
                Session["RoleId"] = Request.QueryString["RoleId"];

            if (Session["DepartmentId"] == null)
            {
                var employee = HrUnitOfWork.Repository<Model.Domain.Assignment>().Where(a => a.EmpId == User.Identity.GetEmpId() && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today)).Select(b => new { b.DepartmentId, b.LocationId, b.IsDepManager, b.JobId, b.PositionId, b.BranchId, b.SectorId }).FirstOrDefault();
                if (employee != null)
                {
                    Session["DepartmentId"] = employee.DepartmentId;
                    Session["LocationId"] = employee.LocationId;
                    Session["PositionId"] = employee.PositionId;
                    Session["IsDepManager"] = employee.IsDepManager.ToString();
                    Session["BranchId"] = employee.BranchId;
                    Session["SectorId"] = employee.SectorId;
                    Session["JobId"] = employee.JobId;
                }
            }
        }

        protected string GetWhereClause(int MenuId)
        {
            int menuId = 0;
            string RoleId = "";

            if(Session["MenuId"] != null ) menuId = int.Parse(Session["MenuId"].ToString());
            if(Session["RoleId"] != null) RoleId = Session["RoleId"].ToString();

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
                        if (whereclause.IndexOf("@IsDepManager") != -1) whereclause = whereclause.Replace("@IsDepManager", Session["IsDepManager"] == null ? "false" : Session["IsDepManager"].ToString());
                        if (whereclause.IndexOf("@JobId") != -1) whereclause = whereclause.Replace("@JobId", Session["JobId"] == null ? "0" : Session["JobId"].ToString());
                        if (whereclause.IndexOf("@LocationId") != -1) whereclause = whereclause.Replace("@LocationId", Session["LocationId"] == null ? "0" : Session["LocationId"].ToString());
                        if (whereclause.IndexOf("@PositionId") != -1) whereclause = whereclause.Replace("@PositionId", Session["PositionId"] == null ? "0" : Session["PositionId"].ToString());
                        if (whereclause.IndexOf("@DepartmentId") != -1) whereclause = whereclause.Replace("@DepartmentId", Session["DepartmentId"] == null ? "0" : Session["DepartmentId"].ToString());
                        if (whereclause.IndexOf("@BranchId") != -1) whereclause = whereclause.Replace("@BranchId", Session["BranchId"] == null ? "0" : Session["BranchId"].ToString());
                        if (whereclause.IndexOf("@SectorId") != -1) whereclause = whereclause.Replace("@SectorId", Session["SectorId"] == null ? "0" : Session["SectorId"].ToString());
                    }

                    return whereclause;
                }
            }

            return "";
        }

        protected void AutoMapper(Models.AutoMapperParm parm)
        {
            Models.AutoMapper mapper = new Models.AutoMapper(parm, HrUnitOfWork, User.Identity);
            mapper.Map();
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

        public List<Model.ViewModel.Error> SaveChanges(string Language)
        {
            return FirstSaveChanges(Language);
        }

        public List<Model.ViewModel.Error> SavePoint1(string Language)
        {
            return FirstSaveChanges(Language);
        }

        public List<Model.ViewModel.Error> SavePointn(string Language)
        {
            var errors = HrUnitOfWork.SaveChanges(Language);
            return errors;
        }

        private  List<Model.ViewModel.Error> FirstSaveChanges(string Language)
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

        public void SaveFlexData(IEnumerable<SaveFlexDataVM> models, int recordId)
        {
            if(models != null && models.Count() > 0)
            {
                SaveFlexDataVM firstModel = models.FirstOrDefault();
                List<FlexData> oldFlexData = HrUnitOfWork.PageEditorRepository.GetSourceFlexData(firstModel.PageId, firstModel.SourceId).ToList();

                foreach (var model in models)
                {
                    //--flexData
                    FlexData flexData = oldFlexData.FirstOrDefault(fd => fd.Id == model.flexId);
                    //Delete from flexData if not exists and value == null 

                    if (model.Value != null)
                    {
                        if (flexData == null) //Add  
                        {
                            flexData = new FlexData() {
                                PageId = model.PageId,
                                SourceId = (model.SourceId == 0 ? recordId : model.SourceId),
                                TableName = model.TableName,
                                ColumnName = model.ColumnName,
                                Value = model.Value,
                                ValueId = model.ValueId
                            };

                            HrUnitOfWork.PageEditorRepository.Add(flexData);
                        }
                        else //Update
                        {
                            flexData.Value = model.Value;
                            flexData.ValueId = model.ValueId;

                            HrUnitOfWork.PageEditorRepository.Attach(flexData);
                            HrUnitOfWork.PageEditorRepository.Entry(flexData).State = EntityState.Modified;
                        }
                    }
                    else if (flexData != null) //Delete
                    {
                        HrUnitOfWork.PageEditorRepository.Delete(flexData);
                    }
                }
            }
        }
    }
}
