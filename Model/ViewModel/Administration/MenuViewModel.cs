using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? CopyId { get; set; }
        public string MenuName { get; set; }
        public string ObjectName { get; set; }
        public byte NodeType { get; set; }
        public bool IsVisible { get; set; }
        public bool SSMenu { get; set; }
        public int Order { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public string Url { get; set; }
        public string Sort { get; set; }
        public short Page { get; set; } = 0;
        public string Title { get; set; }
        public string Icon { get; set; }
        public string WhereClause { get; set; }
        public IEnumerable<int> IFunctions { get; set; }
        public string ColumnList { get; set; }
        public byte Version { get; set; }
        public short Sequence { get; set; } = 0;
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
