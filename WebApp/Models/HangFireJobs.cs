using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using Model.ViewModel;
//using Hangfire;
using Model.Domain.Notifications;
using System.Linq;
using System;
using Db.Persistence.Services;
using Db.Persistence;
using Interface.Core.Repositories;
using WebApp.Models;
using System.Net;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Hangfire;
using Model.ViewModel.Notification;
using WebApp.Controllers;
using Model.ViewModel.Personnel;
using System.Net.Mail;
using Model.Domain;
using Db.Persistence.BLL;
using Interface.Core;

namespace WebApp
{
    public class HangFireJobs
    {
        public HangFireJobs()
        {

        }

        // List<string> names

        public class UserView
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public bool SmsNotify { get; set; } = false;
            public bool EmailNotify { get; set; } = false;
            public bool WebNotify { get; set; } = false;
        }

        public static IList<UserView> GetUsers(string user, int[] employees)
        {
            var users = user == null ? new string[] { } : user.Split(',');

            UserContext db = new UserContext();
            var query = db.Users.Where(a => employees.Contains(a.EmpId.Value) || users.Contains(a.UserName)).Select(a => new UserView { Name = a.UserName, Email = a.Email, PhoneNumber = a.PhoneNumber, SmsNotify = a.SmsNotify, EmailNotify = a.EmailNotify, WebNotify = a.WebNotify });
            return query.ToList();
        }

        private static void SendEmails(INotificationRepository repo, IList<UserView> users, Notification notify, ref EmailAccount emailaccount, ref int counter, ref bool modified)

        {
            string error = null;
            bool emailAccountOk = true;

            if (emailaccount == null)
            {
                emailaccount = repo.GetEmailAccount(0);
                if (emailaccount == null)
                {
                    error = "Email account is not defined";
                    emailAccountOk = false;
                }
                else counter = emailaccount.TodayCount;
            }

            foreach (var user in users)
            {
                if (emailAccountOk)
                {
                    try
                    {
                        Db.Persistence.Services.EmailService.SendEmail(emailaccount, notify.Subject, notify.Message, user.Email, user.Name);
                        error = null;
                        modified = true;
                        counter++;
                        // check capacity
                        if (emailaccount.Capacity - counter <= 0)
                        {
                            emailaccount.TodayCount = emailaccount.Capacity;
                            emailaccount.LastSentDate = DateTime.Now;
                            repo.Attach(emailaccount);
                            repo.Entry(emailaccount).State = System.Data.Entity.EntityState.Modified;
                            modified = false;

                            emailaccount = repo.GetEmailAccount(emailaccount.SendOrder);
                            if (emailaccount == null)
                            {
                                error = "Email limit is exceed max capacity";
                                emailAccountOk = false;
                            }
                            else counter = emailaccount.TodayCount;
                        }
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }
                }

                repo.Add(new EmailLog
                {
                    Error = error,
                    NotificatId = notify.Id,
                    Notificat = notify.Id == 0 ? notify : null,
                    SentOk = error == null,
                    SentTime = DateTime.Now,
                    SentToUser = user.Name
                });
            }
        }

        private static void SendSms(INotificationRepository repo, IList<UserView> users, Notification notify)
        {
            string error = null;
            string username = "waleedhashem@gmail.com";
            string password = "md574";
            string msgsender = "4700000000"; // "DEMO374733";
            //string destinationaddr = "4560991000";

            // Create ViaNettSMS object with username and password
            ViaNettSMS s = new ViaNettSMS(username, password);
            // Declare Result object returned by the SendSMS function
            ViaNettSMS.Result result;

            foreach (var user in users)
            {
                try
                {
                    // Send SMS through HTTP API
                    result = s.sendSMS(msgsender, user.PhoneNumber, notify.Message);
                    // Show Send SMS response
                    if (result.Success)
                    {
                        error = null;
                    }
                    else
                    {
                        error = "Received error: " + result.ErrorCode + " " + result.ErrorMessage;
                    }
                    error = null;

                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }


                repo.Add(new SmsLog
                {
                    Error = error,
                    NotificatId = notify.Id,
                    Notificat = notify.Id == 0 ? notify : null,
                    SentOk = error == null,
                    SentTime = DateTime.Now,
                    SentToUser = user.Name
                });
            }
        }


