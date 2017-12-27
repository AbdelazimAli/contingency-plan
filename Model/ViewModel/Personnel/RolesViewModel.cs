using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel.Personnel
{
    public class RolesViewModel
    {
        public Guid? RoleId { get; set; }
        public string text { get; set; }
        public short? CodeId { get; set; }

        public string value
        {
            get
            {
                return CodeId == null ? RoleId.ToString() : CodeId.ToString();
            }
        }
    }
}
