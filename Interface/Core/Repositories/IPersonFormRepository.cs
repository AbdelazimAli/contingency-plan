using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
  public  interface IPersonFormRepository : IRepository<PersonForm>
    {
        IQueryable<FlexFormGridViewModel> ReadPersonForms(int formType, string culture, int EmpId);
        FlexFormViewModel GetFlexPersonForm(int SendFormfId, string culture, int EmpId);
        //IQueryable<PersonFormVM> GetFlexFormByFormId(int FlexFormId,string lang);
    }
}
