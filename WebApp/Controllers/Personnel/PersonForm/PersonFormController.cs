using Db.Persistence.BLL;
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

namespace WebApp.Controllers.Personnel.PersonForm
{
    public class PersonFormController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;

        public PersonFormController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        // GET: PersonForm
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPersonForms(int FormType, int MenuId, int pageSize, int skip)
        {
            return ApplyFilter<FlexFormGridViewModel>(_hrUnitOfWork.PersonFormRepository.ReadPersonForms(FormType, Language, User.Identity.GetEmpId()), null, MenuId, pageSize, skip);
        }

        public ActionResult PersonFlexFormDetails(int id = 0)
        {
            FlexFormViewModel FlexForm = _hrUnitOfWork.PersonFormRepository.GetFlexPersonForm(id, Language, User.Identity.GetEmpId());
            return FlexForm == null ? (ActionResult)HttpNotFound() : View(FlexForm);

        }
        [HttpPost]
        public ActionResult CreatPersonForm(List<PersonFormPageVM> modelList, string ExpDate)
        {
            if (DateTime.Parse(ExpDate) < DateTime.Now)
            {
                return Json(@MsgUtils.Instance.Trls("QuestionnairExpired"));
            }
            var bl = new PersonFormBL(_hrUnitOfWork, Language, CompanyId);
            var modelstate = bl.IsValid(ModelState, ServerValidationEnabled);

            //Delete Befor saving Data
            FlexFormViewModel FlexForm = _hrUnitOfWork.PersonFormRepository.GetFlexPersonForm(modelList[0].SendFormId, Language, User.Identity.GetEmpId());
            foreach (var item in FlexForm.personForm)
            {
                Model.Domain.PersonForm pf = new Model.Domain.PersonForm();
                pf.Id = item.Id;
                _hrUnitOfWork.PersonFormRepository.Remove(pf);
            }
            if (!modelstate.IsValid)
                return Json(Models.Utils.ParseFormErrors(modelstate));

            List<Model.Domain.PersonForm> recordList = new List<Model.Domain.PersonForm>();
            foreach (var model in modelList)
            {
                if (model.AnswersList != null)
                {
                    if (model.AnswersList.Count > 1)
                    {
                        model.Answer = (model.AnswersList.Any()) ? model.AnswersList.Select(a => a.ToString()).Aggregate<string>((x1, x2) => x1 + "," + x2).ToString() : "";
                    }
                    else
                    {
                        model.Answer = model.AnswersList[0];
                    }
                }
                model.EmpId = User.Identity.GetEmpId();
                if (model.Id == 0) // new
                {
                    Model.Domain.PersonForm record = new Model.Domain.PersonForm
                    {
                        FormId = model.FormId,
                        SendFormId=model.SendFormId,
                        EmpId=model.EmpId,
                        FormColumnId=model.FormColumnId,
                        Question = model.Question,
                        Answer = model.Answer,
                        OtherText=model.OtherText,
                        CreatedTime = DateTime.Now,
                        CreatedUser = UserName
                    };
                    recordList.Add(record);
                    //AutoMapper(new Models.AutoMapperParm
                    //{
                    //    Destination = record,
                    //    Source = model,
                    //    ObjectName = "PersonFormPage",
                    //    Transtype = TransType.Insert
                    //});
                    //_hrUnitOfWork.PersonFormRepository.Add(record);
                }
            }
            _hrUnitOfWork.PersonFormRepository.AddRange(recordList);

            var errors = SaveChanges(Language);
            if (errors.Count > 0)
                return Json(errors.First().errors.First().message);
            else
                return Json("OK");

        }
    }
}