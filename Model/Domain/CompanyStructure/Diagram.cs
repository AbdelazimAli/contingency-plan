using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class Diagram
    {
        [Key]
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string Color { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class DiagramNode
    {
        [Key, Column(Order = 1)]
        public int DiagramId { get; set; }
        public Diagram Diagram { get; set; }

        [Key, Column(Order = 2)]
        public int ParentId { get; set; }

        [Key, Column(Order = 3)]
        public int ChildId { get; set; }
    }
}
