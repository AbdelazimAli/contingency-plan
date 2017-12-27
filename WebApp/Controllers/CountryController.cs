using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using WebApp.Extensions;
using WebApp.Models;
using Model.Domain.Payroll;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class CountryController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        private string UserName { get; set; }
        private string Language { get; set; }
        private int CompanyId { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
                UserName = requestContext.HttpContext.User.Identity.Name;
            }
        }
        #region Cascade Grid country by Mamdouh
        public CountryController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        //Kendo: read ==>  All Countries
        public ActionResult ReadCountry(int MenuId)
        {            
            var query = _hrUnitOfWork.LookUpRepository.GetCountry();
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {

                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {

                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        //Kendo: read ==> Cities
        public ActionResult ReadCity(int Id)
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetCity(Id), JsonRequestBehavior.AllowGet);
        }
        //Kendo: read ==> District
        public ActionResult ReadDistrict(int Id)
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetDistrict(Id), JsonRequestBehavior.AllowGet);
        }
        //Kendo: create ==> Country
        [HttpPost]
        public ActionResult CreateCountry(IEnumerable<CountryViewModel> models , IEnumerable<OptionsViewModel> options)
        {
            var result = new List<Country>();

            var datasource = new DataSource<CountryViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Countries",
                        TableName = "Countries",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                for (var i = 0; i < models.Count(); i++)
                {
                    var model = models.ElementAtOrDefault(i);
                    var country = new Country();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = country,
                        Source = model,
                        ObjectName = "Countries",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = options.ElementAtOrDefault(i),
                        Transtype = TransType.Insert
                    });

                    country.CreatedTime = DateTime.Now;
                    country.CreatedUser = UserName;

                    result.Add(country);
                    _hrUnitOfWork.LookUpRepository.Add(country);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on p.Name equals r.Name
                               select new CountryViewModel
                               {
                                   Id = r.Id,
                                   Name = p.Name,
                                   NameAr = p.NameAr,
                                   DayLightSaving = p.DayLightSaving,
                                   Nationality = p.Nationality,
                                   NationalityAr = p.NationalityAr,
                                   TimeZone = p.TimeZone,
                                   CreatedTime = p.CreatedTime,
                                   CreatedUser =p.CreatedUser
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        //Kendo:Update ==> Country
        public ActionResult UpdateCountry(IEnumerable<CountryViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<CountryViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Countries",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Id);
                var db_Countruies = _hrUnitOfWork.Repository<Country>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var country = db_Countruies.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "Countries", Destination = country, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype=TransType.Update });
                    country.ModifiedTime = DateTime.Now;
                    country.ModifiedUser = UserName;
                    _hrUnitOfWork.LookUpRepository.Attach(country);
                    _hrUnitOfWork.LookUpRepository.Entry(country).State = EntityState.Modified;
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        // Kendo:destroy ==> country
        public ActionResult DeleteCountry(int Id)
        {
            var datasource = new DataSource<CountryViewModel>();
            var Obj = _hrUnitOfWork.Repository<Country>().FirstOrDefault(a => a.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "Countries",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete,
            });

            _hrUnitOfWork.LookUpRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        //Kendo: create ==> City
        [HttpPost]
        public ActionResult CreateCity(IEnumerable<CityViewModel> models, OptionsViewModel moreInfo)
        {
            var result = new List<City>();

            var datasource = new DataSource<CityViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Cities",
                        TableName = "Cities",
                        ParentColumn = "CountryId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (CityViewModel c in models)
                {
                    var city = new City();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = city,
                        Source = c,
                        ObjectName = "Cities",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    result.Add(city);
                    _hrUnitOfWork.LookUpRepository.Add(city);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on p.Name equals r.Name
                               select new CityViewModel
                               {
                                   Id = r.Id,
                                   Name = p.Name,
                                   NameAr = p.NameAr,
                                   Country = p.Country,
                                   CountryId = p.CountryId                                  
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        //Kendo:Update ==> City
        public ActionResult UpdateCity(IEnumerable<CityViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<CityViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Cities",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Id);
                var db_Cities = _hrUnitOfWork.Repository<City>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var city = db_Cities.FirstOrDefault(s => s.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "Cities", Destination = city, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype=TransType.Update});
                    _hrUnitOfWork.LookUpRepository.Attach(city);
                    _hrUnitOfWork.LookUpRepository.Entry(city).State = EntityState.Modified;
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        // Kendo:destroy ==> city
        public ActionResult DeleteCity(int Id)
        {
            var datasource = new DataSource<CityViewModel>();
            var obj = _hrUnitOfWork.Repository<City>().FirstOrDefault(a => a.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = obj,
                ObjectName = "Cities",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete,
            });
            _hrUnitOfWork.LookUpRepository.Remove(obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        //Kendo: create ==> District
        public ActionResult CreateDistrict(IEnumerable<DistrictViewModel> models)
        {
            var result = new List<District>();

            var datasource = new DataSource<DistrictViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Districts",
                        TableName = "Districts",
                        ParentColumn = "City",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (DistrictViewModel c in models)
                {
                    var district = new District();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = district,
                        Source = c,
                        ObjectName = "Countries",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Transtype = TransType.Insert
                    });

                    result.Add(district);
                    _hrUnitOfWork.LookUpRepository.Add(district);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on p.Name equals r.Name
                               select new DistrictViewModel
                               {
                                   Id = r.Id,
                                   Name = p.Name,
                                   CityId = p.CityId,
                                   NameAr = p.NameAr
                                   
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        //Kendo:Update ==> UDistrict
        public ActionResult UpdateDistrict(IEnumerable<DistrictViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<DistrictViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Districts",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                var ids = models.Select(a => a.Id);
                var db_Districts = _hrUnitOfWork.Repository<District>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var district = db_Districts.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "Districts", Destination = district, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype = TransType.Update });
                    _hrUnitOfWork.LookUpRepository.Attach(district);
                    _hrUnitOfWork.LookUpRepository.Entry(district).State = EntityState.Modified;
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        // Kendo:destroy ==> District
        public ActionResult DeleteDistrict(int Id)
        {
            var datasource = new DataSource<DistrictViewModel>();
            var obj = _hrUnitOfWork.Repository<District>().FirstOrDefault(a => a.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = obj,
                ObjectName = "Districts",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete,
            });
            _hrUnitOfWork.LookUpRepository.Remove(obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }

        #endregion

        #region Currency Grid by Mamdouh
        public ActionResult CurrencyIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        //Kend:read ==>Currency
        public ActionResult ReadCurrency(int MenuId)
        {
            var query = _hrUnitOfWork.LookUpRepository.GetCurrency();
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {

                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {

                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(query, JsonRequestBehavior.AllowGet);

        }
        //Kendo:Create ==> currency
        public ActionResult CreateCurrency(IEnumerable<CurrencyViewModel>models ,OptionsViewModel options)
        {
            var result = new List<Currency>();
            var datasource = new DataSource<CurrencyViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId, //CompanyId,
                        ObjectName = "Currencies",
                        TableName = "Currencies",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (CurrencyViewModel model in models)
                {
                    var currency = new Currency();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = currency,
                        Source = model,
                        ObjectName = "Currencies",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Id = "Code",
                        Transtype = TransType.Insert,
                        Options = options 
                         
                    });
                    result.Add(currency);
                    _hrUnitOfWork.LookUpRepository.Add(currency);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from c in models 
                               //join r in result on c.Code equals r.Code
                               select new CurrencyViewModel
                               {
                                   Id = c.Code,
                                   Code = c.Code,
                                   Name = c.Name,
                                   CalcRoundRule = c.CalcRoundRule,
                                   Decimals = c.Decimals,
                                   IsMultiplyBy = c.IsMultiplyBy,
                                   Isocode = c.Isocode,
                                   MidRate = c.MidRate,
                                   PayRoundRule = c.PayRoundRule,
                                   Referenced = c.Referenced,
                                   RoundMethod = c.RoundMethod,
                                   Suffix = c.Suffix,
                                   Symbol = c.Symbol
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }
        //Kendo : Update ==>Currency
        public ActionResult UpdateCurrency(IEnumerable<CurrencyViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<CurrencyViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Currencies",
                        TableName = "Currencies",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Code);
                var db_currency = _hrUnitOfWork.Repository<Currency>().Where(a => ids.Contains(a.Code)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var currency = db_currency.FirstOrDefault(a => a.Code == models.ElementAtOrDefault(i).Code);
                    AutoMapper(new AutoMapperParm() { ObjectName = "Currencies", Destination = currency, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype=TransType.Update, Id = "Code" });
                    _hrUnitOfWork.LookUpRepository.Attach(currency);
                    _hrUnitOfWork.LookUpRepository.Entry(currency).State = EntityState.Modified;
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        // Kendo:destroy ==> District
        public ActionResult DeleteCurrency(string Id)
        {
            var datasource = new DataSource<CurrencyViewModel>();
            var obj = _hrUnitOfWork.Repository<Currency>().FirstOrDefault(a=>a.Code == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = obj,
                ObjectName = "Currencies",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete,
                Id = "Code"
            });
            _hrUnitOfWork.LookUpRepository.Remove(obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }


        


        #endregion

    }
}