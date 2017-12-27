using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
    public class MsgTblViewModel
    {
       
        public string Culture { get; set; }      
        public string Name { get; set; }
        public string Meaning { get; set; }
        public bool JavaScript { get; set; } = false;
         public int Id { get; set; }
    }
    public class LangTblViewModel
    {
        public string Source { get; set; }
        public string Destination { get; set; }


    }

}
