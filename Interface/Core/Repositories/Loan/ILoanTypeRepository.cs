using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain;
using Model.ViewModel.Loans;

namespace Interface.Core.Repositories.Loan
{
   public interface ILoanTypeRepository : IRepository<LoanType>
    {
        IQueryable<LoanTypeViewModel> GetLoanType(string lang);
    }
}
