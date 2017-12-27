using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApp.Extensions;
using System.Web.Script.Serialization;
using WebApp.Models;
using System;
using System.Linq.Dynamic;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class CompanyController : BaseController
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

        public ActionResult Branches(int id = 0)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult Partners(int id = 0)
        {
            ViewBag.Id = id;
            return View();
        }
        public CompanyController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        public string GetConsolidationCompany(int Id)
        {
            var query = _hrUnitOfWork.CompanyRepository
                .Find(c => c.Consolidation)
                .Select(c => new { id = c.Id, name = c.Name })
                .ToList();

            if (query.Count == 0)
                return "0";
            else
            {
               
                if (query[0].id == Id)
                    return "0";
                else
                    return query[0].name;
            }
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
                if (location != null)
                    ViewBag.location = (Language.Substring(0, 2) == "ar" ? location.NameAr : location.Name);
            }

            List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("Addresses", CompanyId, 0);
            if (columns.FirstOrDefault(c => c == "Location") == null)
                ViewBag.LocationList = _hrUnitOfWork.Repository<World>().Select(c => new { id = c.CountryId, country = c.CountryId, city = c.CityId, dist = c.DistrictId, name = c.Name }).ToList();

            return PartialView("_Address", address);
        }

        // POST: Address
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult PostAddress([Bind(Include = "Id,Name,Address1,Address2,Address3,CountryId,CityId,DistrictId,PostalCode,Telephone,Latitude,Longitude")] Address address, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Addresses",
                        TableName = "Addresses",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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

                string message = "OK,";
                if (address.CityId == 0) address.CityId = null;
                if (address.DistrictId == 0) address.DistrictId = null;

                if (address.Id == 0) // New 
                    _hrUnitOfWork.CompanyRepository.Add(address);
                else // Edit 
                {
                    Address oldAddress = _hrUnitOfWork.CompanyRepository.GetAddress(address.Id);
                    byte version;
                    byte.TryParse(Request.Form["version"], out version);
                    AutoMapper(new AutoMapperParm() { Source = address, Destination = oldAddress, ObjectName = "Addresses", Version = version, Options = moreInfo,Transtype = TransType.Update });

                    _hrUnitOfWork.CompanyRepository.Attach(oldAddress);
                    _hrUnitOfWork.CompanyRepository.Entry(oldAddress).State = EntityState.Modified;
                }
                var Errors = SaveChanges(Language);
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;
                else
                    message += (new JavaScriptSerializer()).Serialize(address);

                return Json(message);
            }
            return Json(Models.Utils.ParseFormErrors(ModelState));
        }
        // GET: Company
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadCompanyBranches(int Id)
        {
            var query = _hrUnitOfWork.CompanyRepository.GetCompanyBranches(Id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadCompanyPartners(int Id)
        {
            var query = _hrUnitOfWork.CompanyRepository.GetCompanyPartners(Id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Countries()
        {
            return Json(_hrUnitOfWork.Repository<Country>().Select(c => new { id = c.Id, name = c.Name }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult World()
        {
            return Json(_hrUnitOfWork.Repository<World>().Select(c => new { country = c.CountryId, city = c.CityId, dist = c.DistrictId, name = c.Name }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWorld(string query)
        {
            return Json(_hrUnitOfWork.Repository<World>().Where(c => c.Name.Contains(query)).Select(c => new { country = c.CountryId, city = c.CityId, dist = c.DistrictId, name = c.Name }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCompanies(int MenuId)
        {
            var query = _hrUnitOfWork.CompanyRepository.GetAllCompanies(Language);
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

        public JsonResult PurposeList()
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetLookUpCodes("Purpose", Language).Select(a => new { id = a.CodeId, name = a.Title }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCompany(int id)
        {
            DataSource<CompanyViewModel> Source = new DataSource<CompanyViewModel>();
            UserContext db = new UserContext();
            var UserList = db.Users.Where(a => a.Companies.Where(c => c.Id == id).Count() > 0 || a.DefaultCompany == id).ToList();
            var Company = _hrUnitOfWork.CompanyRepository.Find(a => a.Id == id).FirstOrDefault();
            List<Error> errors = new List<Error>();
            List<ErrorMessage> errorMessages = new List<ErrorMessage>();
            if (id == 0)
            {

                ErrorMessage errormessa = new ErrorMessage() { message = MsgUtils.Instance.Trls("Defaultcannotdeleted") };
                errorMessages.Add(errormessa);
                Error err = new Error() { errors = errorMessages };
                errors.Add(err);
                Source.Errors = errors;
                return Json(Source);
            }
            else if (UserList.Count() > 0)
            {
                ErrorMessage errormessa = new ErrorMessage() { message = MsgUtils.Instance.Trls("Companyhasusers") };
                errorMessages.Add(errormessa);

                Error err = new Error() { errors = errorMessages };
                errors.Add(err);
                Source.Errors = errors;
                return Json(Source);
            }
            else
            {
                var CompanyDivs = _hrUnitOfWork.Repository<PageDiv>().Where(j => j.CompanyId == id).ToList();
                var ColumnsTitles = _hrUnitOfWork.Repository<ColumnTitle>().Where(t => t.CompanyId == id).ToList();
                _hrUnitOfWork.PageEditorRepository.RemoveRange(CompanyDivs);
                _hrUnitOfWork.PageEditorRepository.RemoveRange(ColumnsTitles);
                _hrUnitOfWork.CompanyRepository.Remove(Company);
                _hrUnitOfWork.CompanyRepository.RemoveLName(Language,Company.Name);
            }

            errors = SaveChanges(Language);

            if (errors.Count() > 0)
                Source.Errors = errors;

            return Json(Source);

        }
        void FillViewBag()
        {
            //ViewBag.Code = _hrUnitOfWork.Repository<Model.Domain.Company>().Select(a => a.Code).DefaultIfEmpty(0).Max();
            if (Language.Substring(0, 2) == "ar")
                ViewBag.Country = _hrUnitOfWork.Repository<Country>().Select(c => new { id = c.Id, name = c.NameAr }).ToList();
            else
                ViewBag.Country = _hrUnitOfWork.Repository<Country>().Select(c => new { id = c.Id, name = c.Name }).ToList();
        }
        // GET: Company/Details
        public ActionResult Details(int id = -1)
        {
            FillViewBag();

            if (id == -1)
                return View(new CompanyFormViewModel());

            var company = _hrUnitOfWork.CompanyRepository.ReadCompany(id, Language);
            //ViewBag.Attachments = _hrUnitOfWork.CompanyRepository.CompanyAttachmentsCount(id);
            return company == null ? (ActionResult)HttpNotFound() : View(company);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        //public async System.Threading.Tasks.Task<ActionResult> Details(CompanyFormViewModel model, OptionsViewModel moreInfo) //, System.Web.HttpPostedFileBase upload 
        [HttpPost]
        public ActionResult Details(CompanyFormViewModel model, OptionsViewModel moreInfo) //, System.Web.HttpPostedFileBase upload 
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Company",
                        TableName = "Companies",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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

            Model.Domain.Company record;
            //var Sequence = _hrUnitOfWork.Repository<Model.Domain.Company>().Select(a => a.Code).DefaultIfEmpty(0).Max();
            //var MaxCode = Sequence == 0 ? 1 : Sequence + 1;
            var edit = Request.QueryString["edit"];
            if (model.Id == -1) // New
            {
                record = new Model.Domain.Company();
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = record,
                    Source = model,
                    ObjectName = "Company",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });
                record.CreatedUser = UserName;
                record.CreatedTime = DateTime.Now;
               // record.Code = MaxCode;
                _hrUnitOfWork.CompanyRepository.AddLName(Language, record.Name, model.Name, model.LocalName);
                _hrUnitOfWork.PagesRepository.AddCompany(record);
            }

            else // Edit
            {
                record = _hrUnitOfWork.Repository<Model.Domain.Company>().FirstOrDefault(a => a.Id == model.Id);
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = record,
                    Source = model,
                    ObjectName = "Company",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Update
                });
                record.ModifiedTime = DateTime.Now;
                record.ModifiedUser = UserName;
                _hrUnitOfWork.CompanyRepository.AddLName(Language, record.Name, model.Name, model.LocalName);
                _hrUnitOfWork.CompanyRepository.Attach(record);
                _hrUnitOfWork.CompanyRepository.Entry(record).State = EntityState.Modified;
            }

            try
            {
                _hrUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg);
            }

            var savedCompany = _hrUnitOfWork.Repository<Model.Domain.Company>().FirstOrDefault(c => c.Name == model.Name);
            string message = "OK," + (new JavaScriptSerializer().Serialize(savedCompany));
            return Json(message);
        }

        private Models.Menu GetNewMenu(Models.Company company, Models.Menu copy, Models.Menu parent)
        {
            return new Models.Menu
            {
                Name = copy.Name,
                Order = copy.Order,
                //MenuLevel = copy.MenuLevel,
                Parent = parent,
                Sort = copy.Sort,
                Url = copy.Url,
                Icon = copy.Icon,
                Version = copy.Version,
                Company = company,
                ColumnList = copy.ColumnList
            };
        }
        [HttpPost]
        public ActionResult CreateBranch(IEnumerable<BranchesViewModel> models)
        {
            var result = new List<CompanyBranch>();
            var datasource = new DataSource<BranchesViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CompanyBranches",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                //  Iterate all updated rows which are posted by the PageDiv
                foreach (BranchesViewModel model in models)
                {
                    // Create a new branch entity and set its properties from branchViewModel
                    var branch = new CompanyBranch
                    {
                        BranchNo = model.BranchNo,
                        Name = model.Name,
                        Email = model.Email,
                        Telephone = model.Telephone,
                        CompanyId = model.CompanyId,
                        AddressId = model.AddressId,
                        CreatedTime = DateTime.Now,
                        CreatedUser = UserName
                    };

                    result.Add(branch);
                    _hrUnitOfWork.CompanyRepository.Add(branch);
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }


            datasource.Data = (from m in models
                               join r in result on m.Name equals r.Name into g
                               from r in g.DefaultIfEmpty()
                               select new BranchesViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   BranchNo = m.BranchNo,
                                   Name = m.Name,
                                   Email = m.Email,
                                   Telephone = m.Telephone,
                                   Address = m.Address,
                                   CompanyId = m.CompanyId,
                                   AddressId = m.AddressId,
                                   CreatedTime = DateTime.Now,
                                   CreatedUser = UserName
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        [HttpPost]
        public ActionResult CreatePartner(IEnumerable<PartnersViewModel> models)
        {
            var result = new List<CompanyPartner>();
            var datasource = new DataSource<PartnersViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CompanyPartners",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });
                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                //  Iterate all updated products which are posted by the Kendo PageDiv
                foreach (PartnersViewModel model in models)
                {
                    // Create a new branch entity and set its properties from branchViewModel
                    var partner = new CompanyPartner
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Telephone = model.Telephone,
                        CompanyId = model.CompanyId,
                        AddressId = model.AddressId,
                        Mobile = model.Mobile,
                        JobTitle = model.JobTitle,
                        NationalId = model.NationalId,
                        CreatedTime = DateTime.Now,
                        CreatedUser = UserName

                    };

                    result.Add(partner);
                    _hrUnitOfWork.CompanyRepository.Add(partner);
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            // Return the inserted products - the Kendo PageDiv needs their ID which is generated by SQL server during insertion
            datasource.Data = (from m in models
                               join r in result on m.Name equals r.Name into g
                               from r in g.DefaultIfEmpty()
                               select new PartnersViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   Name = m.Name,
                                   Email = m.Email,
                                   Telephone = m.Telephone,
                                   Address = m.Address,
                                   CompanyId = m.CompanyId,
                                   AddressId = m.AddressId,
                                   Mobile = m.Mobile,
                                   JobTitle = m.JobTitle,
                                   NationalId = m.NationalId,
                                   CreatedTime = DateTime.Now,
                                   CreatedUser = UserName
                               }).ToList();


            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        [HttpPost]
        public ActionResult PutBranch(IEnumerable<BranchesViewModel> models, IEnumerable<OptionsViewModel> options) // [DataSourceRequest] DataSourceRequest dataSourceRequest, 
        {
            var datasource = new DataSource<BranchesViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CompanyBranches",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        ParentColumn = "CompanyId",
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
                    var branch = new CompanyBranch();

                    AutoMapper(new AutoMapperParm() { ObjectName = "CompanyBranches", Destination = branch, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i) });
                    branch.ModifiedTime = DateTime.Now;
                    branch.ModifiedUser = UserName;

                    _hrUnitOfWork.CompanyRepository.Attach(branch);
                    _hrUnitOfWork.CompanyRepository.Entry(branch).State = EntityState.Modified;
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

        [HttpPost]
        public ActionResult UpdatePartner(IEnumerable<PartnersViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<PartnersViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CompanyPartners",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        ParentColumn = "CompanyId",
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
                    var partner = new CompanyPartner();
                    AutoMapper(new AutoMapperParm() { ObjectName = "CompanyPartners", Destination = partner, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i) });
                    partner.ModifiedTime = DateTime.Now;
                    partner.ModifiedUser = UserName;


                    _hrUnitOfWork.CompanyRepository.Attach(partner);
                    _hrUnitOfWork.CompanyRepository.Entry(partner).State = EntityState.Modified;
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

        [HttpPost]
        public ActionResult DeleteBranch(IEnumerable<BranchesViewModel> models)
        {
            var datasource = new DataSource<BranchesViewModel>();

            if (ModelState.IsValid)
            {
                foreach (BranchesViewModel model in models)
                {
                    var branch = new CompanyBranch
                    {
                        Id = model.Id,
                        AddressId = model.AddressId
                    };

                    _hrUnitOfWork.CompanyRepository.Remove(branch);
                }

                datasource.Errors = SaveChanges(Language);
                datasource.Total = models.Count();
            }

            datasource.Data = models;

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        [HttpPost]
        public ActionResult DeletePartner(IEnumerable<PartnersViewModel> models)
        {
            var datasource = new DataSource<PartnersViewModel>();

            if (ModelState.IsValid)
            {
                foreach (PartnersViewModel model in models)
                {
                    var partner = new CompanyPartner
                    {
                        Id = model.Id,
                        AddressId = model.AddressId
                    };

                    _hrUnitOfWork.CompanyRepository.Remove(partner);
                }

                datasource.Errors = SaveChanges(Language);
                datasource.Total = models.Count();
            }

            datasource.Data = models;

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
    }
}
