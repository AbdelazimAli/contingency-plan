using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class MailMergeViewmodel
    {
        public string Path { get; set; }
        public bool Exist { get; set; }
        public string Error { get; set; }
        public string ServerFilePath { get; set; }
    }
}
