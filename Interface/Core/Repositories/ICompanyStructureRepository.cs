using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Interface.Core.Repositories
{
   public interface ICompanyStructureRepository:IRepository<CompanyStructure>
    {
        IQueryable<FormList> GetAllDepartments(int CompanyId, int? DeptId, string Culture);
        IQueryable<FormList> GetAllActiveCompanyStructure(int companyId, string Culture);
        CompanyStructureViewModel GetStructure(int? Id,string Culture);
        IQueryable<CompanyDiagramViewModel> GetDiagram(int companyId, string Culture);
        void Add(DeptJobLeavePlan deptLeavePlan);
        void Attach(DeptJobLeavePlan deptLeavePlan);
        DbEntityEntry<DeptJobLeavePlan> Entry(DeptJobLeavePlan deptLeavePlan);
        void Remove(DeptJobLeavePlan deptLeavePlan);
        IQueryable<DeptLeavePlanViewModel> GetDeptLeavePlan(int companyId, int DeptId, string Culture);
        IQueryable<DeptJobLvPlanViewModel> GetJobLeavePlan(int CompanyId, int DeptId, DateTime? FromDate, DateTime? ToDate, string Culture);
        int CheckLeaveRequests(int companyId, IEnumerable<DeptLeavePlanViewModel> plans);
        JobPercentChartVM GetJobsPercentage(int DeptId, int Week, int CompanyId, string Culture);
    }
}
