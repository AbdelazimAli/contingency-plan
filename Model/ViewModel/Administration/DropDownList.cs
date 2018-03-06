using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class DropDownList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PicUrl { get; set; } = "";
        public int Icon { get; set; }
        public short Gender { get; set; }
    }
}
