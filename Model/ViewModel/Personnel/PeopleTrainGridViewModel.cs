using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class PeopleTrainGridViewModel
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        [MaxLength(150)]
        public string CourseTitle { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CourseSDate { get; set; } // Course start date

        [DataType(DataType.Date)]
        public DateTime? CourseEDate { get; set; } // Course end date
        public int? ActualHours { get; set; }
        // Course Cost
        public bool Internal { get; set; } // Internal Trainging
        public decimal? Cost { get; set; }
        public byte? Adwarding { get; set; } // 1- Company  2-Employee   3-Other

        [DataType(DataType.Date)]
        public DateTime? CantLeaveDate { get; set; } // The employee should pay training cost in case of resignation prior to the date
        public byte Status { get; set; } // 1- Ingoing - 2 - Completed  3-Fail

        [MaxLength(250)]
        public string Notes { get; set; }

        // /////////////////////////////
       
    }
}
