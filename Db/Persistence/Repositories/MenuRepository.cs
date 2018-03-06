using System.Collections.Generic;
using System.Linq;
using Interface.Core.Repositories;
using Model.Domain;
using System.Data.Entity;
using Model.ViewModel;
using System.Collections;
using System.Data.Entity.Infrastructure;
using System;
using Model.ViewModel.Personnel;

namespace Db.Persistence.Repositories
{
    class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public string[] GetUserFunctions(string Role,int Menu)
        {
            return (from f in context.Functions
                    where f.RoleMenus.Any(a => a.MenuId == Menu && a.RoleId == Role)
                    select f.Name).ToArray();
        }
        public void UpdateTitles(IEnumerable<MenuViewModel> models, string culture)
        {
            var menuIds = models.Select(m => m.MenuName).ToList();

            var dbRows = (from t in context.NamesTbl
                          where t.Culture == culture && menuIds.Contains(t.Name)
                          select t).ToList();

            foreach (MenuViewModel title in models)
            {
                var modify = dbRows.FirstOrDefault(t => t.Name == title.MenuName);
                if (modify != null) // modified row
                {
                    if (modify.Title != title.Title) // title has been changed
                    {
                        modify.Title = title.Title;
                        context.NamesTbl.Attach(modify);
                        context.Entry(modify).State = EntityState.Modified;
                    }
                }
                else // new row
                {
                    var newTitle = new NameTbl
                    {
                        Name = title.MenuName,
                        Title = title.Title,
                        Culture = culture
                    };

                    context.NamesTbl.Add(newTitle);
                }
            }
        }
        //func sort menu 
        public void SortMenu(IEnumerable<MenuViewModel> models)
        {
            var menuIds = models.Select(m => m.ParentId).DefaultIfEmpty().ToList();

            var dbRows = context.Menus.
                Where(m => menuIds.Contains(m.Id))
                .Select(m => new
                {
                    Id = m.Id,
                    Sort = m.Sort
                })
                .DefaultIfEmpty()
                .ToList();

            foreach (MenuViewModel menu in models)
            {
                string sort;
                var parent = (menu.ParentId == null ? null : dbRows.SingleOrDefault(m => m.Id == menu.ParentId));

                if (parent != null)
                    sort = parent.Sort + menu.Order.ToString().PadLeft(5, '0');
                else
                    sort = menu.Order.ToString().PadLeft(5, '0');

                // cascade update menu childern
                var childern = context.Menus.
                    Where(m => m.Sort.StartsWith(menu.Sort) && m.Id != menu.Id)
                   .ToList();

                foreach (Menu child in childern)
                {
                    child.Sort = sort + child.Sort.Substring(sort.Length);
                    context.Menus.Attach(child);
                    context.Entry(child).State = EntityState.Modified;
                }
                // update model
                menu.Sort = sort;
            }
        }

        public IQueryable<MenuViewModel> ReadMenu(int companyId, string culture)
        {
            var menus = from m1 in context.Menus
                        where m1.CompanyId == companyId
                        join m2 in context.Menus on m1.ParentId equals m2.Id into g1
                        join t in context.NamesTbl on m1.Name equals t.Name into g2
                        from t in g2.Where(gg => gg.Culture == culture).DefaultIfEmpty()
                        from m2 in g1.DefaultIfEmpty()
                        join t2 in context.NamesTbl on m2.Name equals t2.Name into g3
                        from t2 in g3.Where(ggg => ggg.Culture == culture).DefaultIfEmpty()
                        orderby m1.Sort
                        select new MenuViewModel
                        {
                            Id = m1.Id,
                            CompanyId = m1.CompanyId,
                            MenuName = m1.Name,
                            Version = m1.Version,
                            Title = HrContext.TrlsName(m1.Name + m1.Version, culture),
                            NodeType = m1.NodeType,
                            Order = m1.Order,
                            ParentId = m1.ParentId,
                            ParentName = (t2.Title == null ? (m2.Name == null ? "" : m2.Name) : t2.Title),
                            Sort = m1.Sort,
                            Url = m1.Url,
                            Icon = m1.Icon,
                            ColumnList = m1.ColumnList,
                            WhereClause = m1.WhereClause
                        };
            return menus;
        }

