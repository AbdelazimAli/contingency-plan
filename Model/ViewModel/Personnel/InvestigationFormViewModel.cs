using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class InvestigationFormViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }       
        public DateTime InvestDate { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Accident { get; set; }
        [MaxLength(1000)]
        public string Defense { get; set; }
        public int? ViolationId { get; set; }
        public DateTime ViolDate { get; set; }
        [MaxLength(1000)]
        public string InvestResult { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }
        public IEnumerable<int> Employee { get; set; }
        public IEnumerable<int> Witness { get; set; }
        public IEnumerable<int> Judge { get; set; }
    }
}
