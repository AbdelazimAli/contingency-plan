using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Interface.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int? id);
        IEnumerable<TEntity> GetAll();
        void AddTrail(AddTrailViewModel trailVM);
        bool IsExist(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Attach(TEntity entity);
        //  void AddLName(string culture, string name, string lname);
        void RemoveLName(string Culture, string Name);
        void AddLName(string culture, string oldName, string newName, string lname);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(int? id);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        DbEntityEntry<TEntity> Entry(TEntity entity);

        PersonSetup GetPersonSetup(int company);
        void SetPersonSetup(PersonSetup setup);
        ICacheManager CacheManager { get; }

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        IEnumerable GetLookUpCode(string culture, string code);
        IEnumerable GetGridLookUpCode(string culture, string code);

        IList<Model.ViewModel.Personnel.RolesViewModel> GetOrgChartRoles(string culture);
        WfTrans AddWorkFlow(WfViewModel wf, string culture);
        WfTrans GetWorkFlow(string Source, int SourceId, int DocumentId);

        int GetIntResultFromSql(string Sql);

        List<Error> Check(CheckParm parm);
        List<Error> CheckForm(CheckParm parm);
        List<Error> CheckGrid(CheckParm parm);
        List<Error> CheckPage(IEnumerable<ColumnInfoViewModel> query, CheckParm parm);
        IQueryable<AuditViewModel> GetLog(int companyId, string[] objects, byte version, string culture, string Id);

        IQueryable<FlexDataViewModel> GetFlexData(int companyId, string objectName, byte version, string culture, int SourceId);
        List<string> GetAutoCompleteColumns(string objectName, int compnayId, byte version);

        void RequestRangeFilter(byte range, int companyId, out DateTime? Start, out DateTime? End);
    }
}
