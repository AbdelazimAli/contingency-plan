using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
   public  class WfRoleViewModel
    {
        public int Id { get; set; }
        public byte Order { get; set; }
        public int WFlowId { get; set; }
        public Guid? RoleId { get; set; }
        public short? CodeId { get; set; }
        public string Role { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }
}
