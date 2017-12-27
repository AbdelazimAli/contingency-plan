using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;
using System.Web.Http.ModelBinding;
using System.Web;
using Interface.Core;
using Model.ViewModel.Personnel;
using Model.ViewModel;
using Model.Domain;
using System;
using System.Linq.Dynamic;

namespace WebApp.Controllers.Api
{
    public class TerminationController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;

        public TerminationController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        [ResponseType(typeof(TerminationGridViewModel)), HttpGet]
        [Route("api/Termination/GetTerminationRequests")]
        public IHttpActionResult GetTerminationRequests(int MenuId)
        {
            var query = _hrUnitOfWork.TerminationRepository.ReadTermRequests(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {
                    //TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Ok("");
                }
            }
            return Ok(query);
        }

        [HttpDelete]
        [Route("api/Termination/Delete")]
        public IHttpActionResult Delete(int id)
        {
            string message = "Ok";
            DataSource<TerminationGridViewModel> Source = new DataSource<TerminationGridViewModel>();

            _hrUnitOfWork.TerminationRepository.DeleteRequest(id, User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());

            Source.Errors = SaveChanges(User.Identity.GetLanguage());

            if (Source.Errors.Count() > 0)
            {
                return Json(Source);
            }
            else
                return Ok(message);
        }
        //
        [ResponseType(typeof(TerminationFormViewModel)), HttpGet]
        [Route("api/Termination/GetTermination")]
        public IHttpActionResult GetTermination(int id = 0)
        {
            TerminationFormViewModel term;
            term = _hrUnitOfWork.TerminationRepository.ReadTermination(id, User.Identity.GetLanguage());
            //List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("TermRequestForm", User.Identity.GetDefaultCompany(), Version);
            //if (columns.FirstOrDefault(fc => fc == "EmpId") == null)
            //    ViewBag.Employees = _hrUnitOfWork.EmployeeRepository.GetTermActiveEmployees(User.Identity.GetLanguage(), term.EmpId, User.Identity.GetDefaultCompany()).Distinct().Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus });

            var Emp = _hrUnitOfWork.PeopleRepository.GetEmployment(term.EmpId);
            int[] arr = new int[] { 1, 2, 4 };
            var Employment = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", User.Identity.GetLanguage()).Where(a => a.SysCodeId == (arr.Contains(Emp.PersonType) ? 3 : 6)).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var AssignStatus = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Assignment", User.Identity.GetLanguage()).Where(a => a.SysCodeId == 3).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var Termination = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Termination", User.Identity.GetLanguage()).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            if (id == 0)
                term = new TerminationFormViewModel() { Id = 0 };

