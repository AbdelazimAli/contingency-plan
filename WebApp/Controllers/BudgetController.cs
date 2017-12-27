using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BudgetController : BaseController
    { 
         private IHrUnitOfWork _hrUnitOfWork;
        private string Language { get; set; }
        private string UserName { get; set; }
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
        public BudgetController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region BudgetItem
        public ActionResult BudgetItemIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadBudgetItems(int MenuId)
        {
            var query = _hrUnitOfWork.BudgetRepository.GetBudgetsItems();
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

        public ActionResult CreateBudgetItems(IEnumerable<BudgetItemViewModel> models, OptionsViewModel options)
        {
            var result = new List<BudgetItem>();
            var datasource = new DataSource<BudgetItemViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId, //CompanyId,
                        ObjectName = "BudgetItems",
                        TableName = "BudgetItems",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (BudgetItemViewModel model in models)
                {
                    var Budget = new BudgetItem();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = Budget,
                        Source = model,
                        ObjectName = "BudgetItems",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Transtype = TransType.Insert,
                        Options = options

                    });
                    result.Add(Budget);
                    _hrUnitOfWork.BudgetRepository.Add(Budget);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from c in models
                               join r in result on c.Code equals r.Code
                               select new BudgetItemViewModel
                               {
                                   Id = r.Id,
                                   Code = c.Code,
                                   Name = c.Name,
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }
        public ActionResult UpdateBudgetItems(IEnumerable<BudgetItemViewModel> models, IEnumerable<OptionsViewModel> options)
        {      
                var datasource = new DataSource<BudgetItemViewModel>();
                datasource.Data = models;
                datasource.Total = models.Count();

                if (ModelState.IsValid)
                {
                    if (ServerValidationEnabled)
                    {
                        var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                        {
                            CompanyId = CompanyId,
                            ObjectName = "BudgetItems",
                            TableName = "BudgetItems",
                            Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                            Culture = Language
                        });

                        if (errors.Count() > 0)
                        {
                            datasource.Errors = errors;
                            return Json(datasource);
                        }

                        for (var i = 0; i < models.Count(); i++)
                        {
                            var budget = _hrUnitOfWork.BudgetRepository.FindBudgetItem(models.ElementAtOrDefault(i).Id);

                            AutoMapper(new AutoMapperParm() { ObjectName = "BudgetItems", Destination = budget, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i), Transtype = TransType.Update });
                            budget.ModifiedTime = DateTime.Now;
                            budget.ModifiedUser = UserName;
                            

                            _hrUnitOfWork.BudgetRepository.Attach(budget);
                            _hrUnitOfWork.BudgetRepository.Entry(budget).State = EntityState.Modified;

                        }
                        datasource.Errors = SaveChanges(Language);
                    }
                    else
                    {
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                    }
                }


            if (datasource.Errors.Count() > 0)
                    return Json(datasource);
                else
                    return Json(datasource.Data);

            }
        public ActionResult DeleteBudgetItems(int Id)
        {
            var datasource = new DataSource<BudgetItemViewModel>();
            var Obj = _hrUnitOfWork.Repository<BudgetItem>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "BudgetItems",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.BudgetRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }


        #endregion

        #region Budget
        public ActionResult Index()
        {
            ViewBag.PeriodNames = _hrUnitOfWork.BudgetRepository.GetCalender(CompanyId).Select(a=> new {id = a.Id , name = a.Name }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        public ActionResult GetPeriods(int Id)
        {
            return Json(_hrUnitOfWork.BudgetRepository.GetPeriods(Id).Select(a => new { id = a.Id, name = a.Name }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DrawGrid(int periodId)
        {
            List<string> Columns = new List<string>();
            Columns.Add("SubPeriod");
            var data = _hrUnitOfWork.BudgetRepository.CompanyBudget(CompanyId, periodId).ToList();
            var rows = _hrUnitOfWork.BudgetRepository.GetSubPeriods(periodId).Select(a => new { a.Name,a.Id }).ToList();
            Columns.AddRange(_hrUnitOfWork.BudgetRepository.GetBudgetsItems().Select(a => a.Name).ToList());
            List<string> returnData = new List<string>();
            for (int i = 0; i < rows.Count; i++)
            {
                string item = string.Format("{{'SubPeriod' : '{0}'", rows[i].Name);
                StringBuilder v = new StringBuilder();
                var list = data.Where(a => a.PeriodId == rows[i].Id).ToList();
                for (int j = 0; j < list.Count; j++)
                {
                    v.AppendFormat(", '{0}' : {1} ", list.ElementAtOrDefault(j).BudgetItemName, list.ElementAtOrDefault(j).Amount);
                }
                returnData.Add(string.Format("{0}{1} }}", item, v).Replace('\'', '\"'));
            }

            var obj = new { columns = Columns, data = data, d = returnData };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        

        #endregion

    }
}