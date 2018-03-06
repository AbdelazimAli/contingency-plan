using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Interface.Core.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        bool IsAllRequiredPapersUploaded(string Source, int SourceId, List<int> RequiredDocTypeIDs, int? DocTypeID_AddEditeMode, int? DocTypeID_DeleteMode);
        CompanyDocAttr GetDocAttr_ByAttributeID(int AttributeID);
        IQueryable<CompanyDocAttr> GetDocAttr_ByStreamID(Guid Stream_Id);
        List<StreamID_DocTypeFormViewModel> GetDocsViews_Uploaded(string Source, int SourceId, string CodeName/*, int SysCodeId*/, string Lang);
        IQueryable<CompanyDocsViews> GetDocsViews_Queryable(string Source, int SourceID);
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
        IQueryable<PartnersViewModel> GetCompanyPartners(int CompanyId);
        IQueryable<CompanyDocAttrViewModel> GetDocTypeAttr(Guid streamId, int typeId, string culture);
        IQueryable<CompanyDocAttrViewModel> GetDocTypeAttr(int typeId, string culture, Guid? streamId = null);
        IEnumerable GetLookUpCodesLists(int typeId, string culture);
        int CompanyAttachmentsCount(int SourceId);
        void Add(AudiTrail audit);
        void Add(CompanyDocsViews doc);
        void Add(CompanyPartner partner);
        void Attach(CompanyDocsViews doc);
        void Attach(CompanyPartner partner);
        void Remove(CompanyDocsViews doc);
        void Remove(CompanyPartner partner);
        DbEntityEntry<CompanyDocsViews> Entry(CompanyDocsViews doc);
        DbEntityEntry<CompanyPartner> Entry(CompanyPartner partner);
    }
}
