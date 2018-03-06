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
    public class QualificationsController : BaseController
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
        public QualificationsController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;

        }
        // GET: Qualification
        public ActionResult Index()
        {
            ViewBag.QualRank = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("QualRank", Language).Select(a => new { value = a.CodeId, text = a.Title });
            ViewBag.QualCat = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("QualCat", Language).Select(a => new { value = a.CodeId, text = a.Title });
            return View();
        }
        public ActionResult SchoolIndex()
        {
            string culture = Language;
            ViewBag.SchoolTypes = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("SchoolType", culture).Select(a => new { value = a.CodeId, text = a.Title });
            ViewBag.classifications = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("SchoolClass", culture).Select(a => new { value = a.CodeId, text = a.Title });
            return View();
        }


        public ActionResult GetQualGroup()
        {
            return Json(_hrUnitOfWork.QualificationRepository.GetQualGroups(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSchool()
        {
            return Json(_hrUnitOfWork.QualificationRepository.GetSchools(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateQualGroup(IEnumerable<QualGroupsViewModel> models, OptionsViewModel moreInfo)
        {
            var result = new List<QualGroup>();
            var datasource = new DataSource<QualGroupsViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "QualGroups",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var sequence = _hrUnitOfWork.Repository<QualGroup>().Select(a => a.Code).DefaultIfEmpty(0).Max();
                var MaxCode = sequence == 0 ? 1 : sequence + 1;

                foreach (QualGroupsViewModel model in models)
                {
                    var qualGroup = new QualGroup();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = qualGroup,
                        Source = model,
                        ObjectName = "QualGroups",
                        Transtype = TransType.Insert,
                        Options = moreInfo
                    });
                    qualGroup.Code = MaxCode++;
                    result.Add(qualGroup);

                    _hrUnitOfWork.QualificationRepository.Add(qualGroup);
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
                               select new QualGroupsViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   Name = m.Name,
                                   Code = r.Code,
                                   CreatedTime = DateTime.Now,
                                   CreatedUser = UserName,
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateQualGroup(IEnumerable<QualGroupsViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<QualGroupsViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "QualGroups",
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
                var db_QualGroup = _hrUnitOfWork.Repository<QualGroup>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var qualGroup = db_QualGroup.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "QualGroups", Destination = qualGroup, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i), Transtype = TransType.Update });
                    qualGroup.ModifiedTime = DateTime.Now;
                    qualGroup.ModifiedUser = UserName;
                    _hrUnitOfWork.QualificationRepository.Attach(qualGroup);
                    _hrUnitOfWork.QualificationRepository.Entry(qualGroup).State = EntityState.Modified;
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

        public ActionResult DeleteQualGroup(int Id)
        {
            var datasource = new DataSource<QualGroupsViewModel>();
            var Obj = _hrUnitOfWork.Repository<QualGroup>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "QualGroups",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.QualificationRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");

        }
        #region Crud School by Mamdouh
        public ActionResult UpdateSchool(IEnumerable<Model.ViewModel.Personnel.SchoolViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<SchoolViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Schools",
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
                var db_School = _hrUnitOfWork.Repository<School>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var school = db_School.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "Schools", Destination = school, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i), Transtype = TransType.Update });

                    school.ModifiedTime = DateTime.Now;
                    school.ModifiedUser = UserName;

                    _hrUnitOfWork.QualificationRepository.Attach(school);
                    _hrUnitOfWork.QualificationRepository.Entry(school).State = EntityState.Modified;
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
        public ActionResult CreateSchool(IEnumerable<SchoolViewModel> models, OptionsViewModel options)
        {
            var result = new List<School>();
            var datasource = new DataSource<SchoolViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Schools",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (SchoolViewModel model in models)
                {
                    var school = new School();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = school,
                        Source = model,
                        ObjectName = "Schools",
                        Transtype = TransType.Insert,
                        Options = options
                    });
                    result.Add(school);
                    _hrUnitOfWork.QualificationRepository.Add(school);
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from model in models
                               join r in result on model.Name equals r.Name
                               select new SchoolViewModel
                               {
                                   Id = r.Id,
                                   Name = model.Name,
                                   Classification = model.Classification,
                                   SchoolType = model.SchoolType,
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        public ActionResult DeleteSchool(int Id)
        {
            var datasource = new DataSource<SchoolViewModel>();

            var Obj = _hrUnitOfWork.Repository<School>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "Schools",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.QualificationRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        #endregion
        public ActionResult GetQualification(int Id)
        {
            return Json(_hrUnitOfWork.QualificationRepository.GetQualifications(Id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateQualification(IEnumerable<QualificationViewModel> models, OptionsViewModel moreInfo)
        {
            var result = new List<Qualification>();
            var datasource = new DataSource<QualificationViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Qualifications",
                        ParentColumn = "QualGroupId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                var sequence = _hrUnitOfWork.Repository<Qualification>().Select(a => a.Code).DefaultIfEmpty(0).Max();
                var MaxCode = sequence == 0 ? 1 : sequence + 1;
                 
                foreach (QualificationViewModel model in models)
                {

                    var qualGroup = new Qualification();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = qualGroup,
                        Source = model,
                        ObjectName = "Qualifications",
                        Transtype = TransType.Insert,
                        Options = moreInfo
                    });
                    qualGroup.Code = MaxCode++;
                    result.Add(qualGroup);
                    _hrUnitOfWork.QualificationRepository.Add(qualGroup);
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
                               select new QualificationViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   Name = m.Name,
                                   Code = r.Code,
                                   CreatedTime = DateTime.Now,
                                   CreatedUser = UserName,
                                   Rank = m.Rank,
                                   Category = m.Category,
                                   QualGroupId = m.QualGroupId
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateQualification(IEnumerable<QualificationViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<QualificationViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Qualifications",
                        ParentColumn = "QualGroupId",
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
                var db_Qualification = _hrUnitOfWork.Repository<Qualification>().Where(a => ids.Contains(a.Id)).ToList();
                for (int i = 0; i < models.Count(); i++)
                {
                    var qualification = db_Qualification.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);

                    AutoMapper(new AutoMapperParm() { Source = models.ElementAtOrDefault(i), Destination = qualification, Version = 0, ObjectName = "Qualifications", Options = options.ElementAtOrDefault(i), Transtype = TransType.Update });
                    qualification.ModifiedTime = DateTime.Now;
                    qualification.ModifiedUser = UserName;

                    _hrUnitOfWork.QualificationRepository.Attach(qualification);
                    _hrUnitOfWork.QualificationRepository.Entry(qualification).State = EntityState.Modified;
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
        public ActionResult DeleteQualification(int Id)
        {
            var datasource = new DataSource<QualificationViewModel>();

            var Obj = _hrUnitOfWork.Repository<Qualification>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "Qualification",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.QualificationRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
    }
}
