using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {
        string[] GetUserFunctions(string Role, int Menu);
        IQueryable<FormList> Functions();
        IQueryable<MenuViewModel> ReadMenu(int companyId, string culture);
        IEnumerable<RoleMenuViewModel> ReadRoleMenu(int companyId, string culture ,string RoleId,bool SSRole);
        void UpdateRoleMenu(IEnumerable<RoleMenuViewModel> models, string userName);
        IEnumerable GetParentMenus(int companyId, string culture);
        MenuViewModel GetMenu(int Id, string culture);
        void  UpdateTitles(IEnumerable<MenuViewModel> models, string culture);
        void SortMenu(IEnumerable<MenuViewModel> models);
        List<RoleMenu> GetRoleMenue(string RoleId);
      //  LayOutViewModel GetLayOut(int companyId, string culture);
      // void UpdateRoleMenu(IEnumerable<RoleMenuViewModel> models);
      // IEnumerable FunctionWithComma();
        IQueryable<FunctionViewModel> GetFunctions(int Id);
        void Add(MenuFunction function);
        void Attach(Function function);
        void Remove(Function function);
        void Attach(MenuFunction function);
        void Remove(MenuFunction function);
        DbEntityEntry<Function> Entry(Function function);
        IQueryable GetTree(int? id, string culture, int companyId);
        string GetMenuRoleId(int MenuId, string user);
        //Fatma
        string GetRoleNameById(string RoleId);
        //
        void CopyRoleMenu(string id, string userName, string RoleId);
        void Remove(RoleMenu roleMenu);
        void RemoveRange(IEnumerable<RoleMenu> entities);
        bool IsAllowTable(int MenuId, string user);

    }
}
