using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Extensions;
using System.Web.Script.Serialization;
using System;
using System.Web.Routing;
using Model.ViewModel.Personnel;
using System.Data.Entity;

namespace WebApp.Controllers
{
    public class PagesController : BaseController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;
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
        #region Form
        //Generate Form
        [HttpGet]
        public ActionResult ReadFormInfo(string objectname, byte version, string roleId)
        {
            string key = CompanyId + objectname + version + Language;
            FormViewModel form = new FormViewModel();
            var exist = _hrUnitOfWork.PagesRepository.CacheManager.IsSet(key);
            if (!exist)
            {
                form = _hrUnitOfWork.PagesRepository.GetFormInfo(CompanyId, objectname, version, Language, roleId);
                _hrUnitOfWork.PagesRepository.CacheManager.Set(key, form, 0);
            }
            else
                form = _hrUnitOfWork.PagesRepository.CacheManager.Get<FormViewModel>(key);
            form = _hrUnitOfWork.PagesRepository.GetFormInfo(CompanyId, objectname, version, Language, roleId);
            form.JsMessages = MsgUtils.Instance.GetJsMessages();
            form.CodesLists = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(form.Id, Language);
            form.AllowInsert = User.Identity.GetAllowInsertCode();
            form.SessionVars = "{\"@RoleId\" : \"" + Session["RoleId"]?.ToString() + "\", \"@EmpId\": \"" + User.Identity.GetEmpId() + "\"}";
            form.LogTooltip = User.Identity.LogTooltip();
            return Json(form, JsonRequestBehavior.AllowGet);
        }

        //Save Form Design
        [HttpPost]
        public JsonResult SaveForm(FormDesginViewModel form)
        {

            if (!ModelState.IsValid)
            {
                return Json(Utils.ParseError(ModelState.Values));
            }

            if (Session["MenuId"] != null && Session["MenuId"].ToString().Length > 0 && form.MenuId == null)
                form.MenuId = int.Parse(Session["MenuId"].ToString());
            _hrUnitOfWork.PagesRepository.ApplyFormDesignChanges(CompanyId, form);

            var errors = SaveChanges(Language);
            if (errors.Count() > 0)
            {
                string message = errors.First().errors.First().message;
                return Json(message);
            }
            else
            {
                FormViewModel formvm = new FormViewModel();
                string key = CompanyId + form.ObjectName + form.Version + Language;
                formvm = _hrUnitOfWork.PagesRepository.GetFormInfo(CompanyId, form.ObjectName, form.Version, Language, Session["RoleId"]?.ToString());

                if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                    _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

                _hrUnitOfWork.PagesRepository.CacheManager.Set(key, formvm, 0);
            }
            return Json("OK");
        }
        

        [HttpPost]
        public JsonResult ResetForm(string objectName, byte version)
        {
            string key = CompanyId + objectName + version + Language;
            if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

            _hrUnitOfWork.PagesRepository.RemovePageDiv(CompanyId, objectName, version);
            string message = "OK";

            var errors = SaveChanges(Language);
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }
        public ActionResult resetGrid(string ObjectName, byte Version)
        {
            string key = CompanyId + ObjectName + Version + Language;
            if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

            _hrUnitOfWork.PagesRepository.RemovePageDiv(CompanyId, ObjectName, Version);
            string message = "OK";

            var errors = SaveChanges(Language);
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message, JsonRequestBehavior.AllowGet);

            //SqlDataReader rdr = null;
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HrSample"].ConnectionString);
            //SqlCommand cmd = new SqlCommand("select * from PageDivs where ObjectName='" + objectName + "' and CompanyId= 0 and Version=" + version, con);
            //con.Open();
            //rdr = cmd.ExecuteReader();
            //PageDivViewModel pageObject = new PageDivViewModel();
            //List<ColumnInfoViewModel> column = new List<ColumnInfoViewModel>();

