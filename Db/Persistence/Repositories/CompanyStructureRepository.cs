using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace Db.Persistence.Repositories
{
   class CompanyStructureRepository : Repository<CompanyStructure>,ICompanyStructureRepository
    {
        public CompanyStructureRepository(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public IQueryable<FormList> GetAllActiveCompanyStructure(int companyId,string Culture)
        {
            return (from c in context.CompanyStructures
                    where c.CompanyId == companyId && (c.StartDate <= DateTime.Now && (c.EndDate == null || c.EndDate >= DateTime.Now))
                    select new FormList
                    {
                        id = c.Id,
                        name = HrContext.TrlsName(c.Name, Culture),
                      
                    });
        }
        public IQueryable<CompanyDiagramViewModel> GetDiagram(int companyId, string Culture)
        {
            var compDiag = from s in context.CompanyStructures
                           where ((s.CompanyId == companyId) && (s.StartDate <= DateTime.Today && (s.EndDate == null || s.EndDate >= DateTime.Today)))
                           join a in context.Assignments on s.Id equals a.DepartmentId into g
                           from f in g.Where(c => c.CompanyId == companyId && c.AssignDate <= DateTime.Today.Date && c.EndDate >= DateTime.Today.Date && c.IsDepManager).DefaultIfEmpty()
                           orderby s.Sort
                           select new CompanyDiagramViewModel
                           {
                               Id = s.Id,
                               Image = f != null ? HrContext.GetDoc("EmployeePic", f.EmpId) : null,
                               Employee = f != null ? HrContext.TrlsName(f.Employee.Title + " " + f.Employee.FirstName + " " + f.Employee.Familyname, Culture) : null,
                               Name = HrContext.TrlsName(f == null ? s.Name : f.Department.Name, Culture),
                               ParentId = s.ParentId,
                               colorSchema = s.ColorName == null ? "#55dd28" : s.ColorName,
                               HasImage = f != null ? f.Employee.HasImage : false,
                               Gender = f != null ? f.Employee.Gender : (short)1
                           };

            return compDiag;
        }
       
        public CompanyStructureViewModel GetStructure(int? Id,string culture)
        {
            var Compastruc = (from c in context.CompanyStructures
                              where c.Id == Id
                              select new CompanyStructureViewModel
                              {
                                  Id = c.Id,
                                  Code = c.Code,
                                  ModifiedTime = c.ModifiedTime,
                                  ModifiedUser = c.ModifiedUser,
                                  Name = c.Name,
                                  ParentId = c.ParentId,
                                  PlannedCount = c.PlannedCount,
                                  ParentName = c.Parent.Name,
                                  NodeType = c.NodeType,
                                  StartDate = c.StartDate,
                                  EndDate = c.EndDate,
                                  Icon = c.Icon,
                                  IsVisible = c.IsVisible,
                                  ColorName = c.ColorName != null? c.ColorName:"",
                                  CreatedTime=c.CreatedTime,
                                  CreatedUser=c.CreatedUser,
                                  LocalName = HrContext.TrlsName(c.Name,culture),
                              }).FirstOrDefault();
            return Compastruc;
        }

        public IQueryable<FormList> GetAllDepartments(int CompanyId, int? DeptId, string Culture)
        {
            if (DeptId != null)
                return context.CompanyStructures.Where(a => (a.CompanyId == CompanyId) && (a.StartDate <= DateTime.Today.Date && (a.EndDate == null || a.EndDate >= DateTime.Today.Date)) || (a.Id == DeptId)).Select(a => new FormList { id = a.Id, name = HrContext.TrlsName(a.Name, Culture) });
            else
                return context.CompanyStructures.Where(a => (a.CompanyId == CompanyId) && (a.StartDate <= DateTime.Today.Date && (a.EndDate == null || a.EndDate >= DateTime.Today.Date))).Select(a => new FormList { id = a.Id, name = HrContext.TrlsName(a.Name, Culture) });
        }
        #region LeavePlan
        public IQueryable<DeptLeavePlanViewModel> GetDeptLeavePlan(int companyId, int DeptId, string Culture)
        {
            var query = (from a in context.DeptJobLeavePlans
                         where a.CompanyId == companyId && a.DeptId == DeptId && a.ToDate >= DateTime.Today
                         group a by new { a.CompanyId, a.DeptId, a.FromDate, a.ToDate, a.Stars } into g
                         select new DeptLeavePlanViewModel
                         {
                             Id = g.FirstOrDefault().Id,
                             DeptId = g.Key.DeptId,
                             FromDate = g.Key.FromDate,
                             ToDate = g.Key.ToDate,
                             Stars = g.Key.Stars
                         }); //.Distinct();
            return query;
        }

        public IQueryable<DeptJobLvPlanViewModel> GetJobLeavePlan(int CompanyId, int DeptId, DateTime? FromDate, DateTime? ToDate, string Culture)
        {
            var activeJobs = context.Assignments.Where(j => j.AssignDate <= DateTime.Today && j.EndDate >= DateTime.Today && j.CompanyId == CompanyId && j.DepartmentId == DeptId).Select(j => j.JobId).Distinct().ToList();

            var query = (from j in context.Jobs where activeJobs.Contains(j.Id)
                join a in context.DeptJobLeavePlans on j.Id equals a.JobId into lg
                from a in lg.Where(d => d.CompanyId == CompanyId && d.DeptId == DeptId && d.FromDate == FromDate && d.ToDate == ToDate).DefaultIfEmpty()
                select new DeptJobLvPlanViewModel
                {
                    Id = a == null ? 0 : a.Id,
                    DeptId = DeptId,
                    JobId = j.Id,
                    Job = HrContext.TrlsName(j.Name, Culture),
                    MinAllowPercent = a == null ? 0 : a.MinAllowPercent
                });

            return query;
        }
       
        public void Add(DeptJobLeavePlan deptLeavePlan)
        {
            context.DeptJobLeavePlans.Add(deptLeavePlan);
        }
        public void Attach(DeptJobLeavePlan deptLeavePlan)
        {
            context.DeptJobLeavePlans.Attach(deptLeavePlan);
        }
        public DbEntityEntry<DeptJobLeavePlan> Entry(DeptJobLeavePlan deptLeavePlan)
        {
            return Context.Entry(deptLeavePlan);
        }
        public void Remove(DeptJobLeavePlan deptLeavePlan)
        {
            if (Context.Entry(deptLeavePlan).State == EntityState.Detached)
            {
                context.DeptJobLeavePlans.Attach(deptLeavePlan);
            }
            context.DeptJobLeavePlans.Remove(deptLeavePlan);
        }

        public int CheckLeaveRequests(int companyId, IEnumerable<DeptLeavePlanViewModel> plans)
        {
            int deptId = plans.FirstOrDefault().DeptId;
            string jobs = string.Join(",", plans.Select(a => a.JobId).Distinct());
            var dates = plans.Select(p => new { p.FromDate, p.ToDate }).Distinct().ToList();

            StringBuilder sql = new StringBuilder("SELECT l.Id FROM LeaveRequests l, Assignments a WHERE a.CompanyId = " + companyId +" AND GETDATE() BETWEEN a.AssignDate and a.EndDate AND " 
                + " a.DepartmentId = " + deptId +" AND a.JobId in ("+ jobs +") AND l.EmpId = a.EmpId AND l.ApprovalStatus <= 6 ");

            for (int i = 0; i < dates.Count; i++)
            {
                var item = dates[i];

                sql.Append(i > 0 ? " OR " : " AND (");

                sql.Append(" '" +
                    item.FromDate.ToString("yyyy-MM-dd") + "' BETWEEN l.StartDate AND l.EndDate OR '" +
                    item.ToDate.ToString("yyyy-MM-dd") + "' BETWEEN l.StartDate AND l.EndDate "
                    + " OR l.StartDate BETWEEN '" + item.FromDate.ToString("yyyy-MM-dd") + "' AND '" + item.ToDate.ToString("yyyy-MM-dd")
                    + "' OR l.EndDate BETWEEN '" + item.FromDate.ToString("yyyy-MM-dd") + "' AND '" + item.ToDate.ToString("yyyy-MM-dd") + "'"
                    );
            }
            sql.Append(" )");

            return context.Database.SqlQuery<EmpLeaveDays>(sql.ToString()).ToList().Count;
        }

        public JobPercentChartVM GetJobsPercentage(int DeptId, int Week, int CompanyId, string Culture)
        {
            JobPercentChartVM result = new JobPercentChartVM();

            //Check Jobs
            var jobs = context.Assignments.Where(a => a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today && a.CompanyId == CompanyId && a.DepartmentId == DeptId)
                .Select(a => new { id = a.JobId, name = HrContext.TrlsName(a.Job.Name, Culture) }).Distinct().ToList();
            if (jobs.Count == 0)
            {
                result.message = MsgUtils.Instance.Trls(Culture, "DeptIsEmpty");
                return result;
            }

            #region ---Get Week Range
            DateTime day = DateTime.Today, firstDW, lastDW;

            day = day.AddDays(Week * 7); //for next/prev button

            PersonSetup personnel = GetPersonSetup(CompanyId);
            byte fWorkDay = (personnel?.Weekend2 ?? personnel?.Weekend1) ?? 0;
            fWorkDay = (byte)(fWorkDay == 6 ? 0 : fWorkDay + 1);
            byte endWeek = 6; //working days only
            if (personnel?.Weekend2 != null) endWeek--;
            if (personnel?.Weekend1 != null) endWeek--;

            int diff = ((int)day.DayOfWeek - fWorkDay);
            if (diff < 0) diff += 7;
            firstDW = day.AddDays(-1 * diff).Date;
            lastDW = day.AddDays(endWeek - (diff)).Date;

            #endregion

            List<int> jobsIds = jobs.Select(a => a.id).ToList();
            ///---Get Data
            var query = context.DeptJobLeavePlans.Where(d => d.CompanyId == CompanyId && d.DeptId == DeptId && jobsIds.Contains(d.JobId))
                .Select(d => new { Job = HrContext.TrlsName(d.Job.Name, Culture), d.FromDate, d.ToDate, d.MinAllowPercent });

            List<ChartViewModel> chartData = new List<ChartViewModel>();

            while (firstDW <= lastDW)
            {
                var jobsInDate = query.Where(d => d.FromDate <= firstDW && d.ToDate >= firstDW).ToList();

                if (jobsInDate.Count > 0)
                {
                    chartData.AddRange(jobsInDate.Select(d => 
                        new ChartViewModel { myGroup = d.Job, dateCategory = firstDW, floatValue = d.MinAllowPercent }));
                } 
                else
                {
                    foreach (var job in jobs)
                    {
                        chartData.Add(new ChartViewModel { myGroup = job.name, dateCategory = firstDW, floatValue = 0 });
                    }  
                }
                firstDW = firstDW.AddDays(1);
            }

            result.chartData = chartData.ToList();
            return result;
        }

        #endregion


    }
}
