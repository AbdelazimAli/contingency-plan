using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class CareerPathJobsViewModel
    {
        public int Id { get; set; }
        public int? FormulaId { get; set; }
        public int CareerId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public byte? MinYears { get; set; }
        public short? Performance { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public int? Sequence { get; set; }
    }
}
