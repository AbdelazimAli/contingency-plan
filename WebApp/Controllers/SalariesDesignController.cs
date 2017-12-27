using Interface.Core;
using Model.ViewModel;
using Model.ViewModel.Payroll;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using Model.Domain.Payroll;
using WebApp.Models;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class SalariesDesignController : BaseController
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
        public SalariesDesignController(IHrUnitOfWork unitOfWork) : base(unitOfWork)    
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: SalariesDesign
        public ActionResult Index()
        {
            return View();
        }
        #region Pages Design
        public ActionResult FirstPage()
        {
            List<FormList> arr = new List<FormList>();
            arr.Add(new FormList { id = 1, name = MsgUtils.Instance.Trls("DeterminingSalaryBasis") });
            arr.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("DesigningSalaryBasis") });
            arr.Add(new FormList { id = 3, name = MsgUtils.Instance.Trls("DecisionSalaryBasis") });
            ViewBag.Purpose = arr;
            List<FormList> Basis = new List<FormList>();
            Basis.Add(new FormList { id = 1, name = MsgUtils.Instance.Trls("Hour") });
            Basis.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("Day") });
            Basis.Add(new FormList { id = 3, name = MsgUtils.Instance.Trls("Week") });
            Basis.Add(new FormList { id = 4, name = MsgUtils.Instance.Trls("Month") });
            Basis.Add(new FormList { id = 5, name = MsgUtils.Instance.Trls("Year") });
            Basis.Add(new FormList { id = 6, name = MsgUtils.Instance.Trls("Period") });
            ViewBag.Basi = Basis;    
            return View(new SalaryBasisDesignViewModel());
        }
        public ActionResult SecondPage()
        {
            ViewBag.SalaryItem = _hrUnitOfWork.Repository<SalaryItem>().Where(a => (a.IsSalaryItem) && (!a.IsLocal || a.CompanyId == CompanyId)).Select(b => new { id = b.Id, name = b.Name }).ToList();
            ViewBag.FormulaId = _hrUnitOfWork.Repository<Formula>().Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.Desc = MsgUtils.Instance.Trls("salaryitemsDes");
            return View(new SalaryDesignSecondViewModel());
        }
        public ActionResult ThirdPage()
        {
            

            List<FormList> lst = new List<FormList>();
            lst.Add(new FormList {id=0,name=MsgUtils.Instance.Trls("Job") });
            lst.Add(new FormList {id=1,name= MsgUtils.Instance.Trls("FromExcel") });
            lst.Add(new FormList {id=2,name= MsgUtils.Instance.Trls("PayrollGrade") });
            lst.Add(new FormList {id=3,name= MsgUtils.Instance.Trls("Grade") });
            lst.Add(new FormList {id=4,name= MsgUtils.Instance.Trls("SubGrade") });
            lst.Add(new FormList {id=5,name= MsgUtils.Instance.Trls("Points") });
            lst.Add(new FormList {id=6,name= MsgUtils.Instance.Trls("EditPoints") });
            lst.Add(new FormList {id=7,name= MsgUtils.Instance.Trls("Locations") });
            lst.Add(new FormList {id=8,name= MsgUtils.Instance.Trls("Departments") });
            lst.Add(new FormList {id=9,name= MsgUtils.Instance.Trls("OutLocations") });
            ViewBag.Srcs = lst;

            FillViewBag(CompanyId, Language);
            return View();
        }
        private void FillViewBag(int companyId,string Culture)
        {
            ViewBag.dept = _hrUnitOfWork.CompanyStructureRepository.GetAllActiveCompanyStructure(companyId, Culture).Select(c => new { text = c.name, value = c.id }).ToList();
            ViewBag.payrollDeu = _hrUnitOfWork.Repository<PayrollDue>().Select(c => new { text = c.Name, value = c.Id }).ToList();
            ViewBag.pplgrp = _hrUnitOfWork.PeopleRepository.GetPeoples().Select(c => new { text = c.Name, value = c.Id }).ToList();
            ViewBag.prollgrd = _hrUnitOfWork.PayrollRepository.ReadPayrollGrades(Culture, companyId).Select(c => new { text = c.Name, value = c.Id }).ToList();
            ViewBag.job = _hrUnitOfWork.JobRepository.ReadJobs(companyId, Culture,0).Select(c => new { text = c.LocalName, value = c.Id }).ToList();
            ViewBag.loc = _hrUnitOfWork.LocationRepository.ReadLocations(Culture, companyId).Select(c => new { text = c.LocalName, value = c.Id }).ToList();
            ViewBag.persnType = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", Culture).Select(c => new { text = c.Title, value = c.Id }).ToList();
            ViewBag.Perf = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Performance", Culture).Select(c => new { text = c.Title, value = c.Id }).ToList();
            var Formulalst = _hrUnitOfWork.Repository<Formula>().Where(a => a.CompanyId == companyId && (a.StartDate <= DateTime.Today && (a.EndDate == null || a.EndDate > DateTime.Today))).ToList();
            ViewBag.Formula = Formulalst.Count != 0 ? Formulalst.Where(a => a.Result == 1 || a.Result == 2 || a.Result == 5).Select(c => new { text = c.Name, value = c.Id }).ToList() : null;
            ViewBag.yesNoFrm = Formulalst.Count != 0 ? Formulalst.Where(a => a.Result == 3 || a.Result == 4).Select(c => new { text = c.Name, value = c.Id }).ToList() : null;
            ViewBag.Credit = _hrUnitOfWork.Repository<Account>().Where(a => (!a.IsLocal || (a.CompanyId == companyId && a.IsLocal)) && (a.StartDate <= DateTime.Today && (a.EndDate == null || a.EndDate > DateTime.Today))).Select(c => new { text = c.Name, value = c.Id }).ToList();
        }
        public ActionResult FourthPage()
        {
            return View();
        }
        #endregion
        #region PayrollWizare
        public ActionResult ReadRangeTable()
        {
            return Json("",JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadLinkTable()
        {
           
            return Json("",JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeDataSource(int Choose, int? StartGrade, int? EndGrade, int? PointValue, int? SubGrade)
        {
            List<LinkTableViewModel> cols = new List<LinkTableViewModel>();
            switch (Choose)
            {
                case 1:
                    cols = GetExcel();
                    break;
                case 2:
                    cols = GetPayrollGrade(StartGrade,EndGrade);
                    break;
                case 3:
                    cols = GetGrade(StartGrade,EndGrade);
                    break;
                case 4:
                    cols = GetSubGrade(SubGrade);
                    break;
                case 5:
                case 6:
                    cols = GetPoints(PointValue);
                    break;
                case 7:
                    cols = GetLocations();
                    break;
                case 8:
                    cols = GetDepartments();
                    break;
                case 9:
                    cols = GetOutLocations();
                    break;
                default:
                    cols = GetJobs();
                    break;
            }
            return Json(cols, JsonRequestBehavior.AllowGet);
        }
        public List<LinkTableViewModel> GetExcel()
        {
            return new List<LinkTableViewModel>();
        }
        public List<LinkTableViewModel> GetPayrollGrade(int? start,int? End)
        {
            var allGrades = _hrUnitOfWork.PayrollRepository.ReadPayrollGrades(Language, CompanyId).Select(a => a.Id).ToArray();
            var cols = new List<LinkTableViewModel>();
            for (int i = 0; i < allGrades.Length; i++)
            {
                cols.Add(new LinkTableViewModel
                {
                    GradeId = allGrades[i],
                    CellValue = (i == 0 ? start : start + (End * i))
                });
            }
            
            return cols;
        }
        public List<LinkTableViewModel> GetGrade(int? start,int? End)
        {
            var allGrades = _hrUnitOfWork.PayrollRepository.ReadPayrollGrades(Language, CompanyId).Select(a => a.Grade).ToArray();
            var cols = new List<LinkTableViewModel>();
            for (int i = 0; i < allGrades.Length; i++)
            {
                cols.Add(new LinkTableViewModel
                {
                    Grade = allGrades[i],
                    CellValue = (i==0 ? start : start + (End * i))
                });
            }

            return cols;

        }
        public List<LinkTableViewModel> GetSubGrade(int? Subgrade)
        {
            var allGrades = _hrUnitOfWork.PayrollRepository.ReadPayrollGrades(Language, CompanyId).Select(a => new { a.SubGrade ,a.Grade }).ToArray();

            var cols = new List<LinkTableViewModel>();
            for (int i = 0; i < allGrades.Length; i++)
            {
                int v = i + 1;
                cols.Add(new LinkTableViewModel
                {
                    Grade = allGrades[i].Grade,
                    SubGrade = allGrades[i].SubGrade,
                    CellValue = Subgrade * v
                });
            }

            return cols;

        }
        public List<LinkTableViewModel> GetPoints(int? PointValue)
        {
            var allGrades = _hrUnitOfWork.PayrollRepository.ReadPayrollGrades(Language, CompanyId).Select(a => new { a.Id,a.Point }).ToArray();
            var cols = new List<LinkTableViewModel>();
            int pointlength = 0;
            for (int i = 0; i < allGrades.Length; i++)
            {
                if (allGrades[i].Point != null)
                {
                    pointlength = allGrades[i].Point.Count();
                    for (int j = 0; j < pointlength; j++)
                    {
                        short x = Convert.ToInt16(allGrades[i].Point.ElementAtOrDefault(j));
                        string poi = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("GradePoints", Language).Where(a => a.CodeId ==x ).Select(p => p.Name).FirstOrDefault();
                        cols.Add(new LinkTableViewModel
                        {
                            GradeId = allGrades[i].Id,
                            Point = poi,
                            CellValue = int.Parse(poi) * (PointValue != 0 ? PointValue : 1),

                        });
                    }
                }else
                {
                    cols.Add(new LinkTableViewModel
                    {
                        GradeId = allGrades[i].Id,
                    });
                }
               
            }
            return cols;

        }
        public List<LinkTableViewModel> GetLocations()
        {
            var allLoc = _hrUnitOfWork.LocationRepository.ReadLocations(Language, CompanyId).Select(a => a.Id).ToArray();
            var cols = new List<LinkTableViewModel>();
      
            for (int i = 0; i < allLoc.Length; i++)
            {
               
                    cols.Add(new LinkTableViewModel
                    {
                        LocationId = allLoc[i],
                    });
            }
            return cols;
        }
        public List<LinkTableViewModel> GetOutLocations()
        {
            var allLoc = _hrUnitOfWork.LocationRepository.ReadLocations(Language, CompanyId).Where(s=>s.IsLocal == false).Select(a => a.Id).ToArray();
            var cols = new List<LinkTableViewModel>();

            for (int i = 0; i < allLoc.Length; i++)
            {

                cols.Add(new LinkTableViewModel
                {
                    LocationId = allLoc[i],
                });
            }
            return cols;

        }
        public List<LinkTableViewModel> GetDepartments()
        {
            var allLoc = _hrUnitOfWork.CompanyStructureRepository.GetAllActiveCompanyStructure( CompanyId, Language).Select(a => a.id).ToArray();
            var cols = new List<LinkTableViewModel>();

            for (int i = 0; i < allLoc.Length; i++)
            {

                cols.Add(new LinkTableViewModel
                {
                    DeptId = allLoc[i],
                });
            }
            return cols;

        }
        public List<LinkTableViewModel> GetJobs()
        {
            var allJob = _hrUnitOfWork.JobRepository.ReadJobs(CompanyId, Language,0).Select(a => a.Id).ToArray();
            var cols = new List<LinkTableViewModel>();

            for (int i = 0; i < allJob.Length; i++)
            {

                cols.Add(new LinkTableViewModel
                {
                    JobId = allJob[i],
                });
            }
            return cols;
        }
        private void SaveGrid1(RangeTableVm grid1, IEnumerable<KeyValuePair<string, ModelState>> state, InfoTable info)
        {
            if (grid1.inserted != null)
            {
                foreach (RangeTableViewModel model in grid1.inserted)
                {
                    var range = new RangeTable();
                    AutoMapper(new Models.AutoMapperParm { Destination = range, Source = model });
                    range.CreatedTime = DateTime.Now;
                    range.CreatedUser = UserName;
                    _hrUnitOfWork.SalaryDesignRepository.AddRangeTable(range);
                }
            }
        }
        private void SaveGrid2(LinkTableVM grid1, IEnumerable<KeyValuePair<string, ModelState>> state, InfoTable info)
        {
            if (grid1.inserted != null)
            {
                foreach (LinkTableViewModel model in grid1.inserted)
                {
                    var range = new LinkTable();
                    AutoMapper(new Models.AutoMapperParm { Destination = range, Source = model });
                    range.CreatedTime = DateTime.Now;
                    range.CreatedUser = UserName;
                    _hrUnitOfWork.SalaryDesignRepository.AddLinkTable(range);
                }
            }
        }
        [HttpPost]
        public ActionResult DetailsFirstPage(SalaryBasisDesignViewModel model, OptionsViewModel moreInfo, RangeTableVm grid1)
        {
            string msg = "OK,";
            List<Error> errors = new List<Error>();

            if (model.Purpose != 2)
            {

                if (ModelState.IsValid)
                {
                    if (ServerValidationEnabled)
                    {
                        errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                        {
                            CompanyId = CompanyId,
                            ObjectName = "SalaryBasisPage1",
                            Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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
                }
                msg += ((new JavaScriptSerializer().Serialize(model)));
                return Json(msg);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (ServerValidationEnabled)
                    {
                        errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                        {
                            CompanyId = CompanyId,
                            ObjectName = "SalaryBasisPage1",
                            Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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
                }
                if (model.Id == 0)
                {
                    InfoTable record = new InfoTable();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "SalaryBasisPage1",
                        Version = Convert.ToByte(Request.QueryString["Version"]),
                        Options = moreInfo
                    });
                    record.CompanyId = CompanyId;
                    record.TableType = (grid1.inserted != null ? grid1.inserted.FirstOrDefault().TableType : null);
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    _hrUnitOfWork.SalaryDesignRepository.Add(record);
                    SaveGrid1(grid1, ModelState.Where(a => a.Key.Contains("grid1")), record);

                    errors = SaveChanges(Language);

                    if (errors.Count > 0) msg = errors.First().errors.First().message;
                }
            }

            return Json(msg);
        }
        [HttpPost]
        public ActionResult DetailsSecondPage(SalaryDesignSecondViewModel model, OptionsViewModel moreInfo, SalaryBasisDesignViewModel grid1,LinkTableVM grid2)
        {
            var msg = "OK,";
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "SalaryBasisPage2",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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
            }
            if (model.Id == 0)
            {
                InfoTable record = new InfoTable();
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = record,
                    Source = model,
                    ObjectName = "SalaryBasisPage2",
                    Version = Convert.ToByte(Request.QueryString["Version"]),
                    Options = moreInfo
                });
                record.Name = grid1.Name;
                record.IsLocal = grid1.IsLocal;
                record.StartDate = grid1.StartDate;
                record.EndDate = grid1.EndDate;
                record.Basis = grid1.Basis;
                record.Purpose = grid1.Purpose;
                record.CompanyId = CompanyId;

                record.CreatedTime = DateTime.Now;
                record.CreatedUser = UserName;
                _hrUnitOfWork.SalaryDesignRepository.Add(record);
                SaveGrid2(grid2, ModelState.Where(a => a.Key.Contains("grid2")), record);

                errors = SaveChanges(Language);

                if (errors.Count > 0) msg = errors.First().errors.First().message;
            }

            return Json(msg);
        }
        #endregion
        #region GetAllPayrollDesigns
        public ActionResult PayrollTables()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetAllDesignes(int MenuId)
        {

            var query = _hrUnitOfWork.SalaryDesignRepository.GetPayrollsDesigns(CompanyId);
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
        public ActionResult DeleteDesign(int id)
        {
            List<Error> errors = new List<Error>();
            DataSource<SalaryBasisDesignViewModel> Source = new DataSource<SalaryBasisDesignViewModel>();
            InfoTable info = _hrUnitOfWork.SalaryDesignRepository.Find(a=>a.Id ==id).FirstOrDefault();
            _hrUnitOfWork.SalaryDesignRepository.Remove(info.Id);

            string message = "OK";
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else
                return Json(message);
        }
        public ActionResult DetailsDesigne(int id)
        {
            
            var model = _hrUnitOfWork.SalaryDesignRepository.Find(a => a.Id == id).FirstOrDefault();
            FillViewBag(CompanyId, Language);
            return View(model);
        }
        public ActionResult ReadTable(int id)
        {
            return Json(_hrUnitOfWork.SalaryDesignRepository.GetPayrollsDesign(id), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region LinkTable
        public ActionResult CreateLinkTable(IEnumerable<LinkTableViewModel> models)
        {
            var result = new List<LinkTable>();

            var datasource = new DataSource<LinkTableViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId, //CompanyId,
                        ObjectName = "LinkTables",
                        TableName = "LinkTables",
                        ParentColumn = "GenTableId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (LinkTableViewModel c in models)
                {
                    var Link = new LinkTable
                    {
                        Id = c.Id,
                        GenTableId = c.GenTableId,
                        Basis =  c.Basis,
                        CellValue = c.CellValue,
                        CreditGlAccT = c.CreditGlAccT != 0 ? c.CreditGlAccT :null,
                        DebitGlAccT = c.DebitGlAccT != 0 ? c.DebitGlAccT :null,
                        DeptId = c.DeptId != 0 ? c.DeptId : null,
                        FormulaId = c.FormulaId != 0 ? c.FormulaId : null,
                        GradeId = c.GradeId != 0 ? c.GradeId : null,
                        JobId = c.JobId != 0 ? c.JobId : null,
                        GroupId = c.GroupId != 0 ? c.GroupId : null,
                        LocationId = c.LocationId != 0 ? c.LocationId : null,
                        PayDueId = c.PayDueId != 0 ? c.PayDueId : null,
                        PersonType = c.PersonType != 0 ? c.PersonType : null,
                        Performance = c.Performance != 0 ? c.Performance : null,
                        SalItemId = c.SalItemId != 0 ? c.SalItemId : null,
                        YesNoForm = c.YesNoForm != 0 ? c.YesNoForm : null,
                        MinValue = c.MinValue,
                        MaxValue = c.MaxValue,
                        Grade = c.Grade,
                        SubGrade = c.SubGrade,
                        Point = c.Point,
                        CreatedTime = DateTime.Now,
                        CreatedUser = UserName
                       
                    };

                    result.Add(Link);
                    _hrUnitOfWork.SalaryDesignRepository.AddLinkTable(Link);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from c in models
                               select new LinkTableViewModel
                               {
                                   Id = c.Id,
                                   GenTableId = c.GenTableId,
                                   Basis = c.Basis,
                                   CellValue = c.CellValue,
                                   CreditGlAccT = c.CreditGlAccT,
                                   DebitGlAccT = c.DebitGlAccT,
                                   DeptId = c.DeptId,
                                   FormulaId = c.FormulaId,
                                   Grade = c.Grade,
                                   GradeId = c.GradeId,
                                   JobId = c.JobId,
                                   GroupId = c.GroupId,
                                   MaxValue = c.MaxValue,
                                   LocationId = c.LocationId,
                                   MinValue = c.MinValue,
                                   PayDueId = c.PayDueId,
                                   PersonType = c.PersonType,
                                   Point = c.Point,
                                   Performance = c.Performance,
                                   SalItemId = c.SalItemId,
                                   SubGrade = c.SubGrade,
                                   YesNoForm = c.YesNoForm
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }
        public ActionResult UpdateLinkTable(IEnumerable<LinkTableViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<LinkTableViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {

                        CompanyId = CompanyId,
                        ObjectName = "LinkTables",
                        TableName = "LinkTables",
                        ParentColumn = "GenTableId",
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
                    var item = models.ElementAtOrDefault(i);
                    var Link = new LinkTable();
                    AutoMapper(new AutoMapperParm() { ObjectName = "LinkTables", Destination = Link, Source = item, Version = 0, Options = options.ElementAtOrDefault(i) });
                    Link.CreditGlAccT = item.CreditGlAccT != 0 ? item.CreditGlAccT : null;
                    Link.DebitGlAccT = item.DebitGlAccT != 0 ? item.DebitGlAccT : null;
                    Link.DeptId = item.DeptId != 0 ? item.DeptId : null;
                    Link.FormulaId = item.FormulaId != 0 ? item.FormulaId : null;
                    Link.GradeId = item.GradeId != 0 ? item.GradeId : null;
                    Link.JobId = item.JobId != 0 ? item.JobId : null;
                    Link.GroupId = item.GroupId != 0 ? item.GroupId : null;
                    Link.LocationId = item.LocationId != 0 ? item.LocationId : null;
                    Link.PayDueId = item.PayDueId != 0 ? item.PayDueId : null;
                    Link.PersonType = item.PersonType != 0 ? item.PersonType : null;
                    Link.Performance = item.Performance != 0 ? item.Performance : null;
                    Link.SalItemId = item.SalItemId != 0 ? item.SalItemId : null;
                    Link.YesNoForm = item.YesNoForm != 0 ? item.YesNoForm : null;
                    Link.ModifiedTime = DateTime.Now;
                    Link.ModifiedUser = UserName;
                    _hrUnitOfWork.SalaryDesignRepository.AttachLinkTable(Link);
                    _hrUnitOfWork.SalaryDesignRepository.Entry(Link).State = EntityState.Modified;
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
        public ActionResult DeleteLinkTable(IEnumerable<LinkTableViewModel> models)
        {
            var datasource = new DataSource<LinkTableViewModel>();

            if (ModelState.IsValid)
            {
                foreach (LinkTableViewModel model in models)
                {
                    var link = new LinkTable
                    {
                        Id = model.Id
                    };

                    _hrUnitOfWork.SalaryDesignRepository.Remove(link);
                }

                datasource.Errors = SaveChanges(Language);
                datasource.Total = models.Count();
            }

            datasource.Data = models;

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        #endregion
        #region RangeTable
        public ActionResult CreateRangeTable(IEnumerable<RangeTableViewModel> models)
        {
            var result = new List<RangeTable>();

            var datasource = new DataSource<RangeTableViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId, //CompanyId,
                        ObjectName = "RangeTable",
                        TableName = "RangeTables",
                        ParentColumn = "GenTableId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (RangeTableViewModel c in models)
                {
                    var Range = new RangeTable
                    {
                        Id = c.Id,
                        GenTableId = c.GenTableId,
                        FormValue = c.FormValue,
                        RangeValue = c.RangeValue,
                        TableType = c.TableType,
                        ToValue = c.ToValue,
                        CreatedTime = DateTime.Now,
                        CreatedUser = UserName
                    };

                    result.Add(Range);
                    _hrUnitOfWork.SalaryDesignRepository.AddRangeTable(Range);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from c in models
                               select new RangeTableViewModel
                               {
                                   Id = c.Id,
                                   GenTableId = c.GenTableId,
                                   FormValue = c.FormValue,
                                   RangeValue = c.RangeValue,
                                   TableType = c.TableType,
                                   ToValue = c.ToValue
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }
        public ActionResult UpdateRangeTable(IEnumerable<RangeTableViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<RangeTableViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {

                        CompanyId = CompanyId,
                        ObjectName = "RangeTable",
                        TableName = "RangeTables",
                        ParentColumn = "GenTableId",
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
                    var Range = new RangeTable();
                    AutoMapper(new AutoMapperParm() { ObjectName = "RangeTable", Destination = Range, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i) });
                    Range.ModifiedTime = DateTime.Now;
                    Range.ModifiedUser = UserName;
                    _hrUnitOfWork.SalaryDesignRepository.AttachRangeTable(Range);
                    _hrUnitOfWork.SalaryDesignRepository.Entry(Range).State = EntityState.Modified;
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
        public ActionResult DeleteRangeTable(IEnumerable<RangeTableViewModel> models)
        {
            var datasource = new DataSource<RangeTableViewModel>();

            if (ModelState.IsValid)
            {
                foreach (RangeTableViewModel model in models)
                {
                    var Range = new RangeTable
                    {
                        Id = model.Id
                    };

                    _hrUnitOfWork.SalaryDesignRepository.Remove(Range);
                }

                datasource.Errors = SaveChanges(Language);
                datasource.Total = models.Count();
            }

            datasource.Data = models;

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        #endregion
    }
}                                                                                                                                                                                                       