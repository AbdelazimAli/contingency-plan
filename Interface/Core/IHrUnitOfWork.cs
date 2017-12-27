using Interface.Core.Repositories;
using Model.Domain;

namespace Interface.Core
{
    public partial interface IHrUnitOfWork : IUnitOfWork
    {
        string HandleDbExceptions(System.Exception ex, string culture);
        System.Collections.Generic.List<Model.ViewModel.Error> SaveChanges(string culture, out System.Collections.Generic.List<System.Data.Entity.Infrastructure.DbEntityEntry> entries);
        System.Collections.Generic.List<Model.ViewModel.Error> SaveChanges(string culture);

        ICompanyRepository CompanyRepository { get; }
        IPagesRepository PagesRepository { get; }
        IMenuRepository MenuRepository { get; }
        IPageEditorRepository PageEditorRepository { get; }
        ILookUpRepository LookUpRepository { get; }
        ILocationRepository LocationRepository { get; }
        IJobRepository JobRepository { get;}
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
    }
}
