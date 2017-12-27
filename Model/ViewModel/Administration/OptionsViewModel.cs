using System;
using System.Collections.Generic;

namespace Model.ViewModel
{
    public class OptionsViewModel
    {
        public IEnumerable<SaveFlexDataVM> flexData { get; set; }
        public IEnumerable<ListColumn> OldValues { get; set; }
        public IEnumerable<ListColumn> NewValues { get; set; }
        public List<string> VisibleColumns { get; set; }
    }

    public class ListColumn
    {
        public string ColumnName { get; set; }
        public string Text { get; set; }
    }

    public class NavBarItemVM
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public string MoreInfo { get; set; }
        public string PicUrl { get; set; }
        public bool Read { get; set; }
        public DateTime SentDate { get; set; }
    }
}
