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
using Model.ViewModel.Loans;

namespace WebApp.Controllers.Loan
{
    public class LoanTypeController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        private string UserName { get; set; }
        private string Language { get; set; }
        private int CompanyId { get; set; }
        public LoanTypeController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        // GET: LoanType
        public ActionResult LoanTypeIndex()
        {
            return View();
        }

        //Kend:read ==>LoanType
        public ActionResult ReadLoanType(int MenuId)
        {
            var query = _hrUnitOfWork.LoanTypeRepository.GetLoanType(Language);
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
        //Kendo:Create ==> LoanType
        public ActionResult CreateLoanType(IEnumerable<LoanTypeViewModel> models, OptionsViewModel options)
        {
            var result = new List<LoanType>();
            var datasource = new DataSource<LoanTypeViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LoanTypeRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId, //CompanyId,
                        ObjectName = "LoanTypes",
                        TableName = "LoanTypes",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (LoanTypeViewModel model in models)
                {
                    var loanType = new LoanType();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = loanType,
                        Source = model,
                        ObjectName = "LoanTypes",
                        Id = "Id",
                        Transtype = TransType.Insert,
                        Options = options

                    });
                    result.Add(loanType);
                    _hrUnitOfWork.LoanTypeRepository.Add(loanType);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from c in models
                                   //join r in result on c.Code equals r.Code
                               select new LoanTypeViewModel 
                               {
                                   Id = c.Id,
                                   Name =c.Name,
                                   StartDate =c.StartDate,
                                   EndDate=c.EndDate

                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }

        //Kendo : Update ==>Currency
        public ActionResult UpdateLoanType(IEnumerable<LoanTypeViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<LoanTypeViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LoanTypes",
                        TableName = "LoanTypes",
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
                var db_LoanType = _hrUnitOfWork.Repository<LoanType>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var loanType = db_LoanType.FirstOrDefault(a => a.Id== models.ElementAtOrDefault(i).Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "LoanTypes", Destination = loanType, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i), Transtype = TransType.Update, Id = "Id" });
                    _hrUnitOfWork.LoanTypeRepository.Attach(loanType);
                    _hrUnitOfWork.LoanTypeRepository.Entry(loanType).State = EntityState.Modified;
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
        public ActionResult DeleteLoanType(int Id)
        {
            var datasource = new DataSource<LoanTypeViewModel>();
            var obj = _hrUnitOfWork.Repository<LoanType>().FirstOrDefault(a => a.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = obj,
                ObjectName = "LoanTypes",
                Transtype = TransType.Delete,
                Id = "Id"
            });
            _hrUnitOfWork.LoanTypeRepository.Remove(obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }



    }
}