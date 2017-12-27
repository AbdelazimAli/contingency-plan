using Interface.Core;
using Model.Domain.Notifications;
using Model.ViewModel;
using Model.ViewModel.Notification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;
using System.Linq.Dynamic;
using System.Linq;
using System.Data;
using Model.Domain;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;
using Hangfire;
using System.Threading.Tasks;
using System.Net.Mail;
using Model.ViewModel.Personnel;

namespace WebApp.Controllers
{
    public class NotificationController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        UserContext db = new UserContext();
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
        public NotificationController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region Message Template
        //public ActionResult MsgTemplateIndex()
        //{
        //    return View();
        //}
        ////GetMessageTemp
        //public ActionResult GetMessageTemp()
        //{
        //    int companyId = CompanyId;
        //    string culture = Language;
        //    var query = _hrUnitOfWork.NotificationRepository.ReadMessageTemp(companyId,culture);
        //    return Json(query, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult Details(int id = 0)
        //{
        //    ViewBag.EmailId = _hrUnitOfWork.Repository<EmailAccount>().Select(s => new { id = s.Id, name = s.DisplayName }).ToList();
        //    string culture = Language;
        //    int companyId = CompanyId;
        //    if (id == 0)
        //        return View(new MsgTemplateViewModel());
        //    var message = _hrUnitOfWork.NotificationRepository.ReadFormMsgTemplate(id,culture,companyId);
        //    return message == null ? (ActionResult)HttpNotFound() : View(message);
        //}

        ////SaveMsgTemplate
        //public ActionResult SaveMsgTemplate(MsgTemplateViewModel model, OptionsViewModel moreInfo)
        //{
        //    List<Error> errors = new List<Error>();
        //    if (ModelState.IsValid)
        //    {
        //        if (ServerValidationEnabled)
        //        {
        //            errors = _hrUnitOfWork.MessageRepository.CheckForm(new CheckParm
        //            {
        //                CompanyId = CompanyId,
        //                ObjectName = "MsgTemplateForm",
        //                TableName = "MsgTemplates",
        //                Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
        //                ParentColumn = "Id",
        //                Culture = Language
        //            });

        //            if (errors.Count() > 0)
        //            {
        //                foreach (var e in errors)
        //                {
        //                    foreach (var errorMsg in e.errors)
        //                    {
        //                        ModelState.AddModelError(errorMsg.field, errorMsg.message);
        //                    }
        //                }

        //                return Json(Models.Utils.ParseFormErrors(ModelState));
        //            }
        //        }

        //        var record = _hrUnitOfWork.Repository<MsgTemplate>().FirstOrDefault(j => j.Id == model.Id);
        //        var MsgTempLang = _hrUnitOfWork.Repository<MsgTemplang>().FirstOrDefault(j => j.TemplateId == model.Id);
        //        if (record == null) //Add
        //        {
        //            record = new MsgTemplate();

        //            AutoMapper(new Models.AutoMapperParm
        //            {
        //                Destination = record,
        //                Source = model,
        //                ObjectName = "MsgTemplateForm",
        //                Version = Convert.ToByte(Request.Form["Version"]),
        //                Options = moreInfo
        //            });
        //            record.CompanyId = CompanyId;
        //            record.CreatedTime = DateTime.Now;
        //            record.CreatedUser = UserName;
        //            _hrUnitOfWork.NotificationRepository.Add(record);
        //             MsgTempLang = new MsgTemplang
        //            {
        //                Bcc = model.Bcc,
        //                Body = model.Body,
        //                Culture = Language,
        //                EmailId = model.EmailId,
        //                Subject = model.Subject,
        //                Template =record
        //            };
        //            _hrUnitOfWork.NotificationRepository.Add(MsgTempLang);
        //        }
        //        else //update
        //        {
        //            AutoMapper(new Models.AutoMapperParm
        //            {
        //                Destination = record,
        //                Source = model,
        //                ObjectName = "MsgTemplateForm",
        //                Version = Convert.ToByte(Request.Form["Version"]),
        //                Options = moreInfo
        //            });

