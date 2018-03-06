using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class FlexFormFS
    {
        public int Id { get; set; }
        public int FlexformId { get; set; }
        public FlexForm Flexform { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public byte FSOrder { get; set; } // hidden

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
