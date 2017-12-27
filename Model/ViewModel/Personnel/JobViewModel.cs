using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class JobViewModel
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string Code { get; set; }
        public bool IsLocal { get; set; } = true;
        public string LName { get; set; }
        public int? DefaultGradeId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);
        public DateTime? EndDate { get; set; }
        public IEnumerable<int> IJobClasses { get; set; }
        public ICollection<JobClass> JobClasses { get; set; }
        public string NameInInsurance { get; set; }
        public short? PlanCount { get; set; }

        public float?  PlanTurnOverRate { get; set; }
        public short? ProbationPeriod { get; set; }

        public bool PrimaryRole { get; set; } = true;
        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime? EndTime { get; set; }
        public string ContractTempl { get; set; }
        public short? WorkHours { get; set; }
        public byte? Frequency { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
