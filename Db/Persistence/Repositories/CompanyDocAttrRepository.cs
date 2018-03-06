using Interface.Core.Repositories;
using Model.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Persistence.Repositories
{
  public  class CompanyDocAttrRepository : Repository<CompanyDocAttr>, ICompanyDocAttrRepository
    {
        public CompanyDocAttrRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        public IList<CompanyDocAttr> GetDocAttrByStreamId(Guid streamId)
        {
            return context.CompanyDocAttrs.Where(a => a.StreamId == streamId).ToList();
        }

    }
}
