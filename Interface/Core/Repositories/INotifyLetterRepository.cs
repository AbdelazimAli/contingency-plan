using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
   public interface INotifyLetterRepository : IRepository<NotifyLetter>
    {
        bool IsNotificationSent(int EmpID, DateTime EventDate, string NotifySource);
    }
}
