using System.Collections.Generic;
using System.Linq;
using Interface.Core.Repositories;
using Model.Domain;
using System.Data.Entity;
using Model.ViewModel;
using System.Collections;
using System.Data.Entity.Infrastructure;
using System;
using Model.ViewModel.Administration;

namespace Db.Persistence.Repositories
{
    class PageEditorRepository : Repository<PageDiv>, IPageEditorRepository
    {
        public PageEditorRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public IEnumerable GetobjectName(int companyId, string culture)
        {
            var query = context.PageDiv.Where(w => w.CompanyId == companyId  && w.HasCustCols==true).Select(s => new LabelDDListViewModel { value = s.Id, title = s.ObjectName ,text=HrContext.TrlsName(s.Menu.Name + s.Version,culture) }).ToList();
           
            return query;
        }
        public GridColumnViewModel GetGridColumn(int Id)
        {
            return context.GridColumns.Where(a => a.Id == Id).Select(b => new GridColumnViewModel
            {
                Column = b,
                CompanyId = b.Grid.CompanyId,
                ObjectName = b.Grid.ObjectName,
                Version = b.Grid.Version,
                GridId = b.GridId
            }).FirstOrDefault();
        }
        public PageDivViewModel GetPageObject(int value,string Search)
        {
            switch (Search)
            {
                case "FormColumns":
                    return (from fc in context.FormsColumns
                            where fc.Id == value
                            join s in context.Sections on fc.SectionId equals s.Id
                            join f in context.FieldSets on s.FieldSetId equals f.Id
                            join p in context.PageDiv on f.PageId equals p.Id
                            select new PageDivViewModel
                            {
                                ObjectName = p.ObjectName,
                                Version = p.Version,
                                CompanyId = p.CompanyId,
                                Id = p.Id
                            }).FirstOrDefault();
                case "Section":
                    return (from c in context.Sections
                            where c.Id == value
                            join f in context.FieldSets on c.FieldSetId equals f.Id
                            join p in context.PageDiv on f.PageId equals p.Id
                            select new PageDivViewModel
                            {
                                ObjectName = p.ObjectName,
                                Version = p.Version,
                                CompanyId = p.CompanyId,
                                Id = p.Id
                            }).FirstOrDefault();
                default:
                    return (from p in context.PageDiv
                            where p.Id == value
                            select new PageDivViewModel
                            {
                                ObjectName = p.ObjectName,
                                Version = p.Version,
                                CompanyId = p.CompanyId,
                                Id = p.Id
                            }).FirstOrDefault();
            }
        }
        public IEnumerable GetTablesHasCust(int companyId, string culture)
        {
            return context.PageDiv.Where(w => w.CompanyId == companyId && w.HasCustCols == true).Select(s => new LabelViewModel { value = s.TableName, text = HrContext.TrlsMsg(s.Title, culture) }).ToList();
        }
        public IEnumerable<GridColumn> GetGridColumns(int GdId)
        {
            return context.GridColumns.Where(s => s.GridId == GdId).ToList();
        }
        public IEnumerable<FlexColumnsViewModel> GetFlexColumns(int pageId,string objectname,string culture, int companyId)
        {
            var flexColumns =( from flex in context.FlexColumns
                              where flex.PageId == pageId
                              join p in context.PageDiv on flex.PageId equals p.Id
                              join t in context.ColumnTitles on new { j1 = objectname , j2 = flex.ColumnName}
                              equals new { j1 = t.ObjectName, j2 = t.ColumnName} into g
                              from t in g.Where(a => a.Culture == culture && a.CompanyId ==companyId).DefaultIfEmpty()
                              select new FlexColumnsViewModel
                              {
                                  Id = flex.Id,
                                  CodeName =flex.CodeName,
                                  ColumnName = flex.ColumnName,
                                  ColumnOrder = flex.ColumnOrder,
                                  InputType = flex.InputType,
                                  IsUnique = flex.IsUnique,
                                  Max = flex.Max,
                                  isVisible = flex.isVisible,
                                  Min = flex.Min,
                                  PageId = flex.PageId,
                                  Pattern = flex.Pattern,
                                  PlaceHolder = flex.PlaceHolder,
                                  Required = flex.Required,
                                  UniqueColumns = flex.UniqueColumns,
                                  Title = t.Title,
                                  TableName = p.TableName ,
                                                                                              
                              }).ToList();

            //foreach (var item in flexColumns)
            //{
            //    item.CodeName = MsgUtils.Instance.Trls(culture, item.CodeName);
            //}

            return flexColumns;
        }
        //GetInputType
        public IEnumerable GetInputType(string culture, string code)
        {
            var list = Context.Set<LookUpTitles>()
                .Where(c => c.Culture == culture && c.CodeName == code)
                .Select(c => new { value = c.CodeId, text = c.Title })
                .ToList();

            if (list.Count() == 0)
            {
                list = Context.Set<LookUpCode>()
                .Where(p => (p.CodeName == code) && (p.StartDate <= DateTime.Today && (p.EndDate == null || p.EndDate >= DateTime.Today)))
                .Select(c => new { value = c.CodeId, text = c.Name })
                .ToList();
            }

            return list;
        }

