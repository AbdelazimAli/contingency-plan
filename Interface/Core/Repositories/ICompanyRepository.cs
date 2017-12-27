using Model.Domain;
using Model.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Interface.Core.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        CompanyFormViewModel ReadCompany(int id, string culture);
        IList<CompanyDocsViews> GetDocsViews(string Source, int SourceId);
        IList<CompanyDocsViews> GetDocsViews(string Source, IList<int> SourceIds);
        string ExecuteSqlTrans(string[] sqls);
        string ExecuteSql(string sql);

        string TrlsTable(string culture, string table);
        void Add(RoleMenu RMenue);
        IQueryable<CompanyViewModel> GetAllCompanies(string culture);
        IList<DropDownList> CompanyList(string culture);
        IEnumerable<FileUploaderViewModel> GetCompanyDocsViews(string source, int sourceId);
        //void ReadCompanyLogos();
        IQueryable<BranchesViewModel> GetCompanyBranches(int CompanyId);
        IQueryable<PartnersViewModel> GetCompanyPartners(int CompanyId);
        IQueryable<CompanyDocAttrViewModel> GetDocTypeAttr(Guid streamId, int typeId, string culture);
        IEnumerable GetLookUpCodesLists(int typeId, string culture);
        Address GetAddress(int id);
        int CompanyAttachmentsCount(int SourceId);
        void Add(Address address);
        void Add(AudiTrail audit);
        void Add(CompanyDocsViews doc);
        void Add(CompanyBranch branch);
        void Add(CompanyPartner partner);
        void Attach(Address address);
        void Attach(CompanyDocsViews doc);
        void Attach(CompanyBranch branch);
        void Attach(CompanyPartner partner);
        void Remove(CompanyDocsViews doc);
        void Remove(CompanyBranch branch);
        void Remove(CompanyPartner partner);
        DbEntityEntry<Address> Entry(Address address);
        DbEntityEntry<CompanyDocsViews> Entry(CompanyDocsViews doc);
        DbEntityEntry<CompanyBranch> Entry(CompanyBranch branch);
        DbEntityEntry<CompanyPartner> Entry(CompanyPartner partner);
    }
}
