using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApp.Extensions;
using System;
using System.Linq.Dynamic;
using System.Web.Routing;
using Model.ViewModel.Administration;
using System.Web.Script.Serialization;

namespace WebApp.Controllers
{
    public class SiteController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        public SiteController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetSites(int MenuId)
        {
            var query = _hrUnitOfWork.SiteRepository.ReadSites(Language, CompanyId);
            var x = query.ToList();
            string whereclause = GetWhereClause(MenuId);
            if (whereclause.Length > 0)
            {
                try
                {
                    query = query.Where(whereclause);
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

        [HttpGet]
        public ActionResult Details(int id = 0, int Read = 0)
        {
            ViewBag.TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(a => new { id = a.Id, name = a.DisplayName });
            ViewBag.Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language).ToList();

            if (id == 0)
                return View(new AddSiteViewModel());

            var site = _hrUnitOfWork.SiteRepository.ReadSite(id, Read, Language);
            if (site.CountryId > 0)
            {
                var city = site.CityId ?? 0;
                var dist = site.DistrictId ?? 0;
                var loc = _hrUnitOfWork.Repository<World>().FirstOrDefault(c => c.CountryId == site.CountryId
                && c.CityId == city && c.DistrictId == dist);
                if (site != null)
                     site.Site = (Language.Substring(0, 2) == "ar" ? loc.NameAr : loc.Name);
            }
            return site == null ? (ActionResult)HttpNotFound() : View(site);
        }
        public ActionResult GetMaxCode(int SiteTypeId)
        {
            var sequence = _hrUnitOfWork.Repository<Site>().Where(a => a.SiteType == SiteTypeId).Select(a => a.Code).DefaultIfEmpty(0).Max();
            var MaxCode = sequence == 0 ? 1 : sequence + 1;
            return Json(MaxCode, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(SiteFormViewModel model, OptionsViewModel moreInfo, bool clear = false)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FormSite",
                        TableName = "Sites",
                        Columns = Models.Utils.GetColumnViews(ModelState),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        foreach (var e in errors)
                        {
                            foreach (var errorMsg in e.errors)
                            {
                                ModelState.AddModelError(errorMsg.field, errorMsg.message);
                            }
                        }

                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }

                var sequence = _hrUnitOfWork.Repository<Site>().Where(a=>a.SiteType == model.SiteType).Select(a => a.Code).DefaultIfEmpty(0).Max();
                var MaxCode = sequence == 0 ? 1 : sequence + 1;

                var record = _hrUnitOfWork.Repository<Site>().FirstOrDefault(j => j.Id == model.Id);
                if (record == null) //Add
                {
                    record = new Site();
                    _hrUnitOfWork.SiteRepository.AddLName(Language, record.Name, model.Name, model.LName);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "FormSite",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    record.Code = model.Code == 0 ? MaxCode : model.Code;
                   
                    _hrUnitOfWork.SiteRepository.Add(record);
                    if (model.SiteToEmployees != null)
                    {
                        foreach (var item in model.SiteToEmployees)
                        {
                            AddSiteToEmployee(record, item);
                        }
                    }

                }
                else //update
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "FormSite",
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                   
                    var query = _hrUnitOfWork.Repository<SiteToEmp>().Where(a => a.SiteId == model.Id).Select(a => new { empID = a.EmpId}).ToList();
                    if (model.SiteToEmployees != null)
                    {
                        foreach (var item in model.SiteToEmployees)
                        {
                            var chk = query.Any(a => a.empID == item);
                            if (!chk)
                                AddSiteToEmployee(record, item);
                        }
                    }
                    if (query.Count > 0)
                    {
                        foreach (var item in query)
                        {
                            bool isInList = model.SiteToEmployees.Contains(item.empID);
                            if (!isInList)
                            {
                                RemoveSiteFromEmployee(model.Id, item.empID);
                            }
                        }
                    }
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.Code = model.Code == 0 ? MaxCode : model.Code;
                }

                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                string message = "OK";
                var Errors = SaveChanges(Language);

                if (clear)
                {
                    model = new SiteFormViewModel();
                    message += "," + (new JavaScriptSerializer()).Serialize(model);
                }
                else               
                    message += "," + (new JavaScriptSerializer()).Serialize(record);              

                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        private void AddSiteToEmployee(Site record, int EmpId)
        {
            var SiteEmp = new SiteToEmp
            {
                Site = record,
                EmpId = EmpId
            };
            _hrUnitOfWork.SiteRepository.Add(SiteEmp);
        }
        private void RemoveSiteFromEmployee(int Id,int EmpId)
        {
            var SiteEmp = _hrUnitOfWork.Repository<SiteToEmp>().Where(a => a.SiteId == Id && a.EmpId == EmpId).FirstOrDefault();
            _hrUnitOfWork.SiteRepository.Remove(SiteEmp);
        }
        public ActionResult DeleteSite(int id)
        {
            var message = "OK";
            DataSource<SiteViewModel> Source = new DataSource<SiteViewModel>();
            Site location = _hrUnitOfWork.SiteRepository.Get(id);
            if (location != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = location,
                    ObjectName = "Sites",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.SiteRepository.Remove(location);
                _hrUnitOfWork.SiteRepository.RemoveLName(Language, location.Name);

            }
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }
    }
}