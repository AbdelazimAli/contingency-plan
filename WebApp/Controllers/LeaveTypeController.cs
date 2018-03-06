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
using WebApp.Extensions;
using WebApp.Models;
using Model.Domain.Payroll;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Collections;

namespace WebApp.Controllers
{
    public class LeaveTypeController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        private UserContext db;
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
        public LeaveTypeController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
            db = new UserContext();
        }
        
        #region LeaveType
        public ActionResult Index()
        {
            ViewBag.AbsenceType = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("AbsenceType",Language).Select(a=> new { value=a.CodeId , text=a.Title }); //HrContext.GetLookUpUserCode("AbsenceType", l.AbsenceType, culture),
            return View();
        }
        public ActionResult GetLeaveTypes(int MenuId)
        {
            string culture = Language;

            var query = _hrUnitOfWork.LeaveRepository.GetLeaveTypes(CompanyId, culture);
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

        public ActionResult CheckLeaveCalendar(int id, int calendarId)
        {
            var type = _hrUnitOfWork.Repository<LeaveType>().Where(t => t.Id != id && t.CalendarId == calendarId && t.HasAccrualPlan)
                .Select(t => new { t.CalendarId, t.HasAccrualPlan, t.AbsenceType }).FirstOrDefault();
            
            return Json(type, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteLeaveType(int id)
        {
            var message = "OK";
            DataSource<PositionViewModel> Source = new DataSource<PositionViewModel>();

            LeaveType leaveType = _hrUnitOfWork.LeaveRepository.GetLeaveType(id);
            _hrUnitOfWork.LeaveRepository.Remove(leaveType);
            _hrUnitOfWork.LeaveRepository.RemoveLName(Language, leaveType.Name);

            Source.Errors = SaveChanges(Language);


            //delete period name if doesn't contain data
            if (leaveType.HasAccrualPlan)
            {
                PeriodName periodName = _hrUnitOfWork.Repository<PeriodName>().Where(p => p.Id == leaveType.CalendarId).FirstOrDefault();

                AutoMapper(new Models.AutoMapperParm
                {
                    Source = periodName,
                    ObjectName = "HRCalendar",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.BudgetRepository.Remove(periodName);
            }
            SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
            
        }
        [HttpGet]
        public ActionResult Details(int id=0)
        {
            if (id == 0)
            {
                fillViewBag(id);
                return View(new LeaveTypeFormViewModel());
            }


           var LeaveTypeobj= _hrUnitOfWork.LeaveRepository.ReadleaveType(id,Language);

            fillViewBag(LeaveTypeobj.Id);
            return LeaveTypeobj == null ? (ActionResult)HttpNotFound() : View(LeaveTypeobj);


        }
        public void fillViewBag(int id)
        {
            string culture = Language;
            ViewBag.Jobs = _hrUnitOfWork.JobRepository.GetAllJobs(CompanyId, Language,0).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.Branches = _hrUnitOfWork.BranchRepository.ReadBranches(Language, CompanyId).Select(a => new { id = a.Id, name = a.LocalName});
            ViewBag.CompanyStuctures = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId, null, culture);
            ViewBag.Payrolls = _hrUnitOfWork.Repository<Payrolls>().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.Positions = _hrUnitOfWork.PositionRepository.GetPositions(Language, CompanyId).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.PeopleGroups = _hrUnitOfWork.PeopleRepository.GetPeoples().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.PayrollGrades = _hrUnitOfWork.JobRepository.GetPayrollGrade(CompanyId);

            var PervCalIds =  _hrUnitOfWork.Repository<LeaveType>().Where(a => a.Id != id && a.HasAccrualPlan).Select(a => a.CalendarId).ToList().Distinct();
            var Calender = _hrUnitOfWork.BudgetRepository.GetCalender(CompanyId);
            ViewBag.OpenCalendar = Calender.Where(c => !c.Default).Select(c => new FormList { id = c.Id, name = c.Name }).ToList();
            //ViewBag.HRCalendar = Calender.Where(c => c.Default).Where(c => !PervCalIds.Contains(c.Id)).Select(c => new { c.Id, c.SubPeriodCount }).ToList();

            ViewBag.PayrollId = _hrUnitOfWork.Repository<Payrolls>().Select(p => new { id = p.Id, name = p.Name }).ToList();
            if (culture.Substring(0, 2) == "ar")
                ViewBag.Nationality = _hrUnitOfWork.Repository<Country>().Where(a => a.Nationality != null).Select(a => new { id = a.Id, name = a.NationalityAr }).ToList();
            else
                ViewBag.Nationality = _hrUnitOfWork.Repository<Country>().Where(a => a.Nationality != null).Select(a => new { id = a.Id, name = a.Nationality }).ToList();

           // ViewBag.Nationality = _hrUnitOfWork.Repository<Country>().Where(a => a.Nationality != null).Select(a => new { id = a.Id, name = a.Nationality }).ToList();
            ViewBag.Role = _hrUnitOfWork.LeaveRepository.GetOrgChartRoles(Language).Select(a => new { text = a.text, value = a.value }).ToList();
        }

        private List<Error> SaveGrid2(LeaveRangeVM grid1, IEnumerable<KeyValuePair<string, ModelState>> state, LeaveType leavetype)
        {
            List<Error> errors = new List<Error>();

            // Deleted
            if (grid1.deleted != null)
            {
                foreach (ExcelGridLeaveRangesViewModel model in grid1.deleted)
                {
                    var leaverange = new LeaveRange
                    {
                        Id = model.Id
                    };

                    _hrUnitOfWork.LeaveRepository.Remove(leaverange);
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
                        ObjectName = "LeaveRanges",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0) return errors;
                }
            }

            // updated records
            if (grid1.updated != null)
            {
                foreach (ExcelGridLeaveRangesViewModel model in grid1.updated)
                {
                    var leaverange = new LeaveRange();
                    AutoMapper(new Models.AutoMapperParm { Destination = leaverange, Source = model,Transtype=TransType.Update });
                    leaverange.ModifiedTime = DateTime.Now;
                    leaverange.ModifiedUser = UserName;
                    _hrUnitOfWork.LeaveRepository.Attach(leaverange);
                    _hrUnitOfWork.LeaveRepository.Entry(leaverange).State = EntityState.Modified;
                }
            }

            // inserted records
            if (grid1.inserted != null)
            {
                foreach (ExcelGridLeaveRangesViewModel model in grid1.inserted)
                {
                    var leaverange = new LeaveRange();
                    AutoMapper(new Models.AutoMapperParm { Destination = leaverange, Source = model ,Transtype=TransType.Insert});
                    leaverange.LeaveType = leavetype;
                    leaverange.CreatedTime = DateTime.Now;
                    leaverange.CreatedUser = UserName;
                    _hrUnitOfWork.LeaveRepository.Add(leaverange);
                }
            }

            return errors;
        }

        [HttpPost]
        public ActionResult Details(LeaveTypeFormViewModel model, OptionsViewModel moreInfo, LeaveRangeVM grid1, bool clear)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.SiteRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LeaveType",
                        TableName = "LeaveTypes",
                        ParentColumn = "CompanyId",
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

                PeriodName calendar = new PeriodName { Id = model.CalendarId };
                if (model.HasAccrualPlan) 
                {
                    if (model.CalendarId == 0)
                    {
                        string error;
                        calendar = AddPeriodName(model.Name, model.FrequencyId.Value, model.IsLocal, out error);
                        if(!string.IsNullOrEmpty(error))
                        {
                            ModelState.AddModelError("FrequencyId", error);
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }
                }

                LeaveType record;
                //insert
                if (model.Id == 0)
                {
                    record = new LeaveType();
                    MapLeaveType(record, model,moreInfo);
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    if (record.HasAccrualPlan) record.CalendarId = calendar.Id;
                    _hrUnitOfWork.LeaveRepository.Add(record);
                }
                //update
                else
                {
                    record = _hrUnitOfWork.Repository<LeaveType>().FirstOrDefault(a => a.Id == model.Id);
                    MapLeaveType(record, model, moreInfo);
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;

                    if (record.StartDate > record.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("MustGreaterthanStart"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }

                    if (record.HasAccrualPlan) record.CalendarId = calendar.Id;
                    _hrUnitOfWork.LeaveRepository.Attach(record);
                    _hrUnitOfWork.LeaveRepository.Entry(record).State = EntityState.Modified;
                }

                // Save grid2
                errors = SaveGrid2(grid1, ModelState.Where(a => a.Key.Contains("grid1")), record);
                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                errors = SaveChanges(Language);

                if (clear)
                    model = new LeaveTypeFormViewModel();
                else
                    model.Id = record.Id;

                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                return Json("OK," + ((new JavaScriptSerializer()).Serialize(model)));
            }

            return Json(Models.Utils.ParseFormErrors(ModelState));
        }

        private PeriodName AddPeriodName(string name, byte subPeriodCount, bool isLocal, out string error)
        {

            var calendar = new PeriodName
            {
                Name = MsgUtils.Instance.Trls("Accural Period") + " " + name,
                StartDate = DateTime.Today,
                EndDate = new DateTime(2999, 1, 1),
                IsLocal = isLocal,
                CompanyId = isLocal ? CompanyId : (int?)null,
                PeriodLength = 1,
                SubPeriodCount = subPeriodCount,
                CreatedUser = UserName,
                CreatedTime = DateTime.Now,
                SingleYear = true
            };

            var periodNames = _hrUnitOfWork.Repository<PeriodName>().Where(p => p.Name.StartsWith(calendar.Name)).Select(p => p.Name).ToList();
            string pname = null;
            if (periodNames.Count > 0)
            {
                pname = periodNames.Where(p => p == calendar.Name).FirstOrDefault();
                while (pname != null)
                {
                    calendar.Name += "1";
                    pname = periodNames.Where(p => p == calendar.Name).FirstOrDefault();
                }
            }
            _hrUnitOfWork.BudgetRepository.Add(calendar);
            error = _hrUnitOfWork.BudgetRepository.GenerateAccuralPeriods(calendar, UserName, Language);

            return calendar;
        }

        private void MapLeaveType(LeaveType leaveobject, LeaveTypeFormViewModel model, OptionsViewModel moreInfo)
        {
            model.Branches = model.IBranches == null ? null : string.Join(",", model.IBranches.ToArray());
            model.Jobs = model.IJobs == null ? null : string.Join(",", model.IJobs.ToArray());
            model.Employments = model.IEmployments == null ? null : string.Join(",", model.IEmployments.ToArray());
            model.PeopleGroups = model.IPeopleGroups == null ? null : string.Join(",", model.IPeopleGroups.ToArray());
            model.Payrolls = model.IPayrolls == null ? null : string.Join(",", model.IPayrolls.ToArray());
            model.PayrollGrades = model.IPayrollGrades == null ? null : string.Join(",", model.IPayrollGrades.ToArray());
            model.CompanyStuctures = model.ICompanyStuctures == null ? null : string.Join(",", model.ICompanyStuctures.ToArray());
            model.Positions = model.IPositions == null ? null : string.Join(",", model.IPositions.ToArray());
            moreInfo.VisibleColumns.Add("Branches");
            moreInfo.VisibleColumns.Add("Jobs");
            moreInfo.VisibleColumns.Add("Employments");
            moreInfo.VisibleColumns.Add("PeopleGroups");
            moreInfo.VisibleColumns.Add("Payrolls");
            moreInfo.VisibleColumns.Add("PayrollGrades");
            moreInfo.VisibleColumns.Add("CompanyStuctures");
            moreInfo.VisibleColumns.Add("Positions");
            _hrUnitOfWork.LeaveRepository.AddLName(Language, leaveobject.Name, model.Name, model.LocalName);
            AutoMapper(new Models.AutoMapperParm
            {
                Destination = leaveobject,
                Source = model,
                ObjectName = "LeaveType",
                Options = moreInfo
            });
            //MonthOrYear: 2-year, AccBalDays: 1-Fixed days
            leaveobject.MonthOrYear = (byte)(model.AccBalDays == 1 || !model.HasAccrualPlan ? 0 : model.MonthOrYear ?? 2);
            if (model.ChangAssignStat == false)
            {
                leaveobject.AutoChangStat = null;
                leaveobject.AssignStatus = null;
            }
            if (model.AccBalDays != 1)
                leaveobject.NofDays = null;
            if (model.AllowNegBal == false)
                leaveobject.Percentage = null;
            if (model.Balanace50 == false)
            {
                leaveobject.Age50 = null;
                leaveobject.NofDays50 = null;
            }
            //postOption Sec
             if (model.PostOpt == 1)
            {
                leaveobject.MaxNofDays = null;
                leaveobject.MaxPercent = null;
                leaveobject.DiffDaysOpt = null;
            }
            else if(model.PostOpt==2)
                leaveobject.MaxPercent = null;
            else if (model.PostOpt == 3)
                leaveobject.MaxNofDays = null;
            else 
            {
                leaveobject.MaxNofDays = null;
                leaveobject.MaxPercent = null;
                leaveobject.DiffDaysOpt = null;//
                leaveobject.MinLeaveDays = null;
                leaveobject.IncludContinu = null;
            }

            if (model.HasAccrualPlan == false)
            {
                leaveobject.AccBalDays = null;
                leaveobject.NofDays = null;
                leaveobject.Balanace50 = false;
                leaveobject.Age50 = null;
                leaveobject.NofDays50 = null;
                leaveobject.WorkServMethod = null;
                leaveobject.PostOpt = null;
                leaveobject.MaxNofDays = null;
                leaveobject.MaxPercent = null;
            }
            else
            {
                leaveobject.MaxDaysInPeriod = null;
            }
            leaveobject.Percentage = model.Percentage / 100;
            leaveobject.MaxPercent = model.MaxPercent / 100;
        }
        #endregion

        #region LevelRange
        public ActionResult ReadLeaveRange(int LeaveTypeId)
        {
            return Json(_hrUnitOfWork.LeaveRepository.GetLeaveRange(LeaveTypeId), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region RequestWf
        public ActionResult ReadWfRole(int RequestWfId)
        {
            return Json(_hrUnitOfWork.LeaveRepository.GetWfRole(RequestWfId),JsonRequestBehavior.AllowGet);
        }

        [OutputCache(VaryByParam = "*", Duration = 60)]
        public ActionResult WorkFlow(string source, int sourceid)
        {
            var reqestwf = _hrUnitOfWork.LeaveRepository.ReadRequestWF(sourceid, source, Language);
            if (reqestwf == null)
            {
                reqestwf = new RequestWfFormViewModel();
                reqestwf.Source = source;
                reqestwf.SourceId = sourceid;
                reqestwf.Roles = _hrUnitOfWork.LeaveRepository.GetOrgChartRoles(Language);
                reqestwf.Diagrams = _hrUnitOfWork.Repository<Diagram>().Select(a => new FormDropDown { id = a.Id, name = a.Name }).ToList();
            }
           
            ViewBag.HeirTypeList = new SelectList(new List<FormDropDown> {
                        new FormDropDown { id = 1, name = MsgUtils.Instance.Trls("Org Chart") },
                        new FormDropDown { id = 2, name = MsgUtils.Instance.Trls("Org Chart Hierarchy") },
                        new FormDropDown { id = 3, name = MsgUtils.Instance.Trls("Position Hierarchy") },
                        new FormDropDown { id = 4, name = MsgUtils.Instance.Trls("Direct Manager Hierarchy")}
            },"id", "name", reqestwf.HeirType);

            ViewBag.WaitAction = new SelectList(new List<FormDropDown> {
                        new FormDropDown { id = 1, name = MsgUtils.Instance.Trls("Foreword to next step") },
                        new FormDropDown { id = 2, name = MsgUtils.Instance.Trls("Back to previous step") },
                        new FormDropDown { id = 3, name = MsgUtils.Instance.Trls("Back to requester") }
            }, "id", "name", reqestwf.TimeOutAction);

            return PartialView("_WorkFlow", reqestwf);
        }
        public ActionResult RequestDetails(RequestWfFormViewModel model, OptionsViewModel moreInfo, WfRoleWfVM grid1)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.SiteRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "RequestWfs",
                        TableName = "RequestWf",
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

                RequestWf record;
                //insert
                if (model.Id == 0)
                {
                    record = new RequestWf();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "RequestWfs",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    if (model.HeirType != 3)
                        record.Hierarchy =null;

                    _hrUnitOfWork.LeaveRepository.Add(record);
                }
                //update
                else
                {
                    record = _hrUnitOfWork.Repository<RequestWf>().FirstOrDefault(a => a.Id == model.Id);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "RequestWfs",
                        Version = 0,
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    if (model.HeirType != 3)
                        record.Hierarchy = null;
                    _hrUnitOfWork.LeaveRepository.Attach(record);
                    _hrUnitOfWork.LeaveRepository.Entry(record).State = EntityState.Modified;
                }

                errors = SaveGrid1(grid1, ModelState.Where(a => a.Key.Contains("grid1")), record);
                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                errors = SaveChanges(Language);
                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                model.Id = record.Id;
                
                // clear cache
                Response.RemoveOutputCacheItem(Url.Action("WorkFlow", "LeaveType"));

                return Json("OK," + ((new JavaScriptSerializer()).Serialize(model)));
            }

            return Json(Models.Utils.ParseFormErrors(ModelState));
        }
        private List<Error> SaveGrid1(WfRoleWfVM grid1, IEnumerable<KeyValuePair<string, ModelState>> state, RequestWf Request)
        {
            List<Error> errors = new List<Error>();
            // Deleted
            if (grid1.deleted != null)
            {
                foreach (WfRoleViewModel model in grid1.deleted)
                {
                    var wfrole = new WfRole { Id = model.Id };
                    _hrUnitOfWork.LeaveRepository.Remove(wfrole);
                }
            }

            if (ServerValidationEnabled)
            {
                var modified = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted")));
                if (modified.Count > 0)
                {
                    errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "WfRole",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0) return errors;
                }
            }

            // updated records
            if (grid1.updated != null)
            {
                foreach (WfRoleViewModel model in grid1.updated)
                {
                    var wfrole = new WfRole();
                    AutoMapper(new Models.AutoMapperParm { Destination = wfrole, Source = model ,Transtype=TransType.Update });
                    wfrole.ModifiedTime = DateTime.Now;
                    wfrole.ModifiedUser = UserName;
                    _hrUnitOfWork.LeaveRepository.Attach(wfrole);
                    _hrUnitOfWork.LeaveRepository.Entry(wfrole).State = EntityState.Modified;
                }
            }

            // inserted records
            if (grid1.inserted != null)
            {
                foreach (WfRoleViewModel model in grid1.inserted)
                {
                    var wfrole = new WfRole();
                    AutoMapper(new Models.AutoMapperParm { Destination = wfrole, Source = model ,Transtype=TransType.Insert });
                    wfrole.WorkFlow = Request;
                    wfrole.CreatedTime = DateTime.Now;
                    wfrole.CreatedUser = UserName;
                    _hrUnitOfWork.LeaveRepository.Add(wfrole);
                }
            }

            return errors;
        }
        #endregion
    }
}