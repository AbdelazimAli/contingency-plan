using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace Model.Domain
{
    public class CompanyDocsViews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid stream_id { get; set; }
        public byte[] file_stream { get; set; }
        public byte[] thumbs { get; set; }

        [MaxLength(255)]
        public string name { get; set; }
        public byte AccessLevel { get; set; }
        public string file_type { get; set; }
        public long? cached_file_size { get; set; }
        public DateTimeOffset? creation_time { get; set; }
        public DateTimeOffset? last_write_time { get; set; }
        public DateTimeOffset? last_access_time { get; set; }
        public bool? is_directory { get; set; } = false;

        [MaxLength(20)]
        public string Source { get; set; }
        public int? SourceId { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
        public int? TypeId { get; set; }

        [ForeignKey("TypeId")]
        public DocType DocType { get; set; }

        [MaxLength(100)]
        public string Keyword { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [MaxLength(128)]
        public string CreatedUser { get; set; }
        [MaxLength(128)]
        public string ModifiedUser { get; set; }

        private IList<CompanyDocAttr> _CompanyDocAttrs = new List<CompanyDocAttr>();
        public virtual IList<CompanyDocAttr> CompanyDocAttrs
        {
            get
            {
                return this._CompanyDocAttrs;
            }
        }
    }
}
