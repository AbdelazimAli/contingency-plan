using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApp.Extensions;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class MenuController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
    
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
                UserName = requestContext.HttpContext.User.Identity.Name;
            }
        }
        public MenuController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        public ActionResult Index1()
        {
            int Id = _hrUnitOfWork.Repository<Menu>().Select(a => a.Id).FirstOrDefault();
            var menu = _hrUnitOfWork.MenuRepository.GetMenu(Id, Language);
            ViewBag.Functions = _hrUnitOfWork.MenuRepository.Functions().ToList();
            return View(menu);
        }

        [HttpPost]
        public ActionResult Details(MenuViewModel model, OptionsViewModel moreInfo)
        {
            var Message = "OK";
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyStructureRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "MenuForm",
                        TableName = "Menus",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        foreach (var e in errors)
                            foreach (var errorMsg in e.errors)
                                ModelState.AddModelError(errorMsg.field, errorMsg.message);
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }
            }
            else
                return Json(Models.Utils.ParseFormErrors(ModelState));

            if (model.Id == 0) // New
            {
                Menu NewMenu = new Menu();
                UpdateModel(model, NewMenu, moreInfo);
                NewMenu.CompanyId = CompanyId;
                NewMenu.CreatedTime = DateTime.Now;
                NewMenu.CreatedUser = UserName;
                _hrUnitOfWork.MenuRepository.Add(NewMenu);
            }
            else
            {
                var OldMenu = _hrUnitOfWork.Repository<Menu>().Where(a => a.Id == model.Id).FirstOrDefault();
                UpdateModel(model, OldMenu, moreInfo);
                OldMenu.ModifiedTime = DateTime.Now;
                OldMenu.ModifiedUser = UserName;
                _hrUnitOfWork.MenuRepository.Attach(OldMenu);
                _hrUnitOfWork.MenuRepository.Entry(OldMenu).State = EntityState.Modified;
            }
            errors = SaveChanges(Language);
            if (errors.Count > 0)
                Message = errors.First().errors.First().message;
            return Json(Message);
        }
        private void UpdateModel(MenuViewModel model, Menu NewMenu, OptionsViewModel moreInfo)
        {
            _hrUnitOfWork.MenuRepository.AddLName(Language, NewMenu.Name + NewMenu.Version, model.MenuName + NewMenu.Version, model.Title);
            if (model.IFunctions != null && model.IFunctions.Count() > 0)
            {
                var MenuFunc = _hrUnitOfWork.Repository<MenuFunction>().Where(a => a.MenuId == NewMenu.Id).ToList();
                var Funclist = model.IFunctions.Select(a => a).ToList();
                foreach (var item in Funclist)
                {
                    if(!MenuFunc.Select(a=>a.FunctionId).Contains(item))
                    {
                       _hrUnitOfWork.MenuRepository.Add(new MenuFunction()
                        {
                            FunctionId = item,
                            Menu = NewMenu
                        });
                    }
                }
                foreach (var item in MenuFunc)
                {
                    if (!Funclist.Contains(item.FunctionId))
                    {
                        _hrUnitOfWork.MenuRepository.Remove(item);
                    }
                }

            }
            if (NewMenu == null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = NewMenu,
                    Source = model,
                    ObjectName = "MenuForm",
                    Version = NewMenu.Version,
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });
            }
            else
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = NewMenu,
                    Source = model,
                    ObjectName = "MenuForm",
                    Version = NewMenu.Version,
                    Options = moreInfo,
                    Transtype = TransType.Update
                });
            }
            NewMenu.Name = model.MenuName;
        }
        public ActionResult GetModel(int Id)
        {
            MenuViewModel MenuModel;
            if (Id != 0)
                MenuModel = _hrUnitOfWork.MenuRepository.GetMenu(Id,Language);
            else
                MenuModel = new MenuViewModel();
            return Json(MenuModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string Sort)
        {
            var Message = "OK";

            var list = _hrUnitOfWork.Repository<Menu>().Where(a => a.CompanyId == CompanyId && a.Sort.StartsWith(Sort)).ToList();

            var ids = list.Select(a => a.Id);
            var pages = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.CompanyId == CompanyId && ids.Contains(a.MenuId) && a.Version > 0);
            if (pages != null)
            {
                var titles = _hrUnitOfWork.PagesRepository.GetColumnTitles(CompanyId, ids);
                if (titles != null) _hrUnitOfWork.PagesRepository.RemoveRange(titles);
                _hrUnitOfWork.PagesRepository.RemoveRange(pages);
            }
            
            _hrUnitOfWork.MenuRepository.RemoveRange(list);

            try
            {
                var err = SaveChanges(Language);
                if (err.Count() > 0)
                {
                    foreach (var item in err)
                    {
                        Message = item.errors.Select(a => a.message).FirstOrDefault();
                    }
                    return Json(Message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg, JsonRequestBehavior.AllowGet);
            }

            return Json(Message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReadMenu()
        {
            return Json(_hrUnitOfWork.MenuRepository.ReadMenu(CompanyId, Language), JsonRequestBehavior.AllowGet);           
        }
        public ActionResult CopyGrid()
        {
            return View();
        }
        public ActionResult GetTree(int? id)
        {
            var node = _hrUnitOfWork.MenuRepository.GetTree(id, Language, CompanyId);
            return Json(node, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetParentMenus()
        {
            return Json(_hrUnitOfWork.MenuRepository.GetParentMenus(CompanyId, Language), JsonRequestBehavior.AllowGet);
        }             
        [HttpPost]
        public ActionResult DropMenuItem(MenuViewModel source, MenuViewModel dest)
        {
            IList<Error> errors = null;
            if (ModelState.IsValid)
            {
                string sort;

                if (dest.Sort == null)
                    sort = dest.Order.ToString().PadLeft(5, '0');
                else
                    sort = dest.Sort + dest.Order.ToString().PadLeft(5, '0');


                if (source.Id == 0)
                {
                    var menu = new Model.Domain.Menu
                    {
                       
                        CompanyId = source.CompanyId,
                        Name = source.MenuName,
                        Order = dest.Order,
                        NodeType = source.NodeType,
                        ParentId = dest.ParentId,
                        Sort = sort,
                        Url = source.Url,
                        Icon = source.Icon,
                        ColumnList=source.ColumnList
                    };

                    _hrUnitOfWork.MenuRepository.Attach(menu);
                    _hrUnitOfWork.MenuRepository.Entry(menu).State = EntityState.Added;
                }
                else
                {

                    var menu = new Model.Domain.Menu
                    {
                        Id = source.Id,
                        CompanyId = source.CompanyId,
                        Name = source.MenuName,
                        Order = dest.Order,
                        NodeType = source.NodeType,
                        ParentId = dest.ParentId,
                        Sort = sort,
                        Url = source.Url,
                        Icon = source.Icon,
                        ColumnList = source.ColumnList
                    };

                    _hrUnitOfWork.MenuRepository.Attach(menu);
                    _hrUnitOfWork.MenuRepository.Entry(menu).State = EntityState.Modified;
                }
                if (source.NodeType == 1)
                {
                    var childern = _hrUnitOfWork.MenuRepository.Find(m => m.ParentId == source.Id).ToList();
                    if (childern.Count > 0)
                    {
                        foreach (var child in childern)
                        {
                            child.Sort = sort + child.Order.ToString().PadLeft(5, '0');
                            _hrUnitOfWork.MenuRepository.Attach(child);
                            _hrUnitOfWork.MenuRepository.Entry(child).State = EntityState.Modified;
                        }
                    }
                }

                errors = SaveChanges(Language);
            }
            else
            {
                errors = Models.Utils.ParseErrors(ModelState.Values);
            }
            
            if (errors == null || errors.Count == 0)
                return Json("OK");
            else
                return Json(errors.FirstOrDefault().errors.FirstOrDefault().message);
        }
        [HttpPost]
        public ActionResult DropMenuItemCopy(MenuViewModel source, MenuViewModel dest)
        {
            IList<Error> errors = null;
            if (ModelState.IsValid)
            {
                string sort;

                if (dest.Sort == null)
                    sort = dest.Order.ToString().PadLeft(5, '0');
                else
                    sort = dest.Sort + dest.Order.ToString().PadLeft(5, '0');

                if (source.MenuName != dest.MenuName)
                {
                    ////string name = source.Name;
                    ////.MenuRepository.CopyMenuAndPage(source.Name);
                    //var pages = _hrUnitOfWork.Repository<PageDiv>().Where(p => p.MenuName == source.MenuName);
                    //foreach (var page in pages)
                    //{
                    //    if (page.DivType == "Form")
                    //        _hrUnitOfWork.PagesRepository.NewCopyFormDesign(page);
                    //    else if (page.DivType == "Grid")
                    //        _hrUnitOfWork.PagesRepository.NewCopyGridDesign(page);
                    //}                   
                }

                //   add

                if (source.Id == 0)
                {
                    var menu = new Model.Domain.Menu
                    {
                        CompanyId = source.CompanyId,
                        Name = source.MenuName,
                        Order = dest.Order,
                        NodeType =source.NodeType,
                        ParentId = dest.ParentId,
                        Sort = sort,
                        Url = source.Url,
                        Icon = source.Icon,
                        ColumnList=source.ColumnList                        
                    };

                    _hrUnitOfWork.MenuRepository.Attach(menu);
                    _hrUnitOfWork.MenuRepository.Entry(menu).State = EntityState.Added;
                }
                else
                {

                    var menu = new Model.Domain.Menu
                    {
                        Id = source.Id,
                        CompanyId = source.CompanyId,
                        Name = source.MenuName,
                        Order = dest.Order,
                        NodeType = source.NodeType,
                        ParentId = dest.ParentId,
                        Sort = sort,
                        Url = source.Url,
                        Icon = source.Icon,
                        ColumnList=source.ColumnList
                    };

                    _hrUnitOfWork.MenuRepository.Attach(menu);
                    _hrUnitOfWork.MenuRepository.Entry(menu).State = EntityState.Modified;
                }

                if (source.NodeType == 1)
                {
                    var childern = _hrUnitOfWork.MenuRepository.Find(m => m.ParentId == source.Id).ToList();
                    if (childern.Count > 0)
                    {
                        foreach (var child in childern)
                        {
                            child.Sort = sort + child.Order.ToString().PadLeft(5, '0');
                            _hrUnitOfWork.MenuRepository.Attach(child);
                            _hrUnitOfWork.MenuRepository.Entry(child).State = EntityState.Modified;
                        }
                    }
                }

                errors = SaveChanges(Language);
            }
            else
            {
                errors = Models.Utils.ParseErrors(ModelState.Values);
            }
            
            if (errors == null || errors.Count == 0)
                return Json("OK");
            else
                return Json(errors.FirstOrDefault().errors.FirstOrDefault().message);
        }
        [HttpPost]
        public ActionResult UpdateMenu(IEnumerable<MenuViewModel> models, IEnumerable<OptionsViewModel> options)
        {

            var datasource = new DataSource<MenuViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Menus",
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                _hrUnitOfWork.MenuRepository.SortMenu(models);
                var ids = models.Select(a => a.Id);
                var db_menus = _hrUnitOfWork.Repository<Menu>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var menu = db_menus.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new WebApp.Models.AutoMapperParm() { ObjectName = "Menus", Destination = menu, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Transtype=TransType.Update });
                    menu.ModifiedTime = DateTime.Now;
                    menu.ModifiedUser = UserName;

                    _hrUnitOfWork.MenuRepository.Attach(menu);
                    _hrUnitOfWork.MenuRepository.Entry(menu).State = EntityState.Modified;
                }

                _hrUnitOfWork.MenuRepository.UpdateTitles(models, Language);
                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        [HttpPost]
        public ActionResult CreateMenu(IEnumerable<MenuViewModel> models)
        {
            var result = new List<Model.Domain.Menu>();
            var datasource = new DataSource<MenuViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Menus",
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                _hrUnitOfWork.MenuRepository.SortMenu(models);

                //  Iterate all updated rows which are posted by the PageDiv
                foreach (MenuViewModel m in models)
                {
                    // Create a new branch entity and set its properties from branchViewModel
                    var menu = new Model.Domain.Menu
                    {
                        CompanyId = CompanyId,
                        Name = m.MenuName,
                        NodeType = m.NodeType,
                        Order = m.Order,
                        ParentId = m.ParentId,
                        Sort = m.Sort,
                        Url = m.Url,
                        Icon = m.Icon
                    };

                    result.Add(menu);
                    _hrUnitOfWork.MenuRepository.Add(menu);
                }

                _hrUnitOfWork.MenuRepository.UpdateTitles(models, Language);


                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from m in models
                               join r in result on m.MenuName equals r.Name
                               select new MenuViewModel
                               {
                                   Id = r.Id,
                                   CompanyId = m.CompanyId,
                                   MenuName = m.MenuName,
                                   NodeType = m.NodeType,
                                   Order = m.Order,
                                   ParentId = m.ParentId,
                                   Sort = m.Sort,
                                   ParentName = m.ParentName,
                                   Title = m.Title,
                                   Url = m.Url
                               })
                               .ToList();
           

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        [HttpPost]
        public ActionResult DeleteMenu(IEnumerable<MenuViewModel> models)
        {
            var datasource = new DataSource<MenuViewModel>();

            if (ModelState.IsValid)
            {
                foreach (MenuViewModel model in models)
                {
                    var menu = new Model.Domain.Menu
                    {
                        Id = model.Id
                    };
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Source = menu,
                        ObjectName = "Menus",
                        Transtype = TransType.Delete
                    });

                    _hrUnitOfWork.MenuRepository.Remove(menu);
                }

                datasource.Errors = SaveChanges(Language);
                datasource.Total = models.Count();
            }

            datasource.Data = models;

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult MenuTabs()
        {
            return View();
        }
        //Kendo:read ==>function
        public ActionResult ReadFunction(int Id)
        {
            return Json(_hrUnitOfWork.MenuRepository.GetFunctions(Id), JsonRequestBehavior.AllowGet);
        }
     
        public ActionResult IndexTreeList()
        {
            return View();
        }

    }
}