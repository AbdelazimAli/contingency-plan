using Interface.Core;
using Model.Domain;
using Model.Domain.Payroll;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers.Api
{
    public class TrainingController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;


        public TrainingController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        #region People Train
        [ResponseType(typeof(TrainIndexFollowUpViewModel)), HttpGet]
        [Route("api/Training/GetPeopleTrain")]
        public IHttpActionResult GetPeopleTrain(int MenuId)
        {
            var empId = User.Identity.GetEmpId();
            var query = _hrUnitOfWork.TrainingRepository.GetPeopleTrain(User.Identity.GetLanguage(), User.Identity.GetDefaultCompany());
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
        [Route("api/Training/Delete")]
        public IHttpActionResult Delete(int id)
        {
            string message = "Ok";
            DataSource<TrainIndexFollowUpViewModel> Source = new DataSource<TrainIndexFollowUpViewModel>();

            PeopleTraining PeopleTrain = _hrUnitOfWork.TrainingRepository.GetpeopleTraining(id);
            _hrUnitOfWork.TrainingRepository.Remove(PeopleTrain);

            Source.Errors = SaveChanges(User.Identity.GetLanguage());

            if (Source.Errors.Count() > 0)
            {
                return Json(Source);
            }
            else
                return Ok(message);
        }
        [ResponseType(typeof(PeopleTrainFormViewModel)), HttpGet]
        [Route("api/Training/GetPeopleTrainObj")]
        public IHttpActionResult GetPeopleTrainObj(int id = 0)
        {
            var CourseId = _hrUnitOfWork.TrainingRepository.GetTrainCourse(User.Identity.GetLanguage(), User.Identity.GetDefaultCompany()).Select(p => new { id = p.Id, name = p.LocalName });
            var CurrCode = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
             var EmpLst=_hrUnitOfWork.TrainingRepository.GetPeople(User.Identity.GetLanguage()).Select(a=> new {id=a.Id,name=a.Title, PicUrl = a.PicUrl, Icon = a.EmpStatus });
            PeopleTrainFormViewModel TrainObj;
            if (id == 0)
                TrainObj = new PeopleTrainFormViewModel();
            else
                TrainObj = _hrUnitOfWork.TrainingRepository.ReadPeopleTrain(id, User.Identity.GetLanguage());

            return Ok(new { TrainObj = TrainObj, CourseIdLst = CourseId, CurrCodeLst = CurrCode, EmpLst= EmpLst });
        }
        [ResponseType(typeof(TerminationFormViewModel)), HttpGet]
        [Route("api/Training/GetMidRate")]
        public IHttpActionResult GetMidRate(string CurrCode)
        {
            var MidRate = _hrUnitOfWork.Repository<Currency>().Where(a => a.Code == CurrCode).Select(a => a.MidRate).FirstOrDefault();
            return Ok(MidRate);
        }
        [ResponseType(typeof(PeopleTrainFormViewModel)), HttpPost]
        [Route("api/Training/SavePeopleTrain")]
        public IHttpActionResult SavePeopleTrain(PeopleTrainFormViewModel model)
        {
            List<Model.ViewModel.Error> errors = new List<Model.ViewModel.Error>();
            if (!ModelState.IsValid)
                return Json(Utils.ParseFormError(ModelState));

            string message = "Ok";
           
            if(model.EmpId== 0)
                model.EmpId = User.Identity.GetEmpId();
            model.EmpId = 1042;
            var record = _hrUnitOfWork.Repository<PeopleTraining>().FirstOrDefault(a => a.Id == model.Id);
            //insert
            if (record == null)
            {
                record = new PeopleTraining();
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = record };
                AutoMapper(parms);
                record.CreatedUser = User.Identity.Name;
                record.CreatedTime = DateTime.Now;
                record.CompanyId = User.Identity.GetDefaultCompany();
                record.ApprovalStatus = 6;
                record.RequestDate = DateTime.Now;
                _hrUnitOfWork.TrainingRepository.Add(record);
            }
            //update
            else
            {
                AutoMapperParm parms = new AutoMapperParm() { Source = model, Destination = record };
                AutoMapper(parms);
                record.ModifiedTime = DateTime.Now;
                record.ModifiedUser = User.Identity.Name;
                record.CompanyId = User.Identity.GetDefaultCompany();
                record.RequestDate = DateTime.Now;
                record.ApprovalStatus = 6;
                _hrUnitOfWork.TrainingRepository.Attach(record);
                _hrUnitOfWork.TrainingRepository.Entry(record).State = EntityState.Modified;

            }
            var Errors = SaveChanges(User.Identity.GetLanguage());
            if (Errors.Count > 0)
                message = Errors.First().errors.First().message;

            return Ok(message);

        }

        #endregion

        #region TrainEvent
        
        [ResponseType(typeof(TrainEventViewModel)), HttpGet]
        [Route("api/Training/GetTrainEvent")]
        public IHttpActionResult GetTrainEvent(int MenuId)
        {
            var query = _hrUnitOfWork.TrainingRepository.GetAllEvent(User.Identity.GetDefaultCompany()).AsQueryable();
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
        [ResponseType(typeof(PeopleTrainFormViewModel)), HttpGet]
        [Route("api/Training/GetTrainEventObj")]
        public IHttpActionResult GetTrainEventObj(int id = 0)
        {
            var CourseId = _hrUnitOfWork.TrainingRepository.GetTrainCourse(User.Identity.GetLanguage(), User.Identity.GetDefaultCompany()).Select(p => new { id = p.Id, name = p.LocalName });
            var CenterId = _hrUnitOfWork.LookUpRepository.GetAllHospitals(6).Select(a => new { id = a.Id, name = a.Name });
            var CurrCode = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            var PersonId = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(User.Identity.GetDefaultCompany(), User.Identity.GetLanguage());
            string Curr = _PersonSetup.LocalCurrCode;
            float MidRat = _hrUnitOfWork.LookUpRepository.GetCurrency().Where(a => a.Code == Curr).Select(a => a.MidRate).FirstOrDefault();
            TrainEventFormViewModel TrainObj;
            if (id == 0)
                TrainObj = new TrainEventFormViewModel();
            else
                TrainObj = _hrUnitOfWork.TrainingRepository.ReadTrainEvent(id);
            return Ok(new { TrainObj = TrainObj, CourseIdLst = CourseId, CurrCodeLst = CurrCode, CenterId = CenterId, PersonId = PersonId, Curr = Curr, MidRat = MidRat });
        }
        [ResponseType(typeof(TerminationFormViewModel)), HttpPost]
        [Route("api/Training/SaveTrainEvent")]
        public IHttpActionResult SaveTrainEvent(TrainEventFormViewModel model)
        {
            List<Model.ViewModel.Error> errors = new List<Model.ViewModel.Error>();
            if (!ModelState.IsValid)
                return Json(Utils.ParseFormError(ModelState));

            string message = "Ok";
            PeopleTraining PeopleTrainObj = new PeopleTraining();

            TrainEvent record = _hrUnitOfWork.Repository<TrainEvent>().FirstOrDefault(a => a.Id == model.Id);
            if (model.PersonId != null)
                PeopleTrainObj.EmpId = model.PersonId.GetValueOrDefault();
            else
                PeopleTrainObj.EmpId = 1042;
            PeopleTrainObj.EmpId = User.Identity.GetEmpId();
            PeopleTrainObj.CreatedUser = User.Identity.Name;
            PeopleTrainObj.CreatedTime = DateTime.Now;
            PeopleTrainObj.CompanyId = User.Identity.GetDefaultCompany();
            PeopleTrainObj.ApprovalStatus = 2;
            PeopleTrainObj.RequestDate = DateTime.Now;
            PeopleTrainObj.CourseEDate = model.TrainEndDate;
            PeopleTrainObj.CourseSDate = model.TrainStartDate;
            PeopleTrainObj.EventId = record.Id;
            PeopleTrainObj.CompanyId = User.Identity.GetDefaultCompany();
            PeopleTrainObj.Cost = record.Cost;
            PeopleTrainObj.Curr = record.Curr;
            PeopleTrainObj.CurrRate = record.CurrRate;
            PeopleTrainObj.Notes = record.Notes;
            PeopleTrainObj.CourseId = record.CourseId;
            PeopleTrainObj.Adwarding = record.Adwarding;
            _hrUnitOfWork.TrainingRepository.Add(PeopleTrainObj);

            errors = SaveChanges(User.Identity.GetLanguage());
            if (errors.Count > 0) message = errors.First().errors.First().message;
            if (model.book)
            {
                WfViewModel wf = new WfViewModel()
                {
                    Source = "Training",
                    SourceId = User.Identity.GetDefaultCompany(),
                    DocumentId = PeopleTrainObj.Id,
                    RequesterEmpId = PeopleTrainObj.EmpId,
                    ApprovalStatus = 2,
                    CreatedUser = User.Identity.Name,
                };
                var wfTrans = _hrUnitOfWork.ComplaintRepository.AddWorkFlow(wf, User.Identity.GetLanguage());
                if (wfTrans == null && wf.WorkFlowStatus != "Success")
                {
                    PeopleTrainObj.ApprovalStatus = 2;
                    message = wf.WorkFlowStatus;
                }
                else if (wfTrans != null)
                    _hrUnitOfWork.LeaveRepository.Add(wfTrans);
                errors = SaveChanges(User.Identity.GetLanguage());
            }
            return Ok(message);
        }


        [ResponseType(typeof(TerminationFormViewModel)), HttpGet]
        [Route("api/Training/CheckEvevtMaxCount")]
        public IHttpActionResult CheckEvevtMaxCount(int EventId)
        {
            var query = _hrUnitOfWork.TrainingRepository.CheckCount(EventId, null);
            return Ok(query);
        }
        #endregion
    }
}