        public IQueryable GetTree(int? id, string culture, int companyId)
        {
            var menus = (from m in context.Menus
                        where m.ParentId == id && m.CompanyId==companyId
                        join m2 in context.Menus on m.Id equals m2.ParentId into g2
                        from m2 in g2.DefaultIfEmpty()
                        join t in context.NamesTbl on m.Name equals t.Name into g
                        from t in g.Where(a => a.Culture == culture).DefaultIfEmpty()
                        select new  
                        {
                            Id = m.Id,
                            CompanyId = m.CompanyId,
                            Name = HrContext.TrlsName(m.Name, culture),
                            MenuLevel = m.NodeType,
                            Order = m.Order,
                            Title = (t.Title == null ? m.Name : t.Title),
                            ParentId = m.ParentId,
                            Sort = m.Sort,
                            Url = m.Url,
                            Icon = m.Icon,
                            hasChildren = m2 != null
                        }).Distinct()
                        .OrderBy(r => r.Sort);

            return menus;
        }

        public IEnumerable<RoleMenuViewModel> ReadRoleMenu(int companyId, string culture, string RoleId)
        {
            var menus = (from m in context.Menus where m.CompanyId == companyId && m.IsVisible //&& !m.Config
                        join rm in context.RoleMenus on m.Id equals rm.MenuId into g1
                        from rm in g1.Where(s => s.RoleId == RoleId).DefaultIfEmpty()
                        join t in context.NamesTbl on m.Name + m.Sequence equals t.Name into g2
                        from t in g2.Where(a => a.Culture == culture).DefaultIfEmpty()                        
                        orderby m.Sort
                        select new RoleMenuViewModel
                        {
                            RoleId = rm.RoleId,
                            Id = m.Id,
                            IsChecked = rm != null,
                            MenuLevel = m.NodeType,
                            DataLevelId = rm.DataLevel,
                            NodeType = m.NodeType,
                            Title = (t.Title == null ? m.Name : t.Title),
                            //Title= HrContext.TrlsName(m.Name + m.Version, culture),
                            FuncList = rm.Functions.Select(b => b.Name),
                            Sort = m.Sort,
                            Icon = m.Icon,
                            ModifiedUser=rm.ModifiedUser,
                            ModifiedTime= rm.ModifiedTime,
                            CreatedTime= rm.CreatedTime,
                            CreatedUser= rm.CreatedUser
                        }).ToList();
          
            return menus;
        }
        public MenuViewModel GetMenu(int Id,string culture)
        {
            var menu = (from m in context.Menus
                        where m.Id == Id
                        select new MenuViewModel
                        {
                            Id = m.Id,
                            ColumnList = m.ColumnList,
                            CompanyId = m.CompanyId,
                            Icon = m.Icon,
                            NodeType = m.NodeType,
                            MenuName = m.Name,
                            Order = m.Order,
                            ParentId = m.ParentId,
                            ParentName = HrContext.TrlsName(m.Parent.Name, culture),
                            Sort = m.Sort,
                            Url = m.Url,
                            WhereClause = m.WhereClause,
                            Title = HrContext.TrlsName(m.Name + m.Version, culture),
                            IsVisible = m.IsVisible,
                            SSMenu = m.SSMenu,
                            Version = m.Version,
                            Sequence = m.Sequence,
                            CreatedTime = m.CreatedTime,
                            ModifiedTime = m.ModifiedTime,
                            CreatedUser = m.CreatedUser,
                            ModifiedUser = m.ModifiedUser,
                            IFunctions = context.MenuFunctions.Where(a => a.MenuId == m.Id).Select(c => c.FunctionId).ToList()
                        }).FirstOrDefault();
            return menu;
        }
        public IEnumerable GetParentMenus(int companyId, string culture)
        {
            var menus = from m in context.Menus where m.CompanyId==companyId
                        join t in context.NamesTbl on m.Name equals t.Name into g
                        from t in g.Where(gg => gg.Culture == culture).DefaultIfEmpty()
                        orderby m.Sort
                        select new
                        {
                            id = m.Id,
                            name = (t.Title == null ? m.Name : t.Title),
                            level = m.NodeType
                        };

            return menus;
        }
        //public LayOutViewModel GetLayOut(int companyId, string culture)
        //{
        //    LayOutViewModel result = new LayOutViewModel
        //    {
        //        Companies = context.Companies.Select(c => new LayOutCompanyViewModel { Id = c.Id, Name = c.Name }).ToList(),
        //        Menus = (from m in context.Menus
        //                 where m.CompanyId == companyId && m.MenuLevel < 2
        //                 join t in context.NamesTbl on m.Name equals t.Name into g
        //                 from t in g.Where(gg => gg.Culture == culture).DefaultIfEmpty()
        //                 orderby m.Sort
        //                 select new LayOutMenuViewModel
        //                 {
        //                     Id = m.Id,
        //                     Name = (t.Title == null ? m.Name : t.Title),
        //                     MenuType = m.MenuType,
        //                     Url = m.Url,
        //                     Icon = m.Icon
        //                 }).ToList(),
        //        Tabs = (from m in context.Menus
        //                where m.CompanyId == companyId && m.MenuLevel == 2
        //                join m2 in context.Menus on m.ParentId equals m2.Id
        //                join t in context.NamesTbl on m.Name equals t.Name into g
        //                from t in g.Where(gg => gg.Culture == culture).DefaultIfEmpty()
        //                orderby m.Sort
        //                select new LayOutMenuViewModel
        //                {
        //                    Id = m.Id,
        //                    Name = (t.Title == null ? m.Name : t.Title),
        //                    Icon = m.Icon,
        //                    Order = m.Order,
        //                    ParentName = m2.Name
        //                }).ToList()
        //    };

