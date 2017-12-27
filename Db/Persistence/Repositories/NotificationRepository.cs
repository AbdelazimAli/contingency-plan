using Interface.Core.Repositories;
using Model.Domain.Notifications;
using Model.ViewModel.Notification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Collections;
using Model.ViewModel.Personnel;
using Model.ViewModel;
using Model.Domain;

namespace Db.Persistence.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {

        public NotificationRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        #region Catch immediatally notification
        public IList<NotifyCondition> GetNotifyConditions(int companyId)
        {
            return context.NotifyConditions.Where(c => c.CompanyId == companyId).ToList();
        }
        public void AddInsertNotification(int id, string tablename, DbEntityEntry entity)
        {
            StringBuilder s = new StringBuilder(tablename + " (");
            int i = 10; // limit iterations for only ten times
            foreach (var property in entity.CurrentValues.PropertyNames)
            {
                if (i == 0)
                {
                    s.Append(", ...");
                    break;
                }
                else if (i != 10) s.Append(", ");

                s.Append(property + ":" + entity.CurrentValues.GetValue<object>(property));
                i--;
            }

            AddNotification(id, "Record Inserted", message: s.ToString() + ") inserted");
        }
        public void AddUpdateNotification(int id, string tablename, string columname, DbEntityEntry entity)
        {
            SendNotifications(0);
            AddNotification(id, "Record Updated", message: tablename + " (update " + columname + ": " + entity.CurrentValues.GetValue<object>(columname) + ")");
        }
        public void AddDeleteNotification(int id, string tablename, DbEntityEntry entity)
        {
            StringBuilder s = new StringBuilder(tablename + " (");
            int i = 10; // limit iterations for only ten times
            foreach (var property in entity.OriginalValues.PropertyNames)
            {
                if (i == 0)
                {
                    s.Append(", ...");
                    break;
                }
                else if (i != 10) s.Append(", ");

                s.Append(property + ":" + entity.OriginalValues.GetValue<object>(property));
                i--;
            }

            AddNotification(id, "Record Deleted", message: s.ToString() + ") deleted");
        }

        private void AddNotification(int id, string subject, string message)
        {
            context.Notification.Add(new Model.Domain.Notifications.Notification
            {
                ConditionId = id,
                CreationTime = DateTime.Now,
                Subject = subject,
                Message = message
            });
        }

        public string SendNotifications(int companyId)
        {
            // get unread notifications
            //var messages = from n in context.Notification
            //               where n.CompanyId == companyId && n.ReadOK == false
            //               join consign in context.NotifyConsigns on n.ConditionId equals consign.NotifyCondId
            //               join a in context.Assignments on consign.JobId equals a.JobId into g1
            //               from a in g1.Where(x => x.CompanyId == companyId && x.AssignDate <= DateTime.Today && x.EndDate >= DateTime.Today).DefaultIfEmpty()
            //               join b in context.Assignments on consign.PositionId.Value equals b.PositionId.Value into g2
            //               from b in g2.Where(x => x.CompanyId == companyId && x.AssignDate <= DateTime.Today && x.EndDate >= DateTime.Today).DefaultIfEmpty()
            //               select new { consign.EmpId, job = a.EmpId, pos = b.EmpId, consign.CustMail };
            //if (cond.Email) // send by email
            //{
            //    var emailAccount = context.EmailAccounts.Find(1);
            //    Db.EmailService.SendEmail(emailAccount, subject, message, "waleedhashem@gmail.com", "Waleed Hashem");
            //}

            return "Ok";
        }

        #endregion

        #region Message Template 
        //public IQueryable<MsgTemplateViewModel> ReadMessageTemp(int companyId ,string culture)
        //{
        //    var msg = from m in context.MsgTemplates
        //              where m.CompanyId == companyId
        //              join l in context.MsgTemplangs on m.Id equals l.TemplateId
        //              where l.Culture == culture
        //              select new MsgTemplateViewModel
        //              {
        //                  Id = m.Id,
        //                  Body = l.Body,
        //                  Name = m.Name,
        //                  ModifiedTime = m.ModifiedTime,
        //                  ModifiedUser = m.ModifiedUser,
        //                  CreatedTime = m.CreatedTime,
        //                  CreatedUser = m.CreatedUser
        //              };
        //    return msg;

