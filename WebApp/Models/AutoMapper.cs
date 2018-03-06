using Interface.Core;
using Model.Domain.Notifications;
using System;
using System.Security.Principal;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WebApp.Extensions;
using System.Data;
using Model.ViewModel.Notification;
namespace WebApp.Models
{
    public class AutoMapper
    {
        AutoMapperParm Parm;
        IHrUnitOfWork HrUnitOfWork;
        IList<NotifyCondition> Notifications;
        private static int ResolveId = 0;
        PropertyInfo[] SourceProperties, DestionationProperties;
        int company;
        IIdentity Identity;

        public AutoMapper(AutoMapperParm p, IHrUnitOfWork unitofwork, IIdentity identity)
        {
            Parm = p;
            HrUnitOfWork = unitofwork;
            SourceProperties = p.Source.GetType().GetProperties();
            DestionationProperties = p.Destination == null ? null :
                p.Destination.GetType().GetProperties();
            Identity = identity;
            company = Identity.GetDefaultCompany();

            // get all notificatios for current object 
            Notifications = HrUnitOfWork.NotificationRepository.GetNotifications(company, p.ObjectName, p.Version);
        }

        public void Map()
        {
            string source, dest;

            // get record Id: each record must have field Id & Get EmpId if exist
            int EmpId = 0;
            var property = SourceProperties.Where(s => s.Name == "EmpId").FirstOrDefault();
            object value = "";
            if (property != null)
            {
                value = property.GetValue(Parm.Source);
                int.TryParse(value?.ToString(), out EmpId);
            }

            string Id = "";
            property = SourceProperties.Where(s => s.Name == "Id").FirstOrDefault();
            if (property != null)
            {
                value = property.GetValue(Parm.Source);
                //if (value != null) int.TryParse(value.ToString(), out Id);
                if (value != null) Id = value.ToString();
            }

            // Delete
            if (Parm.Transtype == Model.Domain.TransType.Delete)
            {
                // log once with no details
                HrUnitOfWork.CompanyRepository.Add(new Model.Domain.AudiTrail
                {
                    ColumnName = "Id=" + value.ToString(),
                    CompanyId = company,
                    ModifiedTime = DateTime.Now,
                    ModifiedUser = Identity.Name,
                    ObjectName = Parm.ObjectName,
                    Transtype = (byte)Model.Domain.TransType.Delete,
                    SourceId = Id,
                    Version = Parm.Version
                });

                CheckNotifications(EmpId == 0 ? null : (int?)EmpId);
                return;
            }

            // Update & Insert
            if (Parm.Transtype == Model.Domain.TransType.Insert) ResolveId++;

            var commonproperties = Parm.Options == null || Parm.Options.VisibleColumns == null ? (from sp in SourceProperties
                                                                                                  join dp in DestionationProperties on new { p1 = sp.Name, p2 = sp.PropertyType.ToString() } equals
                                                                                                      new { p1 = dp.Name, p2 = dp.PropertyType.ToString() }
                                                                                                  select new { sp, dp }) :
                               (from sp in SourceProperties
                                where Parm.Options.VisibleColumns.Contains(sp.Name)
                                join dp in DestionationProperties on new { p1 = sp.Name, p2 = sp.PropertyType.ToString() } equals
                                    new { p1 = dp.Name, p2 = dp.PropertyType.ToString() }
                                select new { sp, dp });

            commonproperties = commonproperties.Union(from sp in SourceProperties.Where(a => a.PropertyType.ToString() == "System.Collections.Generic.IList`1[System.Int32]")
                                                      where Parm.Options.VisibleColumns.Contains(sp.Name)
                                                      join dp in DestionationProperties.Where(a => a.PropertyType.ToString() == "System.String") on sp.Name equals dp.Name
                                                      select new { sp, dp });

            foreach (var match in commonproperties)
            {
                var soureValue = match.sp.GetValue(Parm.Source, null);
                var destValue = match.dp.GetValue(Parm.Destination, null);

                CheckNotifications(EmpId == 0 ? null : (int?)EmpId, match.sp.Name, match.sp.PropertyType, soureValue, destValue, Id);

                if (Parm.ObjectName != null &&
                    match.sp.Name != "CreatedUser" &&
                    match.sp.Name != "CreatedTime" &&
                    match.sp.Name != "ModifiedTime" &&
                    match.sp.Name != "ModifiedUser" &&
                    (soureValue != null || destValue != null) &&
                    !Object.Equals(soureValue, destValue))
                {
                    var listcolumn = Parm.Options?.NewValues == null ? null : Parm.Options.NewValues.Where(a => a.ColumnName == match.sp.Name).FirstOrDefault();
                    source = listcolumn != null ? listcolumn.Text : soureValue == null ? null : soureValue.ToString();
                    listcolumn = Parm.Options?.OldValues == null ? null : Parm.Options.OldValues.Where(a => a.ColumnName == match.sp.Name).FirstOrDefault();
                    dest = listcolumn != null ? listcolumn.Text : destValue == null ? null : destValue.ToString();

                    HrUnitOfWork.CompanyRepository.Add(new Model.Domain.AudiTrail
                    {
                        ColumnName = match.sp.Name,
                        CompanyId = company,
                        ModifiedTime = DateTime.Now,
                        ModifiedUser = Identity.Name,
                        ObjectName = Parm.ObjectName,
                        SourceId = Id,
                        ValueBefore = CheckValue(Parm.Transtype == Model.Domain.TransType.Insert ? ResolveId.ToString() : dest),
                        ValueAfter = CheckValue(source),
                        Version = Parm.Version,
                        Transtype = (byte)Parm.Transtype
                    });
                }

                if (soureValue != null && match.sp.PropertyType.ToString() == "System.Collections.Generic.IList`1[System.Int32]" && match.dp.PropertyType.ToString() == "System.String")
                    match.dp.SetValue(Parm.Destination, string.Join(",", ((IList<int>)soureValue).ToArray()), null);
                else
                    match.dp.SetValue(Parm.Destination, soureValue, null);


                if (Parm.Transtype == Model.Domain.TransType.Insert)
                {
                    var p = DestionationProperties.Where(d => d.Name == "ModifiedUser").FirstOrDefault();
                    if (p != null) p.SetValue(Parm.Destination, ResolveId.ToString(), null);
                }
            }
        }

