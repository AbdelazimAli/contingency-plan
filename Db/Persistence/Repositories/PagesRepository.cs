using Interface.Core.Repositories;
using Model;
using Model.Domain;
using Model.ViewModel;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System;
using Model.ViewModel.Personnel;
using System.Data.Entity.Infrastructure;

namespace Db.Persistence.Repositories
{
    class PagesRepository : Repository<ColumnTitle>, IPagesRepository
    {
        public PagesRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        #region AddCompany
        private void AddMenu(NewCompany company0, Menu parent, int? parentId)
        {
            var nodes = company0.Menus.Where(m => m.ParentId == parentId);
            if (nodes == null)
                return;

            foreach (var node in nodes)
            {
                var menu = new Menu
                {
                    Name = node.Name,
                    Order = node.Order,
                    Sort = node.Sort,
                    Url = node.Url,
                    Icon = node.Icon,
                    Version = node.Version,
                    ColumnList = node.ColumnList,
                    IsVisible = node.IsVisible,
                    NodeType = node.NodeType,
                    SSMenu = node.SSMenu,
                    Company = company0.Company,
                    Parent = parent,
                    WhereClause = node.WhereClause,
                    Sequence = node.Sequence,
                 //   Grids = node.Grids,
                };

                context.Menus.Add(menu);

                var functions = new List<MenuFunction>();
                foreach (var func in company0.MenuFunctions.Where(a => a.MenuId == node.Id).ToList())
                {
                    var function = new MenuFunction
                    {
                        Menu = menu,
                        FunctionId = func.FunctionId
                    };

                    context.MenuFunctions.Add(function);
                    functions.Add(function);
                }


                foreach (var rm in company0.RoleMenus.Where(a => a.MenuId == node.Id))
                {
                    var role = new RoleMenu
                    {
                        RoleId = rm.RoleId,
                        Menu = menu,
                        DataLevel = rm.DataLevel,
                        Functions = rm.Functions
                    };

                    context.RoleMenus.Add(role);
                }

                // copy pages
                foreach (var page in company0.Pages.Where(p => p.MenuId == node.Id).ToList())
                {
                    var newpage = new PageDiv
                    {
                        TableName = page.TableName,
                        Company = company0.Company,
                        DivType = page.DivType,
                        Menu = menu,
                        ObjectName = page.ObjectName,
                        Version = page.Version,
                        HasCustCols = page.HasCustCols,
                        Title = page.Title
                    };
                    context.Set<PageDiv>().Add(newpage);

                    // copy all column titles
                    foreach (var title in company0.ColumnTitles.Where(a => a.ObjectName == page.ObjectName && a.Version == page.Version))
                    {
                        context.Set<ColumnTitle>().Add(new ColumnTitle
                        {
                            ColumnName = title.ColumnName,
                            Company = company0.Company,
                            Culture = title.Culture,
                            ObjectName = title.ObjectName,
                            Title = title.Title,
                            Version = page.Version
                        });
                    }

                    if (page.DivType == "Form")
                    {
                        // Copy Field Sets
                        var fieldSets = company0.FieldSets.Where(fs => fs.PageId == page.Id).ToList();

                        // Copy Sections
                        var ids = fieldSets.Select(fs => fs.Id);
                        var allSections = company0.Sections.Where(s => ids.Contains(s.FieldSetId)).ToList();

                        // Copy all columns
                        ids = allSections.Select(s => s.Id);
                        var allColumns = company0.FormColumns.Where(c => ids.Contains(c.SectionId)).ToList();

                        foreach (var fs in fieldSets)
                        {
                            var fieldset = GetNewFieldSet(newpage, fs, fs.Order);
                            context.Set<FieldSet>().Add(fieldset);

                            var sections = allSections.Where(s => s.FieldSetId == fs.Id);
                            foreach (var sn in sections)
                            {
                                var section = GetNewSection(fieldset, sn, sn.Order);
                                context.Set<Section>().Add(section);

                                var columns = allColumns.Where(c => c.SectionId == sn.Id);
                                foreach (var column in columns)
                                {
                                    context.Set<FormColumn>().Add(GetNewFormColumn(section, column));
                                }
                            }
                        }
                    }
                    else if (page.DivType == "Grid")
                    {
                        // copy all grid columns
                        foreach (var column in company0.GridColumns.Where(a => a.GridId == page.Id))
                        {
                            context.Set<GridColumn>().Add(NewGridColumn(column, newpage));
                        }
                    }
                }

                // Recurression call
                AddMenu(company0, menu, node.Id);
            }
        }

        //add company by Mamdouh
        public void AddCompany(Company company)
        {
            // turn off Auto Detect Changes to improve performance
            context.Configuration.AutoDetectChangesEnabled = false;

            var company0 = new NewCompany
            {
                Menus = context.Menus.Where(a => a.CompanyId == 0).ToList(),
                RoleMenus = context.RoleMenus.Include("Functions").Where(a => a.Menu.CompanyId == 0).ToList(),
                MenuFunctions = context.MenuFunctions.Where(a => a.Menu.CompanyId == 0).ToList(),
                Pages = context.PageDiv.Where(a => a.CompanyId == 0).ToList(),
                ColumnTitles = context.ColumnTitles.Where(a => a.CompanyId == 0).ToList(),
                FieldSets = context.FieldSets.Where(a => a.Page.CompanyId == 0).ToList(),
                Sections = context.Sections.Where(a => a.FieldSet.Page.CompanyId == 0).ToList(),
                FormColumns = context.FormsColumns.Where(a => a.Section.FieldSet.Page.CompanyId == 0).ToList(),
                GridColumns = context.GridColumns.Where(a => a.Grid.CompanyId == 0).ToList(),
                Company = company
            };


            context.Companies.Add(company);
            AddMenu(company0, null, null);

            // return it again on
            context.ChangeTracker.DetectChanges();
        }
        #endregion

        public ChartViewModel GetPageData(int companyId, string objectName, byte version)
        {
            return context.PageDiv.Where(frm => frm.CompanyId == companyId && frm.ObjectName == objectName && frm.Version == version).Select(c => new ChartViewModel
            {
                Id = c.Id,
                EmpId = c.CompanyId
            }).FirstOrDefault();
        }

        #region Form
        public FormViewModel GetFormInfo(int companyId, string objectName, byte version, string culture, string roleId)
        {
            //string culture = "en-GB";  //-ToDo: Culture from User Profile
            List<RoleFormColumnViewModel> columnRoles = context.RoleColumns.Where(rc => rc.CompanyId == companyId && rc.ObjectName == objectName && rc.Version == version && rc.RoleId == roleId).Select(rc => new RoleFormColumnViewModel { ColumnName = rc.ColumnName, RoleId = rc.RoleId, isVisible = rc.isVisible, isEnabled = rc.isEnabled }).ToList();
            List<string> hiddenColumns = columnRoles.Where(c => !c.isVisible).Select(c => c.ColumnName).ToList();
            List<string> disabledColumns = columnRoles.Where(c => !c.isEnabled).Select(c => c.ColumnName).ToList();

            var form = context.PageDiv
                .Where(frm => frm.CompanyId == companyId && frm.ObjectName == objectName && frm.Version == version)
                .Select(frm => new FormViewModel
                {
                    Id = frm.Id,
                    CompanyId = frm.CompanyId,
                    ObjectName = frm.ObjectName,
                    Title = frm.Title,
                    TitleTrls = HrContext.GetColumnTitle(companyId, culture, objectName, version, frm.Title),
                    Version = version,
                    HasCustCols = frm.HasCustCols,
                    HiddenColumns = hiddenColumns,
                    DisabledColumns = disabledColumns,
                    FieldSets = context.FieldSets
                   .Where(fs => fs.PageId == frm.Id)
                   .Select(fs => new FieldSetViewModel
                   {
                       Id = fs.Id,
                       layout = fs.LayOut,
                       order = fs.Order,
                       HasFieldSetTag = fs.HasTag,
                       LabelEditable = fs.Editable,
                       Collapsable = fs.Collapsable,
                       Collapsed = fs.Collapsed,
                       legend = fs.Legend,
                       legendTitle = HrContext.GetColumnTitle(companyId, culture, objectName, version, fs.Legend),
                       Freez = fs.Freeze,
                       Reorderable = fs.Reorderable,
                       Sections = context.Sections
                        .Where(s => s.FieldSetId == fs.Id)
                        .Select(s => new SectionViewModel
                        {
                            Id = s.Id,
                            order = s.Order,
                            name = s.Name,
                            layout = s.LayOut,
                            fieldsNumber = s.FieldsNumber,
                            labelsm = s.LabelSm,
                            labelmd = s.LabelMd,
                            labellg = s.LabelLg,
                            Freez = s.Freeze,
                            fields = context.FormsColumns
                            .Where(f => f.SectionId == s.Id && (f.isVisible || !String.IsNullOrEmpty(f.DefaultValue)))
                            .Select(f => new FormColumnViewModel
                            {
                                label = HrContext.GetColumnTitle(companyId, culture, objectName, version, f.ColumnName), //context.ColumnTitles.Where(ct => ct.CompanyId == companyId && ct.Culture == culture && ct.ObjectName == objectName && ct.Version == version && ct.ColumnName == f.ColumnName).Select(ct => new LabelViewModel { text = ct.Title }).ToList(),
                                order = f.ColumnOrder,
                                type = f.InputType,
                                name = f.ColumnName,
                                sm = f.Sm,
                                md = f.Md,
                                lg = f.Lg,
                                placeholder = f.PlaceHolder,
                                minLength = f.MinLength,
                                maxLength = f.MaxLength,
                                min = f.Min,
                                max = f.Max,
                                isunique = f.IsUnique,
                                UniqueColumns = f.UniqueColumns,
                                isVisible = f.isVisible,
                                required = f.Required,
                                pattern = f.Pattern,
                                HtmlAttribute = f.HtmlAttribute,
                                CodeName = f.CodeName,
                                DefaultValue = f.DefaultValue,
                                Formula = f.Formula
                            }).ToList()
                        }).ToList()
                   }).ToList()
                });

            var result = form.FirstOrDefault();
            if (result == null)
            {
                result = GetFormInfo(companyId, objectName, 0, culture, roleId);
                if (result == null) GetFormInfo(0, objectName, 0, culture, roleId);
            }

            return result;
        }