        public string DeleteFlexColumns(FlexColumn model, string culture)
        {
            string msg = "OK";
            if (model == null) return msg;
            // Check before delete
            var columnNames =model.ColumnName;
            var page = model.PageId;
            var related = (from fc in context.FlexColumns
                           where fc.PageId == page && columnNames.Contains(fc.ColumnName)
                          join fd in context.FlexData on new { fc.PageId, fc.ColumnName } equals new { fd.PageId, fd.ColumnName }
                          join pd in context.PageDiv on fc.PageId equals pd.Id
                          join m in context.Menus on pd.MenuId equals m.Id
                          select new { name = fc.ColumnName, page = HrContext.TrlsName(m.Name + m.Sequence, culture) }).FirstOrDefault();

            if (related != null)
            {
                msg = MsgUtils.Instance.Trls(culture, "DeleteRelatedRecord").Replace("{0}", related.page) + ": " + related.name;
                return msg;
            }

            var Id = model.Id;
            var titles = context.ColumnTitles.ToList();
                var DeleteObj = model;
                context.FlexColumns.Attach(DeleteObj);
                context.Entry(DeleteObj).State = EntityState.Deleted;
                var DeleteTitles = titles.Where(a => a.ColumnName == model.ColumnName).ToList();
                RemoveRange(DeleteTitles);
            return msg;
        }
        public void RemoveRange(IEnumerable<ColumnTitle> entities)
        {
            Context.Set<ColumnTitle>().RemoveRange(entities);
        }
        public IQueryable<PageDivViewModel> ReadPage(int companyId, string culture)
        {
            var pages = from p in context.PageDiv
                        where p.CompanyId == companyId && p.DivType == "Form"
                        select new PageDivViewModel
                        {
                            Id = p.Id,
                            Version = p.Version,
                            MenuId = p.MenuId,
                            DivType = p.DivType,
                            TableName = p.TableName,
                            Title = p.Title,
                            ObjectName = p.ObjectName,
                            CompanyId = p.CompanyId
                        };

            return pages;
        }
        public IQueryable<PageDivViewModel> ReadGrid(int companyId, string culture)
        {
            var pages = from g in context.PageDiv
                        where g.CompanyId == companyId && g.DivType == "Grid"
                        select new PageDivViewModel
                        {
                            Id = g.Id,
                            TableName = g.TableName,
                            MenuId = g.MenuId,
                            Version = g.Version,
                            DivType = g.DivType,
                            Title = g.Title,
                            ObjectName = g.ObjectName,
                            CompanyId = g.CompanyId
                        };

            return pages;
        }
        public void AddPage(PageDiv PageDiv)
        {
            context.PageDiv.Add(PageDiv);
        }
        public override void Add(PageDiv entity)
        {
            base.Add(entity);
            AddGridColumns(entity);
        }
        private void AddGridColumns(PageDiv Grid)
        {
            var columns = context.SysColumns
                .Where(sys => sys.obj_name == Grid.TableName)
                .ToList();

            foreach (SysColumns column in columns)
            {
                var gridColumn = new GridColumn {
                    
                    IsUnique = false,
                    Grid = Grid,
                    DefaultWidth = 50,
                    ColumnName = column.column_name,
                    ColumnOrder = (byte)column.column_order,
                    isVisible = column.is_visible == 1,
                    ColumnType = column.data_type,
                    Required = column.is_required == 1,
                    InputType = column.input_type,
                    MaxLength= (column.max_length != null ? (short)column.max_length.Value : (short?)null ),
                    OrgInputType = column.input_type
                };

                context.Set<GridColumn>().Add(gridColumn);
            }
        }
        public IQueryable<ColumnInfoViewModel> GetColumnInfo(int gridId)
        {
            var query = from column in context.GridColumns
                        where column.GridId == gridId && column.InputType != "none"
                        join org in context.SysColumns on new { obj = column.Grid.ObjectName, nam = column.ColumnName }
                        equals new { obj = org.obj_name, nam = org.column_name } into g
                        from org in g.DefaultIfEmpty()
                        select new ColumnInfoViewModel
                        {
                            Id = column.Id,
                            GridId = column.GridId,
                            Version = column.Grid.Version,
                            CompanyId = column.Grid.CompanyId,
                            ObjectName = column.Grid.ObjectName,
                            MenuName = column.Grid.MenuId,
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
                            OrgInputType = column.OrgInputType,
                            OrgRequired = org != null && org.is_required == 1,
                            OrgColumnType = org == null ? "" : org.data_type,
                            OrgMaxLength = org == null ? (short)0 : (short)org.max_length,
                            IsUnique = column.IsUnique,
                            DefaultValue=column.DefaultValue,
                            UniqueColumns = column.UniqueColumns,
                            Message = "",
                           
                        };
            return query;
        }
        public List<ColumnTitle> GetColumnTitles(int companyId, string ObjectName, byte version, string Culture)
        {
            List<ColumnTitle> Titles = new List<ColumnTitle>();
            Titles = context.ColumnTitles.Where(a => a.CompanyId == companyId && a.Culture == Culture && a.ObjectName == ObjectName && a.Version == version).ToList();
            if (Titles.Count == 0)
                Titles = context.ColumnTitles.Where(a => a.CompanyId == 0 && a.Culture == Culture && a.ObjectName == ObjectName && a.Version == version).ToList();
            return Titles;
        }

