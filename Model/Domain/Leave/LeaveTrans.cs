using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class LeaveTrans
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_LeaveTransCompany")]
        public int CompanyId { get; set; }

        [Index("IX_LeaveTrans", Order = 1)]
        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public LeaveType LeaveType { get; set; }

        [Index("IX_LeaveAbsenceType", Order = 1)]
        public short AbsenceType { get; set; }

        [Index("IX_LeaveTrans", Order = 2)]
        [Index("IX_LeaveAbsenceType", Order = 2)]
        public int PeriodId { get; set; }
        public Period Period { get; set; }

        [Index("IX_LeaveTrans", Order = 3)]
        [Index("IX_LeaveAbsenceType", Order = 3)]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public Person Employee { get; set; }

        public short TransType { get; set; }
        /// (Positive) 
        ///0-Open Balance رصيد افتتاحي
        ///1-Post Balance رصيد سابق مرحل 
        ///2-Accrual Balance استحقاق الرصيد
        ///3-Time Compensation  تعويضات الوقت
        ///4-Balance adjustment  تسوية إضافة للرصيد
        ///21-Cancel Leave  الغاءالاجازة
        ///22-Break Leave  قطع الاجازة
        ///23-Cancel Balance Deduction الغاء خصم رصيد
        ///24-Cancel Time Compensation  الغاء تعويضات الوقت
        /// (Negative) 
        /// 11-Leave  اجازة
        /// 12-Balance Replacement  استبدال رصيد
        /// 13-Balance Deduction  تسوية خصم من الرصيد
        /// 14-Cancel Balance adjustment الغاء اضافة رصيد
        public DateTime TransDate { get; set; } = DateTime.Now;
        public short TransFlag { get; set; }
        public float TransQty { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? ExpiryDate { get; set; }
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
