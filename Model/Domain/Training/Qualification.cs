using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class QualGroup
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_QualGroup")]
        public int Code { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class Qualification
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_Qualification")]
        public int Code { get; set; } // suggested to be required & unique

        [MaxLength(100), Required]
        public string Name { get; set; }
        public int? QualGroupId { get; set; } 
        public QualGroup QualGroup { get; set; }

        public short? Rank { get; set; }
        public short Category { get; set; } // look up user code

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
    public class School
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public short? SchoolType { get; set; }
        public short? Classification { get; set; }
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
