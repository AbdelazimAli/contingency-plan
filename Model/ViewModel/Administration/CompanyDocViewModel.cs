using System;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class CompanyDocViewModel
    {
        [Key]
        public Guid stream_id { get; set; }
        public Guid Id { get; set; }
        public int CompanyId { get; set; }

        [MaxLength(255)]
        public string name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
        public int DocType { get; set; }

        [MaxLength(100)]
        public string Keyword { get; set; }
        public DateTime? ExpiryDate { get; set; }

        [MaxLength(128)]
        public string ModifiedUser { get; set; }
    }
}
