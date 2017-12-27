using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Administration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AudiTrialController : BaseController
    {
        UserContext db = new UserContext();
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
        public AudiTrialController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;

        }
        public JsonResult GetMenuId(int CompanyId, string objectName, byte version)
        {
            int menuId = _hrUnitOfWork.PagesRepository.GetMenuId(CompanyId, objectName, version);
            return Json(menuId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if(MenuId != 0)
            ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }

        //ReadAudiTrials
        public ActionResult ReadAudiTrials(int MenuId, int pageSize, int skip)
        {
            var query = _hrUnitOfWork.AudiTrialRepository.ReadAudiTrials(Language, CompanyId);
            string filter = "";
            string Sorting = "";
            string whecls = GetWhereClause(MenuId);
            query = (IQueryable<AuditViewModel>)Utils.GetFilter(query, ref filter, ref Sorting);
            if (whecls.Length > 0 || filter.Length > 0)
            {
                try
                {
                    if (whecls.Length > 0 && filter.Length == 0)
                        query = query.Where(whecls);
                    else if (filter.Length > 0 && whecls.Length == 0)
                        query = query.Where(filter);
                    else
                        query = query.Where(filter).Where(whecls);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }

            var total = query.Count();

            if (Sorting.Length > 0)
                query = query.OrderBy(Sorting).Skip(skip).Take(pageSize);
            else if (skip > 0)
                query = query.Skip(skip).Take(pageSize);
            else
                query = query.Take(pageSize);



            return Json(new { total = total, data = query.ToList() }, JsonRequestBehavior.AllowGet);
        }

        #region Translation tbl
        public ActionResult TranslationIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadLanguage()
        {
            return Json(_hrUnitOfWork.AudiTrialRepository.ReadLanguage(), JsonRequestBehavior.AllowGet);
        }
        //ReadMsgs
        public ActionResult ReadMsgs(string culture)
        {
            return Json(_hrUnitOfWork.AudiTrialRepository.ReadMsgs(culture), JsonRequestBehavior.AllowGet);
        }
        //CreateMsgs
        public ActionResult UpdateMsgTbl(IEnumerable<MsgTblViewModel> models, string culture)
        {
            var datasource = new DataSource<MsgTblViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "MsgTbl",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                var msg = _hrUnitOfWork.Repository<MsgTbl>().Where(a => a.Culture == culture).ToList();
                foreach (MsgTblViewModel p in models)
                {
                    var MsgObj = msg.Where(a => a.SequenceId == p.Id).FirstOrDefault();
                    MsgObj.Name = p.Name;
                    MsgObj.Meaning = p.Meaning;
                    MsgObj.JavaScript = p.JavaScript;
                    _hrUnitOfWork.AudiTrialRepository.Attach(MsgObj);
                    _hrUnitOfWork.AudiTrialRepository.Entry(MsgObj).State = EntityState.Modified;
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
        public ActionResult CreateMsgTbl(IEnumerable<MsgTblViewModel> models, string culture)
        {
            var result = new List<MsgTbl>();
            var datasource = new DataSource<MsgTblViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "MsgTbl",
                        TableName = "MsgTbls",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                }

                foreach (MsgTblViewModel p in models)
                {
                    var msg = new MsgTbl
                    {
                        Culture = culture,
                        JavaScript = p.JavaScript,
                        Meaning = p.Meaning,
                        Name = p.Name
                    };
                    result.Add(msg);
                    _hrUnitOfWork.AudiTrialRepository.Add(msg);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on new { p.Culture, p.Name } equals new { r.Culture, r.Name }
                               select new MsgTblViewModel
                               {
                                   Id = r.SequenceId,
                                   Name = r.Name,
                                   Culture = r.Culture,
                                   Meaning = r.Name,
                                   JavaScript = r.JavaScript
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult DeleteMsgTbl(int Id)
        {
            var datasource = new DataSource<MsgTblViewModel>();
            var Obj = _hrUnitOfWork.Repository<MsgTbl>().FirstOrDefault(k => k.SequenceId == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "MsgTbl",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.AudiTrialRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        public ActionResult AddTrls()
        {
            var message = "OK";
            List<ErrorMessage> errorMessages = new List<ErrorMessage>();
            List<Error> errors = new List<Error>();
            List<MsgTbl> msgstable = new List<MsgTbl>();
            var Source = new DataSource<MsgTblViewModel>();
            string culture = Language;
            var root = Server.MapPath("~/").Replace("WebApp", "Db");
            string Path1 = root + "Persistence\\Repositories";
            var Folder = System.AppDomain.CurrentDomain.BaseDirectory + "Controllers";
            var directories = Directory.GetDirectories(System.AppDomain.CurrentDomain.BaseDirectory + "Views");
            FileInfo[][] arr = new FileInfo[2 + directories.Length][];
            DirectoryInfo di = new DirectoryInfo(Folder);
            DirectoryInfo di1 = new DirectoryInfo(Path1);
            arr[0] = di1.GetFiles("*.cs");
            arr[1] = di.GetFiles("*.cs");
            for (int i = 2; i < arr.Length - 2; i++)
            {
                DirectoryInfo x = new DirectoryInfo(directories[i]);
                arr[i] = x.GetFiles("*.cshtml");
            }
            for (int i = 0; i < arr.Length; i++)
            {
                FileInfo[] file = arr[i];
                if (file != null)
                {
                    foreach (var item in file)
                    {
                        string[] f = System.IO.File.ReadAllText(item.FullName).Split(';');
                        var trlsList = f.Where(j => j.Contains("Trls"));
                        if (trlsList != null)
                        {
                            foreach (var trls in trlsList)
                            {
                                int index = trls.IndexOf("Trls(");
                                if (index != -1 && trls.IndexOf("MsgUtils.Instance.Trls") != -1)
                                {
                                    var b = Regex.Matches(trls.Substring(index), "\"(.+?)\"");
                                    if (b.Count > 0)
                                    {

                                        string key = Regex.Matches(trls.Substring(index), "\"(.+?)\"")[0].Value.ToString().Replace("\"", "").TrimEnd();
                                        var exit = _hrUnitOfWork.Repository<MsgTbl>().Where(a => a.Name == key && a.Culture == culture).FirstOrDefault();
                                        if (exit == null)
                                        {

                                            var msg = new MsgTbl
                                            {
                                                Culture = culture,
                                                Name = key,
                                            };
                                            if (!msgstable.Exists(a => a.Name == msg.Name))
                                            {
                                                msgstable.Add(msg);
                                                _hrUnitOfWork.AudiTrialRepository.Add(msg);
                                            }
                                        }
                                    }

                                }

                            }
                        }
                    }
                }
            }
          
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
            {
                foreach (var item in msgstable)
                {
                    if (item.Name.Length > 30)
                    {
                        message = item.Name;
                        return Json(message, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details()
        {
            return View(new LangTblViewModel());
        }
        public ActionResult CopyLanguage(LangTblViewModel model)
        {
            List<Error> errors = new List<Error>();
            string msg = "OK";
            msg = _hrUnitOfWork.AudiTrialRepository.CopyLanguage(model.Source, model.Destination, CompanyId, Language);
            var result = msg.Split(',');
            var r = result[0];
            if (result.Length > 1)
            {
                if (result[1] == " FailToCopyLanguage")
                {
                    ModelState.AddModelError("", msg.Replace(result[0] + ",", ""));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
            }
            if (result[0] == "Error")
            {
                ModelState.AddModelError("", msg.Replace(result[0] + ",", ""));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Employee request

        public ActionResult EmpRequestIndex()
        {
            ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { value = a.Id, text = a.Employee }).Distinct().ToList();
            ViewBag.Depts = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId,null, Language);
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadEmpRequest(int MenuId)
        {
            var query = _hrUnitOfWork.PeopleRepository.GetAllRequests(CompanyId, Language);
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
        #endregion
        //Refresh Msg Tabel 
        public void DestroySingleToneObj()
        {
            MsgUtils.Instance.Refresh();
        }

        #region Langauge tbl
        public ActionResult LanguageIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadLang()
        {
            return Json(_hrUnitOfWork.AudiTrialRepository.ReadLang(), JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}