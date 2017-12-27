using Model.Domain;
using System.Collections.Generic;

namespace Model.ViewModel
{
    public class GridViewModel
    {
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public bool NewCompany { get; set; } = false;
        public string ObjectName { get; set; }
        public string TableName { get; set; }
        public int? MenuId { get; set; }
        public Menu Menu { get; set; }
        public string Lang { get; set; }
        public byte Version { get; set; } = 0;
        public byte newVersion { get; set; } = 0;
        public IEnumerable<ColumnTitle> columnTitles { get; set; }
        public IEnumerable<RoleColumns> roleColumns { get; set; }
        public IEnumerable<GridColumn> ColumnInfo { get; set; }
    }
}
