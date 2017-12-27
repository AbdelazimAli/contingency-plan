using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain;
using Model.ViewModel;
using System.Collections;
using Model.ViewModel.Personnel;
using System.Data.Entity.Infrastructure;

namespace Interface.Core.Repositories
{
    public interface IHRLettersRepository:IRepository<HRLetter>
    {
        IEnumerable<HrlettersViewModel> EmpLetters(int CompanyId, string Culture);
        void Add(PagePrint pagePrint);
        void Attach(PagePrint pagePrint);
        DbEntityEntry<PagePrint> Entry(PagePrint pagePrint);
    }
}
