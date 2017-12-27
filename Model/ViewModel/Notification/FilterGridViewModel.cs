using Model.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Notification
{
    public class FilterGridViewModel
    {

        public int Id { get; set; }
        public int NotifyCondId { get; set; }
        public NotifyCondition NotifyCond { get; set; }
        public string AndOr { get; set; }

        public string ObjectName { get; set; }

        public byte Version { get; set; } = 0;

        public string ColumnName { get; set; }

        public string ColumnType { get; set; }

        public string Operator { get; set; } // >, >=, <, <=, =, StartWith, EndWith, Contains

        public string Value { get; set; }
        public string ValueText { get; set; }

    }

}
