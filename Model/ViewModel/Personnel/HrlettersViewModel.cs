using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class HrlettersViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string Culture { get; set; }
        public string ObjectName { get; set; }
        public byte Version { get; set; }
        public string LetterTempl { get; set; }
    }
}