        public void ApplyFormDesignChanges(int companyId, FormDesginViewModel form)
        {
            // for new company
            PageDiv page = context.PageDiv.FirstOrDefault(pd => pd.CompanyId == companyId && pd.ObjectName == form.ObjectName && pd.Version == form.Version);
            if (page == null)
            {
                page = context.PageDiv.FirstOrDefault(pd => pd.Id == HrContext.GetPageId(companyId, form.ObjectName, form.Version));
                form.MenuId = page.MenuId;
                form.Title = page.Title;
                form.HasCustCols = page.HasCustCols;
                NewCompanyFormDesign(companyId, form);
            }
            else // Modify Existing form
            {
                if (form.FieldSets != null) ModifyFieldSetsOrder(form.FieldSets);
                if (form.Sections != null) ModifySectionsOrder(form.Sections);
                if (form.DeletedColumnsIds != null) RemoveFormColmns(companyId, form.ObjectName, form.Version, form.DeletedColumnsIds);
                if (form.DeletedSetsIds != null) RemoveFieldSets(companyId, form.DeletedSetsIds);


                if (form.ColumnTitles != null)
                    RenameColumns(new GridViewModel
                    {
                        CompanyId = companyId,
                        columnTitles = form.ColumnTitles,
                        ObjectName = form.ObjectName,
                        Version = form.Version,
                        Lang = form.Culture
                    });
            }
        }

        private FieldSet GetNewFieldSet(PageDiv page, FieldSet fs, byte order)
        {
            return new FieldSet
            {
                Collapsed = fs.Collapsed,
                Collapsable = fs.Collapsable,
                Editable = fs.Editable,
                Freeze = fs.Freeze,
                HasTag = fs.HasTag,
                LayOut = fs.LayOut,
                Legend = fs.Legend,
                Order = order,
                Page = page,
                Reorderable = fs.Reorderable
            };
        }
        private Section GetNewSection(FieldSet fieldset, Section section, byte order)
        {
            return new Section
            {
                FieldsNumber = section.FieldsNumber,
                Freeze = section.Freeze,
                LabelLg = section.LabelLg,
                LabelMd = section.LabelMd,
                LabelSm = section.LabelSm,
                LayOut = section.LayOut,
                Name = section.Name,
                Order = order,
                Reorderable = section.Reorderable,
                FieldSet = fieldset
            };
        }
        private FormColumn GetNewFormColumn(Section section, FormColumn column)
        {
            return new FormColumn
            {
                ColumnName = column.ColumnName,
                ColumnOrder = column.ColumnOrder,
                ColumnType = column.ColumnType,
                HtmlAttribute = column.HtmlAttribute,
                InputType = column.InputType,
                IsUnique = column.IsUnique,
                isVisible = column.isVisible,
                Lg = column.Lg,
                Max = column.Max,
                MaxLength = column.MaxLength,
                Md = column.Md,
                Min = column.Min,
                MinLength = column.MinLength,
                OrgInputType = column.OrgInputType,
                Pattern = column.Pattern,
                PlaceHolder = column.PlaceHolder,
                Required = column.Required,
                Sm = column.Sm,
                CodeName = column.CodeName,
                DefaultValue = column.DefaultValue,
                UniqueColumns = column.UniqueColumns,
                Section = section
            };
        }

        public void NewCopyFormDesign(PageDiv form, Menu menu, Company company)
        {
            var version = form.Version;
            if (company == null) // company != null if create new company else upgrade version
                version = (byte)(context.PageDiv.Where(a => a.CompanyId == form.CompanyId && a.ObjectName == form.ObjectName).Max(a => a.Version) + 1);

            menu.Version = (byte)version;

            // create new page
            var page = new PageDiv
            {
                TableName = form.TableName,
                CompanyId = form.CompanyId, // same company
                Company = company,
                DivType = form.DivType,
                Title = form.Title,
                Menu = menu,
                ObjectName = form.ObjectName,
                Version = (byte)(version) // new version
            };

            // Copy Field Sets
            var fieldSets = context.FieldSets.Where(fs => fs.PageId == form.Id).ToList();

            // Copy Sections
            var ids = fieldSets.Select(fs => fs.Id);
            var allSections = context.Sections.Where(s => ids.Contains(s.FieldSetId)).ToList();

            // Copy all columns
            ids = allSections.Select(s => s.Id);
            var allColumns = context.FormsColumns.Where(c => ids.Contains(c.SectionId)).ToList();

            // Copy Column titles
            var columnTitles = context.ColumnTitles.Where(t => t.CompanyId == form.CompanyId &&
           t.ObjectName == form.ObjectName && t.Version == form.Version).ToList();

            context.Set<PageDiv>().Add(page);

            foreach (var fs in fieldSets)
            {
                var fieldset = GetNewFieldSet(page, fs, fs.Order);
                context.Set<FieldSet>().Add(fieldset);

                var sections = allSections.Where(s => s.FieldSetId == fs.Id);
                foreach (var sn in sections)
                {
                    var section = GetNewSection(fieldset, sn, sn.Order);
                    context.Set<Section>().Add(section);

                    var columns = allColumns.Where(c => c.SectionId == sn.Id);
                    foreach (var column in columns)
                    {
                        context.Set<FormColumn>().Add(GetNewFormColumn(section, column));
                    }
                }
            }

            //Copy column titles
            CopyColumnTitles(columnTitles, form.CompanyId, company, page.Version);
        }

        private void CopyColumnTitles(IEnumerable<ColumnTitle> columnTitles, int companyId, Company company, byte version)
        {
            if (columnTitles == null || columnTitles.Count() == 0) return;
            //Copy column titles
            foreach (var title in columnTitles.ToList())
            {
                context.Set<ColumnTitle>().Add(new ColumnTitle
                {
                    ColumnName = title.ColumnName,
                    Company = company,
                    CompanyId = companyId,
                    Culture = title.Culture,
                    ObjectName = title.ObjectName,
                    Title = title.Title,
                    Version = version
                });
            }
        }

        public void NewCompanyFormDesign(IEnumerable<FormColumnViewModel> models)
        {
            // get any section       
            var sectionId = models.First().SectionId;
            var companyId = models.OrderBy(m => m.Id).First().CompanyId;

            // get page that contains this section
            var form = (from s in context.Sections
                        where s.Id == sectionId
                        join f in context.FieldSets on s.FieldSetId equals f.Id
                        join p in context.PageDiv on f.PageId equals p.Id
                        select p).FirstOrDefault();

            // create new page
            var page = new PageDiv
            {
                TableName = form.TableName,
                CompanyId = companyId, // new company
                DivType = form.DivType,
                Title = form.Title,
                ObjectName = form.ObjectName,
                MenuId = form.MenuId,
                Version = form.Version // Same Version

            };

            // Version Field Sets
            var fieldSets = context.FieldSets.Where(fs => fs.PageId == page.Id).ToList();

            //Version Sections
            var ids = models.Select(m => m.SectionId);
            var allSections = context.Sections.Where(s => ids.Contains(s.Id)).ToList();

            // Version all columns
            ids = models.Select(m => m.Id);
            var allColumns = context.FormsColumns.Where(c => ids.Contains(c.Id)).ToList();

            // Copy Column titles
            var columnTitles = context.ColumnTitles.Where(t => t.CompanyId == 0 &&
           t.ObjectName == form.ObjectName && t.Version == form.Version).ToList();

            context.Set<PageDiv>().Add(page);

            foreach (var fs in fieldSets)
            {
                var fieldset = GetNewFieldSet(page, fs, fs.Order);
                context.Set<FieldSet>().Add(fieldset);

                var sections = allSections.Where(s => s.FieldSetId == fs.Id);
                foreach (var sn in sections)
                {
                    var section = GetNewSection(fieldset, sn, sn.Order);
                    context.Set<Section>().Add(section);

                    var columns = allColumns.Where(c => c.SectionId == sn.Id);
                    foreach (var column in columns)
                    {
                        context.Set<FormColumn>().Add(GetNewFormColumn(section, column));
                    }
                }
            }

            //Copy column titles
            CopyColumnTitles(columnTitles, companyId, null, page.Version);
        }

        private void NewCompanyFormDesign(int companyId, FormDesginViewModel form)
        {
            var page = new PageDiv
            {
                TableName = form.TableName,
                CompanyId = companyId, // new company
                DivType = "Form",
                Title = form.Title,
                ObjectName = form.ObjectName,
                MenuId = form.MenuId.Value,
                Version = form.Version // same version
            };

            // Copy Field Sets
            var ids = form.FieldSets.Select(f => f.Id);
            var fieldSets = context.FieldSets.Where(fs => ids.Contains(fs.Id)).ToList();

            // Copy Sections
            ids = form.Sections.Select(f => f.Id);
            var allSections = context.Sections.Where(s => ids.Contains(s.Id)).ToList();

            // Copy all columns
            IList<FormColumn> allColumns;
            if (form.DeletedColumnsIds == null)
                allColumns = context.FormsColumns.Where(c => ids.Contains(c.SectionId)).ToList();
            else
                allColumns = context.FormsColumns.Where(c => ids.Contains(c.SectionId) && !form.DeletedColumnsIds.Contains(c.ColumnName)).ToList();

            context.Set<PageDiv>().Add(page);

            // Copy Column titles
            var columnTitles = context.ColumnTitles.Where(t => t.CompanyId == 0 &&
           t.ObjectName == form.ObjectName && t.Version == form.Version).ToList();

            foreach (var fs in fieldSets)
            {
                var fset = form.FieldSets.FirstOrDefault(f => f.Id == fs.Id);
                var fieldset = GetNewFieldSet(page, fs, fset == null ? fs.Order : fset.Order);
                context.Set<FieldSet>().Add(fieldset);

                var sections = allSections.Where(s => s.FieldSetId == fs.Id);
                foreach (var sn in sections)
                {
                    var sect = form.Sections.FirstOrDefault(f => f.Id == sn.Id);
                    var section = GetNewSection(fieldset, sn, sect == null ? sn.Order : sect.Order);
                    context.Set<Section>().Add(section);

                    var columns = allColumns.Where(c => c.SectionId == sn.Id);
                    foreach (var column in columns)
                    {
                        context.Set<FormColumn>().Add(GetNewFormColumn(section, column));
                    }
                }
            }

            //Copy column titles
            var titles = from t1 in columnTitles
                         join t2 in form.ColumnTitles
                         on new { t1.ColumnName, t1.Culture } equals new { t2.ColumnName, t2.Culture } into g
                         from t2 in g.DefaultIfEmpty()
                         select new ColumnTitle
                         {
                             ColumnName = t1.ColumnName,
                             Culture = t1.Culture,
                             ObjectName = t1.ObjectName,
                             Title = (t2 == null ? t1.Title : t2.Title)
                         };

            CopyColumnTitles(titles, companyId, null, page.Version);
        }

