using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace Interface.Core.Repositories
{
   public interface IQualificationRepository:IRepository<Qualification>
    {
        void Add(QualGroup qualGroup);
        void Attach(QualGroup qualGroup);
        IQueryable<QualGroupsViewModel> GetQualGroups();
        DbEntityEntry<QualGroup> Entry(QualGroup qualGroup);
        //void Add(Qualification qualification);
        //void Attach(Qualification qualification);
        //DbEntityEntry<Qualification> Entry(Qualification qualification);
       
        void Remove(QualGroup QualGroup);
        IQueryable<QualificationViewModel> GetQualifications(int Id);
    
      //  void Remove(Qualification Qualification);
        IQueryable<SchoolViewModel> GetSchools();
        void Remove(School school);
        void Add(School school);

        void Attach(School school);

        DbEntityEntry<School> Entry(School school);
      
    }
}
