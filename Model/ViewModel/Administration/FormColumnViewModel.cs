using Model.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class FormColumnViewModel
    {
        [Key]
        public int Id { get; set; }
        public string TableName { get; set; }
        public int SectionId { get; set; }
        public int CompanyId { get; set; }
        public string SectionDesc { get; set; }
        public int TempId { get; set; }
        public string label { get; set; }
        public byte? order { get; set; }
        public string name { get; set; }
        public bool isVisible { get; set; } = true;
        public byte? sm { get; set; } = null;
        public byte? md { get; set; } = null;
        public byte? lg { get; set; } = null;

        public string ColumnType { get; set; }
        public bool required { get; set; } = false;
        public int? min { get; set; } = null;
        public int? max { get; set; } = null;

        public string pattern { get; set; }
        public short? maxLength { get; set; }
        public byte? minLength { get; set; }

        public string placeholder { get; set; }

        public string HtmlAttribute { get; set; }

        public string type { get; set; }
        public string TypeText { get; set; }
        public string OrgInputType { get; set; }

        public bool isunique { get; set; }

        public string UniqueColumns { get; set; }
        [MaxLength(20)]
        public string CodeName { get; set; }
        public string DefaultValue { get; set; }
        public string Formula { get; set; }
        public int PageId { get; set; }
        public short? OrgMaxLength { get; set; }
    }
}
