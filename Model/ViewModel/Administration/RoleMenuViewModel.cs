using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class RoleMenuViewModel
    {
        public string RoleId { get; set; }
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Title { get; set; }
        public byte MenuLevel { get; set; } = 0;
        public byte NodeType { get; set; } = 0;
        public bool HasChildern { get; set; }
        public byte? DataLevelId { get; set; }
        public string Icon { get; set; }
        public IEnumerable<string> FuncList { get; set; }
        public short? Page { get; set; }
        public string Sort { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }


    }
}
