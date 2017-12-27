using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System;
using Model.Domain.Payroll;

namespace Db.Persistence.Repositories
{
  class LookUpRepoitory : Repository<LookUpCode>, ILookUpRepository
    {
        public LookUpRepoitory(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public DocType DocTypeObject(int id)
        {
            //,"Nationalities"
            return context.DocTypes.Include("Jobs").Include("Nationalities").Where(j => j.Id == id).FirstOrDefault();
        }
       
        public DocType GetDocType(int? id)
        {
            return context.DocTypes.Find(id);
        }
        
        #region Kafeel
        public IQueryable<KafeelViewModel> GetAllKafeels()
        {
            var result= from k in context.Kafeel
                        select new KafeelViewModel
                        {
                            Id = k.Id,
                            Code=k.Code,
                            Name = k.Name,
                            Email = k.Email,
                            AddressId = k.AddressId,
                            Tel = k.Tel,
                            Address = k.Address.Address1 + (k.Address.Address2 == null ? "" : ", " + k.Address.Address2) + (k.Address.Address3 == null ? "" : ", " + k.Address.Address3)
                        };
            return result ;
        }

        public void Add(Kafeel Kafeel)
        {
            context.Kafeel.Add(Kafeel);
        }
        public void Attach(Kafeel Kafeel)
        {
            context.Kafeel.Attach(Kafeel);
        }
        public DbEntityEntry<Kafeel> Entry(Kafeel Kafeel)
        {
            return Context.Entry(Kafeel);
        }
        public void Remove(Kafeel kafeel)
        {
            if (Context.Entry(kafeel).State == EntityState.Detached)
            {
                context.Kafeel.Attach(kafeel);
            }
            context.Kafeel.Remove(kafeel);
        }

       
        #endregion
        #region Hosipital
        public IQueryable<HospitalViewModel> GetAllHospitals(int providType)
        {
            var result = from h in context.Providers
                         where h.ProviderType== providType
                         select new HospitalViewModel
                         {
                             Id = h.Id,
                             Code = h.Code,
                             Name = h.Name,
                             Email = h.Email,
                             Manager = h.Manager,
                             Tel = h.Tel,
                             AddressId = h.AddressId,
                             Address = h.Address.Address1 + (h.Address.Address2 == null ? "" : ", " + h.Address.Address2) + (h.Address.Address3 == null ? "" : ", " + h.Address.Address3),
                             CreatedUser = h.CreatedUser,
                             CreatedTime = h.CreatedTime,
                             ModifiedTime = h.ModifiedTime,
                             ModifiedUser = h.ModifiedUser
                         };
            return result;
        }
        public void Add(Provider Provider)
        {
            context.Providers.Add(Provider);
        }
        public void Attach(Provider Provider)
        {
            context.Providers.Attach(Provider);
        }
        public DbEntityEntry<Provider> Entry(Provider Provider)
        {
            return Context.Entry(Provider);
        }
        public void Remove(Provider Provider)
        {
            if (Context.Entry(Provider).State == EntityState.Detached)
            {
                context.Providers.Attach(Provider);
            }
            context.Providers.Remove(Provider);
        }


        #endregion

        #region 

        public IEnumerable<LookUpViewModel> GetLookUpUserCode(string culture)
        {
           var query= context.SystemCode.Select(c => new LookUpViewModel{Id = c.CodeName,CodeName = c.CodeName , Title=c.CodeName}).Distinct().ToList();
            foreach (var item in query)
            {
                item.Title = MsgUtils.Instance.Trls(culture, item.Title);
            }
            return query;
        }

        public IEnumerable<LookUpViewModel> GetLookUp(string culture)
        {
            var query =(from a in  context.LookUpCodes
                where((a.StartDate <= DateTime.Today && (a.EndDate == null || a.EndDate >= DateTime.Today )))
                select new LookUpViewModel {
                    Id = a.CodeName,
                    CodeName = a.CodeName ,
                    Title = HrContext.TrlsName(a.CodeName, culture),
                    Protected = a.Protected
                }).Distinct().ToList();
           
            return query;
        }
        public IEnumerable GetCodeName()
        {
            return context.LookUpCodes.Where(a => (a.StartDate <= DateTime.Today && (a.EndDate == null || a.EndDate >= DateTime.Today))).Select(c => new LookUpViewModel
            {
                Id = c.CodeName,
                CodeName = c.CodeName
            }).Distinct();

        }
        public IEnumerable GetCode()
        {
            return context.LookUpCodes.Where(a => (a.StartDate <= DateTime.Today && (a.EndDate == null || a.EndDate >= DateTime.Today)))
                .Select(c => new { text = c.CodeName,value = c.CodeName}).Distinct().ToList();

        }

        public IQueryable<LookupUserCodeViewModel> GetLookUpUserCodes(string Id, string culture)
        {
            var result = from c in context.LookUpUserCodes
                         where c.CodeName == Id && (c.StartDate <= DateTime.Today && (c.EndDate == null || c.EndDate >= DateTime.Today ))
                         join t in context.LookUpTitles on new { c.CodeName, c.CodeId } equals new { t.CodeName, t.CodeId } into g
                         from t in g.Where(gg => gg.Culture == culture).DefaultIfEmpty()
                         select new LookupUserCodeViewModel
                         {
                             Id = c.Id,
                             CodeId = c.CodeId,
                             CodeName = c.CodeName,
                             SysCodeId = c.SysCodeId,
                             Name = c.Name,
                             Title = t.Title == null ? c.Name : t.Title,
                             Description = c.Description,
                             StartDate = c.StartDate,
                             EndDate = c.EndDate,
                             ModifiedTime = c.ModifiedTime,
                             ModifiedUser = c.ModifiedUser,
                             CreatedUser = c.CreatedUser,
                             CreatedTime = c.CreatedTime
                         };

            return result;
        }

        public IEnumerable GetsyscodeId(string CodeName)
        {
            return context.SystemCode.Where(w => w.CodeName == CodeName).Select(s => new { value = s.SysCodeId, text = s.SysCodeName }).ToList();
        }
        public IList<SysCodeViewModel> GetsyscodeForm(string CodeName , string culture)
        {
            var query= context.SystemCode.Where(w => w.CodeName == CodeName).Select(s => new SysCodeViewModel { id = s.SysCodeId, name = s.SysCodeName }).ToList();
            foreach (var item in query)
            {
                      item.name= MsgUtils.Instance.Trls(culture, item.name);
            }
            return query;
        }

        public IQueryable<LookupCodesViewModel> GetLookUpCodes(string Id, string culture)
        {
            var result = from c in context.LookUpCodes
                         where (c.CodeName == Id) &&(c.StartDate <= DateTime.Today && (c.EndDate == null || c.EndDate >= DateTime.Today )) 
                         join t in context.LookUpTitles on new { c.CodeName, c.CodeId } equals new { t.CodeName, t.CodeId } into g
                         from t in g.Where(gg => gg.Culture == culture).DefaultIfEmpty()
                         select new LookupCodesViewModel
                         {
                             Id = c.Id,
                             CodeId = c.CodeId,
                             CodeName = c.CodeName,
                             Name = c.Name,
                             Title = t.Title == null ? c.Name : t.Title,
                             Description = c.Description,
                             StartDate = c.StartDate,
                             EndDate = c.EndDate,
                             Protected = c.Protected,
                             ModifiedTime=c.ModifiedTime,
                             ModifiedUser=c.ModifiedUser,
                            CreatedUser=c.CreatedUser,
                            CreatedTime=c.CreatedTime 
                         };
            return result;
        }

        public IQueryable<JobClassViewModel> GetJobClasses(int CompanyId)
        {
            return from j in context.JobClasses
                   where ((j.IsLocal && j.CompanyId == CompanyId) || !j.IsLocal)
                   select new JobClassViewModel
                   {
                       Id = j.Id,
                       Name = j.Name,
                       IsLocal = j.IsLocal,
                       CompanyId = j.CompanyId,
                       Notes = j.Notes,
                       JobClassCode = j.JobClassCode,
                       CreatedTime=j.CreatedTime,
                       CreatedUser=j.CreatedUser,
                       ModifiedUser=j.ModifiedUser,
                       ModifiedTime=j.ModifiedTime
                   };
        }
        public IQueryable<DocTypeViewModel> GetDocTypes(int CompanyId, string culture)
        {
            var query = from d in context.DocTypes
                        where (((d.IsLocal && d.CompanyId == CompanyId) || d.IsLocal == false) && (d.StartDate <= DateTime.Today && (d.EndDate == null || d.EndDate >= DateTime.Today)))
                        select new DocTypeViewModel
                        {
                            Id = d.Id,
                            EndDate = d.EndDate,
                            StartDate = d.StartDate,
                            Name = d.Name,
                            HasExpiryDate = d.HasExpiryDate,
                            CompanyId=d.CompanyId,
                            IsLocal=d.IsLocal,
                            RequiredOpt=d.RequiredOpt,
                            LocalName = HrContext.TrlsName(d.Name, culture)
                            
                        };
            return query;
        }

        public string DeleteLookUpCode(LookUpViewModel models, string culture)
        {
            if (models == null) return "OK";

            var Codename = models.CodeName;
            var LookUpCodeList = context.LookUpCodes.Where(a => a.CodeName == Codename).ToList();
            var titles = context.LookUpTitles.ToList();
            var result = "OK";

            foreach (var code in LookUpCodeList)
            {
                result = CheckLookUpCode(code.CodeName, code.CodeId, culture);
                if (result != "OK") return MsgUtils.Instance.Trls(culture, "DeleteRelatedRecord").Replace("{0}", result) + ": " + context.LookUpCodes.Where(a => a.Id == code.Id).Select(a => a.Name).FirstOrDefault();

                var DeleteObj = LookUpCodeList.Where(a => a.Name == code.Name).FirstOrDefault();
                context.LookUpCodes.Attach(DeleteObj);
                context.Entry(DeleteObj).State = EntityState.Deleted;
                var DeleteTitles = titles.Where(a => a.CodeName == code.CodeName).ToList();
                RemoveRange(DeleteTitles);
            }

            return result;
        }
       
        public void DeleteLookUpCodes(IEnumerable<LookupCodesViewModel> models)
        {
            if (models == null) return;
            var Codename = models.Select(n => n.CodeName).First();
            var LookUpCodeList = context.LookUpCodes.Where(a => a.CodeName == Codename).ToList();
            var titlies = context.LookUpTitles.Where(a => a.CodeName == Codename).ToList();
            foreach (var code in LookUpCodeList)
            {
                var DeleteObj = LookUpCodeList.Where(a => a.Name == code.Name).FirstOrDefault();
                var DeleteTitlies = titlies.Where(a => a.CodeId == code.CodeId).ToList();
                context.LookUpCodes.Attach(DeleteObj);
                context.Entry(DeleteObj).State = EntityState.Deleted;
                RemoveRange(DeleteTitlies);
            }

        }
        public void UpdateLookUpCode(IEnumerable<LookUpViewModel> models)
        {
            if (models == null) return;
            var Codename = models.Select(n => n.Id).First();
            var LookUpCodeList = context.LookUpCodes.Where(a => a.CodeName == Codename).ToList();
            var NewCodeName = models.Select(a => a.CodeName).First();
            UpdateCodeName(Codename, NewCodeName);
        }
        public void UpdateLookUpUserCode(IEnumerable<LookupUserCodeViewModel> models, string culture, string name)
        {
            if (models == null) return;
            var codeName = models.First().CodeName;
            var db = context.LookUpUserCodes.ToList();
            var titiles = context.LookUpTitles.Where(a => a.CodeName == codeName && a.Culture == culture).ToList();
            foreach (var model in models)
            {
                var record = db.FirstOrDefault(a => a.Id == model.Id);
                var title = titiles.FirstOrDefault(a => a.CodeId == model.CodeId);

                if (record == null)
                {
                    //insert 
                    var UserCode = new LookUpUserCode
                    {
                        SysCodeId = model.SysCodeId,
                        CodeId = model.CodeId,
                        Name = model.Name,
                        Description = model.Description,
                        EndDate = model.EndDate,
                        CreatedUser = name,
                        StartDate = model.StartDate,
                        CodeName = model.CodeName,
                        CreatedTime = DateTime.Now

                    };
                    context.LookUpUserCodes.Add(UserCode);
                }
                //Update
                else
                {
                    record.Id = model.Id;
                    record.Name = model.Name;
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = name;
                    record.SysCodeId = model.SysCodeId;
                    record.StartDate = model.StartDate;
                    record.EndDate = model.EndDate;
                    record.Description = model.Description;
                    record.CodeId = model.CodeId;
                    record.CodeName = model.CodeName;
                    context.LookUpUserCodes.Attach(record);
                    context.Entry(record).State = EntityState.Modified;
                }
                //  if title changed update && insert
                if (title != null)
                {
                    title.CodeId = model.CodeId;
                    title.CodeName = model.CodeName;
                    title.Name = model.Name;
                    title.Title = model.Title;
                    context.LookUpTitles.Attach(title);
                    context.Entry(title).State = EntityState.Modified;

                }
                else
                {
                    var Lookuptitle = new LookUpTitles
                    {

                        Culture = culture,
                        CodeName = model.CodeName,
                        Name = model.Name,
                        Title = model.Title,
                        CodeId = model.CodeId
                    };
                    context.LookUpTitles.Add(Lookuptitle);
                }

            }

        }

        public void UpdateLookUpCodes(IEnumerable<LookupCodesViewModel> models, string culture, string name)
        {
            if (models == null) return;
            var codeName = models.First().CodeName;
            var db = context.LookUpCodes.ToList();
            var titiles = context.LookUpTitles.Where(a => a.CodeName == codeName && a.Culture == culture).ToList();
            foreach (var model in models)
            {
                var record = db.FirstOrDefault(a => a.Id == model.Id);
                var title = titiles.FirstOrDefault(a => a.CodeId == model.CodeId);

                if (record == null)
                {
                    //insert 
                    var UserCode = new LookUpCode
                    {
                        
                        CodeId = model.CodeId,
                        Name = model.Name,
                        Description = model.Description,
                        EndDate = model.EndDate,
                        CreatedUser = name,
                        StartDate = model.StartDate,
                        CodeName = model.CodeName,
                        CreatedTime = DateTime.Now

                    };
                    context.LookUpCodes.Add(UserCode);
                }
                //Update
                else
                {
                    record.Name = model.Name;
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = name;
                    record.StartDate = model.StartDate;
                    record.EndDate = model.EndDate;
                    record.Description = model.Description;
                    record.CodeId = model.CodeId;
                    record.CodeName = model.CodeName;
                    context.LookUpCodes.Attach(record);
                    context.Entry(record).State = EntityState.Modified;
                }
                //  if title changed update && insert
                if (title != null)
                {
                    title.CodeName = model.CodeName;
                    title.Name = model.Name;
                    title.Title = model.Title;
                    context.LookUpTitles.Attach(title);
                    context.Entry(title).State = EntityState.Modified;

                }
                else
                {
                    var Lookuptitle = new LookUpTitles
                    {

                        Culture = culture,
                        CodeName = model.CodeName,
                        Name = model.Name,
                        Title = model.Title,
                        CodeId = model.CodeId
                    };
                    context.LookUpTitles.Add(Lookuptitle);
                }

            }

        }

        public void UpdateCodeName(string codeNameOld, string codeNameNew)
        {
            context.Database.ExecuteSqlCommand("UPDATE lookupcode SET CodeName = '" + codeNameNew + "' WHERE CodeName = '" + codeNameOld + "'");
        }

        public IQueryable<CountryViewModel> GetCountry()
        {
            var countries = from m1 in context.Countries
                            select new CountryViewModel
                            {
                                Id = m1.Id,
                                Name = m1.Name,
                                NameAr = m1.NameAr,
                                Nationality = m1.Nationality,
                                TimeZone = m1.TimeZone,
                                NationalityAr = m1.NationalityAr,
                                DayLightSaving = m1.DayLightSaving,
                                CreatedTime=m1.CreatedTime,
                                CreatedUser=m1.CreatedUser,
                                ModifiedUser=m1.ModifiedUser,
                                ModifiedTime=m1.ModifiedTime

                            };

            return countries;
        }
        public IQueryable<CurrencyViewModel> GetCurrency()
        {
            var currencies = from c in context.Currencies
                             select new CurrencyViewModel
                             {
                                 Id = c.Code,
                                 Code = c.Code,
                                 Name = c.Name,
                                 IsMultiplyBy = c.IsMultiplyBy,
                                 Isocode = c.Isocode,
                                 Symbol = c.Symbol,
                                 MidRate = c.MidRate,
                                 PayRoundRule = c.PayRoundRule,
                                 Decimals = c.Decimals,
                                 CalcRoundRule = c.CalcRoundRule,
                                 RoundMethod = c.RoundMethod,
                                 CalcRoundRuleText = (int)c.CalcRoundRule,
                                 PayRoundRuleText = (int)c.PayRoundRule,
                                 Referenced = c.Referenced,
                                 Suffix = c.Suffix
                             };

            return currencies;
        }

        public IEnumerable GetCurrencyCode()
        {
            var currencies = Context.Set<Currency>()

                .Select(c => new { id = c.Code, name = c.Name })
                .ToList();
            return currencies;
        }
        public IEnumerable GetCurrencyCodeGrid()
        {
            var currencies = Context.Set<Currency>()

                .Select(c => new  { text = c.Name, value = c.Code })
                .ToList();
            return currencies;
        }
        
        public IEnumerable BudgetPeriod()
        {
            var Preiod = Context.Set<Period>()
                .Select(c => new { value = c.Id, text = c.Name }).ToList();
            return Preiod;
        }

        public IQueryable<CityViewModel> GetCity(int Id)
        {
            var cities = from c1 in context.Cities
                         where c1.CountryId == Id
                         select new CityViewModel
                         {
                             Id = c1.Id,
                             CountryId = c1.CountryId,
                             Name = c1.Name,
                             NameAr = c1.NameAr

                         };

            return cities;

        }
        public IEnumerable<DocTypeAttrViewModel> GetDocAttr(int Id ,string culture)
        {
            var query = context.DocTypeAttrs.Where(d => d.TypeId == Id).Select(d => new DocTypeAttrViewModel { Id = d.Id, InputType = d.InputType, Attribute = d.Attribute, TypeId = d.TypeId, CodeName = d.CodeName }).ToList();
            foreach (var item in query)
            {
                item.CodeName = MsgUtils.Instance.Trls(culture, item.CodeName);
               
            }
            return query;
        }
        public IQueryable<DistrictViewModel> GetDistrict(int Id)
        {
            var cities = from d1 in context.Districts
                         where d1.CityId == Id
                         select new DistrictViewModel
                         {
                             Id = d1.Id,
                             CityId = d1.CityId,
                             Name = d1.Name,
                             NameAr = d1.NameAr

                         };

            return cities;

        }
        public DocTypeFormViewModel ReadDocType(int Id ,string culture)
        {
            var query =( from d in context.DocTypes
                        where d.Id==Id
                        select new DocTypeFormViewModel
                        {
                            Id = d.Id,
                            EndDate = d.EndDate,
                            IsLocal = d.IsLocal,
                            Name = d.Name,
                            RequiredOpt = d.RequiredOpt,
                            StartDate = d.StartDate,
                            DocumenType = d.DocumenType,
                            HasExpiryDate = d.HasExpiryDate,
                            AccessLevel = d.AccessLevel,
                            LocalName = HrContext.TrlsName(d.Name,culture),
                            IJobs = d.Jobs.Select(a => a.Id),
                            INationalities=d.Nationalities.Select(a=>a.Id),
                            ModifiedTime = d.ModifiedTime,
                            ModifiedUser = d.ModifiedUser,
                            CreatedUser = d.CreatedUser,
                            CreatedTime = d.CreatedTime,
                            Gender=d.Gender
                        }).FirstOrDefault();
            return query;
        }
        //GetJobs
        public IQueryable GetJobs(int companyId)
        {

            var Job = Context.Set<Job>()
                .Where(a => ((a.IsLocal && a.CompanyId == companyId) || a.IsLocal == false))
                .Select(c => new { id = c.Id, name = c.Name });
            return Job;
        }
        public void Add(Country country)
        {
            context.Countries.Add(country);
        }
        public void Add(LookUpUserCode lookupusercode)
        {
            context.LookUpUserCodes.Add(lookupusercode);
        }
        public void Add(City city)
        {
            context.Cities.Add(city);
        }
        public void Add(DocType doctype)
        {
            context.DocTypes.Add(doctype);
        }
        public void Add(DocTypeAttr docattr)
        {
            context.DocTypeAttrs.Add(docattr);
        }
        public void Add(PersonSetup setup)
        {
            context.PersonSetup.Add(setup);
        }
        public void AddLookUpCode(LookUpCode Look)
        {
            context.LookUpCodes.Add(Look);
        }
        public void Add(CompanyDocAttr companyDocAttr)
        {
            context.CompanyDocAttrs.Add(companyDocAttr);
        }
        public void Attach(DocType doctype)
        {
            context.DocTypes.Attach(doctype);
        }
        public void Attach(DocTypeAttr docattr)
        {
            context.DocTypeAttrs.Attach(docattr);
        }
        public void Attach(Country country)
        {
            context.Countries.Attach(country);
        }
        public void Attach(LookUpUserCode lookupusercode)
        {
            context.LookUpUserCodes.Attach(lookupusercode);
        }
        public void Attach(PersonSetup setup)
        {
            context.PersonSetup.Attach(setup);
        }
        public void Attach(City city)
        {
            context.Cities.Attach(city);
        }
        public void Attach(CompanyDocAttr companyDocAttr)
        {
            context.CompanyDocAttrs.Attach(companyDocAttr);
        }
        public void Remove(Country country)
        {
            if (Context.Entry(country).State == EntityState.Detached)
            {
                context.Countries.Attach(country);
            }
            context.Countries.Remove(country);
        }
        public void Remove(LookUpUserCode UserCode)
        {
            if (Context.Entry(UserCode).State == EntityState.Detached)
            {
                context.LookUpUserCodes.Attach(UserCode);
            }
            context.LookUpUserCodes.Remove(UserCode);
        }
       
        public void Remove(DocTypeAttr docattr)
        {
            if (Context.Entry(docattr).State == EntityState.Detached)
            {
                context.DocTypeAttrs.Attach(docattr);
            }
            context.DocTypeAttrs.Remove(docattr);
        }
        public void Remove(DocType doctype)
        {
            if (Context.Entry(doctype).State == EntityState.Detached)
            {
                context.DocTypes.Attach(doctype);
            }
            context.DocTypes.Remove(doctype);
        }
        public void Remove(City city)
        {
            if (Context.Entry(city).State == EntityState.Detached)
            {
                context.Cities.Attach(city);
            }
            context.Cities.Remove(city);
        }
        public DbEntityEntry<Country> Entry(Country country)
        {
            return Context.Entry(country);
        }
        public DbEntityEntry<JobClass> Entry(JobClass job)
        {
            return Context.Entry(job);
        }
        public DbEntityEntry<DocType> Entry(DocType doctype)
        {
            return Context.Entry(doctype);
        }
        public DbEntityEntry<DocTypeAttr> Entry(DocTypeAttr docattr)
        {
            return Context.Entry(docattr);
        }
        public DbEntityEntry<LookUpUserCode> Entry(LookUpUserCode lookupusercode)
        {
            return Context.Entry(lookupusercode);
        }
        public DbEntityEntry<PersonSetup> Entry(PersonSetup setup)
        {
            return Context.Entry(setup);
        }
        public DbEntityEntry<City> Entry(City city)
        {
            return Context.Entry(city);
        }
        public DbEntityEntry<CompanyDocAttr> Entry(CompanyDocAttr companyDocAttr)
        {
            return Context.Entry(companyDocAttr);
        }
        public void Add(Currency currency)
        {
            context.Currencies.Add(currency);
        }
        public void Add(JobClass job)
        {
            context.JobClasses.Add(job);
        }
        public void Attach(Currency currency)
        {
            context.Currencies.Attach(currency);
        }
        public void Attach(JobClass job)
        {
            context.JobClasses.Attach(job);
        }
        public void Remove(Currency currency)
        {
            if (Context.Entry(currency).State == EntityState.Detached)
            {
                context.Currencies.Attach(currency);
            }
            context.Currencies.Remove(currency);
        }
        public void Remove(JobClass job)
        {
            if (Context.Entry(job).State == EntityState.Detached)
            {
                context.JobClasses.Attach(job);
            }
            context.JobClasses.Remove(job);
        }
        public DbEntityEntry<Currency> Entry(Currency currency)
        {
            return Context.Entry(currency);
        }
        public void Add(District district)
        {
            context.Districts.Add(district);
        }
        public void Attach(District district)
        {
            context.Districts.Attach(district);
        }
        public void Remove(District district)
        {
            if (Context.Entry(district).State == EntityState.Detached)
            {
                context.Districts.Attach(district);
            }
            context.Districts.Remove(district);
        }
        public DbEntityEntry<District> Entry(District district)
        {
            return Context.Entry(district);
        }

        public DbEntityEntry<LookUpCode> Entry(LookUpCode lookupcode)
        {
            return Context.Entry(lookupcode);
        }
        public void Add(LookUpTitles lookuptitles)
        {
            context.LookUpTitles.Add(lookuptitles);
        }
        public void Attach(LookUpTitles lookuptitles)
        {
            context.LookUpTitles.Attach(lookuptitles);
        }
        public void Remove(LookUpTitles lookuptitles)
        {
            if (Context.Entry(lookuptitles).State == EntityState.Detached)
            {
                context.LookUpTitles.Attach(lookuptitles);
            }
            context.LookUpTitles.Remove(lookuptitles);
        }
        public DbEntityEntry<LookUpTitles> Entry(LookUpTitles lookuptitles)
        {
            return Context.Entry(lookuptitles);
        }
        public void RemoveRange(IEnumerable<LookUpTitles> entities)
        {
            Context.Set<LookUpTitles>().RemoveRange(entities);
        }
        public void RemoveRange(IEnumerable<LookUpUserCode> entities)
        {
            Context.Set<LookUpUserCode>().RemoveRange(entities);
        }

        #endregion

        public IEnumerable<LookupCodesViewModel> GetFlexLookUpCodesLists(int companyId, string objectName, byte version, string culture)
        {
            var result = from flex in context.FlexColumns
                         where flex.PageId == HrContext.GetPageId(companyId, objectName, version)
                         join code in context.LookUpCodes on flex.CodeName equals code.CodeName
                         join t in context.LookUpTitles on new { code.CodeName, code.CodeId } equals new { t.CodeName, t.CodeId } into g
                         from t in g.Where(a => a.Culture == culture).DefaultIfEmpty()
                         select new LookupCodesViewModel
                         {
                             Id = code.Id,
                             CodeName = code.CodeName,
                             CodeId = code.CodeId,
                             Name = (t.Title != null ? t.Title : code.Name)
                         };

            return result.Distinct();
        }

        public string CheckLookUpCode(string code, short id, string culture)
        {
            return context.Database.SqlQuery<string>("exec dbo.sp_GetPageName '" + code + "', " + id + ", '" + culture + "'").FirstOrDefault();
        }

        
    }
}
