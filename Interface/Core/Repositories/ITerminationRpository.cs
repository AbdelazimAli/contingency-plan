using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
   public interface ITerminationRpository :IRepository<Termination>
    {
        Dictionary<string, string> ReadMailEmpTerm(string Language, int Id);
        IQueryable<TerminationGridViewModel> ReadTermRequests(int companyId, string culture);
        TerminationFormViewModel ReadTermination(int id, string culture);
        IQueryable<TerminationGridViewModel> ReadTermsApproved(int companyId, string culture);
        IQueryable<TerminationGridViewModel> ReadTermFollowUp(int companyId, string culture);
        IQueryable<TermDurationViewModel> ReadTermDur(int companyId, int TermSysCode);
        void Add(TermDuration termDuration);
        void Attach(TermDuration termDuration);
        DbEntityEntry<TermDuration> Entry(TermDuration termDuration);
        void Remove(TermDuration termDuration);
        void DeleteRequest(int Id, int SourceId, string culture);
        MobileTerminationFormViewModel ReadEmpTermination(int EmpId, string culture, int companyId);
    }
}
