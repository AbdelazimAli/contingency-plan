using Interface.Core.Repositories;
using Model.Domain;
using System;
using Model.ViewModel.Personnel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Db.Persistence.Repositories
{
    public class SendFormRepository : Repository<SendForm>, ISendFormRepository
    {
        public SendFormRepository(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        public IQueryable<SendFormGridVM> GetForms(int company, string lang, string targetEmp, string targetDept, string targetJob)
        {
            var query = from s in context.SendForms
                        where s.CompanyId == company
                        select new SendFormGridVM
                        {
                            Id = s.Id,
                            CreatedUser = s.CreatedUser,
                            Departments = HrContext.CommaSeperatedNames("CompanyStructures", s.Departments, lang),
                            DeptLabel = targetDept,
                            Employees = HrContext.CommaSeperatedNames("People", s.Employees, lang),
                            EmpLabel = targetEmp,
                            FormId = s.FormId, 
                            ExpiryDate = s.ExpiryDate,
                            Jobs = HrContext.CommaSeperatedNames("Jobs", s.Jobs, lang),
                            JobLabel = targetJob,
                            SentDateTime = s.CreatedTime
                        };

            return query;
        }

        public SendFormPageVM GetFormPage(int companyId, string lang)
        {
            //if (id == 0) return new SendFormPageVM();

            //var model = context.SendForms.Find(id);
            //var vmodel = model == null ? new SendFormPageVM() : new SendFormPageVM
            //{
            //    Id = model.Id,
            //    ExpiryDate = model.ExpiryDate,
            //    FormId = model.FormId,
            //    Departments = model.Departments.Split(',').Select(int.Parse).ToList(),
            //    Jobs = model.Jobs.Split(',').Select(int.Parse).ToList(),
            //    Employees = model.Employees.Split(',').Select(int.Parse).ToList()
            //};

            var vmodel = from c in context.LookUpCodes
                         where c.Id == 1
                         select new SendFormPageVM
                         {
                             FormList = context.FlexForms.Select(a => new FormDropDown { id = a.Id, name = a.Name }).ToList(),
                             DeptList = context.CompanyStructures.Where(a => (a.CompanyId == companyId) && (a.StartDate <= DateTime.Today.Date && (a.EndDate == null || a.EndDate >= DateTime.Today.Date))).Select(a => new FormDropDown { id = a.Id, name = HrContext.TrlsName(a.Name, lang) }).ToList(),
                             JobList = context.Jobs.Where(a => (a.CompanyId == companyId || !a.IsLocal) && (a.StartDate <= DateTime.Today.Date && (a.EndDate == null || a.EndDate >= DateTime.Today.Date))).Select(a => new FormDropDown { id = a.Id, name = HrContext.TrlsName(a.Name, lang) }).ToList(),
                             EmpList = context.Assignments.Where(a => a.CompanyId == companyId && a.AssignDate <= DateTime.Today.Date && a.EndDate >= DateTime.Today.Date).Select(a => new FormDropDown { id = a.EmpId, name = HrContext.TrlsName(a.Employee.Title + " " + a.Employee.FirstName + " " + a.Employee.Familyname, lang) }).ToList()
                         };

            return vmodel.FirstOrDefault();
        }
    }
}