        //}
        ////ReadFormMsgTemplate
        //public MsgTemplateViewModel ReadFormMsgTemplate(int id, string culture, int companyId)
        //{
        //    var query = from msg in context.MsgTemplates
        //                where msg.Id == id && msg.CompanyId == companyId
        //                join msgLang in context.MsgTemplangs on msg.Id equals msgLang.TemplateId
        //                where msgLang.Culture == culture
        //                select new MsgTemplateViewModel
        //                {
        //                    Id = msg.Id,
        //                    Name = msg.Name,
        //                    Body = msgLang.Body,
        //                    StartDate = msg.StartDate,
        //                    EndDate = msg.EndDate,
        //                    Bcc = msgLang.Bcc,
        //                    Subject = msgLang.Subject,
        //                    EmailId = msgLang.EmailId
        //                };
        //    return query.FirstOrDefault();

        //}
        //public void Add(MsgTemplang msglang)
        //{
        //    context.MsgTemplangs.Add(msglang);
        //}
        //public void Attach(MsgTemplang msglang)
        //{
        //    context.MsgTemplangs.Attach(msglang);
        //}
        //public void Remove(MsgTemplang msglang)
        //{
        //    if (Context.Entry(msglang).State == EntityState.Detached)
        //    {
        //        context.MsgTemplangs.Attach(msglang);
        //    }
        //    context.MsgTemplangs.Remove(msglang);
        //}
        //public DbEntityEntry<MsgTemplang> Entry(MsgTemplang msglang)
        //{
        //    return Context.Entry(msglang);
        //}
        #endregion

        #region Email Accounts
        public IQueryable<EmailAccountViewModel> ReadEmailAccount()
        {
            var email = from e in context.EmailAccounts
                        select new EmailAccountViewModel
                        {
                            Id = e.Id,
                            DisplayName = e.DisplayName,
                            Email = e.Email,
                            EnableSsl = e.EnableSsl,
                            UseDefaultCredentials = e.UseDefaultCredentials,
                            Username = e.Username
                        };
            return email;

        }

        public EmailAccountFormViewModel ReadFormEmailAccount(int id)
        {
            var query = from email in context.EmailAccounts
                        where email.Id == id
                        select new EmailAccountFormViewModel
                        {
                            Id = email.Id,
                            DisplayName = email.DisplayName,
                            Email = email.Email,
                            Password = email.Password,
                            Host = email.Host,
                            EnableSsl = email.EnableSsl,
                            Port = email.Port,
                            Username = email.Username,
                            UseDefaultCredentials = email.UseDefaultCredentials,
                            Capacity = email.Capacity,
                            LastSentDate = email.LastSentDate,
                            TodayCount = email.TodayCount,
                            CreatedTime = email.CreatedTime,
                            CreatedUser = email.CreatedUser,
                            ModifiedTime = email.ModifiedTime,
                            ModifiedUser = email.ModifiedUser
                        };
            return query.FirstOrDefault();

        }

        public void Add(EmailAccount email)
        {
            context.EmailAccounts.Add(email);
        }
        public void Add(EmailLog log)
        {
            context.EmailLog.Add(log);
        }

        public void Add(SmsLog log)
        {
            context.SmsLog.Add(log);
        }
        public void Attach(EmailAccount email)
        {
            context.EmailAccounts.Attach(email);
        }
        public void Remove(EmailAccount email)
        {
            if (Context.Entry(email).State == EntityState.Detached)
            {
                context.EmailAccounts.Attach(email);
            }
            context.EmailAccounts.Remove(email);
        }
        public DbEntityEntry<EmailAccount> Entry(EmailAccount email)
        {
            return Context.Entry(email);
        }

