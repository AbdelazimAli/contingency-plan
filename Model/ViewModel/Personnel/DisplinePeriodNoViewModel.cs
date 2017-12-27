using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class DisplinePeriodNoViewModel
    {
        public int Id { get; set; }      
        public int PeriodId { get; set; }     
        public int PeriodNo { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime PeriodSDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime PeriodEDate { get; set; }

        public bool Posted { get; set; } = false;
        public DateTime? PostDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
