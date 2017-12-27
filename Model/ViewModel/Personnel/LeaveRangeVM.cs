using System.Collections.Generic;

namespace Model.ViewModel.Personnel
{
    public class LeaveRangeVM
    {
        public IEnumerable<ExcelGridLeaveRangesViewModel> inserted { get; set; }
        public IEnumerable<ExcelGridLeaveRangesViewModel> updated { get; set; }
        public IEnumerable<ExcelGridLeaveRangesViewModel> deleted { get; set; }
    }
}