        public EmailAccount GetEmailAccount(int order)
        {
            var emails = context.EmailAccounts.Where(a => a.SendOrder > order).OrderBy(a => a.SendOrder).ToList();

            foreach (var email in emails)
            {
                if (email.LastSentDate == null || email.LastSentDate.Value.Date != DateTime.Today.Date)
                {
                    email.TodayCount = 0;
                    return email;
                }

                if (email.Capacity <= email.TodayCount)
                    continue;

                return email;
            }

            return null;
        }



        #endregion
        #region Notify Condition Index

        public IQueryable<NotifyConditionIndexViewModel> ReadNotificationConditions(int companyId)
        {
            var notify = from n in context.NotifyConditions
                         where n.CompanyId == companyId
                         select new NotifyConditionIndexViewModel
                         {
                             Id = n.Id,
                             AlertMeFor = n.AlertMeFor,
                             ColumnName = n.ColumnName,
                             CustEmail = n.CustEmail,
                             EncodedMsg = n.EncodedMsg,
                             Subject = n.Subject,
                             AlertMeUntil = n.AlertMeUntil,
                             Users = n.Users,
                             NotifyRef = n.NotifyRef,
                             Sms = n.Sms,
                             CreatedUser = n.CreatedUser,
                             ModifiedUser = n.ModifiedUser
                         };
            return notify;
        }
        public NotifyConditionViewModel ReadNotificationCondition(int id)
        {
            var n = context.NotifyConditions.FirstOrDefault(a => a.Id == id);
            var query = new NotifyConditionViewModel
            {
                Id = n.Id,
                ObjectName = n.ObjectName,
                EncodedMsg = n.EncodedMsg,
                Subject = n.Subject,
                AlertMeFor = n.AlertMeFor,
                AlertMeUntil = n.AlertMeUntil,
                ColumnName = n.ColumnName,
                CompanyId = n.CompanyId,
                NotifyRef = n.NotifyRef,
                Sms = n.Sms,
                TableName = n.TableName,
                filter = n.filter,
                Users = n.Users == null ? null : n.Users.Split(',').ToList(),
                CustEmail = n.CustEmail,
                Event = (byte)n.Event,
                EventValue = n.EventValue,
                CreatedTime = n.CreatedTime,
                CreatedUser = n.CreatedUser,
                ModifiedTime = n.ModifiedTime,
                ModifiedUser = n.ModifiedUser
            };
            return query;
        }

