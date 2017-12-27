using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Administration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{

    public interface IAudiTrialRepository : IRepository<AudiTrail>
    {
        IQueryable<AuditViewModel> ReadAudiTrials(string culture,int companyId);
        IEnumerable<string> ReadLanguage();
        IQueryable<MsgTblViewModel> ReadMsgs(string culture);
        IQueryable<LanguageGridViewModel> ReadLang();
        void Add(MsgTbl msg);
        void Add(NameTbl name);
        void Attach(MsgTbl msg);
        void Remove(MsgTbl msg);
        DbEntityEntry<MsgTbl> Entry(MsgTbl msg);
        string CopyLanguage(string source, string destintion, int CompanyId, string culture);

    }
}