        void ModifyFieldSetsOrder(IEnumerable<FieldSet> fieldSets)
        {
            var ids = fieldSets.Select(ss => ss.Id);
            var dbFieldSets = context.FieldSets.Where(s => ids.Contains(s.Id)).ToList();
            foreach (FieldSet fieldSet in fieldSets)
            {
                var modify = dbFieldSets.Find(fs => fs.Id == fieldSet.Id);
                if (modify != null)
                {
                    modify.Order = fieldSet.Order;
                    context.Entry(modify).State = EntityState.Modified;
                }
            }
        }

        void ModifySectionsOrder(IEnumerable<Section> sections)
        {
            var ids = sections.Select(ss => ss.Id);
            var dbSections = context.Sections.Where(s => ids.Contains(s.Id)).ToList();
            foreach (Section section in sections)
            {
                var modify = dbSections.Find(s => s.Id == section.Id);
                if (modify != null)
                {
                    modify.Order = section.Order;
                    context.Entry(modify).State = EntityState.Modified;
                }
            }
        }

        private void RemoveFormColmns(int companyId, string objectName, byte version,
            IEnumerable<string> DeletedColumnsIds)
        {
            if (companyId > 0)
            {
                List<FormColumn> columns = context.FormsColumns
                    .Where(fc => fc.Section.FieldSet.Page.CompanyId == companyId
                    && fc.Section.FieldSet.Page.ObjectName == objectName
                    && fc.Section.FieldSet.Page.Version == version
                    && DeletedColumnsIds.Contains(fc.ColumnName))
                    .ToList();

                context.FormsColumns.RemoveRange(columns);
            }
        }

        private void RemoveFieldSets(int companyId, IEnumerable<int> DeletedColumnsIds)
        {
            if (companyId > 0)
            {
                List<FieldSet> sets = context.FieldSets
                    .Where(fs => DeletedColumnsIds.Contains(fs.Id))
                    .ToList();

                context.FieldSets.RemoveRange(sets);
            }
        }

        //Reset Grid & Form (Remove PageDiv and related entities)
        public void RemovePageDiv(int companyId, string objectName, byte version)
        {
            if (companyId > 0)
            {
                PageDiv pageDiv = context.PageDiv.FirstOrDefault(p => p.CompanyId == companyId && p.ObjectName == objectName && p.Version == version);
                if (pageDiv != null)
                {
                    if (Context.Entry(pageDiv).State == EntityState.Detached)
                    {
                        context.PageDiv.Attach(pageDiv);
                    }
                    context.PageDiv.Remove(pageDiv);

                    List<ColumnTitle> columnTitles = context.ColumnTitles.Where(t => t.CompanyId == companyId && t.ObjectName == objectName && t.Version == version).ToList();
                    RemoveRange(columnTitles);

                    List<RoleColumns> roleColumns = context.RoleColumns.Where(r => r.CompanyId == companyId && r.ObjectName == objectName && r.Version == version).ToList();
                    context.RoleColumns.RemoveRange(roleColumns);
                }
            }
        }

        public void RemoveRange(IEnumerable<PageDiv> pages)
        {
            context.PageDiv.RemoveRange(pages);
        }

        //role Form Columns
        public IQueryable<RoleFormColumnViewModel> GetRoleFormColumns(string RoleId, string objectName, byte version, int companyId, string culture)
        {
            // left outer 
            var query = from page in context.PageDiv
                        where page.CompanyId == companyId && page.ObjectName == objectName && page.Version == version
                        join column in context.FormsColumns on page.Id equals column.Section.FieldSet.PageId
                        where column.InputType != "none"
                        join rc in context.RoleColumns on new { j1 = companyId, j2 = objectName, j3 = version, j4 = RoleId, j5 = column.ColumnName }
                        equals new { j1 = rc.CompanyId, j2 = rc.ObjectName, j3 = rc.Version, j4 = rc.RoleId, j5 = rc.ColumnName } into g
                        from rc in g.DefaultIfEmpty()
                        select new RoleFormColumnViewModel
                        {
                            Id = column.Id,
                            RoleId = RoleId,
                            Title = HrContext.GetColumnTitle(companyId, culture, objectName, version, column.ColumnName),
                            ColumnName = column.ColumnName,
                            isEnabled = rc.ObjectName == null ? true : rc.isEnabled,
                            isVisible = rc.ObjectName == null ? true : rc.isVisible
                        };

            
            return query;
        }
        public void UpdateRoleFormColumns(IEnumerable<RoleFormColumnViewModel> models, string objectName, byte version, int companyId)
        {
            if (models == null) return;
            var roleId = models.Select(r => r.RoleId).First();
            var roleColumns = context.RoleColumns.Where(a => a.CompanyId == companyId && a.ObjectName == objectName && a.Version == version && a.RoleId == roleId).ToList();

            foreach (var model in models)
            {
                var rc = roleColumns.FirstOrDefault(a => a.ColumnName == model.ColumnName);
                //&& model.isVisible == false
                if (rc == null)
                {
                    //insert new record in Role columns
                    RoleColumns record = new RoleColumns
                    {
                        RoleId = model.RoleId,
                        isVisible = model.isVisible,
                        ColumnName = model.ColumnName,
                        isEnabled = model.isEnabled,
                        ObjectName = objectName,
                        CompanyId = companyId,
                        Version = version
                    };
                    context.RoleColumns.Add(record);
                }
                // && model.isVisible == true
                else if (rc != null)
                {
                    //update record in Role colums
                    rc.isVisible = model.isVisible;
                    rc.isEnabled = model.isEnabled;
                    context.RoleColumns.Attach(rc);
                    context.Entry(rc).State = EntityState.Modified;
                }
            }
        }

        public PageDiv GetPageType(int companyId, string objectName, byte version)
        {
            return context.PageDiv.Where(p => p.CompanyId == companyId && p.ObjectName == objectName && p.Version == version).FirstOrDefault();
        }
        public int GetMenuId(int companyId, string objectName, byte version)
        {
            var MenuList = context.Menus.ToList();
            var PageList = context.PageDiv.ToList();

            var query = PageList.Where(p => p.CompanyId == companyId && p.ObjectName == objectName && p.Version == version).Select(p => p.MenuId).FirstOrDefault();
            if (query == 0)
            {
                var x = PageList.Where(p => p.CompanyId == 0 && p.ObjectName == objectName && p.Version == version).Select(p => p.MenuId).FirstOrDefault();
                var v = MenuList.Select(m => m).FirstOrDefault(m => m.Id == x);
                var n = MenuList.Where(a => a.CompanyId == companyId && a.Name == v.Name).Select(a => a.Id).FirstOrDefault();
                return n;
            }
            return query;
        }
        //Column Property
        public IQueryable<FormColumnViewModel> GetFormColumnInfo(int companyId, string objectName, byte version, string culture)
        {
            // left outer join use group join or link query
            var query = from column in context.FormsColumns
                        where column.Section.FieldSet.Page.CompanyId == companyId && column.Section.FieldSet.Page.ObjectName == objectName && column.Section.FieldSet.Page.Version == version && column.InputType != "none"
                        join org in context.SysColumns on new { obj = objectName, nam = column.ColumnName }
                        equals new { obj = org.obj_name, nam = org.column_name } into g
                        from org in g.DefaultIfEmpty()
                        orderby new { fieldorder = column.Section.FieldSet.Order, secorder = column.Section.Order, col = column.ColumnOrder }
                        select new FormColumnViewModel
                        {
                            Id = column.Id,
                            SectionId = column.SectionId,
                            CompanyId = companyId,
                            order = column.ColumnOrder,
                            type = column.InputType,
                            name = column.ColumnName,
                            label = HrContext.GetColumnTitle(companyId, culture, objectName, version, column.ColumnName),
                            placeholder = column.PlaceHolder,
                            minLength = column.MinLength,
                            maxLength = column.MaxLength,
                            min = column.Min,
                            max = column.Max,
                            isunique = column.IsUnique,
                            UniqueColumns = column.UniqueColumns,
                            isVisible = column.isVisible,
                            required = column.Required,
                            pattern = column.Pattern,
                            HtmlAttribute = column.HtmlAttribute,
                            ColumnType = column.ColumnType,
                            CodeName = column.CodeName,
                            TableName = column.Section.FieldSet.Page.TableName,
                            DefaultValue = column.DefaultValue,
                            Formula = column.Formula,
                            PageId = column.Section.FieldSet.PageId,
                            OrgMaxLength = org == null ? (short)0 : (short)org.max_length,

                        };
            return query;
        }

