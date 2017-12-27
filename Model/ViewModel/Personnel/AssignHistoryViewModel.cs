using System;


namespace Model.ViewModel.Personnel
{
   public class AssignHistoryViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? AssignDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AssignStatus { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }
        public string Position { get; set; }
        public int AssignCodeId { get; set; }

    }
}
