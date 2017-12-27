using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Domain
{
    [Table("v_World")]
    public class World
    {
        [Key, Column(Order = 1)]
        public int CountryId { get; set; }

        [Key, Column(Order = 2)]
        public int CityId { get; set; }

        [Key, Column(Order = 3)]
        public int DistrictId { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
    }
}
