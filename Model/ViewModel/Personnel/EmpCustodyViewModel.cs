using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class EmpCustodyViewModel
    {
        public int Id { get; set; }
        public int EmpId { get; set; }      
        public int CustodyId { get; set; }
        public string Employee { get; set; }
        [DataType(DataType.Date)]
        public DateTime? RecvDate { get; set; } // Recieve Date
        public short? CustodyStat { get; set; } // look up code  1-New   2-Reused

        [DataType(DataType.Date)]
        public DateTime? delvryDate { get; set; } // Delivery date
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }


    }
}
