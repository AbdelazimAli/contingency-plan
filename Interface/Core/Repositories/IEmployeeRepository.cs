using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Administration;
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
    public interface IEmployeeRepository : IRepository<Assignment>
    {
        void GetAssignment(int EmpID, out int JobID, out int Gender, out int NationalityID);
        Dictionary<string, string> ReadMailEmpNewContract(string Language, int Id);
        Dictionary<string, string> ReadMailEmpContractFinish(string Language, int Id);
        IEnumerable<ExtendOrFinishContractViewModel> SendMailEmployees();
        string[] GetEmpMergeData(int EmpId, int CompanyId, string Culture);
        IQueryable<DropDownList> GetTermActiveEmployees(string culture, int Id, int CompanyId);
        IEnumerable CountGenderByEmployment(int companyId, string culture);
        string GetFlexDataCheck(string tableName, int SourceId, int EmpId);
        IQueryable<ManagerEmployeeDiagram> EmployeesDiagram(int CompanyId, string Culture);
        IQueryable<PeopleGridViewModel> GetAssignments(string culture);
        IQueryable<PeopleGridViewModel> GetCurrentEmployee(int company, string culture);
        IQueryable<PeopleGridViewModel> GetWaitingEmployee(int company, string culture);
        IQueryable<PeopleGridViewModel> GetTerminatedEmployee(int company, string culture);
        EmployementViewModel GetPersonTypeAndEmployee(int EmpId);
        IList<DropDownList> GetEmpBenefit(int empId, string culture, int CompanyId);
        AssignmentFormViewModel GetAssignment(int EmpId,string culture);
        IQueryable<SysCodeViewModel> BranchName(int DepId, string culture);
        IQueryable<SysCodeViewModel> Sector(int DepId, string culture);
        IQueryable<ManagerEmployeeDiagram> GetManagers(int CompanyId, string Culture);
        IQueryable<PeopleGridViewModel> GetActiveEmployees(string culture, int Id, int CompanuId);
        //Assignment GetDomainAssignment(int? Id);
        IQueryable<AssignHistoryViewModel> GetHistoryAssignments(int CompanyId, string culture, int Id);
        IQueryable<FormList> EmployeeMangers(int CompanyId, string Culture, int? Position);

        // Assignment GetDomainAssignment(int? Id);
        IQueryable<EmployeeBenefitViewModel> GetEmpBenefits(int id);
        void Add(EmpBenefit benefitPlan);
        DbEntityEntry<EmpBenefit> Entry(EmpBenefit empBenefit);
        void Attach(EmpBenefit empBenefit);
        void Remove(EmpBenefit empBenefit);

        IQueryable<EmpRelativeViewModel> GetEmpRelative(int id);
        void Add(EmpRelative empRelative);
        DbEntityEntry<EmpRelative> Entry(EmpRelative empRelative);
        void Attach(EmpRelative empRelative);
        void Remove(EmpRelative empRelative);

        #region Dashboard

        IEnumerable<PeoplesViewModel> GetEmpStutes(int[] Depts, int stutes, int companyId, string culture);
        IEnumerable<PeoplesViewModel> GetPeopleAgeDepts(bool isDefault, int DeptId, string AgeRang, int companyId, string culture);
        IEnumerable<PeoplesViewModel> GetPeopleLocDepts(bool isDefault, int DeptId, int LocId, int companyId, string culture);
        IEnumerable<PeoplesViewModel> GetPeopleGenderDepts(bool isDefault, int DeptId, int GenderID, int companyId, string culture);
        IEnumerable<PeoplesViewModel> GetPeopleDepts(bool isDefault, int DeptId, int companyId, string culture);
        IEnumerable<PeoplesViewModel> GetPeopleAge(string ageRange, int companyId, string culture);
        IEnumerable CountNationalityByEmployment(int companyId, string culture);
        IEnumerable<PeoplesViewModel> GetPeopleGenderPersonType(int genderId, int personType, int companyId, string culture);
        IEnumerable<PeoplesViewModel> GetPeopleWithNational(int NationaltyId, string culture, int companyId);
        IEnumerable CountEmpsByDepts(int[] depts, int compantyId, string cultuer);
        IEnumerable CountEmpsByBranches(int compantyId, string cultuer);
        IEnumerable<PeoplesViewModel> GetPeopleInBranch(int LocId, int companyId, string culture);
        IEnumerable BranchesByDepts(int[] depts, int compantyId, string cultuer);
        //IEnumerable HeadCountByJob(int CompanyId, string culture);
        //IEnumerable<ChartViewModel> CountLenghtofService(int companyId, string culture);
        IEnumerable EmployeesAges(int companyId, string cultuer);
        IEnumerable AgesByDepts(int[] depts, int companyId, string culture);
        IQueryable<PeopleGridViewModel> GetAllEmployees(string culture);
        IEnumerable<ChartViewModel> EmployeesStatus(int[] depts, int CompanyId, string culture);
        IEnumerable GenderByDepts(int[] depts, int companyId, string culture);
        IEnumerable<EmpsInYearViewModel> GetActiveByMonth(int CompanyId);
        #endregion

        bool CheckDocs(int CompanyId, int jobId, int EmpId);
        void RemoveContext();
        bool CheckEmployment(Employement Emp, int EmpId, DateTime AssignDate);
        //Candidates
        IQueryable<JobCandidatesViewModel> ReadCandidates(string tableName, int sourceId, int companyId, string culture);
        IQueryable<EmpIdenticalViewModel> ReadEmpIdentical(int empId, string tableName, int sourceId, string culture);
        IQueryable<ExcelPeopleViewModel> GetPeopleExcel(string Language, int CompanyId);

        //Cancel Leave
        void CancelLeaveAssignState(LeaveRequest request, string UserName, byte version, string culture);

    }
}
