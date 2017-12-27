using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class WorkFlowGridViewModel
    {
        public int WorkFlowId { get; set; }
        public string Source { get; set; } // Leave
        public int SourceId { get; set; } // ex: leave type Id
        public int? DocumentId { get; set; }

        public int? Id { get; set; }
        public short? Sequence { get; set; }
        public byte? Order { get; set; }
        public string Role { get; set; }
        public short? CodeId { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Employee { get; set; }
        public byte? ApprovalStatus { get; set; }
    }
}
