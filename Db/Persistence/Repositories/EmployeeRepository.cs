using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Data.Entity.Infrastructure;
using Model.ViewModel;
using System.Collections.Generic;
using System.Collections;
using Model.ViewModel.Administration;

namespace Db.Persistence.Repositories
{
    class EmployeeRepository : Repository<Assignment>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext context) : base(context)
        {
        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        #region Relative
        public IQueryable<EmpRelativeViewModel> GetEmpRelative(int id)
        {
            var result = from e in context.EmpRelative
                         where e.EmpId == id
                         select new EmpRelativeViewModel
                         {
                             Id = e.Id,
                             BirthDate = e.BirthDate,
                             ExpiryDate = e.ExpiryDate,
                             GateIn = e.GateIn,
                             NationalId = e.NationalId,
                             Name = e.Name,
                             PassportNo = e.PassportNo,
                             Relation = e.Relation,
                             Telephone = e.Telephone,
                             Entry = e.Entry,
                             EmpId = e.EmpId,
                             CreatedUser = e.CreatedUser,
                             CreatedTime = e.CreatedTime,
                             ModifiedUser = e.ModifiedUser,
                             ModifiedTime = e.ModifiedTime,
                         };

            return result;
        }
        public DbEntityEntry<EmpRelative> Entry(EmpRelative empRelative)
        {
            return Context.Entry(empRelative);
        }
        public void Add(EmpRelative empRelative)
        {
            context.EmpRelative.Add(empRelative);
        }
        public void Attach(EmpRelative empRelative)
        {
            context.EmpRelative.Attach(empRelative);
        }
        public void Remove(EmpRelative empRelative)
        {
            if (Context.Entry(empRelative).State == EntityState.Detached)
            {
                context.EmpRelative.Attach(empRelative);
            }
            context.EmpRelative.Remove(empRelative);
        }
        #endregion
        #region benefit
        public IQueryable<EmployeeBenefitViewModel> GetEmpBenefits(int id)
        {
            var result = from EP in context.EmpBenefits
                         join pl in context.BenefitPlans on  EP.BenefitPlanId equals pl.Id
                         where EP.EmpId == id
                         // where ((EP.StartDate <= DateTime.Today && (EP.EndDate == null || EP.EndDate >= DateTime.Today) && EP.EmpId == id))
                         select new EmployeeBenefitViewModel
                         {
                             Id = EP.Id,
                             BenefitId = EP.BenefitId,
                             BenefitPlanId = EP.BenefitPlanId,
                             StartDate = EP.StartDate,
                             BeneficiaryId=(EP.BeneficiaryId==null ?null : EP.BeneficiaryId) ,
                             EndDate = EP.EndDate,
                             EmpId = EP.EmpId,
                             CreatedUser = EP.CreatedUser,
                             CreatedTime = EP.CreatedTime,
                             ModifiedTime = EP.ModifiedTime,
                             ModifiedUser = EP.ModifiedUser,
                             BenefitPlanName=pl.PlanName,
                         };

            return result;
        }
        public IList<DropDownList> GetEmpBenefit(int empId, string culture, int CompanyId)
        {
            string sql = "SELECT B.Id, dbo.fn_TrlsName(B.Name, '" + culture + "') Name FROM Benefits B, Assignments A, Employements E WHERE A.EmpId = E.EmpId And A.EmpId =" + empId + "AND ((B.IsLocal=1 AND B.CompanyId =" + CompanyId + ") OR B.IsLocal = 0)  AND (Convert(date,GETDATE()) Between A.AssignDate And A.EndDate) AND E.Status = 1 AND (DATEDIFF(m, A.AssignDate, GETDATE()) >= (CASE B.EmpAccural WHEN 3 THEN ISNULL(B.WaitMonth, 0) ELSE 0 END)) AND(Convert(date,GETDATE()) Between B.StartDate And ISNULL(B.EndDate, '2099/01/01'))  AND(CASE WHEN LEN(B.Employments) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(B.Employments, ',') WHERE VALUE = E.PersonType), 0) ELSE E.PersonType END) = E.PersonType AND(CASE WHEN LEN(B.Jobs) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(B.Jobs, ',') WHERE VALUE = A.JobId), 0) ELSE A.JobId END) = A.JobId AND(CASE WHEN LEN(B.CompanyStuctures) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(B.CompanyStuctures, ',') WHERE VALUE = A.DepartmentId), 0) ELSE A.DepartmentId END) = A.DepartmentId AND(CASE WHEN LEN(B.Locations) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(B.Locations, ',') WHERE VALUE = ISNULL(A.LocationId, 0)), -1) ELSE ISNULL(A.LocationId, 0) END) = ISNULL(A.LocationId, 0) AND(CASE WHEN LEN(B.Positions) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(B.Positions, ',') WHERE VALUE = ISNULL(A.PositionId, 0)), -1) ELSE ISNULL(A.PositionId, 0) END) = ISNULL(A.PositionId, 0) AND(CASE WHEN LEN(B.PeopleGroups) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(B.PeopleGroups, ',') WHERE VALUE = ISNULL(A.GroupId, 0)), -1) ELSE ISNULL(A.GroupId, 0) END) = ISNULL(A.GroupId, 0) AND(CASE WHEN LEN(B.Payrolls) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(B.Payrolls, ',') WHERE VALUE = ISNULL(A.PayrollId, 0)), -1) ELSE ISNULL(A.PayrollId, 0) END) = ISNULL(A.PayrollId, 0) AND(CASE WHEN LEN(B.PayrollGrades) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(B.PayrollGrades, ',') WHERE VALUE = ISNULL(A.PayGradeId, 0)), -1) ELSE ISNULL(A.PayGradeId, 0) END) = ISNULL(A.PayGradeId, 0)";
            return context.Database.SqlQuery<DropDownList>(sql).ToList();
        }
        public void Add(EmpBenefit empBenefit)
        {
            context.EmpBenefits.Add(empBenefit);
        }
        public void Attach(EmpBenefit empBenefit)
        {
            context.EmpBenefits.Attach(empBenefit);
        }
        public void Remove(EmpBenefit empBenefit)
        {
            if (Context.Entry(empBenefit).State == EntityState.Detached)
            {
                context.EmpBenefits.Attach(empBenefit);
            }
            context.EmpBenefits.Remove(empBenefit);
        }
        public DbEntityEntry<EmpBenefit> Entry(EmpBenefit empBenefit)
        {
            return Context.Entry(empBenefit);
        }
        #endregion

        public IQueryable<FormList> EmployeeMangers(int CompanyId, string Culture, int? Position)
        {
                DateTime Today = DateTime.Today.Date;
                return from o in context.Positions
                       where o.Id == Position
                       join a in context.Assignments on o.Supervisor equals a.PositionId
                       where (a.AssignDate <= Today && a.EndDate >= Today && a.CompanyId == CompanyId)
                       join p in context.People on a.EmpId equals p.Id
                       orderby a.AssignDate
                       select new FormList
                       {
                           id = p.Id,
                           name = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, Culture)
                       };
           

        }
        public Dictionary<string,string> ReadMailEmpContractFinish(string Language,int Id)
        {
            DateTime Today = DateTime.Today.Date;

            var query = (from r in context.Employements
                         where r.Id == Id
                         join a in context.Assignments on r.EmpId equals a.EmpId into g
                         from a in g.Where(x => x.CompanyId == r.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                         select new
                         {                        
                             EmployeeName = HrContext.TrlsName(r.Person.Title + " " + r.Person.FirstName + " " + r.Person.Familyname, Language),
                             Job = HrContext.TrlsName(a.Job.Name, Language),
                             Department = HrContext.TrlsName(a.Department.Name, Language),
                             InformDate = Today,
                             ContractEndDate = r.EndDate.Value.ToString()
                         }).FirstOrDefault();

            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (query != null)
            {
                var ObjProps = query.GetType().GetProperties();
                for (int i = 0; i < ObjProps.Length; i++)
                {
                    var p = ObjProps[i].GetValue(query);
                    if (ObjProps[i].Name == "InformDate")
                        p = query.InformDate.ToShortDateString();
                    dic.Add(ObjProps[i].Name, p != null ? p.ToString() : " ");
                }
            }
            return dic;
        }

        public Dictionary<string, string> ReadMailEmpNewContract(string Language, int Id)
        {
            DateTime Today = DateTime.Today.Date;

            var query = (from r in context.Employements
                         where r.Id == Id
                         join a in context.Assignments on r.EmpId equals a.EmpId into g
                         from a in g.Where(x => x.CompanyId == r.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                         select new
                         {
                             EmployeeName = HrContext.TrlsName(r.Person.Title + " " + r.Person.FirstName + " " + r.Person.Familyname, Language),
                             InformDate = Today,
                             ContractStartDate = r.StartDate,
                             ContractEndDate = r.EndDate,
                             Department = HrContext.TrlsName(a.Department.Name,Language),
                             Job = HrContext.TrlsName(a.Job.Name,Language),
                             Years = r.DurInYears,
                             Month = r.DurInMonths
                         }).FirstOrDefault();

            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (query != null)
            {
                var ObjProps = query.GetType().GetProperties();
                for (int i = 0; i < ObjProps.Length; i++)
                {
                    var p = ObjProps[i].GetValue(query);
                    if (ObjProps[i].Name == "ContractStartDate")
                        p = query.ContractEndDate.Value.AddDays(1).ToShortDateString();
                    else if (ObjProps[i].Name == "ContractEndDate")
                        p = query.ContractEndDate.Value.AddMonths(query.Month).AddYears(query.Years).ToShortDateString();
                    else if (ObjProps[i].Name == "InformDate")
                        p = query.InformDate.ToShortDateString();

                    dic.Add(ObjProps[i].Name, p != null ? p.ToString() : " ");
                }
            }
            return dic;
        }
        public IEnumerable<ExtendOrFinishContractViewModel> SendMailEmployees()
        {

            //First Job Send Email for Employees
            var Emps = (from e in context.Employements 
                        where e.Status == 1 && e.EndDate != null 
                        join p in context.People on e.EmpId equals p.Id 
                        join l in context.LookUpUserCodes on e.PersonType equals l.CodeId
                        where l.CodeName == "PersonType" && l.SysCodeId == 1
                        select new ExtendOrFinishContractViewModel
                        {

                            CompanyId = e.CompanyId,
                            Email = p.WorkEmail,
                            EmpId = e.EmpId,
                            Id = e.Id,
                            Renew = e.AutoRenew,
                            RemindarDays = e.RemindarDays,
                            EndDate = e.EndDate.Value
                        } ).ToList();

         //  var Emps = context.Employements.Include("Person").Where(e=>e.Status == 1 && e.EndDate != null).Join<(context.LookUpUserCodes,)
            List<ExtendOrFinishContractViewModel> SendEmps = new List<ExtendOrFinishContractViewModel>();
            DateTime Today = DateTime.Today.Date;
            foreach (var item in Emps)
            {
                if (item.EndDate != null && item.RemindarDays != null && item.RemindarDays.Value > 0)
                    if (item.EndDate.Date.AddDays(-item.RemindarDays.Value) == Today)
                        SendEmps.Add(new ExtendOrFinishContractViewModel { Id = item.Id, EmpId = item.EmpId, Renew = item.Renew, Email = item.Email, CompanyId = item.CompanyId });
            }


            // Second Job Renew Contract
            RenewContract();

            return SendEmps;
        }

        private void RenewContract()
        {
            DateTime DayBefore = DateTime.Today.AddDays(-1).Date;

            //   var renewEmps = context.Employements.Include("Person").Where(a => a.Status == 1 && a.AutoRenew && a.EndDate != null && a.EndDate == DayBefore).ToList();
            var renewEmps = (from e in context.Employements
                             where e.Status == 1 && e.EndDate != null && e.EndDate == DayBefore
                             join l in context.LookUpUserCodes on e.PersonType equals l.CodeId
                             where l.CodeName == "PersonType" && l.SysCodeId == 1
                             select e).ToList();

            foreach (var item in renewEmps)
            {
                context.Employements.Add(new Employement
                {
                    Allowances = item.Allowances,
                    AutoRenew = item.AutoRenew,
                    BenefitDesc = item.BenefitDesc,
                    Code = item.Code,
                    CompanyId = item.CompanyId,
                    ContIssueDate = item.ContIssueDate,
                    Curreny = item.Curreny,
                    DurInMonths = item.DurInMonths,
                    DurInYears = item.DurInYears,
                    EmpId = item.EmpId,
                    VacationDur = item.VacationDur,
                    FromCountry = item.FromCountry,
                    JobDesc = item.JobDesc,
                    Person = item.Person,
                    Company = item.Company,
                    Curr = item.Curr,
                    ToCountry = item.ToCountry,
                    TicketCnt = item.TicketCnt,
                    TicketAmt = item.TicketAmt,
                    SuggestJobId = item.SuggestJobId,
                    SuggestJob = item.SuggestJob,
                    Status = 1,
                    SpecialCond = item.SpecialCond,
                    Sequence = item.Sequence,
                    Salary = item.Salary,
                    Renew = true,
                    RemindarDays = item.RemindarDays,
                    Profession = item.Profession,
                    PersonType = item.PersonType,
                    StartDate = item.EndDate.Value.AddDays(1),
                    EndDate = item.EndDate.Value.AddMonths(item.DurInMonths).AddYears(item.DurInYears),
                });
                item.Status = 2;
                context.Employements.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }

            context.SaveChanges();
        }
        public void RemoveContext()
        {
            var changesEntity = context.ChangeTracker.Entries().Where(a => a.State == EntityState.Added || a.State == EntityState.Modified).ToList();
            foreach (var item in changesEntity)
            {
                context.Entry(item.Entity).State = EntityState.Detached;
            }
        }
     
        public bool CheckEmployment(Employement Emp,int EmpId, DateTime AssignDate)
        {
            var endDate = Emp?.EndDate == null ? new DateTime(2099, 1, 1) : Emp.EndDate;
            
             if (Emp != null && Emp.StartDate <= AssignDate && endDate >= AssignDate)
                return true;
            else
                 return false;          
        }

        #region Employment
        public bool CheckDocs(int CompanyId, int jobId, int EmpId)
        {
            var EmpDocs = context.CompanyDocsView.Where(cd =>  cd.CompanyId == CompanyId && cd.Source == "People" && cd.SourceId == EmpId).Select(c => c.TypeId == null ? 0 : c.TypeId.Value).ToList();
            var JobDocs = context.DocTypes.Where(a => (a.RequiredOpt == 1 && (!a.IsLocal || a.CompanyId == CompanyId)) || (a.RequiredOpt == 2 && a.Jobs.Any(b => b.Id == jobId))).Select(d => d.Id).ToList();
            if (JobDocs == null)
                return true;
            else
                return EmpDocs.Intersect(JobDocs).Count() >= JobDocs.Count();
        }

        public string GetFlexDataCheck(string tableName, int SourceId, int EmpId)
        {
            var query = (from J in context.FlexData
                         where J.TableName == tableName && J.SourceId == SourceId 
                         join fx in context.FlexColumns on new { J.PageId, J.ColumnName } equals new { fx.PageId, fx.ColumnName }
                         where fx.Required 
                         join E in context.FlexData on J.ColumnName equals E.ColumnName into g
                         from E in g.Where(a => a.TableName == "People" && a.SourceId == EmpId).DefaultIfEmpty()
                         .Where(e => e.Value == null || e.Value.CompareTo(J.Value) == -1)
                         select new
                         {
                             J.ColumnName, J.Value, EmployeeValue = E.Value
                         }).ToList();
            return string.Join(",", query.Select(a => " " + a.ColumnName));

        }
        public IQueryable<ManagerEmployeeDiagram> EmployeesDiagram(int CompanyId, string Culture)
        {
            var res = from di in context.Assignments
                      where (di.AssignDate <= DateTime.Today && di.EndDate >= DateTime.Today) && di.CompanyId == CompanyId
                 
                      select new ManagerEmployeeDiagram
                      {
                          Id = di.EmpId,
                          PositionName = HrContext.TrlsName(di.Job.Name, Culture),
                          colorSchema = "#4d5f77",
                          ParentId = di.ManagerId,
                          HasImage = di.Employee.HasImage,
                          Name = HrContext.TrlsName(di.Employee.Title + " " + di.Employee.FirstName + " " + di.Employee.Familyname, Culture)
                      };
            return res;
        }
     
        public IQueryable<ManagerEmployeeDiagram> GetManagers(int CompanyId,string culture)
        {
            var employees = from p in context.People
                            join a in context.Assignments on p.Id equals a.EmpId
                            where (a.CompanyId == CompanyId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today && a.IsDepManager)
                            select new ManagerEmployeeDiagram
                            {
                                Id = p.Id,
                                Name = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                                ParentId = a.ManagerId
                            };
            return employees;
        }
        public IQueryable<AssignmentGridViewModel> GetTermActiveEmployees(string culture, int Id, int CompanyId)
        {
            var employees = from p in context.People
                            join a in context.Employements on p.Id equals a.EmpId
                            where (a.Status==1 && a.CompanyId == CompanyId) || a.EmpId == Id
                            select new AssignmentGridViewModel
                            {
                                Id = p.Id,
                                Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                                PicUrl = (p.HasImage ? p.Id + ".jpeg" : "noimage.jpg"),
                                EmpStatus = HrContext.GetEmpStatus(p.Id)
                            };
            return employees;
        }
        public IQueryable<AssignmentGridViewModel> GetActiveEmployees(string culture, int Id, int CompanuId)
        {
            var employees = from a in context.Assignments
                            where (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today && a.CompanyId == CompanuId) || a.EmpId == Id
                            join p in context.People on a.EmpId equals p.Id
                            select new AssignmentGridViewModel
                            {
                                Id = p.Id,
                                Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                                PicUrl = (p.HasImage ? p.Id + ".jpeg" : "noimage.jpg"),
                                EmpStatus = HrContext.GetEmpStatus(p.Id)
                            };
            return employees;
        }
        public IQueryable<AssignmentGridViewModel> GetAllEmployees(string culture)
        {
            var employees = from p in context.People
                            join a in context.Assignments on p.Id equals a.EmpId
                            select new AssignmentGridViewModel
                            {
                                Id = p.Id,
                                Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture)
                            };
            return employees;
        }
        public IQueryable<AssignHistoryViewModel> GetHistoryAssignments(int CompanyId, string culture, int Id)
        {
            var history = from a in context.Assignments
                          where a.EmpId == Id && a.CompanyId == CompanyId
                          select new AssignHistoryViewModel
                          {
                              Id = a.Id,
                              AssignDate = a.AssignDate,
                              AssignStatus = HrContext.GetLookUpUserCode("Assignment", a.AssignStatus, culture),
                              Code = a.Code,
                              Department = HrContext.TrlsName(a.Department.Name, culture),
                              EndDate = a.EndDate == new DateTime(2099, 1, 1) ? (DateTime?)null : a.EndDate,
                              Job = HrContext.TrlsName(a.Job.Name, culture),
                              Position = HrContext.TrlsName(a.Position.Name, culture),
                              AssignCodeId = a.SysAssignStatus//HrContext.GetSysCodeId("Assignment", a.AssignStatus)
                                            
                          };
            return history;
        }
        public IQueryable<AssignmentGridViewModel> GetAssignments(string culture)
        {
            DateTime Today = DateTime.Today.Date;
            var Assignments = from p in context.People
                              join e in context.Employements on p.Id equals e.EmpId into g
                              from e in g.Where(s => s.Status == 1).DefaultIfEmpty()
                              join a in context.Assignments on e.EmpId equals a.EmpId into g1
                              from a in g1.Where(x => x.CompanyId == e.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                              join c in context.CompanyStructures on a.BranchId equals c.Id into g2
                              from c in g2.DefaultIfEmpty()
                              select new AssignmentGridViewModel
                              {
                                  Id = p.Id,
                                  EmpId = p.Id,
                                  CompanyId = e.CompanyId,
                                  Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                                  Code = e.Code,
                                  Department = HrContext.TrlsName(a.Department.Name, culture),
                                  Docs = HrContext.GetDocsCount(e.CompanyId, p.Id, a.JobId),
                                  EmployementDate = a != null ? a.AssignDate : e.StartDate,
                                  Job = HrContext.TrlsName(a.Job.Name, culture),
                                  Position = context.Positions.Where(z => z.Id == a.PositionId && z.JobId == a.JobId && z.DeptId == a.DepartmentId).Select(b => HrContext.TrlsName(b.Name, culture)).FirstOrDefault(),
                                  DepartmentId = a.DepartmentId,
                                  JobId = a.JobId,
                                  PositionId = a.PositionId,
                                  Supervisor = a.Position.Supervisor,
                                  ManagerId = a.ManagerId,
                                  Gender = p.Gender,
                                  Age = DateTime.Now.Year - p.BirthDate.Year,
                                  JoinedDate = p.JoinDate,
                                  PersonType = e.PersonType,
                                  Qualification = p.QualificationId,
                                  EmpStatus = HrContext.GetEmpStatus(p.Id),
                                  Location = HrContext.TrlsName(a.Location.Name, culture),
                                  HasImage = p.HasImage,
                                  BranchId = a.BranchId,
                                  BranchName = HrContext.TrlsName(c.Name, culture),
                                  SectorId = a.SectorId,
                                  CompanyName = HrContext.TrlsName(e.Company.Name, culture),
                                  LocationId = a.LocationId,
                                  PayGradeId = a.PayGradeId,
                                  PayrollId = a.PayrollId,
                                  GroupId = a.GroupId,
                                  Nationality = p.Nationality, //for documents
                                  IsDeptManger = a != null ? a.IsDepManager : false,
                                  Attachement = HrContext.GetDoc("EmployeePic", p.Id)
                              };
            return Assignments;
        }
        public IQueryable<SysCodeViewModel> BranchName(int DepId, string culture)
        {
            var query = "select top 1 B.Id As id, dbo.fn_TrlsName(B.Name, '" + culture + "') AS name from CompanyStructures A, CompanyStructures B where A.Id = 1056 and B.Id != 1056 and A.CompanyId = B.CompanyId and A.Sort LIKE B.Sort + '%' and (B.NodeType in (select C.CodeId from LookUpUserCodes C where C.CodeName = 'CompanyStructure' and C.SysCodeId = 2)) order by LEN(B.Sort) desc";
            return context.Database.SqlQuery<SysCodeViewModel>(query).AsQueryable();
        }
        public EmployementViewModel GetPersonTypeAndEmployee (int EmpId)
        {
            var query = (from e in context.Employements
                         where e.EmpId == EmpId && e.Status == 1
                         join sc in context.SystemCode on e.PersonType equals sc.SysCodeId into g
                         from sc in g.Where(a=>a.CodeName== "PersonType")
                         select new EmployementViewModel
                         {
                             EmpId = e.EmpId,
                             SysCodeId = sc.SysCodeId,
                             Code=e.Code ,
                             PersonType=e.PersonType,
                             StartDate =e.StartDate  
                         }).FirstOrDefault();
            return query;
        }
        public IQueryable<SysCodeViewModel> Sector(int DepId, string culture)
        {
            var query = "select top 1 B.Id As id, dbo.fn_TrlsName(B.Name, '" + culture + "') AS name from CompanyStructures A, CompanyStructures B where A.Id = 1056 and B.Id != 1056 and A.CompanyId = B.CompanyId and A.Sort LIKE B.Sort + '%' and (B.NodeType in (select C.CodeId from LookUpUserCodes C where C.CodeName = 'CompanyStructure' and C.SysCodeId = 3)) order by LEN(B.Sort) desc";
            return context.Database.SqlQuery<SysCodeViewModel>(query).AsQueryable();
        }
        public AssignmentFormViewModel GetAssignment(int EmpId,string culture)
        {
            var assignment = (from e in context.Employements
                              where e.EmpId == EmpId && (e.Status==1)
                              join a in context.Assignments on e.EmpId equals a.EmpId into g
                              from a in g.Where(c => (c.AssignDate <= DateTime.Today && c.EndDate >= DateTime.Today)).DefaultIfEmpty()
                              select new AssignmentFormViewModel
                              {
                                  Id = a == null ? 0 : a.Id,
                                  Code = e.Code,
                                  CompanyId = e.CompanyId,
                                  AssignDate = a == null ? e.StartDate : a.AssignDate,
                                  AssignStatus = a == null ? (short)0 : a.AssignStatus,
                                  CareerId = a.CareerId,
                                  DepartmentId = a == null ? 0 : a.DepartmentId,
                                  Employee = e.Person.Title + " " + e.Person.FirstName + " " + e.Person.Familyname,
                                  EmpId = e.EmpId,
                                  GroupId = a.GroupId,
                                  IsDepManager = a == null ? false : a.IsDepManager,
                                  JobId = a == null ? 0 : a.JobId,
                                  LocationId = a.LocationId,
                                  ManagerId = a.ManagerId,
                                  NoticePrd = a.NoticePrd,
                                  PayGradeId = a.PayGradeId,
                                  PayrollId = a.PayrollId,
                                  PositionId = a.PositionId,
                                  ProbationPrd = a.ProbationPrd,
                                  SalaryBasis = a.SalaryBasis,
                                  CreatedTime = a.CreatedTime,
                                  CreatedUser = a.CreatedUser,
                                  ModifiedTime = a.ModifiedTime,
                                  ModifiedUser = a.ModifiedUser,
                                  Payrolls = a.Payrolls,
                                  Employments = a.Employments,
                                  Jobs = a.Jobs,
                                  Locations = a.Locations,
                                  CompanyStuctures = a.CompanyStuctures,
                                  Positions = a.Positions,
                                  PayrollGrades = a.PayrollGrades,
                                  PeopleGroups = a.PeopleGroups,
                                  BranchId = a.BranchId,
                                  SectorId = a.SectorId,
                                  EmpTasks=a.EmpTasks,
                                  Performance = a.Performance,
                                  BranchName = a.BranchId != null ? HrContext.TrlsName(e.Company.Name,culture)+"-"+HrContext.TrlsName(context.CompanyStructures.FirstOrDefault(n=> n.Id == a.BranchId).Name,culture) : HrContext.TrlsName(e.Company.Name, culture)
                              }).FirstOrDefault();
           
            var assignmentObj = new AssignmentFormViewModel
            {
                Id = assignment.Id,
                Code = assignment.Code,
                CompanyId = assignment.CompanyId,
                AssignDate = assignment.AssignDate,
                AssignStatus = assignment.AssignStatus,
                CareerId = assignment.CareerId,
                DepartmentId = assignment.DepartmentId,
                Employee = assignment.Employee,
                EmpId = assignment.EmpId,
                GroupId = assignment.GroupId,
                IsDepManager = assignment.IsDepManager,
                JobId = assignment.JobId,
                SectorId = assignment.SectorId,
                BranchId = assignment.BranchId,
                BranchName = assignment.BranchName,
                LocationId = assignment.LocationId,
                ManagerId = assignment.ManagerId,
                NoticePrd = assignment.NoticePrd,
                PayGradeId = assignment.PayGradeId,
                PayrollId = assignment.PayrollId,
                PositionId = assignment.PositionId,
                ProbationPrd = assignment.ProbationPrd,
                SalaryBasis = assignment.SalaryBasis,
                CreatedTime = assignment.CreatedTime,
                CreatedUser = assignment.CreatedUser,
                ModifiedTime = assignment.ModifiedTime,
                ModifiedUser = assignment.ModifiedUser,
                EmpTasks=assignment.EmpTasks,
                Performance = assignment.Performance,
                IPayrolls = assignment.Payrolls == null ? null : assignment.Payrolls.Split(',').Select(int.Parse).ToList(),
                ICompanyStuctures = assignment.CompanyStuctures == null ? null : assignment.CompanyStuctures.Split(',').Select(int.Parse).ToList(),
                IEmployments = assignment.Employments == null ? null : assignment.Employments.Split(',').Select(int.Parse).ToList(),
                IPayrollGrades = assignment.PayrollGrades == null ? null : assignment.PayrollGrades.Split(',').Select(int.Parse).ToList(),
                IPositions = assignment.Positions == null ? null : assignment.Positions.Split(',').Select(int.Parse).ToList(),
                ILocations = assignment.Locations == null ? null : assignment.Locations.Split(',').Select(int.Parse).ToList(),
                IJobs = assignment.Jobs == null ? null : assignment.Jobs.Split(',').Select(int.Parse).ToList(),
                IPeopleGroups = assignment.PeopleGroups == null ? null : assignment.PeopleGroups.Split(',').Select(int.Parse).ToList()

            };

            return assignmentObj;
        }
        #endregion

        #region Candidates
        public IQueryable<JobCandidatesViewModel> ReadCandidates(string tableName, int sourceId, int companyId, string culture)
        {
            string sql = "select Q.Id, Q.EmpName, Q.Department, Q.JobName, Q.PosName, CAST(Q.Identical as varchar(10)) + '%' Identical from (select P.Id, dbo.fn_TrlsName(ISNULL(P.Title, '') + ' ' + P.FirstName + ' ' + P.Familyname, '" + culture + "') EmpName, dbo.fn_TrlsName(PS.Name, '" + culture + "') PosName, dbo.fn_TrlsName(Jobs.Name, '" + culture + "') JobName, dbo.fn_TrlsName(CS.Name, '" + culture + "') Department, (select Sum(R.Found) * 100/Count(R.JobValue) from (select J.ColumnName, J.Value JobValue, E.Value EmpValue, (CASE WHEN E.Value >= J.Value THEN 1 ELSE 0 END) Found from FlexData J left outer join FlexData E on (E.TableName = 'People' And E.ColumnName = J.ColumnName And E.SourceId = P.Id) inner join FlexColumns Fx on (J.PageId = Fx.PageId And J.ColumnName = Fx.ColumnName) where J.TableName = '" + tableName + "' And J.SourceId = " + sourceId + " And Fx.Required = 1) R) Identical from People P left outer join Assignments A on (A.EmpId = P.Id And (GETDATE() between A.AssignDate And A.EndDate) And A.CompanyId = "+ companyId +") left outer join Positions PS on (A.PositionId = PS.Id) left outer join Jobs on ( A.JobId = Jobs.Id) left outer join CompanyStructures CS  on (A.DepartmentId = CS.Id) inner join Employements EM on (P.Id = EM.EmpId And EM.Status = 1)) Q where Q.Identical > 0 Order by Q.Identical DESC";
            return context.Database.SqlQuery<JobCandidatesViewModel>(sql).AsQueryable();
        }

        public IQueryable<EmpIdenticalViewModel> ReadEmpIdentical(int empId, string tableName, int sourceId, string culture)
        {
            var sql = "select " + empId + " EmpId, J.ColumnName, dbo.fn_GetLookUpCode('Grade', CAST(J.Value as tinyint), '" + culture + "') JobValue, dbo.fn_GetLookUpCode('Grade', CAST(E.Value as tinyint), '" + culture + "') EmpValue, CAST((CASE WHEN E.Value >= J.Value THEN 1 ELSE 0 END) as tinyint) Found from FlexData J left outer join FlexData E on (E.TableName = 'People' And E.ColumnName = J.ColumnName And E.SourceId = " + empId + ") where J.TableName = '" + tableName + "' And J.SourceId = " + sourceId;
            return context.Database.SqlQuery<EmpIdenticalViewModel>(sql).AsQueryable();
        }

        #endregion

        public IQueryable<ExcelPeopleViewModel> GetPeopleExcel(string culture, int CompanyId)
        {
            string Ar = culture.Split('-')[0];
            var Assignments = from p in context.People
                              join e in context.Employements on p.Id equals e.EmpId into g
                              from e in g.Where(s => s.Status == 1 && s.CompanyId == CompanyId)
                              join a in context.Assignments on e.EmpId equals a.EmpId into g1
                              from a in g1.Where(x => x.CompanyId == e.CompanyId && x.AssignDate <= DateTime.Today && x.EndDate >= DateTime.Today).DefaultIfEmpty()
                              join c in context.CompanyStructures on a.BranchId equals c.Id into g2
                              from c in g2.DefaultIfEmpty()
                              select new ExcelPeopleViewModel
                              {
                                  Id = p.Id,
                                  Title = p.Title,
                                  localName = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                                  FirstName = p.FirstName,
                                  Fathername = p.Fathername,
                                  GFathername = p.GFathername,
                                  Familyname = p.Familyname,
                                  Gender = HrContext.GetLookUpCode("Gender", p.Gender, culture),
                                  NationalId = p.NationalId,
                                  IdIssueDate = p.IdIssueDate != null ? p.IdIssueDate.Value.ToString() : " ",
                                  Ssn = p.Ssn,
                                  JoinDate = p.JoinDate != null ? p.JoinDate.Value.ToString() : " ",
                                  StartExpDate = p.StartExpDate != null ? p.StartExpDate.Value.ToString() : " ",
                                  QualificationId = HrContext.TrlsName(p.Qualification.Name, culture),
                                  BirthDate = p.BirthDate.ToString(),
                                  BirthLocation = context.World.Where(a=>a.CityId == p.BirthCity && a.CountryId == p.BirthCountry && a.DistrictId == p.BirthDstrct).Select(f=> Ar =="ar"? f.NameAr : f.Name).FirstOrDefault(),
                                  Nationality = context.Countries.Where(a => a.Id == p.Nationality).Select(a => Ar == "ar" ? a.NationalityAr : a.Nationality).FirstOrDefault(),
                                  MaritalStat = p.MaritalStat != null ? HrContext.GetLookUpUserCode("MaritalStat", p.MaritalStat.Value, culture) : " ",
                                  TaxFamlyCnt = p.TaxFamlyCnt != null ? p.TaxFamlyCnt.ToString() : " ",
                                  BnftFamlyCnt = p.TaxFamlyCnt != null ? p.BnftFamlyCnt.ToString() : " ",
                                  Religion = p.Religion != null ? HrContext.GetLookUpCode("Religion", p.Religion.Value, culture) : " ",
                                  Address = p.AddressId != null ? p.Address.Address1 + " " + p.Address.Address2 + " " + p.Address.Address3 : " ",
                                  HostAddress = p.HoAddressId != null ? p.HoAddress.Address1 + " " + p.HoAddress.Address2 + " " + p.HoAddress.Address3 : "",
                                  Mobile = p.Mobile,
                                  Curr = e != null ? e.Curr != null ? e.Curreny.Name : " " : " ",
                                  InspectDate = p.InspectDate != null ? p.InspectDate.ToString() : " ",
                                  MedStatDate = p.MedStatDate != null ? p.MedStatDate.ToString() : " ",
                                  MedicalStat = p.MedicalStat != null ? HrContext.GetLookUpCode("MedicalStat", p.MedicalStat.Value, culture) : " ",
                                  KafeelId = p.KafeelId != null ? HrContext.TrlsName(p.Kafeel.Name, culture) : " ",
                                  HomeTel = p.HomeTel,
                                  EmergencyTel = p.EmergencyTel,
                                  WorkTel = p.WorkTel,
                                  WorkEmail = p.WorkEmail,
                                  OtherEmail = p.OtherEmail,
                                  MilitaryStat = p.MilitaryStat != null ? (p.MilitaryStat == 1 ? HrContext.TrlsMsg("Performed", culture) : (p.MilitaryStat == 2 ? HrContext.TrlsMsg("Exempt", culture) : (p.MilitaryStat == 3 ? HrContext.TrlsMsg("Deferred", culture) : HrContext.TrlsMsg("Underage", culture)))) : " ",
                                  MilStatDate = p.MilStatDate != null ? p.MilStatDate.Value.ToString() : " ",
                                  MilitaryNo = p.MilitaryNo,
                                  MilResDate = p.MilResDate != null ? p.MilResDate.Value.ToString() : " ",
                                  Rank = p.Rank != null ? HrContext.GetLookUpCode("Rank", p.Rank.Value, culture) : " ",
                                  MilCertGrade = p.MilCertGrade != null ? HrContext.GetLookUpCode("MilCertGrade", p.MilCertGrade.Value, culture) : " ",
                                  PassportNo = p.PassportNo,
                                  VisaNo = p.VisaNo,
                                  IssueDate = p.IssueDate != null ? p.IssueDate.Value.ToString() : " ",
                                  ExpiryDate = p.ExpiryDate != null ? p.ExpiryDate.Value.ToString() : " ",
                                  IssuePlace = p.IssuePlace,
                                  Profession = p.Profession,
                                  ProviderId = HrContext.TrlsName(p.Provider.Name, culture),
                                  BloodClass = p.BloodClass != null ? HrContext.GetLookUpCode("BloodClass", p.BloodClass.Value, culture) : " ",
                                  Recommend = p.Recommend,
                                  RecommenReson = p.RecommenReson != null ? HrContext.GetLookUpCode("RecommenReson", p.RecommenReson.Value, culture) : " ",
                                  LocationId = HrContext.TrlsName(p.Location.Name, culture),
                                  RoomNo = p.RoomNo,
                                  SubscripDate = p.SubscripDate != null ? p.SubscripDate.Value.ToString() : " ",
                                  BasicSubAmt = p.BasicSubAmt != null ? p.BasicSubAmt.ToString() : " ",
                                  VarSubAmt = p.VarSubAmt != null ? p.VarSubAmt.ToString() : " ",
                                  Sequence = e != null ? e.Sequence.ToString() : " ",
                                  PersonType = HrContext.GetLookUpUserCode("PersonType", e.PersonType, culture),
                                  AutoRenew = HrContext.TrlsMsg(e.AutoRenew.ToString(), culture),
                                  RemindarDays = e.RemindarDays != null ? e.RemindarDays.Value.ToString():" ",
                                  ContIssueDate = e != null ? e.ContIssueDate.ToString() : " ",
                                  DurInYears = e != null ? e.DurInYears.ToString() : " ",
                                  VacationDur = e != null ? e.VacationDur.ToString() : " ",
                                  DurInMonths = e != null ? e.DurInMonths.ToString() : " ",
                                  StartDate = e != null ? e.StartDate.ToString() : " ",
                                  EndDate = e != null ? (e.EndDate != null ? e.EndDate.Value.ToString() : " ") : " ",
                                  Salary = e != null ? e.Salary != null ? e.Salary.ToString() : " " : " ",
                                  Allowances = e != null ? e.Salary != null ? e.Allowances.ToString() : " " : " ",
                                  TicketAmt = e != null ? e.TicketAmt != null ? e.TicketAmt.ToString() : " " : " ",
                                  TicketCnt = e != null ? e.TicketCnt != null ? e.TicketCnt.ToString() : " " : " ",
                                  FromCountry = e != null ? e.FromCountry != null ? context.Countries.Where(a => a.Id == e.FromCountry).Select(a => Ar == "ar" ? a.NameAr : a.Name).FirstOrDefault() : " " : " ",
                                  ToCountry = e != null ? e.ToCountry != null ? context.Countries.Where(a => a.Id == e.ToCountry).Select(a => Ar == "ar" ? a.NameAr : a.Name).FirstOrDefault() : " " : " ",
                                  SuggestJob = e != null ? HrContext.TrlsName(e.SuggestJob.Name, culture) : " ",
                                  JobDesc = e != null ? e.JobDesc : " ",
                                  BenefitDesc = e != null ? e.BenefitDesc : " ",
                                  SpecialCond = e != null ? e.SpecialCond : " ",
                                  AssignDate = a != null ? a.AssignDate.ToString() : " ",
                                  AssignStatus = a != null ? HrContext.GetLookUpUserCode("Assignment", a.AssignStatus, culture) : " ",
                                  DepartmentId = a != null ? HrContext.TrlsName(a.Department.Name, culture) : " ",
                                  IsDepManager = a != null ? HrContext.TrlsMsg(a.IsDepManager.ToString(),culture) : " ",
                                  Code = e != null ? e.Code : " ",
                                  JobId = a != null ? HrContext.TrlsName(a.Job.Name, culture) : " ",
                                  AssignLocation = a != null ? a.LocationId != null ? HrContext.TrlsName(a.Location.Name, culture) : " " : " ",
                                  PositionId = a != null ? HrContext.TrlsName(a.Position.Name, culture) : " ",
                                  GroupId = a != null ? HrContext.TrlsName(a.PeopleGroup.Name, culture) : " ",
                                  PayrollId = a != null ? HrContext.TrlsName(a.Payroll.Name, culture) : " ",
                                  SalaryBasis = a != null ? a.SalaryBasis != null ? HrContext.GetLookUpUserCode("SalaryBasis", a.SalaryBasis.Value,culture) : " ":" ",
                                  PayGradeId = a != null ? HrContext.TrlsName(a.PayrollGrade.Name, culture) : " ",
                                  Performance = a != null ? a.Performance != null ? HrContext.GetLookUpCode("Performance", a.Performance.Value, culture) : " " : " ",
                                  CareerId = a != null ? a.CareerId != null ? HrContext.TrlsName(a.CareerPath.Name, culture) : " " : " ",
                                  ManagerId = a != null ? a.ManagerId != null ? context.People.Where(d => d.Id == a.ManagerId.Value).Select(k => HrContext.TrlsName(k.Title + " " + k.FirstName + " " + k.Familyname, culture)).FirstOrDefault() : " " : " ",
                                  ProbationPrd = a != null ? a.ProbationPrd.ToString() : " ",
                                  NoticePrd = a != null ? a.NoticePrd.ToString() : " ",
                                  EmpTasks = a != null ? a.EmpTasks != null ? (a.EmpTasks == 1 ? HrContext.TrlsMsg("Employee whose direct managed", culture) : HrContext.TrlsMsg("Use eligibility criteria", culture)) : " " : " ",
                                  EmpProfession = e.Profession != null ? e.Profession : " ",
                              };
            
            return Assignments;
        }
        #region Dashboard

        public IEnumerable CountEmpsByDepts(int[] depts, int companyId, string culture)
        {
            string sql = "select dbo.fn_TrlsName(CS.Name, '" + culture + "') [category],Cs.Id Id, Count(1) [value] from Assignments A, CompanyStructures CS "
                 + (depts == null ? ", CompanyStructures S where  S.CompanyId=" + companyId + " and  A.DepartmentId = S.Id" : "where A.DepartmentId = CS.Id")
                + " And A.CompanyId = " + companyId + "  and CS.CompanyId=" + companyId + "   and GETDATE() between A.AssignDate And A.EndDate "
                 + (depts == null ? "And S.Sort LIKE CS.Sort + '%' And Len(CS.Sort) = 5" : "And CS.Id in (" + string.Join(",", depts) + ")")
                + " Group by CS.Id, CS.Name";

            return context.Database.SqlQuery<ChartViewModel>(sql.ToString()).ToList();
        }

        public IEnumerable<ChartViewModel> EmployeesStatus(int[] depts, int CompanyId, string culture)
        {
            if (depts == null) depts = new int[0];

            var query = (from LR in context.LeaveRequests
                         where (LR.CompanyId == CompanyId && LR.ApprovalStatus == 6 && LR.ActualStartDate <= DateTime.Today.Date && LR.ActualEndDate >= DateTime.Today.Date)
                         join a in context.Assignments on LR.EmpId equals a.EmpId
                         where (depts.Count() == 0 ? true : depts.Contains(a.DepartmentId))
                         select new
                         {
                             LREmpId = LR.EmpId,
                             state = "11",
                         }).Distinct().Union
                         (from a in context.Assignments
                          where a.CompanyId == CompanyId && a.AssignDate <= DateTime.Today.Date && a.EndDate >= DateTime.Today.Date && (depts.Count() == 0 ? true : depts.Contains(a.DepartmentId))
                          select new
                          {
                              LREmpId = a.EmpId,
                              state = a.SysAssignStatus.ToString(),
                          }).Union(from e in context.Employements
                                   where e.CompanyId == CompanyId && e.Status == 1 && depts.Count() == 0
                                   select new
                                   {
                                       LREmpId = e.EmpId,
                                       state = "12",
                                   }).GroupBy(a => a.state).Select(x => new ChartViewModel
                                   {
                                       category = x.Key,
                                       value = x.Count(),
                                   }).OrderBy(s => s.value).ToList();
            int Count = 0;

            foreach (var item in query)
            {
                ChartViewModel tbl;
                switch (item.category)
                {
                    case "1":
                        tbl = query.FirstOrDefault(a => a.category == "1");
                        tbl.Month = Convert.ToInt32(query.Where(a=>a.category=="1").Select(c=>c.category).FirstOrDefault());
                        tbl.category = MsgUtils.Instance.Trls(culture, "Active");
                        tbl.color = "#B0D877";
                        Count += tbl.value;
                        break;
                    case "2":
                        tbl = query.FirstOrDefault(a => a.category == "2");
                        tbl.Month = Convert.ToInt32(query.Where(a => a.category == "2").Select(c => c.category).FirstOrDefault());
                        tbl.category = MsgUtils.Instance.Trls(culture, "Suspend");
                        tbl.color = "#797979";
                        Count += tbl.value;
                        break;
                    case "3":
                        tbl = query.FirstOrDefault(a => a.category == "3");
                        tbl.Month = Convert.ToInt32(query.Where(a => a.category == "3").Select(c => c.category).FirstOrDefault());
                        tbl.category = MsgUtils.Instance.Trls(culture, "Terminate");
                        tbl.color = "#d54c7e";
                        Count += tbl.value;
                        break;
                    case "11":
                        tbl = query.FirstOrDefault(a => a.category == "11");
                        tbl.Month = Convert.ToInt32(query.Where(a => a.category == "11").Select(c => c.category).FirstOrDefault());
                        tbl.category = MsgUtils.Instance.Trls(culture, "Leave");
                        tbl.color = "#ededed";
                        Count += tbl.value;
                        break;
                    case "0":
                        tbl = query.FirstOrDefault(a => a.category == "0");
                        tbl.Month = Convert.ToInt32(query.Where(a => a.category == "0").Select(c => c.category).FirstOrDefault());
                        tbl.category = MsgUtils.Instance.Trls(culture, "NotEmployee");
                        Count += tbl.value;
                        tbl.color = "antiquewhite";
                        break;
                    default:
                        tbl = query.FirstOrDefault(a => a.category == "12");
                        tbl.Month = Convert.ToInt32(query.Where(a => a.category == "12").Select(c => c.category).FirstOrDefault());
                        tbl.category = MsgUtils.Instance.Trls(culture, "NewEmployee");
                        tbl.value -= Count;
                        tbl.color = "#FFD700";
                        break;
                }
            }
            return query;

        }
        public IEnumerable CountEmpsByLocations(int companyId, string culture)
        {
            var query = (from a in context.Assignments
                         where a.CompanyId == companyId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today
                         join loc in context.Locations on a.LocationId.Value equals loc.Id
                         group a by new  { loc.Name , loc.Id } into g
                         select new ChartViewModel
                         {
                             category = HrContext.TrlsName(g.Key.Name, culture),
                             value = g.Count(),
                             Id=g.Key.Id
                             
                         }).ToList();
            return query;
        }
        public IEnumerable LocationsByDepts(int[] depts, int compantyId, string cultuer)
        {
            string sql = "select CS.Id, L.Id [EmpId], dbo.fn_TrlsName(CS.Name, '" + cultuer + "') [category], dbo.fn_TrlsName(L.Name, '" + cultuer + "') [myGroup], Count(1) [value] from Assignments A , CompanyStructures CS, Locations L "
                + (depts == null ? ", CompanyStructures S where S.CompanyId=" + compantyId + " and A.DepartmentId = S.Id" : "where  A.DepartmentId = CS.Id")
                + " And A.CompanyId = " + compantyId + " and CS.CompanyId="+compantyId+" And GETDATE() between A.AssignDate and A.EndDate And L.Id = A.LocationId "
                + (depts == null ? "And S.Sort LIKE CS.Sort + '%' And Len(CS.Sort) = 5" : "And CS.Id in (" + string.Join(",", depts) + ")")
                + " Group by CS.Id,L.Id, CS.Sort, CS.Name, L.Name";
            return context.Database.SqlQuery<ChartViewModel>(sql.ToString()).ToList();
        }
        public IEnumerable CountGenderByEmployment(int companyId,string culture)
        {
            var query = (from a in context.Employements
                         where a.CompanyId == companyId && a.Status == 1
                         join P in context.People on a.EmpId equals P.Id
                         group a by new { P.Gender, a.PersonType } into g
                         orderby g.Key.PersonType
                         select new ChartViewModel
                         {
                             category = HrContext.GetLookUpCode("Gender", g.Key.Gender, culture) + " - " + HrContext.GetLookUpUserCode("PersonType", g.Key.PersonType, culture),
                             value = g.Count(),
                             Id=g.Key.Gender,
                             EmpId=g.Key.PersonType,
                         }).ToList();
            return query;
        }
        public IEnumerable CountNationalityByEmployment(int companyId, string culture)
        {
            var query = (from a in context.Assignments
                         where a.CompanyId == companyId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today
                         join P in context.People on a.EmpId equals P.Id 
                         join c in context.Countries on P.Nationality equals c.Id 
                         group a by new { P.Nationality, NAR = c.NationalityAr, NEng = c.Nationality, id=c.Id } into g
                         select new ChartViewModel
                         {
                             // category =(g.Key.Nationality != null ? (culture == "ar-EG" ? g.Key.NAR : g.Key.NEng) : HrContext.TrlsName("other Nationality", culture)),
                             category = (culture == "ar-EG" ? g.Key.NAR : g.Key.NEng) ,
                             value = g.Count(),
                             Id=g.Key.id
                         }).ToList();
          
            return query;
        }
        public IEnumerable<PeoplesViewModel> GetPeopleWithNational(int NationaltyId,string culture,int companyId)
        {
            var query = (from a in context.Assignments
                         where a.CompanyId == companyId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today
                         join p in context.People on a.EmpId equals p.Id
                         where p.Nationality == NationaltyId
                         select new PeoplesViewModel
                         {
                             localName = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             Id = p.Id,
                         }).ToList();
            return query;
        }
        public IEnumerable<PeoplesViewModel> GetPeopleWithLoc(int LocId,int companyId, string culture)
        {
            var query = (from a in context.Assignments
                         where a.CompanyId == companyId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today &&  a.LocationId.Value==LocId
                         join p in context.People on a.EmpId equals p.Id
                         select new PeoplesViewModel
                         {
                             localName = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             Id = p.Id,
                         }).ToList();
            return query;
        }
        public IEnumerable<PeoplesViewModel> GetEmpStutes(int[] depts, int status, int companyId, string culture)
        {
            if (depts == null) depts = new int[0];

            var query = (from LR in context.LeaveRequests
                         where (LR.CompanyId == companyId && LR.ApprovalStatus == 6 && LR.ActualStartDate <= DateTime.Today.Date && LR.ActualEndDate >= DateTime.Today.Date)
                         join a in context.Assignments on LR.EmpId equals a.EmpId
                         where (depts.Count() == 0 ? true : depts.Contains(a.DepartmentId))
                         join p in context.People on LR.EmpId equals p.Id
                         select new
                         {
                             LREmpId = LR.EmpId,
                             LocalName = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             state = "11",
                         })
                         .Distinct().Union
                         (from a in context.Assignments
                          where a.CompanyId == companyId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today && (depts.Count() == 0 ? true : depts.Contains(a.DepartmentId))
                          select new
                          {
                              LREmpId = a.EmpId,
                              LocalName = HrContext.TrlsName(a.Employee.Title + " " + a.Employee.FirstName + " " + a.Employee.Familyname, culture),
                              state = a.SysAssignStatus.ToString(),
                          }).Union(from e in context.Employements
                                   where e.CompanyId == companyId && e.Status == 1 && depts.Count() == 0
                                   join p in context.People on e.EmpId equals p.Id
                                   select new
                                   {
                                       LREmpId = e.EmpId,
                                       LocalName = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                                       state = "12",
                                   }).ToList();

            return query.Where(s => int.Parse(s.state) == status && (s.state != "12" ? true : query.Where(q => q.state != "12" && q.LocalName == s.LocalName).Count() == 0) )
                .Select(s => new PeoplesViewModel { Id = s.LREmpId, localName = s.LocalName }).ToList();
        }

        public IEnumerable<PeoplesViewModel> GetPeopleGenderPersonType(int genderId,int personType, int companyId, string culture)
        {
            var query = (from a in context.Employements
                         where a.CompanyId == companyId && a.Status == 1 && a.PersonType == personType
                         join p in context.People on a.EmpId equals p.Id
                         where p.Gender== genderId
                         select new PeoplesViewModel
                         {
                             localName = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             Id = p.Id,
                         }).ToList();
            return query;
        }
        public IEnumerable<PeoplesViewModel> GetPeopleDepts(bool isDefault, int DeptId,int companyId, string culture)
        {
            string sql = "select dbo.fn_TrlsName(ISNULL(P.Title, '')+' '+P.FirstName+' '+P.Familyname, '" + culture+"') localName, A.Id from Assignments A, People P , CompanyStructures CS "
                + (isDefault ? ", CompanyStructures S where A.DepartmentId = S.Id  and  S.CompanyId=" + companyId + "" : " where  A.DepartmentId = CS.Id ")
                + " And A.CompanyId = "+companyId+ "  and  CS.CompanyId=" + companyId+" and GETDATE() between A.AssignDate And A.EndDate And P.Id = A.EmpId And "
                + (isDefault ? "S.Sort LIKE CS.Sort + '%' And Len(CS.Sort) = 5" : "A.DepartmentId=" + DeptId);
            var query = context.Database.SqlQuery<PeoplesViewModel>(sql).ToList();
            return query;
        }
        public IEnumerable<PeoplesViewModel> GetPeopleGenderDepts(bool isDefault,int DeptId, int GenderID, int companyId, string culture)
        {
            string sql = "select CS.Id, dbo.fn_TrlsName(CS.Name, '"+culture+ "') [category],dbo.fn_TrlsName(ISNULL(P.Title, '')+' '+P.FirstName+' '+P.Familyname, '" + culture + "') localName ,dbo.fn_GetLookUpCode('Gender', P.Gender ,'" + culture + "') [Title] from Assignments A, CompanyStructures CS, People P "
                            + (isDefault ? ", CompanyStructures S where A.DepartmentId = S.Id   and S.CompanyId=" + companyId + " And S.Sort LIKE CS.Sort + '%' AND Len(CS.Sort) = 5 " : "where A.DepartmentId = CS.Id AND CS.Id = " + DeptId)
                            + " And A.EmpId = P.Id And A.CompanyId = "+companyId+ " and CS.CompanyId=" + companyId + "  and GETDATE() between A.AssignDate And A.EndDate And P.Gender =" + GenderID;
            var query = context.Database.SqlQuery<PeoplesViewModel>(sql).ToList();
            return query;
        }
        public IEnumerable<PeoplesViewModel> GetPeopleAgeDepts(bool isDefault, int DeptId, string AgeRang, int companyId, string culture)
        {
            string sql = "select C.Id, C.localName from (select B.Id, B.localName, (case when B.Age < 21 then '- 21' when B.Age between 21 and 30 then '21 - 30' when B.Age between 30 and 40 then '30 - 40' when B.Age between 40 and 50 then '40 - 50' when B.Age between 50 and 60 then '50 - 60' when B.Age between 60 and 65 then '60 - 65' when B.Age > 65 then '+ 65' end) Ages, B.Dept from (select CS.Id, dbo.fn_TrlsName(P.Title+' '+P.FirstName+' '+P.Familyname, '"+ culture+ "') localName , DATEDIFF(year, P.BirthDate, GETDATE()) Age, dbo.fn_TrlsName(CS.Name, '"+culture+"') Dept from Assignments A, People P, CompanyStructures CS "
                + (isDefault ? ", CompanyStructures S  " : "") + " where A.EmpId = P.Id And A.CompanyId = "+companyId+ " and CS.CompanyId=" + companyId + " And (GETDATE() between A.AssignDate And A.EndDate)"
                + (isDefault ? " And S.CompanyId=" + companyId + " And A.DepartmentId = S.Id And S.Sort LIKE CS.Sort + '%' And Len(CS.Sort) = 5 ": "And A.DepartmentId = CS.Id And CS.Id = " + DeptId) + 
                " ) as B) as C where C.Ages = '" + AgeRang + "'";
            var query = context.Database.SqlQuery<PeoplesViewModel>(sql).ToList();
            return query;
        }
        public IEnumerable<PeoplesViewModel> GetPeopleLocDepts(bool isDefault, int DeptId, int LocId, int companyId, string culture)
        {
            string sql = "select CS.Id, dbo.fn_TrlsName(CS.Name, '" + culture + "') [category],dbo.fn_TrlsName(ISNULL(P.Title, '')+' '+P.FirstName+' '+P.Familyname, '" + culture + "') localName ,dbo.fn_GetLookUpCode('Gender', P.Gender ,'" + culture + "') [Title] from Assignments A, CompanyStructures CS, People P ,Locations L "
                            + (isDefault ? ", CompanyStructures S where A.DepartmentId = S.Id and S.CompanyId=" + companyId + "  And L.Id = A.LocationId And S.Sort LIKE CS.Sort + '%' AND Len(CS.Sort) = 5 " : "where A.DepartmentId = CS.Id AND L.Id = A.LocationId AND CS.Id = " + DeptId)
                            + " And A.EmpId = P.Id And A.CompanyId = " + companyId + " and CS.CompanyId=" + companyId + " and GETDATE() between A.AssignDate And A.EndDate And L.Id =" + LocId;

            var query = context.Database.SqlQuery<PeoplesViewModel>(sql).ToList();
            return query;
        }
        public IEnumerable<PeoplesViewModel> GetPeopleAge(string ageRange,int companyId, string culture)
        {
            string sql = "select C.Id, C.LocalName, C.Age from (select (case when B.Age < 21 then '- 21' when B.Age between 21 and 30 then '21 - 30' when B.Age between 30 and 40 then '30 - 40' when B.Age between 40 and 50 then '40 - 50' when B.Age between 50 and 60 then '50 - 60' when B.Age between 60 and 65 then '60 - 65' when B.Age > 65 then '+ 65' end)Ages , B.Id, B.LocalName, B.Age from (select dbo.fn_TrlsName(ISNULL(P.Title, '') +' ' + P.FirstName + ' ' + P.Familyname, '" + culture + "') LocalName, P.Id, DATEDIFF(year, P.BirthDate, GETDATE()) Age from Assignments A, People P where A.EmpId = P.Id And A.CompanyId = " + companyId + "  And (GETDATE() between A.AssignDate And A.EndDate)) as B) as C where C.Ages = '" + ageRange + "'";
            var query = context.Database.SqlQuery<PeoplesViewModel>(sql).ToList();
            return query;
        }
        public IEnumerable GenderByDepts(int[] depts, int companyId, string culture)
        {
            string sql = "select CS.Id,P.Gender, dbo.fn_TrlsName(CS.Name, '" + culture + "') [category], dbo.fn_GetLookUpCode('Gender', P.Gender ,'" + culture + "') [myGroup],Count(1) [value] from Assignments A, CompanyStructures CS, People P "
                + (depts == null ? ", CompanyStructures S where A.DepartmentId = S.Id And S.Sort LIKE CS.Sort + '%' AND Len(CS.Sort) = 5" : "where A.DepartmentId = CS.Id AND CS.Id In (" + string.Join(",", depts) + ")")
                + " And A.EmpId = P.Id And A.CompanyId = " + companyId + " and CS.CompanyId=" + companyId + " and GETDATE() between A.AssignDate And A.EndDate group by CS.Id, CS.Name, P.Gender";

            return context.Database.SqlQuery<ChartViewModel>(sql).ToList();
        }
        public IEnumerable EmployeesAges(int companyId, string cultuer)
        {
            string sql = "select count(1) [value], C.Ages category from (select (case when B.Age < 21 then '- 21' when B.Age between 21 and 30 then '21 - 30' when B.Age between 30 and 40 then '30 - 40' when B.Age between 40 and 50 then '40 - 50' when B.Age between 50 and 60 then '50 - 60' when B.Age between 60 and 65 then '60 - 65' when B.Age > 65 then '+ 65' end) Ages from (select P.Id, DATEDIFF(year, P.BirthDate, GETDATE()) Age from Assignments A, People P where A.EmpId = P.Id And A.CompanyId = " + companyId + " And (GETDATE() between A.AssignDate And A.EndDate)) as B) as C group by C.Ages";
            return context.Database.SqlQuery<ChartViewModel>(sql).AsQueryable();
        }
        public IEnumerable AgesByDepts(int[] depts, int companyId, string culture)
        {
            string sql = "select C.DeptId [Id],count(1) [value], C.Ages [myGroup], C.Dept category from (select (case when B.Age < 21 then '- 21' when B.Age between 21 and 30 then '21 - 30' when B.Age between 30 and 40 then '30 - 40' when B.Age between 40 and 50 then '40 - 50' when B.Age between 50 and 60 then '50 - 60' when B.Age between 60 and 65 then '60 - 65' when B.Age > 65 then '+ 65' end) Ages, B.Dept , B.DeptId from (select CS.Id [DeptId], P.Id, DATEDIFF(year, P.BirthDate, GETDATE()) Age, dbo.fn_TrlsName(CS.Name, '" + culture +"') Dept "
                + " from Assignments A, People P, CompanyStructures CS" + (depts == null ? ", CompanyStructures S " : " ")
                + " where A.EmpId = P.Id And A.CompanyId = " + companyId + " and CS.CompanyId=" + companyId + "  And (GETDATE() between A.AssignDate And A.EndDate) And A.DepartmentId = "
                + (depts == null ? "S.Id And S.Sort LIKE CS.Sort + '%' And Len(CS.Sort) = 5 " : "CS.Id And CS.Id in ("+ string.Join(",", depts) +") ")
                + " ) as B) as C group by C.Ages, C.Dept , C.DeptId";
            return context.Database.SqlQuery<ChartViewModel>(sql).AsQueryable();
        }
        public IEnumerable<EmpsInYearViewModel> GetActiveByMonth(int CompanyId)
        {
            //join t in context.Terminations on e.TerminationId equals t.Id into g1
            //             from t in g1.DefaultIfEmpty()
            //             join c in context.LookUpUserCodes on t.TermReason equals c.CodeId into g2
            //             from c in g2.Where(a => a.CodeName == "Termination" && (a.SysCodeId != 2 || a.SysCodeId != 3))

            var query = (from e in context.Employements
                         where (e.StartDate.Year <= DateTime.Now.Year) && (e.EndDate == null || e.EndDate.Value.Year >= DateTime.Now.Year)
                         select new 
                         {
                             Enddate = (e.EndDate == null ? new DateTime(2099, 1, 1) : e.EndDate),
                             EmpId = e.EmpId,
                             EmployeesStatus = e.Status
                         } into g
                         group g by new { g.Enddate.Value.Month, g.Enddate.Value.Year } into g1
                         select new EmpsInYearViewModel
                         {
                             Active = g1.Where(a => a.EmployeesStatus == 1).Count(),
                             Month = g1.Key.Month,
                             All = g1.Count(),
                             Year = g1.Key.Year,
                             InActive = g1.Where(a => a.EmployeesStatus != 1).Count()
                         }).ToList();

            List<EmpsInYearViewModel> Emps = new List<EmpsInYearViewModel>();
            for (int i = 1; i <= 12; i++)
            {
                Emps.Add(new EmpsInYearViewModel
                {
                    Month=i
                });
            }

            for (int i = 1; i <= Emps.Count; i++)
            {
                var item = Emps.Where(a => a.Month == i).FirstOrDefault();
                var iteminQuery = query.Where(a => a.Month == i && a.Year == 2017).FirstOrDefault();
                if (item != null && iteminQuery != null)
                {
                    item.Active += iteminQuery.Active;
                    item.All += iteminQuery.All;
                }
                var ItemBiggerThanYear = query.Where(a => a.Year > 2017).ToList();
                if(ItemBiggerThanYear.Count != 0)
                {
                    foreach (var item1 in ItemBiggerThanYear)
                    {
                        item.Active =item1.Active+item1.InActive;
                        item.All += item.All +item1.Active+item1.InActive;
                    }
                }
                var ItemInYearAndBiggerMonth = query.Where(a => a.Year == 2017 && a.Month > i).ToList();
                if(ItemInYearAndBiggerMonth.Count != 0)
                {
                    foreach (var item2 in ItemInYearAndBiggerMonth)
                    {
                        item.Active = item.Active+item2.InActive+item2.Active;
                        item.All = item.Active + item2.InActive + item2.Active;
                    }
                }
            }

            //for (int i = 0; i < query.Count; i++)
            //{

            //    var item = query.ElementAtOrDefault(i);

            //    if ((item.Year > 2017) && (item.Year - 2017 != 1))
            //    {
            //        var inMon = query.Where(a => a.Month == item.Month).FirstOrDefault();
            //        inMon.Active += item.InActive;
            //        inMon.All += item.InActive;

            //        for (int j = 1; j <= 12; j++)
            //        {
            //            if (j != inMon.Month)
            //            {
            //                var inQu = query.Where(a => a.Month == j).FirstOrDefault();
            //                if (inQu == null)
            //                {
            //                    query.Add(new EmpsInYearViewModel
            //                    {
            //                        Active = item.Active,
            //                        All = item.All,
            //                        InActive = item.InActive,
            //                        Month = item.Month,
            //                        Year = 2017,
            //                    });
            //                }
            //                else
            //                {
            //                    inQu.Active += item.Active;
            //                    inQu.All += item.All;
            //                }
            //            }
            //        }
            //    }
            //    else if ((item.Year > 2017) && (item.Year - 2017 == 1))
            //    {
            //        var inMon = query.Where(a => a.Month == item.Month).FirstOrDefault();
            //        inMon.Active += item.InActive;
            //        inMon.All += item.InActive;
            //        int Mon = item.Month;
            //        for (int j = 1; j <= 12; j++)
            //        {
            //            if (j != inMon.Month || Mon == 0)
            //            {
            //                var inQu = query.Where(a => a.Month == j).FirstOrDefault();
            //                if (inQu == null)
            //                {
            //                    query.Add(new EmpsInYearViewModel
            //                    {
            //                        Active = item.Active,
            //                        All = item.All,
            //                        InActive = item.InActive,
            //                        Month = item.Month,
            //                        Year = 2017,
            //                    });
            //                }
            //                else
            //                {
            //                    inQu.Active += item.Active;
            //                    inQu.All += item.All;
            //                }
            //            }
            //            Mon--;

            //        }
            //    }
            //    else
            //    {
            //        var inMon = query.Where(a => a.Month == item.Month).FirstOrDefault();
            //        inMon.Active += item.InActive;
            //        inMon.All += item.InActive;
            //        int Mon = item.Month;
            //        for (int j = 1; j <= 12; j++)
            //        {
            //            if (j != inMon.Month)
            //            {
            //                if (Mon > 0)
            //                {
            //                    var inQu = query.Where(a => a.Month == j).FirstOrDefault();
            //                    if (inQu == null)
            //                    {
            //                        query.Add(new EmpsInYearViewModel
            //                        {
            //                            Active = item.Active,
            //                            All = item.All,
            //                            InActive = item.InActive,
            //                            Month = item.Month,
            //                            Year = 2017,
            //                        });
            //                    }
            //                    else
            //                    {
            //                        inQu.Active += item.Active;
            //                        inQu.All += item.All;
            //                    }
            //                }
            //            }
            //            else
            //                break;

            //            Mon--;

            //        }
            //    }
            //}
            return Emps;
        }

        #region Manager DashBoard
        //ManagerEmployeeTask

        #endregion


        #region commented
        //public IEnumerable<ChartViewModel> CountLenghtofService(int companyId, string culture)
        //{
        //    var query = "select sum(G.cnt) [value], G.yearlength [category]from(select count(1) cnt, (case when B.lenght between 0 and 2 then '0 - 2' when B.lenght between 3 and 5 then '3 - 5'when B.lenght between 6 and 10 then '6 - 10'when B.lenght between 11 and 15 then '11 - 15'when B.lenght between 16 and 20 then '16 - 20'when B.lenght > 20 then '+20'end) yearlength from (select E.EmpId, DATEDIFF(YEAR, Max( E.StartDate), ISNULL(Max(E.EndDate), GETDATE())) as [lenght]  from People P inner join Employements E  on E.EmpId = p.Id where E.CompanyId="+companyId+" group by E.EmpId) as B group by B.lenght ) as G group by G.yearlength";
        //    var newquery = context.Database.SqlQuery<ChartViewModel>(query).AsQueryable().ToList();
        //    return newquery.Select(s => new ChartViewModel { value = ((s.value * 100) / newquery.Sum(a => a.value)), category = s.category }).ToList();
        //}

        //public IEnumerable HeadCountByJob(int CompanyId,string culture)
        //{
        //    var query = (from p in context.Positions
        //                 where (p.StartDate <= DateTime.Today && (p.EndDate >= DateTime.Today || p.EndDate == null)) && p.CompanyId == CompanyId
        //                 join j in context.Jobs on p.JobId equals j.Id 
        //                 group p by j.Name into g
        //                 select new
        //                 {
        //                     job = HrContext.TrlsName(g.Key, culture),
        //                     HeadCount = g.Sum(d=>d.Headcount)
        //                 }).ToList();

        //    return query.Select(a => new { job = a.job, HeadCount = a.HeadCount, max = query.Max(b => b.HeadCount) }).ToList();
        //}
        #endregion

        #endregion

        #region EmpDataMerge
        public string[] GetEmpMergeData(int EmpId, int CompanyId, string Culture)
        {
            var cul = Culture.Split('-')[0];
            var query = (from p in context.People
                         where p.Id == EmpId
                         join e in context.Employements on p.Id equals e.EmpId into g
                         from e in g.Where(a => a.Status == 1).DefaultIfEmpty()
                         join a in context.Assignments on p.Id equals a.EmpId into g1
                         from a in g1.DefaultIfEmpty()
                         select new DocMergeViewModel
                         {
                             _0_EmployeeName = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, Culture),
                             _11_Salary = e.Salary != null ? e.Salary.Value.ToString() : " ",
                             _12_Allowances = e.Allowances != null ? e.Allowances.Value.ToString() : " ",
                             _13_Currency = e.Curr == null ? " ":e.Curr,
                             _14_TicketCnt = e.TicketCnt != null ? e.TicketCnt.Value.ToString() : " ",
                             _15_TicketAmt = e.TicketAmt != null ? e.TicketAmt.Value.ToString() : " ",
                             _16_FromCountry = e.FromCountry != null ? context.Countries.Where(c => c.Id == e.FromCountry.Value).Select(d => (Culture.Substring(0, 2) == "ar" ? d.NameAr : d.Name)).FirstOrDefault() : " ",
                             _17_ToCountry = e.ToCountry != null ? context.Countries.Where(c => c.Id == e.ToCountry.Value).Select(d => (Culture.Substring(0, 2) == "ar" ? d.NameAr : d.Name)).FirstOrDefault() : " ",
                             _10_Job = a != null ? HrContext.TrlsName(a.Job.Name, Culture) : " ",
                             _18_SeasonHolidayPeriod = e.VacationDur != null ? e.VacationDur.Value.ToString() : " ",
                             _19_EmploymentDay = e.StartDate.Day.ToString(),
                             _1_NationalId =p.NationalId != null ? p.NationalId:" ",
                             _2_NationalIdDate = p.IdIssueDate ,
                             _3_EmployeeAddress = p.Address != null ? p.Address.Address1 + " " + p.Address.Address2 + " " + p.Address.Address3 + "/" + p.Address.City.Name + "/" + p.Address.Country.Name : " ",
                             _4_PassportNo =p.PassportNo != null ? p.PassportNo:" ",
                             _5_PassportIssueDate = p.IssueDate,
                             _9_EmploymentEndDate = e.EndDate,
                             _8_EmploymentStartDate = e.StartDate,
                             _7_ContractPeriod = e.DurInYears.ToString()+""+HrContext.TrlsMsg("Year", Culture) + " " + HrContext.TrlsMsg("And", Culture) + " " + e.DurInMonths.ToString() + "" + HrContext.TrlsMsg("Month", Culture),
                             _6_AddressingNo = p.NationalId != null ? p.NationalId : " ",
                             _20_SuggestedJob = a != null ? a.JobId.ToString() : " ",
                             _21_Location = a.LocationId != null ? HrContext.TrlsName(a.Location.Name, Culture) : " ",
                             _22_Mobile = p.Mobile != null ? p.Mobile : " ",
                             _23_HomeTelephone = p.HomeTel != null ? p.HomeTel : " ",
                             _24_Kafel = p.KafeelId != null ? HrContext.TrlsName(p.Kafeel.Name, Culture) : " ",
                             _25_Department = HrContext.TrlsName(a.Department.Name, Culture),
                             _26_AssignDate = a.AssignDate,
                             _27_Position = a.PositionId != null ? HrContext.TrlsName(a.Position.Name, Culture) : " ",
                             _28_AssignStatus = HrContext.GetLookUpUserCode("Assignment", a.AssignStatus, Culture),
                             _29_PayrollGrad = a.PayGradeId != null ? HrContext.TrlsName(a.PayrollGrade.Name, Culture) : " ",
                             _30_CareerPath = a.CareerId != null ? HrContext.TrlsName(a.CareerPath.Name, Culture) : " ",
                             _31_JoinWorkingPeriod = p.JoinDate != null ? p.JoinDate.ToString() : " ",
                             _32_EmployWorkingPeriod = e != null ? e.StartDate.ToString() : " ",
                             _33_Nationality = p.Nationality != null ? context.Countries.Where(d => d.Id == p.Nationality).Select(m => (cul == "ar" ? m.NameAr : m.Name)).FirstOrDefault() : " ",
                             _34_CompanyName = HrContext.TrlsName(context.Companies.Where(c => c.Id == CompanyId).Select(m => m.Name).FirstOrDefault(), Culture),
                             _35_Gender = HrContext.GetLookUpCode("Gender", p.Gender, Culture),
                             _36_ExpPeriod = p.StartExpDate != null ? p.StartExpDate.Value.ToString() : " ",
                             _37_LastQual = HrContext.TrlsName(p.Qualification.Name, Culture),
                             _38_BirthDate = p.BirthDate,
                             _39_MaritalStatus = p.MaritalStat != null ? HrContext.GetLookUpUserCode("MaritalStat", p.MaritalStat.Value, Culture) : " ",
                             _40_Religion = p.Religion != null ? HrContext.GetLookUpCode("Religion", p.Religion.Value, Culture):" ",
                             _41_MilitaryStatus = p.MilitaryStat != null ? p.MilitaryStat.Value.ToString() : " ",
                             _42_ContractDate = e.ContIssueDate,
                             _43_ContractDay = e.ContIssueDate.Day.ToString(),
                             EmpTrainig = context.PeopleTraining.Where(t => t.EmpId == EmpId).Select(t => t.CourseTitle).ToList(),
                             EmpQual = context.PeopleQuals.Where(t => t.EmpId == EmpId).Select(t => t.Qualification.Name).ToList(),

                         }).FirstOrDefault();


            string day = query._19_EmploymentDay;
            string ContDay = query._43_ContractDay;
            string Milt = query._41_MilitaryStatus;

            string Sun = MsgUtils.Instance.Trls(Culture, "Sunday");
            string Mon = MsgUtils.Instance.Trls(Culture, "Monday");
            string Tus = MsgUtils.Instance.Trls(Culture, "Tuseday");
            string Wen = MsgUtils.Instance.Trls(Culture, "Wednesday");
            string Thu = MsgUtils.Instance.Trls(Culture, "Thursday");
            string Fri = MsgUtils.Instance.Trls(Culture, "Friday");
            string Sat = MsgUtils.Instance.Trls(Culture, "Saturday");

            query._19_EmploymentDay = (day == "0" ? Sun : (day == "1" ? Mon : (day == "2" ? Tus : (day == "3" ? Wen : (day == "4" ? Thu : (day == "5" ? Fri : Sat))))));
            query._43_ContractDay = (ContDay == "0" ? Sun: (ContDay == "1" ? Mon : (ContDay == "2" ? Tus : (ContDay == "3" ? Wen : (ContDay == "4" ? Thu : (ContDay == "5" ? Fri : Sat))))));
            query._44_TrainingCrs = query.EmpTrainig.Count != 0 ? string.Join(",", query.EmpTrainig) : " ";
            query._45_Qualifications = query.EmpQual.Count != 0 ? string.Join(",", query.EmpQual) : " ";

            query._41_MilitaryStatus = (Milt == "1" ? MsgUtils.Instance.Trls(Culture, "Performed") : (Milt == "2" ? MsgUtils.Instance.Trls(Culture, "Exempt") : (Milt == "3" ? MsgUtils.Instance.Trls(Culture, "Deferred") : (Milt == "4" ? MsgUtils.Instance.Trls(Culture, "Underage") : " "))));
            query._36_ExpPeriod = (query._36_ExpPeriod != " " ? (Convert.ToInt32((DateTime.Now.Subtract(Convert.ToDateTime(query._36_ExpPeriod)).TotalDays) / 365.25)).ToString() : query._36_ExpPeriod);
            query._31_JoinWorkingPeriod = (query._31_JoinWorkingPeriod != " " ? (Convert.ToInt32((DateTime.Now.Subtract(Convert.ToDateTime(query._31_JoinWorkingPeriod)).TotalDays) / 365.25)).ToString() : query._31_JoinWorkingPeriod);
            query._32_EmployWorkingPeriod = (query._32_EmployWorkingPeriod != " " ? (Convert.ToInt32((DateTime.Now.Subtract(Convert.ToDateTime(query._32_EmployWorkingPeriod)).TotalDays) / 365.25)).ToString() : query._32_EmployWorkingPeriod);
            string[] arr = new string[46];
            if (query != null)
            {
                var ObjProp = query.GetType().GetProperties();
                for (int i = 0; i < ObjProp.Length; i++)
                {
                    if (ObjProp[i].PropertyType.Name != "List`1")
                    {
                        int index = Convert.ToInt32(ObjProp[i].Name.Split('_')[1]);
                        var p = ObjProp[index].GetValue(query);
                        if (ObjProp[index].PropertyType.Name == "Nullable`1")
                            arr[index] = p != null ? p.ToString().Split(' ')[0] : " ";

                        else
                            arr[index] = p != null ? p.ToString() : " ";
                    }
                }
            }
            return arr;
        }

        #endregion

        ///Cancel Leave
        public void CancelLeaveAssignState(LeaveRequest request, string UserName, byte version, string culture)
        {
            LeaveType type = context.LeaveTypes.Where(t => t.Id == request.TypeId).FirstOrDefault();
            if (type.AssignStatus != null)
            {
                var EmpAssignments = Find(a => a.EmpId == request.EmpId);

                Assignment changed = EmpAssignments.Where(e => e.EndDate == request.ActualStartDate.Value.AddDays(-1)).FirstOrDefault();
                changed.EndDate = new DateTime(2099, 1, 1);
                changed.ModifiedUser = UserName;
                changed.ModifiedTime = DateTime.Now;
                Attach(changed);
                context.Entry(changed).State = EntityState.Modified;

                Assignment leave = EmpAssignments.Where(e => e.AssignStatus == type.AssignStatus && e.AssignDate == request.ActualStartDate.Value).FirstOrDefault();
                if(leave != null) Remove(leave);

                if (type.AutoChangStat == 1)
                {
                    Assignment returned = EmpAssignments.Where(e => e.AssignDate == request.ActualEndDate.Value.AddDays(1)).FirstOrDefault();
                    if(returned != null) Remove(returned);
                }

                var assignmentCodes = GetLookUpUserCodes("Assignment", culture);
                
                AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "AssignStatus",
                    CompanyId = request.CompanyId,
                    ObjectName = "AssignmentsForm",
                    SourceId = changed.Id.ToString(),
                    UserName = UserName,
                    Version = version,
                    ValueAfter = assignmentCodes.FirstOrDefault(a => a.CodeId == changed.AssignStatus).Title,
                    ValueBefore = assignmentCodes.FirstOrDefault(a => a.CodeId == type.AssignStatus.Value).Title
                });
            }
        }

        private IQueryable<LookupUserCodeViewModel> GetLookUpUserCodes(string Id, string culture)
        {
            return (from c in context.LookUpUserCodes
                    where c.CodeName == Id && (c.StartDate <= DateTime.Today && (c.EndDate == null || c.EndDate >= DateTime.Today))
                    join t in context.LookUpTitles on new { c.CodeName, c.CodeId } equals new { t.CodeName, t.CodeId } into g
                    from t in g.Where(gg => gg.Culture == culture).DefaultIfEmpty()
                    select new LookupUserCodeViewModel
                    {
                        Id = c.Id,
                        CodeId = c.CodeId,
                        CodeName = c.CodeName,
                        SysCodeId = c.SysCodeId,
                        Name = c.Name,
                        Title = t.Title == null ? c.Name : t.Title,
                    });
        }
    }
}
