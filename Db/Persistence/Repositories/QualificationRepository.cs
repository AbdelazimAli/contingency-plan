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
    class QualificationRepository :Repository<Qualification>, IQualificationRepository
    {
        public QualificationRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public void Add(QualGroup qualGroup)
        {
            context.QualGroups.Add(qualGroup);
        }
        public void Attach(QualGroup qualGroup)
        {
            context.QualGroups.Attach(qualGroup);
        }
        public DbEntityEntry<QualGroup> Entry(QualGroup qualGroup)
        {
            return Context.Entry(qualGroup);
        }
        public void Add(School school)
        {
            context.Schools.Add(school);
        }
        public void Attach(School school)
        {
            context.Schools.Attach(school);
        }
        public DbEntityEntry<School> Entry(School school)
        {
            return Context.Entry(school);
        }
        public DbEntityEntry<Qualification> Entry(Qualification qualification)
        {
            return Context.Entry(qualification);
        }
        public IQueryable<QualGroupsViewModel> GetQualGroups()
        {
            var QualGroup = from Q in context.QualGroups
                            select new QualGroupsViewModel
                            {
                                Id = Q.Id,
                                Name = Q.Name,
                                Code = Q.Code,
                                CreatedTime = Q.CreatedTime,
                                CreatedUser=Q.CreatedUser,
                                ModifiedTime=Q.ModifiedTime,
                                ModifiedUser=Q.ModifiedUser                          
                         };
            return QualGroup;
        }
        public IQueryable<SchoolViewModel> GetSchools()
        {
            var Schools = from S in context.Schools
                            select new SchoolViewModel
                            {
                               Id = S.Id,
                               Classification = S.Classification,
                               Name = S.Name,
                               SchoolType = S.SchoolType,
                               CreatedUser=S.CreatedUser,
                               CreatedTime=S.CreatedTime,
                               ModifiedTime=S.ModifiedTime,
                               ModifiedUser=S.ModifiedUser  
                            };
            return Schools;
        }
        public void Remove(QualGroup qualGroup)
        {
            //  var QualifList = context.Qualifications.Where(q => q.QualGroupId == qualGroup.Id).ToList();
            //  if(QualifList.Count()>=0)
            //  {
            //      foreach (var item in QualifList)
            //      {
            //          if (Context.Entry(item).State == EntityState.Detached)
            //          {
            //              context.Qualifications.Attach(item);
            //          }
            //          context.Qualifications.Remove(item);
            //      }
            //  }
            //if (Context.Entry(qualGroup).State == EntityState.Detached)
            // {
            //   context.QualGroups.Attach(qualGroup);
            // }
            //context.QualGroups.Remove(qualGroup);
            if (Context.Entry(qualGroup).State == EntityState.Detached)
            {
                context.QualGroups.Attach(qualGroup);
            }
            context.QualGroups.Remove(qualGroup);
        }
        public void Remove(School school)
        {
            if (Context.Entry(school).State == EntityState.Detached)
            {
                context.Schools.Attach(school);
            }
            context.Schools.Remove(school);
        }
        public IQueryable<QualificationViewModel> GetQualifications(int Id)
        {
            return context.Qualifications.Where(Q => Q.QualGroupId == Id ).Select(Qu => new QualificationViewModel
            {
                Id = Qu.Id,
                QualGroupId = Qu.QualGroupId,
                Code=Qu.Code,
                Name = Qu.Name,
                Rank=Qu.Rank,
                Category=Qu.Category,
                CreatedTime = Qu.CreatedTime,
                CreatedUser = Qu.CreatedUser,
                ModifiedTime = Qu.ModifiedTime,
                ModifiedUser = Qu.ModifiedUser
            });
        }
    }
}
