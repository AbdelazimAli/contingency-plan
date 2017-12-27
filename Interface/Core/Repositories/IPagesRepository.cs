using Model.Domain;
using Model.ViewModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.ViewModel.Personnel;
using System.Data.Entity.Infrastructure;

namespace Interface.Core.Repositories
{
    public interface IPagesRepository : IRepository<ColumnTitle>
    {
        IQueryable<FlexFormGridViewModel> ReadFlexForms(int formType, string culture);
        FlexFormViewModel GetFlexForm(int id, string culture);

        void Add(FlexForm FlexForm);
        DbEntityEntry<FlexForm> Entry(FlexForm FlexForm);
        void Attach(FlexForm FlexForm);
        void Remove(FlexForm FlexForm);
        void Add(FlexFormFS FlexFormFS);
        DbEntityEntry<FlexFormFS> Entry(FlexFormFS FlexFormFS);
        void Attach(FlexFormFS FlexFormFS);
        void Remove(FlexFormFS FlexFormFS);
        void Add(FlexFormColumn FlexFormColumn);
        DbEntityEntry<FlexFormColumn> Entry(FlexFormColumn FlexFormColumn);
        void Attach(FlexFormColumn FlexFormColumn);
        void Remove(FlexFormColumn FlexFormColumn);


        ChartViewModel GetPageData(int companyId, string objectName, byte version);
        void AddCompany(Company company);
        GridDesignViewModel GetGrid(int companyId, string objectName, byte version, string lang, string role);
        void ApplyAdminChanges(GridViewModel grid);
        PageDiv GetPageType(int companyId, string objectName, byte version);
        int GetMenuId(int companyId, string objectName, byte version);
        void Attach(GridColumn info);
        System.Data.Entity.Infrastructure.DbEntityEntry<GridColumn> Entry(GridColumn info);
        FormViewModel GetFormInfo(int companyId, string objectName, byte version, string culture,string RoleId);

        IQueryable<ColumnInfoViewModel> GetColumnInfo(int companyId, string objectName, byte version, string culture);
        IQueryable<FormColumnViewModel> GetGridColumnInfo(int companyId, string objectName, byte version, string culture);
        void ApplyFormDesignChanges(int companyId, FormDesginViewModel form);

        void RemovePageDiv(int companyId, string objectName, byte version);
        IQueryable<FormColumnViewModel> GetFormColumnInfo(int companyId, string objectName, byte version, string culture);
        IQueryable<FormColumnViewModel> GetExcelColumnInfo(int companyId, string objectName, byte version, string culture);
        void Attach(FormColumn info);
        System.Data.Entity.Infrastructure.DbEntityEntry<FormColumn> Entry(FormColumn info);
        void NewCompanyFormDesign(IEnumerable<FormColumnViewModel> models);
        void CopyColumnsInfo(IEnumerable<ColumnInfoViewModel> models);
        void NewCopyGridDesign(PageDiv grid, Menu menu, Company company);
        void NewCopyFormDesign(PageDiv form, Menu menu, Company company);
        IQueryable<RoleFormColumnViewModel> GetRoleFormColumns(string RoleId, string objectName, byte version, int companyId, string culture);
        void UpdateRoleFormColumns(IEnumerable<RoleFormColumnViewModel> models, string objectName, byte version, int companyId);
        IList<TreeViewModel> GetTree(TreeViewParm parm);
        string DropMenuItem(TreeViewModel source, TreeViewModel dest, string table, string culture);
        string DropMenuItemCopy(TreeViewModel model, string table, string culture, bool copyPages);
        IEnumerable<FormLookUpCodeVM> GetFormLookUpCodes(int pageId, string culture);
        int AddToTable(SelectOptionsViewModel model, string tableName, int companyId);
        void RemoveRange(IEnumerable<PageDiv> pages);
        IEnumerable<ColumnTitle> GetColumnTitles(int companyId, IEnumerable<int> menuIds);
        IQueryable<DropDownList> GetRemoteList(string tableName, string query, string formTableName, int companyId, string culture);
        FormFlexColumnsVM GetFormFlexData(int companyId, string objectName, byte version, string culture, int SourceId);
        IQueryable<ColumNameVM> GetColumns(string objectName, int companyId, int version);
        IQueryable<MenuViewModel> GetMenu(string culture, int companyId);
    }
}