        //    return result;
        //}
        public void UpdateRoleMenu(IEnumerable<RoleMenuViewModel> models, string userName)
        {
            if (models == null) return;

            var menuIds = models.Select(m => m.Id).ToList();
            var roleId = models.Select(r => r.RoleId).First();
            var RoleMenus = context.RoleMenus.Include("Functions").Where(a => menuIds.Contains(a.MenuId) && a.RoleId == roleId).ToList();
            var functions = context.Functions.ToList();

            foreach (RoleMenuViewModel m in models)
            {
                var RoleMenuObj = RoleMenus.FirstOrDefault(a => a.MenuId == m.Id && a.RoleId == m.RoleId);
                var funclist = new List<Function>();

                if (m.FuncList != null && m.FuncList.Count() > 0)
                {
                    var list = m.FuncList.Select(a => a.Split(',')).ToList();
                    foreach (var name in list)
                    {
                        var fun = functions.Where(f => name.Contains(f.Name)).ToList();
                        if (fun != null && fun.Count > 0) funclist.AddRange(fun);
                    }
                }

                //delete RoleMenu
                if (!m.IsChecked && RoleMenuObj != null)
                {
                    context.RoleMenus.Remove(RoleMenuObj);
                    context.Entry(RoleMenuObj).State = EntityState.Deleted;
                }
                //insert RoleMenu
                else if (m.IsChecked && RoleMenuObj == null)
                {
                    RoleMenu record = new RoleMenu
                    {
                        MenuId = m.Id,
                        RoleId = m.RoleId,
                        DataLevel = m.DataLevelId,

                    };
                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = userName;
                    record.Functions = funclist;
                    context.RoleMenus.Add(record);
                }

                //update RoleMenu
                else if (m.IsChecked && RoleMenuObj != null)
                {
                    var dbFunc = RoleMenuObj.Functions.ToList();
                    if (funclist != null)
                    {
                        foreach (var func in funclist)
                        {
                            var fn = dbFunc.FirstOrDefault(f => f.Id == func.Id);
                            if (fn == null) RoleMenuObj.Functions.Add(func);
                        }
                    }
                    foreach (var func in dbFunc)
                    {
                        if (funclist != null)
                        {
                            var fn = funclist.FirstOrDefault(f => f.Id == func.Id);
                            if (fn == null) RoleMenuObj.Functions.Remove(func);
                        }
                        else
                           RoleMenuObj.Functions.Remove(func);
                        
                    }
                    RoleMenuObj.ModifiedTime = DateTime.Now;
                    RoleMenuObj.ModifiedUser = userName;
                    RoleMenuObj.DataLevel = m.DataLevelId;
                    context.RoleMenus.Attach(RoleMenuObj);
                    context.Entry(RoleMenuObj).State = EntityState.Modified;
                }
            }
        }
        public IQueryable<FormList> Functions()
        {
            return from m in context.Functions
                   select new FormList
                   {
                       id = m.Id,
                       name = m.Name         
                   };
        }
        public IQueryable<FunctionViewModel> GetFunctions(int Id)
        {
            var Functions = from m in context.MenuFunctions
                            where m.MenuId == Id
                            select new FunctionViewModel
                            {
                                Id = m.FunctionId,
                                MenuId = m.MenuId,
                                Name = m.Function.Name
                            };

            return Functions;
        }
        public void CopyRoleMenu(string id,string userName , string RoleId )
        {
            var MenuIds = context.RoleMenus.Where(a => a.RoleId == id).Select(a => a.MenuId).ToList(); 
            var RoleMenus = context.RoleMenus.Include("Functions").Where(a=>a.RoleId == id && MenuIds.Contains(a.MenuId)).ToList();

            foreach (var r in RoleMenus)
            {
                //insert RoleMenu                
                RoleMenu record = new RoleMenu
                {
                    MenuId = r.MenuId,
                    RoleId = RoleId,
                    DataLevel = r.DataLevel

                };
                record.WhereClause = r.WhereClause;
                record.CreatedTime = DateTime.Now;
                record.CreatedUser = userName;
                record.Functions = r.Functions.ToList();
                context.RoleMenus.Add(record);

            }
        }
        

