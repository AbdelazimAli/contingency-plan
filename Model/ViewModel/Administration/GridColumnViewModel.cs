using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Administration
{
   public class GridColumnViewModel
    {
        public GridColumn Column { get; set; }
        public int CompanyId { get; set; }
        public string ObjectName { get; set; }
        public byte Version { get; set; }
        public int GridId { get; set; }
    }
}