        private string CheckValue(string value)
        {
            if (value == null) return null;
            return value.Length <= 250 ? value : value.Substring(0, 250);
        }

        /// <summary>
        /// check all type of notifications events then add record to notification table
        /// </summary>
        /// <param name="columname">event happened in this column</param>
        /// <param name="propertyType">data type of the column</param>
        /// <param name="soureValue">data after changed</param>
        /// <param name="destValue">data before changed if exist</param>
        /// <param name="Id">pk of the modified record</param>
        private string CheckNotifications(int? EmpId, string columname = null, Type propertyType = null,
            object soureValue = null, object destValue = null, string Id = null)
        {
            string msg = "Ok";
            foreach (var notification in Notifications.Where(a => a.ColumnName == columname))
            {
                if (notification.filter != null && !MatchFilter(notification.filter, Parm.Source, out msg))
                    continue;

                // delete or insert events
                if ((columname == null) && (notification.Event == Events.RecordHassBeenCreated ||
                    notification.Event == Events.RecordHasBeenDeleted)) // insert or delete
                {
                    AddNotification(notification, Id, EmpId);
                    continue;
                }

                // date events
                if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                {
                    DateTime after = DateTime.MinValue, before = DateTime.MinValue, value = DateTime.MinValue;
                    if (soureValue != null) DateTime.TryParse(soureValue.ToString(), out after);
                    if (destValue != null) DateTime.TryParse(destValue.ToString(), out before);
                    if (!string.IsNullOrEmpty(notification.EventValue)) DateTime.TryParse(notification.EventValue, out value);

                    if (after != before) // value is changed
                    {
                        switch (notification.Event)
                        {
                            case Events.DateHasChanged:
                                if (after != before) AddNotification(notification, Id, EmpId);
                                break;
                            case Events.HasBeenPostponed: // اذا تم تأجيل التاريخ
                                if (after > before) AddNotification(notification, Id, EmpId);
                                break;
                            case Events.HasBeenPostponedUntilEarliest: // اذا تم تأجيل التاريخ بعد التاريخ المحدد
                                if (after > before && after > value) AddNotification(notification, Id, EmpId);
                                break;
                            case Events.IsSetToDate:
                                if (after == value) AddNotification(notification, Id, EmpId);
                                break;
                            case Events.IsSetToDateEarlierThan: // تم تقديم التاريخ قبل تاريخ محدد
                                if (after < before && after < value) AddNotification(notification, Id, EmpId);
                                break;
                            case Events.IsSetToEarlierDate: // تم تقديم التاريخ
                                if (after < before) AddNotification(notification, Id, EmpId);
                                break;
                            default:
                                break;
                        }
                    }
                    else if (after == value && notification.Event == Events.IsSetToDate)
                        AddNotification(notification, Id, EmpId);
                }

                //string events
                else if (propertyType == typeof(string))
                {
                    if (notification.Event == Events.StringHasChanged && destValue != null && soureValue?.ToString() != destValue.ToString())
                        AddNotification(notification, Id, EmpId);

                    if (notification.Event == Events.StringIsSetTo && soureValue != null && soureValue.ToString() == notification.EventValue)
                        AddNotification(notification, Id, EmpId);

                    continue;
                }
                else if (propertyType == typeof(Boolean)) //bool events
                {
                    bool after = false, before = false, value = false;
                    bool.TryParse(soureValue?.ToString(), out after);
                    bool.TryParse(destValue?.ToString(), out before);
                    bool.TryParse(notification.EventValue, out value);

                    if (notification.Event == Events.StringIsSetTo && after == value)
                        AddNotification(notification, Id, EmpId);
                    else if (notification.Event == Events.StringHasChanged && after != before)
                        AddNotification(notification, Id, EmpId);
                }

                // Number events
                else
                {
                    decimal after = 0, before = 0, value = 0;
                    decimal.TryParse(soureValue?.ToString(), out after);
                    decimal.TryParse(destValue?.ToString(), out before);
                    decimal.TryParse(notification.EventValue, out value);

                    if (notification.Event == Events.NumberIsSetTo && after == value)
                        AddNotification(notification, Id, EmpId);
                    else if (after != before)
                    {
                        switch (notification.Event)
                        {
                            case Events.HasDecreased:
                                if (after < before) AddNotification(notification, Id, EmpId);
                                break;
                            case Events.HasDecreasedBelow:
                                if (after < before && after < value) AddNotification(notification, Id, EmpId);
                                break;
                            case Events.HasIncreased:
                                if (after > before) AddNotification(notification, Id, EmpId);
                                break;
                            case Events.HasIncreasedAbove:
                                if (after > before && after > value) AddNotification(notification, Id, EmpId);
                                break;
                            case Events.NumberHasChanged: // or list
                                if (after != before) AddNotification(notification, Id, EmpId);
                                break;
                            default:
                                break;
                        }
                    }

                }
            }

