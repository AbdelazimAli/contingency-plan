using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class ManagerEmployeeDiagram
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string colorSchema { get; set; }
        public string PositionName { get; set; }
        public IList<ManagerEmployeeDiagram> Children { get; set; }
        public string Image { get; set; }
        public short Gender { get; set; }
        public bool isActive { get; set; }


    }
}
