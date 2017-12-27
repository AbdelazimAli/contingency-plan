using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class WfViewModel
    {
        /// <summary>
        /// Leave Request Id or PK of the request
        /// </summary>
        public int WFlowId { get; set; } = 0;

        public int DocumentId { get; set; }

        /// <summary>
        /// Leave Type Id or PK of request type
        /// </summary>
        public int SourceId { get; set; }
        /// <summary>
        /// Request Source ex: Leave, Terminate
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Requestor Employee Id
        /// </summary>
        public int RequesterEmpId { get; set; }
        /// <summary>
        /// Any Valid Manager Id or Zero for employee direct manager
        /// </summary>
        public int? ManagerId { get; set; }

        public byte ApprovalStatus { get; set; }
        public bool BackToEmployee { get; set; } = false;
        public string CreatedUser { get; set; }
        /// <summary>
        /// Output: record inserted or status error
        /// </summary>
        public string WorkFlowStatus { get; set; } = "Success";
    }
}
