using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Administration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Db.Persistence.Repositories
{
   class LocationRepoitory : Repository<Location>, ILocationRepository
    {
        public LocationRepoitory(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

 

        public IQueryable<LocationViewModel> ReadLocations(string culture,int CompanyId)
        {
         
            var global = MsgUtils.Instance.Trls(culture, "Global");

            var Locations = from l in context.Locations
                            where (((l.IsLocal && l.CompanyId == CompanyId) || (!l.IsLocal)) && (l.StartDate <= DateTime.Today && (l.EndDate == null || l.EndDate >= DateTime.Today)))
                            join c in context.Companies on l.CompanyId equals c.Id into g1
                            from c in g1.DefaultIfEmpty()
                            //join a in context.Addresses on l.AddressId equals a.Id into g2
                            //from a in g2.DefaultIfEmpty()
                            select new LocationViewModel
                            {
                                Id = l.Id,
                                Code = l.Code,
                                Name = l.Name,
                                LocalName= HrContext.TrlsName(l.Name, culture),
                                IsLocal = l.IsLocal,
                                IsInternal = l.IsInternal,
                                CompanyId = l.CompanyId,
                                CompanyName = c == null ? global : c.Name,
                                TimeZone = l.TimeZone,
                                Latitude =l.Latitude,
                                Longitude =l.Longitude,
                                CreatedTime=l.CreatedTime,
                                CreatedUser=l.CreatedUser,
                                ModifiedTime=l.ModifiedTime,
                                ModifiedUser=l.ModifiedUser
                            };
            return Locations;
        }

        public AddLocationViewModel ReadLocation(int Id, string culture)
        {
            var global = MsgUtils.Instance.Trls(culture, "Global");
            var Location = (from l in context.Locations
                            where l.Id == Id
                            join c in context.Companies on l.CompanyId equals c.Id into g1
                            from c in g1.DefaultIfEmpty()
                                //join a in context.Addresses on l.AddressId equals a.Id into g2
                                //from a in g2.DefaultIfEmpty()
                            select new AddLocationViewModel
                            {
                                Id = l.Id,
                                Code = l.Code,
                                Name = l.Name,
                                IsLocal = l.IsLocal,
                                //Address = a == null ? "" : (a.Address1 + (a.Address2 == null ? "" : ", " + a.Address2) + (a.Address3 == null ? "" : ", " + l.Address.Address3)),
                                CompanyName = c == null ? global : c.Name,
                                IsInternal = l.IsInternal,
                                TimeZone = l.TimeZone,
                                CompanyId = l.CompanyId,
                                //AddressId = l.AddressId,
                                DaylightSaving = l.DaylightSaving,
                                Description = l.Description,
                                EndDate = l.EndDate,
                                StartDate = l.StartDate,
                                LName = HrContext.TrlsName(l.Name, culture),
                                Longitude = l.Longitude,
                                Latitude = l.Latitude,
                                CreatedTime = l.CreatedTime,
                                ModifiedTime = l.ModifiedTime,
                                CreatedUser = l.CreatedUser,
                                ModifiedUser = l.ModifiedUser,
                            }).FirstOrDefault();

            return Location;
        }

        public IEnumerable<ExcelFormLocationViewModel> ReadExcelLocations(int CompanyId, string Language)
        {
            var Location = (from l in context.Locations
                            where (((l.IsLocal && l.CompanyId == CompanyId) || (!l.IsLocal)) && (l.StartDate <= DateTime.Today && (l.EndDate == null || l.EndDate >= DateTime.Today)))
                            join c in context.Companies on l.CompanyId equals c.Id into g1
                            from c in g1.DefaultIfEmpty()
                            select new ExcelFormLocationViewModel
                            {
                                Id = l.Id,
                                Code = l.Code,
                                Name = l.Name,
                                IsLocal = HrContext.TrlsMsg(l.IsLocal.ToString(),Language),
                                IsInternal =HrContext.TrlsMsg (l.IsInternal.ToString(),Language),
                                TimeZone = l.TimeZone,   
                                DaylightSaving = l.DaylightSaving != null ? HrContext.TrlsMsg(l.DaylightSaving.ToString(),Language):" ",
                                Description = l.Description,
                                EndDate = l.EndDate!= null? l.EndDate.ToString():" ",
                                StartDate = l.StartDate.ToString(),
                                LName = HrContext.TrlsName(l.Name, Language),
                                Longitude =l.Longitude != null ? l.Longitude.ToString():" ",
                                Latitude = l.Latitude != null ? l.Latitude.ToString():" "
                            }).ToList();

            return Location;
        }
    }
}