        public FormColumnViewModel GetFormColumn(int companyId, string objectName, byte version, string culture, string columnname)
        {
            // left outer join use group join or link query
            var result = (from column in context.FormsColumns
                        where column.Section.FieldSet.Page.CompanyId == companyId && column.Section.FieldSet.Page.ObjectName == objectName && column.Section.FieldSet.Page.Version == version && column.ColumnName == columnname
                        select new FormColumnViewModel
                        {
                            Id = column.Id,
                            SectionId = column.SectionId,
                            CompanyId = companyId,
                            order = column.ColumnOrder,
                            type = column.InputType,
                            name = column.ColumnName,
                            label = HrContext.GetColumnTitle(companyId, culture, objectName, version, column.ColumnName),
                            placeholder = column.PlaceHolder,
                            minLength = column.MinLength,
                            maxLength = column.MaxLength,
                            min = column.Min,
                            max = column.Max,
                            isunique = column.IsUnique,
                            UniqueColumns = column.UniqueColumns,
                            isVisible = column.isVisible,
                            required = column.Required,
                            pattern = column.Pattern,
                            HtmlAttribute = column.HtmlAttribute,
                            ColumnType = column.ColumnType,
                            CodeName = column.CodeName,
                            TableName = column.Section.FieldSet.Page.TableName,
                            DefaultValue = column.DefaultValue,
                            Formula = column.Formula,
                            PageId = column.Section.FieldSet.PageId,
                            OrgMaxLength = 0
                        }).FirstOrDefault();

            return result;
        }
        public IList<StringDropDown> GetCodeNamesList(string culture)
        {
            return context.Database.SqlQuery<StringDropDown>("select t.CodeName id, IsNULL(MsgTbl.Meaning, t.CodeName) name from (select distinct s.CodeName from SystemCodes s union select distinct c.CodeName from lookupcode c) t left outer join MsgTbl on (t.CodeName = MsgTbl.Name and MsgTbl.Culture = '" + culture + "')").ToList();
        }

        public IQueryable<FormColumnViewModel> GetExcelColumnInfo(int companyId, string objectName, byte version, string culture)
        {
            // left outer join use group join or link query
            string[] excludedtypes = new string[] { "hidden", "file", "multiselect", "label", "button", "none" };

            var query = from column in context.FormsColumns
                        where column.Section.FieldSet.Page.CompanyId == companyId && column.Section.FieldSet.Page.ObjectName == objectName && column.Section.FieldSet.Page.Version == version && column.isVisible && (!excludedtypes.Contains(column.InputType) || column.InputType == null) && (string.IsNullOrEmpty(column.HtmlAttribute) || !column.HtmlAttribute.Contains("readonly"))
                        orderby new { fieldorder = column.Section.FieldSet.Order, secorder = column.Section.Order, col = column.ColumnOrder }
                        select new FormColumnViewModel
                        {
                            Id = column.Id,
                            SectionId = column.SectionId,
                            CompanyId = companyId,
                            order = column.ColumnOrder,
                            type = column.InputType,
                            name = column.ColumnName,
                            label = HrContext.GetColumnTitle(companyId, culture, objectName, version, column.ColumnName),
                            placeholder = column.PlaceHolder,
                            minLength = column.MinLength,
                            maxLength = column.MaxLength,
                            min = column.Min,
                            max = column.Max,
                            isunique = column.IsUnique,
                            UniqueColumns = column.UniqueColumns,
                            isVisible = column.isVisible,
                            required = column.Required,
                            pattern = column.Pattern,
                            HtmlAttribute = column.HtmlAttribute,
                            ColumnType = column.ColumnType,
                            CodeName = column.CodeName,
                            TableName = column.Section.FieldSet.Page.TableName,
                            DefaultValue = column.DefaultValue,
                            Formula = column.Formula,
                            PageId = column.Section.FieldSet.PageId,
                            OrgMaxLength = column.MaxLength
                        };

            return query;
        }

        public void Attach(FormColumn info)
        {
            context.FormsColumns.Attach(info);
        }

        public System.Data.Entity.Infrastructure.DbEntityEntry<FormColumn> Entry(FormColumn info)
        {
            return Context.Entry(info);
        }

        public IEnumerable<FormLookUpCodeVM> GetFormLookUpCodes(int pageId, string culture)
        {
            var sql = "SELECT C.CodeName, C.CodeId id, (CASE WHEN (T.Title IS NOT NULL) THEN T.Title  ELSE C.Name END) name, 0 SysCode, C.Protected isFreez, CAST(0 AS BIT) isUserCode, (CASE WHEN (GETDATE() BETWEEN C.StartDate And ISNULL(C.EndDate, '2099-01-01')) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) isActive FROM FieldSets F, Sections S, FormColumns FC, lookupcode C LEFT OUTER JOIN LookUpTitles T ON (C.CodeName = T.CodeName And C.CodeId = T.CodeId And T.Culture = '" + culture + "') WHERE F.PageId = " + pageId + " And F.Id = S.FieldSetId And S.Id = FC.SectionId And FC.CodeName = C.CodeName UNION SELECT C.CodeName, C.CodeId id, (CASE WHEN (T.Title IS NOT NULL) THEN T.Title ELSE C.Name END) name, C.SysCodeId SysCode, CAST(0 AS BIT) isFreez, CAST(1 AS BIT) isUserCode, (CASE WHEN (GETDATE() BETWEEN C.StartDate And ISNULL(C.EndDate, '2099-01-01')) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) isActive FROM FieldSets F, Sections S, FormColumns FC, LookUpUserCodes C LEFT OUTER JOIN LookUpTitles T ON (C.CodeName = T.CodeName And C.CodeId = T.CodeId And T.Culture = '" + culture + "') WHERE F.PageId = " + pageId + " And F.Id = S.FieldSetId And S.Id = FC.SectionId And FC.CodeName = C.CodeName";
            return context.Database.SqlQuery<FormLookUpCodeVM>(sql).ToList();
        }

        public int AddToTable(SelectOptionsViewModel model, string tableName, int companyId)
        {
            StringBuilder Sql = new StringBuilder("Insert INTO " + tableName + " ( ");
            int id = context.Database.SqlQuery<int>("Select Id From " + tableName + " Where Name = '" + model._Name + "'").FirstOrDefault();
            if (id > 0) return 0;

            //check code is exist
            if (context.SysColumns.Count(s => s.obj_name == tableName && s.column_name == "Code") > 0)
            {
                var code = context.Database.SqlQuery<int?>("Select Max(Code) From " + tableName).FirstOrDefault();
                code = code == null ? 1 : code + 1;
                if (!model.ColumnsNames.Contains("Code"))
                {
                    model.ColumnsNames.Add("Code");
                    model.ColumnsValue.Add(code.ToString());
                }
            }

            for (int i = 0; i < model.ColumnsNames.Count; i++)
            {
                Sql.Append(model.ColumnsNames[i]);
                if (i != model.ColumnsNames.Count - 1) Sql.Append(", ");

                if (model.ColumnsNames[i] == "IsLocal" && model.ColumnsValue[i] == "true")
                {
                    Sql.Append("CompanyId");
                    if (i != model.ColumnsNames.Count - 1) Sql.Append(", ");
                }
            }

            Sql.Append(") VALUES ( ");

            for (int i = 0; i < model.ColumnsValue.Count; i++)
            {
                DateTime date = new DateTime();
                if (DateTime.TryParse(model.ColumnsValue[i], out date))
                    model.ColumnsValue[i] = date.ToString("yyyy/MM/dd");

                Sql.Append("'" + model.ColumnsValue[i] + "'");
                if (i != model.ColumnsValue.Count - 1) Sql.Append(", ");

                if (model.ColumnsNames[i] == "IsLocal" && model.ColumnsValue[i] == "true")
                {
                    Sql.Append("'" + companyId + "'");
                    if (i != model.ColumnsValue.Count - 1) Sql.Append(", ");
                }
            }
            Sql.Append(")");

            var res = ((Db.HrContext)Context).Database.ExecuteSqlCommand(Sql.ToString(), "");
            if (res != 0)
            {
                Sql = new StringBuilder("Select Id From " + tableName + " Where Name = '" + model._Name + "'");
                res = GetIntResultFromSql(Sql.ToString());
            }
            return res;
        }

