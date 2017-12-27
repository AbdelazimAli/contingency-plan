using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel.Personnel;
using System.Data.Entity;
using System.Linq;
using System;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using Model.ViewModel.Administration;

namespace Db.Persistence.Repositories
{
  class CustodyRepository : Repository<Custody>, ICustodyRepository
    {
        public CustodyRepository(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public IQueryable<ExcelCustodyViewModel> ReadExcelCustody(int CompanyId,string Language)
        {
            return from c in context.Custody
                   where c.CompanyId == CompanyId && !c.CustodyCat.Disposal

                   select new ExcelCustodyViewModel
                   {
                       Code = c.Code,
                       CustodyCatId = HrContext.TrlsName(c.CustodyCat.Name, Language),
                       Name = c.Name,
                       Description = c.Description,
                       Id = c.Id,
                       SerialNo = c.SerialNo,
                       Status = c.Status.ToString(),
                       PurchaseAmount = c.PurchaseAmount.ToString(),
                       Curr = HrContext.TrlsName(c.Currency.Name, Language),
                       InUse = HrContext.TrlsMsg(c.InUse.ToString(), Language),
                       ItemCode = c.ItemCode,
                       JobId = HrContext.TrlsName(c.Job.Name, Language),
                       LocationId = HrContext.TrlsName(c.Location.Name, Language),
                       PurchaseDate = c.PurchaseDate != null ? c.PurchaseDate.Value.ToString() : " ",
                       Freeze = HrContext.TrlsMsg(c.Freeze.ToString(), Language),
                   };
        }
        public IQueryable<AssignmentGridViewModel> GetActiveEmployee(string culture, List<int> Id)
        {


            var employees = from p in context.People
                            join a in context.Assignments on p.Id equals a.EmpId
                            where (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today )|| Id.Contains(a.EmpId)
                            select new AssignmentGridViewModel
                            {
                                Id = p.Id,
                                Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture)
                            };
            return employees;
        }

