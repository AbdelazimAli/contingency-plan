using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class JobClass
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_JobClassCode", IsUnique = true)]
        [MaxLength(20), Required]
        public string JobClassCode { get; set; }

        public bool IsLocal { get; set; } = false;
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [MaxLength(100), Required]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Notes { get; set; }

        public ICollection<Job> Jobs { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
