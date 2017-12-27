using Interface.Core;
using Model.ViewModel.Personnel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using WebApp.Extensions;
using Model.ViewModel;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Model.Domain;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class MessageController : BaseController
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
        public MessageController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: Message
        public ActionResult Index()
        {
            var empId = User.Identity.GetEmpId();
            ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { value = a.Id, text = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }).Distinct().ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult EmployeeMessagesIndex()
        {
            return View();
        }
        public ActionResult GetEmployeeMessages(int MenuId)
        {
            var query = _hrUnitOfWork.MessageRepository.GetEmployeeMessages(Language);

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
        public ActionResult EmployeeMessagesDetails(int Id)
        {

            var EmpMess = _hrUnitOfWork.MessageRepository.GetEmpMessage(Id);
            if (!EmpMess.Read)
            {
                EmpMess.Read = true;
                _hrUnitOfWork.MessageRepository.Attach(EmpMess);
                _hrUnitOfWork.MessageRepository.Entry(EmpMess).State = EntityState.Modified;
                SaveChanges(Language);
            }
            return View(_hrUnitOfWork.MessageRepository.GetEmployeeMessages(Language).Where(a => a.Id == Id).FirstOrDefault());
        }
        public ActionResult EmployeeMessagesDelete(int Id)
        {
            var Message = "OK";

            var EmpMess = _hrUnitOfWork.MessageRepository.GetEmpMessage(Id);
            if (EmpMess != null)
                _hrUnitOfWork.MessageRepository.Remove(EmpMess);

            try
            {
                var err = SaveChanges(Language);
                if (err.Count() > 0)
                {
                    foreach (var item in err)
                    {
                        Message = item.errors.Select(a => a.message).FirstOrDefault();
                    }
                    return Json(Message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMessage(int MenuId)
        {
            var query = _hrUnitOfWork.MessageRepository.ReadMessage(User.Identity.GetEmpId(), Language,CompanyId);
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
        public ActionResult Details(int id = 0)
        {
            var empId = User.Identity.GetEmpId();
            ViewBag.Jobs = _hrUnitOfWork.JobRepository.ReadJobs(CompanyId, Language,0).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.PeopleGroups = _hrUnitOfWork.PeopleRepository.GetPeoples().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }).Distinct().ToList();
            ViewBag.Depts = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId, null, Language);
            if (id == 0)
                return View(new MessageViewModel());
            var message = _hrUnitOfWork.MessageRepository.ReadFormMessage(id, Language);
            return message == null ? (ActionResult)HttpNotFound() : View(message);
        }
        public ActionResult SaveMessages(MessageViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.MessageRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpMessageForm",
                        TableName = "Messages",
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
                if (User.Identity.GetEmpId() == 0)
                {
                    ModelState.AddModelError("", MsgUtils.Instance.Trls("connectedEmp"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                if(model.All == false && model.IJobs == null && model.IPeopleGroups ==null && model.IEmployees ==null && model.IDepts ==null)
                {
                    ModelState.AddModelError("", MsgUtils.Instance.Trls("chooseMsgReciever"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                var record = _hrUnitOfWork.Repository<Message>().FirstOrDefault(j => j.Id == model.Id);
                if(model.All == true)
                {
                    model.IJobs = null;
                    model.IPeopleGroups = null;
                    model.IEmployees = null;
                    model.IDepts = null;
                }

                if (record == null) //Add
                {
                    record = new Message();
                    model.Jobs = model.IJobs == null ? null : string.Join(",", model.IJobs.ToArray());
                    model.Employees = model.IEmployees == null ? null : string.Join(",", model.IEmployees.ToArray());
                    model.PeopleGroups = model.IPeopleGroups == null ? null : string.Join(",", model.IPeopleGroups.ToArray());
                    model.Depts = model.IDepts == null ? null : string.Join(",", model.IDepts.ToArray());
                    moreInfo.VisibleColumns.Add("Jobs");
                    moreInfo.VisibleColumns.Add("Employees");
                    moreInfo.VisibleColumns.Add("PeopleGroups");
                    moreInfo.VisibleColumns.Add("Depts");
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "EmpMessageForm",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    if (model.Check)
                        record.Sent = true;

                    record.CompanyId = CompanyId;
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    record.FromEmpId = User.Identity.GetEmpId();
                    _hrUnitOfWork.MessageRepository.Add(record);
                }
                else //update
                {
                    model.Jobs = model.IJobs == null ? null : string.Join(",", model.IJobs.ToArray());
                    model.Employees = model.IEmployees == null ? null : string.Join(",", model.IEmployees.ToArray());
                    model.PeopleGroups = model.IPeopleGroups == null ? null : string.Join(",", model.IPeopleGroups.ToArray());
                    model.Depts = model.IDepts == null ? null : string.Join(",", model.IDepts.ToArray());
                    moreInfo.VisibleColumns.Add("Jobs");
                    moreInfo.VisibleColumns.Add("Employees");
                    moreInfo.VisibleColumns.Add("PeopleGroups");
                    moreInfo.VisibleColumns.Add("Depts");
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "EmpMessageForm",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    if (model.Check)
                        record.Sent = true;

                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    _hrUnitOfWork.MessageRepository.Attach(record);
                    _hrUnitOfWork.MessageRepository.Entry(record).State = EntityState.Modified;

                }

             
                var Errors = SaveChanges(Language);

                if (model.Check && Errors.Count == 0)
                {
                    _hrUnitOfWork.MessageRepository.Send(record.Id, CompanyId);
                    model.Id = record.Id;
                }
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(model));

                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
       
        public ActionResult DeleteMessage(int Id)
        {
            var Message = "OK";
            var Mess = _hrUnitOfWork.MessageRepository.GetMessage(Id);
            if (Mess != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = Mess,
                    ObjectName = "EmpMessages",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.MessageRepository.Remove(Mess);
            }

            try
            {
                var err = SaveChanges(Language);
                if (err.Count() > 0)
                {
                    foreach (var item in err)
                    {
                        Message = item.errors.Select(a => a.message).FirstOrDefault();
                    }
                    return Json(Message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(Message, JsonRequestBehavior.AllowGet);
        }

    }
}