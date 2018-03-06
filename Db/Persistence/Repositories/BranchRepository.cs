using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Administration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Db.Persistence.Repositories
{
   class BranchRepository : Repository<Branch>, IBranchRepository
    {
        public BranchRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        public IQueryable<BranchViewModel> ReadBranches(string culture, int companyid)
        {
            var branches = from l in context.Branches
                            where l.CompanyId == companyid
                            select new BranchViewModel
                            {
                                Id = l.Id,
                                Code = l.Code,
                                Name = l.Name,
                                LocalName = HrContext.TrlsName(l.Name, culture),
                                TimeZone = l.TimeZone,
                                Latitude = l.Latitude,
                                Longitude = l.Longitude,
                                CreatedTime = l.CreatedTime,
                                CreatedUser = l.CreatedUser,
                                ModifiedTime = l.ModifiedTime,
                                ModifiedUser = l.ModifiedUser,
                                Telephone = l.Telephone,
                                Address = l.Address1
                                
                            };

            return branches;
        }

        public AddBranchViewModel ReadBranch(int id, string culture)
        {
            var branch = (from l in context.Branches
                            where l.Id == id
                            select new AddBranchViewModel
                            {
                                Id = l.Id,
                                Code = l.Code,
                                Name = l.Name,
                                TimeZone = l.TimeZone,
                                LName = HrContext.TrlsName(l.Name, culture),
                                Longitude = l.Longitude,
                                Latitude = l.Latitude,
                                Address1 = l.Address1,
                                CityId = l.CityId,
                                CountryId = l.CountryId,
                                DistrictId = l.DistrictId,
                                Telephone = l.Telephone,
                                CreatedTime = l.CreatedTime,
                                ModifiedTime = l.ModifiedTime,
                                CreatedUser = l.CreatedUser,
                                ModifiedUser = l.ModifiedUser
                            }).FirstOrDefault();
            return branch;
        }
    }
}
