using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Db.Persistence.Repositories
{
    class DisciplineRepository : Repository<Model.Domain.DisplinPeriod>, IDisciplineRepository
    {
        public DisciplineRepository(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public IQueryable<DisplinPeriodViewModel> ReadDisplinePeriod()
        {
            var Displin = from c in context.DisplinPeriods
                          where (c.StartDate <= DateTime.Today && (c.EndDate == null || c.EndDate >= DateTime.Today))
                          select new DisplinPeriodViewModel
                          {
                              Id = c.Id,
                              Frequency = c.Frequency,
                              EndDate = c.EndDate,
                              MaxPoints = c.MaxPoints,
                              Name = c.Name,
                              SysType = c.SysType,
                              StartDate = c.StartDate,
                              PointsAdd = c.PointsAdd,
                              Times = c.Times,
                              MaxDaysDeduction = c.MaxDaysDeduction,
                              CreatedUser = c.CreatedUser,
                              ModifiedUser = c.ModifiedUser,
                              ModifiedTime = c.ModifiedTime,
                              CreatedTime = c.CreatedTime
                          };
            return Displin;
        }
        public IQueryable<DisciplineViewModel> ReadDiscipline(int CompanyId)
        {
            var Displin = from c in context.Disciplines
                          where (((c.IsLocal && c.CompanyId == CompanyId) || c.IsLocal == false) && (c.StartDate <= DateTime.Today && (c.EndDate == null || c.EndDate >= DateTime.Today)))
                          select new DisciplineViewModel
                          {
                              Id = c.Id,
                              Code = c.Code,
                              Name = c.Name,
                              Description = c.Description,
                              EndDate = c.EndDate,
                              IsLocal = c.IsLocal,
                              DisciplineClass = c.DisciplineClass,
                              PeriodId = c.PeriodId,
                              StartDate = c.StartDate,
                              CreatedUser = c.CreatedUser,
                              CreatedTime = c.CreatedTime,
                              ModifiedTime = c.ModifiedTime,
                              ModifiedUser = c.ModifiedUser
                          };
            return Displin;
        }
        public IQueryable<DisplinePeriodNoViewModel> ReadDisPeriodNo(int Id)
        {
            var Displin = from c in context.DisPeriodNo
                          where c.PeriodId == Id
                          select new DisplinePeriodNoViewModel
                          {
                              Id = c.Id,
                              PeriodNo = c.PeriodNo,
                              PeriodEDate = c.PeriodEDate,
                              PeriodId = c.PeriodId,
                              Name = c.Name,
                              PostDate = c.PostDate,
                              PeriodSDate = c.PeriodSDate,
                              Posted = c.Posted,
                              CreatedUser = c.CreatedUser,
                              CreatedTime = c.CreatedTime,

                          };
            return Displin;
        }
        public IQueryable<DisplinRangeViewModel> ReadDisplinRange(int Id)
        {
            var Displin = from c in context.DisplinRanges
                          where c.DisPeriodId == Id
                          select new DisplinRangeViewModel
                          {
                              Id = c.Id,
                              DisPeriodId = c.DisPeriodId,
                              FromPoint = c.FromPoint,
                              Percentage = c.Percentage,
                              ToPoint = c.ToPoint,
                              CreatedUser = c.CreatedUser,
                              CreatedTime = c.CreatedTime,
                              ModifiedUser = c.ModifiedUser,
                              ModifiedTime = c.ModifiedTime

                          };

            return Displin;
        }
        public IQueryable<DisplinRepeatViewModel> ReadDisRepeat(int Id)
        {
            var Displin = from c in context.DisplinRepeats
                          where c.DisplinId == Id
                          select new DisplinRepeatViewModel
                          {
                              Id = c.Id,
                              DisplinId = c.DisplinId,
                              DenyPeriod = c.DenyPeriod,
                              DisplinType = c.DisplinType,
                              NofDays = c.NofDays,
                              RepNo = c.RepNo,
                              Description = c.Description, 
                              CreatedUser = c.CreatedUser,
                              CreatedTime = c.CreatedTime,
                              ModifiedUser = c.ModifiedUser,
                              ModifiedTime = c.ModifiedTime


                          };
            return Displin;
        }
        public IQueryable<EmpDisciplineViewModel> ReadEmpDiscipline(string culture, int CompanuId)
        {
            var EmpDisplin = from c in context.EmpDisciplines
                             join e in context.People on c.EmpId equals e.Id
                             join a in context.Assignments on e.Id equals a.EmpId
                             where (a.CompanyId == CompanuId && a.AssignDate <= DateTime.Today.Date && a.EndDate >= DateTime.Today.Date)
                             select new EmpDisciplineViewModel
                             {
                                 Id = c.Id,
                                 ActDispline = c.ActDispline.Value,
                                 ActualNofDays = c.ActualNofDays,
                                 ActualPeriod = c.ActualPeriod,
                                 DeductPoint = c.DeductPoint,
                                 DescionDate = c.DescionDate,
                                 DescionNo = c.DescionNo,
                                 SuggPeriod = c.SuggPeriod,
                                 Defense = c.Defense,
                                 Description = c.Description,
                                 EmpId = HrContext.TrlsName(e.Title + " " + e.FirstName + " " + e.Familyname, culture),
                                 DiscplinId = c.DiscplinId,
                                 ViolDate = c.ViolDate,
                                 Summary = c.Summary,
                                 SuggDispline = c.SuggDispline,
                                 EffectEDate = c.EffectEDate,
                                 Manager = c.Manager,
                                 SuggNofDays = c.SuggNofDays,
                                 DeptId = a.DepartmentId,
                                 BranchId = a.BranchId,
                                 PositionId = a.PositionId,
                                 Image = HrContext.GetDoc("EmployeePic", c.EmpId),
                                 Gender = e.Gender,
                                 EmpStatus = HrContext.GetEmpStatus(e.Id),
                                 empId = c.EmpId
                             };

            return EmpDisplin;
        }
        public EmpDisciplineFormViewModel ReadEmployeeDiscipline(int Id)
        {
            var c = context.EmpDisciplines.FirstOrDefault(a => a.Id == Id);
            var query = new EmpDisciplineFormViewModel
            {
                Id = c.Id,
                ActDispline = c.ActDispline,
                ActualNofDays = c.ActualNofDays,
                ActualPeriod = c.ActualPeriod,
                DeductPoint = c.DeductPoint,
                DescionDate = c.DescionDate,
                DescionNo = c.DescionNo,
                SuggPeriod = c.SuggPeriod,
                Defense = c.Defense,
                Description = c.Description,
                EmpId = c.EmpId,
                DiscplinId = c.DiscplinId,
                ViolDate = c.ViolDate,
                Summary = c.Summary,
                SuggDispline = c.SuggDispline,
                EffectEDate = c.EffectEDate,
                Manager = c.Manager,
                Attachments = HrContext.GetAttachments("EmpDisciplines", c.Id),
                SuggNofDays = c.SuggNofDays,
                IWitness = c.Witness == null ? null : c.Witness.Split(',').Select(int.Parse).ToList(),
                ModifiedTime = c.ModifiedTime,
                ModifiedUser = c.ModifiedUser,
                CreatedUser = c.CreatedUser,
                CreatedTime = c.CreatedTime,
                InvestigatId = c.InvestigatId
            };
            return query;
        }
        public IQueryable<SysDisciplinePeriodViewModel> SysDiscipline()
        {
            var result = from c in context.Disciplines
                         where c.EndDate >= DateTime.Now || c.EndDate == null
                         join t in context.DisplinPeriods on c.PeriodId equals t.Id into g
                         from t in g.DefaultIfEmpty()
                         select new SysDisciplinePeriodViewModel
                         {
                             Id = c.Id,
                             SysType = t.SysType,
                             Name = c.Name
                         };

            return result;
        }
        public List<EmployeePointsViewModel> ReadEmployeePoints(int periodId)
        {
            List<EmployeePointsViewModel> ListEmpPoints = new List<EmployeePointsViewModel>();
            //Get PeriodId && Period Name
            var PeriodObj = context.DisPeriodNo.Where(a => a.Id == periodId).Select(a => new { a.Id, a.Name, a.PeriodId }).FirstOrDefault();
            //Get PointAdd
            int? PointAdd = context.DisplinPeriods.Where(a => a.Id == PeriodObj.PeriodId).Select(s => s.PointsAdd).FirstOrDefault();
            //Get List of Emp desplin
            var EmpDisplin = context.EmpDisciplines.ToList();
            //List of Person
            var ListOfEmployees = (from p in context.People
                                   join e in context.Employements on p.Id equals e.EmpId into g
                                   from em in g.Where(s => s.Status == 1).DefaultIfEmpty()
                                   join a in context.Assignments on p.Id equals a.EmpId into g1
                                   from d in g1.Where(x => x.AssignDate <= DateTime.Today && x.EndDate >= DateTime.Today).DefaultIfEmpty()
                                   select new PeopleGridViewModel
                                   {
                                       Id = p.Id,
                                       EmpId = p.Id,
                                       Employee = p.FirstName + " " + p.Familyname
                                   }).ToList();
            var EmpPoint = context.EmpPoints.ToList();
            string EmpName = "";
            int EmpId = 0;
            foreach (var obj in ListOfEmployees)
            {
                var check = EmpDisplin.Where(a => a.EmpId == obj.Id).FirstOrDefault();
                if (check != null)
                {
                    var List = EmpDisplin.Where(a => a.EmpId == obj.Id && a.PeriodId == PeriodObj.Id).ToList();
                    var balance = EmpPoint.Where(a => a.EmpId == obj.Id && a.PeriodId == PeriodObj.Id).Select(a => a.Balance).LastOrDefault();
                    if (List.Count() != 0)
                    {
                        EmpName = obj.Employee;
                        EmpId = obj.Id;
                        int? TotalDeduct = 0;
                        int? Balance = 0;
                        foreach (var item in List)
                        {
                            TotalDeduct = TotalDeduct + item.DeductPoint;

                        }
                        if (EmpPoint.Count() != 0)
                        {
                            if (TotalDeduct == null)
                            {
                                Balance = balance + PointAdd;
                                if (Balance > 100)
                                    Balance = 100;
                            }
                            else
                            {
                                Balance = 100 - TotalDeduct;
                                if (balance < 0)
                                    balance = 0;
                            }
                        }
                        else
                            Balance = 100;
                        EmployeePointsViewModel empPoint = new EmployeePointsViewModel
                        {
                            TotalDeduction = TotalDeduct == null ? 0 : TotalDeduct,
                            Emp = EmpName,
                            Period = PeriodObj.Name,
                            PointsAdd = PointAdd,
                            Balance = Balance,
                            EmpId = EmpId,
                            PeriodId = PeriodObj.Id

                        };
                        ListEmpPoints.Add(empPoint);

                    }
                }

            }

            return ListEmpPoints;
        }
        public DeciplineInfoViewModel GetDesplinInfo(string violdata, int desplinId, string culture)
        {
            DateTime dt = Convert.ToDateTime(violdata);
            var repeatObj = new DisplinRepeat();
            var SuggestDisplin = new List<DisplinDLLViewModel>();
            var result1 = (from d in context.Disciplines
                           join p in context.DisplinPeriods on d.PeriodId equals p.Id
                           join n in context.DisPeriodNo on p.Id equals n.PeriodId
                           where n.PeriodSDate <= dt && n.PeriodEDate >= dt && n.Posted == false
                           select new PeriodIDTypeViewModel
                           {
                               Id = d.Id,
                               PeriodId = n.PeriodId,
                               SysType = p.SysType,
                               DisPeriodNOId = n.Id
                           }).ToList();
            var Actdisplin = context.DisplinRepeats.Where(a => a.DisplinId == desplinId).Select(a => new DisplinDLLViewModel { id = a.Id, name = a.Description, value = a.DisplinType }).ToList();
            var record = result1.Where(s => s.SysType == 2 && s.Id == desplinId).FirstOrDefault();
            var recordPoint = result1.Where(s => s.SysType == 1 && s.Id == desplinId).FirstOrDefault();

            if (record != null)
            {
                int? periodId = record.PeriodId;

                var result2 = (from e in context.EmpDisciplines
                               join d in context.Disciplines on e.DiscplinId equals d.Id
                               join p in context.DisplinPeriods on d.PeriodId equals p.Id
                               where p.Id == periodId
                               select new PeriodIDTypeViewModel
                               {
                                   PeriodId = d.PeriodId
                               }).ToList().Count();

                var repNo = result2 + 1;

                var Repnum = context.DisplinRepeats.Max(a => a.RepNo);

                repeatObj = context.DisplinRepeats.Where(s => s.RepNo == repNo && s.DisplinId == desplinId).FirstOrDefault();
                SuggestDisplin = context.DisplinRepeats.Where(a => a.DisplinId == desplinId && a.RepNo == repNo).Select(a => new DisplinDLLViewModel { id = a.Id, name = HrContext.GetLookUpUserCode("DisplinType", a.DisplinType, culture) }).ToList();

                if (repeatObj == null)
                {
                    repeatObj = context.DisplinRepeats.Where(s => s.RepNo == Repnum && s.DisplinId == desplinId).FirstOrDefault();
                    SuggestDisplin = context.DisplinRepeats.Where(a => a.DisplinId == desplinId && a.RepNo == Repnum).Select(a => new DisplinDLLViewModel { id = a.Id, name = HrContext.GetLookUpUserCode("DisplinType", a.DisplinType, culture) }).ToList();

                }
            }
            DeciplineInfoViewModel disInfo = new DeciplineInfoViewModel { periodmodel = record, periodPoint = recordPoint, ReapetObj = repeatObj, ActDisplinDDl = Actdisplin, SuggDisplinDDl = SuggestDisplin };

            return disInfo;
        }

        public List<DisplinDLLViewModel> GetDisplinDDl(int desplinId, string culture)
        {
            var Actdisplin = context.DisplinRepeats.Where(a => a.DisplinId == desplinId).Select(a => new DisplinDLLViewModel { id = a.Id, name = HrContext.GetLookUpUserCode("DisplinType", a.DisplinType, culture), value = a.DisplinType }).ToList();
            return Actdisplin;
        }
        public IEnumerable ReadPeriods()
        {
            var query = from p in context.DisPeriodNo
                        where p.Posted == false
                        join D in context.DisplinPeriods on p.PeriodId equals D.Id
                        where D.SysType == 1
                        select new { value = p.Id, text = p.Name };
            return query;
        }

        #region Investigations
        public IQueryable<InvesigationIndexViewModel> ReadInvestigation(string culture, int companyId)
        {
            var Investigations = from c in context.Investigations
                                 where c.CompanyId == companyId
                                 select new InvesigationIndexViewModel
                                 {
                                     Id = c.Id,
                                     InvestDate = c.InvestDate,
                                     Name = c.Name,
                                     ViolationId = c.ViolationId
                                 };
            return Investigations;
        }
        public InvestigationFormViewModel ReadInvestigations(int id)
        {
            var Investigate = (from I in context.Investigations
                               where I.Id == id
                               join e in context.InvestigatEmp on I.Id equals e.InvestigatId
                               select new InvestigationFormViewModel
                               {
                                   Id = I.Id,
                                   Accident = I.Accident,
                                   InvestDate = I.InvestDate,
                                   Defense = I.Defense,
                                   Name = I.Name,
                                   InvestResult = I.InvestResult,
                                   Notes = I.Notes,
                                   ViolationId = I.ViolationId,
                                   ViolDate = I.ViolDate,
                                   CompanyId = I.CompanyId,
                                   Employee = context.InvestigatEmp.Where(a => a.InvestigatId == id && a.EmpType == 1).Select(s => s.EmpId).ToList(),
                                   Witness = context.InvestigatEmp.Where(a => a.InvestigatId == id && a.EmpType == 2).Select(s => s.EmpId).ToList(),
                                   Judge = context.InvestigatEmp.Where(a => a.InvestigatId == id && a.EmpType == 3).Select(s => s.EmpId).ToList()
                               }).FirstOrDefault();

            return Investigate;
        }

        #endregion

        public void Add(DisPeriodNo period)
        {
            context.DisPeriodNo.Add(period);
        }
        public void Add(Investigation investigate)
        {
            context.Investigations.Add(investigate);
        }
        public void Add(InvestigatEmp investigateEmp)
        {
            context.InvestigatEmp.Add(investigateEmp);
        }
        public void Attach(InvestigatEmp investigateEmp)
        {
            context.InvestigatEmp.Attach(investigateEmp);
        }
        public DbEntityEntry<InvestigatEmp> Entry(InvestigatEmp investigateEmp)
        {
            return Context.Entry(investigateEmp);
        }
        public void Remove(InvestigatEmp investigateEmp)
        {
            if (Context.Entry(investigateEmp).State == EntityState.Detached)
            {
                context.InvestigatEmp.Attach(investigateEmp);
            }
            context.InvestigatEmp.Remove(investigateEmp);
        }
        public void Attach(DisPeriodNo period)
        {
            context.DisPeriodNo.Attach(period);
        }
        public void Attach(Investigation investigate)
        {
            context.Investigations.Attach(investigate);
        }
        public DbEntityEntry<DisPeriodNo> Entry(DisPeriodNo period)
        {
            return Context.Entry(period);
        }
        public DbEntityEntry<Investigation> Entry(Investigation investigate)
        {
            return Context.Entry(investigate);
        }
        public void Remove(DisPeriodNo period)
        {
            if (Context.Entry(period).State == EntityState.Detached)
            {
                context.DisPeriodNo.Attach(period);
            }
            context.DisPeriodNo.Remove(period);
        }
        public void Remove(Investigation investigate)
        {
            if (Context.Entry(investigate).State == EntityState.Detached)
            {
                context.Investigations.Attach(investigate);
            }
            context.Investigations.Remove(investigate);
        }
        public void Add(DisplinRange period)
        {
            context.DisplinRanges.Add(period);
        }
        public void Add(EmpPoints period)
        {
            context.EmpPoints.Add(period);
        }
        public void Attach(DisplinRange period)
        {
            context.DisplinRanges.Attach(period);
        }
        public DbEntityEntry<DisplinRange> Entry(DisplinRange period)
        {
            return Context.Entry(period);
        }
        public void Remove(DisplinRange period)
        {
            if (Context.Entry(period).State == EntityState.Detached)
            {
                context.DisplinRanges.Attach(period);
            }
            context.DisplinRanges.Remove(period);
        }
        public void Add(EmpDiscipline period)
        {
            context.EmpDisciplines.Add(period);
        }
        public void Attach(EmpDiscipline period)
        {
            context.EmpDisciplines.Attach(period);
        }
        public DbEntityEntry<EmpDiscipline> Entry(EmpDiscipline period)
        {
            return Context.Entry(period);
        }
        public void Remove(EmpDiscipline period)
        {
            if (Context.Entry(period).State == EntityState.Detached)
            {
                context.EmpDisciplines.Attach(period);
            }
            context.EmpDisciplines.Remove(period);
        }
        public void Add(Discipline period)
        {
            context.Disciplines.Add(period);
        }
        public void Attach(Discipline period)
        {
            context.Disciplines.Attach(period);
        }
        public DbEntityEntry<Discipline> Entry(Discipline period)
        {
            return Context.Entry(period);
        }
        public void Remove(Discipline period)
        {
            if (Context.Entry(period).State == EntityState.Detached)
            {
                context.Disciplines.Attach(period);
            }
            context.Disciplines.Remove(period);
        }
        public DisplinPeriod GetDisplinPeriod(int? id)
        {
            return Context.Set<DisplinPeriod>().Find(id);
        }
        public EmpDiscipline GetEmpDisplin(int? id)
        {
            return Context.Set<EmpDiscipline>().Find(id);
        }
        public Investigation GetEmpInvestigation(int? id)
        {
            return Context.Set<Investigation>().Find(id);
        }
        public void RemoveDisplinPeriod(int? id)
        {
            var Displine = Context.Set<DisplinPeriod>().Find(id);
            if (Displine != null) Remove(Displine);
        }

        public void RemoveRange(IEnumerable<DisPeriodNo> entities)
        {
            Context.Set<DisPeriodNo>().RemoveRange(entities);
        }

        public void RemoveEmpDisplin(int? id)
        {
            var Displine = Context.Set<EmpDiscipline>().Find(id);
            if (Displine != null) Remove(Displine);
        }
        public void RemoveEmpInvestigation(int? id)
        {
            var Invesigation = Context.Set<Investigation>().Find(id);
            if (Invesigation != null) Remove(Invesigation);
        }
        public void Add(DisplinRepeat period)
        {
            context.DisplinRepeats.Add(period);
        }
        public void Attach(DisplinRepeat period)
        {
            context.DisplinRepeats.Attach(period);
        }
        public DbEntityEntry<DisplinRepeat> Entry(DisplinRepeat period)
        {
            return Context.Entry(period);
        }
        public void Remove(DisplinRepeat period)
        {
            if (Context.Entry(period).State == EntityState.Detached)
            {
                context.DisplinRepeats.Attach(period);
            }
            context.DisplinRepeats.Remove(period);
        }
        public void RemoveDiscipline(int? id)
        {
            var Displine = Context.Set<Discipline>().Find(id);
            if (Displine != null) Remove(Displine);
        }
        public Discipline GetDiscipline(int? id)
        {
            return Context.Set<Discipline>().Find(id);
        }
        public void RemoveRange(IEnumerable<DisplinRepeat> entities)
        {
            Context.Set<DisplinRepeat>().RemoveRange(entities);
        }


    }

}
