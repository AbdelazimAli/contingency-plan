using Interface.Core.Repositories;
using Interface.Core.Repositories.Loan;
using Model.Domain;
using System.Data.Entity;

namespace Interface.Core
{
    public partial interface IHrUnitOfWork : IUnitOfWork
    {
        DbContextTransaction GetTransaction();
        string HandleDbExceptions(System.Exception ex, string culture);
        System.Collections.Generic.List<Model.ViewModel.Error> SaveChanges(string culture, out System.Collections.Generic.List<System.Data.Entity.Infrastructure.DbEntityEntry> entries);
        System.Collections.Generic.List<Model.ViewModel.Error> SaveChanges(string culture);
        System.Data.Entity.DbContextTransaction BeginTransaction();

        ICompanyDocAttrRepository CompanyDocAttrRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IPagesRepository PagesRepository { get; }
        IMenuRepository MenuRepository { get; }
        IPageEditorRepository PageEditorRepository { get; }
        ILookUpRepository LookUpRepository { get; }
        ISiteRepository SiteRepository { get; }
        IBranchRepository BranchRepository { get; }
        IJobRepository JobRepository { get; }
        IPeopleRepository PeopleRepository { get; }
        ICompanyStructureRepository CompanyStructureRepository { get; }
        IPositionRepository PositionRepository { get; }
        IBudgetRepository BudgetRepository { get; }
        IQualificationRepository QualificationRepository { get; }
        ILeaveRepository LeaveRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        ITrainingRepository TrainingRepository { get; }
        ICustodyRepository CustodyRepository { get; }
        IBenefitsRepository BenefitsRepository { get; }
        IAudiTrialRepository AudiTrialRepository { get; }
        ITerminationRpository TerminationRepository { get; }
        IDisciplineRepository DisciplineRepository { get; }
        ICheckListRepository CheckListRepository { get; }
        IComplaintRepository ComplaintRepository { get; }
        IMessageRepository MessageRepository { get; }
        IMedicalRepository MedicalRepository { get; }
        IHRLettersRepository HrLettersRepository { get; }
        IPayrollRepository PayrollRepository { get; }
        ISalryDesignRepository SalaryDesignRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IDocTypesRepository DocTypesRepository { get; }
        ICompanyDocsViewsRepository CompanyDocsViewsRepository { get; }
        IMissionRepository MissionRepository { get; }
        INotifyLetterRepository NotifyLetterRepository { get; }
        ISendFormRepository SendFormRepository { get; }
        IPersonFormRepository PersonFormRepository { get; }
        IMeetingRepository MeetingRepository { get; }

        #region  Loans
        ILoanTypeRepository LoanTypeRepository { get; }
        #endregion
    }
}
