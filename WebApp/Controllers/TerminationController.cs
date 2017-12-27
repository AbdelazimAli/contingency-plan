using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Interface.Core;
using WebApp.Extensions;
using Model.Domain;
using Model.ViewModel.Personnel;
using Model.ViewModel;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class TerminationController : BaseController
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
        public TerminationController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: Termination
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetRequests(int MenuId)
        {
            var query = _hrUnitOfWork.TerminationRepository.ReadTermRequests(CompanyId, Language);
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
            return Json(query,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int Id=0, byte Version=0)
        {
            var term = _hrUnitOfWork.TerminationRepository.ReadTermination(Id, Language);
            List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("TermRequestForm", CompanyId, Version);
            if (columns.FirstOrDefault(fc => fc == "EmpId") == null)
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetTermActiveEmployees(Language, term.EmpId, CompanyId).Distinct().Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus });
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();

            var Emp = _hrUnitOfWork.PeopleRepository.GetEmployment(term.EmpId);
            int[] arr = new int[] { 1, 2, 4 };
            ViewBag.Employment = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", Language).Where(a => a.SysCodeId == (arr.Contains(Emp.PersonType) ? 3 : 6)).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            ViewBag.AssignStatus = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Assignment", Language).Where(a => a.SysCodeId == 3 ).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            return View(term);
        }
        public ActionResult GetJoined(int Id, int? LeveId)
        {
            var type = _PersonSetup.WorkServMethod;
            var Emp =_hrUnitOfWork.Repository<Employement>().Where(a => a.Status==1 && a.CompanyId == CompanyId && a.EmpId == Id).Select(b => b).FirstOrDefault();
            var date = Emp.StartDate;
            if (type == 1)
            {
                var newdate = _hrUnitOfWork.Repository<Person>().Where(a => a.Id == Id).Select(b => b.JoinDate == null ? new DateTime(2999, 1, 1) : b.JoinDate.Value).FirstOrDefault();
                if (newdate != new DateTime(2999, 1, 1))
                    date = newdate;
            }
            double serveinYears = Math.Round((DateTime.Now.Subtract(date).TotalDays) / 365.25, 5);
            double BonusinMonth = 0;
            if (LeveId != null)
            {
                int syscode = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Termination", Language).Where(a => a.CodeId == LeveId).Select(b => b.SysCodeId).FirstOrDefault();
                var Periods = _hrUnitOfWork.Repository<TermDuration>().Where(a => a.CompanyId == CompanyId && a.TermSysCode == syscode && a.WorkDuration >= Convert.ToByte(serveinYears)).FirstOrDefault();
                if (Periods != null)
                {
                    if (serveinYears > Periods.FirstPeriod)
                        BonusinMonth = Math.Round((Periods.FirstPeriod * Periods.Percent1) + (serveinYears - Periods.FirstPeriod) * (Periods.Percent2 == null ? 0 : Periods.Percent2.Value), 5);
                    else
                        BonusinMonth = Math.Round((serveinYears * Periods.Percent1), 5);
                }
            }
            int[] arr = new int[] { 1, 2, 4 };
            var EmpStatus = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", Language).Where(a => a.SysCodeId == (arr.Contains(Emp.PersonType) ? 3 : 6)).Select(b => new {id=b.CodeId , name=b.Title }).ToList();

            var myobj = new { date = date, bouns = BonusinMonth, ServYear = serveinYears ,EmpStatus = EmpStatus};
            return Json(myobj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Details(TerminationFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            var message = "OK";

            var Term = _hrUnitOfWork.TerminationRepository.Get(model.Id);
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.TerminationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "TermRequestForm",
                        TableName = "Terminations",
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
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            if (Term == null) // New
            {
                Term = new Termination();

                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = Term,
                    Source = model,
                    ObjectName = "TermRequestForm",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });
                Term.CompanyId = CompanyId;
                Term.CreatedTime = DateTime.Now;
                Term.CreatedUser = UserName;
                Term.RequestDate = DateTime.Now;

                _hrUnitOfWork.TerminationRepository.Add(Term);
            }
            else // Edit
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = Term,
                    Source = model,
                    ObjectName = "TermRequestForm",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Update
                });
                Term.ModifiedTime = DateTime.Now;
                Term.ModifiedUser = UserName;
                Term.CompanyId = CompanyId;

                _hrUnitOfWork.TerminationRepository.Attach(Term);
                _hrUnitOfWork.TerminationRepository.Entry(Term).State = EntityState.Modified;
            }

            //Use 2 SaveChanges in same action to get Id of request to use it in Workflow Transation (WFTrans)
            errors = SaveChanges(Language);
            if (errors.Count > 0)
            {
                message = errors.First().errors.First().message;
                return Json(message);
            }

            if (Term.ApprovalStatus == 2)
            {
                string error = AddWFTrans(Term, null, null);
                if (error.Length > 0)
                    return Json(error);

                var checklist = _hrUnitOfWork.CheckListRepository.GetTermCheckLists(CompanyId);
                    if (checklist != null)
                    {
                        EmpChkList EmpList = _hrUnitOfWork.CheckListRepository.AddEmpChlst(checklist, UserName, Term.EmpId,CompanyId);
                        _hrUnitOfWork.CheckListRepository.Add(EmpList);
                        var checkTask = _hrUnitOfWork.CheckListRepository.ReadCheckListTask(checklist.Id).ToList();
                        if (checkTask.Count > 0)
                        {
                            _hrUnitOfWork.CheckListRepository.AddEmpTask(checkTask, UserName, EmpList);
                        }

                    }
            }
            errors =  SaveChanges(Language);
            if (errors.Count > 0) message = errors.First().errors.First().message;
            return Json(message);
        }
        public ActionResult DeleteRequest(int Id)
        {
            DataSource<TerminationGridViewModel> Source = new DataSource<TerminationGridViewModel>();
            
            string message = "OK";
            _hrUnitOfWork.TerminationRepository.DeleteRequest(Id, CompanyId, Language);

            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
            {
                return Json(Source);
            }
            else
                return Json(message);
        }
        public ActionResult ApproveIndex()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetRequestsApprove(int MenuId)
        {
            var query = _hrUnitOfWork.TerminationRepository.ReadTermsApproved(CompanyId, Language);
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
        public ActionResult ApproveDetails(int Id)
        {
            var approveTerm = _hrUnitOfWork.TerminationRepository.ReadTermination(Id, Language);
            ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language,approveTerm.EmpId, CompanyId).Distinct().Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }); 
           
            ViewBag.CancelReason = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("CancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });


            return View(approveTerm);
        }
        [HttpPost]
        public ActionResult ApprovalDetails(TerminationFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            string message = "OK";
            var Term = _hrUnitOfWork.TerminationRepository.Get(model.Id);
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.TerminationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpTermForm",
                        TableName = "Terminations",
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
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }


            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = Term,
                    Source = model,
                    ObjectName = "EmpTermForm",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Update
                });
                Term.ModifiedTime = DateTime.Now;
                Term.ModifiedUser = UserName;

                _hrUnitOfWork.TerminationRepository.Attach(Term);
                _hrUnitOfWork.TerminationRepository.Entry(Term).State = EntityState.Modified;
                if (Term.Terminated == true)
                {
                    var EmpChList = _hrUnitOfWork.Repository<EmpChkList>().Where(a => a.EmpId == Term.EmpId && a.CompanyId == CompanyId && a.Status == 0).FirstOrDefault();
                    if (EmpChList != null)
                    {
                        ModelState.AddModelError("", MsgUtils.Instance.Trls("Thereischecopen"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    var Custody = _hrUnitOfWork.Repository<EmpCustody>().Where(a => a.EmpId == Term.EmpId && a.CompanyId == CompanyId && a.delvryDate >= DateTime.Now).ToList();
                    if (Custody.Count > 0)
                    {
                        ModelState.AddModelError("", MsgUtils.Instance.Trls("ThereisCustody"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    var Emploment = _hrUnitOfWork.Repository<Employement>().Where(a => a.EmpId == Term.EmpId && a.Status == 1 && a.StartDate <= DateTime.Today && a.CompanyId == CompanyId).FirstOrDefault();
                    var Assignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == Term.EmpId && (a.AssignDate <= DateTime.Today && a.EndDate > DateTime.Today) && a.CompanyId == CompanyId).FirstOrDefault();
                    var anotherTerminations = _hrUnitOfWork.TerminationRepository.GetAll().Where(a => a.EmpId == Term.EmpId && a.ApprovalStatus < 7).ToList();


                    //Cancel Leave Requests
                    _hrUnitOfWork.LeaveRepository.CancelLeaveRequests(Term.EmpId, UserName, CompanyId, 0, Language);

                    foreach (var item in anotherTerminations)
                    {
                        item.ApprovalStatus = 7;
                        item.ModifiedTime = DateTime.Now;
                        item.ModifiedUser = UserName;
                        _hrUnitOfWork.TerminationRepository.Attach(item);
                        _hrUnitOfWork.TerminationRepository.Entry(item).State = EntityState.Modified;
                    }
                    if (Emploment != null)
                    {

                        _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                        {
                            ColumnName = "PersonType",
                            CompanyId = CompanyId,
                            ObjectName = "Emp",
                            SourceId = Emploment.Id.ToString(),
                            UserName = UserName,
                            Version = Convert.ToByte(Request.Form["Version"]),
                            ValueAfter = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", Language).Where(a => a.CodeId == Term.PersonType).Select(b => b.Title).FirstOrDefault(),
                            ValueBefore = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", Language).Where(a => a.CodeId == Emploment.PersonType).Select(b => b.Title).FirstOrDefault()
                        });
                        _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                        {
                            ColumnName = "Status",
                            CompanyId = CompanyId,
                            ObjectName = "Emp",
                            SourceId = Emploment.Id.ToString(),
                            UserName = UserName,
                            Version = Convert.ToByte(Request.Form["Version"]),
                            ValueAfter = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Status", Language).Where(a => a.CodeId == 3).Select(b => b.Title).FirstOrDefault(),
                            ValueBefore = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Status", Language).Where(a => a.CodeId == Emploment.Status).Select(b => b.Title).FirstOrDefault()
                        });
                        _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                        {
                            ColumnName = "TerminationId",
                            CompanyId = CompanyId,
                            ObjectName = "Emp",
                            SourceId = Emploment.Id.ToString(),
                            UserName = UserName,
                            Version = Convert.ToByte(Request.Form["Version"]),
                            ValueAfter = Term.Id.ToString(),
                            ValueBefore = Emploment.TerminationId.ToString()
                        });


                        Emploment.PersonType = Term.PersonType;
                        Emploment.Status = 3;
                        Emploment.TerminationId = Term.Id;
                        Emploment.EndDate = Term.ActualDate;
                        Emploment.ModifiedTime = DateTime.Now;
                        Emploment.ModifiedUser = UserName;

                        _hrUnitOfWork.PeopleRepository.Attach(Emploment);
                        _hrUnitOfWork.PeopleRepository.Entry(Emploment).State = EntityState.Modified;

                    }
                    if (Assignment != null)
                    {
                        _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                        {
                            ColumnName = "AssignStatus",
                            CompanyId = CompanyId,
                            ObjectName = "AssignmentsForm",
                            SourceId = Assignment.Id.ToString(),
                            UserName = UserName,
                            Version = Convert.ToByte(Request.Form["Version"]),
                            ValueAfter = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Assignment", Language).Where(a => a.CodeId == Assignment.AssignStatus).Select(b => b.Title).FirstOrDefault(),
                            ValueBefore = Emploment.TerminationId.ToString()
                        });


                        var OtheAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == Term.EmpId && a.Id != Assignment.Id && a.AssignDate > DateTime.Today && a.CompanyId == CompanyId).ToList();
                        foreach (var item in OtheAssignment)
                        {
                            _hrUnitOfWork.EmployeeRepository.Remove(item);
                        }

                        Assignment.EndDate = Term.ActualDate != null ? Term.ActualDate.Value.AddDays(-1) : DateTime.Now.AddDays(-1);
                        Assignment.ModifiedTime = DateTime.Now;
                        Assignment.ModifiedUser = UserName;
                        _hrUnitOfWork.EmployeeRepository.Attach(Assignment);
                        _hrUnitOfWork.EmployeeRepository.Entry(Assignment).State = EntityState.Modified;

                        Assignment NewAssign = new Assignment();
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = NewAssign,
                            Source = Assignment,
                            ObjectName = "AssignmentsForm",
                            Version = Convert.ToByte(Request.Form["Version"]),
                            Options = null,
                            Transtype = TransType.Insert
                        });

                        NewAssign.AssignStatus = Term.AssignStatus;
                        NewAssign.SysAssignStatus = 3;
                        NewAssign.AssignDate = Term.ActualDate != null ? Term.ActualDate.Value : DateTime.Now;
                        NewAssign.EndDate = Term.ActualDate != null ? Term.ActualDate.Value : DateTime.Now;
                        NewAssign.CreatedTime = DateTime.Now;
                        NewAssign.CreatedUser = UserName;
                        _hrUnitOfWork.EmployeeRepository.Add(NewAssign);

                    }
                }
            }

            errors = SaveChanges(Language);
            if (errors.Count > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }
        public ActionResult TermFollowUpIndex()
        {
            ViewBag.CanselReasons = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("CancelReason", Language).Select(a => new { id = a.CodeId, name = a.Title });

            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();

            return View();
        }
        public ActionResult GetFollows(int MenuId)
        {
            var query = _hrUnitOfWork.TerminationRepository.ReadTermFollowUp(CompanyId, Language);
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
        public ActionResult TermFollowUpDetails(int Id,byte Version=0)
        {
            var termFollow = _hrUnitOfWork.TerminationRepository.ReadTermination(Id, Language);
            List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("EmpTermFollowUp", CompanyId, Version);
            if (columns.FirstOrDefault(fc => fc == "EmpId") == null)
                ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language,termFollow.EmpId, CompanyId).Distinct().Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus });

            return View(termFollow);

        }
        [HttpPost]
        public ActionResult TermFollowUpDetails(TerminationFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            var Term = _hrUnitOfWork.TerminationRepository.Get(model.Id);
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.TerminationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpTermFollowUp",
                        TableName = "Terminations",
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
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }



            if (model.ApprovalStatus != 9)
            {
                model.RejectDesc = null;
                model.RejectReason = null;
            }
            if (model.ApprovalStatus != 5)
                model.ActualDate = null;

                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = Term,
                    Source = model,
                    ObjectName = "EmpTermFollowUp",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype=TransType.Update
                });
                Term.ModifiedTime = DateTime.Now;
                Term.ModifiedUser = UserName;

            if (Term.ApprovalStatus == 5 || Term.ApprovalStatus == 6) //Accept or Rejected
            {
                string error = AddWFTrans(Term, null, null);
                if (error.Length > 0)
                    return Json(error);
            }

            _hrUnitOfWork.TerminationRepository.Attach(Term);
            _hrUnitOfWork.TerminationRepository.Entry(Term).State = EntityState.Modified;

            try
            {
                _hrUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg);
            }

            return Json("OK");
        }
        [HttpPost]
        public ActionResult SendTo(int Id,byte? Send)
        {
            Termination Term = _hrUnitOfWork.TerminationRepository.Find(a => a.Id == Id).FirstOrDefault();
            string error = "";
            if (Send == 3)
            {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel() { ColumnName = "ApprovalStatus", CompanyId = CompanyId, ObjectName = "EmpTermFollowUp", SourceId = Id.ToString(), UserName=UserName, Version = Convert.ToByte(Request.Form["Version"]), ValueAfter= MsgUtils.Instance.Trls("EmployeeReview") ,ValueBefore = MsgUtils.Instance.Trls("Submit") });
                Term.ApprovalStatus = 3;
                error = AddWFTrans(Term, null, true); 
            }
            else
            {
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel() { ColumnName = "ApprovalStatus", CompanyId = CompanyId, ObjectName = "EmpTermFollowUp", SourceId = Id.ToString(), UserName = UserName, Version = Convert.ToByte(Request.Form["Version"]), ValueAfter = MsgUtils.Instance.Trls("ManagerReview"), ValueBefore = MsgUtils.Instance.Trls("Submit") });
                Term.ApprovalStatus = 4;
                error = AddWFTrans(Term, null , null);
            }
            if (error.Length > 0)
                return Json(error);

            _hrUnitOfWork.TerminationRepository.Attach(Term);
            _hrUnitOfWork.TerminationRepository.Entry(Term).State = EntityState.Modified;

            try
            {
                _hrUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg);
            }

            return Json("Ok");
        }
        public ActionResult CancelReq(int Id, short? Reason,string Desc )
        {
            Termination Term = _hrUnitOfWork.TerminationRepository.Find(a => a.Id == Id).FirstOrDefault();
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelReason",
                CompanyId = CompanyId,
                ObjectName = "EmpTermFollowUp",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = 0,
                ValueAfter = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("CancelReason", Language).Where(a => a.CodeId == Reason).Select(b => b.Title).FirstOrDefault(),
                ValueBefore = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("CancelReason", Language).Where(a => a.CodeId == Term.CancelReason).Select(b => b.Title).FirstOrDefault()
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "CancelDesc",
                CompanyId = CompanyId,
                ObjectName = "EmpTermFollowUp",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = Desc,
                ValueBefore = Term.CancelDesc
            });
            _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
            {
                ColumnName = "ApprovalStatus",
                CompanyId = CompanyId,
                ObjectName = "EmpTermFollowUp",
                SourceId = Id.ToString(),
                UserName = UserName,
                Version = Convert.ToByte(Request.Form["Version"]),
                ValueAfter = MsgUtils.Instance.Trls("Cancel before accepted"),
                ValueBefore = MsgUtils.Instance.Trls("Submit")
            });
            Term.CancelDesc = Desc;
            Term.CancelReason = Reason;
            Term.ApprovalStatus = 7;

            string error = AddWFTrans(Term, null, null);
            if (error.Length > 0)
                return Json(error);

            _hrUnitOfWork.TerminationRepository.Attach(Term);
            _hrUnitOfWork.TerminationRepository.Entry(Term).State = EntityState.Modified;

            try
            {
                _hrUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg);
            }

            return Json("Ok");
        }

        private string AddWFTrans(Termination request, int? ManagerId, bool? backToEmp)
        {
            WfViewModel wf = new WfViewModel()
            {
                Source = "Termination",
                SourceId = CompanyId,
                DocumentId = request.Id,
                RequesterEmpId = request.EmpId,
                ApprovalStatus = request.ApprovalStatus,
                CreatedUser = UserName
            };

            if (ManagerId != null) wf.ManagerId = ManagerId;
            else if (backToEmp != null) wf.BackToEmployee = backToEmp.Value;

            var wfTrans = _hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, Language);
            request.WFlowId = wf.WFlowId;
            if (wfTrans == null && wf.WorkFlowStatus != "Success")
                return wf.WorkFlowStatus;
            else if (wfTrans == null && wf.WorkFlowStatus == "Success")
                request.ApprovalStatus = 6;
            else if(wfTrans != null)
                _hrUnitOfWork.LeaveRepository.Add(wfTrans);

            return "";
        }
    }
}