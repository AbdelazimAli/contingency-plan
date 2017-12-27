using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Domain
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int FromEmpId { get; set; }
        public Person FromEmp { get; set; }
        public bool Sent { get; set; } = false;

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Body { get; set; }

        public bool All { get; set; }

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
    }

    public class MsgEmployee
    {
        [Key]
        public int Id { get; set; }
        public int FromEmpId { get; set; }
        public int ToEmpId { get; set; }
        public Person ToEmp { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }

        public bool Read { get; set; } = false;
    }
}
