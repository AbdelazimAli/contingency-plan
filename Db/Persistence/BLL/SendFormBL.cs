using Interface.Core;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain.Notifications;
using System.Web.Mvc;
using Model.Domain;

namespace Db.Persistence.BLL
{
    public class SendFormBL
    {
        IHrUnitOfWork hrUnitOfWork;
        string Lang;
        int CompanyId;
        public SendFormBL(IHrUnitOfWork _hrUnitOfWork, string _lang, int _companyid)
        {
            hrUnitOfWork = _hrUnitOfWork;
            Lang = _lang;
            CompanyId = _companyid;
        }

        #region Save
        public ModelStateDictionary IsValid(ModelStateDictionary ModelState, bool ServerValidationEnabled)
        {
            List<Error> errors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "SendFormPage",
                        TableName = "SendForms",
                        Columns = MsgUtils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        Culture = Lang
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
                        return ModelState;
                    }
                }
            }

            return ModelState;
        }
        #endregion
        public List<int> GetEmpIdList(string ConnectionString, SendFormPageVM model, string Culture)
        {
            HrUnitOfWork unitofwork = new HrUnitOfWork(new HrContextFactory(ConnectionString));

            List<int> EmpIdList = new List<int>();
            //Get Employees in the department
            if (model.Departments != null)
            {
                var EmployeesListByDept = unitofwork.EmployeeRepository.GetActiveEmployees(Culture, 0, model.CompanyId)
                    .Where(e => model.Departments.Contains((int)e.DepartmentId))
                    .Select(e => new { id = e.Id }).ToList();
                foreach (var item in EmployeesListByDept)
                {
                    EmpIdList.Add(item.id);
                }
            }


            //Get Employees in the job
            if (model.Jobs != null)
            {
                var EmployeesListByJob = unitofwork.EmployeeRepository.GetActiveEmployees(Culture, 0, model.CompanyId)
                    .Where(e => model.Jobs.Contains((int)e.JobId))
                    .Select(e => new { id = e.Id }).ToList();
                foreach (var item in EmployeesListByJob)
                {
                    EmpIdList.Add(item.id);
                }
            }

            if (model.Employees != null)
                EmpIdList.AddRange(model.Employees);

            return EmpIdList;
        }
        #region Notifications
        public  void RunNotificationsAlgorithm(string ConnectionString, SendFormPageVM model,string Culture)
        {
            HrUnitOfWork unitofwork = new HrUnitOfWork(new HrContextFactory(ConnectionString));

            try
            {
                DateTime Today = DateTime.Now.Date;
                List<int> EmployeesToNotificate = GetEmpIdList( ConnectionString, model, Culture);
                string FormName = unitofwork.Repository<FlexForm>().Where(a => a.Id == model.FormId).SingleOrDefault().Name;
                List<FormDropDown> EmpsLangs = unitofwork.MeetingRepository.GetUsersLang((EmployeesToNotificate.Any()) ? EmployeesToNotificate.Select(a => a.ToString()).Aggregate<string>((x1, x2) => x1 + "," + x2).ToString() : "");
                foreach (var e in EmployeesToNotificate.Distinct())
                {
                    string Lang;
                    if (EmpsLangs.Select(a=>a.id).Contains(e))
                         Lang = EmpsLangs.Where(a => a.id == e).FirstOrDefault().name;
                    else
                        Lang = Culture;
                    NotifyLetter NL = new NotifyLetter()
                    {
                        CompanyId = model.CompanyId,
                        EmpId = e,
                        NotifyDate = Today,
                        NotifySource = MsgUtils.Instance.Trls(Lang, "Questionnaire") + " " + MsgUtils.Instance.Trls(Lang, FormName),
                        SourceId = model.FormId.ToString(),
                        Sent = true,
                        EventDate = model.ExpiryDate,
                        Description = MsgUtils.Instance.Trls(Lang, "Please fill") +" " + MsgUtils.Instance.Trls(Lang,FormName)+" "+ MsgUtils.Instance.Trls(Lang, "Before") + " " + model.ExpiryDate.ToShortDateString()
                    };
                    unitofwork.NotifyLetterRepository.Add(NL);
                }


                unitofwork.SaveChanges();

            }

            catch (Exception ex)
            {
                unitofwork.HandleDbExceptions(ex);
            }
            finally
            {
            }
        }

        #endregion

    }
}
