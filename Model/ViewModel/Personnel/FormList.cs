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
        public short Gender { get; set; }
        public string PicUrl { get; set; }
        public int Icon { get; set; }
        public bool IsLocal { get; set; }

        private int _value;
        public int value { get { return id != 0 ? id : _value; } set { _value = value; } }

        private string _text;
        public string text { get { return name != null ? name : _text; } set {_text = value; } }
    }


    public class FormDropDown
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class StringDropDown
    {
        public string id { get; set; }
        public string name { get; set; }
    }


}