        public IEnumerable<DropDownList> GetRemoteList(string tableName, string query, string formTableName, int companyId, string culture, string id)
         {
            StringBuilder sql = new StringBuilder();
            switch (tableName)
            {
                case "People":
                    if (id == "")
                    {
                        sql.Append("select top 7 Id,Name,PicUrl,Gender,Icon from (SELECT P.Id, dbo.fn_TrlsName(ISNULL(P.Title, '') + ' ' + P.FirstName + ' ' + P.Familyname, '" + culture + "') Name, dbo.fn_GetDoc('EmployeePic', P.Id) PicUrl, P.Gender, dbo.fn_GetEmpStatus(P.Id) Icon FROM People P");

                        if (formTableName == "Terminations") //join with Employments
                        {
                            sql.Append(", Employements AS E WHERE E.CompanyId = " + companyId + " AND E.EmpId = P.Id AND E.Status = 1");
                        }
                        else //join with assignments
                        {
                            sql.Append(",Assignments A WHERE A.CompanyId = " + companyId + " And A.EmpId = P.Id AND (Convert(date, GETDATE()) Between A.AssignDate And A.EndDate) AND A.SysAssignStatus = 1");
                            //if (formTableName == "LeaveRequests")
                            //    sql.Append(" AND A.SysAssignStatus = 1");
                        }

                        sql.Append(") t where Name like '" + query + "'");
                    }
                    else
                        sql.Append("SELECT P.Id, dbo.fn_TrlsName(ISNULL(P.Title, '') + ' ' + P.FirstName + ' ' + P.Familyname, '" + culture + "') Name, dbo.fn_GetDoc('EmployeePic', P.Id) PicUrl, P.Gender, dbo.fn_GetEmpStatus(P.Id) Icon FROM People P WHERE Id = " + id);

                    break;
                case "Jobs":
                    if (id == "")
                        sql.Append("select top 7 Id,Name from (SELECT Id, dbo.fn_TrlsName(Name, '" + culture + "') Name FROM Jobs WHERE (CompanyId = " + companyId + " OR IsLocal = 0) AND (Convert(date, GETDATE()) Between StartDate AND ISNULL(EndDate, '2099/01/01'))) t where Name like '" + query + "'");
                    else
                        sql.Append("SELECT Id, dbo.fn_TrlsName(Name, '" + culture + "') Name FROM Jobs WHERE Id = " + id);
                    break;
                case "CompanyStructures":
                    if (id == "")
                        sql.Append("select top 7 Id,Name from (SELECT Id, dbo.fn_TrlsName(Name, '" + culture + "') Name FROM CompanyStructures WHERE (CompanyId = " + companyId + ") AND (Convert(date, GETDATE()) Between StartDate AND ISNULL(EndDate, '2099/01/01'))) t where Name like '" + query + "'");
                    else
                        sql.Append("SELECT Id, dbo.fn_TrlsName(Name, '" + culture + "') Name FROM CompanyStructures WHERE Id = " + id);

                    break;
                case "Positions":
                    sql.Append("select top 7 Id,Name from (SELECT Id, dbo.fn_TrlsName(Name, '" + culture + "') Name FROM Positions WHERE (CompanyId = " + companyId + ") AND (Convert(date, GETDATE()) Between StartDate AND ISNULL(EndDate, '2099/01/01'))) t where Name like '" + query + "'");
                    break;
                case "Countries":
                    string lang = culture.Substring(0, 2) == "ar" ? "Ar" : "";
                    if (id == "")
                        sql.Append("SELECT top 7 Id, Name" + lang + " Name FROM Countries where Name" + lang + " like '" + query + "'");
                    else
                        sql.Append("SELECT top 7 Id, Name" + lang + " Name FROM Countries where Id = " + id);
                    break;
            }

            if (sql.Length == 0)
                return new List<DropDownList>();
            else
            {
                var list = context.Database.SqlQuery<DropDownList>(sql.ToString()).ToList();
                return list;
            }
        }

        public FormFlexColumnsVM GetFormFlexData(int companyId, string objectName, byte version, string culture, int SourceId)
        {
            var page = context.PageDiv.Where(p => p.CompanyId == companyId && p.ObjectName == objectName && p.Version == version)
                .Select(p => new
                {
                    p.Id, p.TableName,
                    legendTitle = HrContext.GetColumnTitle(companyId, culture, objectName, version, "AdditionalColumns"),
                });

            if (page != null)
            {
                var flexData = (from p in page
                                join fc in context.FlexColumns on p.Id equals fc.PageId
                                where fc.isVisible
                                join fd in context.FlexData on new { fc.PageId, fc.ColumnName } equals new { fd.PageId, fd.ColumnName } into g1
                                from fd in g1.Where(b => b.SourceId == SourceId).DefaultIfEmpty()
                                orderby fc.ColumnOrder
                                select new FormFlexColumnsViewModel
                                {
                                    name = fc.ColumnName,
                                    label = HrContext.GetColumnTitle(companyId, culture, objectName, version, fc.ColumnName) ?? fc.ColumnName,
                                    PageId = fc.PageId,
                                    //SourceId = SourceId,
                                    flexId = fd == null ? 0 : fd.Id,
                                    Value = fd == null ? null : fd.Value,
                                    ValueId = fd == null ? null : fd.ValueId,
                                    //ValueText = (fc.InputType == 3 && code != null ? (t == null ? code.Name : t.Title) : fd.Value),
                                    CodeName = fc.CodeName,
                                    InputType = fc.InputType,
                                    isunique = fc.IsUnique,
                                    max = fc.Max,
                                    min = fc.Min,
                                    pattern = fc.Pattern,
                                    placeholder = fc.PlaceHolder,
                                    required = fc.Required,
                                    UniqueColumns = fc.UniqueColumns,
                                    //ObjectName = objectName,
                                    TableName = p.TableName,
                                    //Version = version
                                }).ToList();

                FormFlexColumnsVM result = new FormFlexColumnsVM
                {
                    FlexData = flexData,
                    Legend = page.FirstOrDefault().legendTitle
                };

                return result;
            }

            return new FormFlexColumnsVM();
        }

        #endregion

        #region FlexForm

        public IQueryable<FlexFormGridViewModel> ReadFlexForms(int formType, string culture)
        {
            return context.FlexForms.Where(f => f.FormType == formType).Select(f => new FlexFormGridViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Purpose = f.Purpose,
                DesignedBy = HrContext.TrlsName(f.Designer.Title + " " + f.Designer.FirstName + " " + f.Designer.Familyname, culture),
                //FormType = f.FormType
            });
        }

        public FlexFormViewModel GetFlexForm(int id, string culture)
        {
            var qurey = (from f in context.FlexForms
                         where f.Id == id
                         select new FlexFormViewModel
                         {
                             Id = f.Id,
                             FormName = f.Name,
                             Purpose = f.Purpose,
                             DesignedBy = f.DesignedBy,
                             FormType = f.FormType,
                             FieldSets = context.FlexFormFS.Where(fs => fs.FlexformId == f.Id).OrderBy(fs => fs.FSOrder).Select(fs =>
                             new FlexFormFSViewModel
                             {
                                 Id = fs.Id,
                                 order = fs.FSOrder,
                                 Description = fs.Description,
                                 Columns = context.FlexFormColumns.Where(fc => fc.FlexFSId == fs.Id).OrderBy(fc => fc.ColumnOrder).Select(fc =>
                                         new FlexFormColumnViewModel
                                         {
                                             Id = fc.Id,
                                             Name = fc.Name,
                                             ColumnOrder = fc.ColumnOrder,
                                             InputType = fc.InputType,
                                             Selections = fc.Selections,
                                             ShowTextBox = fc.ShowTextBox,
                                             ShowHint = fc.ShowHint,
                                             Hint = fc.Hint
                                         }).ToList()
                             }).ToList()
                         }).ToList();

            return qurey.FirstOrDefault();
        }


        public void Add(FlexForm FlexForm)
        {
            context.FlexForms.Add(FlexForm);
        }
        public void Attach(FlexForm FlexForm)
        {
            context.FlexForms.Attach(FlexForm);
        }
        public DbEntityEntry<FlexForm> Entry(FlexForm FlexForm)
        {
            return Context.Entry(FlexForm);
        }
        public void Remove(FlexForm FlexForm)
        {
            context.FlexForms.Remove(FlexForm);
        }

        public void Add(FlexFormFS FlexFormFS)
        {
            context.FlexFormFS.Add(FlexFormFS);
        }
        public void Attach(FlexFormFS FlexFormFS)
        {
            context.FlexFormFS.Attach(FlexFormFS);
        }
        public DbEntityEntry<FlexFormFS> Entry(FlexFormFS FlexFormFS)
        {
            return Context.Entry(FlexFormFS);
        }
        public void Remove(FlexFormFS FlexFormFS)
        {
            context.FlexFormFS.Remove(FlexFormFS);
        }

        public void Add(FlexFormColumn FlexFormColumn)
        {
            context.FlexFormColumns.Add(FlexFormColumn);
        }
        public void Attach(FlexFormColumn FlexFormColumn)
        {
            context.FlexFormColumns.Attach(FlexFormColumn);
        }
        public DbEntityEntry<FlexFormColumn> Entry(FlexFormColumn FlexFormColumn)
        {
            return Context.Entry(FlexFormColumn);
        }
        public void Remove(FlexFormColumn FlexFormColumn)
        {
            context.FlexFormColumns.Remove(FlexFormColumn);
        }

        #endregion

        #region Grid
        public IQueryable<ColumnInfoViewModel> GetColumnInfo(int companyId, string objectName, byte version, string culture)
        {
            var query = from column in context.GridColumns
                        where column.Grid.CompanyId == companyId && column.Grid.ObjectName == objectName && column.Grid.Version == version && column.OrgInputType != "none"
                        join org in context.SysColumns on new { obj = objectName, nam = column.ColumnName }
                        equals new { obj = org.obj_name, nam = org.column_name } into g
                        from org in g.DefaultIfEmpty()
                        select new ColumnInfoViewModel
                        {
                            Id = column.Id,
                            GridId = column.GridId,
                            Version = version,
                            DefaultValue = column.DefaultValue,
                            CompanyId = companyId,
                            ObjectName = objectName,
                            ColumnName = column.ColumnName,
                            ColumnOrder = column.ColumnOrder,
                            isVisible = column.isVisible,
                            DefaultWidth = column.DefaultWidth,
                            ColumnType = column.ColumnType,
                            Required = column.Required,
                            Min = column.Min,
                            Max = column.Max,
                            Pattern = column.Pattern,
                            MaxLength = column.MaxLength,
                            MinLength = column.MinLength,
                            PlaceHolder = column.PlaceHolder,
                            Custom = column.Custom,
                            InputType = column.InputType,
                            DataType = org.data_type,
                            OrgInputType = column.OrgInputType,
                            OrgRequired = org != null && org.is_required == 1,
                            OrgColumnType = org == null ? column.ColumnType : org.data_type,
                            OrgMaxLength = org == null ? column.MaxLength : (short)org.max_length,
                            IsUnique = column.IsUnique,
                            UniqueColumns = column.UniqueColumns,
                            Message = "",
                            CodeName = column.CodeName,
                            TableName = column.Grid.TableName,
                            label = HrContext.GetColumnTitle(companyId, culture, objectName, version, column.ColumnName),

                        };
            return query;
        }
        public IQueryable<FormColumnViewModel> GetGridColumnInfo(int companyId, string objectName, byte version, string culture)
        {
            var query = from column in context.GridColumns
                        where column.Grid.CompanyId == companyId && column.Grid.ObjectName == objectName && column.Grid.Version == version && column.OrgInputType != "none"
                        select new FormColumnViewModel
                        {
                            Id = column.Id,
                            PageId = column.GridId,
                            DefaultValue = column.DefaultValue,
                            CompanyId = companyId,
                            name = column.ColumnName,
                            order = column.ColumnOrder,
                            isVisible = column.isVisible,
                            ColumnType = column.ColumnType,
                            required = column.Required,
                            min = column.Min,
                            max = column.Max,
                            pattern = column.Pattern,
                            maxLength = column.MaxLength,
                            minLength = column.MinLength,
                            placeholder = column.PlaceHolder,
                            HtmlAttribute = column.Custom,
                            type = column.InputType == null ?column.ColumnType : column.InputType,
                            OrgInputType = column.OrgInputType,
                            isunique = column.IsUnique,
                            UniqueColumns = column.UniqueColumns,
                            CodeName = column.CodeName,
                            TableName = column.Grid.TableName,
                            label = HrContext.GetColumnTitle(companyId, culture, objectName, version, column.ColumnName)
                        };

            return query;
        }

