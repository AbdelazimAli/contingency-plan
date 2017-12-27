using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Payroll
{
    public class PayReqGridViewModel
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int RequestNo { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Today;
        public string Requester { get; set; }

        public byte PayMethod { get; set; } // 1-Cash  2-Cheque  3-Bank Transfer

        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected
        public bool Paid { get; set; } = false;

        public DateTime? PayDate { get; set; }

        public int? WFlowId { get; set; }


        // WorkFlow
        public string RoleId { get; set; }
        public int? PositionId { get; set; }
        public int? DeptId { get; set; }
        public int? BranchId { get; set; } // Branch = 2
        public int? SectorId { get; set; } // Sector = 3
        public int? AuthBranch { get; set; } // Branch = 2
        public int? AuthDept { get; set; }
        public int? AuthPosition { get; set; }
        public int? AuthEmp { get; set; }
        public string AuthEmpName { get; set; }
        public string AuthPosName { get; set; }
        public string AuthDeptName { get; set; }
        public float PayPercent { get; set; }
    }


    //form
    public class PayRequestViewModel
    {
        public int Id { get; set; }

        public int RequestNo { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Today;
        public int Requester { get; set; }
        public byte PayMethod { get; set; } // 1-Cash  2-Cheque  3-Bank Transfer

        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected
        public bool Paid { get; set; } = false;

        public DateTime? PayDate { get; set; }
        public short? RejectReason { get; set; } // lookup code PayRejectReason

        public string RejectDesc { get; set; }

        public short? CancelReason { get; set; } // lookup code PayCancelReason

        public string CancelDesc { get; set; }
        public int? WFlowId { get; set; }

        // Employee Selection
        public byte EmpSelect { get; set; } // 1- Specific departments  2-Specific employees

        public string Departments { get; set; }

        public string Employees { get; set; }

        // Payroll Selection

        public byte PaySelect { get; set; } // 1- Payroll group  2-Payroll  3-Specific salary items  4-Formula
        public short? PayrollGroup { get; set; } // lookup code (problem in multicompany)
        public int? PayrollId { get; set; }

        public string SalaryItems { get; set; } // SalaryItem

        public int? FormulaId { get; set; }

        public float PayPercent { get; set; } = 1; // 100%

        public bool submit { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string RequesterEmp { get; set; }
    }

    public class PayRequestEmpsViewModel
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string Employee { get; set; }
        public decimal PayAmount { get; set; }
        public int EmpId { get; set; }

        public int? BankId { get; set; }

        public string EmpAccountNo { get; set; } // Employee Account No
        public bool Stopped { get; set; } = false;
        public bool dirty { get; set; }
    }

}
