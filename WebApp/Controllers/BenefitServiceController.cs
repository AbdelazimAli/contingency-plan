using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BenefitServiceController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        private string Language { get; set; }
        private int CompanyId { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
            }
        }
        public BenefitServiceController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: BenefitService
        public ActionResult Index()
        {
            ViewBag.curr = _hrUnitOfWork.LookUpRepository.GetCurrency().Select(a => new { value = a.Code, text = a.Code});
            ViewBag.BenefitId = _hrUnitOfWork.BenefitsRepository.GetBenefits(User.Identity.GetLanguage(),User.Identity.GetDefaultCompany()).Select(a => new { value =a.Id, text = a.LocalName }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetBenefitServ(bool IsGroup, int ? Id)
        {
            var result = _hrUnitOfWork.BenefitsRepository.GetBenefitServ(IsGroup,Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateBenefitServ(IEnumerable<BenfitServiceViewModel> models ,bool IsGroup, OptionsViewModel moreInfo)
        {
            var result = new List<BenefitServ>();
            var datasource = new DataSource<BenfitServiceViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "BenefitServ",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });
                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var sequence = _hrUnitOfWork.Repository<BenefitServ>().Select(a => a.Code).DefaultIfEmpty(0).Max();
                var MaxCode = sequence == 0 ? 1 : sequence + 1;
                foreach (BenfitServiceViewModel model in models)
                {
                    var benfitSer = new BenefitServ();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = benfitSer,
                        Source = model,
                        ObjectName = "BenefitServ",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    benfitSer.IsGroup = IsGroup;
                    benfitSer.Code = MaxCode++;
                    if (benfitSer.ParentId != null)
                        benfitSer.BenefitId = _hrUnitOfWork.Repository<BenefitServ>().FirstOrDefault(a => a.Id == model.ParentId.GetValueOrDefault()).BenefitId;
                    result.Add(benfitSer);
                    _hrUnitOfWork.BenefitsRepository.Add(benfitSer);
                }
                datasource.Errors = SaveChanges(Language);
                datasource.Data = (from m in models
                                   join r in result on m.Name equals r.Name into g
                                   from r in g.DefaultIfEmpty()
                                   select new BenfitServiceViewModel
                                   {
                                       Id = (r == null ? 0 : r.Id),
                                       Name = m.Name,
                                       Code = r.Code,
                                       EmpPercent = m.EmpPercent,
                                       CompPercent = m.CompPercent,
                                       EndDate = m.EndDate,
                                       StartDate = m.StartDate,
                                       IsGroup = m.IsGroup,
                                       ParentId = m.ParentId,
                                       Cost = m.Cost,
                                       Curr = m.Curr,
                                       BenefitId = r.BenefitId
                                   }).ToList();
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

        public ActionResult UpdateBenefitServ(IEnumerable<BenfitServiceViewModel> models, IEnumerable<OptionsViewModel> options , bool IsGroup)
        {
            var datasource = new DataSource<BenfitServiceViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "BenefitServ",
                        ParentColumn = "ParentId",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
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
                    var benfitserv = _hrUnitOfWork.Repository<BenefitServ>().FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);

                    AutoMapper(new AutoMapperParm() { ObjectName = "BenefitServ", Destination = benfitserv, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype=TransType.Update});
                    benfitserv.IsGroup = IsGroup;
                    _hrUnitOfWork.BenefitsRepository.Attach(benfitserv);
                    _hrUnitOfWork.BenefitsRepository.Entry(benfitserv).State = EntityState.Modified;
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
        public ActionResult DeleteBenefitServ(int Id)
        {
            var datasource = new DataSource<BenfitServiceViewModel>();

            var Obj = _hrUnitOfWork.Repository<BenefitServ>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "BenefitServ",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.BenefitsRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }

    }
}