        public Dictionary<string, string> ReadMailEmpBorrowDoc(string Language, int Id)
        {
            DateTime Today = DateTime.Today.Date;

            var query = (from c in context.EmpDocBorrow
                         where c.Id == Id
                         join a in context.Assignments on c.EmpId equals a.EmpId into g
                         from a in g.Where(x => x.CompanyId == c.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                         select new
                         {
                             EmployeeName = HrContext.TrlsName(c.Employee.Title + " " + c.Employee.FirstName + " " + c.Employee.Familyname, Language),
                             DeleveryDate = c.ExpdelvryDate,
                             Department = HrContext.TrlsName(a.Department.Name, Language),
                             Job = HrContext.TrlsName(a.Job.Name, Language),
                             RecievedDate = c.RecvDate,
                             Site = c.Site,
                             Document = context.DocBorrowList.Where(d => d.DocBorrowId == Id).Select(s => s.Doc.Name).ToList(),
                             RecieveDay = "",
                             DeleveryDay = "",
                             Papers = ""
                         }).FirstOrDefault();


            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (query != null)
            {
               
                var ObjProps = query.GetType().GetProperties();
                for (int i = 0; i < ObjProps.Length; i++)
                {
                    var p = ObjProps[i].GetValue(query);
                    if (ObjProps[i].Name == "Papers")
                        p = query.Document.Count > 0 ? string.Join(",", query.Document) : " ";
                    else if (ObjProps[i].Name == "RecieveDay")
                        p = MsgUtils.Instance.Trls(Language, query.RecievedDate.DayOfWeek.ToString());
                    else if (ObjProps[i].Name == "DeleveryDay")
                        p = query.DeleveryDate != null ? MsgUtils.Instance.Trls(Language, query.DeleveryDate.Value.DayOfWeek.ToString()) : " ";
                    else if (ObjProps[i].Name == "RecievedDate")
                        p = query.RecievedDate.ToShortDateString();
                    else if (ObjProps[i].Name == "DeleveryDate")
                        p = query.DeleveryDate != null ? query.DeleveryDate.Value.ToShortDateString():" ";
                    dic.Add(ObjProps[i].Name, p != null ? p.ToString() : " ");
                }
            }
            return dic;
        }
        public Dictionary<string, string> ReadMailEmpCustody(string Language, int Id)
        {
            DateTime Today = DateTime.Today.Date;
          
            var query = (from c in context.EmpCustodies
                         where c.Id == Id
                         join a in context.Assignments on c.EmpId equals a.EmpId into g
                         from a in g.Where(x => x.CompanyId == c.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                         select new
                         {
                             EmployeeName = HrContext.TrlsName(c.Employee.Title + " " + c.Employee.FirstName + " " + c.Employee.Familyname, Language),
                             SerialNo = c.Custody.SerialNo,
                             RecivedDate = c.RecvDate.ToString(),
                             PurchaseAmount = c.Custody.PurchaseAmount,
                             Status = c.Custody.Status,
                             Notes = c.Notes,
                             CustodyCategory = HrContext.TrlsName(c.Custody.CustodyCat.Name, Language),
                             CustodyName = c.Custody.Name,
                             EmployeeDepartment = HrContext.TrlsName(a.Department.Name, Language),
                             EmployeeJob = HrContext.TrlsName(a.Job.Name, Language),
                             Day = c.RecvDate,
                             Description = c.Custody.Description,
                             Quantity = c.Qty.ToString(),
                             Code = c.Custody.Code
                         }).FirstOrDefault();


            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (query != null)
            {
                var ObjProps = query.GetType().GetProperties();
                for (int i = 0; i < ObjProps.Length; i++)
                {
                    var p = ObjProps[i].GetValue(query);
                    if (ObjProps[i].Name == "Day")
                        p = MsgUtils.Instance.Trls(Language, query.Day.DayOfWeek.ToString());
                    dic.Add(ObjProps[i].Name, p != null ? p.ToString() : " ");
                }
            }
            return dic;
        }
        public IQueryable<AssignmentGridViewModel> GetActiveJobEmployee(string culture, int jobId)
        {
            var employees = from p in context.People
                            join a in context.Assignments on p.Id equals a.EmpId
                            where (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today) && a.JobId == jobId
                            select new AssignmentGridViewModel
                            {
                                Id = p.Id,
                                Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture)
                            };
            return employees;
        }
        public IQueryable<CustodyViewModel> ReadCustody(byte Range, DateTime? Start, DateTime? End, string culture , int CompanyId)
        {
            //10- All, 0-Custom
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, CompanyId, out Start, out End);

            var custody = from c in context.Custody
                          where ( c.CompanyId == CompanyId && !c.CustodyCat.Disposal && !c.InUse)
                          select new CustodyViewModel
                         {
                            Id = c.Id,
                            Code = c.Code,
                            Name = c.Name,
                            SerialNo = c.SerialNo,
                            CompanyId=c.CompanyId,
                            CustodyCatId = HrContext.TrlsName(c.CustodyCat.Name, culture),
                            PurchaseDate = c.PurchaseDate,
                            PurchaseAmount = c.PurchaseAmount,
                            Status = c.Status,
                            InUse = c.InUse,
                            Freeze = c.Freeze,
                            Description = c.Description,                        
                            Qty = c.Qty                   
                          };

            if (Range != 10)
                custody = custody.Where(c => Start <= c.PurchaseDate && End >= c.PurchaseDate);

            return custody;
        }
        public CustodyFormViewModel ReadCustObject(int id,string culture)
        {
            var res = (from c in context.Custody
                      where c.Id == id
                      select new CustodyFormViewModel
                      {
                          Id = c.Id,
                          Name = c.Name,
                          Code = c.Code,
                          CustodyCatId = c.CustodyCatId,
                          Description = c.Description,
                          SerialNo = c.SerialNo,
                          Attachments = HrContext.GetAttachments("Custody", c.Id),
                          PurchaseDate = c.PurchaseDate,
                          JobId = c.JobId,
                          LocationId = c.LocationId,
                          Status = c.Status,
                          PurchaseAmount = c.PurchaseAmount,
                          ItemCode = c.ItemCode,
                          Qty = c.Qty,
                          Curr = c.Curr,
                          Freeze = c.Freeze,
                          CurrencyRate = c.CurrencyRate == 0 ? 1 : c.CurrencyRate,
                          CreatedTime = c.CreatedTime,
                          CreatedUser = c.CreatedUser,
                          ModifiedTime = c.ModifiedTime,
                          ModifiedUser = c.ModifiedUser
                      }).FirstOrDefault();
            return res;
        }
        public List<string> CustodyNames(bool Disposal)
        {
            var req = (from c in context.Custody
                       join g in context.CustodyCategory on c.CustodyCatId equals g.Id
                       where g.Disposal == Disposal
                       select c.Name).ToList();
            return req;
        }
        //ReadRecievedCustody
        public RecievedCustodyForm ReadRecievedCustody(int id, string culture)
        {
            var res = (from c in context.Custody
                       where c.Id == id
                       select new RecievedCustodyForm
                       {
                           CustodyId = id,
                           Name = c.Name,
                           CompanyId = c.CompanyId,
                           CustodyCatId = c.CustodyCatId,
                           PurchaseDate = c.PurchaseDate,                          
                           RecvStatus = c.Status,
                           PurchaseAmount = c.PurchaseAmount,
                           ItemCode = c.ItemCode,
                           Qty = 1,
                           SerialNo = c.SerialNo,
                           Attachments = HrContext.GetAttachments("RecieveCustody", id),
                       }).FirstOrDefault();
            return res;
        }
        public RecievedCustodyForm ReadEditRecievedCustody(int id, string culture)
        {
            var res = (from e in context.EmpCustodies
                       where e.Id == id && e.delvryDate == null
                       join c in context.Custody on e.CustodyId equals c.Id
                       select new RecievedCustodyForm
                       {
                           Id = e.Id,
                           CustodyId =c.Id,
                           Name = c.Name,
                           CompanyId = c.CompanyId,
                           CustodyCatId = c.CustodyCatId,
                           PurchaseDate = c.PurchaseDate,
                           RecvStatus = c.Status,
                           PurchaseAmount = c.PurchaseAmount,
                           ItemCode = c.ItemCode,
                           Qty = 1,
                           SerialNo = c.SerialNo,
                           Attachments = HrContext.GetAttachments("RecieveCustody", id),
                           EmpId = e.EmpId,
                           RecvDate = e.RecvDate,
                           Notes = e.Notes,
                           LocationId = e.LocationId
                       }).FirstOrDefault();
            return res;
        }
        //ReadDeleverCustody
        public DeleverCustodyFormViewModel ReadDeleverCustody(int id,int EmpId,int EmpCustodyId,int companyId, string culture)
        {
            var res = (from c in context.Custody
                       where c.Id == id && c.CompanyId == companyId
                       join e in context.EmpCustodies on c.Id equals e.CustodyId where e.Id == EmpCustodyId 
                       select new DeleverCustodyFormViewModel
                       {
                           Id = EmpCustodyId,
                           CustodyId = id,
                           EmpCustodyId = EmpCustodyId,
                           Name = c.Name,
                           CompanyId = c.CompanyId,
                           CustodyCatId = c.CustodyCatId,
                           delvryStatus = c.Status,
                           EmpId = EmpId,
                           RecvStatus = c.Status,
                           Notes = e.Notes,
                           RecvDate = e.RecvDate,
                           Attachments = HrContext.GetAttachments("DeleverCustody", id)
                       }).FirstOrDefault();
            return res;
        }
        public IQueryable<CustodyViewModel> ReadEmpCustody(int CompanyId,string culture)
        {
            var result = (from c in context.Custody
                          where (c.CompanyId == CompanyId && c.InUse == true)
                          join Emp in context.EmpCustodies on c.Id equals Emp.CustodyId
                          where Emp.delvryDate == null
                          join p in context.People on Emp.EmpId equals p.Id
                          join l in context.Locations on Emp.LocationId equals l.Id into j
                          from l in j.DefaultIfEmpty()
                          select new CustodyViewModel
                          {
                              Id = c.Id,
                              EmpCustodyId = Emp.Id,
                              Code = c.Code,
                              Name = c.Name,
                              SerialNo = c.SerialNo,
                              CompanyId = c.CompanyId,
                              CustodyCatId = HrContext.TrlsName(c.CustodyCat.Name, culture),
                              PurchaseAmount = c.PurchaseAmount,
                              Status = c.Status,
                              InUse = c.InUse,
                              Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                              Location = HrContext.TrlsName(l.Name, culture),
                              EmpId = Emp.EmpId,
                              Description = c.Description,
                              Qty = Emp.Qty,
                              RecvDate = Emp.RecvDate,
                              PurchaseDate = c.PurchaseDate,
                              AttUrl = HrContext.GetDoc("RecieveCustody", Emp.Id),
                              Filename = context.CompanyDocsView.Where(a=>a.Source == "RecieveCustody" && a.SourceId == Emp.Id).Select(c=>c.name).FirstOrDefault()
                          });
            return result;
        }
        public List<FormList> fillCatCustody(bool Disposal,string culture)
        {
            var query = (from c in context.CustodyCategory
                         where c.Disposal == Disposal
                         select new FormList
                         {
                             id = c.Id,
                             value =c.Id,
                             name = HrContext.TrlsName(c.Name, culture),
                             text = HrContext.TrlsName(c.Name, culture)
                         }).ToList();
            return query;
        }
        public List<FormList> GetCatCustody(string culture)
        {
            var query = (from c in context.CustodyCategory
                         select new FormList
                         {
                             id = c.Id,
                             name = HrContext.TrlsName(c.Name, culture)
                         }).ToList();
            return query;
        }
        public IQueryable<CustodyViewModel> ReadEmpConsumeCustody(int CompanyId, string culture)
        {
            var result = from c in context.Custody
                         where (c.CompanyId == CompanyId && c.CustodyCat.Disposal)
                         join Emp in context.EmpCustodies on c.Id equals Emp.CustodyId
                         join p in context.People on Emp.EmpId equals p.Id
                         join l in context.Locations on Emp.LocationId equals l.Id into j
                         from l in j.DefaultIfEmpty()
                         select new CustodyViewModel
                         {
                             Id = Emp.Id,
                             Code = c.Code,
                             Name = c.Name,
                             SerialNo = c.SerialNo,
                             CompanyId = c.CompanyId,
                             CustodyCatId = HrContext.TrlsName(c.CustodyCat.Name, culture),
                             PurchaseDate = c.PurchaseDate,                    
                             PurchaseAmount = c.PurchaseAmount,
                             Status = c.Status,
                             InUse = c.InUse,
                             Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             Location = HrContext.TrlsName(l.Name, culture),
                             EmpId = Emp.EmpId,
                             Description = c.Description,
                             Qty = Emp.Qty
                         };
            return result;
        }
        //ReadEmployeeCustody
        public IEnumerable<EmpCustodyViewModel> ReadEmployeeCustody(int empId)
        {
            var result = (from c in context.EmpCustodies
                         where c.EmpId == empId
                         select new EmpCustodyViewModel
                         {
                             Id = c.Id,
                             CustodyId = c.CustodyId,
                             delvryDate = c.delvryDate,
                             EmpId = c.EmpId,
                             RecvDate = c.RecvDate,
                             CustodyStat = c.RecvStatus
                         }).ToList();
            return result;
        }
        //ReadConsumeCustody
        public IQueryable<CustodyViewModel> ReadConsumeCustody(byte Range, DateTime? Start, DateTime? End, string culture, int CompanyId)
        {
            //10- All, 0-Custom
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, CompanyId, out Start, out End);
            var query = from c in context.Custody
                        where (c.CompanyId == CompanyId && c.CustodyCat.Disposal)
                        join ec in context.EmpCustodies on c.Id equals ec.CustodyId into g
                        from ec in g.DefaultIfEmpty()
                        group ec by new { Id = c.Id, Name = c.Name, PurchaseDate = c.PurchaseDate, PurchaseAmount = c.PurchaseAmount, Qty = c.Qty, Description = c.Description, CustodyCat = c.CustodyCat.Name ,CurrRate=c.CurrencyRate }  into g
                        select new CustodyViewModel
                        {
                            Id = g.Key.Id,
                            Name = g.Key.Name,
                            PurchaseDate = g.Key.PurchaseDate,
                            PurchaseAmount = g.Key.PurchaseAmount,
                            Qty = g.Key.Qty,
                            CurrencyRate = g.Key.CurrRate,
                            CustodyCatId = HrContext.TrlsName(g.Key.CustodyCat, culture),
                            RestQty = g.Key.Qty - (g.Sum(x => x.Qty) == null ? 0 : g.Sum(x => x.Qty)),
                            Description = g.Key.Description
                        };
            if (Range != 10)
                query = query.Where(c => Start <= c.PurchaseDate && End >= c.PurchaseDate);
            return query.Where(a=>a.RestQty >0);
        }     
        public IQueryable<CustodyCategoryViewModel> ReadCustCategory(string Culture)
        {
            var query = from c in context.CustodyCategory
                        select new CustodyCategoryViewModel
                        {
                            Id = c.Id,
                            CodeLength = c.CodeLength,
                            Disposal = c.Disposal,
                            Name = c.Name,
                            Prefix = c.Prefix,
                            Title = HrContext.TrlsName(c.Name,Culture)
                        };
            return query;
        }
        //GetCustodyReport
        public IQueryable<EmpCustodyViewModel> GetCustodyReport(int Id,string Culture)
        {
            var query = from c in context.EmpCustodies
                        where c.CustodyId == Id
                        join p in context.People on c.EmpId equals p.Id
                        select new EmpCustodyViewModel
                        {
                            Id = c.Id,
                            delvryDate = c.delvryDate,
                            RecvDate = c.RecvDate,
                            Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, Culture),
                        };
            return query;
        }
        public Custody GetCustody(int? id)
        {
            return Context.Set<Custody>().Find(id);
        }
        public EmpCustody GetEmpCustody(int? id)
        {
            return Context.Set<EmpCustody>().Find(id);
        }
        public CustodyCat GetCustodyCat(int? id)
        {
            return Context.Set<CustodyCat>().Find(id);
        }
        public void RemoveCustody(int? id)
        {
            var person = Context.Set<Custody>().Find(id);
            if (person != null) Remove(person);
        }
        public void Add(EmpCustody emp)
        {
            context.EmpCustodies.Add(emp);
        }
        public void Add(EmpDocBorrow Borrow)
        {
            context.EmpDocBorrow.Add(Borrow);
        }
        public void Add(DocBorrowList BorrowList)
        {
            context.DocBorrowList.Add(BorrowList);
        }
        public void Attach(EmpDocBorrow Borrow)
        {
            context.EmpDocBorrow.Attach(Borrow);
        }
        public DbEntityEntry<EmpDocBorrow> Entry(EmpDocBorrow Borrow)
        {
            return Context.Entry(Borrow);
        }
        public void Add(CustodyCat cat)
        {
            context.CustodyCategory.Add(cat);
        }
        public void Attach(CustodyCat cat)
        {
            context.CustodyCategory.Attach(cat);
        }
        public void Remove(CustodyCat cat)
        {
            if (Context.Entry(cat).State == EntityState.Detached)
            {
                context.CustodyCategory.Attach(cat);
            }
            context.CustodyCategory.Remove(cat);
        }
        public void Remove(DocBorrowList doc)
        {
            if (Context.Entry(doc).State == EntityState.Detached)
            {
                context.DocBorrowList.Attach(doc);
            }
            context.DocBorrowList.Remove(doc);
        }
        public DbEntityEntry<CustodyCat> Entry(CustodyCat cat)
        {
            return Context.Entry(cat);
        }
        public void Attach(EmpCustody emp)
        {
            context.EmpCustodies.Attach(emp);
        }
        public void Remove(EmpCustody emp)
        {
            if (Context.Entry(emp).State == EntityState.Detached)
            {
                context.EmpCustodies.Attach(emp);
            }
            context.EmpCustodies.Remove(emp);
        }
        public DbEntityEntry<EmpCustody> Entry(EmpCustody emp)
        {
            return Context.Entry(emp);
        }

        #region Emp Doc Borrow
        //read Borrow Document with Employee
        public IEnumerable<EmpDocBorrowViewModel> ReadDocBorrow(byte Range, DateTime? Start, DateTime? End, int CompanyId, string culture)
        {
            //10- All, 0-Custom
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, CompanyId, out Start, out End);
            var query= (from d in context.EmpDocBorrow
                         where (d.CompanyId == CompanyId && d.delvryDate == null)
                         join p in context.People on d.EmpId equals p.Id
                         select new EmpDocBorrowViewModel
                         {
                             Id = d.Id,
                             CompanyId = d.CompanyId,
                             Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             EmpId = d.EmpId,
                             Purpose = d.Purpose,
                             Notes = d.Notes,
                             RecvDate = d.RecvDate,
                             Site = d.Site,
                             ExpdelvryDate = d.ExpdelvryDate,
                             AttUrl = HrContext.GetDoc("BorrowPapers", d.Id),
                             Document = context.DocBorrowList.Where(a => a.DocBorrowId == d.Id).Select(s =>s.Doc.Name).ToList()                          
                         }).ToList();
            if (Range != 10)
                query = query.Where(c => Start <= c.RecvDate && End >= c.RecvDate).ToList();
            return query;
        }
        // Edit Emp Document Borrow 
        public EmpDocBorrowFormViewModel ReadEmpDocBorrow(int Id, string Culture)
        {
            var query = (from c in context.EmpDocBorrow
                        where c.Id == Id
                        select new EmpDocBorrowFormViewModel
                        {
                            Id = c.Id,
                            RecvDate = c.RecvDate,
                            Purpose = c.Purpose,
                            Site = c.Site,
                            EmpId = c.EmpId,
                            ExpdelvryDate = c.ExpdelvryDate,
                            Document = context.DocBorrowList.Where(a => a.DocBorrowId == Id).Select(s => s.DocId).ToList(),
                            Notes = c.Notes,
                            
                        }).FirstOrDefault();
            return query;
        }

        public EmpDocBorrowFormViewModel ReadDeleverDocBorrow(int Id)
        {
            var query = (from c in context.EmpDocBorrow
                         where c.Id == Id
                         select new EmpDocBorrowFormViewModel
                         {
                             Id = c.Id,
                             RecvDate = c.RecvDate,
                             Purpose = c.Purpose,
                             Site = c.Site,
                             EmpId = c.EmpId,
                             Document = context.DocBorrowList.Where(a => a.DocBorrowId == Id).Select(s => s.DocId).ToList(),
                             Notes = c.Notes
                         }).FirstOrDefault();
            return query;
        }

        #endregion

        #region API
        public IQueryable<CustodyViewModel> GetEmpCustody(int EmpId,int CompanyId, string culture)
        {
            var result = from c in context.Custody
                         where (c.CompanyId == CompanyId )
                         join Emp in context.EmpCustodies on c.Id equals Emp.CustodyId
                         where Emp.delvryDate == null && Emp.EmpId == EmpId
                         join p in context.People on Emp.EmpId equals p.Id
                         join l in context.Locations on Emp.LocationId equals l.Id into j
                         from l in j.DefaultIfEmpty()
                         select new CustodyViewModel
                         {
                             Id = c.Id,
                             Code = c.Code,
                             Name = c.Name,
                             SerialNo = c.SerialNo,
                             CustodyCatId = HrContext.TrlsName(c.CustodyCat.Name, culture),
                             Disposal = c.CustodyCat.Disposal,
                             Status = c.Status,
                             InUse = c.InUse,
                             Description = c.Description,
                             PurchaseDate = c.PurchaseDate,
                             PurchaseAmount = c.PurchaseAmount,
                             Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             Location = HrContext.TrlsName(l.Name, culture),
                             EmpId = Emp.EmpId,
                             Qty = Emp.Qty
                         };
            return result;
        }
        #endregion



    }
}