        #endregion
        //condition
        public NotifyCondition ReadNotifyCondition(int? CondId)
        {
            return context.NotifyConditions.Where(a => a.Id == CondId).FirstOrDefault();
        }
        public IQueryable<FilterGridViewModel> ReadCondition(int notifyCondId, string culture)
        {
            return context.Filters.Where(c => c.NotifyCondId == notifyCondId).Select(c => new FilterGridViewModel
            {
                Id = c.Id,
                NotifyCondId = c.NotifyCondId,
                ObjectName = c.ObjectName,
                Version = c.Version,
                ColumnType = c.ColumnType,
                ColumnName = c.ColumnName,
                Operator = c.Operator,
                Value = c.Value,
                AndOr = c.AndOr
            });
        }
        public IQueryable<NotifiyLetterViewModel> GetEmpLetters(int CompanyId,string Language)
        {
            DateTime Today = DateTime.Today.Date;
            return (from n in context.NotifyLetters
                    join a in context.Assignments on n.EmpId equals a.EmpId into g1
                    from a in g1.Where(x => x.CompanyId == n.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                    select new NotifiyLetterViewModel
                    {
                        CompanyId = n.CompanyId,
                        Department = HrContext.TrlsName(a.Department.Name,Language),
                        Description = n.Description,
                        Employee = HrContext.TrlsName(n.Emp.Title + " " + n.Emp.FirstName + " " + n.Emp.Familyname,Language),
                        EventDate = n.EventDate,
                        Id = n.Id,
                        Job = HrContext.TrlsName(a.Job.Name, Language),
                        NotifyDate = n.NotifyDate,
                        NotifySource = n.NotifySource,
                        ReadTime = n.ReadTime,
                        read = n.read,
                        Sent = n.Sent,
                        EmpId = n.EmpId
                    });
        }

        #region Meetings
        public IQueryable<MeetingViewModel> GetMeetings(byte Range, DateTime? Start, DateTime? End, string culture, int CompanyId)
        {
            //10- All, 0-Custom
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, CompanyId, out Start, out End);
            var query = (from m in context.Meetings
                         where m.Status != 3 && m.CompanyId == CompanyId
                         select new MeetingViewModel
                         {
                             Id = m.Id,
                             MeetDate = m.MeetDate,
                             Status = m.Status,
                             StartTime = m.StartTime.ToString(),
                             EndTime = m.EndTime.ToString(),
                             MeetSubject = HrContext.GetLookUpCode("MeetSubject", m.MeetSubject, culture),
                             EmpId = m.EmpId
                         }).ToList().Select(l => new MeetingViewModel
                         {
                             Id = l.Id,
                             StartTime = Convert.ToDateTime(l.StartTime).ToString("hh:mm tt"),
                             EndTime = Convert.ToDateTime(l.EndTime).ToString("hh:mm tt"),
                             Status = l.Status,
                             MeetDate = l.MeetDate,
                             SubjectDesc = l.SubjectDesc,
                             MeetSubject = l.MeetSubject,
                             Time = (Convert.ToDateTime(l.EndTime) - Convert.ToDateTime(l.StartTime)).ToString(@"hh\:mm"),
                             EmpId = l.EmpId
                         });
            if (Range != 10)
                query = query.Where(c => Start <= c.MeetDate && End >= c.MeetDate);

            return query.AsQueryable();
        }

        public MeetingFormViewModel ReadMeeting (int Id)
        {
            var query = (from m in context.Meetings
                        where m.Id == Id
                        select new MeetingFormViewModel
                        {
                            Id = m.Id,
                            MeetDate = m.MeetDate,
                            SubjectDesc = m.SubjectDesc,
                            StartTime = m.StartTime,
                            EndTime = m.EndTime,
                            MeetSubject = m.MeetSubject.ToString(),
                            MeetingAttendee = context.MeetAttendees.Where(a => a.MeetingId == Id).Select(s => s.EmpId).ToList(),
                            EmpId = m.EmpId,
                            LocationText = m.LocationText,
                            LocationId = m.LocationId
                        }).ToList().Select(l=>new MeetingFormViewModel
                        {

                            Id = l.Id,
                            StartTime = l.StartTime,
                            EndTime = l.EndTime,
                            Status = l.Status,
                            MeetDate = l.MeetDate,
                            SubjectDesc = l.SubjectDesc,
                            LocationText = l.LocationText,
                            MeetSubject = l.MeetSubject,
                            Time = (Convert.ToDateTime(l.EndTime) - Convert.ToDateTime(l.StartTime)).ToString(@"hh\:mm"),
                            EmpId = l.EmpId,
                            MeetingAttendee = l.MeetingAttendee,
                            LocationId = l.LocationId
                        }
                        ).FirstOrDefault();
            return query;
        }
        //GetMeetingAttendee
        public List<FormList> GetMeetingAttendee(int Id, string culture)
        {
            var query = (from m in context.MeetAttendees
                         where m.MeetingId == Id
                         select new FormList
                         {
                             id = m.EmpId,
                             name = HrContext.TrlsName(m.Attendee.Title + " " + m.Attendee.FirstName + " " + m.Attendee.Familyname, culture)
                         }).ToList();

            return query;

        }

