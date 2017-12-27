using Model.Domain;
using Model.ViewModel.Administration;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Interface.Core.Repositories
{
    public interface ICustodyRepository : IRepository<Custody>
    {
        IQueryable<AssignmentGridViewModel> GetActiveEmployee(string culture,List<int> Id);
        IQueryable<ExcelCustodyViewModel> ReadExcelCustody(int CompanyId, string Language);
        IQueryable<CustodyViewModel> ReadCustody(byte Range, DateTime? Start, DateTime? End, string culture, int CompanyId);
        Custody GetCustody(int? id);
        List<string> CustodyNames(bool Disposal);
        List<FormList> fillCatCustody(bool Disposal, string culture);
        List<FormList> GetCatCustody(string culture);
        EmpCustody GetEmpCustody(int? id);
        CustodyCat GetCustodyCat(int? id);
        CustodyFormViewModel ReadCustObject(int id, string culture);
        RecievedCustodyForm ReadRecievedCustody(int id, string culture);
        IQueryable<CustodyViewModel> ReadEmpCustody(int CompanyId, string culture);
        IQueryable<CustodyViewModel> ReadEmpConsumeCustody(int CompanyId, string culture);
        Dictionary<string, string> ReadMailEmpCustody(string Language, int Id);
        Dictionary<string, string> ReadMailEmpBorrowDoc(string Language, int Id);
        void RemoveCustody(int? id);
        void Add(EmpCustody emp);        
        void Attach(EmpCustody emp);   
        void Remove(EmpCustody emp);
        void Add(CustodyCat cat);
        void Add(DocBorrowList BorrowList);
        void Remove(DocBorrowList doc);
        void Attach(CustodyCat cat);
        void Remove(CustodyCat cat);
        DbEntityEntry<EmpDocBorrow> Entry(EmpDocBorrow Borrow);
        void Attach(EmpDocBorrow Borrow);
        void Add(EmpDocBorrow Borrow);
        DbEntityEntry<CustodyCat> Entry(CustodyCat cat);      
        IQueryable<CustodyCategoryViewModel> ReadCustCategory(string Culture);
        DbEntityEntry<EmpCustody> Entry(EmpCustody emp);
        IEnumerable<EmpCustodyViewModel> ReadEmployeeCustody(int empId);
        IQueryable<AssignmentGridViewModel> GetActiveJobEmployee(string culture, int jobId);
        DeleverCustodyFormViewModel ReadDeleverCustody(int id, int EmpId,int EmpCustodyId, int companyId, string culture);
        IQueryable<CustodyViewModel> ReadConsumeCustody(byte Range, DateTime? Start, DateTime? End, string culture, int CompanyId);
        IQueryable<CustodyViewModel> GetEmpCustody(int EmpId, int CompanyId, string culture);
        IQueryable<EmpCustodyViewModel> GetCustodyReport(int Id, string Culture);
        RecievedCustodyForm ReadEditRecievedCustody(int id, string culture);
        IEnumerable<EmpDocBorrowViewModel> ReadDocBorrow(byte Range, DateTime? Start, DateTime? End,int CompanyId, string culture);
        EmpDocBorrowFormViewModel ReadEmpDocBorrow(int Id, string Culture);
        EmpDocBorrowFormViewModel ReadDeleverDocBorrow(int Id);
    }
}
