using Db.Persistence.Repositories;
using Db.Persistence.Services;
using Interface.Core;
using Interface.Core.Repositories;
using System.Data.Entity;
using System;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Collections.Generic;
using Model.ViewModel;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Common;
using System.IO;
using Interface.Core.Repositories.Loan;
using Db.Persistence.Repositories.Loans;

namespace Db.Persistence
{
   
    public sealed partial class HrUnitOfWork : UnitOfWork<DbContext>, IHrUnitOfWork
    {
        public HrUnitOfWork(IContextFactory<DbContext> contextFactory) // , int companyId, string culture
            : base(contextFactory)
        {
            // Initialize
            CompanyRepository = new CompanyRepository(Context); // , companyId, culture
            PagesRepository = new PagesRepository(Context);
            MenuRepository = new MenuRepository(Context);
            PageEditorRepository = new PageEditorRepository(Context);
            LookUpRepository = new LookUpRepoitory(Context);
            CompanyStructureRepository = new CompanyStructureRepository(Context);
            JobRepository = new JobRepository(Context);
            PeopleRepository = new PeopleRepository(Context);
            PositionRepository = new PositionRepository(Context);
            BudgetRepository = new BudgetRepository(Context);
            QualificationRepository = new QualificationRepository(Context);
            LeaveRepository = new LeaveRepository(Context);
            EmployeeRepository = new EmployeeRepository(Context);
            CustodyRepository = new CustodyRepository(Context);
            TrainingRepository = new TrainingRepository(Context);
            BenefitsRepository = new BenefitsRepository(Context);
            AudiTrialRepository = new AudiTrialRepository(Context);
            TerminationRepository = new TerminationRepository(Context);
            DisciplineRepository = new DisciplineRepository(Context);
            CheckListRepository = new CheckListRepository(Context);
            ComplaintRepository = new ComplaintRepository(Context);
            MessageRepository = new MessageRepository(Context);
            MedicalRepository = new MedicalRepository(Context);
            HrLettersRepository = new HRLettersRepository(Context);
            PayrollRepository = new PayrollRepository(Context);
            SalaryDesignRepository = new SalaryDesignRepository(Context);
            NotificationRepository = new NotificationRepository(Context);
            MissionRepository = new MissionRepository(Context);
            MeetingRepository = new MeetingRepository(Context);
        }
        
        public string HandleDbExceptions(Exception ex, string culture)
        {
            var message = "";
            if (ex is DbEntityValidationException)
            {
                var e = (DbEntityValidationException)ex;
                foreach (var errors in e.EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                    {
                        message += error.ErrorMessage;
                    }
                }
            }
            else if (ex.InnerException.InnerException is SqlException)
            {
                var e = (SqlException)ex.InnerException.InnerException;
                if (e.Number == 2601)
                    message = MsgUtils.Instance.Trls(culture, "AlreadyExists");
                else if (e.Number == 547)
                {
                    message = MsgUtils.Instance.Trls(culture, "DeleteRelatedInformation");
                    var index = e.Message.IndexOf("table");
                    if (index >= 0)
                    {
                        //var table = System.Text.RegularExpressions.Regex.Matches(e.Message.Substring(index), "\"(.+?)\"");
                        index = index + e.Message.Substring(index).IndexOf('"') + 1;
                        var length = e.Message.Substring(index).IndexOf('"');
                        var table = e.Message.Substring(index, length).Replace("dbo.", "");
                        if (table.Length > 0) message += ": " + CompanyRepository.TrlsTable(culture, table);
                    }
                }
                else
                    message += e.Message;
            }
            else
            {
                message += ex.InnerException.Message;
            }

            return message;
        }

        //private void AddNotifications()
        //{
        //    var conditions = NotificationRepository.GetNotifyConditions(0);
        //    var entries = GetTableNames(Context.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged));

        //    // Event 1-Insert 2-Update 3-Delete
        //    // handle deleted records
        //    var deleted = from cond in conditions
        //                  where cond.Event == 3
        //                  join e in entries.Where(e => e.Entity.State == EntityState.Deleted) on cond.TableName equals e.TableName
        //                  select new { cond.Id, cond.TableName, e.Entity};
                          
        //    //var list = from e in entries where e.State == EntityState.Deleted
        //    //           join d in deleted on e.Entity.
        //    foreach (var record in deleted)
        //        NotificationRepository.AddDeleteNotification(record.Id, record.TableName, record.Entity);

        //    // handle inserted records
        //    var inserted = from cond in conditions
        //                  where cond.Event == 1
        //                  join e in entries.Where(e => e.Entity.State == EntityState.Added) on cond.TableName equals e.TableName
        //                  select new { cond.Id, cond.TableName, e.Entity };
        //    foreach (var record in inserted)
        //        NotificationRepository.AddInsertNotification(record.Id, record.TableName, record.Entity);

