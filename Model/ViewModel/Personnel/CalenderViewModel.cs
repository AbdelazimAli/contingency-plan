using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class CalenderViewModel
    {
        public byte? weekend1 { get; set; }
        public byte? weekend2 { get; set; }
        public short? WorkHours { get; set; }
        public DateTime? WorkStartTime { get; set; }
        public List<DateTime> CustomHolidays { get; set; }
        public List<HolidayViewModel>  StanderdHolidays { get; set; }
    }
}
