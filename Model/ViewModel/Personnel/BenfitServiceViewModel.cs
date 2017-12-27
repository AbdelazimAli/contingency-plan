using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class BenfitServiceViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? Code { get; set; }
        public string Name { get; set; }
        public bool IsGroup { get; set; } = false;
        public float? EmpPercent { get; set; }
        public float? CompPercent { get; set; }
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }
        public string Curr { get; set; }
        public int BenefitId { get; set; }

    }
}
