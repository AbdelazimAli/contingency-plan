using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class ExtendOrFinishContractViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public bool Renew { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public int? RemindarDays { get; set; }
        public DateTime EndDate { get; set; }
    }
}