        public void AddFieldSetColumns(PageDiv Grid)
        {
            var columns = context.SysColumns
               .Where(sys => sys.obj_name == Grid.TableName)
               .Take(1);
            foreach (var item in columns)
            {
                var filedset = new FieldSet
                {
                    Editable = true,
                    Freeze = false,
                    HasTag = true,
                    Page = Grid,
                    Order = (byte)item.column_order,
                    Collapsable = true,
                    Reorderable = true,

                };
                context.Set<FieldSet>().Add(filedset);
            }

        }
        public void AddSectionColumns(List<int> Id)
        {
            var db = context.FieldSets.Where(a => Id.Contains(a.PageId)).ToList();
            //var fieldsId = context.FieldSets
            //   .Where(sys => sys.PageId == Id)
            //   .ToList();
            
            foreach (var sec in db)
            {
                var section = new Section
                {
                  FieldSetId=sec.Id,
                  Freeze=sec.Freeze,
                  LayOut=sec.LayOut,
                  Reorderable=sec.Reorderable,
                  Order=sec.Order,
                 
                };
                context.Set<Section>().Add(section);
            }

        }
        public void AddFormColumns( int id ,string tblName)
        {
            var columns = context.SysColumns
               .Where(sys => sys.obj_name == tblName)
               .ToList();
           // var columns=context.SysColumns.Where(a=>tblName.Contains(a.obj_name)).ToList();
            // i will optimize this code later 
            var fieldsetId = context.FieldSets.Where(i => i.PageId == id).Select(i => i.Id).FirstOrDefault();
          //  var fieldsetId = context.FieldSets.Where(i =>id.Contains(i.PageId)).Select(i => i.Id).ToList();

            var sectionId = context.Sections.Where(i => i.FieldSetId == fieldsetId).Select(i => i.Id).FirstOrDefault();
            // var sectionId = context.Sections.Where(i =>fieldsetId.Contains(i.FieldSetId)).Select(i => i.Id).ToList();

            foreach (SysColumns column in columns)
            {

                var formcolumn = new FormColumn
                {
                    ColumnName = column.column_name,
                    ColumnOrder = (byte)column.column_order,
                    ColumnType = column.data_type,
                    IsUnique = false,
                    isVisible = column.is_visible == 1,
                    Required = column.is_required == 1,
                    MaxLength = (column.max_length != null ? (short)column.max_length.Value : (short?)null),
                    Lg = null,
                    Md = null,
                    Max = null,
                    Min = null,
                    SectionId = sectionId,

                };
                context.Set<FormColumn>().Add(formcolumn);

            }           

        }
        public IQueryable<FieldSetViewModel> GetFieldSet(int Id)
        {
            // get from user company  
            var Filelds = from filed in context.FieldSets
                          where filed.PageId == Id
                          select new FieldSetViewModel
                          {
                              Id = filed.Id,
                              layout = filed.LayOut,
                              order = filed.Order,
                              HasFieldSetTag = filed.HasTag,
                              LabelEditable = filed.Editable,
                              Collapsable = filed.Collapsable,
                              Collapsed = filed.Collapsed,
                              legend = filed.Legend,
                              Freez = filed.Freeze,
                              Reorderable = filed.Reorderable,

                          };

            return Filelds;
        }        
        public IEnumerable GetFieldSets(int FieldSetId)
        {

          int pageId= context.FieldSets.Where(i => i.Id == FieldSetId).Select(s => s.PageId).FirstOrDefault();
          return  context.FieldSets.Where(w => w.PageId == pageId).Select(s => new { id = s.Id ,name = s.Order + "-" + s.Legend}).ToList();
        }
        public IEnumerable Getsections(int SectionId)
        {
            int fieldsetId = context.Sections.Where(i => i.Id == SectionId).Select(s => s.FieldSetId).FirstOrDefault();
            return context.Sections.Where(i => i.FieldSetId == fieldsetId).Select(r => new { id = r.Id, name = r.Order + "-" + r.Name }).ToList();
        }

