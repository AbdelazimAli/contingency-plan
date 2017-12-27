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
   public interface IJobRepository : IRepository<Job>
    {
        IQueryable<FormList> GetJobClass(int companyId);
        IQueryable GetPayrollGrade();
        // IQueryable<JobGridViewModel> ReadJobs(int company);
        IEnumerable<ExcelJobViewModel> ReadExcelJobs(int company, string culture);
        IEnumerable<JobGridViewModel> ReadJobs(int company, string culture,int Id);
        JobViewModel ReadJob(int id, string culture);
        //List<JobClass> AddJobClass(IEnumerable<int> JobClassesIds);
        Job JobObject(int id);
        IQueryable<CareerPathViewModel> ReadCareerPaths(int comanyId);
        void Add(CareerPath job);

        void Add(CareerPathJobs CareerPJobs);
        void Add(Period period);
        void Add(SubPeriod subperiod);
        SubPeriod GetSubPeriod(int? id);
        void Attach(Period period);
        DbEntityEntry<SubPeriod> Entry(SubPeriod subperiod);

        void Attach(SubPeriod subperiod);
        DbEntityEntry<Period> Entry(Period period);
        
        void Attach(CareerPath job);

        void Attach(CareerPathJobs CareerPJobs);

        void Remove(CareerPath job);
        void Remove(Period period);
        void Remove(SubPeriod subperiod);
        void Remove(CareerPathJobs CareerPJobs);
        IQueryable<CareerPathJobsViewModel> ReadCareerJobs(int id,string culture);
        DbEntityEntry<CareerPath> Entry(CareerPath careerPath);

        DbEntityEntry<CareerPathJobs> Entry(CareerPathJobs CareerPjob);

        void RemoveRange(IEnumerable<CareerPath> entities);

        void RemoveRange(IEnumerable<CareerPathJobs> entities);
        CareerPath Find(int id);
      
    }
}
