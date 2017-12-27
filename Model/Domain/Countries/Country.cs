using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        [Index("IX_CountryName", IsUnique = true, Order = 1)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string NameAr { get; set; }

        //[MaxLength(50)]
        //public string NameFr { get; set; }
        [MaxLength(50)]
        public string Nationality { get; set; }

        public ICollection<DocType> DocType { get; set; }

        [MaxLength(50)]
        public string NationalityAr { get; set; }
        //public string NationalityFr { get; set; }

        [MaxLength(10)]
        public string TimeZone { get; set; }
        public bool DayLightSaving { get; set; } = false;

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
