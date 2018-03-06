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
using System.Web.Routing;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class KafeelController : BaseController
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
        public KafeelController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;

        }
        // GET: Kafeel
        #region Kafeel
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllKafeels(int MenuId)
        {
            var query = _hrUnitOfWork.LookUpRepository.GetAllKafeels();
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

        public ActionResult UpdateKafeel(IEnumerable<KafeelViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<KafeelViewModel>();
            var result = new List<Kafeel>();
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
                        ObjectName = "Kafeel",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var sequence = _hrUnitOfWork.Repository<Kafeel>().Select(a => a.Code).DefaultIfEmpty(0).Max();
                var MaxCode = sequence == 0 ? 1 : sequence + 1;
                var ids = models.Select(a => a.Id);
                var db_Kafel = _hrUnitOfWork.Repository<Kafeel>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    // Create
                    if (models.ElementAtOrDefault(i).Id == 0)
                    {
                        var kaf = new Kafeel();
                        AutoMapper(new AutoMapperParm() { ObjectName = "Kafeel", Destination = kaf, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i), Transtype = TransType.Insert });
                        updateOnly = false;
                        kaf.CreatedUser = UserName;
                        kaf.CreatedTime = DateTime.Now;
                        kaf.Code = MaxCode++;
                        result.Add(kaf);
                        _hrUnitOfWork.LookUpRepository.Add(kaf);
                    }

                    //Update
                    else
                    {
                        Kafeel kafeel = db_Kafel.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                        AutoMapper(new AutoMapperParm() { ObjectName = "Kafeel", Destination = kafeel, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i), Transtype = TransType.Update });

                        kafeel.ModifiedUser = UserName;
                        kafeel.ModifiedTime = DateTime.Now;
                        _hrUnitOfWork.LookUpRepository.Attach(kafeel);
                        _hrUnitOfWork.LookUpRepository.Entry(kafeel).State = EntityState.Modified;

                    }
                }
                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (updateOnly)
                datasource.Data = models;
            else
            {
                datasource.Data = (from m in models
                                   join r in result on m.Name equals r.Name into g
                                   from r in g.DefaultIfEmpty()
                                   select new KafeelViewModel
                                   {
                                       Id = (r == null ? 0 : r.Id),
                                       Name = m.Name,
                                       Email = m.Email,
                                       Tel = m.Tel,
                                       Address = m.Address,
                                       Code = r.Code,
                                       AddressId = m.AddressId,
                                       
                                   }).ToList();
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }
        public ActionResult DeleteKafeel(int Id) // IEnumerable<KafeelViewModel> models
        {
            var datasource = new DataSource<KafeelViewModel>();
            var kafeel = _hrUnitOfWork.Repository<Kafeel>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = kafeel,
                ObjectName = "Kafeel",
                Transtype = TransType.Delete
            });

            _hrUnitOfWork.LookUpRepository.Remove(kafeel);
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