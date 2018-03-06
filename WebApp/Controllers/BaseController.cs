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
using System.Web.Routing;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string UserName { get; set; }
        protected string Language { get; set; }
        protected int CompanyId { get; set; }
        protected string Culture { set; get; }
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

        protected JsonResult ApplyFilter<T>(IQueryable<T> query, Expression<Func<T, string>> orderby,  int MenuId, int pageSize, int skip)
        {
            string filter = "";
            string Sorting = "";
            string whecls = GetWhereClause(MenuId);
            query = (IQueryable<T>)WebApp.Models.Utils.GetFilter(query, ref filter, ref Sorting);
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
                    WebApp.Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }

            var total = query.Count();

            if (Sorting.Length > 0)
                query = query.OrderBy(Sorting);
            else if (orderby != null)
                query = query.OrderBy(orderby);

            if (skip > 0)
                query = query.Skip(skip).Take(pageSize);
            else
                query = query.Take(pageSize);

            return Json(new { total = total, data = query.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public BaseController()
        {

        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
                UserName = requestContext.HttpContext.User.Identity.Name;
                Culture= requestContext.HttpContext.User.Identity.GetCulture();
            }
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

        protected override void HandleUnknownAction(string actionName)
        {
            RedirectToAction("Index").ExecuteResult(this.ControllerContext);
        }

        /// <summary>
        /// Fill : Admin , compantId , culture , rtl
        /// </summary>
        protected void FillBasicData(bool FillAdmin, bool FillCompantId, bool FillCulture, bool FillRTL)
        {
            if (FillAdmin)
                ViewBag.admin = User.Identity.CanCustomize();

            if (FillCompantId)
                ViewBag.compantId = CompanyId;

            if (FillCulture)
                ViewBag.culture = User.Identity.GetCulture();

            if (FillRTL)
                ViewBag.rtl = User.Identity.RTL();
        }
        protected List<FormErrorViewModel> ValidateForm(string ObjectName, string TableName, ModelStateDictionary ModelState)
        {
            try
            {
                var errors = new List<Error>();
                if (ServerValidationEnabled)
                {
                    errors = HrUnitOfWork.SiteRepository.CheckForm(new CheckParm
                    {
                        CompanyId = User.Identity.GetDefaultCompany(),
                        ObjectName = ObjectName,
                        TableName = TableName,
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        Culture = User.Identity.GetCulture()
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

                        return Models.Utils.ParseFormErrors(ModelState);
                    }
                }
            }
            catch
            {

            }

            return null;
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (Request.QueryString["MenuId"] == null) return;
            Session["MenuId"] = Request.QueryString["MenuId"];


            if (Request.QueryString["RoleId"] != null)
                Session["RoleId"] = Request.QueryString["RoleId"];
            else if (Session["RoleId"] == null)
                Session["RoleId"] = HrUnitOfWork.MenuRepository.GetMenuRoleId(int.Parse(Session["MenuId"].ToString()), User.Identity.Name);


            if (Session["DepartmentId"] == null)
            {
                var empid = User.Identity.GetEmpId();
                var employee = HrUnitOfWork.Repository<Model.Domain.Assignment>().Where(a => a.EmpId == empid && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today)).Select(b => new { b.DepartmentId, b.BranchId, b.IsDepManager, b.JobId, b.PositionId }).FirstOrDefault();
                if (employee != null)
                {
                    Session["DepartmentId"] = employee.DepartmentId;
                    Session["PositionId"] = employee.PositionId;
                    Session["IsDepManager"] = employee.IsDepManager.ToString();
                    Session["BranchId"] = employee.BranchId;
                    Session["JobId"] = employee.JobId;
                }
            }
        }

        protected string GetWhereClause(int MenuId)
        {
            int menuId = 0;
            string RoleId = "";

            if (Session["MenuId"] != null) menuId = int.Parse(Session["MenuId"].ToString());
            if (Session["RoleId"] != null) RoleId = Session["RoleId"].ToString();

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
                        if (whereclause.IndexOf("@PositionId") != -1) whereclause = whereclause.Replace("@PositionId", Session["PositionId"] == null ? "0" : Session["PositionId"].ToString());
                        if (whereclause.IndexOf("@DepartmentId") != -1) whereclause = whereclause.Replace("@DepartmentId", Session["DepartmentId"] == null ? "0" : Session["DepartmentId"].ToString());
                        if (whereclause.IndexOf("@BranchId") != -1) whereclause = whereclause.Replace("@BranchId", Session["BranchId"] == null ? "0" : Session["BranchId"].ToString());
                    }

                    return whereclause;
                }
            }

            return "";
        }

        protected void AutoMapper(Models.AutoMapperParm parm)
        {
            parm.Version = Convert.ToByte(Request.Form["Version"]);
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
            // use this function only to save main record
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

        public List<Model.ViewModel.Error> Save(string Language)
        {
            // use this function within transaction for any change
            var errors = HrUnitOfWork.SaveChanges(Language);
            return errors;
        }

        public void SaveFlexData(IEnumerable<SaveFlexDataVM> models, int recordId)
        {
            if (models != null && models.Count() > 0)
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
                            flexData = new FlexData()
                            {
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
