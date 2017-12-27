using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Payroll;
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
    public class FormulaController : BaseController
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

        public FormulaController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: Formula
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        public ActionResult GetFormulas(int MenuId)
        {
            var query = _hrUnitOfWork.PayrollRepository.ReadFormulaGrid(CompanyId, Language);
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


        [HttpGet]
        public ActionResult Details(int id = 0)
        {
            ViewBag.Curr = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            ViewBag.InfoIds = _hrUnitOfWork.PayrollRepository.GetRangeTableList(CompanyId, Language);

            if (id == 0)
                return View(new FormulaViewModel());
            
            var record = _hrUnitOfWork.PayrollRepository.GetFormulaVM(id);
            return record == null ? (ActionResult)HttpNotFound() : View(record);
        }

        [HttpPost]
        public ActionResult Details(FormulaViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.PayrollRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Formula",
                        TableName = "Formulas",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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
                return Json(Models.Utils.ParseFormErrors(ModelState));


            byte version = Convert.ToByte(Request.Form["Version"]);

            Formula formula = _hrUnitOfWork.PayrollRepository.GetFormula(model.Id);
            if (model.Id == 0)
            { //Add
                formula = new Formula();
                AutoMapper(new Models.AutoMapperParm { Source = model, Destination = formula, Version = version, ObjectName = "Formula", Options = moreInfo ,Transtype = TransType.Insert});
                formula.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                _hrUnitOfWork.PayrollRepository.Add(formula);
            }
            else
            { //Edit
                AutoMapper(new Models.AutoMapperParm { Source = model, Destination = formula, Version = version, ObjectName = "Formula", Options = moreInfo,Transtype=TransType.Update });
                formula.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                _hrUnitOfWork.PayrollRepository.Attach(formula);
                _hrUnitOfWork.PayrollRepository.Entry(formula).State = EntityState.Modified;
            }

            string message = "OK";
            var Errors = SaveChanges(Language);
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;

            return Json(message);
        }

        public ActionResult DeleteFormula(int id)
        {
            List<Error> errors = new List<Error>();

            string message = "OK";
            Formula request = _hrUnitOfWork.PayrollRepository.GetFormula(id);
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = request,
                    ObjectName = "Formula",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.PayrollRepository.Remove(request);
            }
            errors = SaveChanges(Language);
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }
    }
}