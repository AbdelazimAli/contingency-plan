using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class MessageViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int FromEmpId { get; set; }
        public bool Sent { get; set; } = false;

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Body { get; set; }
        public string TextBody { get; set; }

        public bool Check { get; set; }
        public bool All { get; set; }
        public IEnumerable<int> IPeopleGroups { get; set; } // comma seperated PeopleGroups
        public IEnumerable<int> IDepts { get; set; } // comma seperated Payrolls
        public IEnumerable<int> IJobs { get; set; } // comma seperated Jobs   
        public double Docs { get; set; }
        public IEnumerable<int> IEmployees{ get; set; }

        [MaxLength(50)]
        public string Jobs { get; set; }

        [MaxLength(50)]
        public string Depts { get; set; }

        [MaxLength(50)]
        public string PeopleGroups { get; set; }

        [MaxLength(100)]
        public string Employees { get; set; }

        public DateTime CreatedTime { get; set; }

        [MaxLength(20)]
        public string CreatedUser { get; set; }
        public int Attachments { get; set; }
    }
}
