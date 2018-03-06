using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
    public interface ISendFormRepository : IRepository<SendForm>
    {
        IQueryable<SendFormGridVM> GetForms(int company, string lang, string targetEmp, string targetDept, string targetJob);
        SendFormPageVM GetFormPage(int companyId, string lang);
    }
}