            return Ok(new { request = term, PersonType = Employment, AssignStatusLst = AssignStatus, termReson = Termination });
        }
        [ResponseType(typeof(TerminationFormViewModel)), HttpGet]
        [Route("api/Termination/GetJoined")]
        public IHttpActionResult GetJoined(int? LeveId)
        {
            int Id = User.Identity.GetEmpId();
            Id = 1042;
            var type = _PersonSetup.WorkServMethod;
            var Emp = _hrUnitOfWork.Repository<Employement>().Where(a => a.Status == 1 && a.CompanyId == User.Identity.GetDefaultCompany() && a.EmpId == Id).Select(b => b).FirstOrDefault();
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
                int syscode = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Termination", User.Identity.GetLanguage()).Where(a => a.CodeId == LeveId).Select(b => b.SysCodeId).FirstOrDefault();
                var Periods = _hrUnitOfWork.Repository<TermDuration>().Where(a => a.CompanyId == User.Identity.GetDefaultCompany() && a.TermSysCode == syscode && a.WorkDuration >= Convert.ToByte(serveinYears)).FirstOrDefault();
                if (Periods != null)
                {
                    if (serveinYears > Periods.FirstPeriod)
                        BonusinMonth = Math.Round((Periods.FirstPeriod * Periods.Percent1) + (serveinYears - Periods.FirstPeriod) * (Periods.Percent2 == null ? 0 : Periods.Percent2.Value), 5);
                    else
                        BonusinMonth = Math.Round((serveinYears * Periods.Percent1), 5);
                }
            }
            int[] arr = new int[] { 1, 2, 4 };
            var EmpStatus = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", User.Identity.GetLanguage()).Where(a => a.SysCodeId == (arr.Contains(Emp.PersonType) ? 3 : 6)).Select(b => new { id = b.CodeId, name = b.Title }).ToList();

            var myobj = new { date = date, bouns = BonusinMonth, ServYear = serveinYears, EmpStatus = EmpStatus };
            return Ok(myobj);
        }

        private string AddWFTrans(Termination request, int? ManagerId, bool? backToEmp)
        {
            WfViewModel wf = new WfViewModel()
            {
                Source = "Termination",
                SourceId = User.Identity.GetDefaultCompany(),
                DocumentId = request.Id,
                RequesterEmpId = request.EmpId,
                ApprovalStatus = request.ApprovalStatus,
                CreatedUser = User.Identity.Name
            };

            if (ManagerId != null) wf.ManagerId = ManagerId;
            else if (backToEmp != null) wf.BackToEmployee = backToEmp.Value;

            var wfTrans = _hrUnitOfWork.LeaveRepository.AddWorkFlow(wf, User.Identity.GetLanguage());
            request.WFlowId = wf.WFlowId;
            if (wfTrans == null && wf.WorkFlowStatus != "Success")
                return wf.WorkFlowStatus;
            else if (wfTrans == null && wf.WorkFlowStatus == "Success")
                request.ApprovalStatus = 6;
            else if (wfTrans != null)
                _hrUnitOfWork.LeaveRepository.Add(wfTrans);

            return "";
        }

        [ResponseType(typeof(TerminationFormViewModel)), HttpPost]
        [Route("api/Termination/SaveTermination")]
        public IHttpActionResult SaveTermination(TerminationFormViewModel model)
        {
            List<Model.ViewModel.Error> errors = new List<Model.ViewModel.Error>();

            if (!ModelState.IsValid)
                return Json(Utils.ParseFormError(ModelState));


            string message = "Ok";
            model.EmpId = User.Identity.GetEmpId();
            model.EmpId = 1042;
            
            var Term = _hrUnitOfWork.TerminationRepository.Get(model.Id);
      
            if (Term == null) // New
            {
                Term = new Termination();

                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = Term}; 
                AutoMapper(parms);
                Term.CreatedTime = DateTime.Now;
                Term.CreatedUser = User.Identity.Name;
                Term.RequestDate = DateTime.Now;
                if(model.Excute)
                    Term.ApprovalStatus = 2;
                _hrUnitOfWork.TerminationRepository.Add(Term);

            }
            else // Edit
            {
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = Term };
                AutoMapper(parms);
                Term.ModifiedTime = DateTime.Now;
                Term.ModifiedUser = User.Identity.Name;
                if (model.Excute)
                    Term.ApprovalStatus = 2;
                _hrUnitOfWork.TerminationRepository.Attach(Term);
                _hrUnitOfWork.TerminationRepository.Entry(Term).State = EntityState.Modified;
            }
            if (model.Excute)
            {
              
                string error = AddWFTrans(Term, null, null);
                if (error.Length > 0)
                    return Json(error);

                var checklist = _hrUnitOfWork.CheckListRepository.GetTermCheckLists(User.Identity.GetDefaultCompany());
                if (checklist != null)
                {
                    EmpChkList EmpList = _hrUnitOfWork.CheckListRepository.AddEmpChlst(checklist, User.Identity.Name, Term.EmpId, User.Identity.GetDefaultCompany());
                    _hrUnitOfWork.CheckListRepository.Add(EmpList);
                    var checkTask = _hrUnitOfWork.CheckListRepository.ReadCheckListTask(checklist.Id).ToList();
                    if (checkTask.Count > 0)
                    {
                        _hrUnitOfWork.CheckListRepository.AddEmpTask(checkTask, User.Identity.Name, EmpList);
                    }

                }
            }
            try
            {
                _hrUnitOfWork.Save();
            }
            catch (Exception ex)
            {
                 message = _hrUnitOfWork.HandleDbExceptions(ex, User.Identity.GetLanguage());
                if (message.Length > 0)
                    return Ok(message);
            }

            return Ok(message);
        }
        
     
    }
}
    