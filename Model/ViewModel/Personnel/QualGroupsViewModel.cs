using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class QualGroupsViewModel
    {
        public int Id { get; set; }
        public int? Code { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }


    }
    public class QualificationViewModel
    {
        public int Id { get; set; }
        public int? Code { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }
        public int? QualGroupId { get; set; }
      
        public short? Rank { get; set; }
        public short Category { get; set; }
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
    public class SchoolViewModel
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public short? SchoolType { get; set; }
        public short? Classification { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
