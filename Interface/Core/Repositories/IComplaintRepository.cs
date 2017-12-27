using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
    public interface IComplaintRepository : IRepository<ComplainRequest>
    {
        IEnumerable<ComplaintIndexViewModel> GetComplaintRequest(int companyId,string culture);
        ComplaintRequestViewModel ReadComplaint(int id);
        IQueryable<ComplaintIndexViewModel> GetComplaintReqFollowUp(int companyId, string culture);
        ComplaintRequestViewModel GetRequest(int requestId, string culture);
    }
}