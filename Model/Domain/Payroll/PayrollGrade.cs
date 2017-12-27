using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain.Payroll
{
    public class PayrollGrade
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_PayrollGrade", IsUnique = true)]
        public int Code { get; set; }

        [MaxLength(60), Required]
        public string Name { get; set; }

        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }

        [MaxLength(100)]
        public string Points { get; set; } // comma seperated point (PayScale lookup code)

        [MaxLength(20)]
        public string Group { get; set; }

        [MaxLength(20)]
        public string Grade { get; set; }

        [MaxLength(20)]
        public string SubGrade { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    //public class PayScale
    //{
    //    [Key]
    //    public int Id { get; set; }
    //    public int Seq { get; set; }

    //    [MaxLength(20)]
    //    public string Point { get; set; }
    //}
}
