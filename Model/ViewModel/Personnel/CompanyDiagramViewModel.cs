using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class CompanyDiagramViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Employee { get; set; }
        public int Code { get; set; }
        public string colorSchema { get; set; }
        public string Image { get; set; }
        public bool HasImage { get; set; }
        public short Gender { get; set; }
        public IList<CompanyDiagramViewModel> Children { get; set; }
        
    }
}
