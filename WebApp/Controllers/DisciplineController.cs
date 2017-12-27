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
    public class DisciplineController : BaseController
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
        public DisciplineController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        public ActionResult Details(int id = 0)
        {
            if (id != 0)
            {
                ViewBag.StartPeriod = _hrUnitOfWork.Repository<DisPeriodNo>().Where(a => a.PeriodId == id).OrderBy(c => c.PeriodEDate).LastOrDefault();
                if (ViewBag.StartPeriod != null)
                {
                    ViewBag.date = _hrUnitOfWork.Repository<DisPeriodNo>().Where(a => a.PeriodId == id).OrderBy(c => c.PeriodEDate).Select(a => a.PeriodEDate).LastOrDefault().AddDays(1);
                }
            }
            if (id == 0)
                return View(new DisplinPeriod());
            var DisplinePeriod = _hrUnitOfWork.Repository<DisplinPeriod>().Where(a => a.Id == id).FirstOrDefault();
            ViewBag.desplinObj = DisplinePeriod;
            return DisplinePeriod == null ? (ActionResult)HttpNotFound() : View(DisplinePeriod);
        }
        public ActionResult Index()
        {
            ViewBag.SysType = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("SysType", Language).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            ViewBag.Frequency = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Frequency", Language).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadDisplinePeriod(int MenuId)
        {

            var query = _hrUnitOfWork.DisciplineRepository.ReadDisplinePeriod();
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
        public ActionResult ReadDisPeriodNo(int PeriodId)
        {
            return Json(_hrUnitOfWork.DisciplineRepository.ReadDisPeriodNo(PeriodId), JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveDisplinPeriod(DisplinPeriodViewModel model, OptionsViewModel moreInfo, RequestDisplinRangeGrid grid1,int? periodval)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "DisplinPeriods",
                        TableName = "DisplinPeriods",
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
                var record = _hrUnitOfWork.Repository<DisplinPeriod>().FirstOrDefault(j => j.Id == model.Id);
                if (record == null) //Add
                {
                    record = new DisplinPeriod();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "DisplinPeriods",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    _hrUnitOfWork.DisciplineRepository.Add(record);
                    if (model.submit == true)
                    {
                        // && model.Frequency != null && model.Times != null
                        if (model.TotalYear != null)
                        {
                            GetPeriodNO(model, record);
                        }
                        else
                        {
                            ModelState.AddModelError("TotalYear", MsgUtils.Instance.Trls("MustInsertTotalYear"));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }

                }
                else //update
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "People",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    _hrUnitOfWork.DisciplineRepository.Attach(record);
                    _hrUnitOfWork.DisciplineRepository.Entry(record).State = EntityState.Modified;
                    if (model.submit == true)
                    {
                        if (model.TotalYear != null)
                        {
                            GetPeriodNO(model, record);
                        }
                        else
                        {
                            ModelState.AddModelError("TotalYear", MsgUtils.Instance.Trls("MustInsertTotalYear"));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }

                }
                errors = SaveGrid1(grid1, ModelState.Where(a => a.Key.Contains("grid1")), record);
                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                string message = "OK," + record.Id;
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }

            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        private void GetPeriodNO(DisplinPeriodViewModel model, DisplinPeriod record )
        {
            var period = _hrUnitOfWork.Repository<DisPeriodNo>().Where(s=>s.PeriodId==record.Id).Select(a => a.PeriodNo).LastOrDefault();
            int day = model.PeriodSDate.Day;
            int modelYear = model.PeriodSDate.Year;
            DateTime startTime = model.PeriodSDate;
            DateTime endTime = model.PeriodSDate;
            DateTime customName = model.PeriodSDate;
            //int year1 = 0;
            int year = 0;
            if (model.Frequency == 1)
                year = 360;
            else if (model.Frequency == 2)
                year = 52;
            else if (model.Frequency == 3)
                year = 12;
            else
                year = 1;
            int? Iteration = (model.TotalYear * year) / model.Times;
            for (int i = 0; i < Iteration; i++)
            {
                if (model.Frequency == 3)
                {
                    startTime = model.PeriodSDate.AddMonths(model.Times * i);
                    endTime = model.PeriodSDate.AddMonths(model.Times * (i + 1)).AddDays(-1);
                    //int month = model.PeriodSDate.Month + i * model.Times;
                    //int Year = modelYear;
                    //if (month > 12)
                    //{
                        
                    //    Year = modelYear + Convert.ToInt32(month/12);
                    //    year1 = Convert.ToInt32(month / 12);
                    //    month = month - (year1 * 12);
                    //}
                    //startTime = new DateTime(Year, month, day);
                    //endTime = startTime.AddMonths(model.Times).AddDays(-1);
                }
                else if (model.Frequency == 4)
                {
                    startTime = model.PeriodSDate.AddYears(model.Times * i);
                    endTime = model.PeriodSDate.AddYears(model.Times * (i + 1)).AddDays(-1);
                  
                }
                else if (model.Frequency == 1)
                {
                    
                   startTime= model.PeriodSDate.AddDays(model.Times * i);
                    endTime = model.PeriodSDate.AddDays(model.Times * (i+1)).AddDays(-1);
                }
                else
                {
                    int Calcweak = model.Times *7;
                    startTime = model.PeriodSDate.AddDays(Calcweak * i);
                    endTime = model.PeriodSDate.AddDays(Calcweak * (i + 1)).AddDays(-1);

                }
              
                DisPeriodNo periodNo = new DisPeriodNo()
                {
                    PeriodNo = ++period,
                    CreatedUser = UserName,
                    Posted = false,
                    PostDate = DateTime.Now,
                    CreatedTime = DateTime.Now,
                    Name = startTime.ToString("MM/yyyy"),
                    PeriodEDate = endTime,
                    PeriodSDate = startTime,
                    PeriodId = record.Id

                };

                _hrUnitOfWork.DisciplineRepository.Add(periodNo);

            }
        }
        public ActionResult DeleteDisplinePeriod(int id)
        {
            List<Error> errors = new List<Error>();
            DataSource<DisplinPeriodViewModel> Source = new DataSource<DisplinPeriodViewModel>();
            DisplinPeriod Displine = _hrUnitOfWork.DisciplineRepository.GetDisplinPeriod(id);
            if (Displine != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = Displine,
                    ObjectName = "DisplinPeriods",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.DisciplineRepository.RemoveDisplinPeriod(Displine.Id);
            }
            var DeletedPeriods = _hrUnitOfWork.Repository<DisPeriodNo>().Where(a => a.PeriodId == id).ToList();
            if (DeletedPeriods != null)
                _hrUnitOfWork.DisciplineRepository.RemoveRange(DeletedPeriods);

            string message = "OK";
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else
                return Json(message);
        }
        public ActionResult UpdateDisPeriodNo(IEnumerable<DisplinePeriodNoViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<DisplinePeriodNoViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "DisplinPeriod",
                        TableName = "DisplinPeriods",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        Culture = Language

                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Id);
                var db_displines = _hrUnitOfWork.Repository<DisPeriodNo>().Where(a => ids.Contains(a.Id)).ToList();

                for (var i = 0; i < models.Count(); i++)
                {
                    var displine = db_displines.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "DisplinPeriod", Destination = displine, Source = models.ElementAtOrDefault(i), Version = Convert.ToByte(Request.Form["Version"]), Options = options.ElementAtOrDefault(i) , Transtype = TransType.Update});

                    _hrUnitOfWork.DisciplineRepository.Attach(displine);
                    _hrUnitOfWork.DisciplineRepository.Entry(displine).State = EntityState.Modified;
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
        public ActionResult DisciplineIndex()
        {
            ViewBag.DisciplineClass = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("DisciplineClass", Language).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            ViewBag.PeriodId = _hrUnitOfWork.Repository<DisplinPeriod>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadDiscipline(int MenuId)
        {
            var query = _hrUnitOfWork.DisciplineRepository.ReadDiscipline(CompanyId);
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
        public ActionResult DisciplineDetails(int id = 0)
        {
            string culture = Language;
            ViewBag.DisplinType = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("DisplinType", culture).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            ViewBag.DisciplineClass = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("DisciplineClass", culture).Select(a => new { id = a.CodeId, name = a.Title }).ToList();
            ViewBag.PeriodId = _hrUnitOfWork.Repository<DisplinPeriod>().Where(a=>a.EndDate >= DateTime.Now).Select(a => new { id = a.Id, name = a.Name }).ToList();
            if (id == 0)
                return View(new Discipline());
            var Discipline = _hrUnitOfWork.Repository<Discipline>().Where(a => a.Id == id).FirstOrDefault();
            return Discipline == null ? (ActionResult)HttpNotFound() : View(Discipline);
        }
        public ActionResult ReadDisRepeat(int DisplinId)
        {
            return Json(_hrUnitOfWork.DisciplineRepository.ReadDisRepeat(DisplinId), JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveDiscipline(DisciplineViewModel model, OptionsViewModel moreInfo, RequestDisplinRepeatGrid grid1)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Discipline",
                        TableName = "Disciplines",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
                var Sequence = _hrUnitOfWork.Repository<Discipline>().Select(s => s.Code).DefaultIfEmpty(0).Max();
                var MaxCode = Sequence == 0 ? 1 : Sequence + 1;
                var record = _hrUnitOfWork.Repository<Discipline>().FirstOrDefault(j => j.Id == model.Id);
                if (record == null) //Add
                {
                    record = new Discipline();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Discipline",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    record.Code = MaxCode;
                   
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("MustGreaterthanStart"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    _hrUnitOfWork.DisciplineRepository.Add(record);

                }
                else //update
                {
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Discipline",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Update

                    });
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("MustGrthanStart"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    _hrUnitOfWork.DisciplineRepository.Attach(record);
                    _hrUnitOfWork.DisciplineRepository.Entry(record).State = EntityState.Modified;

                }
                // Save grid1
                errors = SaveGrid1(grid1, ModelState.Where(a => a.Key.Contains("grid1")), record);
                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(record));
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        private List<Error> SaveGrid1(RequestDisplinRepeatGrid grid1, IEnumerable<KeyValuePair<string, ModelState>> state, Discipline Displinobj)
        {
            List<Error> errors = new List<Error>();
            // Deleted
            if (grid1.deleted != null)
            {
                foreach (DisplinRepeatViewModel model in grid1.deleted)
                {
                    var RequestDisplinRepeat = new DisplinRepeat
                    {
                        Id = model.Id
                    };

                    _hrUnitOfWork.DisciplineRepository.Remove(RequestDisplinRepeat);
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
                        ObjectName = "DisplinRepeats",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });
                    if (errors.Count() > 0) return errors;
                }
            }

            // updated records
            if (grid1.updated != null)
            {
                foreach (DisplinRepeatViewModel model in grid1.updated)
                {
                    var requestRe = new DisplinRepeat();
                    AutoMapper(new Models.AutoMapperParm { Destination = requestRe, Source = model });
                    requestRe.ModifiedTime = DateTime.Now;
                    requestRe.ModifiedUser = UserName;
                    _hrUnitOfWork.DisciplineRepository.Attach(requestRe);
                    _hrUnitOfWork.DisciplineRepository.Entry(requestRe).State = EntityState.Modified;
                }
            }

            // inserted records

            if (grid1.inserted != null)
            {
                foreach (DisplinRepeatViewModel model in grid1.inserted)
                {
                    var requestRe = new DisplinRepeat();
                    AutoMapper(new Models.AutoMapperParm { Destination = requestRe, Source = model });
                    requestRe.Discipline = Displinobj;
                    requestRe.CreatedTime = DateTime.Now;
                    requestRe.CreatedUser = UserName;
                    _hrUnitOfWork.DisciplineRepository.Add(requestRe);
                }
            }

            return errors;
        }
        private List<Error> SaveGrid1(RequestDisplinRangeGrid grid1, IEnumerable<KeyValuePair<string, ModelState>> state, DisplinPeriod Displinobj)
        {
            List<Error> errors = new List<Error>();
            // Deleted
            if (grid1.deleted != null)
            {
                foreach (DisplinRangeViewModel model in grid1.deleted)
                {
                    var RequestDisplinRange = new DisplinRange
                    {
                        Id = model.Id
                    };

                    _hrUnitOfWork.DisciplineRepository.Remove(RequestDisplinRange);
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
                        ObjectName = "DisplinRanges",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });
                    if (errors.Count() > 0) return errors;
                }
            }

            // updated records
            if (grid1.updated != null)
            {
                foreach (DisplinRangeViewModel model in grid1.updated)
                {
                    var requestRange = new DisplinRange();
                    AutoMapper(new Models.AutoMapperParm { Destination = requestRange, Source = model });
                    requestRange.ModifiedTime = DateTime.Now;
                    requestRange.ModifiedUser = UserName;
                    _hrUnitOfWork.DisciplineRepository.Attach(requestRange);
                    _hrUnitOfWork.DisciplineRepository.Entry(requestRange).State = EntityState.Modified;
                }
            }

            // inserted records

            if (grid1.inserted != null)
            {
                foreach (DisplinRangeViewModel model in grid1.inserted)
                {
                    var requestRange = new DisplinRange();
                    AutoMapper(new Models.AutoMapperParm { Destination = requestRange, Source = model });
                    requestRange.DisplinPeriod = Displinobj;
                    requestRange.CreatedTime = DateTime.Now;
                    requestRange.CreatedUser = UserName;
                    _hrUnitOfWork.DisciplineRepository.Add(requestRange);
                }
            }

            return errors;
        }
        public ActionResult EmpDisciplineIndex()
        {
            ViewBag.DiscplinId = _hrUnitOfWork.Repository<Discipline>().Select(a => new { text = a.Name, value = a.Id }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();

            return View();
        }
        //ReadEmpDiscipline
        public ActionResult ReadEmpDiscipline(int MenuId)
        {
            var query = _hrUnitOfWork.DisciplineRepository.ReadEmpDiscipline(Language,CompanyId);
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
        public ActionResult DeleteDiscipline(int id)
        {
            DataSource<DisciplineViewModel> Source = new DataSource<DisciplineViewModel>();
            Discipline Displine = _hrUnitOfWork.DisciplineRepository.GetDiscipline(id);
            if (Displine != null)
                _hrUnitOfWork.DisciplineRepository.RemoveDiscipline(Displine.Id);
            var DeletedPeriods = _hrUnitOfWork.Repository<DisplinRepeat>().Where(a => a.DisplinId == id).ToList();
            if (DeletedPeriods != null)
                _hrUnitOfWork.DisciplineRepository.RemoveRange(DeletedPeriods);

            string message = "OK";

            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else

                return Json(message);
        }
        public ActionResult EmpDisciplineDetails(int id = 0,byte Version = 0 )
        {
            string culture = Language;
            var empId= _hrUnitOfWork.Repository<EmpDiscipline>().Where(a => a.Id == id).Select(a => a.EmpId).FirstOrDefault();
            ViewBag.InvestigatId = _hrUnitOfWork.Repository<Investigation>().Select(a => new { id = a.Id, name = a.Name }).ToList();
            // ViewBag.DisplinType = _hrUnitOfWork.DisciplineRepository.FillDLLDesplin(Language);
            List<string> columns = _hrUnitOfWork.PeopleRepository.GetAutoCompleteColumns("EmpDisciplines", CompanyId, Version);
           // if (columns.Where(fc => fc == "EmpId" || fc == "Manager").Count() < 2)
                ViewBag.EmpId = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, empId, CompanyId).Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }).Distinct().ToList();
            ViewBag.DiscplinId = _hrUnitOfWork.DisciplineRepository.SysDiscipline().Select(a => new { name = a.Name, id = a.Id, Systype = a.SysType }).ToList();
            if (id == 0)
                return View(new EmpDisciplineFormViewModel());
            var EmpDiscipline = _hrUnitOfWork.DisciplineRepository.ReadEmployeeDiscipline(id);
            ViewBag.EmpDisplineObj = EmpDiscipline;
            return EmpDiscipline == null ? (ActionResult)HttpNotFound() : View(EmpDiscipline);
        }
        public ActionResult DeleteEmpDiscipline(int id)
        {
            List<Error> errors = new List<Error>();
            DataSource<EmpDisciplineViewModel> Source = new DataSource<EmpDisciplineViewModel>();
            EmpDiscipline empDesplin = _hrUnitOfWork.DisciplineRepository.GetEmpDisplin(id);
            _hrUnitOfWork.DisciplineRepository.RemoveEmpDisplin(empDesplin.Id);

            string message = "OK";
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else

                return Json(message);
        }
        public ActionResult SaveEmpDiscipline(EmpDisciplineFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            //_hrUnitOfWork.Repository<DisPeriodNo>().Where(a=>a.)
          
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpDisciplines",
                        TableName = "EmpDisciplines",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
                var record = _hrUnitOfWork.Repository<EmpDiscipline>().FirstOrDefault(j => j.Id == model.Id);
                if (record == null) //Add
                {
                    record = new EmpDiscipline();
                    model.Witness = model.IWitness == null ? null : string.Join(",", model.IWitness.ToArray());
                    moreInfo.VisibleColumns.Add("Witness");
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "EmpDisciplines",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo
                    });
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    record.CompanyId = CompanyId;

                    if (record.Manager == record.EmpId)
                    {
                        ModelState.AddModelError("Manager", MsgUtils.Instance.Trls("MangerIsSameEmployee"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    if (record.ViolDate > record.DescionDate)
                    {
                        ModelState.AddModelError("DescionDate", MsgUtils.Instance.Trls("DescionMustGrtViolDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    

                    _hrUnitOfWork.DisciplineRepository.Add(record);

                }
                else //update
                {
                    model.Witness = model.IWitness == null ? null : string.Join(",", model.IWitness.ToArray());
                    moreInfo.VisibleColumns.Add("Witness");

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "EmpDisciplines",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo
                    });
                    if (record.Manager == record.EmpId)
                    {
                        ModelState.AddModelError("Manager", MsgUtils.Instance.Trls("MangerIsSameEmployee"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    if (record.ViolDate > record.DescionDate)
                    {
                        ModelState.AddModelError("DescionDate", MsgUtils.Instance.Trls("DescionMustGrtViolDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                   // record.CompanyId = CompanyId;
                    moreInfo.VisibleColumns.Add("Jobs");
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    _hrUnitOfWork.DisciplineRepository.Attach(record);
                    _hrUnitOfWork.DisciplineRepository.Entry(record).State = EntityState.Modified;

                }

                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(record));
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        public ActionResult GetViolationInfo(int InvestigatId)
        {
            var query = _hrUnitOfWork.Repository<Investigation>().Where(a => a.Id == InvestigatId).FirstOrDefault();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmpInvestigation(int EmpId)
        {
            var InvestigateIds = _hrUnitOfWork.Repository<InvestigatEmp>().Where(a => a.EmpId == EmpId).Select(a => a.InvestigatId).ToList();
            var ListOfInvestigations = _hrUnitOfWork.Repository<Investigation>().Where(a => InvestigateIds.Contains(a.Id)).Select(a => new { id = a.Id, name = a.Name }).ToList();
            return Json(ListOfInvestigations, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadDisplinRange(int PeriodId)
        {
            return Json(_hrUnitOfWork.DisciplineRepository.ReadDisplinRange(PeriodId), JsonRequestBehavior.AllowGet);

        }
        //GetDesplinRepeat
        public ActionResult GetDesplinInfo(string violationDate , int desplinId)
        {
            var x = _hrUnitOfWork.DisciplineRepository.GetDesplinInfo(violationDate, desplinId,Language);
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        //GetDisplinDDl
        public ActionResult GetDisplinDDl( int desplinId)
        {
            var x = _hrUnitOfWork.DisciplineRepository.GetDisplinDDl( desplinId, Language);
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmpPointsIndex()
        {
            return View();
        }
        //Ajax to ReadPeriods
        public ActionResult ReadPeriods()
        {
            return Json(_hrUnitOfWork.DisciplineRepository.ReadPeriods(), JsonRequestBehavior.AllowGet);
        }
        //ReadEmloyeePoints
        public ActionResult ReadEmloyeePoints(int periodId)
        {
            return Json(_hrUnitOfWork.DisciplineRepository.ReadEmployeePoints(periodId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult postPoints(IEnumerable<EmployeePointsViewModel> newRecords)
        {
            var datasource = new DataSource<EmployeePointsViewModel>();

            if (newRecords != null)
            {
                var result = new List<EmpPoints>();
                datasource.Data = newRecords;
                datasource.Total = newRecords.Count();

                if (ModelState.IsValid)
                {
                    if (ServerValidationEnabled)
                    {
                        var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                        {
                            CompanyId = CompanyId,
                            ObjectName = "EmployeePoints",
                            TableName = "EmployeePoints",
                            Columns = Models.Utils.GetModifiedRows(ModelState),
                            Culture = Language
                        });

                        if (errors.Count() > 0)
                        {
                            datasource.Errors = errors;
                            return Json(datasource);
                        }
                    }

                    foreach (EmployeePointsViewModel p in newRecords)
                    {
                        var EmpPoint = new EmpPoints
                        {
                            Balance = (int)p.Balance,
                            EmpId = p.EmpId,
                            PeriodId = p.PeriodId,
                            CreatedTime = System.DateTime.Now,
                            CreatedUser = UserName
                        };

                        result.Add(EmpPoint);
                        _hrUnitOfWork.DisciplineRepository.Add(EmpPoint);
                    }
                    int periodid = newRecords.Select(a => a.PeriodId).FirstOrDefault();


                    var PeriodNoobj = _hrUnitOfWork.Repository<DisPeriodNo>().Where(a => a.Id == periodid).FirstOrDefault();
                    PeriodNoobj.Posted = true;
                    _hrUnitOfWork.DisciplineRepository.Attach(PeriodNoobj);
                    _hrUnitOfWork.DisciplineRepository.Entry(PeriodNoobj).State = EntityState.Modified;

                    datasource.Errors = SaveChanges(Language);

                }
                else
                {
                    datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                }

                if (datasource.Errors.Count() > 0)
                    return Json(datasource);
                else
                    return Json(datasource);
            }
            return Json(datasource);
        }

        #region Investigation 
        public ActionResult InvestigationIndex()
        {
            ViewBag.investigations = _hrUnitOfWork.Repository<Discipline>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult ReadInvestigationDiscipline(int MenuId)
        {
            var query = _hrUnitOfWork.DisciplineRepository.ReadInvestigation(Language, CompanyId);
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
        public ActionResult InvestigationDetails(int id = 0, byte Version = 0)
        {
            string culture = Language;
            ViewBag.DiscplinId = _hrUnitOfWork.DisciplineRepository.SysDiscipline().Select(a => new { name = a.Name, id = a.Id}).ToList();         
            ViewBag.EmpId = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }).Distinct().ToList();         
            if (id == 0)
                return View(new InvestigationFormViewModel());
           var Investigation = _hrUnitOfWork.DisciplineRepository.ReadInvestigations(id);

           return Investigation == null ? (ActionResult)HttpNotFound() : View(Investigation);
        }
        public ActionResult SaveEmpInvestigation(InvestigationFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmployeeInvestigatationForm",
                        TableName = "Investigations",
                        Columns = Models.Utils.GetColumnViews(ModelState),
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
                List<int> chkList = new List<int>();
                chkList.AddRange(model.Employee);
                chkList.AddRange(model.Witness);
                chkList.AddRange(model.Judge);
                if(chkList.Count != chkList.Distinct().Count())
                {                    
                        ModelState.AddModelError("", MsgUtils.Instance.Trls("EmployeeWitnessJudge"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));                   
                }
                var record = _hrUnitOfWork.Repository<Investigation>().FirstOrDefault(j => j.Id == model.Id);
                if (record == null) //Add
                {
                    record = new Investigation();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "EmployeeInvestigatationForm",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    record.CompanyId = CompanyId;
                    foreach (var item in model.Employee)
                    {                        
                            AddInvetigateEmp(model.Id, item, 1);                                         
                    }
                    foreach (var item in model.Witness)
                    {                        
                            AddInvetigateEmp(model.Id, item, 2);                        
                    }
                    foreach (var item in model.Judge)
                    {                                                                  
                            AddInvetigateEmp(model.Id, item, 3);                        
                    }
                    _hrUnitOfWork.DisciplineRepository.Add(record);

                }
                else //update
                {
                   
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "EmployeeInvestigatationForm",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    var query = _hrUnitOfWork.Repository<InvestigatEmp>().Where(a => a.InvestigatId == model.Id).Select(a => new {empID= a.EmpId,type= a.EmpType }).ToList();
                    foreach (var item in model.Employee)
                    {
                        var chk = query.Any(a => a.empID == item && a.type == 1);
                        if(!chk)
                        AddInvetigateEmp(model.Id, item, 1);                               
                    }
                    foreach(var item in query.Where(a=>a.type==1))
                    {
                        bool isInList = model.Employee.Contains(item.empID);
                        if(!isInList)
                        {
                            RemoveEmpInvestigate(model.Id, item.empID, 1);
                        }                          
                    }
                    foreach (var item in model.Witness)
                    {
                        var chk = query.Any(a => a.empID == item && a.type == 2);
                        if (!chk)
                            AddInvetigateEmp(model.Id, item, 2);
                    }
                    foreach (var item in query.Where(a => a.type == 2))
                    {
                        bool isInList = model.Witness.Contains(item.empID);
                        if (!isInList)
                        {
                            RemoveEmpInvestigate(model.Id, item.empID, 2);
                        }
                    }
                    foreach (var item in model.Judge)
                    {
                        var chk = query.Any(a => a.empID == item && a.type == 3);
                        if (!chk)
                            AddInvetigateEmp(model.Id, item, 3);
                    }
                    foreach (var item in query.Where(a => a.type == 3))
                    {
                        bool isInList = model.Judge.Contains(item.empID);
                        if (!isInList)
                        {
                            RemoveEmpInvestigate(model.Id, item.empID, 3);
                        }
                    }
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    _hrUnitOfWork.DisciplineRepository.Attach(record);
                    _hrUnitOfWork.DisciplineRepository.Entry(record).State = EntityState.Modified;

                }
                if (errors.Count > 0) return Json(errors.First().errors.First().message);
                var Errors = SaveChanges(Language);
                string message = "OK," + ((new JavaScriptSerializer()).Serialize(record));
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }
        private void AddInvetigateEmp(int investigateId, int EmpId ,byte EmpType )
        {
            var investigateEmp = new InvestigatEmp
            {
                InvestigatId = investigateId,
                EmpId = EmpId,
                EmpType = EmpType
            };
            _hrUnitOfWork.DisciplineRepository.Add(investigateEmp);
        }
        private void RemoveEmpInvestigate(int investigateId, int EmpId, byte EmpType)
        {
            var investigateEmp = _hrUnitOfWork.Repository<InvestigatEmp>().Where(a => a.InvestigatId == investigateId && a.EmpId == EmpId && a.EmpType == EmpType).FirstOrDefault();
            _hrUnitOfWork.DisciplineRepository.Remove(investigateEmp);
        }
        public ActionResult DeleteInvestigation(int id)
        {
            List<Error> errors = new List<Error>();
            DataSource<InvesigationIndexViewModel> Source = new DataSource<InvesigationIndexViewModel>();
            Investigation empInvestigation = _hrUnitOfWork.DisciplineRepository.GetEmpInvestigation(id);
            _hrUnitOfWork.DisciplineRepository.Remove(empInvestigation);
            string message = "OK";
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else
                return Json(message);
        }

        #endregion

    }


}