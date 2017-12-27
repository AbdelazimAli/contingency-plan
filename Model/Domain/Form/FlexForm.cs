using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class FlexForm
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Purpose { get; set; }

        public int DesignedBy { get; set; }

        [ForeignKey("DesignedBy")]
        public Person Designer { get; set; }

        public byte FormType { get; set; } = 1; // Hidden field use as flag  1-Questionnaire  2-Interview   3-Test

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
