using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Model.Domain.Payroll
{
    public class PayRequest
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_RequestNo", Order = 1)]
        [Index("IX_PayRequest", Order = 1)]
        public int CompanyId { get; set; }

        [Index("IX_RequestNo", Order = 2)]
        public int RequestNo { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Today;

        public byte PayMethod { get; set; } // 1-Cash  2-Cheque  3-Bank Transfer

        [Index("IX_PayRequest", Order = 2)]
        public byte ApprovalStatus { get; set; } = 1; // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected
        public bool Paid { get; set; } = false;

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? PayDate { get; set; }
        public short? RejectReason { get; set; } // lookup code PayRejectReason

        [MaxLength(250)]
        public string RejectDesc { get; set; }

        public short? CancelReason { get; set; } // lookup code PayCancelReason

        [MaxLength(250)]
        public string CancelDesc { get; set; }
        public int? WFlowId { get; set; }

        // Employee Selection
        public byte EmpSelect { get; set; } // 1- Specific departments  2-Specific employees

        [MaxLength(100)]
        public string Departments { get; set; }

        [MaxLength(100)]
        public string Employees { get; set; }

        // Payroll Selection

        public byte PaySelect { get; set; } // 1- Payroll group  2-Payroll  3-Specific salary items  4-Formula
        public short? PayrollGroup { get; set; } // lookup code (problem in multicompany)
        public int? PayrollId { get; set; }
        public Payrolls Payroll { get; set; }
       
        [MaxLength(100)]
        public string SalaryItems { get; set; } // SalaryItem

        public int? FormulaId { get; set; }
        public Formula Formula { get; set; }

        public float PayPercent { get; set; } = 1; // 100%
        public int Requester { get; set; }

        [ForeignKey("Requester")]
        public Person Employee { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class PayRequestDet
    {
        [Key, Column(Order = 1)]
        public int RequestId { get; set; }
        public PayRequest Request { get; set; }

        [Key, Column(Order = 2)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }
        public int? BankId { get; set; }
        public Provider Bank { get; set; }

        [MaxLength(50)]
        public string EmpAccountNo { get; set; } // Employee Account No
        public decimal PayAmount { get; set; }
        public bool Stopped { get; set; } = false;
    }
}
