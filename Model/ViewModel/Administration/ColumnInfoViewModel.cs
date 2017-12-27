using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class ColumnInfoViewModel
    {
        [key]
        public int Id { get; set; }
        public string label { get; set; }
        public int CompanyId { get; set; }
        public int GridId { get; set; }
        public byte Version { get; set; } = 0;
        public string ObjectName { get; set; }

        public string TableName { get; set; }
        public int MenuName { get; set; }
        public string ColumnName { get; set; }
        public byte ColumnOrder { get; set; }
        public bool isVisible { get; set; } = true;
        public short DefaultWidth { get; set; }
        public string ColumnType { get; set; }
        public string OrgColumnType { get; set; }
        public bool Required { get; set; } = false;
        public bool OrgRequired { get; set; } = false;
        public int? Min { get; set; }
        public int? Max { get; set; }
        public string Pattern { get; set; }
        public short? MaxLength { get; set; }
        public byte? MinLength { get; set; }
        public short? OrgMaxLength { get; set; }
        public string PlaceHolder { get; set; }
        public string Custom { get; set; }
        public string InputType { get; set; }
        public string DataType { get; set; }
        public string OrgInputType { get; set; }
        public bool IsUnique { get; set; }
        public string UniqueColumns { get; set; }
        public string CodeName { get; set; }

        public string Message { get; set; }

        public short Row { get; set; }
        public short Cell  { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }
    }
}
