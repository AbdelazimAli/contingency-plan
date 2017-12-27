using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public  class JobGridViewModel
    {
        [Key]
        public int Id { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public IEnumerable<string> JobClasses { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Islocal { get; set; }
        public int? CompanyId { get; set; }
        public string LocalName { get; set; }


    }
}
