using System.Collections.Generic;

namespace Model.ViewModel.Personnel
{
    public class RequestWfFormViewModel
    {
        public int Id { get; set; }     
        public string Source { get; set; }
        public int SourceId { get; set; } // ex: leave type Id
        public byte HeirType { get; set; } = 1;
        public int? Hierarchy { get; set; }
        public byte? NofApprovals { get; set; }
        public byte? TimeOutDays { get; set; }
        public byte? TimeOutAction { get; set; } = 1;
        public bool ForceUpload { get; set; }
        public IList<RolesViewModel> Roles { get; set; }
        public IList<FormDropDown> Diagrams { get; set; }
    }
}
