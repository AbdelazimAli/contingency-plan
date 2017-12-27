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

namespace WebApp.Controllers
{
    public class LocationController : BaseController
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
        public LocationController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        public ActionResult LocationIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult Index(bool isInternal = false)
        {
            ViewBag.IsInternal = isInternal == true ? "Locations" : "OutLocations";
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetLocation(string MenuName)
        {
            var MenuId = _hrUnitOfWork.Repository<Menu>().Where(a => a.CompanyId == CompanyId && a.Name == MenuName && a.Url == null).Select(a => a.Id).FirstOrDefault();
            var query = _hrUnitOfWork.LocationRepository.ReadLocations(Language, CompanyId);
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
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
                return View(new AddLocationViewModel());

            var location = _hrUnitOfWork.LocationRepository.ReadLocation(id, Language);
            return location == null ? (ActionResult)HttpNotFound() : View(location);
        }
        [HttpPost]
        public ActionResult Details(AddLocationViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            var msg = "OK";

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LocationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FormLocation",
                        TableName = "Locations",
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
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            var sequence = _hrUnitOfWork.Repository<Location>().Select(a => a.Code).DefaultIfEmpty(0).Max();
            var MaxCode = sequence == 0 ? 1 : sequence + 1;
            var loc = new Location();

            if (model.Id == 0) // New
            {
                model.CompanyId = CompanyId;
                _hrUnitOfWork.LocationRepository.AddLName(Language, loc.Name, model.Name, model.LName);

                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = loc,
                    Source = model,
                    ObjectName = "FormLocation",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });
                loc.CreatedTime = DateTime.Now;
                loc.CreatedUser = UserName;
                loc.Code = MaxCode;
                _hrUnitOfWork.LocationRepository.Add(loc);

            }
            else // Edit
            {
                loc = _hrUnitOfWork.LocationRepository.Find(a => a.Id == model.Id).FirstOrDefault();
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = loc,
                    Source = model,
                    ObjectName = "FormLocation",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Update
                });

                loc.ModifiedTime = DateTime.Now;
                loc.ModifiedUser = UserName;
                _hrUnitOfWork.LocationRepository.Attach(loc);
                _hrUnitOfWork.LocationRepository.Entry(loc).State = EntityState.Modified;
                _hrUnitOfWork.LocationRepository.AddLName(Language, loc.Name, model.Name, model.LName);

            }
            errors = SaveChanges(Language);
            if (errors.Count() > 0)
            {
                foreach (var item in errors)
                {
                    msg = item.errors.Select(a => a.message).FirstOrDefault();
                }
                return Json(msg);
            }


            return Json(msg);
        }
        public ActionResult GetAddress(int addressId)
        {
            var address = _hrUnitOfWork.CompanyRepository.GetAddress(addressId);

            if (address.CountryId > 0)
            {
                var city = address.CityId ?? 0;
                var dist = address.DistrictId ?? 0;
                var location = _hrUnitOfWork.Repository<World>().FirstOrDefault(c => c.CountryId == address.CountryId
                && c.CityId == city && c.DistrictId == dist);
            }

            return Json(address);
        }
        public ActionResult DeleteLocation(int id)
        {
            var message = "OK";
            DataSource<LocationViewModel> Source = new DataSource<LocationViewModel>();
            Location location = _hrUnitOfWork.LocationRepository.Get(id);

            //if (location.AddressId != null)
            //{
            //    var Address = _hrUnitOfWork.Repository<Address>().FirstOrDefault(a => a.Id == location.AddressId);
            //    if (Address != null)
            //        _hrUnitOfWork.PageEditorRepository.Remove(Address);
            //}
            if (location != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = location,
                    ObjectName = "Locations",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.LocationRepository.Remove(location);
                _hrUnitOfWork.LocationRepository.RemoveLName(Language,location.Name);

            }
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }
    }
}