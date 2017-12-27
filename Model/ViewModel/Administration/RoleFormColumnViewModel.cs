using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public  class RoleFormColumnViewModel
    {

        public int Id { get; set; }
        public string  RoleId { get; set; }
        public string Title { get; set; }
        public string ColumnName { get; set; }
        public bool isVisible { get; set; } = false;
        public bool isEnabled { get; set; } = false;

    }
}
