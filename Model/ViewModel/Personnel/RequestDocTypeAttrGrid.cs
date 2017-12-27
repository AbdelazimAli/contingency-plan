using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class RequestDocTypeAttrGrid
    {
        public IEnumerable<DocTypeAttrViewModel> inserted { get; set; }
        public IEnumerable<DocTypeAttrViewModel> updated { get; set; }
        public IEnumerable<DocTypeAttrViewModel> deleted { get; set; }
    }
}
