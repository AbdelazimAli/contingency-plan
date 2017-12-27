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
    public class HospitalController : BaseController
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
        public HospitalController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;

        }

        #region Hosipital
        public ActionResult Index()
        {
            ViewBag.Prov = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Provider", Language).Select(a=> new {value=a.CodeId,text=a.Title });
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        
        public ActionResult GetAllHospitals(int ProviderType)
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetAllHospitals(ProviderType), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateHospital(IEnumerable<HospitalViewModel> models, IEnumerable<OptionsViewModel> options , int ProviderType)
        {

            var datasource = new DataSource<HospitalViewModel>();
            var result = new List<Provider>();
            datasource.Data = models;
            datasource.Total = models.Count();
            bool updateOnly = true;

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Providers",
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
                var db_Provider = _hrUnitOfWork.Repository<Provider>().Where(a => ids.Contains(a.Id)).ToList();
                var sequence = _hrUnitOfWork.Repository<Provider>().Select(a => a.Code).DefaultIfEmpty(0).Max();
                var MaxCode = sequence == 0 ? 1 : sequence + 1;
                for (var i = 0; i < models.Count(); i++)
                {
                    var ProviderRecord = db_Provider.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    if (ProviderRecord == null)
                    {
                        Provider Provider = new Provider();
                        AutoMapper(new AutoMapperParm() { ObjectName = "Providers", Destination = Provider, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i), Transtype = TransType.Insert });
                        // Create
                            updateOnly = false;
                            Provider.CreatedUser = UserName;
                            Provider.ProviderType = (short)ProviderType;
                            Provider.CreatedTime = DateTime.Now;
                            Provider.Code = MaxCode++;
                            result.Add(Provider);
                            _hrUnitOfWork.LookUpRepository.Add(Provider);
                        
                    }
                    //Update
                    else
                    {
                        AutoMapper(new AutoMapperParm() { ObjectName = "Providers", Destination = ProviderRecord, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i), Transtype = TransType.Insert });
                        ProviderRecord.ModifiedUser = UserName;
                        ProviderRecord.ModifiedTime = DateTime.Now;
                        ProviderRecord.ProviderType = (short)ProviderType;
                        _hrUnitOfWork.LookUpRepository.Attach(ProviderRecord);
                        _hrUnitOfWork.LookUpRepository.Entry(ProviderRecord).State = EntityState.Modified;
                    }
                }
                datasource.Errors = SaveChanges(Language);

            }
            else  datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);

            if (updateOnly) datasource.Data = models;
            else
            {
                datasource.Data = (from m in models
                                   join r in result on m.Name equals r.Name into g
                                   from r in g.DefaultIfEmpty()
                                   select new HospitalViewModel
                                   {
                                       Id = (r == null ? 0 : r.Id),
                                       Name = m.Name,
                                       Email = m.Email,
                                       Tel = m.Tel,
                                       Address = m.Address,
                                       Code = r.Code,
                                       Manager = m.Manager,
                                       AddressId = m.AddressId,
                                       CreatedTime=m.CreatedTime,
                                       ProviderType=m.ProviderType,
                                       CreatedUser=m.CreatedUser,
                                       ModifiedUser=m.ModifiedUser,
                                       ModifiedTime=m.ModifiedTime

                                   }).ToList();
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }
        public ActionResult DeleteHospital(int Id)
        {
            var datasource = new DataSource<HospitalViewModel>();
            var Obj = _hrUnitOfWork.Repository<Provider>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "Providers",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });

            _hrUnitOfWork.LookUpRepository.Remove(Obj);
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