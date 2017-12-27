using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class CompanyDocAttr
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid StreamId { get; set; }

        [ForeignKey("StreamId")]
        public CompanyDocsViews CompanyDocument { get; set; }

        public int AttributeId { get; set; }

        [ForeignKey("AttributeId")]
        public DocTypeAttr DocAttribute { get; set; }

        [MaxLength(250)]
        public string Value { get; set; }
        public int? ValueId { get; set; }

    }
}
