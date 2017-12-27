using Model.Domain;
using Interface.Core.Repositories;
using System.Data.Entity;
using Model.ViewModel.Personnel;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;

namespace Db.Persistence.Repositories
{
    class HRLettersRepository:Repository<HRLetter>,IHRLettersRepository
    {
        public HRLettersRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public IEnumerable<HrlettersViewModel> EmpLetters(int CompanyId, string Culture)
        {
            return context.Database.SqlQuery<HrlettersViewModel>("select Id, ISNULL(LetterTempl,'') LetterTempl, Title [Name], Culture, CompanyId, ObjectName, [Version], [Language] from (select r.Id, r.LetterTempl, dbo.fn_GetColumnTitle(0,'ar-EG',r.ObjectName,r.[Version],p.Title) Title, r.Culture, r.CompanyId, r.ObjectName, r.[Version], l.[Name] [Language] from PageDivs p, PagePrints r, Languages l where p.CompanyId = r.CompanyId and p.ObjectName = r.ObjectName and p.[Version] = r.[Version] and r.Culture = l.LanguageCulture and r.CompanyId = 0 union select distinct -1, null, p.Title, l.LanguageCulture, r.CompanyId, r.ObjectName, r.[Version], l.[Name] from PageDivs p, PagePrints r, Languages l where p.CompanyId = r.CompanyId and p.ObjectName = r.ObjectName and p.[Version] = r.[Version] and r.CompanyId = 0 and l.LanguageCulture not in (select a.Culture from PagePrints a where a.CompanyId = r.CompanyId and a.ObjectName = r.ObjectName and a.[Version] = r.[Version])) c order by c.ObjectName, c.Culture").ToList();            
        }
        public void Add(PagePrint pagePrint)
        {
            context.PagePrint.Add(pagePrint);
        }
        public void Attach(PagePrint pagePrint)
        {
            context.PagePrint.Attach(pagePrint);
        }
        public DbEntityEntry<PagePrint> Entry(PagePrint pagePrint)
        {
            return Context.Entry(pagePrint);
        }
    }
}
