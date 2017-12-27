using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class MenuTreeListViewModel
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte MenuLevel { get; set; } = 0;
        public string MenuLevelText
        {
            get
            {
                switch (MenuLevel)
                {
                    case 1:
                        return "Page";
                    case 2:
                        return "Page Tab";
                    default:
                        return "Menu";
                }
            }
        }
        public string ParentName { get; set; }
        public int? ParentId { get; set; }
        public string Sort { get; set; }
        public byte Order { get; set; }
    }
}
