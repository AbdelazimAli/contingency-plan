using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class FormDesginViewModel
    {
        public string TableName { get; set; }
        public int? MenuId { get; set; }
        public string ObjectName { get; set; }
        public string Culture { get; set; }
        public string Title { get; set; }
        public byte Version { get; set; }
        public bool HasCustCols { get; set; } = false;
        public IEnumerable<ColumnTitle> ColumnTitles { get; set; }
        public IEnumerable<FieldSet> FieldSets { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public IEnumerable<string> DeletedColumnsIds { get; set; }
        public IEnumerable<int> DeletedSetsIds { get; set; }
    }
}
