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
   class SiteRepository : Repository<Site>, ISiteRepository
    {
        public SiteRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        public IQueryable<SiteViewModel> ReadSites(string culture, int CompanyId)
        {
            var Sites = from l in context.Sites
                            join code in context.LookUpCodes on new { p1 = "SiteType", p2 = l.SiteType } equals new { p1=code.CodeName, p2=code.CodeId }
                            join t in context.LookUpTitles on new { code.CodeName, code.CodeId } equals new { t.CodeName, t.CodeId } into g1
                            from t in g1.DefaultIfEmpty()
                            join d in context.Districts on l.DistrictId equals d.Id into g2
                            from d in g2.DefaultIfEmpty()
                            join city in context.Cities on l.CityId equals city.Id into g3
                            from city in g3.DefaultIfEmpty()
                            join co in context.Countries on l.CountryId equals co.Id into g4
                            from co in g4.DefaultIfEmpty()
                            select new SiteViewModel
                            {
                                Id = l.Id,
                                Code = l.Code,
                                Name = l.Name,
                                LocalName = HrContext.TrlsName(l.Name, culture),
                                TimeZone = l.TimeZone,
                                Latitude = l.Latitude,
                                Longitude = l.Longitude,
                                Address = l.Address1,
                                City = culture.Substring(0, 2) == "ar" ? city.NameAr : code.Name,
                                District = culture.Substring(0, 2) == "ar" ? d.NameAr : d.Name,
                                Country = culture.Substring(0, 2) == "ar" ? co.NameAr : d.Name,
                                SiteType = (t == null ? code.Name : t.Title),
                                SiteToEmployees = context.SiteToEmployees.Where(a => a.SiteId == l.Id).Select(s => HrContext.TrlsName(s.Employee.Title + " " + s.Employee.FirstName + " " + s.Employee.Familyname, culture)).ToList(),
                                ContactPerson = l.ContactPerson,
                                Email = l.Email,
                                Mobile = l.Telephone
                            };
            return Sites;
        }

        public AddSiteViewModel ReadSite(int Id, int read, string culture)
        {
            var site = (from l in context.Sites
                            where l.Id == Id
                            select new AddSiteViewModel
                            {
                                Id = l.Id,
                                Code = l.Code,
                                Name = l.Name,
                                TimeZone = l.TimeZone,
                                Description = l.Description,
                                EndDate = l.EndDate,
                                StartDate = l.StartDate,
                                LName = HrContext.TrlsName(l.Name, culture),
                                Longitude = l.Longitude,
                                Latitude = l.Latitude,
                                CityId = l.CityId,
                                Address1 = l.Address1,
                                DistrictId = l.DistrictId,
                                CountryId = l.CountryId,
                                PostalCode = l.PostalCode,
                                ContactPerson = l.ContactPerson,
                                Telephone = l.Telephone,
                                Email = l.Email,
                                SiteToEmployees =  context.SiteToEmployees.Where(a => a.SiteId == Id).Select(s => s.EmpId).ToList() ,
                                SiteToEmployeesNames = context.SiteToEmployees.Where(a => a.SiteId == l.Id).Select(s => HrContext.TrlsName(s.Employee.Title + " " + s.Employee.FirstName + " " + s.Employee.Familyname, culture)).ToList(),
                                CreatedTime = l.CreatedTime,
                                ModifiedTime = l.ModifiedTime,
                                CreatedUser = l.CreatedUser,
                                ModifiedUser = l.ModifiedUser,
                                SiteType = l.SiteType
                            }).FirstOrDefault();

            return site;
        }

        public void Add(SiteToEmp request)
        {
            context.SiteToEmployees.Add(request);
        }
        public void Remove(SiteToEmp request)
        {
            if (Context.Entry(request).State == EntityState.Detached)
            {
                context.SiteToEmployees.Attach(request);
            }
            context.SiteToEmployees.Remove(request);
        }
        public IEnumerable<ExcelFormSiteViewModel> ReadExcelSites(int CompanyId, string Language)
        {
            var sites = (from l in context.Sites
                            select new ExcelFormSiteViewModel
                            {
                                Id = l.Id,
                                Code = l.Code,
                                Name = l.Name,
                                TimeZone = l.TimeZone,   
                                Description = l.Description,
                                LName = HrContext.TrlsName(l.Name, Language),
                                Longitude =l.Longitude != null ? l.Longitude.ToString():" ",
                                Latitude = l.Latitude != null ? l.Latitude.ToString():" "
                            }).ToList();

            return sites;
        }
    }
}
