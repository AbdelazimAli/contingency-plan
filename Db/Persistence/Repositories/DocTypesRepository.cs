using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Model.ViewModel.Personnel;
using Model.Domain;
using Interface.Core.Repositories;

namespace Db.Persistence.Repositories
{
    public class DocTypesRepository : Repository<DocType>, IDocTypesRepository
    {
        public DocTypesRepository(DbContext context) : base(context)
        {
        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public bool HasExpirationDate(int DocTypeID)
        {
            try
            {
                return GetAll().SingleOrDefault(a => a.Id == DocTypeID).HasExpiryDate;
            }
            catch
            {
                return false;
            }
        }
        //public IQueryable<DocTypeFormViewModel> GetPapers(int JobID, int Gender, int Nationality, int CompanyId, string Lang, List<int> Include_SystemCodes, List<int> Exclude_SystemCodes)
        //{
        //    try
        //    {
        //        return GetPapers_Search(JobID, Gender, Nationality, CompanyId, Lang, Include_SystemCodes, Exclude_SystemCodes);

        //    }
        //    catch
        //    {
        //        return new List<DocTypeFormViewModel>().AsQueryable();
        //    }
        //}


        private IQueryable<DocType> GetPapers_BasicQuery(int JobID, int Gendre, int Nationality, int CompanyId, string Lang)
        {
            try
            {
                IQueryable<DocType> BasicQuery = GetAll();

                BasicQuery = BasicQuery.Where<DocType>(d => (d.RequiredOpt == (int)Constants.Enumerations.RequiredOptEnum.Not_Required ||
                                                            (d.RequiredOpt == (int)Constants.Enumerations.RequiredOptEnum.Required_For_All_Jobs && (d.Gender == null || d.Gender == Gendre) && (!d.Nationalities.Any() || d.Nationalities.Any(n => n.Id == Nationality))) ||
                                                            (d.RequiredOpt == (int)Constants.Enumerations.RequiredOptEnum.Required_For_Some_Jobs && d.Jobs.Any(j => j.Id == JobID) && (d.Gender == null || d.Gender == Gendre) && (!d.Nationalities.Any() || d.Nationalities.Any(n => n.Id == Nationality)))));

                BasicQuery = BasicQuery.Where<DocType>(d => ((d.IsLocal && d.CompanyId == CompanyId) || d.IsLocal == false) &&
                                                            (d.StartDate <= DateTime.Today && (d.EndDate == null || d.EndDate.Value >= DateTime.Today)));



                return BasicQuery;
            }
            catch (Exception ex)
            {
                return new List<DocType>().AsQueryable();
            }
        }

        public IQueryable<DocTypeFormViewModel> GetPapers(int JobID, int Gendre, int Nationality, int CompanyId, string Lang, List<int> Include_SystemCodes, List<int> Exclude_SystemCodes)
        {
            try
            {
                IQueryable<DocType> BasicQuery = GetPapers_BasicQuery(JobID, Gendre, Nationality, CompanyId, Lang);

                var JoinedQuery = from d in BasicQuery
                                  join l in context.LookUpUserCodes on new { p1 = Constants.SystemCodes.DocType.CodeName, p2 = d.DocumenType } equals new { p1 = l.CodeName, p2 = l.CodeId }
                                  select new { d = d, l = l };

                if (Include_SystemCodes.Any())
                    JoinedQuery = JoinedQuery.Where(j => Include_SystemCodes.Contains(j.l.SysCodeId));

                if (Exclude_SystemCodes.Any())
                    JoinedQuery = JoinedQuery.Where(j => !Exclude_SystemCodes.Contains(j.l.SysCodeId));

                var FinalQuery = from x in JoinedQuery
                                 select new DocTypeFormViewModel
                                 {
                                     Id = x.d.Id,
                                     Name = HrContext.TrlsName(x.d.Name, Lang),
                                     RequiredOpt = x.d.RequiredOpt,
                                     HasExpiryDate = x.d.HasExpiryDate
                                 };


                return FinalQuery;
            }
            catch
            {
                return new List<DocTypeFormViewModel>().AsQueryable();
            }
        }


        public int GetDocumentType_ByDocTypeID(int DocTypeID)
        {
            try
            {
                int SysCodeId = (from d in GetAll().Where(a => a.Id == DocTypeID)
                             join l in context.LookUpUserCodes.Where(a => a.CodeName == Constants.SystemCodes.DocType.CodeName)
                             on d.DocumenType equals l.CodeId 
                             select l.SysCodeId).FirstOrDefault();


                return SysCodeId;
            }
            catch
            {
                return 0;
            }
        }



    }
}
