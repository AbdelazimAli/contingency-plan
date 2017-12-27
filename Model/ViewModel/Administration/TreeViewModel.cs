using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class TreeViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public byte Level { get; set; } = 0;
        public string ParentName { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public string Sort { get; set; }
        public int Order { get; set; }
        public bool hasChildren { get; set; }
        public string[] Msg { get; set; }
        public byte NodeType { get; set; }
        public byte? Version { get; set; }

    }
}
