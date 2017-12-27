using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class PersonViewModel
    {
        [Key]
        public int Id { get; set; }
        public string PicUrl { get; set; }
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [MaxLength(20)]       
        public string FirstName { get; set; }

        [MaxLength(20)]        
        public string Fathername { get; set; }
        [MaxLength(20)]
        public string GFathername { get; set; }
        [MaxLength(20)]
        [Required]
        public string Familyname { get; set; }
        public short? PersonType { get; set; }
        public int? CompanyId { get; set; }
        public int EmpStatus { get; set; }

        public short Gender { get; set; } // 1-Male 2-Female
        public DateTime? JoinDate { get; set; }
        public int? QualificationId { get; set; }
        public int? Age { get; set; }
        public string Code { get; set; }
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
