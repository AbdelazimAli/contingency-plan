using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Index("IX_CityName", IsUnique = true, Order = 1)]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [Required, MaxLength(50)]
        [Index("IX_CityName", IsUnique = true, Order = 2)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string NameAr { get; set; }

        //[MaxLength(50)]
        //public string NameFr { get; set; }
    }
}
