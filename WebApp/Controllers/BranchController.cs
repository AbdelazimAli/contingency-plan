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
    public class BranchController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        public BranchController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }   
        public ActionResult GetBranches(int MenuId)
        {
            var query = _hrUnitOfWork.BranchRepository.ReadBranches(Language, CompanyId);
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
            ViewBag.TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(a => new { id = a.Id, name = a.DisplayName });
         
            if (id == 0)
                return View(new AddBranchViewModel());

            var branch = _hrUnitOfWork.BranchRepository.ReadBranch(id, Language);
            if (branch.CountryId > 0)
            {
                var city = branch.CityId ?? 0;
                var dist = branch.DistrictId ?? 0;
                var loc = _hrUnitOfWork.Repository<World>().FirstOrDefault(c => c.CountryId == branch.CountryId
                && c.CityId == city && c.DistrictId == dist);
                if (branch != null)
                    branch.Site = (Language.Substring(0, 2) == "ar" ? loc.NameAr : loc.Name);
            }
            return branch == null ? (ActionResult)HttpNotFound() : View(branch);
        }

        [HttpPost]
        public ActionResult Details(AddBranchViewModel model, OptionsViewModel moreInfo, bool clear)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.SiteRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FormBranch",
                        TableName = "Branches",
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

            var sequence = _hrUnitOfWork.Repository<Branch>().Select(a => a.Code).DefaultIfEmpty(0).Max();
            var MaxCode = sequence == 0 ? 1 : sequence + 1;
            var branch = new Branch();

            if (model.Id == 0) // New
            {
                model.CompanyId = CompanyId;
                _hrUnitOfWork.BranchRepository.AddLName(Language, branch.Name, model.Name, model.LName);

                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = branch,
                    Source = model,
                    ObjectName = "FormBranch",
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });
                branch.CreatedTime = DateTime.Now;
                branch.CreatedUser = UserName;
                branch.Code = MaxCode;
                _hrUnitOfWork.BranchRepository.Add(branch);

            }
            else // Edit
            {
                branch = _hrUnitOfWork.BranchRepository.Find(a => a.Id == model.Id).FirstOrDefault();
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = branch,
                    Source = model,
                    ObjectName = "FormBranch",
                    Options = moreInfo,
                    Transtype = TransType.Update
                });

                branch.ModifiedTime = DateTime.Now;
                branch.ModifiedUser = UserName;
                _hrUnitOfWork.BranchRepository.AddLName(Language, branch.Name, model.Name, model.LName);
            }

            string message = "OK";
            var Errors = SaveChanges(Language);
            model.Id = branch.Id;
            if (clear) model = new AddBranchViewModel();
            message += "," + (new JavaScriptSerializer()).Serialize(model);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;

            return Json(message);
        }
        public ActionResult DeleteBranch(int id)
        {
            var message = "OK";
            DataSource<BranchViewModel> Source = new DataSource<BranchViewModel>();
            Branch branch = _hrUnitOfWork.BranchRepository.Get(id);
            if (branch != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = branch,
                    ObjectName = "Branches",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.BranchRepository.Remove(branch);
                _hrUnitOfWork.BranchRepository.RemoveLName(Language,branch.Name);
            }
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }

    }
}