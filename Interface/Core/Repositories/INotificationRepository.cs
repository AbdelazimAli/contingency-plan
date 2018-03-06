using Model.Domain;
using Model.Domain.Notifications;
using Model.ViewModel;
using Model.ViewModel.Notification;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
       
       List<WebMobLog> GetWebMobLog(int Id);
        void AddNotifyLetter(NotifyLetter notify);
        void AttachNotifyLetter(NotifyLetter notify);
        void Remove(NotifyLetter notify);
        void Attach(NotifyLetter notify);
        DbEntityEntry<NotifyLetter> Entry(NotifyLetter notify);
        IList<NotifyCondition> GetNotifyConditions(int companyId);
        void AddDeleteNotification(int id, string tablename, DbEntityEntry entity);
        void AddUpdateNotification(int id, string tablename, string columname, DbEntityEntry entity);
        void AddInsertNotification(int id, string tablename, DbEntityEntry entity);
        void Add(NotifyCondition notify);
        #region MsgTemplate
        //IQueryable<MsgTemplateViewModel> ReadMessageTemp(int companyId, string culture);
        //MsgTemplateViewModel ReadFormMsgTemplate(int id, string culture, int companyId);
        //void Add(MsgTemplang msglang);
        //void Attach(MsgTemplang msglang);
        //void Remove(MsgTemplang msglang);
        //DbEntityEntry<MsgTemplang> Entry(MsgTemplang msglang);

        #endregion

        #region Email Account
        IQueryable<EmailAccountViewModel> ReadEmailAccount();
        EmailAccountFormViewModel ReadFormEmailAccount(int id);
        void Add(EmailAccount email);
        void Add(EmailLog log);
        void Add(SmsLog log);
        void Attach(EmailAccount email);
        void Remove(EmailAccount email);
        DbEntityEntry<EmailAccount> Entry(EmailAccount email);
        EmailAccount GetEmailAccount(int order);

        #endregion

        #region SchedualTask
        NotifyCondition ReadNotifyCondition(int? CondId);
        IQueryable<SchedualeTaskViewModel> ReadSchedualeTasks(int CompanyId);
        void Attach(SchedualTask ScheduTask);
        DbEntityEntry<SchedualTask> Entry(SchedualTask ScheduTask);
        void Remove(SchedualTask ScheduTask);
        NavBarItemVM GetNotify(int Id,string culture,int CompanyId,string Name);
        IQueryable<NavBarItemVM> GetAllNotifications(string UserName, string Lan, int CompanyId);
        IQueryable<NotificationViewModel> GetCompanyNotifications(string UserName,int CompanyId);
        IList<NotifyCondition> ReadNotifications(int CompanyId);
        #endregion

        IQueryable<NotifiyLetterViewModel> GetEmpLetters(int CompanyId, string Language);
        IQueryable<NotifiyLetterViewModel> GetMyLetters(int CompanyId, string Language, int EmpId);
        //condition
        IEnumerable<NotifyColumnsViewModel> GetColumnList(string tableName, string ObjectName, byte version, string type, int companyId, string culture);
        IQueryable<FilterGridViewModel> ReadCondition(int notifyCondId, string culture);
        void Add(Filter Filter);
        void Attach(Filter Filter);
        DbEntityEntry<Filter> Entry(Filter Filter);
        void Remove(Filter Filter);
        void Add(WebMobLog webLog);
        void Attach(WebMobLog webLog);
        DbEntityEntry<WebMobLog> Entry(WebMobLog webLog);
        void Remove(WebMobLog webLog);
        IQueryable<NotifyConditionIndexViewModel> ReadNotificationConditions(int companyId);
        NotifyConditionViewModel ReadNotificationCondition(int id);
        IList<NotifyCondition> GetNotifications(int companyId, string objectname, int version);
        void Attach(NotifyCondition Notify);
        DbEntityEntry<NotifyCondition> Entry(NotifyCondition Notify);
        void Remove(NotifyCondition Notify);
        IQueryable<SmsLogViewModel> ReadSMSLogs(int CompanyId);
        IQueryable<EmailLogViewModel> ReadEmailLogs();

    }
}
