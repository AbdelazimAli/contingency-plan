using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class DocType
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30), Required]
        [Index("IX_DocType", IsUnique = true)]
        public string Name { get; set; }

        public short DocumenType { get; set; } // Look Up User Code
        public byte AccessLevel { get; set; } // 0-Not Shared  1-Shared only   2-Shared and can be downloaded
        public bool IsLocal { get; set; }
        public int? CompanyId { get; set; }
        public byte? RequiredOpt { get; set; } // 0-Not Required 1-Required for all jobs  2-Required for some jobs
        public ICollection<Job> Jobs { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }
        public bool HasExpiryDate { get; set; }
        public int? NotifyDays { get; set; }

        public ICollection<Country> Nationalities { get; set; }
        public short? Gender { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class DocTypeAttr
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_DocTypeAttr", IsUnique = true, Order = 1)]
        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public DocType DocType { get; set; }

        [Index("IX_DocTypeAttr", IsUnique = true, Order = 2)]
        [MaxLength(100), Required]
        public string Attribute { get; set; }
        public byte InputType { get; set; }
       
        [MaxLength(20)]
        public string CodeName { get; set; }
        public bool IsRequired { get; set; } = false;
    }
}
