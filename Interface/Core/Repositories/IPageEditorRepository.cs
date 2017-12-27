using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Administration;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace Interface.Core.Repositories
{
    public interface IPageEditorRepository:IRepository<PageDiv>
    {
        GridColumnViewModel GetGridColumn(int Id);
        PageDivViewModel GetPageObject(int value, string Search);
        List<ColumnTitle> GetColumnTitles(int companyId, string ObjectName, byte version, string Culture);
        IQueryable<FormColumn> GetFormColsToGridImport(int PageId);
        IQueryable<PageDivViewModel> ReadPage(int companyId, string culture);
        IQueryable<PageDivViewModel> ReadGrid(int companyId, string culture);
        IEnumerable<GridColumn> GetGridColumns(int Id);
        void AddFieldSetColumns(PageDiv Grid);
        void AddSectionColumns(List<int> Id);
        IEnumerable GetTablesHasCust(int companyId, string culture);

        void Add(FlexData flexData);
        void Attach(FlexData flexData);
        void Delete(FlexData flexData);
        DbEntityEntry<FlexData> Entry(FlexData flexData);
        IEnumerable<FlexData> GetSourceFlexData(int pageId, int sourceId);

        void AddPage(PageDiv PageDiv);
        void Add(FieldSet fieldset);
        void Attach(FieldSet fieldset);
        void Remove(FieldSet fieldset);
        void Remove(ColumnTitle title);
        void Add(Section section);
        void Attach(Section section);
        void Remove(Section section);
        void Add(FormColumn formcolumn);
        void Attach(FormColumn formcolumn);
        void Remove(FormColumn formcolumn);
        void Remove(RoleColumns rolecolumn);
        void Add(GridColumn GridColumn);
        void Attach(GridColumn GridColumn);
        void Remove(GridColumn GridColumn);
        DbEntityEntry<GridColumn> Entry(GridColumn GridColumn);

        DbEntityEntry<FormColumn> Entry(FormColumn formcolumn);      
        DbEntityEntry<Section> Entry(Section section);       
        DbEntityEntry<FieldSet> Entry(FieldSet fieldset);
        IQueryable<SectionViewModel> GetSection(int Id);
        IEnumerable GetFieldSets(int FieldSetId);
        void  AddFormColumns(int id,string tblName);
        IQueryable<FormColumnViewModel> GetFormColumns(int Id);
        IEnumerable Getsections(int SectionId);
        IQueryable<FieldSetViewModel> GetFieldSet(int Id);
        IQueryable<ColumnInfoViewModel> GetColumnInfo(int gridId);
        IQueryable<RoleGridColumnsViewModel> GetRoleGridColumns(string RoleId, string objectName, byte version, int companyId, string culture);
        void UpdateRoleGridColumns(IEnumerable<RoleGridColumnsViewModel> models, string objectName, byte version, int companyId);
        void Remove(Address address);
        IEnumerable GetobjectName(int companyId, string culture);
        IEnumerable<FlexColumnsViewModel> GetFlexColumns(int pageId,string name, string culture, int companyId);
        IEnumerable GetInputType(string culture, string code);
        string DeleteFlexColumns(FlexColumn model, string culture);
        void RemoveRange(IEnumerable<ColumnTitle> entities);
        void Add(FlexColumn flexColumn);
        DbEntityEntry<FlexColumn> Entry(FlexColumn flexColumn);
        void Attach(FlexColumn flexColumn);
        void Add(ColumnTitle columnTitle);
        void Attach(ColumnTitle columnTitle);
        DbEntityEntry<ColumnTitle> Entry(ColumnTitle columnTitle);

    }
}
