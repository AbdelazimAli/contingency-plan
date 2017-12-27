using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public class UserRoleVm
    {
        public IEnumerable<UserRoleViewModel> inserted { get; set; }
        public IEnumerable<UserRoleViewModel> updated { get; set; }
        public IEnumerable<UserRoleViewModel> deleted { get; set; }
    }
}
