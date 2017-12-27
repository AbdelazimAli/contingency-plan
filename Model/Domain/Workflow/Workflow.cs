using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    [Table("Workflow")]
    public class Workflow
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [MaxLength(20)]
        public string Source { get; set; }
        public bool IsRequired { get; set; }

        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