            return msg;
        }

        public bool MatchFilter(string filter, object model, out string message)
        {
            bool result = false;
            message = null;

            //Get used columns only
            var filterList = filter.Split(' ').Where(a => a != "");
            StringBuilder newfilter = new StringBuilder();
            string[] array = { ">=", ">", "<=", "<", "=", "or", "Or", "OR", "and", "And", "AND", "(", ")" };
            foreach (var f in filterList)
            {
                PropertyInfo s = !array.Contains(f) ? SourceProperties.Where(a => a.Name == f).FirstOrDefault() : null;
                if (s != null && s.GetValue(model) != null)
                    newfilter.Append(s.GetValue(model).GetType() != typeof(DateTime) ? "'" + s.GetValue(model).ToString() + "' " : "'" + DateTime.Parse(s.GetValue(model).ToString()).ToString("yyyy-MM-dd") + "' ");
                else
                    newfilter.Append(f + " ");
            }

            newfilter.Replace('"', '\'');

            //Evaluate
            DataTable dt = new DataTable();
            try
            {
                result = (bool)dt.Compute(newfilter.ToString(), "");
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return result;
        }

        Notification AddNotification(NotifyCondition condition, string Id, int? EmpId)
        {
            var notification = new Notification
            {
                CompanyId = company,
                ConditionId = condition.Id,
                CreationTime = DateTime.Now,
                SourceId = Id,
                Message = MessageMerge(condition.EncodedMsg.Replace("&nbsp;", " ").Replace("&lt;", "<").Replace("&gt;", ">")),
                Subject = condition.Subject,
                EmpId = Identity.GetEmpId(),
                RefEmpId = EmpId
            };

            int[] employees = notification.RefEmpId == null ? new int[] { } : new int[] { notification.RefEmpId.Value };
            IList<HangFireJobs.UserView> users = HangFireJobs.GetUsers(condition.Users, employees);
            foreach (var u in users.Where(a => a.WebNotify))
            {
                HrUnitOfWork.NotificationRepository.Add(new WebMobLog
                {
                    MarkAsRead = false,
                    Message = notification.Message,
                    Notificat = notification,
                    SentTime = DateTime.Now,
                    SentToUser = u.Name,
                    Subject = notification.Subject,
                    CompanyId = company
                });
            }
            HrUnitOfWork.NotificationRepository.Add(notification);

            // Send it now
            HangFireJobs.SendNotication(notification, condition, HrUnitOfWork.NotificationRepository);

            return notification;
        }
      

        string GetNewValue(string columname)
        {
            var listcolumn = Parm.Options == null || Parm.Options.NewValues == null ? null : Parm.Options.NewValues.Where(a => a.ColumnName == columname).FirstOrDefault();
            string value = "";

            if (listcolumn != null)
                value = listcolumn.Text;
            else
            {
                var column = SourceProperties.Where(s => s.Name == columname).FirstOrDefault();
                if (column != null)
                    value = column.GetValue(Parm.Source)?.ToString() ?? "";
            }

            return value;
        }

        string GetOldValue(string columname)
        {
            if (DestionationProperties == null || Parm.Destination == null)
                return "";

            var listcolumn = Parm.Options.OldValues == null ? null : Parm.Options.OldValues.Where(a => a.ColumnName == columname).FirstOrDefault();
            string value = "";

            if (listcolumn != null)
                value = listcolumn.Text;
            else
            {
                var column = DestionationProperties.Where(s => s.Name == columname).FirstOrDefault();
                if (column != null)
                    value = column.GetValue(Parm.Destination).ToString();
            }

            return value;
        }

        string GetColumnValue(string columname)
        {
            if (columname.Contains(".old"))
                return GetOldValue(columname.Substring(0, columname.Length - 4));
            else
                return GetNewValue(columname);
        }

        string MessageMerge(string message)
        {
            if (message == null) return "";
            int start = message.IndexOf("%"), end; // {
            StringBuilder result = new StringBuilder();
            int index = 0;

            while (start >= 0)
            {
                end = message.Substring(index + start + 1).IndexOf("%"); // }
                if (end > 0)
                {
                    result.Append(message.Substring(index, start) +
                        GetColumnValue(message.Substring(index + start + 1, end)));

                    index = index + start + end + 2;
                    start = message.Substring(index).IndexOf("%"); // {
                }
                else break;
            }

            result.Append(message.Substring(index, message.Length - index));

            return result.ToString();
        }
    }
}