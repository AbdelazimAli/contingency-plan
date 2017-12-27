using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using WebApp.Extensions;
using Model.Domain.Payroll;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class BenefitController : BaseController
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
        public BenefitController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
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
        public ActionResult ReadBenefits(int MenuId)
        {
            var query = _hrUnitOfWork.BenefitsRepository.GetBenefits(Language, CompanyId);
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
        public ActionResult GetBenefitServs()
        {
            return Json(_hrUnitOfWork.BenefitsRepository.GetBenefitServs(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadBenefitPlan(int BenefitId = 0)
        {
            return Json(_hrUnitOfWork.BenefitsRepository.ReadBenefitPlan(BenefitId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id = 0)
        {
            fillViewBag();
            if (id == 0)
                return View(new BenefitFormViewModel());
            var benefit = _hrUnitOfWork.BenefitsRepository.ReadBenefit(id, Language);
            return benefit == null ? (ActionResult)HttpNotFound() : View(benefit);
        }
        public void fillViewBag()
        {
            ViewBag.Jobs = _hrUnitOfWork.JobRepository.ReadJobs(CompanyId, Language,0).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.Locations = _hrUnitOfWork.LocationRepository.ReadLocations(Language, CompanyId).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.CompanyStuctures = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId, null, Language);
            ViewBag.Payrolls = _hrUnitOfWork.Repository<Payrolls>().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.Positions = _hrUnitOfWork.PositionRepository.GetPositions(Language, CompanyId).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.PeopleGroups = _hrUnitOfWork.PeopleRepository.GetPeoples().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.PayrollGrades = _hrUnitOfWork.JobRepository.GetPayrollGrade();
            ViewBag.calender = _hrUnitOfWork.Repository<PeriodName>().Select(a => new { id = a.Id, name = a.Name }).ToList();
        }
        [HttpPost]
        public ActionResult Details(BenefitFormViewModel model, OptionsViewModel moreInfo, BenefitPlanVM grid1)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LocationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Benefit",
                        TableName = "Benefits",
                        ParentColumn = "CompanyId",
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
                var sequence = _hrUnitOfWork.Repository<Benefit>().Select(a => a.Code).DefaultIfEmpty(0).Max();
               // var MaxCode 

                Benefit record;
                //insert
                if (model.Id == 0)
                {
                    record = new Benefit();

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Benefit",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    MapBenefit(record, model, moreInfo);
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    if (model.EmpAccural != 3)
                        record.WaitMonth = null;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    _hrUnitOfWork.BenefitsRepository.Add(record);
                }
                //update
                else
                {
                    record = _hrUnitOfWork.Repository<Benefit>().FirstOrDefault(a => a.Id == model.Id);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Benefit",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    MapBenefit(record, model, moreInfo);
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    if (model.EmpAccural != 3)
                        record.WaitMonth = null;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    _hrUnitOfWork.BenefitsRepository.Attach(record);
                    _hrUnitOfWork.BenefitsRepository.Entry(record).State = EntityState.Modified;
                }

                // Save grid1
                errors = SaveGrid(grid1, ModelState.Where(a => a.Key.Contains("grid")), record);

                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                errors = SaveChanges(Language);

                var message = "OK";
                if (errors.Count > 0) message = errors.First().errors.First().message;

                return Json(message);
            }

            return Json(Models.Utils.ParseFormErrors(ModelState));

        }

        private void MapBenefit(Benefit Benefitobject, BenefitFormViewModel model, OptionsViewModel moreInfo)
        {
            model.Locations = model.ILocations == null ? null : string.Join(",", model.ILocations.ToArray());
            model.Jobs = model.IJobs == null ? null : string.Join(",", model.IJobs.ToArray());
            model.Employments = model.IEmployments == null ? null : string.Join(",", model.IEmployments.ToArray());
            model.PeopleGroups = model.IPeopleGroups == null ? null : string.Join(",", model.IPeopleGroups.ToArray());
            model.Payrolls = model.IPayrolls == null ? null : string.Join(",", model.IPayrolls.ToArray());
            model.PayrollGrades = model.IPayrollGrades == null ? null : string.Join(",", model.IPayrollGrades.ToArray());
            model.CompanyStuctures = model.ICompanyStuctures == null ? null : string.Join(",", model.ICompanyStuctures.ToArray());
            model.Positions = model.IPositions == null ? null : string.Join(",", model.IPositions.ToArray());
            moreInfo.VisibleColumns.Add("Locations");
            moreInfo.VisibleColumns.Add("Jobs");
            moreInfo.VisibleColumns.Add("Employments");
            moreInfo.VisibleColumns.Add("PeopleGroups");
            moreInfo.VisibleColumns.Add("Payrolls");
            moreInfo.VisibleColumns.Add("PayrollGrades");
            moreInfo.VisibleColumns.Add("CompanyStuctures");
            moreInfo.VisibleColumns.Add("Positions");
            _hrUnitOfWork.BenefitsRepository.AddLName(Language, Benefitobject.Name, model.Name, model.LocalName);

            AutoMapper(new Models.AutoMapperParm
            {
                Destination = Benefitobject,
                Source = model,
                ObjectName = "Benefit",
                Version = Convert.ToByte(Request.Form["Version"]),
                Options = moreInfo
            });

        }

        private List<Error> SaveGrid(BenefitPlanVM grid1, IEnumerable<KeyValuePair<string, ModelState>> state, Benefit benefit)
        {
            List<Error> errors = new List<Error>();

            // Deleted
            if (grid1.deleted != null)
            {
                foreach (BenefitPlanViewModel model in grid1.deleted)
                {
                    var benefitplan = new BenefitPlan
                    {
                        Id = model.Id
                    };
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Source = benefitplan,
                        ObjectName = "Benefit",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Transtype = TransType.Delete
                    });

                    _hrUnitOfWork.BenefitsRepository.Remove(benefitplan);
                }
            }

            // Exclude delete models from sever side validations
            if (ServerValidationEnabled)
            {
                var modified = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted")));
                if (modified.Count > 0)
                {
                    errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "BenefitPlans",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0) return errors;
                }
            }

            // updated records
            if (grid1.updated != null)
            {
                foreach (BenefitPlanViewModel model in grid1.updated)
                {
                    var benefitplan = new BenefitPlan();
                    _hrUnitOfWork.BenefitsRepository.BenefitServs(model.BenefitServs, model.Id);
                    AutoMapper(new Models.AutoMapperParm { Destination = benefitplan, Source = model, Transtype = TransType.Update });
                    benefitplan.ModifiedTime = DateTime.Now;
                    benefitplan.ModifiedUser = UserName;
                    _hrUnitOfWork.BenefitsRepository.Attach(benefitplan);
                    _hrUnitOfWork.BenefitsRepository.Entry(benefitplan).State = EntityState.Modified;
                }
            }

            // inserted records
            if (grid1.inserted != null)
            {
                foreach (BenefitPlanViewModel model in grid1.inserted)
                {
                    var benefitplan = new BenefitPlan();
                    AutoMapper(new Models.AutoMapperParm { Destination = benefitplan, Source = model,Transtype=TransType.Insert });
                    benefitplan.Benefit = benefit;
                    benefitplan.CreatedTime = DateTime.Now;
                    benefitplan.CreatedUser = UserName;
                    var ids = _hrUnitOfWork.Repository<BenefitServ>().Where(a => model.BenefitServs.Contains(a.Name)).Select(a => a.Id).ToList();
                    var benfitServPlans = new List<BenefitServPlans>();
                    ids.ForEach(delegate (int id)
                    {
                        benfitServPlans.Add(new BenefitServPlans { BenefitPlan = benefitplan, BenefitServId = id });
                    });
                    _hrUnitOfWork.BenefitsRepository.AddRange(benfitServPlans);
                    _hrUnitOfWork.BenefitsRepository.Add(benefitplan);
                }
            }

            return errors;
        }

        public ActionResult Delete(int id)
        {
            var message = "OK";
            DataSource<PositionViewModel> Source = new DataSource<PositionViewModel>();

            Benefit benfobj = _hrUnitOfWork.BenefitsRepository.Get(id);
            if (benfobj != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = benfobj,
                    ObjectName = "Benefit",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });

                _hrUnitOfWork.BenefitsRepository.Remove(benfobj);
                _hrUnitOfWork.BenefitsRepository.RemoveLName(Language, benfobj.Name);
            }
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);


        }

    }
}