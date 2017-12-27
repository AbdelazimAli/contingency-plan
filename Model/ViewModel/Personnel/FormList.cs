using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class FormList
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool? isActive { get; set; } = true;
        public string PicUrl { get; set; }
        public int Icon { get; set; }
        public bool IsLocal { get; set; }
        public int value { get; set; }
        public string text { get; set; }
    }
}