        //    //handle update records
        //    var updated = from cond in conditions
        //                  where cond.Event == 2
        //                  join e in entries.Where(e => e.Entity.State == EntityState.Modified) on cond.TableName equals e.TableName
        //                  where e.Entity.CurrentValues.PropertyNames.Contains(cond.ColumnName)
        //                  select new { cond.Id, cond.TableName, cond.ColumnName, e.Entity };
        //    foreach (var record in updated)
        //        NotificationRepository.AddUpdateNotification(record.Id, record.TableName, record.ColumnName, record.Entity);
        //}

        //class ChangeTrack
        //{
        //    public string TableName { get; set; }
        //    public DbEntityEntry Entity  { get; set; }
        //}

        //private IList<ChangeTrack> GetTableNames(IEnumerable<DbEntityEntry> entries)
        //{
        //    System.Data.Entity.Core.Objects.ObjectContext objectContext = ((IObjectContextAdapter)Context).ObjectContext;
        //    System.Data.Entity.Core.Metadata.Edm.EntityContainer container =
        //        objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, System.Data.Entity.Core.Metadata.Edm.DataSpace.CSpace);

        //    return (from meta in container.BaseEntitySets
        //            join e in entries on meta.ElementType.Name equals e.Entity.GetType().Name
        //                            select new ChangeTrack { Entity = e, TableName = meta.Name }).ToList();
        //}

        class Original
        {
            public string ModifiedUser { get; set; }
            public DbEntityEntry Entity { get; set; }
        }
        //public class CommandIntercepter : IDbCommandInterceptor
        //{
        //    public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        //    {
        //    }

        //    public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        //    {
        //    }

        //    public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        //    {
                
        //    }

        //    public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        //    {
        //    }

        //    public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        //    {
        //    }

