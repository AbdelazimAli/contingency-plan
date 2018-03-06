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
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LookUpCodeController : BaseController
    {
        // GET: LookUpCode
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
        public LookUpCodeController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult JobClass()
        {
            return View();
        }
        public ActionResult CodesIndex()
        {
            return View();
        }
        public ActionResult Lookupcode()
        {
            return View();
        }
        public ActionResult LookupUsercode()
        {
            return View();
        }
        public ActionResult DocumentIndex()
        {
            ViewBag.DocInputType = _hrUnitOfWork.Repository<LookUpCode>().Where(d => d.CodeName == "DocInputType").Select(d => new { text = d.Name, value = d.CodeId });
            return View();
        }
        public ActionResult GetLookUpCode()
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetLookUp(Language), JsonRequestBehavior.AllowGet);
        }
        //Kendo :read Look up User codeName 
        public ActionResult GetLookUpUserCode()
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetLookUpUserCode(Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLookUpCodes(string Id)
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetLookUpCodes(Id, Language), JsonRequestBehavior.AllowGet);
        }
        //Kendo read look up user codes 
        public ActionResult GetLookUpUserCodes(string Id)
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetLookUpUserCodes(Id, Language), JsonRequestBehavior.AllowGet);
        }
        //ajax call to ReadSysCodeId
        public ActionResult ReadSysCodeId(string CodeName)
        {
            var q = _hrUnitOfWork.LookUpRepository.GetsyscodeForm(CodeName, Language).Select(a => new { value = a.id, text = a.name });
            return Json(q, JsonRequestBehavior.AllowGet);
        }
        //ajax call to Read CodeName from look up code 
        public ActionResult ReadCodeName()
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetCodeName(), JsonRequestBehavior.AllowGet);
        }       
        //Kendo read :ReadJobClass
        public JsonResult ReadJobClass(int MenuId)
        {
            var query = _hrUnitOfWork.LookUpRepository.GetJobClasses(CompanyId);
            string whereclause = GetWhereClause(MenuId);
            if (whereclause.Length > 0) query = query.Where(whereclause);
            return Json(query, JsonRequestBehavior.AllowGet);
        }    
        [HttpPost]          
        public ActionResult CreateDocAttr(IEnumerable<DocTypeAttrViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var result = new List<DocTypeAttr>();
            var datasource = new DataSource<DocTypeAttrViewModel>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "DocTypeAttrs",
                        TableName = "DocTypeAttrs",
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
                    var Attr = new DocTypeAttr();
                    AutoMapper(new AutoMapperParm() { ObjectName = "DocTypeAttrs", Destination = Attr, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype=TransType.Insert });
                    result.Add(Attr);
                    _hrUnitOfWork.LookUpRepository.Add(Attr);
                }
                datasource.Errors = SaveChanges(Language);
            }

            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from model in models
                               join r in result on model.Attribute equals r.Attribute
                               select new DocTypeAttrViewModel
                               {
                                   Id = r.Id,
                                   Attribute = model.Attribute,
                                   TypeId = model.TypeId,
                                   CodeName = model.CodeName,
                                   InputType = model.InputType,
                                   //InputTypeText = model.InputTypeText                                                               

                               }).ToList();
                                  

            if (datasource.Errors.Count() > 0)
            {
                datasource.Data = models;
                return Json(datasource);
            }
            else
                return Json(datasource.Data);
        }
        [HttpPost]
        public ActionResult CreateJobClass(IEnumerable<JobClassViewModel> models ,OptionsViewModel moreInfo)
        {
            var result = new List<JobClass>();

            var datasource = new DataSource<JobClassViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "JobClasses",
                        TableName = "JobClasses",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (JobClassViewModel model in models)
                {
                    var job = new JobClass();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = job,
                        Source = model,
                        ObjectName = "JobClasses",
                        Transtype = TransType.Insert,
                        Options = moreInfo
                    });
                    job.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    result.Add(job);
                    _hrUnitOfWork.LookUpRepository.Add(job);
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on p.JobClassCode equals r.JobClassCode
                               select new JobClassViewModel
                               {
                                   Id = r.Id,
                                   IsLocal = p.IsLocal,
                                   Name = p.Name,
                                   CompanyId = p.CompanyId,
                                   Notes = p.Notes,
                                   JobClassCode = p.JobClassCode
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        [HttpPost]
        public ActionResult CreateDocTypes(IEnumerable<DocTypeViewModel> models)
        {
            var result = new List<DocType>();

            var datasource = new DataSource<DocTypeViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "DocTypes",
                        TableName = "DocTypes",
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (DocTypeViewModel model in models)
                {
                    var doc = new DocType();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = doc,
                        Source = model,
                        ObjectName = "DocTypes",
                        Transtype = TransType.Insert
                    });
                    result.Add(doc);
                    _hrUnitOfWork.LookUpRepository.Add(doc);
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from model in models
                               join r in result on model.Name equals r.Name
                               select new DocTypeViewModel
                               {
                                   Id = r.Id,
                                   HasExpiryDate = model.HasExpiryDate,
                                   Name = model.Name,
                                   EndDate = model.EndDate,
                                   StartDate = model.StartDate,

                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult DeleteJobClass(int Id)
        {
            var datasource = new DataSource<JobClassViewModel>();
            var Obj = _hrUnitOfWork.Repository<JobClass>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "JobClasses",
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
        public ActionResult DeleteDocAttr(int Id)
        {
            var datasource = new DataSource<DocTypeAttrViewModel>();
            var obj = _hrUnitOfWork.Repository<DocTypeAttr>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = obj,
                ObjectName = "DocTypes",
                Transtype = TransType.Delete
            });

            _hrUnitOfWork.LookUpRepository.Remove(obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }    
        public ActionResult DeleteDocTypes(int id)
        {
            var message = "OK";
            DataSource<DocTypeViewModel> Source = new DataSource<DocTypeViewModel>();

            DocType DocTypeObj = _hrUnitOfWork.LookUpRepository.GetDocType(id);

            _hrUnitOfWork.LookUpRepository.Remove(DocTypeObj);
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);


        }
        [HttpPost]
        public ActionResult UpdateJobClass(IEnumerable<JobClassViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<JobClassViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "JobClasses",
                        TableName = "JobClasses",
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                var Ids = models.Select(m => m.Id);
                var db = _hrUnitOfWork.Repository<JobClass>().Where(j => Ids.Contains(j.Id));

                for (var i = 0; i < models.Count(); i++)
                {
                    var record = db.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "JobClasses",
                        Destination = record,
                        Source = models.ElementAtOrDefault(i),
                        Version = 0,
                        Options = options.ElementAtOrDefault(i),
                        Transtype =TransType.Update
                    });
                    record.CompanyId = models.ElementAtOrDefault(i).IsLocal ? CompanyId : (int?)null;
                    //if (models.ElementAtOrDefault(i).IsLocal != record.IsLocal)
                    //{
                    //    if (models.ElementAtOrDefault(i).IsLocal)
                    //        record.CompanyId = CompanyId;
                    //    else
                    //        record.CompanyId = null;

                    //    record.IsLocal = models.ElementAtOrDefault(i).IsLocal;
                    //}
                    record.ModifiedTime =DateTime.Now;
                    record.ModifiedUser = UserName;

                    _hrUnitOfWork.LookUpRepository.Attach(record);
                    _hrUnitOfWork.LookUpRepository.Entry(record).State = EntityState.Modified;
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

        #region Look Up Code
        public ActionResult CreateLookUpCode(IEnumerable<LookUpViewModel> models)
        {
            var datasource = new DataSource<LookUpViewModel>();
            datasource.Data = models;
            foreach (var item in models)
            {
                _hrUnitOfWork.LookUpRepository.AddLName(Language,null, item.CodeName, item.Title);

            }

            datasource.Errors = SaveChanges(Language);
            if (datasource.Errors.Count() > 0)
                return Json(datasource);

            return Json(datasource.Data);
        }
        public ActionResult UpdateLookUpCode(IEnumerable<LookUpViewModel> models, IEnumerable<OptionsViewModel> options)

        {
            var datasource = new DataSource<LookUpViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LookUpCode",
                        ParentColumn = "CodeName",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (var item in models)
                {
                    _hrUnitOfWork.LookUpRepository.AddLName(Language,item.CodeName, item.CodeName, item.Title);

                }
              
                _hrUnitOfWork.LookUpRepository.UpdateLookUpCode(models);
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
        public ActionResult DeleteLookUpCode(string Id)
        {
            var datasource = new DataSource<LookUpViewModel>();
            //datasource.Data = models;
            //datasource.Total = models.Count();
            //  string Id = models.FirstOrDefault().Id;
           
            LookUpViewModel Model = _hrUnitOfWork.Repository<LookUpCode>().Where(a => a.CodeName == Id).
                Select(a => new  LookUpViewModel {CodeName=a.CodeName , Id=a.CodeName ,Protected=a.Protected  }).FirstOrDefault();

            if (ModelState.IsValid)
            {
                string result = _hrUnitOfWork.LookUpRepository.DeleteLookUpCode(Model, Language);
                _hrUnitOfWork.LookUpRepository.RemoveLName(Language, Model.CodeName);
                if (result == "OK")
                    datasource.Errors = SaveChanges(Language);
                else
                    datasource.Errors = new List<Error>() { new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = result } } } };
            }
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
            //else
            //{
            //    datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            //}
           
        }
        [HttpPost]
        public ActionResult CreateLookUpCodes(IEnumerable<LookupCodesViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var result = new List<LookUpCode>();
            var resultTitle = new List<LookUpTitles>();
            var datasource = new DataSource<LookupCodesViewModel>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LookupCodes",
                        TableName = "Lookupcode",
                        ParentColumn = "CodeName",
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
                    var code = new LookUpCode();
                    AutoMapper(new AutoMapperParm() { ObjectName = "LookupCodes", Destination = code, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype=TransType.Insert });
                    code.CreatedTime = DateTime.Now;
                    code.CreatedUser = UserName;
                    result.Add(code);
                    _hrUnitOfWork.LookUpRepository.Add(code);

                    var Lookuptitle = new LookUpTitles
                    {

                        Culture = Language,
                        CodeName = models.ElementAtOrDefault(i).CodeName,
                        Name = models.ElementAtOrDefault(i).Name,
                        Title = models.ElementAtOrDefault(i).Title,
                        CodeId = models.ElementAtOrDefault(i).CodeId
                    };
                    resultTitle.Add(Lookuptitle);
                    _hrUnitOfWork.LookUpRepository.Add(Lookuptitle);

                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on p.CodeId equals r.CodeId
                               join t in resultTitle on  p.CodeId equals t.CodeId 
                               select new LookupCodesViewModel
                               {
                                   Id = r.Id,
                                   CodeId = p.CodeId,
                                   Name = p.Name,
                                   Description = p.Description,
                                   EndDate = p.EndDate,
                                   StartDate = p.StartDate,
                                   CodeName = p.CodeName,
                                   Title = t.Title
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateLookUpCodes(IEnumerable<LookupCodesViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<LookupCodesViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LookupCodes",
                        TableName = "Lookupcode",
                        ParentColumn = "CodeName",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                var Ids = models.Select(m => m.Id);
                var CodeNames = models.Select(m => m.CodeName);
                var db = _hrUnitOfWork.Repository<LookUpCode>().Where(j => Ids.Contains(j.Id));
                var db1 = _hrUnitOfWork.Repository<LookUpTitles>().Where(a => CodeNames.Contains(a.CodeName) && a.Culture == Language);
                for (var i = 0; i < models.Count(); i++)
                {
                    var model = models.ElementAtOrDefault(i);
                    var record = db.FirstOrDefault(a => a.Id == model.Id);
                    // Check if Id or Name is changed
                    if (record.CodeId != model.CodeId || record.Name != model.Name)
                    {
                        var code = db.Where(a => a.Id == model.Id).Select(a => a.CodeId).FirstOrDefault();
                        string result = _hrUnitOfWork.LookUpRepository.CheckLookUpCode(model.CodeName, code, Language);
                        if (result != "OK")
                        {
                            datasource.Errors = new List<Error>() { new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("DeleteRelatedRecord").Replace("{0}", result) + ": " + code } } } };
                            return Json(datasource);
                        }
                    }
            
                    var title = db1.FirstOrDefault(a => a.CodeId == model.CodeId && a.Culture== Language);
                    var oldtitle = db1.FirstOrDefault(a => a.CodeId == record.CodeId && a.Culture == Language);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "LookupCodes",
                        Transtype = TransType.Update,
                        Options = options.ElementAtOrDefault(i)
                    });

                    //record.CodeId = model.CodeId;
                    //record.Name = model.Name;
                    //record.StartDate = model.StartDate;
                    //record.EndDate = model.EndDate;
                    record.ModifiedTime = System.DateTime.Now;
                    record.ModifiedUser = UserName;
                    _hrUnitOfWork.LookUpRepository.Attach(record);
                    _hrUnitOfWork.LookUpRepository.Entry(record).State = EntityState.Modified;

                    if (title != null)
                    {
                        if (oldtitle != null)
                            _hrUnitOfWork.LookUpRepository.Remove(oldtitle);
                        title.CodeName = model.CodeName;
                        title.Name = model.Name;
                        title.Title = model.Title;
                        title.CodeId = model.CodeId;
                        _hrUnitOfWork.LookUpRepository.Attach(title);
                        _hrUnitOfWork.LookUpRepository.Entry(title).State = EntityState.Modified;
                       
                    }else
                    {
                        var Lookuptitle = new LookUpTitles
                        {

                            Culture = Language,
                            CodeName = model.CodeName,
                            Name = model.Name,
                            Title = model.Title,
                            CodeId = model.CodeId
                        };                        
                        _hrUnitOfWork.LookUpRepository.Add(Lookuptitle);
                        if(oldtitle != null)
                        _hrUnitOfWork.LookUpRepository.Remove(oldtitle);

                    }
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
        public ActionResult DeleteLookUpCodes(int Id)
        {
            var datasource = new DataSource<LookupCodesViewModel>();
            var obj = _hrUnitOfWork.Repository<LookUpCode>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = obj,
                ObjectName = "LookupCodes",
                Transtype = TransType.Delete
            });

            string result = _hrUnitOfWork.LookUpRepository.CheckLookUpCode(obj.CodeName, obj.CodeId, Language);
            if (result != "OK")
            {
                datasource.Errors = new List<Error>() { new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("DeleteRelatedRecord").Replace("{0}", result) + ": " + _hrUnitOfWork.Repository<LookUpCode>().Where(a => a.Id == obj.Id).Select(a => a.Name).FirstOrDefault() } } } };
                return Json(datasource);
            }

            foreach (LookUpTitles title in _hrUnitOfWork.Repository<LookUpTitles>().Where(t => t.CodeName == obj.CodeName && t.CodeId == obj.CodeId && t.Culture == Language).ToList())
            {
                _hrUnitOfWork.LookUpRepository.Remove(title);
            }
            _hrUnitOfWork.LookUpRepository.Remove(obj);
            _hrUnitOfWork.LookUpRepository.RemoveLName(Language, obj.CodeName);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }

        #endregion

        #region Look Up User Code
        public ActionResult CreateLookUpUserCodes(IEnumerable<LookupUserCodeViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var result = new List<LookUpUserCode>();
            var resultTitle = new List<LookUpTitles>();
            var datasource = new DataSource<LookupUserCodeViewModel>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LookUpUserCodes",
                        TableName = "LookUpUserCodes",
                        ParentColumn = "CodeName",
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
                    var code = new LookUpUserCode();
                    AutoMapper(new AutoMapperParm() { ObjectName = "LookUpUserCodes", Destination = code, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype=TransType.Insert });
                    result.Add(code);
                    _hrUnitOfWork.LookUpRepository.Add(code);

                    var Lookuptitle = new LookUpTitles
                    {
                        Culture = Language,
                        CodeName = models.ElementAtOrDefault(i).CodeName,
                        Name = models.ElementAtOrDefault(i).Name,
                        Title = models.ElementAtOrDefault(i).Title,
                        CodeId = models.ElementAtOrDefault(i).CodeId
                    };
                    resultTitle.Add(Lookuptitle);
                    _hrUnitOfWork.LookUpRepository.Add(Lookuptitle);

                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on p.CodeId equals r.CodeId
                               join t in resultTitle on p.CodeId equals t.CodeId
                               select new LookupUserCodeViewModel
                               {
                                   Id = r.Id,
                                   CodeId = p.CodeId,
                                   Name = p.Name,
                                   Description = p.Description,
                                   EndDate = p.EndDate,
                                   StartDate = p.StartDate,
                                   CodeName = p.CodeName,
                                   SysCodeId = p.SysCodeId,
                                   Title = t.Title
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateLookUpUserCodes(IEnumerable<LookupUserCodeViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<LookupUserCodeViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LookUpUserCodes",
                        TableName = "LookUpUserCodes",
                        ParentColumn = "CodeName",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                var Ids = models.Select(m => m.Id);
                var CodeNames = models.Select(m => m.CodeName);
                var db = _hrUnitOfWork.Repository<LookUpUserCode>().Where(j => Ids.Contains(j.Id));
                var db1 = _hrUnitOfWork.Repository<LookUpTitles>().Where(a => CodeNames.Contains(a.CodeName) && a.Culture == Language);


                for (var i = 0; i < models.Count(); i++)
                {
                    var model = models.ElementAtOrDefault(i);
                    var record = db.FirstOrDefault(a => a.Id == model.Id);
                    var title = db1.FirstOrDefault(a => a.CodeId == model.CodeId);
                    var oldTitle = db1.FirstOrDefault(a => a.CodeId == record.CodeId);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "LookUpUserCodes",
                        Options = options.ElementAtOrDefault(i),
                        Transtype = TransType.Update
                    });
                    //record.CodeId = model.CodeId;
                    //record.Name = model.Name;
                    //record.StartDate = model.StartDate;
                    //record.EndDate = model.EndDate;
                    record.ModifiedTime = System.DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.SysCodeId = model.SysCodeId;
                    _hrUnitOfWork.LookUpRepository.Attach(record);
                    _hrUnitOfWork.LookUpRepository.Entry(record).State = EntityState.Modified;
                    if (title != null)
                    {
                        if (oldTitle != null)
                            _hrUnitOfWork.LookUpRepository.Remove(oldTitle);
                        title.CodeName = model.CodeName;
                        title.Name = model.Name;
                        title.Title = model.Title;
                        title.CodeId = model.CodeId;
                        _hrUnitOfWork.LookUpRepository.Attach(title);
                        _hrUnitOfWork.LookUpRepository.Entry(title).State = EntityState.Modified;
                       
                    }
                    else
                    {
                        var Lookuptitle = new LookUpTitles
                        {

                            Culture = Language,
                            CodeName = model.CodeName,
                            Name = model.Name,
                            Title = model.Title,
                            CodeId = model.CodeId
                        };
                        _hrUnitOfWork.LookUpRepository.Add(Lookuptitle);
                        if (oldTitle != null)
                            _hrUnitOfWork.LookUpRepository.Remove(oldTitle);
                    }

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
        public ActionResult DeleteLookUpUserCodes(int Id)
        {
            var datasource = new DataSource<LookupUserCodeViewModel>();
            var obj = _hrUnitOfWork.Repository<LookUpUserCode>().FirstOrDefault(k => k.Id == Id);
            string culture = Language;

            AutoMapper(new Models.AutoMapperParm
            {
                Source = obj,
                ObjectName = "LookUpUserCodes",
                Transtype = TransType.Delete
            });
            foreach (LookUpTitles title in _hrUnitOfWork.Repository<LookUpTitles>().Where(t => t.CodeName == obj.CodeName && t.CodeId == obj.CodeId && t.Culture == culture).ToList())
            {
                _hrUnitOfWork.LookUpRepository.Remove(title);
            }
            _hrUnitOfWork.LookUpRepository.Remove(obj);
            _hrUnitOfWork.LookUpRepository.RemoveLName(Language, obj.Name);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        #endregion

        #region Documents by Mamadouh
        //DocType Form
        //Kendo read DocType Attr
        public ActionResult ReadDocAttr(int Id)
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetDocAttr(Id,Language), JsonRequestBehavior.AllowGet);
        }
        // Kendo read Doc Types
        public ActionResult GetDocTypes(int MenuId)
        {
            var query = _hrUnitOfWork.LookUpRepository.GetDocTypes(CompanyId, User.Identity.GetCulture());
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
        public ActionResult DocumentDetails(int id = 0)
        {
            byte[] ids = { 1, 2, 4, 5 ,6};
            ViewBag.DocumenType = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("DocType", Language).Select(a => new { id = a.CodeId, name = a.Name }).ToList(); ;
            ViewBag.Jobs = _hrUnitOfWork.JobRepository.GetAllJobs(CompanyId,Language,0).Select(a=> new {id=a.Id,name=a.LocalName});
            ViewBag.InputTypes = _hrUnitOfWork.PageEditorRepository.GetInputType(Language, "DocInputType");
            if (Language.Substring(0, 2) == "ar")
            {
                ViewBag.Nationality = _hrUnitOfWork.Repository<Country>().Where(a => a.Nationality != null).Select(a => new { id = a.Id, name = a.NationalityAr }).ToList();
            }
            else
            {
                ViewBag.Nationality = _hrUnitOfWork.Repository<Country>().Where(a => a.Nationality != null).Select(a => new { id = a.Id, name = a.Nationality }).ToList();
            }
            ViewBag.CodeName = _hrUnitOfWork.LookUpRepository.GetLookUp(Language).Select(s => new { value = s.CodeName, text = s.Title });
            if (id == 0)
                return View(new DocTypeFormViewModel());
            var DocType = _hrUnitOfWork.LookUpRepository.ReadDocType(id, Language);
            return DocType == null ? (ActionResult)HttpNotFound() : View(DocType);
        }       
        //Save Doc Attr
        public ActionResult SaveDocType(DocTypeFormViewModel model, OptionsViewModel moreInfo, RequestDocTypeAttrGrid grid1, bool clear)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "DocType",
                        TableName = "DocTypes",
                        Columns = Models.Utils.GetColumnViews(ModelState).Where(a=>a.Name != "RequiredOpt").ToList(),
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
                if(model.RequiredOpt == 2 && model.IJobs == null)
                {
                    ModelState.AddModelError("", MsgUtils.Instance.Trls("Jobs Required"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));

                }
                if(model.HasExpiryDate && model.NotifyDays == null)
                {
                    ModelState.AddModelError("", MsgUtils.Instance.Trls("Send Notify renew"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
                List<Job> JobList = new List<Job>();
                List<Country> CountryLst = new List<Country>();

                //  var record = _hrUnitOfWork.Repository<DocType>().FirstOrDefault(j => j.Id == model.Id);
                var record = _hrUnitOfWork.LookUpRepository.DocTypeObject(model.Id);
                if (model.RequiredOpt != 2)
                   model.IJobs = null;
                if (model.RequiredOpt == 0 || model.RequiredOpt == null)
                {
                    model.RequiredOpt = 0;
                    model.INationalities = null;
                    model.Gender = null;
                }
                if(model.DocumenType != 2)
                {
                    model.RequiredOpt = 0;
                    model.INationalities = null;
                    model.Gender = null;
                    model.IJobs = null;
                }
                if (model.IJobs != null && model.IJobs.Count() >= 0)
                {

                    foreach (var item in model.IJobs)
                    {
                        Job J = _hrUnitOfWork.Repository<Job>().Where(a => a.Id == item).FirstOrDefault();
                        JobList.Add(J);
                    }
                }
                if (model.INationalities != null && model.INationalities.Count() >= 0)
                {

                    foreach (var item in model.INationalities)
                    {
                        Country J = _hrUnitOfWork.Repository<Country>().Where(a => a.Id == item).FirstOrDefault();
                        CountryLst.Add(J);
                    }
                }
                moreInfo.VisibleColumns.Add("Jobs");
                moreInfo.VisibleColumns.Add("Nationalities");

                if (record == null) //Add
                {
                    record = new DocType();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "DocType",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    if (record.StartDate >= record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("MustGrthanStart"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }                   
                       _hrUnitOfWork.CustodyRepository.AddLName(Language,record.Name, model.Name, model.LocalName);
                    record.Jobs = JobList;
                    record.Nationalities = CountryLst;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    record.RequiredOpt = Convert.ToByte(model.RequiredOpt);
                    _hrUnitOfWork.LookUpRepository.Add(record);

                }
                else //update
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "DocType",
                        Options = moreInfo,
                        Transtype=TransType.Update
                    });
                    //Multi Select Nationality
                    if (record.Nationalities == null)
                        record.Nationalities = CountryLst;
                    else
                    {
                        var National = record.Nationalities.Select(a => a).ToList();
                        foreach (var item in CountryLst)
                        {
                            if (!National.Contains(item))
                                record.Nationalities.Add(item);
                        }
                        foreach (var item in National)
                        {
                            if (!CountryLst.Contains(item))
                                record.Nationalities.Remove(item);
                        }
                    }

                    //Multi Select Jop
                    if (record.Jobs == null)
                        record.Jobs = JobList;
                    else
                    {
                        var Jobobj = record.Jobs.Select(a => a).ToList();
                        foreach (var item in JobList)
                        {
                            if (!Jobobj.Contains(item))
                                record.Jobs.Add(item);
                        }
                        foreach (var item in Jobobj)
                        {
                            if (!JobList.Contains(item))
                                record.Jobs.Remove(item);
                        }
                    }
                    if (record.StartDate >= record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("MustGrthanStart"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }                  
                     _hrUnitOfWork.CustodyRepository.AddLName(Language,record.Name, model.Name, model.LocalName);
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.RequiredOpt =Convert.ToByte(model.RequiredOpt);
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    _hrUnitOfWork.LookUpRepository.Attach(record);
                    _hrUnitOfWork.LookUpRepository.Entry(record).State = EntityState.Modified;

                }

                if (!record.HasExpiryDate)
                    record.NotifyDays = null;

                // Save grid1
                errors = SaveGrid1(grid1, ModelState.Where(a => a.Key.Contains("grid1")), record);
                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);

                model.Id = record.Id;
                string message;
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;
                else
                {
                    if (clear) model = new DocTypeFormViewModel();
                    message = "OK," + ((new JavaScriptSerializer()).Serialize(model));
                }
                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        //Save Grid DocType Attr
        private List<Error> SaveGrid1(RequestDocTypeAttrGrid grid1, IEnumerable<KeyValuePair<string, ModelState>> state, DocType DocTypeobj)
        {
            List<Error> errors = new List<Error>();
            // Deleted
            if (grid1.deleted != null)
            {
                foreach (DocTypeAttrViewModel model in grid1.deleted)
                {
                    var RequestDocTypeAttr = new DocTypeAttr
                    {
                        Id = model.Id
                    };
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Source = RequestDocTypeAttr,
                        ObjectName = "DocTypeAttrs",
                        Transtype = TransType.Delete
                    });
                    _hrUnitOfWork.LookUpRepository.Remove(RequestDocTypeAttr);
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
                        ObjectName = "DocTypeAttrs",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });
                    if (errors.Count() > 0) return errors;
                }
            }

            // updated records
            if (grid1.updated != null)
            {
                foreach (DocTypeAttrViewModel model in grid1.updated)
                {
                    var requestwf = new DocTypeAttr();
                    AutoMapper(new Models.AutoMapperParm { Destination = requestwf, Source = model,Transtype=TransType.Update });
                    _hrUnitOfWork.LookUpRepository.Attach(requestwf);
                    _hrUnitOfWork.LookUpRepository.Entry(requestwf).State = EntityState.Modified;
                }
            }

            // inserted records

            if (grid1.inserted != null)
            {
                foreach (DocTypeAttrViewModel model in grid1.inserted)
                {
                    var docTypeAttr = new DocTypeAttr();
                    AutoMapper(new Models.AutoMapperParm { ObjectName = "DocTypeAttrs", Destination = docTypeAttr, Source = model ,Transtype=TransType.Insert});
                    docTypeAttr.DocType = DocTypeobj;
                    _hrUnitOfWork.LookUpRepository.Add(docTypeAttr);
                }
            }

            return errors;
        }
       
    }
    #endregion
}