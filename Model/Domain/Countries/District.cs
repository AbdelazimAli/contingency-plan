using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class District
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Index("IX_DistrictName", IsUnique = true, Order = 1)]
        public int CityId { get; set; }
        public City City { get; set; }

        [Index("IX_DistrictName", IsUnique = true, Order = 2)]
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string NameAr { get; set; }

        //[MaxLength(50)]
        //public string NameFr { get; set; }
    }
}
