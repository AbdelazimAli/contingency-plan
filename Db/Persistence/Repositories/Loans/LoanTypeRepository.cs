using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interface.Core.Repositories.Loan;
using System.Data.Entity;
using Model.ViewModel.Loans;

namespace Db.Persistence.Repositories.Loans
{
    class LoanTypeRepository : Repository<LoanType>, ILoanTypeRepository
    {
        public LoanTypeRepository(DbContext context) : base(context)
        {
        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public IQueryable<LoanTypeViewModel> GetLoanType(string lang)
        {
            var LoanTypes = from l in context.LoanTypes
                             select new LoanTypeViewModel
                             {
                                 Id = l.Id,
                                 Name = HrContext.TrlsName(l.Name, lang),
                                 StartDate=l.StartDate,
                                 EndDate=l.EndDate
                             };

            return LoanTypes;
        }
    }
}
