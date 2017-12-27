using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class HolidayViewModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public bool Standard { get; set; } = true;
        public byte? SDay { get; set; }
        public byte? SMonth { get; set; }
        public DateTime? HoliDate { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class WeekendViewModel
    {
        public byte Weekend1 { get; set; }
        public byte? Weekend2 { get; set; }
    }

    public class HolidayVM
    {
        public IEnumerable<HolidayViewModel> inserted { get; set; }
        public IEnumerable<HolidayViewModel> updated { get; set; }
        public IEnumerable<HolidayViewModel> deleted { get; set; }
    }
}
