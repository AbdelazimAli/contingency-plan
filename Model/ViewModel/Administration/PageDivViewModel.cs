using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class PageDivViewModel
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public byte Version { get; set; }
        [MaxLength(15)]
        public string DivType { get; set; } = "Grid";

        [Required, MaxLength(30)]
        public string ObjectName { get; set; }

        [MaxLength(30)]
        public string TableName { get; set; }
        public int MenuId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }
        

    }
}
