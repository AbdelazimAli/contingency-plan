using Model.Domain;
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
    public interface ITrainingRepository : IRepository<TrainCourse>

    {
        #region
        IEnumerable<EmployeeTrainPathViewModel> GetEmpsTrainpaths(string culture, int companyId);
        IEnumerable<TrainCourseViewModel> GetMissCourses(int id, int[] courses, string Culture);
        IQueryable<TrainCourseViewModel> GetTrainCourse(string culture,int companyId);
      //  IEnumerable<EmployeeTrainPathViewModel> GetEmpsTrainpaths(string culture);
        IQueryable<PeoplesViewModel> GetPeople(string culture);
        TrainCourseFormViewModel ReadTrainCourse(int Id, string culture);
        IEnumerable<EmployeeTrainPathViewModel> TrainPathProgress(int id,string culture);
        TrainCourse GetTrainCourse(int? id);
        IQueryable<TrainPathViewModel> GetTrainPath(string culture,int companyId);
        void Add(TrainPath trainPath);
        DbEntityEntry<TrainPath> Entry(TrainPath trainPath);
        void Attach(TrainPath trainPath);
        void Remove(TrainPath trainPath);
        TrainPathFormViewModel ReadTrainPath(int Id, string culture);
        TrainPath GetTrainPath(int? id);
        IQueryable<TrainPathCourseViewModel> GetCours(int Id);
        TrainPath Courses(int id);
        IQueryable<TrainCourseViewModel> GetTrainCourseLST(string culture, int CompanyId, bool IsLocal);

        #endregion
        #region Event
        IEnumerable<TrainEventViewModel> GetAllEvent(int CompanyId);
        IQueryable<TrainEventViewModel> GetAllEvents();
        TrainEventFormViewModel ReadTrainEvent(int id);
        TrainEvent GetTrainEvent(int? id);
        void Add(TrainEvent trainEvent);
        string CheckCount(int EventId,int? PTrainId);
        DbEntityEntry<TrainEvent> Entry(TrainEvent trainEvent);
        void Attach(TrainEvent trainEvent);
        void Remove(TrainEvent trainEvent);
        int GetPeriod(DateTime DateValue);


        #endregion

        #region PeopleTrain
        string GetMenuId(int MenuId, int companyId);
        IQueryable<TrainIndexFollowUpViewModel> GetPeopleTrain(string culture, int CompanyId);
        PeopleTrainFormViewModel ReadPeopleTrain(int Id, string culture);
        void Add(PeopleTraining peopleTraining);
        DbEntityEntry<PeopleTraining> Entry(PeopleTraining peopleTraining);
        void Attach(PeopleTraining peopleTraining);
        void Remove(PeopleTraining peopleTraining);
        PeopleTraining GetpeopleTraining(int? id);
        IQueryable<TrainIndexFollowUpViewModel> GetTrainFollowUp(int companyId, string culture);
        PeopleTraining GetPeopleTraining(int? id);

        #endregion


    }
}

