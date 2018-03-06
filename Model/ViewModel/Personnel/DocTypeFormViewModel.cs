using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class DocTypeFormViewModel
    {
        public int Id { get; set; }
        [MaxLength(30)]       
        public string Name { get; set; }
        public string LocalName { get; set; }
        public short DocumenType { get; set; } // Look Up User Code
        public byte AccessLevel { get; set; } = 2; // 0-Not Shared  1-Shared only   2-Shared and can be downloaded
        public bool IsLocal { get; set; }
        public int? CompanyId { get; set; }
        public byte? RequiredOpt { get; set; }// 0-Not Required 1-Required for all jobs  2-Required for some jobs
        public IEnumerable<int> IJobs { get; set; }
        public ICollection<Job> Jobs{ get; set; }
        public IEnumerable<int> INationalities { get; set; }
        public ICollection<Job> Nationalities { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = new DateTime(2000, 1, 1);

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public bool HasExpiryDate { get; set; }
        public int? NotifyDays { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public short? Gender { get; set; }
    }
}