        //            record.ModifiedTime = DateTime.Now;
        //            record.ModifiedUser = UserName;
        //            _hrUnitOfWork.NotificationRepository.Attach(record);
        //            _hrUnitOfWork.NotificationRepository.Entry(record).State = EntityState.Modified;
        //            MsgTempLang.Subject = model.Subject;
        //            MsgTempLang.EmailId = model.EmailId;
        //          //  MsgTempLang.Culture = Language;
        //            MsgTempLang.Body = model.Body;
        //            MsgTempLang.Bcc = model.Bcc;
        //            _hrUnitOfWork.NotificationRepository.Attach(MsgTempLang);
        //            _hrUnitOfWork.NotificationRepository.Entry(MsgTempLang).State = EntityState.Modified;

        //        }
        //        var Errors = SaveChanges(Language);

        //        string message = "OK," + ((new JavaScriptSerializer()).Serialize(model));

        //        if (Errors.Count > 0)
        //            message = Errors.First().errors.First().message;

        //        return Json(message);
        //    }
        //    else
        //    {
        //        return Json(Models.Utils.ParseFormErrors(ModelState));
        //    }
        //}

        //public ActionResult DeleteMessageTemp(int Id)
        //{
        //    var Message = "OK";
        //    var Mess = _hrUnitOfWork.NotificationRepository.Get(Id);
        //    if (Mess != null)
        //        _hrUnitOfWork.NotificationRepository.Remove(Mess);

        //    try
        //    {
        //        var err = SaveChanges(Language);
        //        if (err.Count() > 0)
        //        {
        //            foreach (var item in err)
        //            {
        //                Message = item.errors.Select(a => a.message).FirstOrDefault();
        //            }
        //            return Json(Message, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
        //        if (msg.Length > 0)
        //            return Json(msg, JsonRequestBehavior.AllowGet);
        //    }

