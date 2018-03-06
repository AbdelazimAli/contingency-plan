using Interface.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Db.Persistence.BLL
{
   public  class MissionRequestB
    {

        IHrUnitOfWork hrUnitOfWork;
        public MissionRequestB(IHrUnitOfWork _hrUnitOfWork)
        {
            hrUnitOfWork = _hrUnitOfWork;
        }
        private JsonResult Json(string v)
        {
            throw new NotImplementedException();
        }
    }
}
