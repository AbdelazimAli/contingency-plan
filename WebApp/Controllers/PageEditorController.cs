using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Extensions;

namespace WebApp.Controllers
{
    public class PageEditorController : BaseController
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
        public PageEditorController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        // GET: PageEdit
        public ActionResult Index()
        {
            ViewBag.MenuName = _hrUnitOfWork.Repository<Menu>().Where(a => a.CompanyId == CompanyId && a.NodeType>0).Select(s => new { value = s.Id, text = s.Name }).ToList();
            
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GridIndex()
        {
            ViewBag.MenuName = _hrUnitOfWork.Repository<Menu>().Where(a => a.CompanyId == CompanyId && a.NodeType> 0).Select(s => new { text = s.Name, value = s.Id }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = int.Parse(Request.QueryString["MenuId"]?.ToString());
            ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadPage()
        {
            return Json(_hrUnitOfWork.PageEditorRepository.ReadPage(CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadGrid()
        {

            return Json(_hrUnitOfWork.PageEditorRepository.ReadGrid(CompanyId, Language), JsonRequestBehavior.AllowGet);
        }     
        //Inialize Field set 
        public ActionResult ReadFieldSet(int Id)
        {
            return Json(_hrUnitOfWork.PageEditorRepository.GetFieldSet(Id), JsonRequestBehavior.AllowGet);
        }
        //ajax call to get field ids
        public ActionResult ReadFieldSets(int FieldSetId)
        {
            return Json(_hrUnitOfWork.PageEditorRepository.GetFieldSets(FieldSetId), JsonRequestBehavior.AllowGet);
        }
        // ajax intialize to get section ids
        public ActionResult ReadSections(int SectionId)
        {
            return Json(_hrUnitOfWork.PageEditorRepository.Getsections(SectionId), JsonRequestBehavior.AllowGet);
        }
        //intialize section
        public ActionResult ReadSection(int Id)
        {
            return Json(_hrUnitOfWork.PageEditorRepository.GetSection(Id), JsonRequestBehavior.AllowGet);
        }
        //Intialize form column
        public ActionResult ReadFormColumns(int Id)
        {
            return Json(_hrUnitOfWork.PageEditorRepository.GetFormColumns(Id), JsonRequestBehavior.AllowGet);
        }
        //Read Grid Colums 
        public ActionResult ReadColumnInfo(int GridId)
        {
            return Json(_hrUnitOfWork.PageEditorRepository.GetColumnInfo(GridId), JsonRequestBehavior.AllowGet);
        }
        //Kendo :read RoleGrid columns permissions
        public ActionResult ReadRoleGridColumns(string RoleId, string objectName, byte version)
        {
            return Json(_hrUnitOfWork.PageEditorRepository.GetRoleGridColumns(RoleId, objectName, version, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        //Kend:update ==>Role Grid colums by Mamdouh
        public ActionResult UpdateRoleGridColumns(IEnumerable<RoleGridColumnsViewModel> models, string objectName, byte version)
        {
            var datasource = new DataSource<RoleGridColumnsViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                _hrUnitOfWork.PageEditorRepository.UpdateRoleGridColumns(models, objectName, version, CompanyId);
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
        [HttpPost]
        public ActionResult UpdatePage(IEnumerable<PageDivViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<PageDivViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = models.ElementAtOrDefault(0).CompanyId,
                        ObjectName = "PageDivs",
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

                var ids = models.Select(a => a.Id);
                var db_Pages = _hrUnitOfWork.Repository<PageDiv>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var page = db_Pages.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new Models.AutoMapperParm() { ObjectName = "PageDivs",
                        Destination = page,
                        Source = models.ElementAtOrDefault(i),
                        Version = 0,
                        Options = options.ElementAtOrDefault(i),
                        Transtype =TransType.Update
                    });
                    _hrUnitOfWork.PageEditorRepository.Attach(page);
                    _hrUnitOfWork.PageEditorRepository.Entry(page).State = EntityState.Modified;
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
            {

                for (int i = 0; i < models.Count(); i++)
                {
                    var x = models.ElementAtOrDefault(i);
                    string key = x.CompanyId + x.ObjectName + x.Version + Language;

                    if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                        _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);
                }

                return Json(datasource.Data);
            }
        }
        public ActionResult CreatePage(IEnumerable<PageDivViewModel> models)
        {
            var result = new List<PageDiv>();
           
            var datasource = new DataSource<PageDivViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PageDivs",
                        TableName= "PageDivs",
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
               
                foreach (PageDivViewModel p in models)
                {
                    var page = new PageDiv
                    {
                        ObjectName = p.ObjectName,
                        TableName = p.TableName,
                        DivType = p.DivType,
                        Title = p.Title,
                        MenuId = p.MenuId,
                        CompanyId = CompanyId

                    };
                  
                    result.Add(page);
                    _hrUnitOfWork.PageEditorRepository.Add(page);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on new { p.CompanyId, p.ObjectName } equals new { r.CompanyId, r.ObjectName }
                               select new PageDivViewModel
                               {
                                   Id = r.Id,
                                   TableName=p.TableName,                                  
                                   ObjectName = p.ObjectName,
                                   DivType = p.DivType,
                                   Title = p.Title,
                                   MenuId = p.MenuId,
                                   CompanyId = CompanyId
                               }).ToList();
         
            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult CreateForm(IEnumerable<PageDivViewModel> models)
        {
            var result = new List<PageDiv>();

            var datasource = new DataSource<PageDivViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PageDivs",
                        TableName = "PageDivs",
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

                foreach (PageDivViewModel p in models)
                {
                    var page = new PageDiv
                    {

                        ObjectName = p.ObjectName,
                        TableName = p.TableName,
                        DivType = "Form",
                        Title = p.Title,
                        MenuId = p.MenuId,
                        CompanyId = CompanyId
                    };

                    result.Add(page);
                    _hrUnitOfWork.PageEditorRepository.AddFieldSetColumns(page);
                }

                datasource.Errors = SaveChanges(Language);
                

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }
           
            datasource.Data = (from p in models
                               join r in result on new { p.CompanyId, p.ObjectName } equals new { r.CompanyId, r.ObjectName }
                               select new PageDivViewModel
                               {
                                   Id = r.Id,
                                   TableName = p.TableName,
                                   ObjectName = p.ObjectName,
                                   DivType = "Form",
                                   Title = p.Title,
                                   MenuId = p.MenuId,
                                   CompanyId = CompanyId
                               }).ToList();         
           var Id = datasource.Data.Select(i => i.Id).ToList();
           var tblName = datasource.Data.Select(i => i.TableName).ToList();
            _hrUnitOfWork.PageEditorRepository.AddSectionColumns(Id);
            SaveChanges(Language);
            for (int i = 0; i < Id.Count(); i++)
            {
                _hrUnitOfWork.PageEditorRepository.AddFormColumns(Id[i], tblName[i]);
            }
           // _hrUnitOfWork.PageEditorRepository.AddFormColumns(Id,tblName);
            SaveChanges(Language);


            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult DeletePage(IEnumerable<PageDivViewModel> models)
        {
            var datasource = new DataSource<PageDivViewModel>();
            List<string> keys = new List<string>();
            for (int i = 0; i < models.Count(); i++)
            {
                var x = models.ElementAtOrDefault(i);
                keys.Add(x.CompanyId + x.ObjectName + x.Version + Language);
            }
          

            if (ModelState.IsValid)
            {
                foreach (PageDivViewModel model in models)
                {
                    var page = new PageDiv
                    {
                        Id = model.Id
                    };

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Source = page,
                        ObjectName = "PageDivs",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Transtype = TransType.Delete
                    });


                    _hrUnitOfWork.PageEditorRepository.Remove(page);

                    foreach (ColumnTitle title in _hrUnitOfWork.Repository<ColumnTitle>().Where(t => t.CompanyId == model.CompanyId && t.ObjectName == model.ObjectName && t.Version == model.Version).ToList())
                    {
                        _hrUnitOfWork.PageEditorRepository.Remove(title);
                    }
                    foreach (RoleColumns role in _hrUnitOfWork.Repository<RoleColumns>().Where(r => r.CompanyId == model.CompanyId && r.ObjectName == model.ObjectName && r.Version == model.Version).ToList())
                    {
                        _hrUnitOfWork.PageEditorRepository.Remove(role);
                    }
                }

                datasource.Errors = SaveChanges(Language);
                datasource.Total = models.Count();
            }

            datasource.Data = models;

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(keys[i]))
                        _hrUnitOfWork.PagesRepository.CacheManager.Remove(keys[i]);
                }
                return Json(datasource.Data);
            }
        }
        public ActionResult CreateFieldSet(IEnumerable<FieldSetViewModel> models)
        {
            var result = new List<FieldSet>();

            var datasource = new DataSource<FieldSetViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FieldSets",
                        TableName = "FieldSets",
                        ParentColumn = "PageId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }


                foreach (FieldSetViewModel p in models)
                {
                    var field = new FieldSet
                    {
                       PageId=p.PageId,
                       Collapsable=p.Collapsable,
                       Collapsed=p.Collapsed,
                       Editable=p.LabelEditable,
                       Freeze=p.Freez,
                       LayOut=p.layout,
                       Legend=p.legend,
                       HasTag=p.HasFieldSetTag,
                       Order=p.order,
                       Reorderable=p.Reorderable
                    };

                    result.Add(field);
                    _hrUnitOfWork.PageEditorRepository.Add(field);
                    
                }
              

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from f in models
                               join r in result on f.PageId equals r.PageId
                               select new FieldSetViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   Collapsable =f.Collapsable,
                                   Collapsed=f.Collapsed,
                                   Freez=f.Freez,
                                   HasFieldSetTag=f.HasFieldSetTag,
                                   LabelEditable=f.LabelEditable,
                                   layout=f.layout,
                                   legend=f.legend,
                                   order=f.order,
                                   Reorderable=f.Reorderable,                                                    
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        //delete GridColumn
        public ActionResult DeleteGridColumn(int Id)
        {
            List<Error> errors = new List<Error>();
            DataSource<ColumnInfoViewModel> Source = new DataSource<ColumnInfoViewModel>();
            var model = _hrUnitOfWork.PageEditorRepository.GetGridColumn(Id);
            string keys = model.CompanyId + model.ObjectName + model.Version + Language;

            if (model != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = model,
                    ObjectName = "GridColumns",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.PageEditorRepository.Remove(model.Column);
            }
            string message = "OK";

            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else
            {
                if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(keys))
                    _hrUnitOfWork.PagesRepository.CacheManager.Remove(keys);
                return Json(message);
            }
        }

        //Create Grid Columns
        public ActionResult CreateGridColumn(IEnumerable<ColumnInfoViewModel> models)
        {
            var result = new List<GridColumn>();

            var datasource = new DataSource<ColumnInfoViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "GridColumns",
                        TableName = "GridColumns",
                        ParentColumn = "GridId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (ColumnInfoViewModel column in models)
                {
                    var record = new GridColumn
                    {
                        GridId = column.GridId,
                        ColumnName = column.ColumnName,
                        ColumnOrder = column.ColumnOrder,
                        isVisible = column.isVisible,
                        DefaultWidth = column.DefaultWidth,
                        ColumnType = column.ColumnType,
                        Required = column.Required,
                        Min = column.Min,
                        Max = column.Max,
                        Pattern = column.Pattern,
                        MaxLength = column.MaxLength,
                        MinLength = column.MinLength,
                        PlaceHolder = column.PlaceHolder,
                        Custom = column.Custom,
                        InputType = column.InputType,
                        IsUnique = column.IsUnique,
                        UniqueColumns = column.UniqueColumns,
                        OrgInputType = column.OrgInputType                        
                        
                    };

                    result.Add(record);
                    _hrUnitOfWork.PageEditorRepository.Add(record);

                }


                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from column in models
                               join r in result on column.GridId equals r.Id
                               select new ColumnInfoViewModel
                               {
                                   Id = r.Id,
                                   GridId = column.GridId,
                                   ColumnName = column.ColumnName,
                                   ColumnOrder = column.ColumnOrder,
                                   isVisible = column.isVisible,
                                   DefaultWidth = column.DefaultWidth,
                                   ColumnType = column.ColumnType,
                                   Required = column.Required,
                                   Min = column.Min,
                                   Max = column.Max,
                                   Pattern = column.Pattern,
                                   MaxLength = column.MaxLength,
                                   MinLength = column.MinLength,
                                   PlaceHolder = column.PlaceHolder,
                                   Custom = column.Custom,
                                   InputType = column.InputType,
                                   IsUnique = column.IsUnique,
                                   UniqueColumns = column.UniqueColumns,
                                   OrgInputType = column.OrgInputType,                                   

                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }   
        public ActionResult CreateSection(IEnumerable<SectionViewModel> models)
        {
            var result = new List<Section>();

            var datasource = new DataSource<SectionViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Sections",
                        TableName = "Sections",
                        ParentColumn = "FieldSetId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }


                foreach (SectionViewModel s in models)
                {
                    var section = new Section
                    {
                       
                        FieldSetId = s.FieldSetId,
                        Freeze = s.Freez,
                        LabelLg = s.labellg,
                        LabelSm = s.labelsm,
                        LayOut = s.layout,
                        FieldsNumber = s.fieldsNumber,
                        Name = s.name,
                        Order =(byte) s.order,
                        LabelMd=s.labelmd,
                        Reorderable=s.Reorderable
                                           
                    };

                    result.Add(section);
                    _hrUnitOfWork.PageEditorRepository.Add(section);

                }


                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from s in models
                               join r in result on s.FieldSetId equals r.FieldSetId
                               select new SectionViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   FieldSetId =s.FieldSetId,
                                  fieldsNumber=s.fieldsNumber,
                                  Freez=s.Freez,
                                  labellg=s.labellg,
                                  layout=s.layout,
                                  name=s.name,
                                  labelmd=s.labelmd,
                                  labelsm=s.labelsm,
                                  order=s.order,
                                  Reorderable=s.Reorderable                                

                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult CreateFormColumn(IEnumerable<FormColumnViewModel> models)
        {
            var result = new List<FormColumn>();

            var datasource = new DataSource<FormColumnViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FormColumns",
                        TableName = "FormColumns",
                        ParentColumn = "SectionId",

                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }


                foreach (FormColumnViewModel frm in models)
                {
                    var formcolumn = new FormColumn
                    {
                        
                        SectionId=frm.SectionId,
                        ColumnName = frm.name,
                        ColumnType = frm.ColumnType,
                        HtmlAttribute = frm.HtmlAttribute,
                        IsUnique = frm.isunique,
                        Lg = (frm.lg !=null ? frm.lg :null),
                        Max = frm.max,
                        Min = frm.min,
                        Md = frm.md,
                        isVisible = frm.isVisible,
                        ColumnOrder = frm.order,
                        MaxLength  = frm.maxLength,
                        MinLength = frm.minLength,
                        Pattern = frm.pattern,
                        PlaceHolder = frm.placeholder,
                        Required = frm.required,
                        Sm = frm.sm,
                        UniqueColumns = frm.UniqueColumns,
                        OrgInputType =frm.OrgInputType,
                        InputType =frm.type ,
                        CodeName = frm.CodeName,
                        DefaultValue=frm.DefaultValue, 
                        Formula = frm.Formula
                                                                 
                    };

                    result.Add(formcolumn);
                    _hrUnitOfWork.PageEditorRepository.Add(formcolumn);

                }


                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from frm in models
                               join r in result on frm.name equals r.ColumnName
                               select new FormColumnViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   SectionId =frm.SectionId,
                                   name = frm.name,
                                   type = frm.ColumnType,
                                   isunique = frm.isunique,
                                   lg = frm.lg,
                                   max = frm.max,
                                   min = frm.min,
                                   md = frm.md,
                                   order = frm.order,
                                   maxLength = frm.maxLength,
                                   minLength = frm.minLength,
                                   pattern = frm.pattern,
                                   placeholder = frm.placeholder,
                                   required = frm.required,
                                   sm = frm.sm,
                                   ColumnType = frm.type,
                                   isVisible=frm.isVisible,
                                   label=frm.label,
                                   UniqueColumns=frm.UniqueColumns,
                                   OrgInputType=frm.OrgInputType,
                                   HtmlAttribute=frm.HtmlAttribute,
                                   Formula = frm.Formula

                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult DeleteFieldSet(int id)
        {
            var datasource = new DataSource<FieldSetViewModel>();

            //var pageDiv = _hrUnitOfWork.PageEditorRepository.GetPageObject(models.FirstOrDefault().PageId, "FieldSet");
            //string key = string.Format("{0}{1}{2}", pageDiv.Id, pageDiv.CompanyId, Language);

            if (ModelState.IsValid)
            {
                
                    var field = new FieldSet
                    {
                        Id = id
                    };
                    _hrUnitOfWork.PageEditorRepository.Remove(field);
              
               
                datasource.Errors = SaveChanges(Language);
              
            }       

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json("OK");

        }
        public ActionResult DeleteSection(int id)
        {
            var datasource = new DataSource<SectionViewModel>();
            if (ModelState.IsValid)
            {
                var section = new Section
                {
                    Id = id
                };
                    _hrUnitOfWork.PageEditorRepository.Remove(section);

                datasource.Errors = SaveChanges(Language);
            }
            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        public ActionResult DeleteFormColumn(int id)
        {
            var datasource = new DataSource<FormColumnViewModel>();

            if (ModelState.IsValid)
            {

                var formcolumn = new FormColumn
                {
                    Id = id
                };
                _hrUnitOfWork.PageEditorRepository.Remove(formcolumn);


                datasource.Errors = SaveChanges(Language);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        public ActionResult UpdateFieldSet(IEnumerable<FieldSetViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<FieldSetViewModel>();
           
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FieldSets",
                        TableName = "FieldSets",
                        ParentColumn = "PageId",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }


                foreach (FieldSetViewModel p in models)
                {
                    var field = new FieldSet
                    {
                        Id = p.Id,
                        PageId = p.PageId,
                        Collapsable = p.Collapsable,
                        Freeze = p.Freez,
                        Editable = p.LabelEditable,
                        LayOut = p.layout,
                        HasTag = p.HasFieldSetTag,
                        Legend = p.legend,
                        Order = p.order,
                        Collapsed = p.Collapsed,
                        Reorderable = p.Reorderable,

                    };

                    _hrUnitOfWork.PageEditorRepository.Attach(field);
                    _hrUnitOfWork.PageEditorRepository.Entry(field).State = EntityState.Modified;
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
            {
                //  var pageDiv = _hrUnitOfWork.PageEditorRepository.GetPageObject(models.FirstOrDefault().PageId, "FieldSet");
                var page = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.Id == models.FirstOrDefault().PageId).Select(a => new { companyid = a.CompanyId, objectname = a.ObjectName, version = a.Version }).FirstOrDefault();
                string key = page.companyid + page.objectname + page.version + Language;
                if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                    _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

                return Json(datasource.Data);
            }
        }
        public ActionResult UpdateSection(IEnumerable<SectionViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<SectionViewModel>();
            
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Sections",
                        TableName = "Sections",
                        ParentColumn = "FieldSetId",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }


                foreach (SectionViewModel p in models)
                {
                    var section = new Section
                    {
                        Id = p.Id,
                        FieldSetId = p.TempId,
                        Freeze = p.Freez,
                        Order = (byte)p.order,
                        Name = p.name,
                        FieldsNumber = p.fieldsNumber,
                        LabelLg = p.labellg,
                        LabelMd = p.labelmd,
                        LabelSm = p.labelsm,
                        LayOut = p.layout,
                        Reorderable = p.Reorderable
                    };

                    _hrUnitOfWork.PageEditorRepository.Attach(section);
                    _hrUnitOfWork.PageEditorRepository.Entry(section).State = EntityState.Modified;
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
            {
                var page = _hrUnitOfWork.Repository<FieldSet>().Where(a => a.Id == models.FirstOrDefault().FieldSetId).Select(a => new { companyid = a.Page.CompanyId, objectname = a.Page.ObjectName, version = a.Page.Version }).FirstOrDefault();
                string key = page.companyid + page.objectname + page.version + Language;
                if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                    _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

                return Json(datasource.Data);
            }
        }
        public ActionResult UpdateFormColumn(IEnumerable<FormColumnViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<FormColumnViewModel>();
            
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FormColumns",
                        TableName = "FormColumns",
                        ParentColumn = "SectionId",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }


                foreach (FormColumnViewModel col in models)
                {
                    var formcolumn = new FormColumn
                    {
                        Id = col.Id,
                        SectionId = col.TempId,
                        ColumnName = col.name,
                        ColumnOrder = col.order,
                        ColumnType = col.ColumnType,
                        InputType = col.type,
                        IsUnique = col.isunique,
                        isVisible = col.isVisible,
                        Max = col.max,
                        Min= col.min,
                        Lg = col.lg,
                        MinLength = col.minLength,
                        MaxLength = col.maxLength,
                        Md = col.md,
                        Pattern = col.pattern,
                        PlaceHolder = col.placeholder,
                        OrgInputType = col.OrgInputType,
                        Required = col.required,
                        Sm = col.sm,
                        UniqueColumns = col.UniqueColumns,
                        HtmlAttribute = col.HtmlAttribute,
                        CodeName = col.CodeName,
                        DefaultValue=col.DefaultValue,        
                        Formula = col.Formula               
                    };

                    _hrUnitOfWork.PageEditorRepository.Attach(formcolumn);
                    _hrUnitOfWork.PageEditorRepository.Entry(formcolumn).State = EntityState.Modified;
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
            {
                var page = _hrUnitOfWork.Repository<Section>().Where(a => a.Id == models.FirstOrDefault().SectionId).Select(a => new { companyid = a.FieldSet.Page.CompanyId, objectname = a.FieldSet.Page.ObjectName, version = a.FieldSet.Page.Version }).FirstOrDefault();
                string key = page.companyid + page.objectname + page.version + Language;
                if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                    _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

                return Json(datasource.Data);
            }
        }
        [HttpPost]
        public ActionResult UpdateColumnInfo(IEnumerable<ColumnInfoViewModel> models)
        {
            var datasource = new DataSource<ColumnInfoViewModel>();
           
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                var model = models.OrderBy(m => m.Id).FirstOrDefault(m => m.CompanyId == 0);
                if (model != null && model.CompanyId != CompanyId)
                {
                    model.CompanyId = CompanyId;
                    _hrUnitOfWork.PagesRepository.CopyColumnsInfo(models);
                }
                else
                {
                    foreach (ColumnInfoViewModel column in models)
                    {
                        var record = new GridColumn
                        {
                            Id = column.Id,
                            GridId = column.GridId,
                            ColumnName = column.ColumnName,
                            ColumnOrder = column.ColumnOrder,
                            isVisible = column.isVisible,
                            DefaultWidth = column.DefaultWidth,
                            ColumnType = column.ColumnType,
                            Required = column.Required,
                            Min = column.Min,
                            Max = column.Max,
                            Pattern = column.Pattern,
                            MaxLength = column.MaxLength,
                            MinLength = column.MinLength,
                            PlaceHolder = column.PlaceHolder,
                            Custom = column.Custom,
                            InputType = column.InputType,
                            IsUnique = column.IsUnique,
                            UniqueColumns = column.UniqueColumns,
                            OrgInputType = column.OrgInputType,
                            DefaultValue=column.DefaultValue
                        };

                        _hrUnitOfWork.PagesRepository.Attach(record);
                        _hrUnitOfWork.PagesRepository.Entry(record).State = System.Data.Entity.EntityState.Modified;
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
            {
                var x = models.ElementAtOrDefault(0);
                var page = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.Id == x.GridId).Select(a => new { companyid = a.CompanyId, objectname = a.ObjectName, version = a.Version }).FirstOrDefault();
                string key = page.companyid + page.objectname + page.version + Language;
                if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                    _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

                return Json(datasource.Data);
            }
        }


    }
}