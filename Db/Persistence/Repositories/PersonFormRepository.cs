using Interface.Core.Repositories;
using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model.ViewModel.Personnel;
using Model.ViewModel;

namespace Db.Persistence.Repositories
{
    public class PersonFormRepository : Repository<PersonForm>, IPersonFormRepository
    {
        public PersonFormRepository(DbContext context) : base(context)
        {
        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public IQueryable<FlexFormGridViewModel> ReadPersonForms(int formType, string culture, int EmpId)
        {
            var query = from f in context.FlexForms
                        join s in context.SendForms on f.Id equals s.FormId
                        join p in context.SendFormPeople on s.Id equals p.SendFormId
                        where f.FormType == formType && p.EmpId == EmpId
                        select new FlexFormGridViewModel
                        {
                            Id = s.Id,                              //SendForm Id
                            Name = f.Name,                          //Questionnair Name
                            Purpose = f.Purpose,                    //Questionnair Purpose
                            DesignedBy = HrContext.TrlsName(f.Designer.Title + " " + f.Designer.FirstName + " " + f.Designer.Familyname, culture),
                            SendCreationDate = s.CreatedTime,       //Questionnair Creation Date
                            ExpiryDate = s.ExpiryDate               //Questionnair Expiry Date
                        };

            return query;
        }

        public FlexFormViewModel GetFlexPersonForm(int SendFormId, string culture, int EmpId)
        {
            var qurey = (from f in context.FlexForms
                         join s in context.SendForms on f.Id equals s.FormId
                         join p in context.SendFormPeople on s.Id equals p.SendFormId
                         where p.SendFormId == SendFormId && p.EmpId == EmpId
                         select new FlexFormViewModel
                         {
                             Id = f.Id,
                             FormName = f.Name,
                             Purpose = f.Purpose,
                             DesignedBy = f.DesignedBy,
                             FormType = f.FormType,
                             SendFormId=s.Id,
                             ExpiryDate=s.ExpiryDate,
                             personForm = context.PersonForms.Where(pf => pf.SendFormId == SendFormId && pf.EmpId == EmpId).Select(pf =>
                             new PersonFormPageVM
                             {
                                 Id = pf.Id,
                                 FormId = pf.FormId,
                                 Question = pf.Question,
                                 FormColumnId = pf.FormColumnId,
                                 Answer = pf.Answer,
                                 OtherText = pf.OtherText
                             }).ToList(),
                             FieldSets = context.FlexFormFS.Where(fs => fs.FlexformId == f.Id).OrderBy(fs => fs.FSOrder).Select(fs =>
                             new FlexFormFSViewModel
                             {
                                 Id = fs.Id,
                                 order = fs.FSOrder,
                                 Description = fs.Description,
                                 Columns = context.FlexFormColumns.Where(fc => fc.FlexFSId == fs.Id).OrderBy(fc => fc.ColumnOrder).Select(fc =>
                                         new FlexFormColumnViewModel
                                         {
                                             Id = fc.Id,
                                             Name = fc.Name,
                                             ColumnOrder = fc.ColumnOrder,
                                             InputType = fc.InputType,
                                             Selections = fc.Selections,
                                             ShowTextBox = fc.ShowTextBox,
                                             ShowHint = fc.ShowHint,
                                             Hint = fc.Hint
                                         }).ToList()
                             }).ToList()
                         }).ToList();

            return qurey.FirstOrDefault();
        }
        //public IQueryable<PersonFormVM> GetFlexFormByFormId(int FlexFormId,string lang)
        //{
        //    var query = from s in context.PersonForms
        //                where s.Id == FlexFormId
        //                select new PersonFormVM
        //                {
        //                    Id = s.Id,
        //                    CreatedUser = s.CreatedUser,
        //                    Departments = HrContext.CommaSeperatedNames("CompanyStructures", s.Departments, lang),
        //                    DeptLabel = targetDept,
        //                    Employees = HrContext.CommaSeperatedNames("People", s.Employees, lang),
        //                    EmpLabel = targetEmp,
        //                    FormId = s.FormId,
        //                    ExpiryDate = s.ExpiryDate,
        //                    Jobs = HrContext.CommaSeperatedNames("Jobs", s.Jobs, lang),
        //                    JobLabel = targetJob,
        //                    SentDateTime = s.CreatedTime
        //                };

        //    return query;
        //}
    }
}
