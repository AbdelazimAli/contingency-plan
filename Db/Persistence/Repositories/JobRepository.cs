using System.Collections.Generic;
using System.Linq;
using Interface.Core.Repositories;
using Model.Domain;
using System.Data.Entity;
using Model.ViewModel;
using System.Collections;
using System.Data.Entity.Infrastructure;
using Model.ViewModel.Personnel;
using System;
using Model.Domain.Payroll;
using Model.ViewModel.Administration;

namespace Db.Persistence.Repositories
{
     class JobRepository:Repository<Job>, IJobRepository
    {
        public JobRepository(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public IQueryable<FormList> GetJobClass(int companyId)
        {
             return Context.Set<JobClass>()
                      .Where(a =>((a.IsLocal && a.CompanyId == companyId) || !a.IsLocal))
                      .Select(c => new FormList { id = c.Id, name = c.Name , IsLocal = c.IsLocal });
        }
        //ReadCareerPaths
        public IQueryable<CareerPathViewModel> ReadCareerPaths(int companyId)
        {
            var carrer = (from c in context.CareerPathes
                      select new CareerPathViewModel
                      {
                          Id = c.Id,
                          CompanyId = c.CompanyId,
                          Code = c.Code,
                          Description = c.Description,
                          EndDate = c.EndDate,
                          StartDate = c.StartDate,
                          Name = c.Name,
                          IsLocal =c.IsLocal,
                          ModifiedUser=c.ModifiedUser,
                          ModifiedTime=c.ModifiedTime,
                          CreatedUser=c.CreatedUser,
                          CreatedTime=c.CreatedTime

                      }).Where(a => (a.CompanyId == companyId || a.IsLocal == false) && (a.StartDate <= DateTime.Today && (a.EndDate == null || a.EndDate >= DateTime.Today )));

            return carrer;
        }

        public IQueryable GetPayrollGrade(int companyId)
        {
            var PayrollGrade = Context.Set<PayrollGrade>()
                .Where(a => (a.CompanyId == companyId || !a.IsLocal) &&  a.StartDate <= DateTime.Today && (a.EndDate == null || a.EndDate >= DateTime.Today ))
                .Select(c => new { id = c.Id, name = c.Name });

            return PayrollGrade;
        }

        public IEnumerable<ExcelJobViewModel> ReadExcelJobs(int company, string culture)
        {
            var Job = (from J in context.Jobs
                       where (((J.IsLocal && J.CompanyId == company) || J.IsLocal == false) && (J.StartDate <= DateTime.Today && (J.EndDate == null || J.EndDate >= DateTime.Today)))
                       select new ExcelJobViewModel
                       {
                           Id = J.Id,
                           Code = J.Code,
                           Name = J.Name,
                           IsLocal = HrContext.TrlsMsg(J.IsLocal.ToString(), culture),
                           LName = HrContext.TrlsName(J.Name, culture),
                           DefaultGradeId = J.DefaultGradeId != null ? HrContext.TrlsName(J.PayrollGrade.Name, culture) : " ",
                           StartDate = J.StartDate.ToString(),
                           EndDate = J.EndDate != null ? J.EndDate.Value.ToString() : " ",
                           NameInInsurance = J.NameInInsurance,
                           PlanCount = J.PlanCount.ToString(),
                           PlanTurnOverRate = J.PlanTurnOverRate!= null? J.PlanTurnOverRate.Value.ToString():" ",
                           ProbationPeriod = J.ProbationPeriod != null? J.ProbationPeriod.Value.ToString():" ",
                           PrimaryRole = HrContext.TrlsMsg(J.PrimaryRole.ToString(), culture),
                           StartTime = J.StartTime != null ? J.StartTime.Value.ToString() : " ",
                           EndTime = J.EndTime != null ? J.EndTime.Value.ToString() : " ",
                           Frequency = J.Frequency != null ? HrContext.GetLookUpCode("Frequency", J.Frequency.Value, culture) : " ",
                           WorkHours = J.WorkHours != null? J.WorkHours.Value.ToString():" ",
                           DescInRecruitment = J.DescInRecruitment,
                           ReplacementRequired = HrContext.TrlsMsg(J.ReplacementRequired.Value.ToString(), culture),
                           EnTime = J.EndTime,
                           StarTime = J.StartTime

                       }).ToList();
            foreach (var item in Job)
            {
                item.StartTime = item.StarTime != null ? item.StarTime.Value.ToString("HH:mm") : " ";
                item.EndTime = item.EnTime != null ? item.EnTime.Value.ToString("HH:mm") : " ";
            }
            return Job;
        }

        public IQueryable<JobGridViewModel> GetAllJobs(int company,string culture , int Id)
        {
            var Job = (from J in context.Jobs
                        where ((J.CompanyId == company || J.IsLocal == false) && (J.StartDate <= DateTime.Today.Date && (J.EndDate == null || J.EndDate >= DateTime.Today.Date)) || J.Id == Id)
                        select new JobGridViewModel
                        {
                            Id = J.Id,
                            JobName = J.Name,
                            JobClasses = J.JobClasses.Select(a => a.Name),
                            JobCode = J.Code,
                            StartDate = J.StartDate,
                            EndDate = J.EndDate,
                            Islocal = J.IsLocal,
                            CompanyId = J.CompanyId,
                            LocalName=HrContext.TrlsName(J.Name,culture)
                        });

            return Job;
        }
       
        public List<JobClass> AddJobClass(IEnumerable<int> JobClassesIds)
        {
            var JClass = new List<JobClass>();
            foreach (var item in JobClassesIds)
            {

                JobClass J = context.JobClasses.Where(a => a.Id == item).FirstOrDefault();
                   
                    JClass.Add(J);

            }
            return JClass;

        }
        public Job JobObject(int id)
        {
            return  context.Jobs.Include("JobClasses").Where(j => j.Id == id).FirstOrDefault();
        }
        public JobViewModel ReadJob(int id,string culture)
        {
            var Job = (from J in context.Jobs
                       where J.Id == id
                       select new JobViewModel
                       {
                           Id = J.Id,
                           Code = J.Code,
                           Name = J.Name,
                           IsLocal = J.IsLocal,
                           LName=HrContext.TrlsName(J.Name,culture),
                           DefaultGradeId = J.DefaultGradeId,
                           IJobClasses=J.JobClasses.Select(a=>a.Id),
                           StartDate = J.StartDate,
                           EndDate = J.EndDate,
                           NameInInsurance = J.NameInInsurance,
                           PlanCount = J.PlanCount,
                           PlanTurnOverRate = J.PlanTurnOverRate,
                           ProbationPeriod = J.ProbationPeriod,
                           PrimaryRole = J.PrimaryRole,
                           StartTime = J.StartTime,
                           EndTime = J.EndTime,
                           Frequency = J.Frequency,
                           WorkHours = J.WorkHours,
                           ModifiedTime = J.ModifiedTime,
                           ModifiedUser = J.ModifiedUser,
                           CreatedTime = J.CreatedTime,
                           CreatedUser = J.CreatedUser,
                           ContractTempl=J.ContractTempl,
                           CompanyId = J.CompanyId
                       }).FirstOrDefault();

            return Job;
        }
        public void Add(CareerPath job)
        {
            context.CareerPathes.Add(job);
        }
        public void Add(Period period)
        {
            context.Periods.Add(period);
        }
        public void Add(SubPeriod subperiod)
        {
            context.SubPeriods.Add(subperiod);
        }
        public void Add(CareerPathJobs CareerPJobs)
        {
            context.CareerPathJobs.Add(CareerPJobs);
        }
        public CareerPath Find(int Id)
        {
          return context.CareerPathes.FirstOrDefault(a => a.Id == Id);
        }
        public SubPeriod GetSubPeriod(int? id)
        {
            return context.SubPeriods.Find(id);
        }
        public void Attach(Period period)
        {
            context.Periods.Attach(period);
        }
        public void Attach(CareerPath careerpath)
        {
            context.CareerPathes.Attach(careerpath);
        }
        public void Attach(SubPeriod subperiod)
        {
            context.SubPeriods.Attach(subperiod);
        }
        public void Attach(CareerPathJobs CareerPJobs)
        {
            context.CareerPathJobs.Attach(CareerPJobs);
        }
        public void Remove(CareerPath job)
        {
            if (Context.Entry(job).State == EntityState.Detached)
            {
                context.CareerPathes.Attach(job);
            }
            context.CareerPathes.Remove(job);
        }
        public void Remove(Period period)
        {
            if (Context.Entry(period).State == EntityState.Detached)
            {
                context.Periods.Attach(period);
            }
            context.Periods.Remove(period);
        }
        public void Remove(SubPeriod subperiod)
        {
            if (Context.Entry(subperiod).State == EntityState.Detached)
            {
                context.SubPeriods.Attach(subperiod);
            }
            context.SubPeriods.Remove(subperiod);
        }
        public void Remove(CareerPathJobs CareerPJobs)
        {
            if (Context.Entry(CareerPJobs).State == EntityState.Detached)
            {
                context.CareerPathJobs.Attach(CareerPJobs);
            }
            context.CareerPathJobs.Remove(CareerPJobs);
        }
        public DbEntityEntry<CareerPath> Entry(CareerPath careerPath)
        {
            return Context.Entry(careerPath);
        }
        public DbEntityEntry<Period> Entry(Period period)
        {
            return Context.Entry(period);
        }
        public DbEntityEntry<SubPeriod> Entry(SubPeriod subperiod)
        {
            return Context.Entry(subperiod);
        }
        public DbEntityEntry<CareerPathJobs> Entry(CareerPathJobs CareerPjob)
        {
            return Context.Entry(CareerPjob);
        }
        public void RemoveRange(IEnumerable<CareerPath> entities)
        {
            Context.Set<CareerPath>().RemoveRange(entities);
        }
       
        public void RemoveRange(IEnumerable<CareerPathJobs> entities)
        {
            Context.Set<CareerPathJobs>().RemoveRange(entities);
        }

        public IQueryable<CareerPathJobsViewModel> ReadCareerJobs(int id,string culture)
        {
          var Quer=  from cj in context.CareerPathJobs
                      where cj.CareerId == id                   
                   select new CareerPathJobsViewModel
                   {
                       Id = cj.Id,
                       CareerId = cj.CareerId,
                       JobId = cj.JobId,
                       FormulaId = cj.FormulaId,
                       MinYears = cj.MinYears,
                       Performance = cj.Performance,
                       ModifiedTime=cj.ModifiedTime,
                       ModifiedUser=cj.ModifiedUser,
                       CreatedUser=cj.CreatedUser,
                       CreatedTime=cj.CreatedTime,
                       Sequence=cj.Sequence
                      
                   };
            return Quer;
        }
    }
}
