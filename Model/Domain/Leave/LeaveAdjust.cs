using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class LeaveAdjust
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_LeaveAdjustCompany")]
        public int CompanyId { get; set; }

        [Index("IX_LeaveAdjust", Order = 1)]
        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public LeaveType LeaveType { get; set; }

        [Index("IX_LeaveAdjust", Order = 2)]
        public int PeriodId { get; set; }
        public Period Period { get; set; }

        [Index("IX_LeaveAdjust", Order = 3)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        [Column(TypeName = "Date")]
        public DateTime AdjustDate { get; set; } // today date can't change

        public short TransType { get; set; }
        /// (Positive) 
        ///1-Open Balance رصيد اول المدة
        ///2-Post Balance رصيد مرحل 
        ///3-Time Compensation  تعويضات الوقت
        ///4-Balance Addition  اضافة رصيد
        ///5-Cancel Leave  الغاءالاجازة
        ///6-Break Leave  قطع الاجازة
        ///10-Cancel Debit Adjustment الغاء تسويات مدينة
        /// (Negative) 
        /// 11-Leave  اجازة
        /// 12-Balance Replacement  استبدال رصيد
        /// 13-Balance Deduction  خصم من الرصيد
        /// 14-Cancel Credit Balance الغاء تسويات دائنة
        /// 
        public float NofDays { get; set; }
        //public bool Posted { get; set; } = false; // hidden Poseted: 0- Not Posted   1-Posted

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? WorkingDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ExpiryDate { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
        public bool PayDone { get; set; } = false;

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
