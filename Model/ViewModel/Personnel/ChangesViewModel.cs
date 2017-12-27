using System;

namespace Model.ViewModel.Personnel
{
  public class ChangesViewmodel
    {
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public object Value { get; set; }
    }

    public class ChangesObject
    {

        public string objectname { get; set; }
        public int companyId { get; set; }
        public int version { get; set; }
        public string columnname { get; set; }
        public string sourceId { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string CreatedUser { get; set; }
    }
}
