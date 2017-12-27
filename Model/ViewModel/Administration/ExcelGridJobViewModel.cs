using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class ExcelGridJobViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameInInsurance { get; set; }
        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        public DateTime? EndDate { get; set; }
        public int? DefaultGradeId { get; set; }
        public short? PlanCount { get; set; }
        public float? PlanTurnOverRate { get; set; }
        public bool PrimaryRole { get; set; } = false;
        public short? ProbationPeriod { get; set; }
        public string DescInRecruitment { get; set; }
        public short? WorkHours { get; set; }
        public byte? Frequency { get; set; }
        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }
        public string LName { get; set; }
        [DataType(DataType.Time)]
        public DateTime? EndTime { get; set; }
        public bool? ReplacementRequired { get; set; } = false;

       
    }
}