        public IQueryable<SectionViewModel> GetSection(int Id)
        {
            var Sections = from filed in context.Sections
                          where filed.FieldSetId == Id
                          select new SectionViewModel
                          {
                              Id = filed.Id,
                              FieldSetDesc = "order"+ filed.FieldSet.Order+"-"+filed.FieldSet.Legend,
                              TempId=filed.FieldSetId,
                              fieldsNumber = filed.FieldsNumber,
                              Freez = filed.Freeze,
                              name = filed.Name,
                              layout = filed.LayOut,
                              labellg = filed.LabelLg,
                              order = filed.Order,
                              labelmd = filed.LabelLg,
                              labelsm = filed.LabelSm,
                              Reorderable = filed.Reorderable
                          };

            return Sections;

        }
        public IQueryable<FormColumnViewModel> GetFormColumns(int Id)
        {
            var FormColumns = from col in context.FormsColumns
                           where col.SectionId == Id
                           select new FormColumnViewModel
                           {
                             Id = col.Id,
                             SectionDesc = "order"+col.Section.Order+"-"+col.Section.Name,
                             TempId = col.SectionId,
                             ColumnType = col.ColumnType,
                             isunique = col.IsUnique,
                             isVisible = col.isVisible,
                             lg = col.Lg,
                             max = col.Max,
                             maxLength = col.MaxLength,
                             md = col.Md,
                             min = col.Min,
                             minLength = col.MinLength,
                             order = col.ColumnOrder,
                             pattern = col.Pattern,
                             name = col.ColumnName,
                             required = col.Required,
                             placeholder = col.PlaceHolder,
                             sm=col.Sm,
                             type = col.InputType,
                             OrgInputType = col.OrgInputType,
                             UniqueColumns = col.UniqueColumns,
                             HtmlAttribute = col.HtmlAttribute,
                             CodeName = col.CodeName,
                             DefaultValue=col.DefaultValue,
                             Formula = col.Formula
                                                                                        
                           };

            return FormColumns;

        }
        public IQueryable<RoleGridColumnsViewModel> GetRoleGridColumns(string RoleId, string objectName, byte version, int companyId, string culture)
        {
         var query = from column in context.GridColumns
                     where column.Grid.CompanyId == companyId && column.Grid.Version == version && column.Grid.ObjectName == objectName && column.OrgInputType != "none"
                     join rc in context.RoleColumns on new { j1 = companyId, j2 = objectName, j3 = version, j4 = RoleId, j5 = column.ColumnName }
                        equals new { j1 = rc.CompanyId, j2 = rc.ObjectName, j3 = rc.Version, j4 = rc.RoleId, j5 = rc.ColumnName } into g
                        from rc in g.DefaultIfEmpty()
                        select new RoleGridColumnsViewModel
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
        public IQueryable<FormColumn> GetFormColsToGridImport(int PageId)
        {
            return context.FormsColumns.Where(a => a.Section.FieldSet.PageId == PageId).OrderBy(a => a.ColumnOrder);
        }
        public void UpdateRoleGridColumns(IEnumerable<RoleGridColumnsViewModel> models, string objectName, byte version, int companyId)
        {
            if (models == null) return;
            var roleId = models.Select(r => r.RoleId).First();
            var roleColumns = context.RoleColumns.Where(a => a.CompanyId == companyId && a.ObjectName == objectName && a.Version == version && a.RoleId == roleId).ToList();

            foreach (var model in models)
            {
                var rc = roleColumns.FirstOrDefault(a => a.ColumnName == model.ColumnName);
                //&& model.isVisible == false
                if (rc == null )
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
                //&& model.isVisible == true
                else if (rc != null )
                {
                    //update record in Role colums
                    rc.isVisible = model.isVisible;
                    rc.isEnabled = model.isEnabled;
                    context.RoleColumns.Attach(rc);
                    context.Entry(rc).State = EntityState.Modified;
                }
            }

        }
        public void Add(FieldSet fieldset)
        {
            context.FieldSets.Add(fieldset);
        }

        public void Attach(FieldSet fieldset)
        {
            context.FieldSets.Attach(fieldset);
        }
        public void Attach(ColumnTitle columnTitle)
        {
            context.ColumnTitles.Attach(columnTitle);
        }

        public void Remove(FieldSet fieldset)
        {
            if (Context.Entry(fieldset).State == EntityState.Detached)
            {
                context.FieldSets.Attach(fieldset);
            }
            context.FieldSets.Remove(fieldset);
        }

        public DbEntityEntry<FieldSet> Entry(FieldSet fieldset)
        {
            return Context.Entry(fieldset);
        }
        public DbEntityEntry<ColumnTitle> Entry(ColumnTitle columnTitle)
        {
            return Context.Entry(columnTitle);
        }
        public void Add(Section section)
        {
            context.Sections.Add(section);
        }

        public void Attach(Section section)
        {
            context.Sections.Attach(section);
        }

        public void Remove(Section section)
        {
            if (Context.Entry(section).State == EntityState.Detached)
            {
                context.Sections.Attach(section);
            }
            context.Sections.Remove(section);
        }
        
        public void Remove(ColumnTitle title)
        {
            if (Context.Entry(title).State == EntityState.Detached)
            {
                context.ColumnTitles.Attach(title);
            }
            context.ColumnTitles.Remove(title);
        }

        public void Remove(RoleColumns rolecolumn)
        {
            if (Context.Entry(rolecolumn).State == EntityState.Detached)
            {
                context.RoleColumns.Attach(rolecolumn);
            }
            context.RoleColumns.Remove(rolecolumn);
        }

        public DbEntityEntry<Section> Entry(Section section)
        {
            return Context.Entry(section);
        }
        public void Add(FormColumn formcolumn)
        {
            context.FormsColumns.Add(formcolumn);
        }
        public void Add(ColumnTitle columnTitle)
        {
            context.ColumnTitles.Add(columnTitle);
        }

        public void Attach(FormColumn formcolumn)
        {
            context.FormsColumns.Attach(formcolumn);
        }

        public void Remove(FormColumn formcolumn)
        {
            if (Context.Entry(formcolumn).State == EntityState.Detached)
            {
                context.FormsColumns.Attach(formcolumn);
            }
            context.FormsColumns.Remove(formcolumn);
        }

        public DbEntityEntry<FormColumn> Entry(FormColumn formcolumn)
        {
            return Context.Entry(formcolumn);
        }
        public void Add(GridColumn GridColumn)
        {
            context.GridColumns.Add(GridColumn);
        }

        public void Attach(GridColumn GridColumn)
        {
            context.GridColumns.Attach(GridColumn);
        }

        public void Remove(GridColumn GridColumn)
        {
            if (Context.Entry(GridColumn).State == EntityState.Detached)
            {
                context.GridColumns.Attach(GridColumn);
            }
            context.GridColumns.Remove(GridColumn);
        }

        public DbEntityEntry<GridColumn> Entry(GridColumn GridColumn)
        {
            return Context.Entry(GridColumn);
        }

        //Flex Data
        public void Add(FlexData flexData)
        {
            context.FlexData.Add(flexData);
        }
        public void Add(FlexColumn flexColumn)
        {
            context.FlexColumns.Add(flexColumn);
        }
        public void Attach(FlexData flexData)
        {
            context.FlexData.Attach(flexData);
        }
        public void Delete(FlexData flexData)
        {
            context.FlexData.Remove(flexData);
        }
        public void Attach(FlexColumn flexColumn)
        {
            context.FlexColumns.Attach(flexColumn);
        }
        public DbEntityEntry<FlexData> Entry(FlexData flexData)
        {
            return Context.Entry(flexData);
        }
        public DbEntityEntry<FlexColumn> Entry(FlexColumn flexColumn)
        {
            return Context.Entry(flexColumn);
        }
        public IEnumerable<FlexData> GetSourceFlexData(int pageId, int sourceId)
        {
            return context.FlexData.Where(fd => fd.PageId == pageId && fd.SourceId == sourceId);
        }
    }
}