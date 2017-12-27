using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class PositionDiagram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public IList<string> Employee { get; set; }
        public byte? HeadCount { get; set; }
        public int? NoofHolder { get; set; }
        public string colorSchema { get; set; }
        public int? Relief { get; set; }
        public IList<PositionDiagram> Children { get; set; }
    }
}
