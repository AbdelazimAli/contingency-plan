using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class LoanRequest
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public int EmpId { get; set; }
        public Person Emp { get; set; }
        public int? RequestNo { get; set; } // User Entry Or Max + 1

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime RequestDate { get; set; } 
        public int LoanTypeId { get; set; }
        public LoanType LoanType { get; set; }
        public decimal Amount { get; set; }
        public short InstallCnt { get; set; } // Installment Count
        public decimal InstallAmt { get; set; } // Installment Amount

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartPayDate { get; set; } // Start payment date

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? PaidDate { get; set; }

        [MaxLength(500)]
        public string LoanCause { get; set; }

        // Workflow /////////////////////////////
        public byte ApprovalStatus { get; set; } = 1; // 1- Draft 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected 

        // Time Stamp /////////////////////////////
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

  
}