        #region Send Notification
        public static void Append(List<string> names, NavBarItemVM msg)
        {
            IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            HubContext.Clients.Users(names).AppendMessage(msg);
        }
        private static NavBarItemVM SignalR(Notification notify, int? empId)
        {
            return new NavBarItemVM
            {
                Id = notify.Id,
                From = notify.Subject,
                Message = notify.Message,
                Read = false,
                PicUrl = empId == null ? "noimage.jpg" : empId + ".jpeg"
            };
        }
        public static void SendEmails(IList<UserView> users, Notification notify, string email = null)
        {
            if (email != null) users.Add(new UserView { EmailNotify = true, Email = email });
            HrUnitOfWork unitofwork = new HrUnitOfWork(new HrContextFactory(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString));
            bool modified = false;
            int counter = 0;
            EmailAccount emailaccount = null;

            if (users.Count() > 0)
            {
                SendEmails(unitofwork.NotificationRepository, users, notify, ref emailaccount, ref counter, ref modified);

                if (emailaccount != null && modified)
                {
                    emailaccount.LastSentDate = DateTime.Now;
                    emailaccount.TodayCount = counter;
                    unitofwork.NotificationRepository.Attach(emailaccount);
                    unitofwork.NotificationRepository.Entry(emailaccount).State = System.Data.Entity.EntityState.Modified;
                }

                unitofwork.SaveChanges();
            }
        }
        public static void ReadNotifications(int CompanyId, int empId, string lang)
        {
            HrUnitOfWork unitofwork = new HrUnitOfWork(new HrContextFactory(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString));
            var conditions = unitofwork.NotificationRepository.ReadNotifications(CompanyId);
            EmailAccount emailaccount = null;
            bool modified = false;
            int counter = 0;
            holidays = null;

            if (emailaccount != null && modified)
            {
                emailaccount.LastSentDate = DateTime.Now;
                emailaccount.TodayCount = counter;
                unitofwork.NotificationRepository.Attach(emailaccount);
                unitofwork.NotificationRepository.Entry(emailaccount).State = System.Data.Entity.EntityState.Modified;
            }

            List<AppendMsgViewModel> SignalRAppend = new List<AppendMsgViewModel>();
            foreach (var cond in conditions)
            {
                if (string.IsNullOrEmpty(cond.TableName) || string.IsNullOrEmpty(cond.ColumnName))
                    continue; // can't get required data

                if (string.IsNullOrEmpty(cond.Users) && string.IsNullOrEmpty(cond.CustEmail) && !cond.NotifyRef)
                    continue; // no one to sent

                string[] arr = { "Currencies", "SchedualTasks" };
                // prepare sql statement
                var sql = "SELECT " + cond.ColumnName + (Array.Exists(arr, f => f == cond.TableName) ? "" : ",Id") +
                (string.IsNullOrEmpty(cond.Fields) ? "" : "," + cond.Fields) +
                " FROM " + cond.TableName +
                " WHERE " + (cond.filter == null ? "" : (cond.filter.Replace("\"", "'") + " AND ")) +
                "CAST( " + cond.ColumnName + " as date )= '" + GetNotificationDate(unitofwork, CompanyId, cond.Event, cond.EventValue).ToString("yyyy-MM-dd") + "'";

                // dynamic create datatable
                var table = GetData(sql);
                if (table.Rows.Count == 0) continue; // condition doesn't match records

                var employees = new List<int>();
                if (cond.NotifyRef)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row.Table.Columns.Contains("EmpId"))
                            if (row["EmpId"] != null && row["EmpId"].ToString().Length > 0)
                                employees.Add(int.Parse(row["EmpId"].ToString()));
                    }
                }

                IList<UserView> users = GetUsers(cond.Users, employees.ToArray<int>());
                if (users == null) // no users to send to
                    continue;

                // create notification for each record
                foreach (DataRow row in table.Rows)
                {
                    string m = MessageMerge(cond.Message, unitofwork, table, row, cond.TableName, lang);
                    var notify = new Notification
                    {
                        CompanyId = CompanyId,
                        ConditionId = cond.Id,
                        CreationTime = DateTime.Now,
                        EmpId = GetEmpId(row),
                        RefEmpId = GetEmpId(row),
                        Message = MessageMerge(cond.Message, unitofwork, table, row, cond.TableName, lang),
                        Subject = cond.Subject,
                        SourceId = GetId(row)
                    };

                    foreach (var u in users.Where(a => a.WebNotify))
                    {
                        WebMobLog log = new WebMobLog
                        {

                            MarkAsRead = false,
                            Message = notify.Message,
                            Notificat = notify,
                            SentTime = DateTime.Now,
                            SentToUser = u.Name,
                            CompanyId = CompanyId,
                            Subject = notify.Subject
                        };
                        unitofwork.NotificationRepository.Add(log);


                    }
                    SignalRAppend.Add(new AppendMsgViewModel
                    {
                        User = users.Where(a => a.WebNotify).Select(a => a.Name).ToList(),
                        Notify = notify
                    });
                    var EmailUsers = users.Where(a => a.EmailNotify).ToList();
                    if (EmailUsers.Count > 0) SendEmails(unitofwork.NotificationRepository, EmailUsers, notify, ref emailaccount, ref counter, ref modified);
                    if (cond.Sms) SendSms(unitofwork.NotificationRepository, users.Where(a => a.SmsNotify).ToList(), notify);
                }
            }

            unitofwork.SaveChanges();
            foreach (var item in SignalRAppend)
            {
                Append(item.User, SignalR(item.Notify, item.Notify.EmpId));
            }


        }
        public static void SendNotication(Notification notify, NotifyCondition cond, INotificationRepository context)
        {
            if (!string.IsNullOrEmpty(cond.Users) || notify.RefEmpId != null)
            {

                int[] employees = notify.RefEmpId == null ? new int[] { } : new int[] { notify.RefEmpId.Value };
                IList<UserView> users = GetUsers(cond.Users, employees);

                var navbar = SignalR(notify, notify.EmpId);
                BackgroundJob.Enqueue(() => Append(users.Where(a => a.WebNotify).Select(a => a.Name).ToList(), navbar));
                BackgroundJob.Enqueue(() => SendEmails(users.Where(a => a.EmailNotify).ToList(), notify, cond.CustEmail));
                //  if (cond.Sms) BackgroundJob.Enqueue(() => SendSms(context, users.Where(a => a.SmsNotify).ToList(), notify));
            }
        }
        #endregion

        #region ExtendContract

        public static bool ExtendContractMethod(IHrUnitOfWork unitofwork, string Language, out string ErrorMessage)
        {
            bool Result = false;
            ErrorMessage = "";
            try
            {
                // Get All employees The will Send Email to whose contract Finish and before No of days
                var Employments = unitofwork.EmployeeRepository.SendMailEmployees();

                List<NotifyLetter> NotifyLettersList = new List<NotifyLetter>();
                foreach (var item in Employments)
                {
                    string NotifySource = "", Description = "";
                    if (item.Renew)
                    {
                        NotifySource = Constants.Sources.RenewContract;
                        Description = MsgUtils.Instance.Trls("Contract has been renewed", Language);
                    }
                    else
                    {
                        NotifySource = Constants.Sources.ContractFinish;
                        Description = MsgUtils.Instance.Trls("Contract has been finished", Language);
                    }

                    bool IsSentBefore = unitofwork.NotifyLetterRepository.IsNotificationSent(item.EmpId, DateTime.Today.Date, NotifySource);

                    if (!IsSentBefore)
                    {

                        NotifyLetter NL = new NotifyLetter()
                        {
                            CompanyId = item.CompanyId,
                            EmpId = item.EmpId,
                            NotifyDate = DateTime.Today,
                            NotifySource = NotifySource,
                            SourceId = item.Id.ToString(),
                            Sent = false,
                            EventDate = item.EndDate,
                            Description = Description
                        };

                        NotifyLettersList.Add(NL);
                        //unitofwork.NotifyLetterRepository.Add(NL);
                    }
                }
                string DefaultErrorMessage = MsgUtils.Instance.Trls("NotifyLetterNotSent",Language);
                AddNotifyLetters AddNotifyLetters = new AddNotifyLetters(unitofwork, NotifyLettersList, Language);
                Result = AddNotifyLetters.Run(out ErrorMessage, DefaultErrorMessage);
            }
            catch
            {
            }
            return Result;
        }
        public static void ExtendContract(string Language)
        {
            HrUnitOfWork unitofwork = new HrUnitOfWork(new HrContextFactory(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString));
            string ErrorMessage;
            ExtendContractMethod(unitofwork, Language, out ErrorMessage);
            //unitofwork.Save();


            //// Get First Email Account
            //EmailAccount emailAcc = unitofwork.Repository<EmailAccount>().FirstOrDefault();

            //// Get All employees The will Send Email to whose contract Finish and before No of days
            //var Employments = unitofwork.EmployeeRepository.SendMailEmployees();

            //// Get an instance of Letter Controller which has the mailmerge Functin
            //LettersController LetterController = new LettersController(unitofwork);

            //// Instance of mailmergeviewmodel that will get result of function mailmerge
            //MailMergeViewmodel Temp = new MailMergeViewmodel();
            //string Send = "";

            //// Translation of subject and body of mail
            //string Subject = MsgUtils.Instance.Trls("ContractFinish", Language);
            //string Body = MsgUtils.Instance.Trls("YourContractFinish", Language);

            //foreach (var item in Employments)
            //{

            //    if (item.Email != null)
            //    {
            //        if (item.Renew)  // the employees whose contract will Renewed
            //            Temp = LetterController.MergeData("RenewContract", item.Id, item.EmpId, Language);

            //        else    // the employees whose contract will finished
            //            Temp = LetterController.MergeData("ContractFinish", item.Id, item.EmpId, Language);

            //        if (Temp.Exist) // if there is File or no error of mail merge
            //        {
            //            // The Attachement file that will send to employee
            //            Attachment Attach = new Attachment(Temp.ServerFilePath);

            //            //Send email Function
            //            Send = Db.Persistence.Services.EmailService.SendEmail(emailAcc,item.Renew? MsgUtils.Instance.Trls("ContractRenew", Language):Subject,item.Renew ? MsgUtils.Instance.Trls("YourContractRenewed", Language):Body  , item.Email, "", null, Attach);
            //            try
            //            {
            //                if (Send == "Ok")
            //                {
            //                    if (emailAcc.TodayCount <= emailAcc.Capacity) // if count of mails sent today less than the capacity of mailacount capacity
            //                    {
            //                        emailAcc.TodayCount = emailAcc.TodayCount + 1; // increase the mails sent today +1
            //                        emailAcc.LastSentDate = DateTime.Now;
            //                        unitofwork.NotificationRepository.Attach(emailAcc);
            //                        unitofwork.NotificationRepository.Entry(emailAcc).State = System.Data.Entity.EntityState.Modified;
            //                    }
            //                    else  // Change the email Account
            //                        emailAcc = unitofwork.NotificationRepository.GetEmailAccount(emailAcc.SendOrder);

            //                }

            //                // Create Notification if the email sent to employee or not to sent it later if not sent
            //                NotifyLetter NL = new NotifyLetter()
            //                {
            //                    CompanyId = item.CompanyId,
            //                    EmpId = item.EmpId,
            //                    NotifyDate = DateTime.Today,
            //                    NotifySource = item.Renew ? "RenewContract" : "ContractFinish",
            //                    SourceId = item.Id.ToString(),
            //                    Sent = Send == "Ok" ? true : false,  
            //                    EventDate = item.EndDate
            //                };


            //                unitofwork.NotificationRepository.AddNotifyLetter(NL);
            //                unitofwork.Save();

            //                // Add the employee file to Documents 
            //                unitofwork.CompanyRepository.Add(new CompanyDocsViews()
            //                {
            //                    CompanyId = item.CompanyId,
            //                    name = Temp.Path.Substring(Temp.Path.LastIndexOf("/") + 1),
            //                    Source = "NotifyLetter",
            //                    SourceId = NL.Id,
            //                    file_stream = System.IO.File.ReadAllBytes(Temp.ServerFilePath),
            //                    thumbs = null,

            //                });
            //                unitofwork.Save();
            //            }
            //            catch (Exception ex)
            //            {
            //                unitofwork.HandleDbExceptions(ex);
            //            }
            //            finally
            //            {
            //                Attach.Dispose();
            //            }
            //        }

            //    }
            //}

        }
        #endregion

        #region Business Date
        private class Holidays
        {
            public bool Standard { get; set; }
            public byte? SMonth { get; set; }
            public byte? SDays { get; set; }
            public DateTime? HoliDate { get; set; }
        }

        private static IList<Holidays> holidays = null;

        private static bool IsHoliday(Model.Domain.PersonSetup setup, DateTime businessdate)
        {
            var day = holidays.Where(a => (a.Standard && businessdate.Day == a.SDays.Value && businessdate.Month == a.SMonth.Value) ||
                                (!a.Standard && a.HoliDate == businessdate)).FirstOrDefault();

            if (day != null) return true;

            if ((int)businessdate.DayOfWeek == setup.Weekend1.Value || (int)businessdate.DayOfWeek == setup.Weekend2.Value)
                return true;

            return false;
        }

        /// <summary>
        /// shift business days + or - to specific no of days
        /// </summary>
        /// <param name="businessdate"></param>
        /// <param name="days"></param>
        /// <param name="sign"></param>
        /// <returns>new business date</returns>
        private static DateTime GetNotificationDate(HrUnitOfWork unitofwork, int company, Events e, string value)
        {
            DateTime businessdate = DateTime.Today;
            if (e == Events.IsDueIn) return businessdate;

            int temp = 0;
            int sign = e == Events.WasDueThisAmountofTimeAgo ? -1 : 1;

            if (!string.IsNullOrEmpty(value)) int.TryParse(value, out temp);
            NotifyDays days = (NotifyDays)temp;

            if (days == 0 || e == Events.IsDueIn) return businessdate;
            var setup = unitofwork.CompanyRepository.GetPersonSetup(company);

            if (holidays == null)
            {
                holidays = unitofwork.Repository<Model.Domain.Holiday>()
                   .Where(a => a.CompanyId == company || !a.IsLocal)
                   .Select(a => new Holidays { HoliDate = a.HoliDate, SDays = a.SDay, SMonth = a.SMonth, Standard = a.Standard })
                   .ToList();
            }


            if (days <= NotifyDays._13CalendarDays)
            {
                int i = 1;
                while (i <= (int)days)
                {
                    businessdate = businessdate.AddDays(i * sign);
                    if (!IsHoliday(setup, businessdate)) i++;
                }
            }
            else if (days == NotifyDays._2Weeks)
                businessdate = businessdate.AddDays(14 * sign);
            else if (days == NotifyDays._3Weeks)
                businessdate = businessdate.AddDays(21 * sign);
            else if (days == NotifyDays._1Month)
                businessdate = businessdate.AddMonths(sign);
            else if (days == NotifyDays._2Months)
                businessdate = businessdate.AddMonths(2 * sign);
            else if (days == NotifyDays._3Months)
                businessdate = businessdate.AddMonths(3 * sign);
            else if (days == NotifyDays._4Months)
                businessdate = businessdate.AddMonths(4 * sign);

            return businessdate;
        }
        #endregion

        #region Message Data
        private static DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(sql, conn));
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    var x = ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    conn.Dispose();
                }
            }

            return dt;
        }
        private static int? GetEmpId(DataRow row)
        {
            int empId = 0;
            if (row.Table.Columns.Contains("EmpId"))
                if (row["EmpId"] != null)
                    int.TryParse(row["EmpId"].ToString(), out empId);

            return empId == 0 ? null : (int?)empId;
        }
        private static string GetId(DataRow row)
        {
            if (row.Table.Columns.Contains("Id"))
                return row["Id"].ToString();
            else
                return "";
        }
        private static string MessageMerge(string message, HrUnitOfWork unitofwork, DataTable table, DataRow row, string tablename, string lang)
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
                        GetColumnValue(message.Substring(index + start + 1, end), unitofwork, table, row, tablename, lang));

                    index = index + start + end + 2;
                    start = message.Substring(index).IndexOf("%"); // {
                }
                else break;
            }

            result.Append(message.Substring(index, message.Length - index));

            return result.ToString();
        }
        private static string GetColumnValue(string columname, HrUnitOfWork unitofwork, DataTable table, DataRow row, string tablename, string lang)
        {
            if (row[columname] == null) return " .. ";
            var datatype = table.Columns[columname].DataType;
            string value = row[columname].ToString();

            if (datatype == typeof(int) || datatype == typeof(short) || datatype == typeof(byte))
            { // so this seems to be Id or code so resolve code
                value = unitofwork.Context.Database.SqlQuery<string>("exec dbo.sp_GetDisplayName '" + tablename + "', '" + columname + "', " + value + ", '" + lang + "'").FirstOrDefault();
            }

            return value;
        }
        #endregion
    }
}
