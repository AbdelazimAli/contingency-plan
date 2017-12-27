using System;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
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
using System.Collections;
using System.Linq.Dynamic;

namespace WebApp.Controllers.Api
{
    public class MedicalController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;

        public MedicalController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        public List<FormList> ApprovelStutesList()
        {
            List<FormList> ApprovelStytes = new List<FormList>();
            ApprovelStytes.Add(new FormList { id = 1, name = MsgUtils.Instance.Trls("Darft") });
            ApprovelStytes.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("Submit") });
            ApprovelStytes.Add(new FormList { id = 3, name = MsgUtils.Instance.Trls("Employee Review") });
            ApprovelStytes.Add(new FormList { id = 4, name = MsgUtils.Instance.Trls("Cancel before accepted") });
            ApprovelStytes.Add(new FormList { id = 5, name = MsgUtils.Instance.Trls("Rejected") });
            ApprovelStytes.Add(new FormList { id = 6, name = MsgUtils.Instance.Trls("Accepted") });
            ApprovelStytes.Add(new FormList { id = 7, name = MsgUtils.Instance.Trls("Cancel after accepted") });
            ApprovelStytes.Add(new FormList { id = 8, name = MsgUtils.Instance.Trls("Manager Review") });
            return ApprovelStytes;
        }
        [ResponseType(typeof(MedicalIndexViewModel)), HttpGet]
        [Route("api/Medical/GetMedRequests")]
        public IHttpActionResult GetMedRequests(int MenuId)
        {
            List<FormList> ApprovelStytes = ApprovelStutesList();
            var query = _hrUnitOfWork.MedicalRepository.GetMedicalRequest(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
            string whecls = GetWhereClause(MenuId);
            if (whecls.Length > 0)
            {
                try
                {
                    query = query.Where(whecls);
                }
                catch (Exception ex)
                {
                    Models.Utils.LogError(ex.Message);
                    return Ok("");
                }
            }
            var Returnquery = query.ToList();
            foreach (var item in Returnquery)
            {
                item.ApprovalStatusName= ApprovelStytes.Where(a => a.id == item.ApprovalStatus).Select(a => a.name).FirstOrDefault();
            }
            return Ok(Returnquery);
        }
        [HttpDelete]
        [Route("api/Medical/Delete")]
        public IHttpActionResult Delete(int id)
        {
            string message = "Ok";
            DataSource<MedicalIndexViewModel> Source = new DataSource<MedicalIndexViewModel>();
            BenefitRequest request = _hrUnitOfWork.MedicalRepository.Get(id);
            if (request.ApprovalStatus == 1)
            {
                _hrUnitOfWork.MedicalRepository.Remove(request);
            }
            else
                message = "CantDelete";
            Source.Errors = SaveChanges(User.Identity.GetLanguage());
            if (Source.Errors.Count() > 0)
                return Ok(Source);
            else
                return Ok(message);
        }

        [ResponseType(typeof(TerminationFormViewModel)), HttpGet]
        [Route("api/Medical/GetMedicalRequest")]
        public IHttpActionResult GetMedicalRequest(int id = 0)
        {

            var Medical = _hrUnitOfWork.MedicalRepository.ReadMedical(id);
            IEnumerable BeneficiaryId = null;
            IEnumerable Services = null;
            //if (User.Identity.IsSelfServiceUser())
            //{
            int EmpId = User.Identity.GetEmpId();
            BeneficiaryId = _hrUnitOfWork.Repository<EmpRelative>().Where(a => a.EmpId == 1034).Select(p => new { id = p.Id, name = p.Name }).ToList();

            //}
            if (Medical != null)
            {
                BeneficiaryId = _hrUnitOfWork.Repository<EmpRelative>().Where(a => a.EmpId == Medical.EmpId).Select(p => new { id = p.Id, name = p.Name }).ToList();
                Services = _hrUnitOfWork.MedicalRepository.GetAllservice(Medical.EmpId, Medical.BenefitId, Medical.BeneficiaryId);
            }
            var Providers = _hrUnitOfWork.MedicalRepository.GetAllProvider();
            var Currency = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            var BenefitClass = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("BenefitClass", User.Identity.GetLanguage()).Select(a => new { id = a.CodeId, name = a.Title });
            // string culture = User.Identity.GetLanguage();
            //List<string> columns = _hrUnitOfWork.LeaveRepository.GetAutoCompleteColumns("MedicalRequestsForm", User.Identity.GetDefaultCompany(), Version);
            //if (columns.FirstOrDefault(fc => fc == "EmpId") == null)
            //    var Employees = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(User.Identity.GetLanguage(), Medical != null ? Medical.EmpId : 0, User.Identity.GetDefaultCompany()).Select(a => new { id = a.Id, name = a.Employee, PicUrl = a.PicUrl, Icon = a.EmpStatus }).ToList();
            if (id == 0)
                Medical = new MedicalRequestViewModel();
            return Ok(new { request = Medical, Currency = Currency, Providers = Providers, BeneficiaryId = BeneficiaryId, Services = Services, BenefitClass = BenefitClass });
        }

        [ResponseType(typeof(MedicalRequestViewModel)), HttpPost]
        [Route("api/Medical/SaveMedRequests")]
        public IHttpActionResult SaveMedRequests(MedicalRequestViewModel model)
        {
            List<Model.ViewModel.Error> errors = new List<Model.ViewModel.Error>();

            if (!ModelState.IsValid)
                return Json(Utils.ParseFormError(ModelState));


            string message = "Ok";
            model.EmpId = User.Identity.GetEmpId();
            model.EmpId = 1034;

            BenefitRequest request = _hrUnitOfWork.MedicalRepository.Get(model.Id);
            var benefitplanId = _hrUnitOfWork.MedicalRepository.GetBenefitPlanId(model.ParentId, model.EmpId, model.BeneficiaryId);
            ///Save
            if (model.Id == 0)
            { /// New
                request = new BenefitRequest();
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request };
                AutoMapper(parms);
                request.BenefitPlanId = benefitplanId;
                request.ApprovalStatus = (byte)(model.submit == true ? 2 : 1); //1- New, 2- Submit
                request.CreatedUser = User.Identity.Name;
                request.CreatedTime = DateTime.Now;
                request.RequestDate = DateTime.Now;
                request.CompanyId = User.Identity.GetDefaultCompany();
                _hrUnitOfWork.MedicalRepository.Add(request);
            }
            else if (model.ApprovalStatus == 3 || model.ApprovalStatus == 1)
            { /// Edit
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = request };
                AutoMapper(parms);
                if (model.submit)
                    _hrUnitOfWork.TrainingRepository.AddTrail(new AddTrailViewModel()
                    {
                        ColumnName = "ApprovalStatus",
                        CompanyId = User.Identity.GetDefaultCompany(),
                        ObjectName = "ComplainRequests",
                        SourceId = request.Id.ToString(),
                        UserName = User.Identity.Name,
                        Version = 0,
                        ValueAfter = MsgUtils.Instance.Trls("Submit"),
                        ValueBefore = MsgUtils.Instance.Trls("Darft")
                    });
                request.BenefitPlanId = benefitplanId == 0 ? model.BenefitPlanId : benefitplanId;
                request.ApprovalStatus = (byte)(model.submit == true ? 2 : model.ApprovalStatus); //1- New, 2- Submit
                request.ModifiedUser = User.Identity.Name;
                request.ModifiedTime = DateTime.Now;
                request.RequestDate = DateTime.Now;
                _hrUnitOfWork.MedicalRepository.Attach(request);
                _hrUnitOfWork.MedicalRepository.Entry(request).State = System.Data.Entity.EntityState.Modified;
            }

            var Errors = SaveChanges(User.Identity.GetLanguage());
            if (Errors.Count > 0)
            {
                message = Errors.First().errors.First().message;
                return Json(message);
            }

            if (model.submit)
            {
                WfViewModel wf = new WfViewModel()
                {
                    Source = "Medical",
                    SourceId = User.Identity.GetDefaultCompany(),
                    DocumentId = request.Id,
                    RequesterEmpId = request.EmpId,
                    ApprovalStatus = 2,
                    CreatedUser = User.Identity.Name,
                };
                var wfTrans = _hrUnitOfWork.ComplaintRepository.AddWorkFlow(wf, User.Identity.GetLanguage());
                if (wfTrans == null && wf.WorkFlowStatus != "Success")
                {
                    request.ApprovalStatus = 1;
                    _hrUnitOfWork.MedicalRepository.Attach(request);
                    _hrUnitOfWork.MedicalRepository.Entry(request).State = System.Data.Entity.EntityState.Modified;
                    message = wf.WorkFlowStatus;
                }
                else if (wfTrans != null)
                    _hrUnitOfWork.LeaveRepository.Add(wfTrans);
            }

            Errors = SaveChanges(User.Identity.GetLanguage());
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;

            return Json(message);

        }
        [ResponseType(typeof(MedicalRequestViewModel)), HttpGet]
        [Route("api/Medical/GetBeneficiaryService")]
        public IHttpActionResult GetBeneficiaryService(int BeneficiaryId, short BenefitClass)
        {
            var loginEmployee = User.Identity.GetEmpId();
            var EmpId = User.Identity.GetEmpId();
            EmpId = 1034;
            var EmployeeId = EmpId == 0 ? loginEmployee : EmpId;
            var Service = _hrUnitOfWork.MedicalRepository.GetAllservice(EmployeeId, BenefitClass, BeneficiaryId);
            return Ok(Service);
        }
        [ResponseType(typeof(MedicalRequestViewModel)), HttpGet]
        [Route("api/Medical/SetCurrency")]
        public IHttpActionResult SetCurrency(int ServiceId)
        {
            var query = _hrUnitOfWork.MedicalRepository.SetCurrency(ServiceId);
            var CompCost = query.Cost * (decimal)query.CompPercent / 100;
            var EmpCost = query.Cost * (decimal)query.EmpPercent / 100;

            return Ok(new { EmployeeCost = EmpCost, CompanyCost = CompCost, Currency = query });
        }
    }
}