        //    return Json(Message, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Email Account
        public ActionResult EmailAccountIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetEmailAccount()
        {
            var query = _hrUnitOfWork.NotificationRepository.ReadEmailAccount();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetailsOfEmail(int id = 0)
        {
            if (id == 0)
                return View(new EmailAccountFormViewModel());
            var Email = _hrUnitOfWork.NotificationRepository.ReadFormEmailAccount(id);
            return Email == null ? (ActionResult)HttpNotFound() : View(Email);
        }
        public ActionResult SaveEmailAccount(EmailAccountFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.MessageRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmailAccountForm",
                        TableName = "EmailAccounts",
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

                var record = _hrUnitOfWork.Repository<EmailAccount>().FirstOrDefault(j => j.Id == model.Id);

                if (record == null) //Add
                {
                    record = new EmailAccount();

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "EmailAccountForm",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = Model.Domain.TransType.Insert
                    });

                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    _hrUnitOfWork.NotificationRepository.Add(record);
                }
                else //update
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "EmailAccountForm",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = Model.Domain.TransType.Update
                    });

                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    _hrUnitOfWork.NotificationRepository.Attach(record);
                    _hrUnitOfWork.NotificationRepository.Entry(record).State = EntityState.Modified;

                }
                var Errors = SaveChanges(Language);

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
        [HttpPost]
        public ActionResult SendTestEmail(EmailAccountFormViewModel model)
        {
            EmailAccount Email = _hrUnitOfWork.Repository<EmailAccount>().Where(a => a.Id == model.Id).FirstOrDefault();
            if (Email != null)
            {
                try
                {
                    Db.Persistence.Services.EmailService.SendEmail(Email, "Subject Test", "Test Email", model.TestEmail, model.DisplayName);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ModelState.AddModelError("", MsgUtils.Instance.Trls("SaveFailed"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region NotificationList
        public ActionResult SchedualeTask()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadNotification(int MenuId)
        {
            var query = _hrUnitOfWork.NotificationRepository.ReadSchedualeTasks(CompanyId);
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
        public ActionResult UpdateNotification(IEnumerable<SchedualeTaskViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<SchedualeTaskViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "SchedualeNotifications",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                for (var i = 0; i < models.Count(); i++)
                {
                    var model = models.ElementAtOrDefault(i);
                    var SchedualTsk = _hrUnitOfWork.Repository<SchedualTask>().Where(a => a.EventId == model.Id).FirstOrDefault();
                    SchedualTsk.CompanyId = CompanyId;
                    SchedualTsk.Enabled = model.Enabled;
                    SchedualTsk.PeriodInMinutes = model.PeriodInMinutes;
                    SchedualTsk.StopOnError = model.StopOnError;
                    SchedualTsk.EventName = model.EventName;
                    SchedualTsk.ModifiedTime = DateTime.Now;
                    SchedualTsk.ModifiedUser = UserName;

                    if (!model.Enabled)
                        RecurringJob.RemoveIfExists(model.Id.ToString());
                    else
                    {
                        string method = model.EventUrl.Split('/')[2];

                        Type thistype = GetType();
                        System.Reflection.MethodInfo ModelMethod = thistype.GetMethod(method);
                        ModelMethod.Invoke(this, new object[] { model.Id, model.PeriodInMinutes, true, model.StopOnError, true });
                        var Now = DateTime.Now;
                        SchedualTsk.LastEndDate = Now;
                        SchedualTsk.LastStartDate = Now;
                        SchedualTsk.LastSuccessDate = Now;
                    }

                    _hrUnitOfWork.NotificationRepository.Attach(SchedualTsk);
                    _hrUnitOfWork.NotificationRepository.Entry(SchedualTsk).State = EntityState.Modified;
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult RunNow(int id, int period, string EvenUrl, bool Enabled, bool OnError)
        {
            string Url = string.Format("{0}?Id={1}&Period={2}&Enable={3}&OnStopError={4}&Invoke={5}", EvenUrl, id, period, Enabled, OnError, false);
            RecurringJob.Trigger(id.ToString());
            return Redirect(Url);
        }
        public ActionResult ExtendContract(int Id, int Period, bool Enable, bool OnStopError, bool Invoke)
        {
            string msg = "Enabled";
            if (Enable)
            {

                try
                {
                    RecurringJob.AddOrUpdate(Id.ToString(), () => HangFireJobs.ExtendContract(Language), Cron.Daily(09));
                }
                catch
                {
                    if (OnStopError)
                        RecurringJob.RemoveIfExists(Id.ToString());
                }


                if (!Invoke)
                {
                    var model = _hrUnitOfWork.Repository<SchedualTask>().Where(a => a.EventId == Id).FirstOrDefault();
                    UpdateTimeofTask(model);
                }
            }
            else
                msg = "Disabled";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddNotify(int Id, int Period, bool Enable, bool OnStopError, bool Invoke)
        {

            string msg = "Enabled";
            if (Enable)
            {

                EmailAccount ea = _hrUnitOfWork.Repository<EmailAccount>().FirstOrDefault();

                RecurringJob.AddOrUpdate(Id.ToString(), () => HangFireJobs.ReadNotifications(User.Identity.GetDefaultCompany(), User.Identity.GetEmpId(), User.Identity.GetLanguage()), Cron.MinuteInterval(Period));


                if (!Invoke)
                {
                    var model = _hrUnitOfWork.Repository<SchedualTask>().Where(a => a.EventId == Id).FirstOrDefault();
                    UpdateTimeofTask(model);
                }
            }
            else
                msg = "Disabled";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendEmail(int Id, int Period, bool Enable, bool OnStopError, bool Invoke)
        {
            string msg = "Enabled";
            if (Enable)
            {
                EmailAccount emailAcc = _hrUnitOfWork.Repository<EmailAccount>().FirstOrDefault();

                try
                {
                    RecurringJob.AddOrUpdate(Id.ToString(), () => Db.Persistence.Services.EmailService.SendEmail(emailAcc, "", "", "", "", null, null, null, null, null, null, null, null), Cron.MinuteInterval(Period));
                }
                catch
                {
                    if (OnStopError)
                        RecurringJob.RemoveIfExists(Id.ToString());
                }

                if (!Invoke)
                {
                    var model = _hrUnitOfWork.Repository<SchedualTask>().Where(a => a.EventId == Id).FirstOrDefault();
                    UpdateTimeofTask(model);
                }
            }
            else
                msg = "Disabled";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        private void UpdateTimeofTask(SchedualTask task)
        {
            var Now = DateTime.Now;
            task.LastEndDate = Now;
            task.LastStartDate = Now;
            task.LastSuccessDate = Now;
            task.Enabled = true;
            _hrUnitOfWork.NotificationRepository.Attach(task);
            _hrUnitOfWork.NotificationRepository.Entry(task).State = EntityState.Modified;
            SaveChanges(Language);

        }
        public ActionResult SMS(int Id, int Period, bool Enable, bool OnStopError, bool Invoke)
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult KeepAlive(int Id, int Period, bool Enable, bool OnStopError, bool Invoke)
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNotify(int Id)
        {
            var model = _hrUnitOfWork.NotificationRepository.GetNotify(Id, Language, CompanyId, UserName);

            return View(model);
        }
        public ActionResult GetAllNotifications()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetNotification(int MenuId, int pageSize, int skip)
        {
            var query = _hrUnitOfWork.NotificationRepository.GetCompanyNotifications(UserName, CompanyId);
            string filter = "";
            string Sorting = "";
            string whecls = GetWhereClause(MenuId);

            query = (IQueryable<NotificationViewModel>)Utils.GetFilter(query, ref filter, ref Sorting);

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
        public ActionResult MarkAsRead(int id)
        {
            string msg = "Ok";
            var model = _hrUnitOfWork.Repository<WebMobLog>().Where(a => a.Id == id).FirstOrDefault();
            if (!model.MarkAsRead)
            {
                model.MarkAsRead = true;
                _hrUnitOfWork.NotificationRepository.Attach(model);
                _hrUnitOfWork.NotificationRepository.Entry(model).State = EntityState.Modified;
                SaveChanges(Language);
            }
            else
                msg = "Read";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadAll()
        {
            string msg = "Ok";
            var model = _hrUnitOfWork.Repository<WebMobLog>().Where(a => a.SentToUser.ToLower() == UserName.ToLower() && !a.MarkAsRead).ToList();
            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    item.MarkAsRead = true;
                    _hrUnitOfWork.NotificationRepository.Attach(item);
                    _hrUnitOfWork.NotificationRepository.Entry(item).State = EntityState.Modified;
                    SaveChanges(Language);
                }
            }
            else
                msg = "Read";


            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Index Grid Notification
        public ActionResult IndexOfNotifications()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetNotifyConditions(int MenuId)
        {
            var query = _hrUnitOfWork.NotificationRepository.ReadNotificationConditions(CompanyId);
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
        public ActionResult NotificationMenu(string TableName, string ColumnName, string ObjectName, byte Version)
        {
            ViewBag.CurrentId = Request.QueryString["CurrentId"];
            //ViewBag.TemplateId = _hrUnitOfWork.Repository<MsgTemplate>().Select(s => new { id = s.Id, name = s.Name }).ToList();
            ViewBag.Users = db.Users.Select(a => new { id = a.Id, name = a.UserName }).ToList();
            var Columns = _hrUnitOfWork.NotificationRepository.GetColumnList(TableName, ObjectName, Version, Request.QueryString["Type"], CompanyId, Language);
            ViewBag.Columns = Columns;
            ViewBag.TableTitle = Columns.FirstOrDefault()?.pageTitle;
            ViewBag.HasCompany = Columns.Where(c => c.id == "CompanyId").FirstOrDefault() != null;
            ViewBag.HasEmpId = Columns.Where(c => c.id == "EmpId").FirstOrDefault() != null;

            return View(new NotifyConditionViewModel() { TableName = TableName, ObjectName = ObjectName, ColumnName = ColumnName });
        }
        //SaveNotification
        public ActionResult SaveNotification(NotifyConditionViewModel model, IEnumerable<FilterGridViewModel> grid1, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            var msg = "OK";
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LocationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Notify",
                        TableName = "NotifyConditions",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            int[] reminderEvent = { (int)Events.IsDueIn, (int)Events.IsDueTo, (int)Events.WasDueThisAmountofTimeAgo };

            var NotifyCondition = _hrUnitOfWork.Repository<NotifyCondition>().Where(a => a.Id == model.Id).FirstOrDefault();
            if (NotifyCondition == null)
            {
                NotifyCondition = new NotifyCondition();

                var parm = new Models.AutoMapperParm
                {
                    Destination = NotifyCondition,
                    Source = model,
                    ObjectName = "Notify",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Insert
                };

                AutoMapper mapper = new Models.AutoMapper(parm, _hrUnitOfWork, User.Identity);
                mapper.Map();
                NotifyCondition.CompanyId = CompanyId;
                NotifyCondition.CreatedTime = DateTime.Now;
                NotifyCondition.CreatedUser = UserName;
                NotifyCondition.ColumnName = model.ColumnName;
                NotifyCondition.TableName = model.TableName;
                NotifyCondition.ObjectName = model.ObjectName;
                NotifyCondition.Version = model.Version;
                NotifyCondition.Message = model.EncodedMsg.Replace("&nbsp;", " ").Replace("&lt;", "<").Replace("&gt;", ">");
                NotifyCondition.EventValue = model.EventValue;
                Events ev = (Events)Enum.ToObject(typeof(Events), model.Event);
                NotifyCondition.Event = ev;
                List<string> ListOfUsers = new List<string>();
                if (model.Users != null)
                {
                    foreach (var item in model.Users)
                    {
                        var users = db.Users.Where(a => a.Id == item).Select(a => a.UserName).FirstOrDefault();
                        ListOfUsers.Add(users);
                    }
                }
                NotifyCondition.Users = string.Join(",", ListOfUsers.ToArray());
                if (model.Message != null)
                {
                    var ListOfFields = GetFields(model.Message).Distinct().ToList();
                    if (model.HasEmpId && !ListOfFields.Contains("EmpId") && reminderEvent.Contains(model.Event)) ListOfFields.Add("EmpId");

                    NotifyCondition.Fields = string.Join(",", ListOfFields.ToArray());
                }


                /// Check Filter
                var columns = _hrUnitOfWork.NotificationRepository.GetColumnList(model.TableName, model.ObjectName, model.Version, model.divType, CompanyId, User.Identity.GetCulture());
                if (!String.IsNullOrEmpty(NotifyCondition.filter))
                {
                    var message = ApplyFilter(NotifyCondition.filter, columns);
                    if (!String.IsNullOrEmpty(message))
                    {
                        ModelState.AddModelError("filter", message);
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }

                if (grid1 != null)
                    SaveFilterGrid(grid1, NotifyCondition);

                _hrUnitOfWork.NotificationRepository.Add(NotifyCondition);
            }
            else
            {
                var parm = new Models.AutoMapperParm
                {
                    Destination = NotifyCondition,
                    Source = model,
                    ObjectName = "Notify",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Update
                };

                AutoMapper mapper = new Models.AutoMapper(parm, _hrUnitOfWork, User.Identity);
                mapper.Map();
                NotifyCondition.CompanyId = User.Identity.GetDefaultCompany();
                NotifyCondition.Message = model.EncodedMsg.Replace("&nbsp;", " ").Replace("&lt;", "<").Replace("&gt;", ">");
                Events ev = (Events)Enum.ToObject(typeof(Events), model.Event);
                NotifyCondition.Event = ev;
                NotifyCondition.EventValue = model.EventValue;
                //NotifyCondition.CreatedTime = DateTime.Now;
                //NotifyCondition.CreatedUser = UserName;
                //NotifyCondition.ColumnName = model.ColumnName;
                //NotifyCondition.TableName = model.TableName;
                //NotifyCondition.ObjectName = model.ObjectName;
                // NotifyCondition.Version = model.Version;
                //Events ev = (Events)Enum.ToObject(typeof(Events), model.Event);
                //NotifyCondition.Event = ev;
                List<string> ListOfUsers = new List<string>();
                if (model.Users != null)
                {
                    foreach (var item in model.Users)
                    {
                        //var users = db.Users.Where(a => a.UserName == item).Select(a => a.UserName).FirstOrDefault();
                        ListOfUsers.Add(item);
                    }
                }
                NotifyCondition.Users = string.Join(",", ListOfUsers.ToArray());
                if (model.Message != null)
                {
                    var ListOfFields = GetFields(model.Message).Distinct().ToList();
                    if (model.HasEmpId && !ListOfFields.Contains("EmpId") && reminderEvent.Contains(model.Event)) ListOfFields.Add("EmpId");
                    NotifyCondition.Fields = string.Join(",", ListOfFields.ToArray());
                }
                //mapper.AddNotification(NotifyCondition, model.DateTimeValue, model.SourceId);
                /// Check Filter
                var columns = _hrUnitOfWork.NotificationRepository.GetColumnList(model.TableName, model.ObjectName, model.Version, model.divType, CompanyId, User.Identity.GetCulture());
                if (!String.IsNullOrEmpty(NotifyCondition.filter))
                {
                    var message = ApplyFilter(NotifyCondition.filter, columns);
                    if (!String.IsNullOrEmpty(message))
                    {
                        ModelState.AddModelError("filter", message);
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }

                if (grid1 != null)
                    SaveFilterGrid(grid1, NotifyCondition);
                _hrUnitOfWork.NotificationRepository.Attach(NotifyCondition);
                _hrUnitOfWork.NotificationRepository.Entry(NotifyCondition).State = EntityState.Modified;


            }
            errors = SaveChanges(Language);

            if (errors.Count() > 0)
            {
                foreach (var item in errors)
                {
                    msg = item.errors.Select(a => a.message).FirstOrDefault();
                }
                return Json(msg);
            }

            return Json(msg);

        }

        List<string> GetFields(string message)
        {
            StringBuilder result = new StringBuilder();
            List<string> fields = new List<string>();
            MatchCollection allcollection = null;
            var regexObj = new Regex(@"%[\w.]*%");
            allcollection = regexObj.Matches(message);
            foreach (var item in allcollection)
            {
                var field = item.ToString().Replace("%", "");
                fields.Add(field);
            }
            return fields;
        }

        void SaveFilterGrid(IEnumerable<FilterGridViewModel> models, NotifyCondition notifyCond)
        {
            for (var i = 0; i < models.Count(); i++)
            {
                var filter = new Model.Domain.Notifications.Filter();
                AutoMapper(new AutoMapperParm() { Destination = filter, Source = models.ElementAtOrDefault(i) });
                if (filter.ColumnType == "date")
                    filter.Value = DateTime.Parse(models.ElementAtOrDefault(i).ValueText).ToString("yyyy/MM/dd");
                filter.NotifyCond = notifyCond;

                if (models.ElementAtOrDefault(i).Id == 0) //Add
                {
                    _hrUnitOfWork.NotificationRepository.Add(filter);
                }
                else // update
                {
                    _hrUnitOfWork.NotificationRepository.Attach(filter);
                    _hrUnitOfWork.NotificationRepository.Entry(filter).State = EntityState.Modified;
                }
            }
        }

        string ApplyFilter(string filter, IEnumerable<NotifyColumnsViewModel> sourceColumns)
        {
            string message = null;

            //Get used columns only
            string[] filterList = filter.Split(' ');
            if (sourceColumns != null)
            {
                var Columns = sourceColumns.Where(s => filterList.Where(f => f == s.id).FirstOrDefault() != null)
                    .Select(s => new { s.id, s.type });

                foreach (var item in Columns)
                {
                    switch (item.type)
                    {
                        case "string":
                            filter = filter.Replace(item.id, "'column'");
                            break;
                        case "number":
                            filter = filter.Replace(item.id, "1");
                            break;
                        case "boolean":
                            filter = filter.Replace(item.id, "true");
                            break;
                        case "date":
                            filter = filter.Replace(item.id, "'2000-01-01'");
                            break;
                        default:
                            break;
                    }
                }
                filter = filter.Replace("Id", "'0'");
                filter = filter.Replace('"', '\'');

                //Evaluate
                DataTable dt = new DataTable();
                try
                {
                    dt.Compute(filter, "");
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
                return message;
            }
            else
            {
                return MsgUtils.Instance.Trls("Can't find columns");
            }
        }

        //Grid
        public ActionResult ReadCondition(int notifyCondId)
        {
            return Json(_hrUnitOfWork.NotificationRepository.ReadCondition(notifyCondId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidateCondition(IEnumerable<FilterGridViewModel> models)
        {
            var datasource = new DataSource<FilterGridViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Conditions",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors != null && datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        public ActionResult DeleteCondition(int id)
        {
            var message = "OK";
            DataSource<FilterGridViewModel> Source = new DataSource<FilterGridViewModel>();

            Model.Domain.Notifications.Filter filter = _hrUnitOfWork.Repository<Model.Domain.Notifications.Filter>().Where(f => f.Id == id).FirstOrDefault();
            if (filter == null)
                return Json(message);

            _hrUnitOfWork.NotificationRepository.Remove(filter);

            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }
        public ActionResult NotificationEdit(int id = 0)
        {
            var Notify = _hrUnitOfWork.NotificationRepository.ReadNotificationCondition(id);
            ViewBag.CurrentId = Notify.SourceId;
            ViewBag.Users = db.Users.Select(a => new { id = a.UserName, name = a.UserName }).ToList();
            var Columns = _hrUnitOfWork.NotificationRepository.GetColumnList(Notify.TableName, Notify.ObjectName, Notify.Version, Request.QueryString["Type"], CompanyId, Language);
            ViewBag.Columns = Columns;
            ViewBag.TableTitle = Columns.FirstOrDefault()?.pageTitle;
            ViewBag.HasCompany = Columns.Where(c => c.id == "CompanyId").FirstOrDefault() != null;

            return Notify == null ? (ActionResult)HttpNotFound() : View("NotificationMenu", Notify);
        }
        //DeleteNotification
        public ActionResult DeleteNotification(int Id)
        {
            DataSource<NotifyConditionIndexViewModel> Source = new DataSource<NotifyConditionIndexViewModel>();
            Source.Errors = new List<Error>();
            var Notify = _hrUnitOfWork.Repository<NotifyCondition>().FirstOrDefault(a => a.Id == Id);
            if (Notify != null)
                _hrUnitOfWork.NotificationRepository.Remove(Notify);
            Source.Errors = SaveChanges(Language);
            string message = "OK";
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else
                return Json(message);
        }

        #endregion

        #region SMS Log

        public ActionResult SMSLogIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadSMSLog()
        {
            var query = _hrUnitOfWork.NotificationRepository.ReadSMSLogs(CompanyId);
            return Json(query, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Email Log
        public ActionResult EmailLogIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadEmailLog()
        {
            var query = _hrUnitOfWork.NotificationRepository.ReadEmailLogs();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region NotifyLetter
        public ActionResult NotifyLetterIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        public ActionResult GetEmpNotificationsLetters(int MenuId, int pageSize, int skip)
        {
            var query = _hrUnitOfWork.NotificationRepository.GetEmpLetters(CompanyId, Language);
            string filter = "";
            string Sorting = "";
            string whecls = GetWhereClause(MenuId);
            query = (IQueryable<NotifiyLetterViewModel>)Utils.GetFilter(query, ref filter, ref Sorting);
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

            if (Sorting.Length > 0)
                query = query.OrderBy(Sorting).Skip(skip).Take(pageSize);
            else if (skip > 0)
                query = query.Skip(skip).Take(pageSize);
            else
                query = query.Take(pageSize);

            return Json(new { total = total, data = query.ToList() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SendSms
        [HttpPost]
        public ActionResult SendSMS()
        {
            Task<string> result = SmsMisrService.SendSms("https://SmsMisr.com/Api/Send.aspx", "WDN35VUW", "WDN35V", "DoubleClick", "01007437151", "Hello");
            return Json(result);
        }
        #endregion

        #region Meetings

        public ActionResult MeetingIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        //ReadMeeting
        public ActionResult ReadMeeting(int MenuId, int pageSize, int skip, byte? Range, DateTime? Start, DateTime? End)
        {
            var query = _hrUnitOfWork.NotificationRepository.GetMeetings(Range ?? 10, Start, End, Language, CompanyId);
            string filter = "";
            string Sorting = "";
            string whecls = GetWhereClause(MenuId);
            query = (IQueryable<MeetingViewModel>)Utils.GetFilter(query, ref filter, ref Sorting);
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

            if (Sorting.Length > 0)
                query = query.OrderBy(Sorting).Skip(skip).Take(pageSize);
            else if (skip > 0)
                query = query.Skip(skip).Take(pageSize);
            else
                query = query.Take(pageSize);

            return Json(new { total = total, data = query.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MeetingDetails(int id = 0, byte Version = 0)
        {
            ViewBag.LocationId = _hrUnitOfWork.LocationRepository.ReadLocations(Language, CompanyId).Where(a => a.IsInternal).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.Emps = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { value = a.Id, text = a.Employee }).ToList();
            List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("MeetingForm", CompanyId, Version);
            if (columns.Where(fc => fc == "EmpId").FirstOrDefault() == null)
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee }).ToList();
            if (id == 0)
                return View(new MeetingViewModel());
            var Meeting = _hrUnitOfWork.NotificationRepository.ReadMeeting(id);
            ViewBag.Attendee = _hrUnitOfWork.NotificationRepository.GetMeetingAttendee(Meeting.Id,Language);
            return Meeting == null ? (ActionResult)HttpNotFound() : View(Meeting);
        }
        public ActionResult ReadAgenda(int MeetingId)
        {
            var query = _hrUnitOfWork.NotificationRepository.GetAgenda(MeetingId, Language);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        #endregion



    }
}
