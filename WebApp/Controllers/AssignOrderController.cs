
using Interface.Core;
using Model.ViewModel.Personnel;
using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Extensions;
using WebApp.Models;
using System.Linq.Dynamic;
using Model.ViewModel;
using System.Web.Script.Serialization;
using System.Data.Entity;

namespace WebApp.Controllers
{
    public class AssignOrderController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
       
        public AssignOrderController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: AssignOrder
        public ActionResult Index(int id = 0)
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            ViewBag.isSSMenu = _hrUnitOfWork.Repository<Model.Domain.Menu>().Where(a => a.Id == MenuId).Select(a => a.SSMenu).FirstOrDefault();

            if (id==2) {
                ViewBag.MangRole = true;
            }
            else { ViewBag.MangRole = false; }

            string[] weekDays = { MsgUtils.Instance.Trls("Sunday"), MsgUtils.Instance.Trls("Monday"), MsgUtils.Instance.Trls("Tuesday"), MsgUtils.Instance.Trls("Wednesday"), MsgUtils.Instance.Trls("Thursday"), MsgUtils.Instance.Trls("Friday"), MsgUtils.Instance.Trls("Saturday") };
            byte? weekend = _PersonSetup.Weekend2 ?? _PersonSetup.Weekend1;
            if (weekend != null)
            {
                byte firstDate = (byte)(weekend == 6 ? 0 : (weekend + 1));
                ViewBag.week = "(" + weekDays[firstDate] + " - " + weekDays[weekend.Value] + ")";
            }
            return View();
        }
        public ActionResult MangerChanged(int MangId)
        {
            var emps = _hrUnitOfWork.PeopleRepository.GetEmployeeManagedByManagerId(CompanyId, Language, MangId);

            return Json(new { emps = emps }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmpChanged(int EmpId)
        {
            var value = _hrUnitOfWork.LeaveRepository.GetLastEmpCalcsMethod(CompanyId, EmpId);
            return Json(new { value = value }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult disableEmpAssignDates(int EmpId)
        {
            var dates = _hrUnitOfWork.LeaveRepository.GetEmpAssignDates(CompanyId, EmpId);
            return Json(new { dates = dates }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllAssignOrders(int MenuId, byte Tab, int pageSize, int skip, byte? Range, string Depts, DateTime? Start, DateTime? End)
        {
            IQueryable<AssignOrderViewModel> query;
            if (Range >= 0 && Range < 10)
            {
                query = _hrUnitOfWork.LeaveRepository.ReadAssignOrdersArchieve(CompanyId, Range ?? 10, Depts, Start, End, Language);
            }
            else
            {
                query = _hrUnitOfWork.LeaveRepository.ReadAssignOrders(CompanyId, Tab, Range ?? 10, Depts, Start, End, Language);
            }
            string filter = "";
            string Sorting = "";
            string whecls = GetWhereClause(MenuId);

            query = (IQueryable<AssignOrderViewModel>)Utils.GetFilter(query, ref filter, ref Sorting);

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

            if (Sorting.Length > 0 && skip > 0)
                query = query.Skip(skip).Take(pageSize).OrderBy(Sorting);
            else if (Sorting.Length > 0)
                query = query.Take(pageSize).OrderBy(Sorting);
            else if (skip > 0)
                query = query.Skip(skip).Take(pageSize);
            else
                query = query.Take(pageSize);

            return Json(new { total = total, data = query.ToList() }, JsonRequestBehavior.AllowGet);
            //return View();
        }
        public ActionResult Details(int id = 0, bool Manager = false , byte Version = 0)
        {
            if (!_hrUnitOfWork.LeaveRepository.CheckAutoCompleteColumn("AssignOrders", CompanyId, Version, "EmpId"))
            {
                var mang = Request.QueryString["Manager"]?.ToString();
                if (mang != null)
                {
                    var MangerData = User.Identity.GetEmpId();
                    ViewBag.Mangers = _hrUnitOfWork.PeopleRepository.GetActiveMangersByMangerId(CompanyId, Language, MangerData);
                }
                else
                {
                    ViewBag.Mangers = _hrUnitOfWork.PeopleRepository.GetActiveMangers(CompanyId, Language);
                }              
            }
            
            ViewBag.Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(CompanyId); //for Calender
            ViewBag.Today = DateTime.Today;
            if (id == 0)
                return View(new AssignOrder());

            AssignOrder request = _hrUnitOfWork.LeaveRepository.GetAssignOrderByiD(id);
            if (id != 0)
            {
                ViewBag.Employee = _hrUnitOfWork.PeopleRepository.GetEmployeeById(CompanyId, Language, request.EmpId);
            }
            return View(request);
        }

        [HttpPost]
        public ActionResult Details(AssignOrderViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "AssignOrders",
                        TableName = "AssignOrders",
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
                return Json(Models.Utils.ParseFormErrors(ModelState));

            AssignOrder request = _hrUnitOfWork.LeaveRepository.GetAssignOrderByiD(model.Id);
            byte version;
            byte.TryParse(Request.Form["version"], out version);
            var isRequired = _hrUnitOfWork.Repository<Workflow>().Where(w => w.Source == "AssignOrder" + model.CalcMethod && w.CompanyId==CompanyId).Select(a => a.IsRequired).FirstOrDefault();

            if (model.Id == 0)
            {
                //new
                request = new AssignOrder();
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "AssignOrders", Options = moreInfo, Transtype = TransType.Insert };
                AutoMapper(parms);

                request.CompanyId = CompanyId;
                request.CreatedUser = UserName;
                request.CreatedTime = DateTime.Now;
                request.ApprovalStatus = (byte)(isRequired ? 1 : 6);
                _hrUnitOfWork.LeaveRepository.AddAssignOrder(request);
            }
            else
            {
                //Edit
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request, Version = version, ObjectName = "AssignOrders", Options = moreInfo, Transtype = TransType.Update };
                AutoMapper(parms);
                //
                _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                {
                    ColumnName = "ApprovalStatus",
                    CompanyId = CompanyId,
                    ObjectName = "AssignOrders",
                    SourceId = request.Id.ToString(),
                    UserName = UserName,
                    ValueAfter = MsgUtils.Instance.Trls("Submit"),
                    ValueBefore = MsgUtils.Instance.Trls("Darft"),
                    Transtype = (byte)TransType.Update
                });

                request.CompanyId = CompanyId;
                request.CreatedUser = UserName;
                request.CreatedTime = DateTime.Now;
                request.ApprovalStatus = (byte)(isRequired ? 1 : 6);
                _hrUnitOfWork.LeaveRepository.AttachAssignOrder(request);
                _hrUnitOfWork.LeaveRepository.EntryAssignOrder(request).State = EntityState.Modified;
            }

            if (!isRequired && model.CalcMethod == 2 && model.Id == 0)
            {
                var error = _hrUnitOfWork.LeaveRepository.AddAssignOrdersLeaveTrans(request, UserName, Language);
                if (error.Length > 0)
                    return Json(error);
            }

             errors = SaveChanges(Language);
             string message = "OK," + ((new JavaScriptSerializer()).Serialize(model));
            if (errors.Count > 0)
            {
                message = errors.First().errors.First().message;
                return Json(message);
            }
            //workflow
            if (isRequired)
            {
                if (request.CalcMethod == 1) //monetary
                { 
                    WfViewModel wf = new WfViewModel()
                    {
                        Source = "AssignOrder1",
                        SourceId = request.CompanyId,
                        DocumentId = request.Id,
                        RequesterEmpId = request.EmpId,
                        ApprovalStatus = 6,
                        CreatedUser = UserName,
                    };

                    var wfTrans = _hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, Language);
                    if (wfTrans == null && wf.WorkFlowStatus != "Success")
                    {
                        request.ApprovalStatus = 1;
                        message += "," + (new JavaScriptSerializer()).Serialize(new { model = request, error = wf.WorkFlowStatus });

                        _hrUnitOfWork.LeaveRepository.AttachAssignOrder(request);
                        _hrUnitOfWork.LeaveRepository.EntryAssignOrder(request).State = EntityState.Modified;
                    }
                    else if (wfTrans != null)
                        _hrUnitOfWork.LeaveRepository.Add(wfTrans);
                }
                else if (request.CalcMethod == 2) //time compensation
                {
                    WfViewModel wf = new WfViewModel()
                    {
                        Source = "AssignOrder2",
                        SourceId = request.CompanyId,
                        DocumentId = request.Id,
                        RequesterEmpId = request.EmpId,
                        ApprovalStatus = 6,
                        CreatedUser = UserName,
                    };

                    var wfTrans = _hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, Language);
                    if (wfTrans == null && wf.WorkFlowStatus != "Success")
                    {
                        request.ApprovalStatus = 1;
                        message += "," + (new JavaScriptSerializer()).Serialize(new { model = request, error = wf.WorkFlowStatus });

                        _hrUnitOfWork.LeaveRepository.AttachAssignOrder(request);
                        _hrUnitOfWork.LeaveRepository.EntryAssignOrder(request).State = EntityState.Modified;
                    }
                    else if (wfTrans != null)
                        _hrUnitOfWork.LeaveRepository.Add(wfTrans);
                }

                errors = Save(Language);
                if (errors.Count > 0)
                    message = errors.First().errors.First().message;
            }
            //end workflow
            
            if (message == "OK")
                message += "," + ((new JavaScriptSerializer()).Serialize(request));

            return Json(message);
        }
        public ActionResult DeleteOrders(int id)
        {
            List<Error> errors = new List<Error>();

            string message = "OK";
            _hrUnitOfWork.LeaveRepository.DeleteAssignOrder(id, Language);

            errors = SaveChanges(Language);
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }
        public ActionResult PrevEmpAssignOrders(int empId)
        {
            ViewBag.TransType = _hrUnitOfWork.LookUpRepository.GetGridLookUpCode(Language, "TransType");
            ViewBag.EmpId = empId;
            return PartialView("_PrevEmpAssignOrder");
        }
        public ActionResult ReadPrevEmpAssignOrders(int empId)
        {
            var result = Json(_hrUnitOfWork.LeaveRepository.GetEmpAssignData(CompanyId, empId, Language), JsonRequestBehavior.AllowGet);
            return result;
        }
        public ActionResult GetEmpInfo(int EmpId)
        {
            var Info = _hrUnitOfWork.LeaveRepository.GetFullEmpInfo(CompanyId, EmpId, Language);
            return Json(new { Info = Info }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSpacificEmpLeave(int empId)
        {
            //var leaves = _hrUnitOfWork.LeaveRepository.GetSpacificLeaveTypes(CompanyId, Language, empId);
            var leaves = _hrUnitOfWork.LeaveRepository.GetSpacificLeaveTypes(CompanyId, Language, empId).Select(t => new { id = t.Id, name = t.Name });
            return Json(new { leaves = leaves }, JsonRequestBehavior.AllowGet);
        }

    }
}