        public void ApplyAdminChanges(GridViewModel grid)
        {
            // for new company
            PageDiv page = context.PageDiv.FirstOrDefault(pd => pd.CompanyId == grid.CompanyId && pd.ObjectName == grid.ObjectName && pd.Version == grid.Version);
            if (page == null) // copy page for this new company
            {
                // copy most matched grid
                var result = context.GridColumns.Where(c => c.GridId == HrContext.GetPageId(grid.CompanyId.Value, grid.ObjectName, grid.Version))
                .Select(c => new { columns = c, menu = c.Grid.MenuId })
                .ToList();
                var ColumnInfo = result.Select(r => r.columns).ToList();
                grid.NewCompany = true;
                grid.newVersion = grid.Version;
                grid.ColumnInfo = ColumnInfo;
                CopyGridDesign(grid);
            }
            else
            {
                if (grid.ColumnInfo != null)
                    ModifyColumnsInfo(page, grid.ColumnInfo);

                if (grid.roleColumns != null)
                    ModifyRoleColumns(new GridViewModel
                    {
                        roleColumns = grid.roleColumns,
                        CompanyId = grid.CompanyId,
                        ObjectName = grid.ObjectName,
                        Version = grid.Version
                    });

                if (grid.columnTitles != null)
                    RenameColumns(new GridViewModel
                    {
                        CompanyId = grid.CompanyId,
                        columnTitles = grid.columnTitles,
                        ObjectName = grid.ObjectName,
                        Lang = grid.Lang,
                        Version = grid.Version
                    });
            }
        }

        public void CopyColumnsInfo(IEnumerable<ColumnInfoViewModel> models)
        {
            var model = models.OrderBy(m => m.Id).First();
            var ids = models.Select(m => m.Id);
            var result = context.GridColumns.Where(c => c.GridId == model.GridId && !ids.Contains(c.Id))
                .Select(c => new { columns = c, menu = c.Grid.MenuId })
                .ToList();
            var ColumnInfo = result.Select(r => r.columns).ToList();
            var MenuId = result.Select(r => r.menu).FirstOrDefault();

            foreach (var column in models)
            {
                var record = new GridColumn
                {
                    ColumnName = column.ColumnName,
                    ColumnOrder = column.ColumnOrder,
                    isVisible = column.isVisible,
                    DefaultWidth = column.DefaultWidth,
                    ColumnType = column.ColumnType,
                    Required = column.Required,
                    Min = column.Min,
                    Max = column.Max,
                    Pattern = column.Pattern,
                    MaxLength = column.MaxLength,
                    MinLength = column.MinLength,
                    CodeName = column.CodeName,
                    DefaultValue = column.DefaultValue,
                    PlaceHolder = column.PlaceHolder,
                    Custom = column.Custom,
                    InputType = column.InputType,
                    IsUnique = column.IsUnique,
                    UniqueColumns = column.UniqueColumns,
                    OrgInputType = column.OrgInputType,
                };

                ColumnInfo.Add(record);
            }

            CopyGridDesign(new GridViewModel
            {
                TableName = model.TableName ?? model.ObjectName,
                CompanyId = model.CompanyId,
                ObjectName = model.ObjectName,
                NewCompany = true,
                MenuId = MenuId,
                Version = model.Version,
                ColumnInfo = ColumnInfo
            });
        }

        private void CopyGridDesign(GridViewModel grid)
        {
            if (grid.Version != grid.newVersion)
            {
                if (grid.columnTitles == null) grid.columnTitles = context.ColumnTitles.Where(a => a.CompanyId == grid.CompanyId && a.ObjectName == grid.ObjectName && a.Version == grid.Version).ToList();
            }

            var page = new PageDiv
            {
                TableName = grid.TableName,
                CompanyId = grid.Company == null ? grid.CompanyId.Value : 0,
                Company = grid.Company,
                DivType = "Grid",
                MenuId = grid.Menu == null ? grid.MenuId.Value : 0,
                Menu = grid.Menu,
                ObjectName = grid.ObjectName,
                Version = grid.newVersion
            };

            context.Set<PageDiv>().Add(page);
            int companyId = (grid.NewCompany ? 0 : grid.CompanyId.Value);

            var columnTitles = context.ColumnTitles.Where(t => t.CompanyId == companyId &&
            t.ObjectName == grid.ObjectName && t.Version == grid.Version).ToList();

            // Version all columns
            foreach (var column in grid.ColumnInfo)
            {
                context.Set<GridColumn>().Add(NewGridColumn(column, page));
            }

            //Copy column titles 
            var titles = grid.columnTitles;
            if (columnTitles.Count > 0)
                titles = grid.columnTitles != null ? (from t1 in columnTitles
                                                      join t2 in grid.columnTitles
                                                      on new { t1.ColumnName, t1.Culture } equals new { t2.ColumnName, t2.Culture } into g
                                                      from t2 in g.DefaultIfEmpty()
                                                      select new ColumnTitle
                                                      {
                                                          ColumnName = t1.ColumnName,
                                                          Culture = t1.Culture,
                                                          ObjectName = t1.ObjectName,
                                                          Version = grid.newVersion,
                                                          CompanyId = grid.Company == null ? grid.CompanyId.Value : 0,
                                                          Company = grid.Company,
                                                          Title = (t2 == null ? t1.Title : t2.Title)
                                                      }).ToList() : columnTitles;

            //Copy column titles
            CopyColumnTitles(titles, page.CompanyId, grid.Company, page.Version);
        }

        private GridColumn NewGridColumn(GridColumn column, PageDiv page)
        {
            var newColumn = new GridColumn
            {
                ColumnName = column.ColumnName,
                ColumnOrder = column.ColumnOrder,
                ColumnType = column.ColumnType,
                InputType = column.InputType,
                IsUnique = column.IsUnique,
                isVisible = column.isVisible,
                Max = column.Max,
                MaxLength = column.MaxLength,
                Min = column.Min,
                CodeName = column.CodeName,
                DefaultValue = column.DefaultValue,
                MinLength = column.MinLength,
                OrgInputType = column.OrgInputType,
                Pattern = column.Pattern,
                PlaceHolder = column.PlaceHolder,
                Required = column.Required,
                UniqueColumns = column.UniqueColumns,
                Custom = column.Custom,
                DefaultWidth = column.DefaultWidth,
                Grid = page
            };

            return newColumn;
        }

        public void NewCopyGridDesign(PageDiv grid, Menu menu, Company company)
        {
            var columns = context.GridColumns.Where(c => c.GridId == grid.Id).ToList();
            var version = grid.Version;
            if (company == null) // if create new company
                version = (byte)(context.PageDiv.Where(a => a.CompanyId == grid.CompanyId && a.ObjectName == grid.ObjectName).Max(a => a.Version) + 1);

            menu.Version = version;

            CopyGridDesign(new GridViewModel
            {
                TableName = grid.TableName,
                CompanyId = grid.CompanyId,
                Company = company,
                ObjectName = grid.ObjectName,
                MenuId = grid.MenuId,
                Menu = menu,
                Version = grid.Version,
                newVersion = version,
                ColumnInfo = columns
            });
        }

        public IEnumerable<ColumnTitle> GetColumnTitles(int companyId, IEnumerable<int> menuIds)
        {
            return from p in context.PageDiv
                   where p.CompanyId == companyId && menuIds.Contains(p.MenuId)
                   join t in context.ColumnTitles on new { p.CompanyId, p.ObjectName, p.Version } equals new { t.CompanyId, t.ObjectName, t.Version }
                   select t;
        }

        public GridDesignViewModel GetGrid(int companyId, string objectName, byte version, string culture, string role)
        {
            var grid = new GridDesignViewModel
            {
                ColumnTitles = GetColumnTitles(companyId, objectName, version, culture),
                ColumnInfo = GetColumns(companyId, objectName, version)
            };

            GetColumnPermission(grid, companyId, objectName, version, role);

            return grid;
        }

        void ModifyColumnsInfo(PageDiv page, IEnumerable<GridColumn> information)
        {
            var currentRows = context.GridColumns.Where(r => r.GridId == page.Id).ToList();

            foreach (GridColumn info in information)
            {
                var modify = currentRows.FirstOrDefault(t => t.ColumnName == info.ColumnName);
                if (modify != null) // modified row
                {
                    modify.ColumnOrder = info.ColumnOrder;
                    modify.isVisible = info.isVisible;
                    modify.DefaultWidth = info.DefaultWidth;
                    context.GridColumns.Attach(modify);
                    context.Entry(modify).State = EntityState.Modified;
                }
                else // new row
                {
                    info.Grid = page;
                    context.GridColumns.Add(info);
                }
            }
        }

