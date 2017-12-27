using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Db.Persistence.Repositories
{
    class TrainingRepository : Repository<TrainCourse>, ITrainingRepository
    {
        public TrainingRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        #region TrainCourse
        public IQueryable<TrainCourseViewModel > GetTrainCourse(string culture , int CompanyId)
        {
            var result = from l in context.TrainCourses
                         where (((l.IsLocal && l.CompanyId == CompanyId) || l.IsLocal == false) && (l.StartDate <= DateTime.Today && (l.EndDate == null || l.EndDate >= DateTime.Today)))
                         select new TrainCourseViewModel
                         {
                             Id = l.Id,
                             Name = l.Name,
                             Code = l.Code,
                             LocalName = HrContext.TrlsName(l.Name, culture),
                             StartDate = l.StartDate,
                             EndDate = l.EndDate,
                             };
            return result;
        }
        public TrainCourse GetTrainCourse(int? id)
        {
            return context.TrainCourses.Find(id);
        }
        public TrainCourseFormViewModel ReadTrainCourse(int Id, string culture)
        {
            var obj = context.TrainCourses.Where(a => a.Id == Id).Select(a => new {a , LocalName = HrContext.TrlsName(a.Name, culture) }).FirstOrDefault();

            var mod = new TrainCourseFormViewModel()
            {
                Id = obj.a.Id,
                Name = obj.a.Name,
                StartDate = obj.a.StartDate,
                Whom = obj.a.Whom,
                Summary = obj.a.Summary,
                IsLocal = obj.a.IsLocal,
                Age = obj.a.Age,
                Code = obj.a.Code,
                CourseCat = obj.a.CourseCat,
                EndDate = obj.a.EndDate,
                Formula = obj.a.Formula,
                Performance = obj.a.Performance,
                YearServ = obj.a.YearServ,
                PlannedHours = obj.a.PlannedHours,
                Requirements = obj.a.Requirements,
                Qualification = obj.a.Qualification,
                LocalName = obj.LocalName,
                QualRank = obj.a.QualRank,
                CreatedTime = obj.a.CreatedTime,
                CreatedUser = obj.a.CreatedUser,
                ModifiedTime = obj.a.ModifiedTime,
                ModifiedUser = obj.a.ModifiedUser,
                IPrevCourses = obj.a.PrevCourses == null ? null : obj.a.PrevCourses.Split(',').Select(int.Parse).ToList(),
                IPayrolls = obj.a.Payrolls == null ? null : obj.a.Payrolls.Split(',').Select(int.Parse).ToList(),
                ICompanyStuctures = obj.a.CompanyStuctures == null ? null : obj.a.CompanyStuctures.Split(',').Select(int.Parse).ToList(),
                IEmployments = obj.a.Employments == null ? null : obj.a.Employments.Split(',').Select(int.Parse).ToList(),
                IPayrollGrades = obj.a.PayrollGrades == null ? null : obj.a.PayrollGrades.Split(',').Select(int.Parse).ToList(),
                IPositions = obj.a.Positions == null ? null : obj.a.Positions.Split(',').Select(int.Parse).ToList(),
                ILocations = obj.a.Locations == null ? null : obj.a.Locations.Split(',').Select(int.Parse).ToList(),
                IJobs = obj.a.Jobs == null ? null : obj.a.Jobs.Split(',').Select(int.Parse).ToList(),
                IPeopleGroups = obj.a.PeopleGroups == null ? null : obj.a.PeopleGroups.Split(',').Select(int.Parse).ToList()

            };
            return mod;
        }
       
        #endregion
        #region TrainPath

        public IQueryable<TrainPathViewModel> GetTrainPath(string culture,int CompanyId)
        {

            var result = from l in context.TrainPath
                         where (((l.IsLocal && l.CompanyId == CompanyId) || l.IsLocal == false) && (l.StartDate <= DateTime.Today && (l.EndDate == null || l.EndDate >= DateTime.Today)))
                         select new TrainPathViewModel
                         {
                             Id = l.Id,
                             Name = l.Name,
                             LocalName = HrContext.TrlsName(l.Name, culture),
                             StartDate = l.StartDate,
                             EndDate = l.EndDate,
                         };
            return result;
        }
        public IQueryable<TrainCourseViewModel> GetTrainCourseLST(string culture, int CompanyId, bool IsLocal)
        {
            var result = from l in context.TrainCourses
                         where ((IsLocal ? l.IsLocal : ((l.IsLocal && l.CompanyId == CompanyId) || l.IsLocal == false) )&& (l.StartDate <= DateTime.Today && (l.EndDate == null || l.EndDate >= DateTime.Today)))
                         select new TrainCourseViewModel
                         {
                             Id = l.Id,
                             Name = l.Name,
                             LocalName = HrContext.TrlsName(l.Name, culture),
                         };
            return result;
        }

        public void Add(TrainPath trainPath)
        {
            context.TrainPath.Add(trainPath);
        }
        public void Attach(TrainPath trainPath)
        {
            context.TrainPath.Attach(trainPath);
        }
        public DbEntityEntry<TrainPath> Entry(TrainPath trainPath)
        {
            return Context.Entry(trainPath);
        }
        public void Remove(TrainPath trainPath)
        {
            if (Context.Entry(trainPath).State == EntityState.Detached)
            {
                context.TrainPath.Attach(trainPath);
            }
            context.TrainPath.Remove(trainPath);
        }
        public TrainPathFormViewModel ReadTrainPath(int Id, string culture)
        {
            var obj = context.TrainPath.Where(a => a.Id == Id).Select(a => new { a, LocalName = HrContext.TrlsName(a.Name, culture) }).FirstOrDefault();
            var mod = new TrainPathFormViewModel()
            {
                Id = obj.a.Id,
                Name = obj.a.Name,
                StartDate = obj.a.StartDate,
                Summary = obj.a.Summary,
                IsLocal = obj.a.IsLocal,
                Age = obj.a.Age,
                EndDate = obj.a.EndDate,
                Formula = obj.a.Formula,
                Performance = obj.a.Performance,
                YearServ = obj.a.YearServ,
                Qualification = obj.a.Qualification,
                LocalName =obj.LocalName,
                QualRank = obj.a.QualRank,
                CreatedTime = obj.a.CreatedTime,
                CreatedUser = obj.a.CreatedUser,
                ModifiedTime = obj.a.ModifiedTime,
                ModifiedUser = obj.a.ModifiedUser,
                IPayrolls = obj.a.Payrolls == null ? null : obj.a.Payrolls.Split(',').Select(int.Parse).ToList(),
                ICompanyStuctures = obj.a.CompanyStuctures == null ? null : obj.a.CompanyStuctures.Split(',').Select(int.Parse).ToList(),
                IEmployments = obj.a.Employments == null ? null : obj.a.Employments.Split(',').Select(int.Parse).ToList(),
                IPayrollGrades = obj.a.PayrollGrades == null ? null : obj.a.PayrollGrades.Split(',').Select(int.Parse).ToList(),
                IPositions = obj.a.Positions == null ? null : obj.a.Positions.Split(',').Select(int.Parse).ToList(),
                ILocations = obj.a.Locations == null ? null : obj.a.Locations.Split(',').Select(int.Parse).ToList(),
                IJobs = obj.a.Jobs == null ? null : obj.a.Jobs.Split(',').Select(int.Parse).ToList(),
                IPeopleGroups = obj.a.PeopleGroups == null ? null : obj.a.PeopleGroups.Split(',').Select(int.Parse).ToList()

            };
            return mod;
        }
        public TrainPath GetTrainPath(int? id)
        {
            return context.TrainPath.Find(id);
        }
        public IQueryable<TrainPathCourseViewModel> GetCours(int Id)
        {
            var result = from c in context.TrainCourses
                         where c.Pathes.Any(a=>a.Id==Id)
                         select new TrainPathCourseViewModel
                         {
                             Id = c.Id,
                             TrainCourse_Id = c.Id,
                             TrainPath_Id=Id
                         };

            return result;
        }

        public TrainPath Courses(int id)
        {
            return context.TrainPath.Include("Courses").Where(c => c.Id == id).FirstOrDefault();
        }

        #endregion
        #region TrainPeople
        public PeopleTraining GetPeopleTraining(int? id)
        {
            return Context.Set<PeopleTraining>().Find(id);
        }
        public string GetMenuId(int MenuId , int companyId)
        {
            var query = (from m in context.Menus
                    where m.Id == MenuId && m.CompanyId == companyId
                    join pM in context.Menus on m.Sort.Substring(0, 5) equals pM.Sort
                    select new { MenuId = pM.Id , name= pM.Name }).FirstOrDefault();
            return query.name;
        }
        public IQueryable<TrainIndexFollowUpViewModel> GetTrainFollowUp(int companyId, string culture)
        {
            var query = from c in context.PeopleTraining
                        where c.CompanyId == companyId && c.ApprovalStatus > 1 && c.ApprovalStatus < 6
                        join p in context.People on c.EmpId equals p.Id
                        join Cu in context.TrainCourses on c.CourseId equals Cu.Id
                        join Ev in context.TrainEvents on c.EventId equals Ev.Id into Ev1
                        from Ev in Ev1.DefaultIfEmpty()
                        join wft in context.WF_TRANS on new { p1 = "Training", p2 = c.CompanyId, p3 = c.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                        from wft in g.DefaultIfEmpty()
                        join ap in context.People on wft.AuthEmp equals ap.Id into g1
                        from ap in g1.DefaultIfEmpty()
                        join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                        from apos in g2.DefaultIfEmpty()
                        join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                        from dep in g3.DefaultIfEmpty()
                        join role in context.Roles on wft.RoleId equals role.Id into g4
                        from role in g4.DefaultIfEmpty()
                        select new TrainIndexFollowUpViewModel
                        {
                            Id = c.Id,
                            EmpId = c.EmpId,
                            Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                            ActualHours =c.ActualHours,
                            Event=Ev.Name,
                            Course=HrContext.TrlsName(Cu.Name,culture),
                            CourseEDate=c.CourseEDate,
                            EventId=c.EventId,
                            RequestDate=c.RequestDate,
                            Status=c.Status,
                            CompanyId = c.CompanyId,
                            ApprovalStatus = c.ApprovalStatus,
                            HasImage = p.HasImage,
                            RoleId = wft.RoleId.ToString(),
                            DeptId = wft.DeptId,
                            PositionId = wft.PositionId,
                            AuthBranch = wft.AuthBranch,
                            AuthDept = wft.AuthDept,
                            AuthDeptName = HrContext.TrlsName(dep.Name, culture),
                            AuthEmp = wft.AuthEmp,
                            AuthEmpName = HrContext.TrlsName(ap.Title + " " + ap.FirstName + " " + ap.Familyname, culture),
                            AuthPosition = wft.AuthPosition,
                            AuthPosName = role == null ? HrContext.TrlsName(apos.Name, culture) : role.Name,
                            BranchId = wft.BranchId,
                            SectorId = wft.SectorId
                        };

            return query;
        }

        public IQueryable<TrainIndexFollowUpViewModel> GetPeopleTrain(string culture, int CompanyId)
        {
            var query = from c in context.PeopleTraining
                        where c.CompanyId == CompanyId  && c.ApprovalStatus ==6 
                        join p in context.People on c.EmpId equals p.Id
                        join Cu in context.TrainCourses on c.CourseId equals Cu.Id
                        join Ev in context.TrainEvents on c.EventId equals Ev.Id into Ev1
                        from Ev in Ev1.DefaultIfEmpty()
                        join wft in context.WF_TRANS on new { p1 = "Training", p2 = c.CompanyId, p3 = c.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                        from wft in g.DefaultIfEmpty()
                        join ap in context.People on wft.AuthEmp equals ap.Id into g1
                        from ap in g1.DefaultIfEmpty()
                        join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                        from apos in g2.DefaultIfEmpty()
                        join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                        from dep in g3.DefaultIfEmpty()
                        join role in context.Roles on wft.RoleId equals role.Id into g4
                        from role in g4.DefaultIfEmpty()
                        select new TrainIndexFollowUpViewModel
                        {
                            Id = c.Id,
                            EmpId = c.EmpId,
                            Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                            ActualHours = c.ActualHours,
                            Event = Ev.Name,
                            Course = HrContext.TrlsName(Cu.Name, culture),
                            CourseEDate = c.CourseEDate,
                            EventId = c.EventId,
                            RequestDate = c.RequestDate,
                            Status = c.Status,
                            CompanyId = c.CompanyId,
                            ApprovalStatus = c.ApprovalStatus,
                            CourseSDate=c.CourseSDate,
                            HasImage = p.HasImage,
                            RoleId = wft.RoleId.ToString(),
                            DeptId = wft.DeptId,
                            PositionId = wft.PositionId,
                            AuthBranch = wft.AuthBranch,
                            AuthDept = wft.AuthDept,
                            AuthDeptName = HrContext.TrlsName(dep.Name, culture),
                            AuthEmp = wft.AuthEmp,
                            AuthEmpName = HrContext.TrlsName(ap.Title + " " + ap.FirstName + " " + ap.Familyname, culture),
                            AuthPosition = wft.AuthPosition,
                            AuthPosName = role == null ? HrContext.TrlsName(apos.Name, culture) : role.Name,
                            BranchId = wft.BranchId,
                            SectorId = wft.SectorId,
                            CourseTitle=c.CourseTitle,
                            Attachement = HrContext.GetDoc("EmployeePic", p.Id),
                            Gender = p.Gender,
                            EmpStatus = HrContext.GetEmpStatus(p.Id)
                        };

            return query;
        }
        public PeopleTrainFormViewModel ReadPeopleTrain(int Id,string culture)
        {
            var result =( from PT in context.PeopleTraining
                         where PT.Id==Id
                         select new PeopleTrainFormViewModel
                         {
                             Id = PT.Id,
                             CompanyId=PT.CompanyId,
                             EmpId = PT.EmpId,
                             CourseId = PT.CourseId,
                             CourseTitle = PT.CourseTitle,
                             CourseEDate = PT.CourseEDate,
                             CourseSDate = PT.CourseSDate,
                             ActualHours=PT.ActualHours,
                             Adwarding=PT.Adwarding,
                             CantLeave=PT.CantLeave,
                             Cost=PT.Cost,
                             Internal=PT.Internal,
                             Notes=PT.Notes,
                             Status=PT.Status,
                             RejectReason=PT.RejectReason,
                             RejectDesc=PT.RejectDesc,
                             CancelDesc=PT.CancelDesc,
                             WFlowId=PT.WFlowId,
                             CancelReason=PT.CancelReason,
                             Curr=PT.Curr,
                             CurrRate=PT.CurrRate,
                             EventId=PT.EventId,
                             ApprovalStatus=PT.ApprovalStatus,
                             RequestDate=PT.RequestDate,
                             CreatedTime = PT.CreatedTime,
                             CreatedUser = PT.CreatedUser,
                             ModifiedTime = PT.ModifiedTime,
                             ModifiedUser = PT.ModifiedUser,
                         }).FirstOrDefault();
            return result;
        }

        public void Add(PeopleTraining peopleTraining)
        {
            context.PeopleTraining.Add(peopleTraining);
        }
        public IQueryable<PeoplesViewModel> GetPeople(string culture)
        {
            var q = from p in context.People
                    join e in context.Employements on p.Id equals e.EmpId into g
                    from e in g.Where(s => s.Status == 1)
                    select new PeoplesViewModel
                    {
                        Id = e.EmpId,
                        Title = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                        PicUrl = (p.HasImage ? p.Id + ".jpeg" : "noimage.jpg"),
                        EmpStatus = HrContext.GetEmpStatus(p.Id)
                    };
            return q;
        }
        public void Attach(PeopleTraining peopleTraining)
        {
            context.PeopleTraining.Attach(peopleTraining);
        }
        public DbEntityEntry<PeopleTraining> Entry(PeopleTraining peopleTraining)
        {
            return Context.Entry(peopleTraining);
        }
        public void Remove(PeopleTraining peopleTraining)
        {
            if (Context.Entry(peopleTraining).State == EntityState.Detached)
            {
                context.PeopleTraining.Attach(peopleTraining);
            }
            context.PeopleTraining.Remove(peopleTraining);
        }
        public PeopleTraining GetpeopleTraining(int? id)
        {
            return context.PeopleTraining.Find(id);
        }
        #endregion
        #region
        public IEnumerable<EmployeeTrainPathViewModel> TrainPathProgress(int id, string culture)
        {
            var q = context.TrainPath.Where(a => a.Id == id).Select(a =>new {c= a.Courses.Select(b => b.Id),id=a.Id }).FirstOrDefault();
            if (q != null)
            {
                var Emp = context.PeopleTraining.
                            Where(a => q.c.Contains(a.CourseId) && a.Status == 2).GroupBy(a => new { person = a.EmpId, Emp = a.Person }).
                            Select(a => new EmployeeTrainPathViewModel
                            {
                                EmpId = a.Key.person,
                                Id = q.id,
                                Percent = a.Count() * 100 / q.c.Count(),
                                Employee = HrContext.TrlsName(a.Key.Emp.Title + " " + a.Key.Emp.FirstName + " " + a.Key.Emp.Familyname, culture),
                                CourseName = a.Select(j => j.Course.Name).ToList(),
                                CourseIds = a.Select(j => j.Course.Id).ToList(),
                            }
                            ).ToList();
                return Emp;
            }
            else
                return null;
        
        }
        public IEnumerable<EmployeeTrainPathViewModel> GetEmpsTrainpaths(string culture , int companyId)
        {
            var PeopleTList = context.PeopleTraining.Include("Person").Select(a=> new {a.Status, a.EmpId, a.CourseId ,EmpName=HrContext.TrlsName(a.Person.Title+" " + a.Person.FirstName+" " + a.Person.Familyname,culture)}).ToList();
            var q = context.TrainPath.Where(l=>((l.IsLocal && l.CompanyId == companyId) || l.IsLocal == false) && (l.StartDate <= DateTime.Today && (l.EndDate == null || l.EndDate >= DateTime.Today))).Select(a => new {
                c = a.Courses.Select(b => b.Id),
                Tid = a.Id,
                TName=a.Name
            }).ToList();
             List<EmployeeTrainPathViewModel> ls = new List<EmployeeTrainPathViewModel>();
                foreach (var item in q)
                {
                    var p = PeopleTList.Where(a => item.c.Contains(a.CourseId) && a.Status == 2).GroupBy(a => new { person = a.EmpId })
                        .Select(a => new EmployeeTrainPathViewModel
                        {
                            EmpId = a.Key.person,
                            TrainPathName = item.TName,
                            Id = item.Tid,
                            Employee = a.FirstOrDefault().EmpName,
                            Percent = a.Count() * 100 / item.c.Count(),
                            CourseIds = a.Select(l => l.CourseId).ToList()

                        }).ToList();
                    ls.AddRange(p);
                }
                return ls;
            
            
             
        }

        public IEnumerable<TrainCourseViewModel> GetMissCourses(int id,int [] courses ,string Culture)
        {
            var q = context.TrainPath.Where(a => a.Id == id).Select(a => a.Courses.Where(b=>!courses.Contains(b.Id)).Select(b => b.Id)).FirstOrDefault();
            var query = context.TrainCourses.Where(c => q.Contains(c.Id)).Select(c => new TrainCourseViewModel {Name=HrContext.TrlsName(c.Name,Culture) }).ToList();
            return query;
        }
        #endregion
        #region TrainEvent
        public IEnumerable<TrainEventViewModel> GetAllEvent(int CompanyId)
        {
            int[] BookId = { 1, 8, 7 };
            var result = (from e in context.TrainEvents
                           join c in context.Providers on e.CenterId equals c.Id into p1
                           from c in p1.DefaultIfEmpty()
                           join p in context.PeopleTraining on e.Id equals p.EventId into j1
                           from p in j1.DefaultIfEmpty()
                           group p by new { p.EventId, id=e.Id, enddate=e.TrainEndDate,TrainStartDate=e.TrainStartDate, allowCandidate=e.AllowCandidate, allowEmpBook=e.AllowEmpBook , MaxBookCount=e.MaxBookCount, name=e.Name, cc = c.Name} into g
                           select new TrainEventViewModel
                           {
                               Id = g.Key.id,
                               AllowCandidate = g.Key.allowCandidate,
                               AllowEmpBook = g.Key.allowEmpBook,
                               Center = g.Key.cc,
                               Name = g.Key.name,
                               TrainEndDate = g.Key.enddate,
                               TrainStartDate = g.Key.TrainStartDate,
                               BookCount= g.Count(a => !BookId.Contains(a.ApprovalStatus)),
                               BookPercent = (g.Where(x => x.CompanyId == CompanyId).Count(a => !BookId.Contains(a.ApprovalStatus)) * 100 )/ (g.Key.MaxBookCount == 0 || g.Key.MaxBookCount == null ? 1 : g.Key.MaxBookCount.Value ),
                               AttendPercent = (g.Where(x => x.CompanyId == CompanyId).Count(a => !BookId.Contains(a.ApprovalStatus))==0? 0 : g.Where(x => x.CompanyId == CompanyId).Count(a => a.ApprovalStatus==6 && a.Status !=0) * 100 / g.Where(x => x.CompanyId == CompanyId).Count(a => !BookId.Contains(a.ApprovalStatus))),
                               SucessPercent=(g.Where(x => x.CompanyId == CompanyId).Count(a => a.ApprovalStatus == 6 && a.Status != 0)==0 ? 0 : g.Where(x => x.CompanyId == CompanyId).Count(a =>a.Status==2) *100 / g.Where(x => x.CompanyId == CompanyId).Count(a => a.ApprovalStatus == 6 && a.Status != 0))
                           }).ToList();
            return result;

        }
        public IQueryable<TrainEventViewModel> GetAllEvents()
        {
            var query = (from e in context.TrainEvents
                         where ((e.StartBookDate <= DateTime.Today && (e.EndBookDate == null || e.EndBookDate >= DateTime.Today)))
                         join c in context.Providers on e.CenterId equals c.Id into p1
                         from c in p1.DefaultIfEmpty()
                         select new TrainEventViewModel
                         {
                             Id = e.Id,
                             AllowCandidate = e.AllowCandidate,
                             AllowEmpBook = e.AllowEmpBook,
                             Center = c.Name,
                             Name = e.Name,
                             TrainEndDate = e.TrainEndDate,
                             TrainStartDate = e.TrainStartDate
                         });
            return query;
        }
        public string CheckCount(int EventId, int? PTrainId)
        {
            var record = context.TrainEvents.Where(a => a.Id == EventId).Select(a => new {max=a.MaxBookCount}).FirstOrDefault();
            if(record != null)
            {
                var EvCount = context.PeopleTraining.Where(a => a.EventId == EventId && a.Id != PTrainId).Count();
                if (EvCount >= record.max)
                    return "This Event Is Completed";
                else
                    return " ";
            }
            return " ";
        }



        public TrainEventFormViewModel ReadTrainEvent(int id)
        {
            var query = (from e in context.TrainEvents
                         where e.Id == id
                         select new TrainEventFormViewModel
                         {
                             Id = e.Id,
                             Name = e.Name,
                             Adwarding = e.Adwarding,
                             AllowCandidate = e.AllowCandidate,
                             AllowEmpBook = e.AllowEmpBook,
                             CenterId = e.CenterId,
                             Cost = e.Cost,
                             CostFlag = e.CostFlag,
                             CourseId = e.CourseId,
                             Curr = e.Curr,
                             CurrRate = e.CurrRate,
                             Description = e.Description,
                             Internal = e.Internal,
                             EndBookDate = e.EndBookDate,
                             MinBookCount = e.MinBookCount,
                             StartBookDate = e.StartBookDate,
                             CreatedTime = e.CreatedTime,
                             CreatedUser = e.CreatedUser,
                             MaxBookCount = e.MaxBookCount,
                             ModifiedTime = e.ModifiedTime,
                             ModifiedUser = e.ModifiedUser,
                             Notes = e.Notes,
                             PeriodId = e.PeriodId,
                             ResponsbleId = e.ResponsbleId,
                             TrainEndDate = e.TrainEndDate,
                             TrainStartDate = e.TrainStartDate
                        }).FirstOrDefault();
            return query;
        }
        public TrainEvent GetTrainEvent(int? id)
        {
            return Context.Set<TrainEvent>().Find(id);
        }
        public void Add(TrainEvent trainEvent)
        {
            context.TrainEvents.Add(trainEvent);
        }
        public void Attach(TrainEvent trainEvent)
        {
            context.TrainEvents.Attach(trainEvent);
        }
        public DbEntityEntry<TrainEvent> Entry(TrainEvent trainEvent)
        {
            return Context.Entry(trainEvent);
        }
        public void Remove(TrainEvent trainEvent)
        {
            if (Context.Entry(trainEvent).State == EntityState.Detached)
            {
                context.TrainEvents.Attach(trainEvent);
            }
            context.TrainEvents.Remove(trainEvent);
        }

        public int  GetPeriod(DateTime DateValue)
        {
            var  q = (from y in context.FiscalYears
                    where y.StartDate <= DateValue && y.EndDate >= DateValue
                    select new
                    {
                        id=y.Id
                    }).FirstOrDefault();

            if (q != null)
                return q.id;
            else
                return 0;
        }


        #endregion



    }
}
