using Model.Domain;
using Model.ViewModel;
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
    public interface ICheckListRepository : IRepository<CheckList>
    {
        int? GetPeriodId(int? calenderId, DateTime? AssignTime);
        IQueryable<CheckListViewModel> GetCheckLists(string culture, int company);
        CheckListFormViewModel ReadCheckList(int id, string culture);
        void Add(ChecklistTask checklistTask);
        void Add(EmpChkList checklistTask);
        void Add(EmpTask checklistTask);
        void Attach(ChecklistTask checklistTask);
        void Attach(EmpTask checklistTask);
        void Attach(EmpChkList checklistTask);
        DbEntityEntry<ChecklistTask> Entry(ChecklistTask checklistTask);
        DbEntityEntry<EmpTask> Entry(EmpTask checklistTask);
        DbEntityEntry<EmpChkList> Entry(EmpChkList checklistTask);
        void Remove(ChecklistTask checklistTask);
        void Remove(EmpChkList checklistTask);
        void Remove(EmpTask checklistTask);
        EmpChkListViewModel ReadEmpList(int Id);
        IQueryable<CheckListTaskViewModel> ReadCheckListTask(int ListId);
        CheckList GetTermCheckLists(int company);
        DataSource<EmpTaskViewModel> AddSubperiods(int CompanyId, DateTime? AssignTime, int Id, short Count,string Culture);
        EmpChkList AddEmpChlst(CheckList checklist, string UserName, int? EmpId, int CompanyId);
        void AddEmpTask(List<CheckListTaskViewModel> chlist, string UserName, EmpChkList Emplist);
        IQueryable<EmpChkListViewModel> GetEmpCheckLists(string culture, int companyId);
        IQueryable<EmpTaskViewModel> ReadEmpListTask(int EmpList);

        //SS Tasks
        IEnumerable ReadTasksSubPeriods(int managerId);
        IEnumerable ReadTasksPeriods(int managerId, int companyId, string culture, out string message);

        IQueryable<EmpTasksViewModel> ReadManagerTasks(int managerId, int? periodId, int? subPeriodId, int companyId, string culture);
        IQueryable<EmpTasksViewModel> ReadManagerEmpTasks(int managerId, int empId, int? periodId, int? subPeriodId, string culture);
        IEnumerable EmployeeProgress(int managerId, int? periodId, int? subPeriodId, string culture);
        EmpTask GetEmpTask(int id);
        IEnumerable GetManagerEmpList(int managerId, int? positionId, int companyId, string culture);
        IEnumerable ReadEmployeeTasks(int CompanyId, int empId, string culture);
        IQueryable<EmpTasksViewModel> ReadEmployeeTasksGrid(int empId, string culture);
        EmpTasksViewModel GetEmployeeTask(int Id, string culture);
        void AssignNextTask(EmpTask currentTask);
        EmpTasksViewModel GetManagerEmpTask(int Id);
        string GetTaskSubPeriod(int CompanyId, DateTime? AssignTime, string Culture, out int? subPeriodId);

        //Charts
        IEnumerable EmpTasksByPerid(int empId, int companyId, string culture);
        IEnumerable EmpTasksBySubPeriod(int empId, int companyId, string culture);
        IEnumerable ManagerEmployeeTask(int MangerId, int companyId, string culture);
        string ChkBeforeEmployment(int companyId, string UserName, int EmpId, string Culture);
    }
}
