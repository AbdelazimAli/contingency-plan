using Model.Domain;
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
    public interface IDisciplineRepository : IRepository<DisplinPeriod>
    {
        EmpDisciplineFormViewModel ReadEmployeeDiscipline(int Id);
        IQueryable<EmpDisciplineViewModel> ReadEmpDiscipline(string culture, int CompanuId);
        List<EmployeePointsViewModel> ReadEmployeePoints(int periodId);
        IQueryable<DisplinPeriodViewModel> ReadDisplinePeriod();
        IQueryable<DisplinePeriodNoViewModel> ReadDisPeriodNo(int Id);
        void Add(DisPeriodNo period);
        void Add(EmpPoints period);
        void Attach(DisPeriodNo period);

        DbEntityEntry<DisPeriodNo> Entry(DisPeriodNo period);

        void Remove(DisPeriodNo period);
        DisplinPeriod GetDisplinPeriod(int? id);
        void RemoveDisplinPeriod(int? id);
        void RemoveRange(IEnumerable<DisPeriodNo> entities);
        IQueryable<DisciplineViewModel> ReadDiscipline(int CompanyId);
        IQueryable<DisplinRepeatViewModel> ReadDisRepeat(int Id);
        void Add(Discipline period);
        EmpDiscipline GetEmpDisplin(int? id);
        void RemoveEmpDisplin(int? id);       
        void Attach(Discipline period);
        IQueryable<InvesigationIndexViewModel> ReadInvestigation(string culture, int companyId);

        DbEntityEntry<Discipline> Entry(Discipline period);

        void Remove(Discipline period);
        void Add(DisplinRepeat period);

        void Attach(DisplinRepeat period);

        DbEntityEntry<DisplinRepeat> Entry(DisplinRepeat period);

        void Remove(DisplinRepeat period);
        void Add(EmpDiscipline period);

        void Attach(EmpDiscipline period);

        DbEntityEntry<EmpDiscipline> Entry(EmpDiscipline period);

        void Remove(EmpDiscipline period);
        void RemoveDiscipline(int? id);
        void RemoveRange(IEnumerable<DisplinRepeat> entities);
        Discipline GetDiscipline(int? id);
        IQueryable<SysDisciplinePeriodViewModel> SysDiscipline();
        IQueryable<DisplinRangeViewModel> ReadDisplinRange(int Id);
        void Add(DisplinRange period);

        void Attach(DisplinRange period);

        DbEntityEntry<DisplinRange> Entry(DisplinRange period);

        void Remove(DisplinRange period);
        DeciplineInfoViewModel GetDesplinInfo(string violdata, int desplinId, string culture);
        IEnumerable ReadPeriods();
        List<DisplinDLLViewModel> GetDisplinDDl(int desplinId, string culture);
        InvestigationFormViewModel ReadInvestigations(int id);
        void Add(Investigation investigate);
        void Attach(Investigation investigate);
        DbEntityEntry<Investigation> Entry(Investigation investigate);
        void Remove(Investigation investigate);
        void Add(InvestigatEmp investigateEmp);

        void Attach(InvestigatEmp investigateEmp);

        DbEntityEntry<InvestigatEmp> Entry(InvestigatEmp investigateEmp);

        void Remove(InvestigatEmp investigateEmp);
        Investigation GetEmpInvestigation(int? id);
        void RemoveEmpInvestigation(int? id);

    }
}