        public IQueryable<MeetingAgendaViewModel> GetAgenda(int Id,string culture)
        {        
            var query = (from m in context.MeetScheduals
                        where m.MeetingId == Id
                         select new MeetingAgendaViewModel
                         {
                             Id = m.Id,
                             StartTime = m.StartTime.ToString(),
                             EndTime = m.EndTime.ToString(),
                             Description = m.Description,
                             EmpId = m.EmpId ,
                         }).ToList().Select(l => new MeetingAgendaViewModel
                         {
                             Id = l.Id,
                             StartTime = Convert.ToDateTime(l.EndTime).ToString("hh:mm tt"),
                             EndTime = Convert.ToDateTime(l.EndTime).ToString("hh:mm tt"),
                             Description = l.Description,
                             Time = (Convert.ToDateTime(l.EndTime) - Convert.ToDateTime(l.StartTime)).ToString(@"hh\:mm")
                         });
            return query.AsQueryable();
        }


        #endregion
        public void AddNotifyLetter(NotifyLetter notify)
        {
            context.NotifyLetters.Add(notify);
        }
        public void AttachNotifyLetter(NotifyLetter notify)
        {
            context.NotifyLetters.Attach(notify);
        }
        public DbEntityEntry<NotifyLetter> Entry(NotifyLetter notify)
        {
            return Context.Entry(notify);
        }
        public void Remove(NotifyLetter notify)
        {
            if (Context.Entry(notify).State == EntityState.Detached)
            {
                context.NotifyLetters.Attach(notify);
            }
            context.NotifyLetters.Remove(notify);
        }
        public void Add(NotifyCondition notify)
        {
            context.NotifyConditions.Add(notify);
        }  
        public void Add(WebMobLog weblog)
        {
            context.WebMobLog.Add(weblog);
        }
        public void Add(Filter Filter)
        {
            context.Filters.Add(Filter);
        }
        public void Attach(Filter Filter)
        {
            context.Filters.Attach(Filter);
        }
        public void Attach(WebMobLog weblog)
        {
            context.WebMobLog.Attach(weblog);
        }
        public DbEntityEntry<WebMobLog> Entry(WebMobLog weblog)
        {
            return Context.Entry(weblog);
        }
        public DbEntityEntry<Filter> Entry(Filter filter)
        {
            return Context.Entry(filter);
        }
        public void Remove(Filter Filter)
        {
            if (Context.Entry(Filter).State == EntityState.Detached)
            {
                context.Filters.Attach(Filter);
            }
            context.Filters.Remove(Filter);
        }
        public void Remove(WebMobLog weblog)
        {
            if (Context.Entry(weblog).State == EntityState.Detached)
            {
                context.WebMobLog.Attach(weblog);
            }
            context.WebMobLog.Remove(weblog);
        }
        public void Attach(NotifyCondition Notify)
        {
            context.NotifyConditions.Attach(Notify);
        }
        public DbEntityEntry<NotifyCondition> Entry(NotifyCondition Notify)
        {
            return Context.Entry(Notify);
        }
        public void Remove(NotifyCondition Notify)
        {
            if (Context.Entry(Notify).State == EntityState.Detached)
            {
                context.NotifyConditions.Attach(Notify);
            }
            context.NotifyConditions.Remove(Notify);
        }
        public IEnumerable<NotifyColumnsViewModel> GetColumnList(string tableName, string ObjectName, byte version, string type, int companyId, string culture)
        {
            IEnumerable<NotifyColumnsViewModel> query = null;

            if (type == null)
                type = context.PageDiv.Where(p => p.CompanyId == companyId && p.ObjectName == ObjectName && p.Version == version).Select(p => p.DivType).FirstOrDefault();

            if (type == "Form")
            {
                query = (from s in context.SysColumns
                         where s.obj_name == tableName
                         join p in context.PageDiv on s.obj_name equals p.TableName
                         where p.CompanyId == companyId && p.ObjectName == ObjectName && p.DivType == "Form" && p.Version == version
                         join fc in context.FormsColumns on p.Id equals fc.Section.FieldSet.PageId
                         where s.column_name == fc.ColumnName || s.column_name == "CompanyId" || s.column_name == "EmpId"
                         select new NotifyColumnsViewModel
                         {
                             pageTitle = HrContext.GetColumnTitle(companyId, culture, p.ObjectName, version, p.Title),
                             pageType = p.DivType,
                             type = s.data_type,
                             objectName = p.ObjectName,
                             value = s.column_name,
                             text = HrContext.GetColumnTitle(p.CompanyId, culture, p.ObjectName, p.Version, s.column_name) ?? s.column_name,
                             id = s.column_name,
                             name = HrContext.GetColumnTitle(p.CompanyId, culture, p.ObjectName, p.Version, s.column_name) ?? s.column_name,
                         }).Distinct().ToList();
            }
            else if (type == "Grid")
            {
                query = (from s in context.SysColumns
                         where s.obj_name == tableName
                         join p in context.PageDiv on s.obj_name equals p.TableName
                         where p.CompanyId == companyId && p.ObjectName == ObjectName && p.DivType == "Grid" && p.Version == version
                         join gc in context.GridColumns on p.Id equals gc.GridId
                         where s.column_name == gc.ColumnName || s.column_name == "CompanyId" || s.column_name == "EmpId"
                         select new NotifyColumnsViewModel
                         {
                             pageTitle = HrContext.GetColumnTitle(companyId, culture, p.ObjectName, version, p.TableName),
                             pageType = type,
                             type = s.data_type,
                             objectName = p.ObjectName,
                             value = s.column_name,
                             text = HrContext.GetColumnTitle(p.CompanyId, culture, p.ObjectName, p.Version, s.column_name) ?? s.column_name,
                             id = s.column_name,
                             name = HrContext.GetColumnTitle(p.CompanyId, culture, p.ObjectName, p.Version, s.column_name) ?? s.column_name,
                         }).Distinct().ToList();
            }
            return query;
        }

