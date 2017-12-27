using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class NewCompany
    {
        public List<Menu> Menus { get; set; }
        public List<RoleMenu> RoleMenus { get; set; }
        public List<MenuFunction> MenuFunctions { get; set; }
        public List<PageDiv> Pages { get; set; }
        public Company Company { get; set; }
        public List<FieldSet> FieldSets { get; set; }
        public List<Section> Sections { get; set; }
        public List<FormColumn> FormColumns { get; set; }
        public List<GridColumn> GridColumns { get; set; }
        public List<ColumnTitle> ColumnTitles { get; set; }
    }
}
