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
  public  class NotifyLetterRepository : Repository<NotifyLetter>, INotifyLetterRepository
    {
        public NotifyLetterRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        public bool IsNotificationSent(int EmpID, DateTime NotifyDate, string NotifySource)
        {
            try
            {
                return context.NotifyLetters.Any(n => n.EmpId == EmpID && n.NotifyDate == NotifyDate && n.NotifySource == NotifySource);
            }
            catch
            {
                return false;
            }
        }
    }
}