        #region SchedualeTask
        public IList<NotifyCondition> ReadNotifications(int CompanyId)
        {
            Events[] events = { Events.IsDueIn, Events.IsDueTo, Events.WasDueThisAmountofTimeAgo };

            return context.NotifyConditions
                .Where(n => n.CompanyId == CompanyId && events.Contains(n.Event) && (n.AlertMeUntil == null || n.AlertMeUntil >= DateTime.Today))
                .ToList();
        }
        public IQueryable<NavBarItemVM> GetAllNotifications(string UserName, string Lan , int CompanyId)
        {
            return context.WebMobLog.Include(ww=>ww.Notificat).Where(a => a.SentToUser == UserName && a.CompanyId == CompanyId ).Select(c => new NavBarItemVM
            {
                Id = c.Id,
                From = c.Subject == null ? " " : c.Subject,
                Message = c.Message,
                PicUrl = c.Notificat.EmpId != null ? c.Notificat.EmpId + ".jpeg" : "noimage.jpg",
                Read = c.MarkAsRead,
                SentDate=c.SentTime
                
            }).OrderByDescending(a => a.SentDate);
        }
        public IQueryable<NotificationViewModel> GetCompanyNotifications(string UserName, int CompanyId)
        {
            return context.WebMobLog.Where(a => a.SentToUser == UserName).Select(a => new NotificationViewModel
            {
                //CompanyId = a.CompanyId,
                //ConditionId = a.ConditionId,
                CreationTime = a.SentTime,
                Id = a.Id,
                SentToUser = UserName,
                Message = a.Message,
                ReadOK = a.MarkAsRead,
                Subject = a.Subject
            });
        }
        public string IncodeMessage( string msg)
        {

          
            return "";
        }
        public NavBarItemVM GetNotify(int Id, string culture, int CompanyId, string UserName)
        {

            var weblog = context.WebMobLog.Include(w=>w.Notificat).Where(a => a.Id == Id && a.CompanyId == CompanyId && a.SentToUser == UserName).FirstOrDefault();
            bool Read = true;

            if (weblog != null)
            {
                if (!weblog.MarkAsRead)
                {
                    Read = false;
                    weblog.MarkAsRead = true;
                    Attach(weblog);
                    context.Entry(weblog).State = EntityState.Modified;
                    context.SaveChanges();
                }

                return new NavBarItemVM()
                {
                    From = weblog.Subject,
                    Read = Read,
                    Id = weblog.Id,
                    Message = weblog.Message,
                    PicUrl = weblog.Notificat.EmpId == null ? "noimage.jpg" : weblog.Notificat.EmpId + ".jpeg",
                    SentDate=weblog.SentTime
                };
            }
            else
                return new NavBarItemVM()
                {
                    From = "",
                    Read = Read,
                    Id = 0,
                    Message = "",
                    PicUrl = "noimage.jpg",
                };

        }
        public List<WebMobLog> GetWebMobLog(int Id)
        {
            return context.WebMobLog.Where(a => a.NotificatId == Id).ToList();
        }
        public IQueryable<SchedualeTaskViewModel> ReadSchedualeTasks(int CompanyId)
        {
            return (from s in context.SchedualTasks
                  //  where s.CompanyId == CompanyId
                    select new SchedualeTaskViewModel
                    {
                        Enabled = s.Enabled,
                        Id = s.EventId,
                        EventName = s.EventName,
                        EventUrl = s.EventUrl,
                        StopOnError = s.StopOnError,
                        LastEndDate = s.LastEndDate,
                        LastStartDate = s.LastStartDate,
                        LastSuccessDate = s.LastSuccessDate,
                        ModifiedTime = s.ModifiedTime,
                        CompanyId = s.CompanyId,
                        ModifiedUser = s.ModifiedUser,
                        PeriodInMinutes = s.PeriodInMinutes
                    });
        }
        public void Add(SchedualTask ScheduTask)
        {
            context.SchedualTasks.Add(ScheduTask);
        }