            //while (rdr.Read())
            //{
            //    pageObject.Id = (int)rdr["Id"];
            //    pageObject.ObjectName = (string)rdr["ObjectName"];
            //    pageObject.TableName = (string)rdr["TableName"];
            //    pageObject.MenuId = (int)rdr["MenuId"];
            //    pageObject.Version = (byte)rdr["Version"];
            //}
            //if (rdr != null) rdr.Close();
            //if (pageObject.Id != 0)
            //{
            //    cmd = new SqlCommand("select * from GridColumns where GridId =" + pageObject.Id + "", con);
            //    rdr = cmd.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        column.Add(new ColumnInfoViewModel
            //        {
            //            Id = (int)rdr["Id"],
            //            GridId = (int)rdr["GridId"],
            //            DefaultValue = DBNull.Value.Equals(rdr["DefaultValue"]) ? "" : rdr["DefaultValue"].ToString(),
            //            ColumnName = (string)rdr["ColumnName"],

            //        });
            //    }
            //}
            //if (con != null) con.Close();
        }

        [HttpGet]
        public ActionResult ReadColumnInfo(string objectName)
        {
            byte version = 0;
            byte.TryParse(Request.QueryString["Version"], out version);
            return Json(_hrUnitOfWork.PagesRepository.GetColumnInfo(CompanyId, objectName, version, Language), JsonRequestBehavior.AllowGet);
        }

        //Column Prop
        [HttpGet]
        public ActionResult FormColumnPropGrid(string objectName)
        {
            ViewBag.CodeName = _hrUnitOfWork.Repository<LookUpCode>().Where(p => (p.StartDate <= DateTime.Today && (p.EndDate == null || p.EndDate >= DateTime.Today))).Select(l => new { value = l.CodeName, text = l.CodeName }).Distinct();
            return PartialView("_FormColumnGrid", objectName);
        }

        //Column Prop (For given column)
        [HttpGet]
        public ActionResult FormColumnPropForm(string tableName, string objectName, string columnName, byte version)
        {
            var codeUserNames = _hrUnitOfWork.LookUpRepository.GetLookUpUserCode(Language).Select(l => new { id = l.CodeName, name = l.Title }).ToList();
            var codeName = _hrUnitOfWork.LookUpRepository.GetLookUp(Language).Select(l => new { id = l.CodeName, name = l.Title }).ToList();
            codeName.AddRange(codeUserNames);
            ViewBag.CodeName = codeName;
            ViewBag.Columns = _hrUnitOfWork.NotificationRepository.GetColumnList(tableName, objectName, version, "Form", CompanyId, Language);

            FormColumnViewModel model = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, objectName, version, Language).Where(c => c.name == columnName).FirstOrDefault();
            return PartialView("_FormColumnForm", model);
        }

        [HttpPost]
        public ActionResult SaveColumnProp(FormColumnViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.MessageRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "FormColumnProp",
                        TableName = "FormColumns",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        ParentColumn = "SectionId",
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

                var record = _hrUnitOfWork.Repository<FormColumn>().Where(c => c.Id == model.Id).FirstOrDefault();
                if (record != null)
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "FormColumnProp",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = Model.Domain.TransType.Update
                    });

                    record.Required = model.required;
                    record.InputType = model.type;
                    record.IsUnique = model.isunique;
                    if (!record.IsUnique) record.UniqueColumns = "";
                    record.Pattern = model.pattern;
                    record.ColumnOrder = model.order;
                    record.Min = model.min;
                    record.Max = model.max;
                    record.MinLength = model.minLength;
                    record.MaxLength = model.maxLength;

                    _hrUnitOfWork.PagesRepository.Attach(record);
                    _hrUnitOfWork.PagesRepository.Entry(record).State = EntityState.Modified;
                }

                var Errors = SaveChanges(Language);

                string message = "OK," + ((new JavaScriptSerializer()).Serialize(model));

                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }

        //roles
        [HttpGet]
        public ActionResult RolePropGrid(string objectName)
        {
            return PartialView("_RoleFormColumns", objectName);
        }

       // Kendo read:ReadRoleFormColumns
        public ActionResult ReadRoleFormColumns(string RoleId, string objectName, byte version)
        {
            return Json(_hrUnitOfWork.PagesRepository.GetRoleFormColumns(RoleId, objectName, version, CompanyId, Language), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ReadFormColumnInfo(string objectName, byte version)
        {
            return Json(_hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, objectName, version, Language), JsonRequestBehavior.AllowGet);
        }
        //Kend:update ==>Role Form colums by Mamdouh
        public ActionResult UpdateRoleFormColumns(IEnumerable<RoleFormColumnViewModel> models,string objectName, byte version )

        {
            var datasource = new DataSource<RoleFormColumnViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                _hrUnitOfWork.PagesRepository.UpdateRoleFormColumns(models,objectName,version,CompanyId);
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
        public ActionResult UpdateFormColumnInfo(IEnumerable<FormColumnViewModel> models)
        {
            var datasource = new DataSource<FormColumnViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                var model = models.OrderBy(m => m.Id).FirstOrDefault(m => m.CompanyId == 0);
                if (model != null && model.CompanyId != CompanyId)
                {
                    model.CompanyId = CompanyId;
                    _hrUnitOfWork.PagesRepository.NewCompanyFormDesign(models);
                }
                else
                {
                    foreach (FormColumnViewModel column in models)
                    {
                        var record = new FormColumn
                        {
                            Id = column.Id,
                            SectionId = column.SectionId,
                            ColumnName = column.name,
                            ColumnOrder = column.order,
                            isVisible = column.isVisible,
                            ColumnType = column.ColumnType,
                            Required = column.required,
                            Min = column.min,
                            Max = column.max,
                            Pattern = column.pattern,
                            MaxLength = column.maxLength,
                            MinLength = column.minLength,
                            PlaceHolder = column.placeholder,
                            InputType = column.type,
                            IsUnique = column.isunique,
                            UniqueColumns = column.UniqueColumns,
                            HtmlAttribute = column.HtmlAttribute,
                            CodeName = column.CodeName,
                            DefaultValue = column.DefaultValue,
                            Formula = column.Formula
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
                var page = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.Id == models.FirstOrDefault().PageId).Select(a => new { companyid = a.CompanyId, objectname = a.ObjectName, version = a.Version}).FirstOrDefault();
                string key = page.companyid + page.objectname + page.version + Language;
                if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                    _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

                return Json(datasource.Data);
            }
        }

        /// for Add Option in Select(DropDpownList)
        /// input: objectname, return:Check if user allowed to ad to table and return List of Columns (form or grid)
        public ActionResult GetColumns(string objectname)
        {
            byte version = 0;
            PageDiv page = _hrUnitOfWork.PagesRepository.GetPageType(CompanyId, objectname, version);
            bool isAllowd = _hrUnitOfWork.MenuRepository.IsAllowTable(page.MenuId, UserName);
            if(!isAllowd) return Json("false," + MsgUtils.Instance.Trls("DontHavePermissionAddTable"), JsonRequestBehavior.AllowGet);

            List<FormList> qualGroups = new List<FormList>();
            if(objectname == "Qualifications")
                qualGroups = _hrUnitOfWork.QualificationRepository.GetQualGroups().Select(q => new FormList { id = q.Id, name = q.Name }).ToList();

            if (page.DivType == "Form")
            {
                var FormColumns = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, objectname, version, Language)
                    .Where(c => c.required);
                return Json(new { columns = FormColumns, qualGroupLst = qualGroups }, JsonRequestBehavior.AllowGet);
            }
            var GridColumns = _hrUnitOfWork.PagesRepository.GetColumnInfo(CompanyId, objectname, version, Language)
                .Where(c => c.Required && c.ColumnType != "button" && !(c.ColumnName == "Code" && c.DataType == "number")).OrderBy(c => c.ColumnOrder);

            return Json(new { columns = GridColumns, qualGroupLst = qualGroups }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCodeList(string codeName)
        {
            return Json(_hrUnitOfWork.LookUpRepository.GetLookUpCode(Language, codeName) , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddOption(SelectOptionsViewModel model)
        {
            var message = "OK,";
            if (ModelState.IsValid)
            {
                if (model.IsLookUp == 1) ///Lookup Code
                {
                    var lookupCodes = _hrUnitOfWork.Repository<LookUpCode>().Where(c => c.CodeName == model.SourceName).ToList();
                    if(lookupCodes.Count == 0)
                    {
                        return Json(2);
                    }
                    var isExists = lookupCodes.Where(c => c.Name == model._Name).FirstOrDefault() != null;
                    if (isExists)
                    {
                        ModelState.AddModelError("_Name", MsgUtils.Instance.Trls("AlreadyExists"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    else
                    {
                        LookUpCode newLookupCode = new LookUpCode();
                        var codeId = (short)(lookupCodes.Count == 0 ? 1 : lookupCodes.Select(c => c.CodeId).ToList().Max() + 1);
                        model._Id = codeId;
                        newLookupCode.CodeName = model.SourceName.ToString();
                        newLookupCode.CodeId = codeId;
                        newLookupCode.Name = model._Name;
                        newLookupCode.StartDate = new DateTime(2010, 1, 1);
                        newLookupCode.CreatedTime = DateTime.Now;
                        newLookupCode.CreatedUser = UserName;
                        _hrUnitOfWork.LookUpRepository.Add(newLookupCode);
                    }
                }
                else if(model.IsLookUp == 2)///Lookup User Code
                {
                    LookUpUserCode newUserCode = new LookUpUserCode();
                    var userCodes = _hrUnitOfWork.Repository<LookUpUserCode>().Where(c => c.CodeName == model.SourceName).ToList();
                    var isExists = userCodes.Where(c => c.Name == model._Name).FirstOrDefault() != null;
                    if (isExists)
                    {
                        ModelState.AddModelError("_Name", MsgUtils.Instance.Trls("AlreadyExists"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    else
                    {
                        var codeId = (short)(userCodes.Count == 0 ? 1 : userCodes.Select(c => c.CodeId).ToList().Max() + 1);
                        model._Id = codeId;
                        newUserCode.CodeName = model.SourceName.ToString();
                        newUserCode.CodeId = codeId;
                        newUserCode.Name = model._Name;
                        if(model.ColumnsNames == null)
                        {
                            ModelState.AddModelError("SysCodeId", MsgUtils.Instance.Trls("Required"));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                        int index = model.ColumnsNames.FindIndex(c => c == "SysCodeId"); 
                        byte sysCodeId;
                        byte.TryParse(model.ColumnsValue[index], out sysCodeId);
                        newUserCode.SysCodeId = sysCodeId;
                        newUserCode.StartDate = new DateTime(2010, 1, 1);
                        newUserCode.CreatedTime = DateTime.Now;
                        newUserCode.CreatedUser = UserName;
                        _hrUnitOfWork.LookUpRepository.Add(newUserCode);
                    }

                }
                else if (model.SourceName != null)///Object  
                {
                    // get table name from the object
                    var tableName = _hrUnitOfWork.Repository<PageDiv>().Where(p => p.CompanyId == CompanyId && p.ObjectName == model.SourceName && p.Version == 0).Select(p => p.TableName).FirstOrDefault();
                    if (tableName.Length == 0)
                        return Json("Table is not exist in page tables");  

                    model.ColumnsNames.Add("CreatedUser");
                    model.ColumnsNames.Add("CreatedTime");
                    model.ColumnsValue.Add(UserName);
                    model.ColumnsValue.Add(DateTime.Now + "");

                    var res = _hrUnitOfWork.PagesRepository.AddToTable(model, tableName, CompanyId);
                    if (res == 0) //Saved Fail
                        return Json(MsgUtils.Instance.Trls("CanotSaveData"));
                    else
                        model._Id = res;
                }

                var Errors = SaveChanges(Language);
                if (Errors.Count > 0 || message.IndexOf("OK") < 0)
                    message = Errors.First().errors.First().message;
                else
                {
                    if(model.IsLookUp == 1)
                        model.seqId = _hrUnitOfWork.Repository<LookUpCode>().Where(c => c.CodeName == model.SourceName && c.CodeId == model._Id && c.Name == model._Name).FirstOrDefault().Id;
                    message += (new JavaScriptSerializer()).Serialize(model);
                }
                return Json(message);
            }
            return Json(Models.Utils.ParseFormErrors(ModelState));
        }

        public JsonResult GetCalender()
        {
            return Json(_hrUnitOfWork.LeaveRepository.GetHolidays(CompanyId), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult ReadRemoteList(string tableName, string formTblName, string query, int? Id)
        {
            if (Id != null && tableName != "World")
                return Json(_hrUnitOfWork.PagesRepository.GetRemoteList(tableName, query, formTblName, CompanyId, Language).Where(a => a.Id == Id).Select(q => new { id = q.Id, name = q.Name, Icon = q.Icon, PicUrl = q.PicUrl }).ToList(), JsonRequestBehavior.AllowGet);
            else if (tableName == "World")
            {
                if (Language.Substring(0, 2) == "ar")
                {
                    var world = _hrUnitOfWork.Repository<World>()
                        .Where(c => c.NameAr != null && query != null ? c.NameAr.Contains(query) : false)
                        .Select(c => new { id = c.CountryId, country = c.CountryId, city = c.CityId, dist = c.DistrictId, name = c.NameAr }).ToList();
                    return Json(world.Take(world.Count < 10 ? world.Count : 10), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (query != null)
                    {
                        var world = _hrUnitOfWork.Repository<World>()
                            .Where(c => c.Name.ToLower().Contains(query.ToLower()))
                            .Select(c => new { id = c.CountryId, country = c.CountryId, city = c.CityId, dist = c.DistrictId, name = c.Name }).ToList();
                        return Json(world.Take(world.Count < 10 ? world.Count : 10), JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var result = _hrUnitOfWork.PagesRepository.GetRemoteList(tableName, query, formTblName, CompanyId, Language).Select(q => new { id = q.Id, name = q.Name, Icon = q.Icon, PicUrl = q.PicUrl });
                return Json(result.Take(result.Count() < 20 ? result.Count() : 20).ToList(), JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet][AllowAnonymous]
        public ActionResult GetImage(int id, string source)
        {
            var stream = _hrUnitOfWork.Repository<CompanyDocsViews>().Where(a => a.Source == source && a.SourceId == id).Select(a => a.file_stream).FirstOrDefault();
            if(stream != null)
                return Json(Convert.ToBase64String(stream), JsonRequestBehavior.AllowGet);

            return Json(false, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult IsUnique(Models.UniqueViewModel model)
        {
            return Json(Unique(model), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsUniqueP(Models.UniqueViewModel model)
        {
            return Json(Unique(model));
        }
        private bool Unique(Models.UniqueViewModel model)
        {
            // create sql statement
            StringBuilder sql = new StringBuilder("Select Count(1) From " + model.tablename + " Where");
            // for update
            if (model.id != null)
                sql.Append(" Id <> '" + model.id + "' And");
           
            // for child rows
            if (model.IsLocal)
                sql.Append(" (" + model.parentColumn + " = '" + model.parentId + "' Or " + model.parentColumn + " is null) And");
            else if (model.parentColumn != null)
                sql.Append(" " + model.parentColumn + " = '" + model.parentId + "' And");
            // basic filter columns
            for (var i = 0; i < model.columns.Length; i++)
            {
                if (i != 0) sql.Append(" And");
                sql.Append(" " + model.columns[i] + " = '" + model.values[i] + "'");
            }

            if (model.tablename == "Currencies") sql = sql.Replace("Id", "Code");       

            return _hrUnitOfWork.PagesRepository.GetIntResultFromSql(sql.ToString()) == 0;
        }
       
        public PagesController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        public ActionResult GetTree(string table, int? id)
        {
            var parm = new TreeViewParm {
                Id = id,
                CompanyId = CompanyId,
                Culture = Language,
                Table = table
            };

            return Json(_hrUnitOfWork.PagesRepository.GetTree(parm), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DropMenuItem(TreeViewModel source, TreeViewModel dest,string tablename)
        {
            string msg;
            if (ModelState.IsValid)
            {
                msg = _hrUnitOfWork.PagesRepository.DropMenuItem(source, dest, tablename, Language);
            }
            else
            {
                msg = Models.Utils.ParseErrors(ModelState.Values).FirstOrDefault().errors.FirstOrDefault().message;
            }

            return Json(msg);
        }

        [HttpPost]
        public ActionResult DropMenuItemCopy(TreeViewModel model, string tablename, bool copypages = false)
        {
            string msg;
            if (ModelState.IsValid)
            {
                msg = _hrUnitOfWork.PagesRepository.DropMenuItemCopy(model, tablename, Language, copypages);
            }
            else
            {
                msg = Models.Utils.ParseErrors(ModelState.Values).FirstOrDefault().errors.FirstOrDefault().message;
            }

            return Json(msg);
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
                var pageDiv = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.CompanyId == CompanyId && a.ObjectName == model.ObjectName && a.Version == model.Version).FirstOrDefault();

                if (model != null && pageDiv == null && model.CompanyId != CompanyId)
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
                            DefaultValue = column.DefaultValue,
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
                var model = models.FirstOrDefault();
                string key = CompanyId + model.ObjectName + model.Version + Language;
                if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                    _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

                return Json(datasource.Data);
            }
            
        }

        public JsonResult GetGrid(string objectName, byte version)
        {

            if (objectName == null || objectName.Length == 0 || Session["RoleId"] == null)
            {
                return Json("Missing reference to object name");
            }

            var grid = new GridDesignViewModel();
            string key = CompanyId + objectName + version + Language;
            if (!_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
            {
                grid = _hrUnitOfWork.PagesRepository.GetGrid(CompanyId, objectName, version, Language, Session["RoleId"].ToString());
                _hrUnitOfWork.PagesRepository.CacheManager.Set(key, grid, 0);
            }
            else
                grid = _hrUnitOfWork.PagesRepository.CacheManager.Get<GridDesignViewModel>(key);

            grid.ColumnInfo.Replace("@EmpId", User.Identity.GetEmpId().ToString());
            grid.JsMessages = MsgUtils.Instance.GetJsMessages();
            grid.IsAllowInsert = User.Identity.GetAllowInsertCode();

            return Json(grid, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveGrid(GridViewModel grid) 
        {
           
            if (Session["MenuId"] != null && Session["MenuId"].ToString().Length > 0 && grid.MenuId == null)
                grid.MenuId = int.Parse(Session["MenuId"].ToString());
           
            if (!ModelState.IsValid)
            {
                return Json(Utils.ParseError(ModelState.Values));
            }

            grid.CompanyId = CompanyId;
            _hrUnitOfWork.PagesRepository.ApplyAdminChanges(grid);

            var errors = SaveChanges(Language);
            if (errors.Count() > 0)
            {
                string message = errors.First().errors.First().message;
                return Json(message);
            }

            string key = CompanyId + grid.ObjectName + grid.Version + Language;
            if (_hrUnitOfWork.PagesRepository.CacheManager.IsSet(key))
                _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

            return Json("OK");
        }
    }
}
