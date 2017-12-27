using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class LeaveTypeViewModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public short AbsenceType { get; set; }
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public bool HasAccrualPlan { get; set; } = false;
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        public DateTime? EndDate { get; set; }

    }
    public class ExcelGridLeaveRangesViewModel
    {
        public int Id { get; set; }
        public int LeaveTypeId { get; set; }
        public short FromPeriod { get; set; }
        public short ToPeriod { get; set; }
        public float NofDays { get; set; }
        public byte MonthOrYear { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}

