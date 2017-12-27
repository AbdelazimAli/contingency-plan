using Model.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class FormViewModel
    {
        [Key]
        public int Id { get; set; }
        public string FormId { get; set; }
        public int CompanyId { get; set; }
        public string ObjectName { get; set; }
        public string TableName { get; set; }
        public string Title { get; set; }
        public string JsMessages { get; set; }
        public byte Version { get; set; }
        public bool AllowInsert { get; set; }
        public string SessionVars { get; set; }
        public bool LogTooltip { get; set; }
        public IEnumerable CodesLists { get; set; }
        public IEnumerable<FieldSetViewModel> FieldSets { get; set; }
        public IEnumerable<string> HiddenColumns { get; set; }
        public IEnumerable<string> DisabledColumns { get; set; }
        public string TitleTrls { get; set; }
        public bool HasCustCols { get; set; }
    }
}