        void ModifyRoleColumns(GridViewModel grid)
        {
            var currentRows = context.RoleColumns.Where(r => r.CompanyId == grid.CompanyId && r.ObjectName == grid.ObjectName && r.Version == grid.Version).ToList();

            foreach (RoleColumns role in grid.roleColumns)
            {
                var modify = currentRows.FirstOrDefault(r => r.RoleId == role.RoleId && r.ColumnName == role.ColumnName);
                if (modify != null) // modified row
                {
                    modify.isVisible = false;
                    context.RoleColumns.Attach(modify);
                    context.Entry(modify).State = EntityState.Modified;
                }
                else // new row
                {
                    role.CompanyId = grid.CompanyId.Value;
                    role.ObjectName = grid.ObjectName;
                    role.Version = grid.Version;
                    context.RoleColumns.Add(role);
                }
            }

            // Unhide Rows
            foreach (RoleColumns role in currentRows)
            {
                var unhide = grid.roleColumns.FirstOrDefault(r => r.RoleId == role.RoleId && r.ColumnName == role.ColumnName);
                if (unhide == null) // modified row
                {
                    role.isVisible = true;
                    context.RoleColumns.Attach(role);
                    context.Entry(role).State = EntityState.Modified;
                }
            }
        }

        void RenameColumns(GridViewModel grid)
        {
            var currentRows = context.ColumnTitles.Where(r => r.CompanyId == grid.CompanyId && r.Culture == grid.Lang && r.ObjectName == grid.ObjectName && r.Version == grid.Version).ToList();

            foreach (ColumnTitle title in grid.columnTitles)
            {
                var modify = currentRows.FirstOrDefault(t => t.ColumnName == title.ColumnName);
                if (modify != null) // modified row
                {
                    if (modify.Title != title.Title) // title has been changed
                    {
                        modify.Title = title.Title;
                        Attach(modify);
                        context.Entry(modify).State = EntityState.Modified;
                    }
                }
                else // new row
                {
                    title.CompanyId = grid.CompanyId.Value;
                    title.Culture = grid.Lang;
                    title.ObjectName = grid.ObjectName;
                    title.Version = grid.Version;
                    Add(title);
                }
            }
        }

      
        string GetColumnTitles(int companyId, string objectName, byte version, string culture)
        {

            var list = context.ColumnTitles
                .Where(t => t.CompanyId == companyId && t.Culture == culture && t.ObjectName == objectName && t.Version == version)
                .Select(t => new { ColumnName = t.ColumnName, Title = t.Title })
                .ToList();

            //Get developement company if empty
            if (list.Count == 0)
            {
                list = context.ColumnTitles
                .Where(t => t.CompanyId == 0 && t.Culture == culture && t.ObjectName == objectName && t.Version == version && t.Title != null)
                .Select(t => new { ColumnName = t.ColumnName, Title = t.Title })
                .ToList();
            }

            if (list.FirstOrDefault(a => a.ColumnName == "edit") == null)
                list.Add(new { ColumnName = "edit", Title = MsgUtils.Instance.Trls(culture, "edit") });
            if (list.FirstOrDefault(a => a.ColumnName == "show") == null)
                list.Add(new { ColumnName = "show", Title = MsgUtils.Instance.Trls(culture, "show") });
            if (list.FirstOrDefault(a => a.ColumnName == "Delete") == null)
                list.Add(new { ColumnName = "Delete", Title = MsgUtils.Instance.Trls(culture, "Delete") });
            if (list.FirstOrDefault(a => a.ColumnName == "CopyRole") == null)
                list.Add(new { ColumnName = "CopyRole", Title = MsgUtils.Instance.Trls(culture, "CopyRole") });
            StringBuilder result = new StringBuilder("[");
            foreach (var item in list.Where(a => a.Title != null))
            {
                if (result.Length > 1) result.Append(",");
                result.Append("{\"id\": \"" + item.ColumnName + "\", \"title\": \"" + item.Title.Trim().Replace('\n', ' ') + "\"}");
            }

            if (result.Length == 1)
                return "empty";
            {
                result.Append("]");
                return result.ToString();
            }
        }

        string GetHiddenColumnRoles(int companyId, string objectName, byte version)
        {
            // [{"column":"BranchNo","roles":[null,"2",null]},{"column":"Name","roles":["1","2"]},{"column":"Address","roles":[null,null,"3"]},{"column":"Email","roles":[null,null,"3"]},{"column":"Telephone","roles":[null,null,"3"]}]
            var list = context.RoleColumns
                .Where(t => t.CompanyId == companyId && t.ObjectName == objectName && t.Version == version && t.isVisible == false)
                .OrderBy(t => t.ColumnName)
                .Select(t => new { column = t.ColumnName, id = t.RoleId })
                .ToList();

            string column = "";
            StringBuilder result = new StringBuilder("[");

            foreach (var item in list)
            {
                if (item.column != column)
                {
                    if (result.Length > 1) result.Append("]},");
                    result.Append("{\"column\":\"" + item.column + "\",\"roles\":[");
                }
                else
                    result.Append(",");

                column = item.column;
                result.Append("\"" + item.id + "\"");

            }

            if (result.Length == 1)
                return "empty";
            {
                result.Append("]}]");
                return result.ToString();
            }
        }

        void GetColumnPermission(GridDesignViewModel grid, int companyId, string objectName, byte version, string role)
        {
            // [{"column":"BranchNo","roles":[null,"2",null]},{"column":"Name","roles":["1","2"]},{"column":"Address","roles":[null,null,"3"]},{"column":"Email","roles":[null,null,"3"]},{"column":"Telephone","roles":[null,null,"3"]}]
            var list = context.RoleColumns
                .Where(t => t.CompanyId == companyId && t.ObjectName == objectName && t.Version == version && t.RoleId == role)
                .OrderBy(t => t.ColumnName)
                .Select(t => new { t.isEnabled, t.isVisible, t.ColumnName })
                .ToList();

            var hidden = list.Where(a => a.isVisible == false).Select(a => a.ColumnName);
            var disabled = list.Where(a => a.isEnabled == false).Select(a => a.ColumnName);
            grid.HiddenColumns = ToArrayString(hidden);
            grid.DisabledColumns = ToArrayString(disabled);
        }

        string ToArrayString(IEnumerable<string> list)
        {
            StringBuilder result = new StringBuilder(); // "["
            bool first = true;
            foreach (var item in list)
            {
                if (first) result.Append(item); else result.Append("," + item);
                first = false;
            }

            //result.Append("]");
            return result.ToString();
        }

        string ToJsonString(IEnumerable<RoleColumns> list)
        {
            StringBuilder result = new StringBuilder("[");
            string column = "";
            foreach (var item in list)
            {
                if (item.ColumnName != column)
                {
                    if (result.Length > 1) result.Append("]},");
                    result.Append("{\"column\":\"" + item.ColumnName + "\",\"roles\":[");
                }
                else
                    result.Append(",");

                column = item.ColumnName;
                result.Append("\"" + item.RoleId + "\"");

            }

            if (result.Length == 1)
                return "empty";
            {
                result.Append("]}]");
                return result.ToString();
            }
        }

        string GetColumns(int companyId, string objectName, byte version)
        {
            // ["", "", ...]      
            // Get Grid from user company
            var list = context.GridColumns
                .Where(t => t.Grid.CompanyId == companyId && t.Grid.ObjectName == objectName && t.Grid.Version == version)
                .OrderBy(t => t.ColumnOrder)
                .ToList();

            if (list == null)
            {
                list = context.GridColumns
                .Where(t => t.Grid.CompanyId == companyId && t.Grid.ObjectName == objectName && t.Grid.Version == 0)
                .OrderBy(t => t.ColumnOrder)
                .ToList();
            }


            StringBuilder result = new StringBuilder("[");
            foreach (var item in list)
            {
                // Serialize object
                if (result.Length > 1) result.Append(",");
                result.Append("{\"id\": " + item.Id + ",\"order\": " + item.ColumnOrder + ", \"name\": \"" + item.ColumnName +
                    "\", \"isVisible\": " + (item.isVisible ? "true" : "false") +
                    ", \"width\": " + item.DefaultWidth + ", \"type\": \"" + item.ColumnType +
                    "\", \"required\": " + (item.Required ? "true" : "false") + ", \"min\": " +
                    (item.Min == null ? "null" : item.Min.Value.ToString()) +
                    ", \"max\": " + (item.Max == null ? "null" : item.Max.Value.ToString()) + ", \"maxlength\": " +
                    (item.MaxLength == null ? "null" : item.MaxLength.Value.ToString()) + ", \"minlength\": " +
                    (item.MinLength == null ? "null" : item.MinLength.Value.ToString()) + ", \"placeholder\": \"" +
                    item.PlaceHolder + "\", \"pattern\": \"" + item.Pattern + "\", \"custom\": \"" + item.Custom + "\", \"codeName\": \"" + item.CodeName +
                    "\", \"input\": \"" + item.InputType + "\", \"isUnique\": \"" + item.IsUnique + "\", \"uniqueColumns\": \"" + item.UniqueColumns +
                    "\", \"defaultValue\": \"" + item.DefaultValue + "\"}");
            }

            if (result.Length == 1)
                return "empty";
            {
                result.Append("]");
                return result.ToString();
            }
        }

        public void Attach(GridColumn info)
        {
            context.GridColumns.Attach(info);
        }

        public System.Data.Entity.Infrastructure.DbEntityEntry<GridColumn> Entry(GridColumn info)
        {
            return Context.Entry(info);
        }

