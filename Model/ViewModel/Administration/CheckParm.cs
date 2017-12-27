using System.Collections.Generic;

namespace Model.ViewModel
{
    public class CheckParm
    {
        public int CompanyId { get; set; }
        public string ObjectName { get; set; }
        public string TableName { get; set; }
        public IList<ColumnsView> Columns { get; set; }
        public string ParentColumn { get; set; }
        public string Culture { get; set; }
    }
}
