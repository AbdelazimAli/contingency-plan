using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class WorkFlowObjectsViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [MaxLength(20)]
        public string Source { get; set; }
        public string DbSource { get; set; }
        public bool IsRequired { get; set; }
        [MaxLength(20)]
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
