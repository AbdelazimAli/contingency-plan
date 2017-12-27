using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
   public class ExcelJobViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameInInsurance { get; set; }
        public string IsLocal { get; set; } 
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DefaultGradeId { get; set; }
        public string PlanCount { get; set; }
        public string PlanTurnOverRate { get; set; }
        public string PrimaryRole { get; set; }
        public string ProbationPeriod { get; set; }
        public string DescInRecruitment { get; set; }
        public string WorkHours { get; set; }
        public string Frequency { get; set; }

        public string StartTime { get; set; }
        public string LName { get; set; }
        [DataType(DataType.Time)]
        public DateTime? StarTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime? EnTime { get; set; }
        public string EndTime { get; set; }
        public string ReplacementRequired { get; set; }

    }
}
