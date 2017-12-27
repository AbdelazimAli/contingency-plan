using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class GridDesignViewModel
    {
        public string HiddenColumns { get; set; }
        public string ColumnTitles { get; set; }
        public string RolesColumns { get; set; }
        public string DisabledColumns { get; set; }
        public string ColumnInfo { get; set; }
        public string JsMessages { get; set; }
        public bool IsAllowInsert { get; set; }
    }
}