        public string GetMenuRoleId(int MenuId, string user)
        {
            return context.Database.SqlQuery<string>("SELECT RoleMenus.RoleId FROM AspNetUsers INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN RoleMenus ON AspNetUserRoles.RoleId = RoleMenus.RoleId WHERE AspNetUsers.UserName = '" + user + "' AND RoleMenus.MenuId = " + MenuId + " ORDER BY RoleMenus.DataLevel DESC").FirstOrDefault();
        }
        //SELECT Name FROM dbo.AspNetRoles WHERE Id='9247dd36-92eb-40ce-8a55-3a959aaf7b59' 
        public string GetRoleNameById(string RoleId)
        {
            var query =  context.Database.SqlQuery<string>("SELECT Name FROM dbo.AspNetRoles WHERE Id='"+RoleId+"'");
            return query.FirstOrDefault();
        }

        public bool IsAllowTable(int MenuId, string user)
        {
            var count = context.Database.SqlQuery<int>("SELECT COUNT(1) FROM AspNetUsers INNER JOIN AspNetUserRoles ON AspNetUsers.Id = AspNetUserRoles.UserId INNER JOIN RoleMenus ON AspNetUserRoles.RoleId = RoleMenus.RoleId WHERE AspNetUsers.UserName = '" + user + "' AND RoleMenus.MenuId = " + MenuId + " And RoleMenus.DataLevel > 1 ").FirstOrDefault();
            return count > 0;
        }

        public void Add(MenuFunction function)
        {
            context.MenuFunctions.Add(function);
        }

        public void Attach(Function function)
        {
            context.Functions.Attach(function);
        }

        public void Remove(Function function)
        {
            if (Context.Entry(function).State == EntityState.Detached)
            {
                context.Functions.Attach(function);
            }
            context.Functions.Remove(function);
        }
        public void Attach(MenuFunction function)
        {
            context.MenuFunctions.Attach(function);
        }

        public void Remove(MenuFunction function)
        {
            if (Context.Entry(function).State == EntityState.Detached)
            {
                context.MenuFunctions.Attach(function);
            }
            context.MenuFunctions.Remove(function);
        }
        public void Remove(RoleMenu roleMenu)
        {
            if (Context.Entry(roleMenu).State == EntityState.Detached)
            {
                context.RoleMenus.Attach(roleMenu);
            }
            context.RoleMenus.Remove(roleMenu);
        }
        public void RemoveRange(IEnumerable<RoleMenu> entities)
        {
            Context.Set<RoleMenu>().RemoveRange(entities);
        }
        public List<RoleMenu> GetRoleMenue(string RoleId)
        {
            var RoleMenu = context.RoleMenus.Where(a => a.RoleId == RoleId).ToList();
            return RoleMenu;
        }
        public DbEntityEntry<Function> Entry(Function function)
        {
            return Context.Entry(function);
        }
    }
}


