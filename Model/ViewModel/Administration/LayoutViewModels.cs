using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class LayOutViewModel
    {
        public IEnumerable<LayOutMenuViewModel> Menus { get; set; }
        public IEnumerable<LayOutMenuViewModel> Tabs { get; set; }
        public IEnumerable<LayOutCompanyViewModel> Companies { get; set; }
    }

    public class LayOutMenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public byte NodeType { get; set; }
        public byte? DataLevel { get; set; }
        public int? ParentId { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public bool SSMenu { get; set; } = false;
        public byte Version { get; set; } = 0;
        public string RoleId { get; set; }
    }

    public class LayOutCompanyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MegaMenu
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public List<SubModule> SubModules { get; set; }
    }

    public class SubModule
    {
        public int SubId { get; set; }
        public string SubName { get; set; }
        public int ModuleId { get; set; }
        public LayOutMenuViewModel MenuObj { get; set; }
        public List<LayOutMenuViewModel> MenuItems { get; set; }
    }
}
