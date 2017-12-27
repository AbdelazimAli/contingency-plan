using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
  public class CompanyStructureViewModel
    {
       
        public int Id { get; set; }
        public int Code { get; set; }//
        public int Leaveyear { get; set; } = new DateTime(DateTime.Now.Year).Year;
        [MaxLength(100)]
        public string Name { get; set; }
        public string LocalName { get; set; }
        public int? ParentId { get; set; }
        public string Sort { get; set; }
        public int Order { get; set; }
        public string ParentName { get; set; }

        [MaxLength(20)]
        public string ModifiedUser { get; set; }
     
        public DateTime? ModifiedTime { get; set; }
        public string CreatedUser { get; set; }

        public DateTime? CreatedTime { get; set; }
        public string ColorName { get; set; }
        public int? PlannedCount { get; set; }

        public byte NodeType { get; set; }
        [MaxLength(50)]
        public string Icon { get; set; }
       
        public bool IsVisible { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}
