using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class LookUpViewModel
    {
        public string Id { get; set; } // Id = CodeName because table doesn't have Id
        [Required]
        public string CodeName { get; set; }
        public string Title { get; set; }
        public bool Protected { get; set; }
    }

    public class FormLookUpCodeVM
    {
        public string CodeName { get; set; }
        public short id { get; set; }
        public string name { get; set; }
        public int SysCode { get; set; }
        public bool isUserCode { get; set; }
        public bool isActive { get; set; } = true;
        public bool isFreez { get; set; } = false;
    }
}