        //    public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        //    {
        //    }
        //}
        public List<Error> SaveChanges(string culture, out List<DbEntityEntry> entries)
        {
            string message = "";
            entries = null;
            try
            {
                entries = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
                var org_entries = entries.Where(e => e.Entity.ToString() != "Model.Domain.AudiTrail").ToList();
                IList<Original> originals = new List<Original>();

                if (org_entries.Count() > 0)
                {
                    try
                    {
                        // reset for new records
                        foreach (var entry in org_entries)
                        {
                            if (entry.CurrentValues.PropertyNames.Contains("ModifiedUser"))
                            {
                                originals.Add(new Original { Entity = entry, ModifiedUser = entry.CurrentValues.GetValue<string>("ModifiedUser") });
                                entry.Property("ModifiedUser").CurrentValue = null;
                                entry.Property("ModifiedTime").CurrentValue = null;
                            }
                        }
                    }
                    catch
                    {
                        // do nothing
                    }
                }

                // save all pending changes
              
                Context.SaveChanges();
               

                if (originals.Count() > 0) // found new inserted records so resolve references
                {
                    var changed = false;
                    foreach (var org in originals)
                    {
                        if (!org.Entity.CurrentValues.PropertyNames.Contains("Id")) continue;

                        int Id = org.Entity.CurrentValues.GetValue<int>("Id");
                        if (Id > 0)
                        {
                            var auditrails = entries.Where(e => e.Entity.ToString() == "Model.Domain.AudiTrail" && e.CurrentValues.GetValue<byte>("Transtype") == 1 && e.CurrentValues.GetValue<string>("ValueBefore") == org.ModifiedUser).Select(e => e.Entity);
                            foreach (var entity in auditrails)
                            {
                                changed = true;
                                var auditrail = (Model.Domain.AudiTrail)entity;
                                auditrail.SourceId = Id.ToString();
                                auditrail.ValueBefore = null;
                                AudiTrialRepository.Attach(auditrail);
                                AudiTrialRepository.Entry(auditrail).State = EntityState.Modified;
                            }
                        }
                    }
                    
                    if (changed)
                        Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                message = HandleDbExceptions(ex, culture);
            }

            if (message.Length > 0)
            {
                return new List<Error>() { new Error { errors = new List<ErrorMessage>() { new ErrorMessage() {
                message = message
                } } } };
            }
            else
            {
                return new List<Error>();
            }
        }

        public DbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public List<Error> SaveChanges(string culture)
        {
            string message = "";
            
            try
            {
                // save all pending changes
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                message = HandleDbExceptions(ex, culture);
            }

            if (message.Length > 0)
            {
                return new List<Error>() { new Error { errors = new List<ErrorMessage>() { new ErrorMessage() {
                message = message
                } } } };
            }
            else
            {
                return new List<Error>();
            }
        }

        public ICompanyRepository CompanyRepository { get; private set; }
        public IPagesRepository PagesRepository { get; private set; }
        public IMenuRepository MenuRepository { get; private set; }
        public IPageEditorRepository PageEditorRepository { get; private set; }
        public ILookUpRepository LookUpRepository { get; private set; }
        public ICompanyStructureRepository CompanyStructureRepository { get; private set; }
        public IJobRepository JobRepository { get; private set; }
        public IEmployeeRepository EmployeeRepository { get; private set; }
        public IPeopleRepository PeopleRepository { get; private set; }
        public IPositionRepository PositionRepository { get; private set; }
        public IBudgetRepository BudgetRepository { get; private set; }
        public IQualificationRepository QualificationRepository { get; private set; }
        public ILeaveRepository LeaveRepository { get; private set; }
        public ITrainingRepository TrainingRepository { get; private set; }
        public ICustodyRepository CustodyRepository { get; private set; }
        public IAudiTrialRepository AudiTrialRepository { get; private set; }
        public IDisciplineRepository DisciplineRepository { get; private set; }
        public ITerminationRpository TerminationRepository  { get; private set; }
        public IPayrollRepository PayrollRepository { get; private set; }
        public IBenefitsRepository BenefitsRepository { get; private set; }
        public ICheckListRepository CheckListRepository { get; private set; }
        public IComplaintRepository ComplaintRepository { get; private set; }
        public IMessageRepository MessageRepository { get; private set; }
        public IMedicalRepository MedicalRepository { get; private set; }
        public IHRLettersRepository HrLettersRepository { get; private set; }
        public ISalryDesignRepository SalaryDesignRepository { get; private set; }
        public ICacheManager CacheManager { get; private set; }
        public INotificationRepository NotificationRepository { get; private set; }
        public IMissionRepository MissionRepository { get; private set; }
        public IMeetingRepository MeetingRepository { get; private set; }


        IBranchRepository _BranchRepoitory;
        public IBranchRepository BranchRepository
        {
            get
            {
                if (_BranchRepoitory == null) { _BranchRepoitory = new BranchRepository(Context); }
                return _BranchRepoitory;
            }
        }

        ISiteRepository _SiteRepository;
        public ISiteRepository SiteRepository
        {
            get
            {
                if (_SiteRepository == null) { _SiteRepository = new SiteRepository(Context); }
                return _SiteRepository;
            }
        }


        IDocTypesRepository _DocTypesRepository;
        public IDocTypesRepository DocTypesRepository
        {
            get
            {
                if (_DocTypesRepository == null) { _DocTypesRepository = new DocTypesRepository(Context); }
                return _DocTypesRepository;
            }
        }

        ICompanyDocsViewsRepository _CompanyDocsViewsRepository;
        public ICompanyDocsViewsRepository CompanyDocsViewsRepository
        {
            get
            {
                if (_CompanyDocsViewsRepository == null) { _CompanyDocsViewsRepository = new CompanyDocsViewsRepository(Context); }
                return _CompanyDocsViewsRepository;
            }
        }
        ICompanyDocAttrRepository _CompanyDocAttrRepository;
        public ICompanyDocAttrRepository CompanyDocAttrRepository
        {
            get
            {
                if (_CompanyDocAttrRepository == null) { _CompanyDocAttrRepository = new CompanyDocAttrRepository(Context); }
                return _CompanyDocAttrRepository;
            }
        }

        ISendFormRepository _SendFormRepository;
        public ISendFormRepository SendFormRepository
        {
            get
            {
                if (_SendFormRepository == null) { _SendFormRepository = new SendFormRepository(Context); }
                return _SendFormRepository;
            }
        }

        IPersonFormRepository _PersonFormRepository;
        public IPersonFormRepository PersonFormRepository
        {
            get
            {
                if (_PersonFormRepository == null) { _PersonFormRepository = new PersonFormRepository(Context); }
                return _PersonFormRepository;
            }
        }

        INotifyLetterRepository _NotifyLetterRepository;
        public INotifyLetterRepository NotifyLetterRepository
        {
            get
            {
                if (_NotifyLetterRepository == null) { _NotifyLetterRepository = new NotifyLetterRepository(Context); }
                return _NotifyLetterRepository;
            }
        }

        #region Loans

        ILoanTypeRepository _LoanTypeRepository;

        public ILoanTypeRepository LoanTypeRepository
        {
            get
            {
                if (_LoanTypeRepository == null) { _LoanTypeRepository = new LoanTypeRepository(Context); }
                return _LoanTypeRepository;
            }
        }
        #endregion

    }
}

