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
using Model.Domain.Payroll;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class TrainEventController : BaseController
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
        public TrainEventController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReadTrainEvent(int MenuId, int pageSize, int skip)
        {
            var query = _hrUnitOfWork.TrainingRepository.GetAllEvent(CompanyId).AsQueryable();
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
            var total = query.Count();
            var data = query.Skip(skip).Take(pageSize).ToList();

            return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult Details(int id = 0)
        {
            int? position = null;
            if (Session["PositionId"] != null)
                position = int.Parse(Session["PositionId"]?.ToString());
            ViewBag.PersonIdManger = _hrUnitOfWork.CheckListRepository.GetManagerEmpList(User.Identity.GetEmpId(), position, CompanyId, Language);

            ViewBag.CourseId = _hrUnitOfWork.TrainingRepository.GetTrainCourse(Language, CompanyId).Select(p => new { id = p.Id, name = p.LocalName });
            ViewBag.CenterId = _hrUnitOfWork.LookUpRepository.GetAllHospitals(6).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.CurrCode = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            ViewBag.PersonId = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId, Language);
            string Curr = _PersonSetup.LocalCurrCode;
            float MidRat = _hrUnitOfWork.LookUpRepository.GetCurrency().Where(a=>a.Code==Curr).Select(a=>a.MidRate).FirstOrDefault();

            if (id == 0)
                return View(new TrainEventFormViewModel() { Curr = Curr  ,CurrRate = MidRat});

            var TrainEventObj = _hrUnitOfWork.TrainingRepository.ReadTrainEvent(id);
            return TrainEventObj == null ? (ActionResult)HttpNotFound() : View(TrainEventObj);
        }
        [HttpPost]
        public ActionResult Details(TrainEventFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.SiteRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "TrainEvents",
                        TableName = "TrainEvents",
                        ParentColumn = "CourseId",
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

                TrainEvent record;
                PeopleTraining PeopleTrainObj = new PeopleTraining();
                var message = "OK";

                //insert
                if (model.Id == 0)
                {
                    record = new TrainEvent();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "TrainEvents",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });

                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;

                    _hrUnitOfWork.TrainingRepository.Add(record);
                }

                //booking
                else if (model.book)
                {
                    record = _hrUnitOfWork.Repository<TrainEvent>().FirstOrDefault(a => a.Id == model.Id);
                    PeopleTrainObj.CreatedUser = UserName;
                    PeopleTrainObj.CreatedTime = DateTime.Now;
                    PeopleTrainObj.CompanyId = CompanyId;
                    PeopleTrainObj.ApprovalStatus = 2;
                    PeopleTrainObj.RequestDate = DateTime.Now;
                    PeopleTrainObj.EmpId = (model.PersonId != null ? model.PersonId.Value: User.Identity.GetEmpId());
                    PeopleTrainObj.CourseEDate = model.TrainEndDate;
                    PeopleTrainObj.CourseSDate = model.TrainStartDate;
                    PeopleTrainObj.EventId = record.Id;
                    PeopleTrainObj.CompanyId = CompanyId;
                    PeopleTrainObj.Cost = record.Cost;
                    PeopleTrainObj.Curr = record.Curr;
                    PeopleTrainObj.CurrRate = record.CurrRate;
                    PeopleTrainObj.Notes = record.Notes;
                    PeopleTrainObj.CourseId = record.CourseId;
                    PeopleTrainObj.Adwarding = record.Adwarding;

                    _hrUnitOfWork.TrainingRepository.Add(PeopleTrainObj);
                }

                //update
                else
                {
                    record = _hrUnitOfWork.Repository<TrainEvent>().FirstOrDefault(a => a.Id == model.Id);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "TrainEvents",
                        Options = moreInfo,
                        Transtype = TransType.Update

                    });

                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;

                    _hrUnitOfWork.TrainingRepository.Attach(record);
                    _hrUnitOfWork.TrainingRepository.Entry(record).State = EntityState.Modified;
                }
                errors = SaveChanges(Language);
                if (errors.Count > 0) message = errors.First().errors.First().message;
                if (model.book)
                {
                    WfViewModel wf = new WfViewModel()
                    {
                        Source = "Training",
                        SourceId = CompanyId,
                        DocumentId = PeopleTrainObj.Id,
                        RequesterEmpId = PeopleTrainObj.EmpId,
                        ApprovalStatus = 2,
                        CreatedUser = UserName,
                    };
                    var wfTrans = _hrUnitOfWork.ComplaintRepository.AddWorkFlow(wf, Language);
                    if (wfTrans == null && wf.WorkFlowStatus != "Success")
                    {
                        PeopleTrainObj.ApprovalStatus = 2;
                        message = wf.WorkFlowStatus;
                    }
                    else if (wfTrans != null)
                        _hrUnitOfWork.LeaveRepository.Add(wfTrans);
                    errors = SaveChanges(Language);
                }

                return Json(message);
            }
            return Json(Models.Utils.ParseFormErrors(ModelState));
        }

        public ActionResult Delete(int id)
        {
            var message = "OK";
            DataSource<TrainEventViewModel> Source = new DataSource<TrainEventViewModel>();

            TrainEvent Eventobj = _hrUnitOfWork.TrainingRepository.GetTrainEvent(id);
            if (Eventobj != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = Eventobj,
                    ObjectName = "TrainEvent",
                    Transtype = TransType.Delete
                });

                _hrUnitOfWork.TrainingRepository.Remove(Eventobj);
            }
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);


        }

        public ActionResult GetCourseDesc(int courseId)
        {
            var summary = _hrUnitOfWork.Repository<TrainCourse>().Where(a => a.Id == courseId).Select(a=>a.Summary).FirstOrDefault();
            return Json(summary, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckPeriod(DateTime DateVal)
        {
            int PeriodId = _hrUnitOfWork.TrainingRepository.GetPeriod(DateVal);
            return Json(PeriodId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMidRate(string CurrCode)
        {
            var r = _hrUnitOfWork.Repository<Currency>().Where(a=>a.Code==CurrCode).Select(a=>a.MidRate).FirstOrDefault();
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMenuName(int MenuId)
        {
            var x = _hrUnitOfWork.TrainingRepository.GetMenuId(MenuId, CompanyId);
            return Json(x, JsonRequestBehavior.AllowGet);
        }
       

    }
}