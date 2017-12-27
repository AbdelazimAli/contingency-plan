using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Domain.Payroll;

namespace Model.Domain
{
    public class PeriodName
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Index("IX_CalendarName", IsUnique = true)]
        public string Name { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        public DateTime? EndDate { get; set; }
        public bool SingleYear { get; set; } 
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public byte PeriodLength { get; set; } = 0; // Period length in years
        public byte SubPeriodCount { get; set; } =1; // 0,1,2,3,4,6
       
        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class Period
    {
        public int Id { get; set; }

        [Index("IX_Period", IsUnique = true, Order = 1)]
        [Index("IX_PeriodName", IsUnique = true, Order = 1)]
        public int CalendarId { get; set; }
        public PeriodName Calendar { get; set; }

        public int PeriodNo { get; set; } // Max + 1

        public int? YearId { get; set; }

        [ForeignKey("YearId")]
        public FiscalYear FiscalYear { get; set; }

        [MaxLength(100)]
        [Index("IX_PeriodName", IsUnique = true, Order = 2)]
        public string Name { get; set; } // 2016, 2016-2018

        [DataType(DataType.Date), Column(TypeName = "Date")]
        [Index("IX_Period", IsUnique = true, Order = 2)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        [Index("IX_Period", IsUnique = true, Order = 3)]
        public DateTime EndDate { get; set; }

        public byte Status { get; set; } = 0; // 0-New  1-Opened  2-Closed

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }

    public class SubPeriod
    {
        public int Id { get; set; }

        [Index("IX_SubPeriod", IsUnique = true, Order = 1)]
        [Index("IX_SubPeriodName", IsUnique = true, Order = 1)]
        public int PeriodId { get; set; }
        public Period Period { get; set; }

        public int SubPeriodNo { get; set; } // Max + 10

        [MaxLength(100)]
        [Index("IX_SubPeriodName", IsUnique = true, Order = 2)]
        public string Name { get; set; } //3-2016, 6-2016


        [DataType(DataType.Date), Column(TypeName = "Date")]
        [Index("IX_SubPeriod", IsUnique = true, Order = 2)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date), Column(TypeName = "Date")]
        [Index("IX_SubPeriod", IsUnique = true, Order = 3)]
        public DateTime EndDate { get; set; }

        public byte Status { get; set; } // 0-New  1-Opened  2-Closed

        public int? PayDueId { get; set; }
        public PayrollDue PayDue { get; set; }
        public DateTime? CalcSalaryDate { get; set; } //Calulation date  must be working day
        public DateTime? PaySalaryDate { get; set; } // Pay Date

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
