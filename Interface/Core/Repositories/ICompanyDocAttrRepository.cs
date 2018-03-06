using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
   public interface ICompanyDocAttrRepository : IRepository<CompanyDocAttr>
    {
        IList<CompanyDocAttr> GetDocAttrByStreamId(Guid streamId);
    }
}