        public FormViewModel GetFormInfo(int companyId, string objectName, byte version, string culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Tree
        public IList<TreeViewModel> GetTree(TreeViewParm parm)
        {
            string sql = "SELECT " + parm.Table + ".Id, " + parm.Table + ".CompanyId, " + parm.Table + ".ParentId, " + (parm.Table == "Menus" ? parm.Table + ".Version, " : "") + parm.Table + ".Sort, " + parm.Table + ".NodeType, " + parm.Table + ".Icon, " + parm.Table + ".[Order], " + parm.Table + ".Name Name, ISNULL(NamesTbl.Title, " + parm.Table + ".Name) Title, CAST(LEN(" + parm.Table + ".Sort)/5 AS tinyint) [Level], CAST(ISNULL((SELECT TOP 1 1 FROM " + parm.Table + " T WHERE T.ParentId = " + parm.Table + ".Id), 0) AS bit) hasChildren FROM " + parm.Table + " LEFT OUTER JOIN NamesTbl ON (NamesTbl.Name = " + (parm.Table == "Menus" ? "Menus.Name + CAST(Menus.Sequence AS VARCHAR(5))" : parm.Table + ".Name") + " And NamesTbl.Culture = '" + parm.Culture + "') WHERE " + parm.Table + ".CompanyId = " + parm.CompanyId + " And " + (parm.Id == null ? "" + parm.Table + ".ParentId is null" : "" + parm.Table + ".ParentId = " + parm.Id) + (parm.Table != "Menus" ? " And ('" + DateTime.Today.ToString("yyyy-MM-dd") + "' Between " + parm.Table + ".StartDate And IsNull(" + parm.Table + ".EndDate, '" + DateTime.Today.ToString("yyyy-MM-dd") + "'))" : "") +  " ORDER BY " + parm.Table + ".Sort";
            var result = context.Database.SqlQuery<TreeViewModel>(sql).ToList();

            if (parm.Id == null) // first time only
            {
                if (result.Count == 0) // New tree only
                {
                    result.Add(new TreeViewModel
                    {
                        CompanyId = parm.CompanyId,
                        hasChildren = false,
                        Icon = "glyphicon glyphicon-star-empty",
                        Id = 0,
                        Level = 1,
                        NodeType = 0,
                        Name = "NewNode",
                        Title = MsgUtils.Instance.Trls(parm.Culture, "NewNode"),
                        Order = 100,
                        ParentId = null,
                        Sort = "00100"
                    });
                }

                result[0].Msg = new string[] { MsgUtils.Instance.Trls(parm.Culture, "NewNode"), MsgUtils.Instance.Trls(parm.Culture, "Add"), MsgUtils.Instance.Trls(parm.Culture, "AddChild"), MsgUtils.Instance.Trls(parm.Culture, "edit"), MsgUtils.Instance.Trls(parm.Culture, "Remove"), MsgUtils.Instance.Trls(parm.Culture, "Cut"), MsgUtils.Instance.Trls(parm.Culture, "Copy"), MsgUtils.Instance.Trls(parm.Culture, "PasteOptions"), MsgUtils.Instance.Trls(parm.Culture, "Paste"), MsgUtils.Instance.Trls(parm.Culture, "PasteAsChild"), MsgUtils.Instance.Trls(parm.Culture, "WithPages"), MsgUtils.Instance.Trls(parm.Culture, "WithPagesChild") };
            }
            return result;
        }

        public string DropMenuItem(TreeViewModel source, TreeViewModel dest, string table, string culture)
        {
            string sort = dest.Sort, msg = "OK";

            if (dest.Order == 100 && sort.Length >= 10) // may happen if user try to drap icon over
            {
                int? max = context.Database.SqlQuery<int?>("SELECT MAX([ORDER]) FROM " + table + " WHERE ParentId = " + dest.ParentId).FirstOrDefault();
                dest.Order = (max == null ? 0 : max.Value) + 100;
                sort = sort.Substring(0, sort.Length - 5) + dest.Order.ToString().PadLeft(5, '0');
            }



            //Update
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    var sql = "UPDATE " + table + " SET Sort = '" + sort + "', [Order] = " + dest.Order + ", ParentId " + (dest.ParentId == null ? " = NULL" : " = " + dest.ParentId) + " WHERE Id = " + source.Id;
                    context.Database.ExecuteSqlCommand(sql);
                    if (source.hasChildren)
                    {
                        sql = "Update " + table + " SET Sort = '" + sort + "' + RIGHT(Sort, LEN(Sort) - " + source.Sort.Length + ") WHERE CompanyId = " + source.CompanyId + " And Sort LIKE '" + source.Sort + "%'";
                        context.Database.ExecuteSqlCommand(sql);
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    msg += ", " + MsgUtils.Instance.Trls(culture, "IllegalMove");
                    dbContextTransaction.Rollback();
                }
                finally
                {
                    dbContextTransaction.Dispose();
                }
            }

            return msg;
        }

        public string DropMenuItemCopy(TreeViewModel model, string table, string culture, bool copyPages)
        {
            string sort = model.Sort, msg = "OK";

            if (model.Order == 100 && sort.Length >= 10) // may happen if user try to drap icon over
            {
                int? max = context.Database.SqlQuery<int?>("SELECT MAX([ORDER]) FROM " + table + " WHERE ParentId = " + model.ParentId).FirstOrDefault();
                model.Order = (max == null ? 0 : max.Value) + 100;
                sort = sort.Substring(0, sort.Length - 5) + model.Order.ToString().PadLeft(5, '0');
            }

            if (table == "Menus")
            {
                model.Sort = sort;
                msg = AddMenu(model, culture, copyPages);
            }
            else //Insert in any other table
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sql = "INSERT INTO " + table + " (CompanyId, [Name], ParentId, Icon, NodeType, [Order], Sort) VALUES (" + model.CompanyId + ",'" + model.Name + "'," + model.ParentId + ",'" + model.Icon + "'," + model.NodeType + "," + model.Order + ",'" + sort + "')";
                        context.Database.ExecuteSqlCommand(sql);
                        //if (model.hasChildren)
                        //{
                        //    sql = "Update " + table + " SET Sort = '" + sort + "' + RIGHT(Sort, 5) WHERE CompanyId = " + model.CompanyId + " And Sort LIKE '" + model.Sort + "%'"; // source.Sort
                        //    context.Database.ExecuteSqlCommand(sql);
                        //}
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        msg += ", " + MsgUtils.Instance.Trls(culture, "IllegalCopy");
                        dbContextTransaction.Rollback();
                    }
                    finally
                    {
                        dbContextTransaction.Dispose();
                    }
                }
            }

            return msg;
        }

        private string AddMenu(TreeViewModel model, string culture, bool copyPages)
        {
            var msg = "OK";
            var old = context.Menus.Find(model.Id);
            short seq = (short)(context.Menus.Where(a => a.CompanyId == old.CompanyId && a.Name == old.Name).Max(a => a.Sequence) + 1);

            var menu = new Menu
            {
                CompanyId = old.CompanyId,
                ColumnList = old.ColumnList,
                Icon = old.Icon,
                IsVisible = old.IsVisible,
                CreatedTime = DateTime.Now,
                CreatedUser = old.CreatedUser,
                Name = model.Name,
                NodeType = old.NodeType,
                Order = model.Order,
                ParentId = model.ParentId,
                Sort = model.Sort,
                SSMenu = old.SSMenu,
                Url = old.Url,
                Version = old.Version,
                Sequence = seq,
                WhereClause = old.WhereClause
            };

            var functions = context.MenuFunctions.Where(f => f.MenuId == old.Id).Select(f => f.FunctionId).ToList();
            foreach (var func in functions)
            {
                context.MenuFunctions.Add(new MenuFunction
                {
                    FunctionId = func,
                    Menu = menu,
                });
            }

            if (copyPages)
            {
                var pages = context.PageDiv.Where(p => p.CompanyId == old.CompanyId && p.MenuId == old.Id && p.Version == old.Version).ToList();
                foreach (var page in pages)
                {
                    if (page.DivType == "Form")
                        NewCopyFormDesign(page, menu, null);
                    else if (page.DivType == "Grid")
                        NewCopyGridDesign(page, menu, null);
                }
            }

            context.Menus.Add(menu);
            try
            {
                context.SaveChanges();
                msg += "," + menu.Id;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                msg += ", " + ex.InnerException.Message + ", " + MsgUtils.Instance.Trls(culture, "IllegalCopy");
            }

            return msg;
        }
        #endregion

        #region mobile
        public IQueryable<ColumNameVM> GetColumns(string objectName, int companyId, int version)
        {
            string sql = "select distinct c.ColumnName [Name] from FormColumns c where c.isVisible='true' and c.SectionId in (select s.id from Sections s where s.FieldSetId in(select f.Id from FieldSets f where f.PageId in(select   d.Id from PageDivs d  where ObjectName = '" + objectName + "' and CompanyId=" + companyId + " and Version=" + version + " )))";
            return context.Database.SqlQuery<ColumNameVM>(sql).AsQueryable();

        }
        public IQueryable<MenuViewModel> GetMenu(string culture, int companyId)
        {
            var Ids = context.Menus.Where(a => a.CompanyId == companyId && (a.Name == "Manager" || a.Name == "Employee")).Select(a => a.Id).ToList();

            var q = (from m in context.Menus
                     where (Ids.Contains(m.ParentId.Value) || Ids.Contains(m.Id)) && m.CompanyId == companyId && m.IsVisible == true
                     join p in context.PageDiv on m.Id equals p.MenuId into g
                     from p in g.Where(a => a.DivType == "Form").DefaultIfEmpty()
                     select new MenuViewModel
                     {
                         Id = m.Id,
                         MenuName = m.Name,
                         Title = HrContext.TrlsName(m.Name + m.Version, culture),
                         Version = m.Version,
                         Sort = m.Sort,
                         ParentName = m.Parent.Name,
                         Icon = m.Icon,
                         ObjectName = p.ObjectName == null ? context.Menus.Where(c => c.Id != m.Id && c.Name == m.Name && c.CompanyId == m.CompanyId && c.Version == m.Version).Select(a => context.PageDiv.Where(pp => pp.MenuId == a.Id && pp.DivType == "Form").Select(pp => pp.ObjectName).FirstOrDefault()).FirstOrDefault() : p.ObjectName //p.ObjectName
                     }).OrderBy(c => c.Sort);
            return q;
        }


        #endregion
    }
}
