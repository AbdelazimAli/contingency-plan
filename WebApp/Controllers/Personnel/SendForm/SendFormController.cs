using Db.Persistence.BLL;
using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WebApp.Controllers.Personnel.SendForm
{
    public class SendFormController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;

        public SendFormController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        // GET: SendForm
        public ActionResult Index()
        {
            ViewBag.FormNameList = (new JavaScriptSerializer()).Serialize(_hrUnitOfWork.Repository<FlexForm>().Select(a => new { value = a.Id, text = a.Name }).ToList());
            return View();
        }

        public ActionResult GetForms(int MenuId, int pageSize, int skip)
        {
            return ApplyFilter<SendFormGridVM>(_hrUnitOfWork.SendFormRepository.GetForms(CompanyId, Language, MsgUtils.Instance.Trls("TargetEmp"), MsgUtils.Instance.Trls("TargetDept"), MsgUtils.Instance.Trls("TargetJob")), null, MenuId, pageSize, skip);
        }

        public ActionResult Detail(int Id)
        {
            //ViewBag.FormId = (new JavaScriptSerializer()).Serialize(_hrUnitOfWork.Repository<FlexForm>().Select(a => new { id = a.Id, name = a.Name }).ToList());
            //ViewBag.Departments = (new JavaScriptSerializer()).Serialize(_hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId, null, Culture).ToList());
            //ViewBag.jobs = (new JavaScriptSerializer()).Serialize(_hrUnitOfWork.JobRepository.GetAllJobs(CompanyId, Culture, 0).Select(a => new { id = a.Id, name = a.LocalName }).ToList());
           // ViewBag.Employees = (new JavaScriptSerializer()).Serialize(_hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Culture, 0, CompanyId).Select(a => new { id = a.Id, name = a.Employee }).ToList());
            return View(_hrUnitOfWork.SendFormRepository.GetFormPage(CompanyId, Culture));
        }

        [HttpPost]
        public ActionResult SaveSendForm(SendFormPageVM model, OptionsViewModel moreInfo, bool clear)
        {
            var bl = new SendFormBL(_hrUnitOfWork, Language, CompanyId);

            var modelstate = bl.IsValid(ModelState, ServerValidationEnabled);
            if (!modelstate.IsValid)
                return Json(Models.Utils.ParseFormErrors(modelstate));

            if (model.Id == 0) // new
            {
                var record = new Model.Domain.SendForm();
                
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = record,
                    Source = model,
                    ObjectName = "SendFormPage",
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });

                record.CreatedTime = DateTime.Now;
                record.CreatedUser = UserName;
                foreach (var item in bl.GetEmpIdList(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString, model, Culture).Distinct())
                {
                    SendFormPeople sendFormPeople = new SendFormPeople()
                    {
                        SendForm= record,
                        EmpId = item
                    };
                    record.sendFormPeople.Add(sendFormPeople);
                }
                _hrUnitOfWork.SendFormRepository.Add(record);
            }

            var errors = SaveChanges(Language);
            if (errors.Count > 0)
                return Json(errors.First().errors.First().message);
            else                //Send Notifications
                bl.RunNotificationsAlgorithm(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString, model,Culture);

            if (clear)
                return Json("OK," + ((new JavaScriptSerializer()).Serialize(new SendFormPageVM())));
            else
                return Json("OK");
        }
    }
}