        public void Attach(SchedualTask ScheduTask)
        {
            context.SchedualTasks.Attach(ScheduTask);
        }
        public DbEntityEntry<SchedualTask> Entry(SchedualTask ScheduTask)
        {
            return Context.Entry(ScheduTask);
        }
        public void Remove(SchedualTask ScheduTask)
        {
            if (Context.Entry(ScheduTask).State == EntityState.Detached)
            {
                context.SchedualTasks.Attach(ScheduTask);
            }
            context.SchedualTasks.Remove(ScheduTask);
        }
        #endregion
        public IList<NotifyCondition> GetNotifications(int companyId, string objectname, int version)
        {
            return context.NotifyConditions
                .Where(a => a.CompanyId == companyId && a.ObjectName == objectname && a.Version == version && (a.AlertMeUntil.Value >= DateTime.Today || a.AlertMeUntil == null)).ToList();
        }

        #region SMS Log && Email Log
        public IQueryable<SmsLogViewModel> ReadSMSLogs(int CompanyId)
        {
            var log = from l in context.SmsLog
                      where l.Notificat.CompanyId == CompanyId
                      join n in context.Notification on l.NotificatId equals n.Id
                      select new SmsLogViewModel
                      {
                          Id = l.Id,
                          Error = l.Error,
                          SentOk = l.SentOk,
                          SentToUser = l.SentToUser,
                          SentTime = l.SentTime,
                          NotificatId = n.Id,
                          CreationTime = n.CreationTime,
                          Message = n.Message,
                          Subject = n.Subject
                      };

            return log;
        }
        public IQueryable<EmailLogViewModel> ReadEmailLogs()
        {
            var log = from l in context.EmailLog
                      join n in context.Notification on l.NotificatId equals n.Id
                      select new EmailLogViewModel
                      {
                          Id = l.Id,
                          Error = l.Error,
                          SentOk = l.SentOk,
                          SentToUser = l.SentToUser,
                          SentTime = l.SentTime,
                          NotificatId = n.Id,
                          CreationTime = n.CreationTime,
                          Message = n.Message,
                          Subject = n.Subject
                      };

            return log;
        }
        #endregion

    }
}
