using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class FlexDataViewModel
    {
        public int? Id { get; set; }
        public int PageId { get; set; } // the reference page Id == CompanyId + ObjectName + Version
        public int SourceId { get; set; } // The Record Key Ex: Position Id, Job Id, Person Id
        public string ObjectName { get; set; }
        public string TableName { get; set; }
        public byte Version { get; set; }

        [Required]
        public string ColumnName { get; set; }
        public string Title { get; set; }

        [MaxLength(250)]
        public string Value { get; set; }
        public string ValueText { get; set; }
        public int? ValueId { get; set; } // Used to register lookup code Id

        // column used in client side
        public byte InputType { get; set; }
        public string CodeName { get; set; }
        public bool Required { get; set; } = false;
        public int? Min { get; set; }
        public int? Max { get; set; }
        public string Pattern { get; set; }
        public string PlaceHolder { get; set; }
        public bool IsUnique { get; set; } = false;
        public string UniqueColumns { get; set; }
    }

    public class FormFlexColumnsViewModel
    {
        [Key]
        public int flexId { get; set; }
        public string TableName { get; set; }
        public int SectionId { get; set; }
        public int CompanyId { get; set; }
        public string label { get; set; }
        public byte? order { get; set; }
        public string name { get; set; }
        public bool isVisible { get; set; } = true;
        public byte? sm { get; set; } = null;
        public byte? md { get; set; } = null;
        public byte? lg { get; set; } = null;

        //public string ColumnType { get; set; }
        public bool required { get; set; } = false;
        public int? min { get; set; } = null;
        public int? max { get; set; } = null;

        public string pattern { get; set; }
        public short? maxLength { get; set; }
        public byte? minLength { get; set; }

        public string placeholder { get; set; }

        public string HtmlAttribute { get; set; }

        public string type { get; set; }

        public bool isunique { get; set; }

        public string UniqueColumns { get; set; }
        [MaxLength(20)]
        public string CodeName { get; set; }
        public string DefaultValue { get; set; }
        public int PageId { get; set; }
        public int SourceId { get; set; }
        public string Value { get; set; }
        public int? ValueId { get; set; }
        public string ValueText { get; set; }
        public string ObjectName { get; set; }
        public byte Version { get; set; }
        public byte InputType { get; set; }

    }

    public class FormFlexColumnsVM
    {
        public IEnumerable<FormFlexColumnsViewModel> FlexData { get; set; }
        public string Legend { get; set; }

        public IEnumerable<FormLookUpCodeVM> Codes { get; set; }
    }

    public class SaveFlexDataVM {
        public int flexId { get; set; }
        public int PageId { get; set; }
        public string TableName { get; set; }

        public string ColumnName { get; set; }
        public int SourceId { get; set; }

        [MaxLength(250)]
        public string Value { get; set; }
        public int? ValueId { get; set; }
    }
}
