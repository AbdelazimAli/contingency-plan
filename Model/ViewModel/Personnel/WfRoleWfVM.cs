using System.Collections.Generic;

namespace Model.ViewModel.Personnel
{
    public class WfRoleWfVM
    {
        public IEnumerable<WfRoleViewModel> inserted { get; set; }
        public IEnumerable<WfRoleViewModel> updated { get; set; }
        public IEnumerable<WfRoleViewModel> deleted { get; set; }
    }
}
