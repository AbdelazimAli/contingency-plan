using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmpQualificationViewModel
    {

        public int Id { get; set; }
        public int EmpId { get; set; }
        public int? QualId { get; set; }
        
        [MaxLength(20)]
        public string SerialNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? GainDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }
        public int? SchoolId { get; set; } // Establishment
        public decimal? Cost { get; set; }
        public short? Awarding { get; set; } // look up code 1-Company  2-Employee  3-Donor
        public short? Grade { get; set; } // look up code Pass - Good - ...
        public decimal? Score { get; set; }
        public string Notes { get; set; }
        public string Title { get; set; }     

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinishDate { get; set; }
        public byte Status { get; set; } = 2; // 1- Ingoing - 2 - Completed
        public short? GradYear { get; set; }
        public bool IsQualification { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        //Mobile Part
        public string QualName { get; set; }
        public string SchoolName { get; set; }
        public string GradName { get; set; }
        public string StutesName { get; set; }
        public string AwardingName { get; set; }
        


    }
}
