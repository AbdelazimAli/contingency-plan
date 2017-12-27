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
    public interface IPeopleRepository : IRepository<PeopleGroup>
    {
        IQueryable<WorkFlowViewModel> GetAllRequests(int companyId, string culture);
        IQueryable<DropDownList> GetAllPeoples(string culture, int CompanyId);
        PeoplesViewModel ReadPerson(int id, string culture);
        string GetMissingAttachments(int companyId, int empId, string culture, int Gender, int? Nationality);
        string AddEmployee(Person person, string Code, short? personType, int sequence, DateTime? start, string culture, int companyId);
        EmployementViewModel GetEmployment(int EmpId);
        string CheckCode(Employement Emp, string culture);
        IQueryable<PeopleGroupViewModel> GetPeoples();
        PeopleGroup GetPersonGroup(int Id);
        Person GetPerson(int? id);
        Employement FindEmployment(int? id);
        Assignment FindAssignment(int? id);
        void Remove(Employement employment);
        void Remove(Assignment assignment);
        void ReadEmployeesPhotos();
        IQueryable<PersonViewModel> ReadPeoples(string culture);
        void Add(Person person);
        void Attach(Person person);
        void Remove(Person person);
        void RemovePerson(int? id);
        DbEntityEntry<Person> Entry(Person person);
        IQueryable<EmployementViewModel> ReadEmployments(int id);
        IQueryable<EmployementViewModel> GetHistoryEmployement(int Id);
        void Add(Employement Emp);
        void Add(PeopleTraining person);
        void Attach(PeopleTraining person);
        void Remove(PeopleTraining person);
        void Add(PeopleQual qual);
        void Attach(Employement Emp);
        void Attach(PeopleQual qual);
        DbEntityEntry<Employement> Entry(Employement Emp);
        DbEntityEntry<PeopleTraining> Entry(PeopleTraining person);
        IQueryable<EmpQualificationViewModel> ReadQualifications(int id, bool flag);
        // IEnumerable getQualification(string CodeName);
        // IEnumerable getCertification(string CodeName);
        void Remove(PeopleQual qual);
        DbEntityEntry<PeopleQual> Entry(PeopleQual qual);
        // IEnumerable GetActiveEmployees(int companyId, string culture);
        IQueryable<PeopleTrainGridViewModel> ReadEmployeeTraining(int Id);
        double GetAttachmentsCount(int Id, out int attachments);
        double GetProfileCount(int empId, int companyId, byte version);
        IEnumerable<FormList> GetEmployeeById(int companyId, string culture, int EmpId);
        IEnumerable<FormList> GetActiveEmployees(int companyId, string culture);
        IEnumerable<FormList> GetActiveDeptMangers(int companyId, string culture);
        IEnumerable<FormList> GetActiveMangers(int companyId, string culture);
        IEnumerable<FormList> GetActiveMangersByMangerId(int companyId, string culture, int MangerId);
        IEnumerable<FormList> GetEmployeeManagedByManagerId(int companyId, string culture, int MangId);
        IEnumerable<GridListViewModel> getQualification(string CodeName);
        IEnumerable<GridListViewModel> getCertification(string CodeName);
        IQueryable<AuditViewModel> EmployeesLog(int companyId, byte version, int Id, string culture);
        void RemoveRange(IEnumerable<Assignment> AssignmentEntities);
        void RemoveRange(IEnumerable<Employement> EmploymentEntities);
        void RemoveRange(IEnumerable<Person> PersonEntities);

        #region RenewRequest By Abdelazim
        IEnumerable<ColumnDropdownViewModel> GetEditColumn(int CompanyId, string ObjectName, byte Version, string Culture, string RoleId);
        IQueryable<RenewRequestViewModel> ReadRenewRequestTabs(int companyId, byte Tab, byte Range, DateTime? Start, DateTime? End, string culture, byte Version, int EmpId);
        RenewRequestViewModel GetRenewRequest(int requestId);
        ColVlueType GetColValue(int EmpId, string ColumnName);
        RenewRequest Getrequest(int? id);
        void Add(RenewRequest request);
        void Attach(RenewRequest request);
        void Remove(RenewRequest request);
        DbEntityEntry<RenewRequest> Entry(RenewRequest request);
        void ReadRenewRequestAtt();
        string GetDocs(string Source, int SourceId);
        IList<int> GetIdsList(int companyId, int EmpId);
        Dictionary<string, CompanyDocsViews> GetEmpDocsView(int EmpId, int CompanyId);
        #endregion
        #region Region
        string GetEpmLocalname(int EmpId, string Culture);
        #endregion

    }

}
