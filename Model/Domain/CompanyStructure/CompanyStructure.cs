using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class CompanyStructure
    {
        public int Id { get; set; }

        [Index("IX_StructureSort", IsUnique = true, Order = 1)]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [MaxLength(150), Required]
        [Index("IX_StructureSort", IsUnique = true, Order = 2)]
        public string  Sort { get; set; }

        public int Order { get; set; }

        [Index("IX_StructureCode")]
        public int Code { get; set; }

        [MaxLength(20)]
        public string ColorName { get; set; }

        [MaxLength(100)]
        [Index("IX_StructureName")]
        public string Name { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public CompanyStructure Parent { get; set; }

        public int? PlannedCount { get; set; }

        public byte NodeType { get; set; }
        public bool IsVisible { get; set; } = true;

        [MaxLength(50)]
        public string Icon { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        public float? MinAllowPercent { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }

}
