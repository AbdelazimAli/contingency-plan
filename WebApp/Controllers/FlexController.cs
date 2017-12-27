using Interface.Core;
using Model.Domain;
using Model.ViewModel;
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

    public class FlexController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        public string UserName { get; set; }
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
        public FlexController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region FlexColumns
        public ActionResult Index()
        {
            ViewBag.InputTypes = _hrUnitOfWork.PageEditorRepository.GetInputType(Language, "DocInputType");
            ViewBag.Result = _hrUnitOfWork.PageEditorRepository.GetobjectName(CompanyId, Language);
            ViewBag.CodeName = _hrUnitOfWork.LookUpRepository.GetLookUp(Language).Select(s => new { value = s.CodeName, text = s.Title }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        //ReadFlexColumns
        public ActionResult ReadFlexColumns(int pageId, string name)
        {
            return Json(_hrUnitOfWork.PageEditorRepository.GetFlexColumns(pageId, name, Language, CompanyId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateFlexColumns(IEnumerable<FlexColumnsViewModel> models, int pageId, IEnumerable<OptionsViewModel> options)
        {
            var result = new List<FlexColumn>();

            var datasource = new DataSource<FlexColumnsViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
           
            var page = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.Id == pageId).Select(a => new { a.ObjectName, a.Version, a.TableName }).FirstOrDefault();
            var titles = _hrUnitOfWork.Repository<ColumnTitle>().Where(a => a.CompanyId == CompanyId && a.Culture == Language && a.ObjectName == page.ObjectName && a.Version == page.Version).ToList();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FlexColumns",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        ParentColumn = "PageId",
                        Culture = Language,
                        TableName = page.TableName

                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                for (var i = 0; i < models.Count(); i++)
                {
                    {
                        var model = models.ElementAtOrDefault(i);
                        var title = titles.FirstOrDefault(a => a.ColumnName == model.ColumnName);

                        var flexCol = new FlexColumn();
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = flexCol,
                            Source = model,
                            ObjectName = "FlexColumns",
                            Version = Convert.ToByte(Request.Form["Version"]),
                            Transtype = TransType.Insert,
                            Options = options.ElementAtOrDefault(i)
                        });

                        if (title == null)
                        {
                            title = new ColumnTitle
                            {
                                ColumnName = model.ColumnName,
                                Culture = Language,
                                Version = page.Version,
                                ObjectName = page.ObjectName,
                                Title = model.Title,
                                CompanyId = CompanyId
                            };
                            _hrUnitOfWork.PageEditorRepository.Add(title);
                        }
                        flexCol.PageId = pageId;
                        flexCol.TableName = page.TableName;
                        result.Add(flexCol);
                        _hrUnitOfWork.PageEditorRepository.Add(flexCol);
                    }

                    datasource.Errors = SaveChanges(Language);

                }
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

                datasource.Data = (from m in models
                                   join r in result on m.ColumnName equals r.ColumnName
                                   select new FlexColumnsViewModel
                                   {
                                       Id = r.Id,
                                       CodeName = m.CodeName,
                                       ColumnOrder = m.ColumnOrder,
                                       isVisible = m.isVisible,
                                       Max = m.Max,
                                       Min = m.Min,
                                       InputType = m.InputType,
                                       Pattern = m.Pattern,
                                       PlaceHolder = m.PlaceHolder,
                                       Title = m.Title,
                                       Required = m.Required,
                                       UniqueColumns = m.UniqueColumns,
                                       IsUnique = m.IsUnique,
                                       ColumnName = m.ColumnName,
                                       PageId = pageId,
                                       TableName = page.TableName
                                   }).ToList();

                if (datasource.Errors.Count() > 0)
                    return Json(datasource);
                else
                    return Json(datasource.Data);
            }

        public ActionResult UpdateFlexColumns(IEnumerable<FlexColumnsViewModel> models, int pageId, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<FlexColumnsViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
           
            var page = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.Id == pageId).Select(a => new { a.ObjectName, a.Version, a.TableName }).FirstOrDefault();
            var titles = _hrUnitOfWork.Repository<ColumnTitle>().Where(a => a.CompanyId == CompanyId && a.Culture == Language && a.ObjectName == page.ObjectName && a.Version == page.Version).ToList();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FlexColumns",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        ParentColumn = "PageId",
                        Culture = Language,
                        TableName = page.TableName
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Id);
                var db_flexcolumns = _hrUnitOfWork.Repository<FlexColumn>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var model = models.ElementAtOrDefault(i);
                    var title = titles.FirstOrDefault(a => a.ColumnName == model.ColumnName);
                    var flexCol = db_flexcolumns.FirstOrDefault(a => a.Id == model.Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "FlexColumns", Destination = flexCol, Source = model, Version = 0, Options = options.ElementAtOrDefault(i), Transtype = TransType.Update });
                    flexCol.TableName = page.TableName;
                    if (title != null)
                    {
                        title.ColumnName = model.ColumnName;
                        title.Title = model.Title;
                        title.ObjectName = page.ObjectName;
                        title.Culture = Language;
                        title.Version = page.Version;
                        title.CompanyId = CompanyId;
                        _hrUnitOfWork.PageEditorRepository.Attach(title);
                        _hrUnitOfWork.PageEditorRepository.Entry(title).State = EntityState.Modified;

                    }
                    else
                    {
                        title = new ColumnTitle
                        {
                            ColumnName = model.ColumnName,
                            Culture = Language,
                            Version = page.Version,
                            ObjectName = page.ObjectName,
                            Title = model.Title,
                            CompanyId = CompanyId
                        };
                        _hrUnitOfWork.PageEditorRepository.Add(title);
                    }

                    //_hrUnitOfWork.PageEditorRepository.Attach(flexCol);
                    //_hrUnitOfWork.PageEditorRepository.Entry(flexCol).State = EntityState.Modified;
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

        public ActionResult DeleteFlexColumns(int Id)
        {
            var datasource = new DataSource<FlexColumnsViewModel>();
            var obj = _hrUnitOfWork.Repository<FlexColumn>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = obj,
                ObjectName = "FlexColumns",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });
            string msg = _hrUnitOfWork.PageEditorRepository.DeleteFlexColumns(obj, Language);
            if (msg != "OK")
            {
                datasource.Errors = new List<Error>() { new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = msg } } } };
                return Json(datasource);
            }

            datasource.Errors = SaveChanges(Language);

            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        #endregion


        #region Form FlexData

        public ActionResult GetFormFlexData(string objectName, int sourceId = 0, byte version = 0)
        {
            FormFlexColumnsVM fs = _hrUnitOfWork.PagesRepository.GetFormFlexData(CompanyId, objectName, version, Language, sourceId);
            fs.Codes = _hrUnitOfWork.LookUpRepository.GetFlexLookUpCodesLists(CompanyId, objectName, version, Language).Select(c => new FormLookUpCodeVM {
                CodeName = c.CodeName,
                id = c.CodeId,
                name = c.Name,
                isUserCode = false
            });

            return Json(fs, JsonRequestBehavior.AllowGet);
        }

        //Flex Data
        public ActionResult FlexData(int id, string objectName)
        {
            ViewBag.objectName = objectName;
            return View(id);
        }

        public ActionResult GetLookUpCodesLists(string objectName)
        {
            byte version;
            byte.TryParse(Request.QueryString["version"], out version);

            return Json(_hrUnitOfWork.LookUpRepository.GetFlexLookUpCodesLists(CompanyId, objectName, version, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadFlexData(int id, string objectName)
        {
            byte version;
            byte.TryParse(Request.QueryString["version"], out version);

            return Json(_hrUnitOfWork.PageEditorRepository.GetFlexData(CompanyId, objectName, version, Language, id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateFlexData(IEnumerable<FlexDataViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<FlexDataViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            var firstModel = models.FirstOrDefault();
            List<FlexData> result = new List<FlexData>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FlexData",
                        TableName = "FlexData",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        ParentColumn = "PageId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var oldFlexData = _hrUnitOfWork.PageEditorRepository.GetSourceFlexData(firstModel.PageId, firstModel.SourceId);
                foreach (FlexDataViewModel model in models)
                {
                    //--flexData
                    var flexData = oldFlexData.FirstOrDefault(fd => fd.Id == model.Id);
                    //Don't save in flexData table if not exists and value == null 
                    if (flexData == null && model.Value != null) //Add  
                    {
                        flexData = new FlexData()
                        {
                            PageId = model.PageId,
                            SourceId = model.SourceId,
                            TableName = model.TableName,
                            ColumnName = model.ColumnName, //title
                            Value = getValue(model),
                            ValueId = model.ValueId
                        };

                        result.Add(flexData);
                        _hrUnitOfWork.PageEditorRepository.Add(flexData);
                    }
                    else if (flexData != null) //Update
                    {
                        flexData.Value = getValue(model);
                        flexData.ValueId = model.ValueId;

                        _hrUnitOfWork.PageEditorRepository.Attach(flexData);
                        _hrUnitOfWork.PageEditorRepository.Entry(flexData).State = EntityState.Modified;
                    }
                }
                datasource.Errors = SaveChanges(Language);
            }
            else
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);

            datasource.Data = (from m in models
                               join r in result on m.ColumnName equals r.ColumnName into g
                               from r in g.DefaultIfEmpty()
                               select new FlexDataViewModel
                               {
                                   ColumnName = m.ColumnName,
                                   Title = m.Title,
                                   Id = (r == null ? m.Id : r.Id),
                                   PageId = m.PageId,
                                   TableName = m.TableName,
                                   SourceId = m.SourceId,
                                   Value = m.Value,
                                   ValueId = m.ValueId,
                                   CodeName = m.CodeName,
                                   InputType = m.InputType,
                                   IsUnique = m.IsUnique,
                                   Max = m.Max,
                                   Min = m.Min,
                                   Pattern = m.Pattern,
                                   PlaceHolder = m.PlaceHolder,
                                   Required = m.Required,
                                   UniqueColumns = m.UniqueColumns,
                                   ValueText = m.ValueText,
                                   ObjectName = m.ObjectName,
                                   Version = m.Version
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        private string getValue(FlexDataViewModel model)
        {
            string value;
            switch (model.InputType)
            {
                case 4:  //date
                    value = DateTime.Parse(model.ValueText).ToString("yyyy/MM/dd");
                    break;
                case 5:  //time
                    value = model.ValueText;
                    break;
                case 6:  //datetime
                    value = DateTime.Parse(model.ValueText).ToString("yyyy/MM/dd hh:mm");
                    break;
                default:
                    value = model.Value;
                    break;
            }
            return value;
        }
        #endregion


        #region FlexForm

        public ActionResult FlexFormIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        public ActionResult ReadFlexForms(int FormType, int MenuId)
        {
            var query = _hrUnitOfWork.PagesRepository.ReadFlexForms(FormType, Language);

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

        public ActionResult FlexFormDetails(int id = 0, byte version = 0)
        {
            List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("LeaveRequest", CompanyId, version);
            if (columns.Where(fc => fc == "DesignedBy").FirstOrDefault() == null)
                ViewBag.Employees = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language);

            if (id == 0)
                return View(new FlexFormViewModel());

            FlexFormViewModel FlexForm = _hrUnitOfWork.PagesRepository.GetFlexForm(id, Language);
            return FlexForm == null ? (ActionResult)HttpNotFound() : View(FlexForm);
        }

        [HttpPost]
        public ActionResult FlexFormDetails(FlexFormViewModel model)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FlexForm",
                        TableName = "FlexForms",
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
            }
            else
                return Json(Models.Utils.ParseFormErrors(ModelState));


            FlexForm flexForm = _hrUnitOfWork.Repository<FlexForm>().Where(f => f.Id == model.Id).FirstOrDefault();

            if(model.Id == 0) //Add
            {
                flexForm = new FlexForm();
                AutoMapper(new AutoMapperParm { ObjectName = "FlexForm", Source = model, Destination = flexForm, Version = Convert.ToByte(Request.Form["version"]) , Transtype = TransType.Insert });
                flexForm.Name = model.FormName;
                flexForm.CreatedUser = User.Identity.Name;
                flexForm.CreatedTime = DateTime.Now;

                _hrUnitOfWork.PagesRepository.Add(flexForm);
            }
            else //Update
            {
                AutoMapper(new AutoMapperParm { ObjectName = "FlexForm", Source = model, Destination = flexForm, Version = Convert.ToByte(Request.Form["version"]), Transtype = TransType.Update });
                flexForm.Name = model.FormName;
                flexForm.ModifiedUser = User.Identity.Name;
                flexForm.ModifiedTime = DateTime.Now;

                _hrUnitOfWork.PagesRepository.Attach(flexForm);
                _hrUnitOfWork.PagesRepository.Entry(flexForm).State = EntityState.Modified;
            }

            //FS
            List<FlexFormFS> fieldSets = _hrUnitOfWork.Repository<FlexFormFS>().Where(f => f.FlexformId == model.Id).ToList();
            List<int> fsIds = fieldSets.Select(f => f.Id).ToList();
            List<FlexFormColumn> columns = _hrUnitOfWork.Repository<FlexFormColumn>().Where(f => fsIds.Contains(f.FlexFSId)).ToList();

            //FieldSets
            foreach (var fs in model.FieldSets)
            {
                FlexFormFS flexSet = fieldSets.Where(f => f.Id == fs.Id).FirstOrDefault();
                if (fs.Id == 0) //Add
                {
                    flexSet = new FlexFormFS();
                    AutoMapper(new AutoMapperParm { Source = fs, Destination = flexSet });
                    flexSet.FSOrder = fs.order;
                    flexSet.CreatedUser = User.Identity.Name;
                    flexSet.CreatedTime = DateTime.Now;
                    flexSet.Flexform = flexForm;

                    _hrUnitOfWork.PagesRepository.Add(flexSet);
                }
                else //Update
                {
                    fs.FlexformId = model.Id;

                    AutoMapper(new AutoMapperParm { Source = fs, Destination = flexSet });
                    flexSet.FSOrder = fs.order;
                    flexSet.ModifiedUser = User.Identity.Name;
                    flexSet.ModifiedTime = DateTime.Now;

                    _hrUnitOfWork.PagesRepository.Attach(flexSet);
                    _hrUnitOfWork.PagesRepository.Entry(flexSet).State = EntityState.Modified;
                }

                //Columns
                foreach (var fc in fs.Columns)
                {
                    FlexFormColumn flexCol = columns.Where(f => f.Id == fc.Id).FirstOrDefault();
                    if (fc.Id == 0) //Add
                    {
                        flexCol = new FlexFormColumn();
                        AutoMapper(new AutoMapperParm { Source = fc, Destination = flexCol });
                        flexCol.CreatedUser = User.Identity.Name;
                        flexCol.CreatedTime = DateTime.Now;
                        flexCol.FlexformFS = flexSet;

                        _hrUnitOfWork.PagesRepository.Add(flexCol);
                    }
                    else //Update
                    {
                        fc.FlexFSId = fs.Id;
                        AutoMapper(new AutoMapperParm { Source = fc, Destination = flexCol });
                        flexCol.ModifiedUser = User.Identity.Name;
                        flexCol.ModifiedTime = DateTime.Now;

                        _hrUnitOfWork.PagesRepository.Attach(flexCol);
                        _hrUnitOfWork.PagesRepository.Entry(flexCol).State = EntityState.Modified;
                    }
                }
            }

            errors = SaveChanges(Language);

            var message = "OK";
            if (errors.Count > 0)
                message = errors.First().errors.First().message;
            else
                message += "," + ((new JavaScriptSerializer()).Serialize(flexForm));

            return Json(message);
        }

        public ActionResult DeleteFlexForm(int id)
        {

            List<Error> errors = new List<Error>();

            string message = "OK";

            FlexForm flexForm = _hrUnitOfWork.Repository<FlexForm>().Where(a => a.Id == id).FirstOrDefault();
            AutoMapper(new Models.AutoMapperParm
            {
                Source = flexForm,
                ObjectName = "FlexForm",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });

            _hrUnitOfWork.PagesRepository.Remove(flexForm);

            errors = SaveChanges(Language);
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);

        }
        #endregion
    }
}
