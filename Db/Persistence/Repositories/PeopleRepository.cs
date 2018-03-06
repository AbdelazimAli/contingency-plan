using Interface.Core.Repositories;
using Model.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Model.ViewModel.Personnel;

using System.IO;
using System.Data.Entity.Infrastructure;
using Model.ViewModel.Administration;
using Model.ViewModel;

namespace Db.Persistence.Repositories
{
    class PeopleRepository : Repository<PeopleGroup>, IPeopleRepository
    {
        public PeopleRepository(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

       
        public void GetJob_Department_Branch_Translated(string Culture, int EmpID,int JobID,int DepartmentID,int BranchID,out string JobName,out string DeptName,out string BranchName)
        {
            JobName = "";
            DeptName = "";
            BranchName = "";

            try
            {
                JobName = context.Jobs.Where(a => a.Id == JobID).Select(a => HrContext.TrlsName(a.Name, Culture)).FirstOrDefault();
                DeptName = context.CompanyStructures.Where(a => a.Id == DepartmentID).Select(a => HrContext.TrlsName(a.Name, Culture)).FirstOrDefault();
                BranchName = context.Branches.Where(a => a.Id == BranchID).Select(a => HrContext.TrlsName(a.Name, Culture)).FirstOrDefault();
            }
            catch
            {

            }
        }

        public IQueryable<PeopleGroupViewModel> GetPeoples()
        {
            var Peoples = from p in context.PeopleGroups
                          select new PeopleGroupViewModel
                          {
                              Code = p.Code,
                              Name = p.Name,
                              Id = p.Id,
                              CreatedTime = p.CreatedTime,
                              CreatedUser = p.CreatedUser,
                              ModifiedTime = p.ModifiedTime,
                              ModifiedUser = p.ModifiedUser
                          };

            return Peoples;
        }
        public PeopleGroup GetPersonGroup(int Id)
        {
            return context.PeopleGroups.Find(Id);
        }
        public IQueryable<WorkFlowViewModel> GetAllRequests(int companyId, string culture)
        {
            var result = from wft in context.WF_TRANS
                         where wft.CompanyId == companyId
                         join p in context.People on wft.EmpId equals p.Id
                         join d in context.CompanyStructures on wft.DeptId equals d.Id
                         join b in context.CompanyStructures on wft.BranchId equals b.Id into g
                         from b in g.DefaultIfEmpty()
                         join ap in context.People on wft.AuthEmp equals ap.Id into g1
                         from ap in g1.DefaultIfEmpty()
                         join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                         from apos in g2.DefaultIfEmpty()
                         join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                         from dep in g3.DefaultIfEmpty()
                         join role in context.Roles on wft.RoleId equals role.Id into g4
                         from role in g4.DefaultIfEmpty()
                         join postion in context.Positions on wft.PositionId equals postion.Id into g5
                         from postion in g5.DefaultIfEmpty()
                         select new WorkFlowViewModel
                         {
                             Id = wft.DocumentId,
                             Source = wft.Source,
                             ApprovalStatus = wft.ApprovalStatus,
                             Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             Department = HrContext.TrlsName(d.Name, culture),
                             Branch = HrContext.TrlsName(b.Name, culture),
                             AuthEmpName = HrContext.TrlsName(ap.Title + " " + ap.FirstName + " " + ap.Familyname, culture),
                             AuthDeptName = HrContext.TrlsName(dep.Name, culture),
                             AuthPosName = role == null ? HrContext.TrlsName(apos.Name, culture) : role.Name,
                             CreatedTime = wft.CreatedTime,
                             CreatedUser = wft.CreatedUser,
                             HasImage = p.HasImage,
                             CompanyId = companyId,
                             EmpId = wft.EmpId,
                             RoleId = wft.RoleId.ToString(),
                             DeptId = wft.DeptId,
                             PositionId = wft.PositionId,
                             AuthBranch = wft.AuthBranch,
                             AuthDept = wft.AuthDept,
                             AuthEmp = wft.AuthEmp,
                             AuthPosition = wft.AuthPosition,
                             BranchId = wft.BranchId,
                             EmpStatus = HrContext.GetEmpStatus(p.Id),
                             PositionName = HrContext.TrlsName(postion.Name, culture),
                             LocalRoleName = HrContext.TrlsName(role.Name, culture),
                         };

            return result;
        }
        public PeoplesViewModel ReadPerson(int id, string culture)
        {
            var person = (from p in context.People
                          where p.Id == id
                          select new PeoplesViewModel
                          {
                              Id = p.Id,
                              BloodClass = p.BloodClass,
                              BirthCity = p.BirthCity,
                              BirthCountry = p.BirthCountry,
                              WorkTel = p.WorkTel,
                              ProviderId = p.ProviderId,
                              EmergencyTel = p.EmergencyTel,
                              BirthDate = p.BirthDate,
                              ExpiryDate = p.ExpiryDate,
                              Familyname = p.Familyname,
                              Gender = p.Gender,
                              localName = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                              Attachments = HrContext.GetAttachments("People", p.Id),
                              BirthDstrct = p.BirthDstrct,
                              Fathername = p.Fathername,
                              FirstName = p.FirstName,
                              HomeTel = p.HomeTel,
                              GFathername = p.GFathername,
                              IssueDate = p.IssueDate,
                              JoinDate = p.JoinDate,
                              IssuePlace = p.IssuePlace,
                              KafeelId = p.KafeelId,
                              InspectDate = p.InspectDate,
                              MaritalStat = p.MaritalStat,
                              MedStatDate = p.MedStatDate,
                              Mobile = p.Mobile,
                              MedicalStat = p.MedicalStat,
                              Rank = p.Rank,
                              MilResDate = p.MilResDate,
                              MilCertGrade = p.MilCertGrade,
                              MilitaryNo = p.MilitaryNo,
                              MilStatDate = p.MilStatDate,
                              NationalId = p.NationalId,
                              MilitaryStat = p.MilitaryStat,
                              Nationality = p.Nationality,
                              PassportNo = p.PassportNo,
                              Profession = p.Profession,
                              RecommenReson = p.RecommenReson,
                              Recommend = p.Recommend,
                              Religion = p.Religion,
                              OtherEmail = p.OtherEmail,
                              Ssn = p.Ssn,
                              QualificationId = p.QualificationId,
                              Title = p.Title,
                              WorkEmail = p.WorkEmail,
                              TaxFamlyCnt = p.TaxFamlyCnt,
                              BnftFamlyCnt = p.BnftFamlyCnt,
                              StartExpDate = p.StartExpDate,
                              EmpStatus = HrContext.GetEmpStatus(p.Id),
                              CreatedTime = p.CreatedTime,
                              ModifiedTime = p.ModifiedTime,
                              ModifiedUser = p.ModifiedUser,
                              CreatedUser = p.CreatedUser,
                              PicUrl = HrContext.GetDocument("EmployeePic", p.Id),
                              HasImage = p.HasImage,
                              IdIssueDate = p.IdIssueDate,
                              VisaNo = p.VisaNo,
                              VarSubAmt = p.VarSubAmt,
                              BasicSubAmt = p.BasicSubAmt,
                              SubscripDate = p.SubscripDate,
                              Address1 = p.Address1,
                              CityId = p.CityId,
                              CountryId = p.CountryId,
                              DistrictId = p.DistrictId,
                              HoAddress = p.HoAddress,
                              Latitude = p.Latitude,
                              Longitude = p.Longitude,
                              TreatCardNo = p.TreatCardNo,
                              Status = p.Status,
                              PaperStatus=p.PaperStatus
                          }).FirstOrDefault();
            return person;
        }

        public string GetDocument(string source, int sourceid)
        {
            var data = context.CompanyDocsView.Where(a => a.Source == source && a.SourceId == sourceid).Select(a => a.file_stream).FirstOrDefault();
            return data == null ? "" : Convert.ToBase64String(data);
        }

        public string GetAttachmentsCount(int Id/*, out int attachments*/)
        {
            var today = DateTime.Today.Date;
            var result = context.Employements
                .Where(a => a.EmpId == Id && a.Status == 1)
                .Select(a => HrContext.GetDocsCount(a.CompanyId, a.EmpId, a.SuggestJobId))
                .FirstOrDefault();

            return result == null ? "0/0" : result;
        }

        public string GetMissingAttachments(int companyId, int empId, string culture, int Gender, int? Nationality)
        {
            //var result = context.Database.SqlQuery<string>("SELECT U.DOCNAME FROM (SELECT dbo.fn_TrlsName(D.[Name], '" + culture + "') AS DOCNAME, D.Id AS DOCID FROM DocTypes D WHERE (D.RequiredOpt = 1 AND (D.Gender=" + Gender + " OR D.Gender IS NULL) And (D.IsLocal = 0 Or D.CompanyId = " + companyId + ")) OR (D.RequiredOpt = 2 AND D.Id In (SELECT DISTINCT DocType_Id FROM JobDocTypes WHERE Job_Id = (SELECT TOP 1 A.JobId FROM Assignments A WHERE A.EmpId = " + empId + " AND GETDATE() Between A.AssignDate And A.EndDate)))) U WHERE U.DOCID NOT IN (SELECT DISTINCT IsNull(TypeId, 0) FROM CompanyDocuments WHERE Source = 'People' And SourceId = " + empId + ")").ToList();
            var result = context.Database.SqlQuery<string>("SELECT U.DOCNAME FROM (SELECT dbo.fn_TrlsName(D.Name, '" + culture + "') AS DOCNAME, D.Id AS DOCID FROM DocTypes D left outer join DocTypeCountries C on D.Id = C.DocType_Id WHERE (D.RequiredOpt <> 0 AND ISNULL(D.Gender, " + Gender + ") = " + Gender + " And ISNULL(C.Country_Id, " + (Nationality ?? 0) + ") = " + (Nationality ?? 0) + ") And ((D.RequiredOpt = 1 AND  (D.IsLocal = 0 Or D.CompanyId = " + companyId + ")) OR (D.RequiredOpt = 2 AND D.Id In (SELECT DISTINCT DocType_Id FROM JobDocTypes WHERE Job_Id = (SELECT TOP 1 A.JobId FROM Assignments A WHERE A.EmpId = " + empId + " AND Convert(date, GETDATE()) Between A.AssignDate And A.EndDate))))) U WHERE U.DOCID NOT IN (SELECT DISTINCT IsNull(TypeId, 0) FROM CompanyDocuments WHERE Source = 'People' And SourceId = " + empId + ")").ToList();
            return string.Join(", ", result);
        }

        public double GetProfileCount(int empId, int companyId, byte version)
        {
            // NeedToReview
            List<PersonProfileVM> list = new List<PersonProfileVM>();
            List<string> IgnoreCols = new List<string>() { "TaxFamlyCnt", "BnftFamlyCnt" };

            //People
            List<string> columns = context.FormsColumns.Where(f => f.Section.FieldSet.PageId == HrContext.GetPageId(companyId, "People", version) && f.isVisible && f.InputType != "button").Select(f => f.ColumnName).ToList();
            Person peopleRecord = context.People.Where(e => e.Id == empId).FirstOrDefault();

            PersonProfileVM countObj = peopleRecord != null ? getCounts(columns, peopleRecord) : getCounts(columns, new Person());
            list.Add(countObj);

            //HasImage
            list.Add(new PersonProfileVM { NofVisible = 1, NofData = peopleRecord.HasImage ? 1 : 0 });

            //Additinal Info
            var flex = (from fc in context.FlexColumns
                        where fc.PageId == HrContext.GetPageId(companyId, "People", version) && fc.isVisible
                        join fd in context.FlexData on new { fc.PageId, fc.ColumnName } equals new { fd.PageId, fd.ColumnName } into g
                        from fd in g.Where(b => b.SourceId == empId).DefaultIfEmpty()
                        select new { fd.Value, fc.ColumnName }).ToList();
            list.Add(new PersonProfileVM { NofVisible = flex.Count(), NofData = flex.Count(l => !String.IsNullOrEmpty(l.Value)) });

            //Employments
            columns = context.FormsColumns.Where(f => f.Section.FieldSet.PageId == HrContext.GetPageId(companyId, "Emp", version) && f.isVisible && f.InputType != "button" && f.ColumnName != "Profession").Select(f => f.ColumnName).ToList();
            Employement empRecord = context.Employements.Where(e => e.EmpId == empId && e.Status == 1).FirstOrDefault() ?? new Employement();

            list.Add(getCounts(columns, empRecord));

            //Assignments
            IgnoreCols = new List<string>() { "ManagerId", "EmpTasks", "IsDepManager", "PositionId", "IBranches", "IPayrollGrades", "IPositions", "ICompanyStuctures", "IEmployments", "IJobs", "IPayrolls", "IPeopleGroups" };

            columns = context.FormsColumns.Where(f => f.Section.FieldSet.PageId == HrContext.GetPageId(companyId, "AssignmentsForm", version) && f.isVisible && !IgnoreCols.Contains(f.ColumnName) && f.InputType != "button").Select(f => f.ColumnName).ToList();
            var today = DateTime.Today.Date;
            Assignment assignRecord = context.Assignments.Where(a => a.EmpId == empId && a.AssignDate <= today && a.EndDate >= today).FirstOrDefault() ?? new Assignment();

            list.Add(getCounts(columns, assignRecord));

            //--Calculations
            double value = Math.Round(list.Sum(l => l.NofData) / list.Sum(l => l.NofVisible), 2) * 100;
            return value;
        }

        private PersonProfileVM getCounts(List<string> columns, object record)
        {
            var matchedProps = record.GetType().GetProperties().Where(r => columns.Contains(r.Name) && !r.PropertyType.FullName.Contains("Model.Domain")).ToList();

            PersonProfileVM counts = new PersonProfileVM()
            {
                NofVisible = matchedProps.Count(),
                NofData = matchedProps.Count(r => r.GetValue(record) != null)
            };

            return counts;
        }
        public IQueryable<AuditViewModel> EmployeesLog(int companyId, byte version, int Id, string culture)
        {
            // NeedToReview
            var Db = Context as HrContext;
            var union = (from p in Db.People where p.Id == Id select new { sourceId = p.Id, objectName = "People" })
                        .Union(from e in Db.Employements where e.EmpId == Id select new { sourceId = e.Id, objectName = "Emp" })
                        .Union(from a in Db.Assignments where a.EmpId == Id select new { sourceId = a.Id, objectName = "AssignmentsForm" })
                        .Union(from r in Db.EmpRelative where r.EmpId == Id select new { sourceId = r.Id, objectName = "EmpRelatives" })
                        .Union(from b in Db.EmpBenefits where b.EmpId == Id select new { sourceId = b.Id, objectName = "EmpBenefits" })
                        .Union(from t in Db.PeopleTraining where t.EmpId == Id select new { sourceId = t.Id, objectName = "PeopleTraining" });

            var query = from a in Db.AuditTrail
                        join u in union on new { c1 = a.CompanyId, c2 = a.ObjectName, c3 = a.Version, c4 = a.SourceId } equals new { c1 = companyId, c2 = u.objectName, c3 = version, c4 = u.sourceId.ToString() }
                        join c in Db.Companies on a.CompanyId equals c.Id
                        join t in Db.ColumnTitles on new { a.CompanyId, a.ObjectName, a.Version, a.ColumnName }
                        equals new { t.CompanyId, t.ObjectName, t.Version, t.ColumnName } into g
                        from t in g.Where(b => b.Culture == culture).DefaultIfEmpty()
                        select new AuditViewModel
                        {
                            ColumnName = t == null ? a.ColumnName : t.Title,
                            ValueAfter = a.ValueAfter,
                            ValueBefore = a.ValueBefore,
                            ModifiedUser = a.ModifiedUser,
                            ModifiedTime = a.ModifiedTime,
                            ObjectName = a.ObjectName,
                            Company = c.Name
                        };

            return query;
        }

        #region Employment by Mamdouh
        //ReadEmployments
        public string CheckCode(Employement Emp, string culture)
        {
            string error = "OK";
            byte genCode = 2;
            bool codeReuse = false;

            var personnal = GetPersonSetup(Emp.CompanyId);
            if (personnal != null)
            {
                genCode = personnal.GenEmpCode;
                codeReuse = personnal.CodeReuse;
            }

            if (string.IsNullOrEmpty(Emp.Code))
            {
                if (genCode == 3)
                    return MsgUtils.Instance.Trls(culture, "NationalIdRequired"); // National/Resident Id is required, please first enter it
                else
                    return MsgUtils.Instance.Trls(culture, "CodeIsRequired"); // Code is required
            }

            var isDuplicate = true;
            while (isDuplicate)
            {
                if (codeReuse)
                    isDuplicate = context.Employements.Where(a => a.CompanyId == Emp.CompanyId && a.Code == Emp.Code && a.Status == 1).FirstOrDefault() != null;
                else
                    isDuplicate = context.Employements.Where(a => a.CompanyId == Emp.CompanyId && a.EmpId != Emp.EmpId && a.Code == Emp.Code).FirstOrDefault() != null;


                if (isDuplicate == false && genCode == 1)
                    return error;
                else if (genCode == 3 && Emp.Code != null)
                    return error;
                else if (genCode == 2 && !isDuplicate)
                    return error;
                else
                    return MsgUtils.Instance.Trls(culture, "CodeUsedBefore"); // Code is used before, please enter anothor code
            }


            return error;
        }

        public EmployementViewModel GetEmployment(int EmpId)
        {
            var employement = (from e in context.Employements
                               where e.EmpId == EmpId && e.Status == 1
                               join lc in context.LookUpUserCodes on e.PersonType equals lc.CodeId
                               select new EmployementViewModel
                               {
                                   Id = e.Id,
                                   Code = e.Code,
                                   CompanyId = e.CompanyId,
                                   StartDate = e.StartDate,
                                   EndDate = e.EndDate,
                                   AutoRenew = e.AutoRenew,
                                   RemindarDays = e.RemindarDays == null ? 10 : e.RemindarDays,
                                   Allowances = e.Allowances,
                                   BenefitDesc = e.BenefitDesc,
                                   Salary = e.Salary,
                                   Status = e.Status,
                                   EmpId = EmpId,
                                   PersonType = e.PersonType,
                                   FromCountry = e.FromCountry,
                                   TicketAmt = e.TicketAmt,
                                   ToCountry = e.ToCountry,
                                   SpecialCond = e.SpecialCond,
                                   TicketCnt = e.TicketCnt,
                                   Sequence = e.Sequence,
                                   JobDesc = e.JobDesc,
                                   CreatedTime = e.CreatedTime,
                                   CreatedUser = e.CreatedUser,
                                   ModifiedTime = e.ModifiedTime,
                                   ModifiedUser = e.ModifiedUser,
                                   Curr = e.Curr,
                                   VacationDur = e.VacationDur,
                                   ContIssueDate = e.ContIssueDate,
                                   DurInYears = e.DurInYears,
                                   DurInMonths = e.DurInMonths,
                                   Profession = e.Profession,
                                   SysCodeId = lc.SysCodeId,
                                   SuggestJobId = e.SuggestJobId
                               }).FirstOrDefault();

            if (employement != null)
                return employement;
            else
                return new EmployementViewModel();
        }
        public IQueryable<EmployementViewModel> GetHistoryEmployement(int Id)
        {
            var history = from e in context.Employements
                          where e.EmpId == Id
                          join c in context.Companies on e.CompanyId equals c.Id
                          select new EmployementViewModel
                          {
                              Id = e.Id,
                              Status = e.Status,
                              Code = e.Code,
                              Salary = e.Salary,
                              CompanyName = c.Name,
                              PersonType = e.PersonType,
                              StartDate = e.StartDate,
                              EndDate = e.EndDate,
                              TicketAmt = e.TicketAmt,
                              TicketCnt = e.TicketCnt

                          };
            return history;
        }


        #endregion
        #region Qualifications by Mamdouh
        //ReadQualifications
        public IQueryable<EmpQualificationViewModel> ReadQualifications(int id, bool flag)
        {
            var qual = from q in context.PeopleQuals
                       where q.EmpId == id && q.IsQualification == flag
                       select new EmpQualificationViewModel
                       {
                           Id = q.Id,
                           EmpId = q.EmpId,
                           Status = q.Status,
                           FinishDate = q.FinishDate,
                           Grade = q.Grade,
                           QualId = q.QualId,
                           GradYear = q.GradYear,
                           Notes = q.Notes,
                           SchoolId = q.SchoolId,
                           Score = q.Score,
                           StartDate = q.StartDate,
                           Title = q.Title,
                           Awarding = q.Awarding,
                           Cost = q.Cost,
                           ExpiryDate = q.ExpiryDate,
                           GainDate = q.GainDate,
                           SerialNo = q.SerialNo,
                           IsQualification = flag,
                           CreatedTime = q.CreatedTime,
                           CreatedUser = q.CreatedUser,
                           ModifiedUser = q.ModifiedUser,
                           ModifiedTime = q.ModifiedTime,
                       };
            return qual;
        }
        //getQualification
        public IQueryable<GridListViewModel> getQualification(string CodeName)
        {
            var query = from q in context.Qualifications
                        join c in context.LookUpUserCodes on new { p1 = CodeName, p2 = q.Category } equals new { p1 = c.CodeName, p2 = c.CodeId }
                        where c.SysCodeId < 4
                        select new GridListViewModel { value = q.Id, text = q.Name };

            return query;
        }

        public IQueryable<DropDownList> GetAllPeoples(string culture)
        {
            return (from p in context.People
                    select new DropDownList
                    {
                        Id = p.Id,
                        Name = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                        PicUrl = HrContext.GetDoc("EmployeePic", p.Id),
                        Icon = HrContext.GetEmpStatus(p.Id),
                        Gender = p.Gender
                    });
        }

        //  getCertification
        public IQueryable<GridListViewModel> getCertification(string CodeName)
        {
            var query = from q in context.Qualifications
                        join c in context.LookUpUserCodes on new { p1 = CodeName, p2 = q.Category } equals new { p1 = c.CodeName, p2 = c.CodeId }
                        where c.SysCodeId >= 4
                        select new GridListViewModel { value = q.Id, text = q.Name };

            return query;

        }
        #endregion
        public void Add(Person person)
        {
            context.People.Add(person);
        }
        public void Add(PeopleQual qual)
        {
            context.PeopleQuals.Add(qual);
        }
        public void Add(Employement Emp)
        {
            context.Employements.Add(Emp);
        }
        public void Attach(Person person)
        {
            context.People.Attach(person);
        }
        public void Attach(PeopleQual qual)
        {
            context.PeopleQuals.Attach(qual);
        }
        public void Attach(Employement Emp)
        {
            context.Employements.Attach(Emp);
        }
        public void Remove(Person person)
        {
            if (Context.Entry(person).State == EntityState.Detached)
            {
                context.People.Attach(person);
            }
            context.People.Remove(person);
        }
        public void RemoveRange(IEnumerable<Assignment> AssignmentEntities)
        {
            Context.Set<Assignment>().RemoveRange(AssignmentEntities);
        }
        public void RemoveRange(IEnumerable<Person> PersonEntities)
        {
            Context.Set<Person>().RemoveRange(PersonEntities);
        }
        public void RemoveRange(IEnumerable<Employement> EmploymentEntities)
        {
            Context.Set<Employement>().RemoveRange(EmploymentEntities);
        }
        public void Remove(Employement employment)
        {
            if (Context.Entry(employment).State == EntityState.Detached)
            {
                context.Employements.Attach(employment);
            }
            context.Employements.Remove(employment);
        }
        public void Remove(PeopleQual qual)
        {
            if (Context.Entry(qual).State == EntityState.Detached)
            {
                context.PeopleQuals.Attach(qual);
            }
            context.PeopleQuals.Remove(qual);
        }
        public DbEntityEntry<Person> Entry(Person person)
        {
            return Context.Entry(person);
        }
        public DbEntityEntry<Employement> Entry(Employement Emp)
        {
            return Context.Entry(Emp);
        }
        public DbEntityEntry<PeopleQual> Entry(PeopleQual qual)
        {
            return Context.Entry(qual);
        }
        public Person GetPerson(int? id)
        {
            return Context.Set<Person>().Find(id);
        }
        public Employement FindEmployment(int? id)
        {
            return Context.Set<Employement>().Find(id);
        }
        public Assignment FindAssignment(int? id)
        {
            return Context.Set<Assignment>().Find(id);
        }
        public void RemovePerson(int? id)
        {
            var person = Context.Set<Person>().Find(id);
            if (person != null) Remove(person);
        }

        public IQueryable<FormList> GetActiveEmployees(int companyId, string culture)
        {
            var today = DateTime.Today.Date;

            return (from a in context.Assignments
                    where a.CompanyId == companyId && a.AssignDate <= today && a.EndDate >= today
                    join p in context.People on a.EmpId equals p.Id
                    select new FormList
                    {
                        id = p.Id,
                        name = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                        PicUrl = HrContext.GetDoc("EmployeePic", p.Id),
                        Gender = p.Gender,
                        Icon = HrContext.GetEmpStatus(p.Id)
                    });
        }

        public IEnumerable<FormList> GetEmployeeById(int companyId, string culture, int EmpId)
        {
            var today = DateTime.Today.Date;

            return (from a in context.Assignments
                    where a.EmpId == EmpId && a.AssignDate <= today && a.EndDate >= today
                    join p in context.People on a.EmpId equals p.Id
                    select new FormList
                    {
                        id = p.Id,
                        name = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                        PicUrl = HrContext.GetDoc("EmployeePic", p.Id),
                        Gender = p.Gender,
                        Icon = HrContext.GetEmpStatus(p.Id)
                    }).ToList();
        }

        // Get the employee with Dept manger role only  // Done By fatma
        public IQueryable<FormList> GetActiveMangers(int companyId, string culture)
        {
            var today = DateTime.Today.Date;
            //Get Departments managers
            var query1 = (from a in context.Assignments
                          where a.CompanyId == companyId && a.AssignDate <= today && a.EndDate >= today && a.IsDepManager == true
                          select new FormList
                          {
                              id = a.EmpId,
                              name = HrContext.TrlsName(a.Employee.Title + " " + a.Employee.FirstName + " " + a.Employee.Familyname, culture),
                              PicUrl = HrContext.GetDoc("EmployeePic", a.EmpId),
                              Gender = a.Employee.Gender,
                              Icon = HrContext.GetEmpStatus(a.Employee.Id)
                          });

            var query2 = (from a in context.Assignments
                          where a.CompanyId == companyId && a.AssignDate <= today && a.EndDate >= today
                          join p in context.People on a.ManagerId equals p.Id
                          select new FormList
                          {
                              id = p.Id,
                              name = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                              PicUrl = HrContext.GetDoc("EmployeePic", p.Id),
                              Gender = p.Gender,
                              Icon = HrContext.GetEmpStatus(p.Id)
                          }).Distinct();

            var query3 = query1.Union(query2);
            return query3;
        }

        public IQueryable<FormList> GetActiveMangersByMangerId(int companyId, string culture, int MangerId)
        {
            var today = DateTime.Today.Date;
            var query1 = (from a in context.Assignments
                          where a.CompanyId == companyId && a.AssignDate <= today && a.EndDate >= today && a.EmpId == MangerId
                          select new FormList
                          {
                              id = a.EmpId,
                              name = HrContext.TrlsName(a.Employee.Title + " " + a.Employee.FirstName + " " + a.Employee.Familyname, culture),
                              PicUrl = HrContext.GetDoc("EmployeePic", a.EmpId),
                              Gender = a.Employee.Gender,
                              Icon = HrContext.GetEmpStatus(a.Employee.Id)
                          });
            return query1;
        }

        //
        public IQueryable<FormList> GetEmployeeManagedByManagerId(int companyId, string culture, int MangId)
        {
            var today = DateTime.Today.Date;

            // Get employees direct managed by manager Id
            // or get employees in the manager tree
            var query1 = from a in context.Assignments
                         where a.CompanyId == companyId && a.AssignDate <= today && a.EndDate >= today && a.SysAssignStatus == 1
                         && a.EmpId != MangId
                         && (a.ManagerId == MangId || (a.Department.Sort.StartsWith(context.Assignments.Where(b => b.EmpId == MangId && b.AssignDate <= today && b.EndDate >= today && b.SysAssignStatus == 1 && b.IsDepManager).Select(b => b.Department.Sort).FirstOrDefault())))
                         select new FormList
                         {
                             id = a.EmpId,
                             name = HrContext.TrlsName(a.Employee.Title + " " + a.Employee.FirstName + " " + a.Employee.Familyname, culture),
                             PicUrl = HrContext.GetDoc("EmployeePic", a.EmpId),
                             Gender = a.Employee.Gender,
                             Icon = HrContext.GetEmpStatus(a.Employee.Id)
                         };

            return query1;
        }
        public void Add(PeopleTraining person)
        {
            context.PeopleTraining.Add(person);
        }
        public void Attach(PeopleTraining person)
        {
            context.PeopleTraining.Attach(person);
        }
        public void Remove(PeopleTraining person)
        {
            if (Context.Entry(person).State == EntityState.Detached)
            {
                context.PeopleTraining.Attach(person);
            }
            context.PeopleTraining.Remove(person);
        }
        public void Remove(Assignment assignment)
        {
            if (Context.Entry(assignment).State == EntityState.Detached)
            {
                context.Assignments.Attach(assignment);
            }
            context.Assignments.Remove(assignment);
        }

        public DbEntityEntry<PeopleTraining> Entry(PeopleTraining person)
        {
            return Context.Entry(person);
        }

        public IQueryable<PeopleTrainGridViewModel> ReadEmployeeTraining(int Id)
        {
            var result = (from PT in context.PeopleTraining
                          where PT.EmpId == Id
                          select new PeopleTrainGridViewModel
                          {
                              Id = PT.Id,
                              PersonId = PT.EmpId,
                              CourseId = PT.CourseId,
                              CourseTitle = PT.CourseTitle,
                              CourseEDate = PT.CourseEDate,
                              CourseSDate = PT.CourseSDate,
                              ActualHours = PT.ActualHours,
                              Adwarding = PT.Adwarding,
                              CantLeaveDate = PT.CantLeave,
                              Cost = PT.Cost,
                              Internal = PT.Internal,
                              Notes = PT.Notes,
                              Status = PT.Status,
                          });
            return result;
        }


        #region RenewRequest By Abdelazim
        public RenewRequest Getrequest(int? id)
        {
            return Context.Set<RenewRequest>().Find(id);
        }
        public void Add(RenewRequest request)
        {
            context.RenewRequests.Add(request);
        }
        public void Attach(RenewRequest request)
        {
            context.RenewRequests.Attach(request);
        }
        public void Remove(RenewRequest request)
        {
            if (Context.Entry(request).State == EntityState.Detached)
            {
                context.RenewRequests.Attach(request);
            }
            context.RenewRequests.Remove(request);
        }
        public DbEntityEntry<RenewRequest> Entry(RenewRequest request)
        {
            return Context.Entry(request);
        }
        public IEnumerable<ColumnDropdownViewModel> GetEditColumn(int CompanyId, string ObjectName, byte Version, string Culture, string RoleId)
        {
            //var res = from FC in context.FormsColumns
            //          where FC.Section.FieldSet.Page.CompanyId == CompanyId && FC.Section.FieldSet.Page.ObjectName == ObjectName && FC.Section.FieldSet.Page.Version == Version && FC.ColumnName != "LocalName" &&
            //          (FC.InputType == null || FC.InputType == "string" || FC.InputType == "text" || FC.InputType == "email" || FC.ColumnName == "MaritalStat" || FC.ColumnName == "QualificationId")
            //          join RC in context.RoleColumns on new { CompanyId = FC.Section.FieldSet.Page.CompanyId, ObjectName = FC.Section.FieldSet.Page.ObjectName, Version = FC.Section.FieldSet.Page.Version, ColumnName = FC.ColumnName }
            //          equals new { CompanyId = RC.CompanyId, ObjectName = RC.ObjectName, Version = RC.Version, ColumnName = RC.ColumnName } into g
            //          from RC in g.Where(a => a.RoleId == RoleId).DefaultIfEmpty()
            //          where (RC == null || RC.isEnabled)
            //          select new ColumnDropdownViewModel
            //          {
            //              ColumnName = FC.ColumnName,
            //              Title = HrContext.GetColumnTitle(FC.Section.FieldSet.Page.CompanyId, Culture, FC.Section.FieldSet.Page.ObjectName, FC.Section.FieldSet.Page.Version, FC.ColumnName)
            //          };
            //FormColumns.ColumnName <> 'Address' AND
            var res = context.Database.SqlQuery<ColumnDropdownViewModel>($"SELECT FormColumns.ColumnName, dbo.fn_GetColumnTitle({CompanyId}, '{Culture}', '{ObjectName}', '{Version}', FormColumns.ColumnName) Title FROM FormColumns INNER JOIN Sections ON FormColumns.SectionId = Sections.Id INNER JOIN FieldSets ON Sections.FieldSetId = FieldSets.Id INNER JOIN PageDivs ON FieldSets.PageId = PageDivs.Id LEFT OUTER JOIN RoleColumns ON (RoleColumns.CompanyId = {CompanyId} AND RoleColumns.ObjectName = '{ObjectName}' AND RoleColumns.[Version] = {Version} AND RoleColumns.ColumnName = FormColumns.ColumnName AND RoleColumns.RoleId = '{RoleId}') WHERE PageDivs.CompanyId = {CompanyId} AND PageDivs.ObjectName = '{ObjectName}' AND PageDivs.[Version] = {Version} AND (RoleColumns.isEnabled = 1 Or RoleColumns.isEnabled is null) AND FormColumns.ColumnName <> 'LocalName' AND  FormColumns.ColumnName <> 'HostAddress' AND FormColumns.ColumnName <> 'Code' AND (FormColumns.InputType is null Or FormColumns.InputType In ('string','text','email') Or FormColumns.ColumnName in ('MaritalStat','QualificationId'))");
            return res.ToList();

        }

        public IList<int> GetIdsList(int companyId, int EmpId)
        {
            var ids = context.RenewRequests.Where(re => re.EmpId == EmpId && re.CompanyId == companyId).Select(re => re.Id).ToList();
            return ids;
        }
        public IQueryable<RenewRequestViewModel> ReadRenewRequestTabs(int companyId, byte Tab, byte Range, DateTime? Start, DateTime? End, string culture, byte Version, int EmpId)
        {

            if (Range != 10 && Range != 0) RequestRangeFilter(Range, companyId, out Start, out End);


            DateTime Today = DateTime.Today.Date;

            var query1 = from RQ in context.RenewRequests
                         where RQ.EmpId == EmpId && RQ.CompanyId == companyId
                         select RQ;
            
            if (Range != 10) // Allow date range
                query1 = query1.Where(l => Start <= l.RequestDate && l.RequestDate <= End);


            if (Tab == 1) //Pending
                query1 = query1.Where(l => l.ApprovalStatus == 2);
            else if (Tab == 2) //Approved
                query1 = query1.Where(l => l.ApprovalStatus == 6);
            else if (Tab == 3) //Rejected
                query1 = query1.Where(l => l.ApprovalStatus == 9);

            IQueryable<RenewRequestViewModel> query = from q in query1
                                                      select new RenewRequestViewModel
                                                      {
                                                          Id = q.Id,
                                                          ApprovalStatus = q.ApprovalStatus,
                                                          ColumnName = HrContext.GetColumnTitle(companyId, culture, "People", Version, q.ColumnName),
                                                          EmpId = q.EmpId,
                                                          CompanyId = q.CompanyId,
                                                          NewValue = q.NewValue,
                                                          NewValueId = q.NewValueId,
                                                          OldValue = q.OldValue,
                                                          OldValueId = q.OldValueId,
                                                          RequestDate = q.RequestDate,
                                                          CreatedUser = q.CreatedUser,
                                                          CreatedTime = q.CreatedTime,
                                                          RejectionRes = q.RejectionRes,
                                                          AttUrl = HrContext.GetDoc("RenewRequest",q.Id)
                                                      };
            return query;


        }
        public RenewRequestViewModel GetRenewRequest(int requestId)
        {
            if (requestId == 0)
                return new RenewRequestViewModel();


            RenewRequestViewModel Request = context.RenewRequests.Where(r => r.Id == requestId).Select(rw => new RenewRequestViewModel
            {
                Id = rw.Id,
                ApprovalStatus = rw.ApprovalStatus,
                ColumnName = rw.ColumnName,
                EmpId = rw.EmpId,
                CompanyId = rw.CompanyId,
                NewValue = rw.NewValue,
                NewValueId = rw.NewValueId,
                OldValue = rw.OldValue,
                OldValueId = rw.OldValueId,
                RejectionRes = rw.RejectionRes,
                RequestDate = rw.RequestDate,
                CreatedUser = rw.CreatedUser,
                CreatedTime = rw.CreatedTime,
                Attachments = HrContext.GetAttachments("RenewRequest", rw.Id)

            }).FirstOrDefault();
            return Request;
        }
        public ColVlueType GetColValue(int EmpId, string ColumnName)
        {
            //select distinct People.MaritalStat,FormColumns.InputType from People ,FormColumns where People.Id=1054 and FormColumns.ColumnName ='MaritalStat'
            //var value = Context.Database.SqlQuery<string>("select " + ColumnName + " from People where Id = " + EmpId).FirstOrDefault();
            //return context.Database.SqlQuery<string>("select " + ColumnName + " from People where Id = " + EmpId) as string;
            var value = Context.Database.SqlQuery<ColVlueType>($"select distinct CAST(People.{ColumnName} As nvarchar(50)) as 'Value',FormColumns.InputType as 'Type' from People ,FormColumns where People.Id={EmpId} and FormColumns.ColumnName ='{ColumnName}'").FirstOrDefault();
            return value;
        }
        public void ReadRenewRequestAtt()
        {
            // Get logo folder
            var logosFolder = System.AppDomain.CurrentDomain.BaseDirectory + "Files\\uploadercash";
            // Get last access time
            //var lastWriteFileTime = Directory.GetLastWriteTime(logosFolder);
            //var CreationTime = Directory.GetCreationTime(logosFolder);
            //if  (lastWriteFileTime < CreationTime || Directory.GetFiles(logosFolder).Count() <= 1) lastWriteFileTime = new System.DateTime(2000, 1, 1);
            // Get required file need to write
            var files = context.CompanyDocsView.Where(f => f.is_directory == false && f.Source == "RenewRequest" && f.DocType.DocumenType == 1).Select(f => new { name = f.name, data = f.file_stream }).ToList();
            // loop to write files
            foreach (var file in files)
            {
                File.WriteAllBytes(Path.Combine(logosFolder, file.name), file.data);
            }
        }
        public  string GetDocs(string Source,int SourceId)
        {
            var value = Context.Database.SqlQuery<string>($"SELECT TOP 1  [name] FROM [dbo].[CompanyDocsViews] WHERE [Source] = '{Source}' AND [SourceId] = {SourceId}").FirstOrDefault();
            if (value == null)
            {
                return "noimage.jpg";
            }
            return value;
        }

        #endregion

        

        #region Login By Abdelazim

        public Dictionary<string,CompanyDocsViews> GetEmpDocsView(int EmpId, int CompanyId)
        {
            var companyLogo = context.CompanyDocsView.FirstOrDefault(c => c.Source == "ComoanyLogo" && c.SourceId == CompanyId);
            var employeePic = context.CompanyDocsView.FirstOrDefault(e => e.Source == "EmployeePic" && e.SourceId == EmpId);
            var res = new Dictionary<string, CompanyDocsViews>();
            res.Add("ComoanyLogo", companyLogo);
            res.Add("EmployeePic", employeePic);
            return res;
        }

        public string GetEpmLocalname(int EmpId , string Culture)
        {
            var emplouee = from emp in context.People
                           where emp.Id == EmpId
                           select new
                           {
                               LocalName = HrContext.TrlsName(emp.Title + " " + emp.FirstName + " " + emp.Familyname, Culture)
                           };
            return emplouee.FirstOrDefault().LocalName;
        }
        #endregion

    }
    
}
