using Interface.Core.Repositories;
using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using Model.ViewModel.Personnel;
using Db.Persistence.Services;

namespace Db.Persistence.Repositories
{
    class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        //int CompanyId;
        //string Culture;
        // , int companyId, string culture
        public CompanyRepository(DbContext context) : base(context)
        {
            //CompanyId = companyId;
            //Culture = culture;
        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public CompanyDocAttr GetDocAttr_ByAttributeID(int AttributeID)
        {
            try
            {
                return context.CompanyDocAttrs.SingleOrDefault<CompanyDocAttr>(m => m.AttributeId == AttributeID);
            }
            catch
            {
                return null;
            }
        }
        public IQueryable<CompanyDocAttr> GetDocAttr_ByStreamID(Guid Stream_Id)
        {
            try
            {
                return context.CompanyDocAttrs.Where<CompanyDocAttr>(m => m.StreamId == Stream_Id);
            }
            catch
            {
                return new List<CompanyDocAttr>().AsQueryable();
            }
        }
        public IQueryable<CompanyDocAttrViewModel> GetDocTypeAttr(int typeId, string culture, Guid? streamId = null)
        {
            var result = new List<CompanyDocAttrViewModel>().AsQueryable();
            var BasicQuery = context.DocTypeAttrs.AsQueryable<DocTypeAttr>();//

            if (typeId > 0)
                BasicQuery = BasicQuery.Where<DocTypeAttr>(d => d.TypeId == typeId);

            if (streamId != null)
            {
                result = from dt in BasicQuery
                         join cd in context.CompanyDocAttrs on dt.Id equals cd.AttributeId into g
                         from cd in g.Where(a => a.StreamId == streamId).DefaultIfEmpty()
                         join code in context.LookUpCodes on cd.ValueId equals code.Id into g2
                         from code in g2.DefaultIfEmpty()
                         join t in context.LookUpTitles on new { code.CodeName, code.CodeId } equals new { t.CodeName, t.CodeId } into g3
                         from t in g3.Where(a => a.Culture == culture).DefaultIfEmpty()

                         select new CompanyDocAttrViewModel
                         {
                             Id = dt.Id,
                             Insert = cd == null,
                             TypeId = dt.TypeId,
                             Attribute = dt.Attribute,
                             InputType = dt.InputType,
                             CodeName = dt.CodeName,
                             Value = cd.Value,
                             ValueId = cd.ValueId,
                             ValueText = (dt.InputType == (int)Constants.Enumerations.InputTypesEnum.Select && code != null ? (t == null ? code.Name : t.Title) : cd.Value),
                             IsRequired=dt.IsRequired
                         };

            }
            else
            {
                result = BasicQuery.Select(basic => new CompanyDocAttrViewModel
                {
                    Id = basic.Id,
                    Insert = true,
                    TypeId = basic.TypeId,
                    Attribute = basic.Attribute,
                    InputType = basic.InputType,
                    CodeName = basic.CodeName,
                    Value = string.Empty,
                    ValueId = null,
                    ValueText = string.Empty,
                    IsRequired = basic.IsRequired
                });
            }

            foreach (var r in result)
                if (r.InputType == (int)Constants.Enumerations.InputTypesEnum.Date && !String.IsNullOrEmpty(r.ValueText)) r.ValueText = r.ValueText.ToMyDateTime(culture).ToMyDateString(culture, "d");
                else if (r.InputType == (int)Constants.Enumerations.InputTypesEnum.DateTime && !String.IsNullOrEmpty(r.ValueText)) r.ValueText = r.ValueText.ToMyDateTime(culture).ToShortTimeString();

            return result.AsQueryable();
        }



        public string TrlsTable(string culture, string table)
        {
            return context.Database.SqlQuery<string>("SELECT dbo.fn_TrlsName(M.[Name] + CAST(M.[Sequence] AS nvarchar(5)), '" + culture + "') FROM Menus M WHERE M.Id = (SELECT TOP 1 MenuId FROM PageDivs WHERE TableName = '" + table + "')").FirstOrDefault();
        }

        public IQueryable<CompanyDocsViews> GetDocsViews_Queryable(string Source, int SourceID)
        {
            try
            {
                return context.CompanyDocsView.Where<CompanyDocsViews>(c => c.Source.Equals(Source) && c.SourceId == SourceID);
            }
            catch
            {
                return new List<CompanyDocsViews>().AsQueryable();
            }
        }

        public List<StreamID_DocTypeFormViewModel> GetDocsViews_Uploaded(string Source, int SourceId, string CodeName/*,int SysCodeId*/, string Lang)
        {
            try
            {
                var BasicQuery = GetDocsViews_Queryable(Source, SourceId);

                var DocsViews = (from c in BasicQuery
                                 join d in context.DocTypes on c.TypeId equals d.Id
                                 //join l in context.LookUpUserCodes on d.DocumenType equals l.SysCodeId//new { p1 = d.Name, p2 = d.DocumenType } equals new { p1 = l.CodeName, p2 = l.CodeId }
                                // where c.Source == Source /*&& l.CodeName == CodeName *///&& l.SysCodeId == SysCodeId
                                 select new StreamID_DocTypeFormViewModel
                                 {
                                     Stream_Id = c.stream_id,
                                     DocTypeFormViewModel = new DocTypeFormViewModel
                                     {
                                         Id = d.Id,
                                         Name = HrContext.TrlsName(d.Name, Lang),
                                         RequiredOpt = d.RequiredOpt,
                                         HasExpiryDate = d.HasExpiryDate
                                     }
                                 }).ToList();
                return DocsViews;
            }
            catch
            {
                return new List<StreamID_DocTypeFormViewModel>();
            }
        }

        public bool IsAllRequiredPapersUploaded(string Source, int SourceId, List<int> RequiredDocTypeIDs, int? DocTypeID_AddEditeMode, int? DocTypeID_DeleteMode)
        {
            try
            {
                var BasicQuery = GetDocsViews_Queryable(Source, SourceId);

                List<int> UploadedDocTypeIDs = BasicQuery.Select(a => a.TypeId.Value).ToList();

                if (DocTypeID_AddEditeMode != null)
                    return !RequiredDocTypeIDs.Any(a => a != DocTypeID_AddEditeMode && !UploadedDocTypeIDs.Contains(a));

                if (DocTypeID_DeleteMode != null)
                {
                    UploadedDocTypeIDs.Remove(DocTypeID_DeleteMode.Value);
                    return !RequiredDocTypeIDs.Any(a => !UploadedDocTypeIDs.Contains(a));
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        public IList<CompanyDocsViews> GetDocsViews(string Source, int SourceId)
        {
            return context.CompanyDocsView.Include("DocType").Where(d => d.Source == Source && d.SourceId == SourceId).ToList();
        }

        public IList<CompanyDocsViews> GetDocsViews(string Source, IList<int> SourceIds)
        {
            return context.CompanyDocsView.Where(d => d.Source == Source && SourceIds.Contains(d.SourceId.Value)).ToList();
        }
        
        public string ExecuteSql(string sql)
        {
            return ExecuteSqlTrans(new string[] { sql });
        }

        public string ExecuteSqlTrans(string[] sqls)
        {
            int result = 0;
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var sql in sqls)
                        result = context.Database.ExecuteSqlCommand(sql);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return ex.Message;
                }

                context.SaveChanges();
                dbContextTransaction.Commit();
            }

            return result.ToString();
        }

        public CompanyFormViewModel ReadCompany(int id, string culture)
        {
            var query = (from c in context.Companies
                         where c.Id == id
                         select new CompanyFormViewModel
                         {
                             Id = c.Id,
                             CommFileNo = c.CommFileNo,
                             Consolidation = c.Consolidation,
                             CountryId = c.CountryId,
                             Email = c.Email,
                             InsuranceNo = c.InsuranceNo,
                             Language = c.Language,
                             Name = c.Name,
                             LocalName = HrContext.TrlsName(c.Name, culture),
                             Memo = c.Memo,
                             Purpose = c.Purpose,
                             TaxCardNo = c.TaxCardNo,
                             WebSite = c.WebSite,
                             TaxAuthority = c.TaxAuthority,
                             SearchName = c.SearchName,
                             CreatedTime = c.CreatedTime,
                             CreatedUser = c.CreatedUser,
                             ModifiedTime = c.ModifiedTime,
                             ModifiedUser = c.ModifiedUser,
                             Attachments = HrContext.GetAttachments("Company", c.Id),
                             LegalForm = c.LegalForm,
                             Office = c.Office,
                             Region = c.Region,
                             Responsible = c.Responsible
                         }).FirstOrDefault();
            return query;
        }
        public void sum()
        {

        }
        public IQueryable<CompanyDocAttrViewModel> GetDocTypeAttr(Guid streamId, int typeId, string culture)
        {
            var result = (from dt in context.DocTypeAttrs
                          where dt.TypeId == typeId
                          join cd in context.CompanyDocAttrs on dt.Id equals cd.AttributeId into g
                          from cd in g.Where(gg => gg.StreamId == streamId).DefaultIfEmpty()
                          join code in context.LookUpCodes on cd.ValueId equals code.Id into g2
                          from code in g2.DefaultIfEmpty()
                          join t in context.LookUpTitles on new { code.CodeName, code.CodeId } equals new { t.CodeName, t.CodeId } into g3
                          from t in g3.Where(a => a.Culture == culture).DefaultIfEmpty()
                          select new CompanyDocAttrViewModel
                          {
                              Id = dt.Id,
                              Insert = cd == null,
                              TypeId = dt.TypeId,
                              Attribute = dt.Attribute,
                              InputType = dt.InputType,
                              CodeName = dt.CodeName,
                              Value = cd.Value,
                              ValueId = cd.ValueId,
                              ValueText = (dt.InputType == 3 && code != null ? (t == null ? code.Name : t.Title) : cd.Value)
                          }).ToList();


            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culture);
            foreach (var r in result)
                if (r.InputType == 4 && !String.IsNullOrEmpty(r.ValueText)) r.ValueText = Convert.ToDateTime(r.ValueText).ToString("d", ci);
                else if (r.InputType == 6 && !String.IsNullOrEmpty(r.ValueText)) r.ValueText = Convert.ToDateTime(r.ValueText).ToShortTimeString();

            return result.AsQueryable();
        }

        public IEnumerable GetLookUpCodesLists(int typeId, string culture)
        {

            var result = from doc in context.DocTypeAttrs
                         where doc.TypeId == typeId
                         join code in context.LookUpCodes on doc.CodeName equals code.CodeName
                         join t in context.LookUpTitles on new { code.CodeName, code.CodeId } equals new { t.CodeName, t.CodeId } into g
                         from t in g.Where(a => a.Culture == culture).DefaultIfEmpty()
                         select new
                         {
                             Id = code.Id,
                             CodeName = code.CodeName,
                             CodeId = code.CodeId,
                             Name = (t != null ? t.Title : code.Name)
                         };

            return result.Distinct();
        }

        public IEnumerable<FileUploaderViewModel> GetCompanyDocsViews(string source, int sourceId)
        {

            return context.CompanyDocsView
              .Where(d => d.Source == source && d.SourceId == sourceId)
              .Select(d => new FileUploaderViewModel
              {
                  Id = d.stream_id,
                  CompanyId = d.CompanyId,
                  stream_id = d.stream_id,
                  Description = d.Description,
                  DocType = d.TypeId,
                  DocName = d.DocType.Name,
                  ExpiryDate = d.ExpiryDate,
                  creation_time = d.creation_time,
                  last_access_time = d.last_access_time,
                  CreatedUser = d.CreatedUser,
                  ModifiedUser = d.ModifiedUser,
                  Keyword = d.Keyword,
                  AccessLevel = d.AccessLevel
              })
              .ToList();
        }


        public IQueryable<CompanyViewModel> GetAllCompanies(string culture)
        {
            var lang = culture.Split('-')[0];
            var result = context.Companies.Select(company => new CompanyViewModel
            {
                Id = company.Id,
                SearchName = company.SearchName != null ? company.SearchName : "",
                Name = company.Name,
                LocalName = HrContext.TrlsName(company.Name, culture),
                LogoUrl = HrContext.GetCompanyDoc("Company", company.Id, 1),
                Purpose = HrContext.GetLookUpCode("Purpose", company.Purpose.Value, culture),
                Country = lang == "ar" ? company.Country.NameAr : company.Country.Name,
                Email = company.Email,
                WebSite = company.WebSite,
                Attachement = HrContext.GetDoc("CompanyLogo", company.Id)
            });

            return result;
        }

        public IList<DropDownList> CompanyList(string culture)
        {
            var result = context.Companies.Select(company => new DropDownList
            {
                Id = company.Id,
                Name = HrContext.TrlsName(company.Name, culture),
            }).ToList();

            return result;
        }
        public IQueryable<PartnersViewModel> GetCompanyPartners(int CompanyId)
        {
            var result = context.CompanyPartners
                .Where(partner => partner.CompanyId == CompanyId)
                .OrderBy(partner => partner.Name)
                .Select(partner => new PartnersViewModel
                {
                    Id = partner.Id,
                    CompanyId = partner.CompanyId,
                    Name = partner.Name,
                    JobTitle = partner.JobTitle,
                    NationalId = partner.NationalId,
                    Email = partner.Email,
                    Telephone = partner.Telephone,
                    Mobile = partner.Mobile,
                    AddressId = partner.AddressId,
                    CreatedUser = partner.CreatedUser,
                    CreatedTime = partner.CreatedTime,
                    ModifiedUser = partner.ModifiedUser,
                    ModifiedTime = partner.ModifiedTime
                });

            return result;
        }

        public int CompanyAttachmentsCount(int SourceId)
        {
            return HrContext.GetAttachments("Company", SourceId);
        }
        public void Add(CompanyDocsViews doc)
        {
            context.CompanyDocsView.Add(doc);
        }
        public void Add(RoleMenu RMenue)
        {
            context.RoleMenus.Add(RMenue);
        }
        public void Add(CompanyPartner partner)
        {
            context.CompanyPartners.Add(partner);
        }


        public void Add(AudiTrail audit)
        {
            context.AuditTrail.Add(audit);
        }

        public void Attach(CompanyDocsViews doc)
        {
            context.CompanyDocsView.Attach(doc);
        }
        public void Attach(CompanyPartner partner)
        {
            context.CompanyPartners.Attach(partner);
        }
        public void Attach(CompanyDocAttr companyDocAttr)
        {
            context.CompanyDocAttrs.Attach(companyDocAttr);
        }

        public void Remove(CompanyDocsViews doc)
        {
            if (Context.Entry(doc).State == EntityState.Detached)
            {
                context.CompanyDocsView.Attach(doc);
            }
            context.CompanyDocsView.Remove(doc);
        }

        public void Remove(CompanyPartner partner)
        {
            if (Context.Entry(partner).State == EntityState.Detached)
            {
                context.CompanyPartners.Attach(partner);
            }
            context.CompanyPartners.Remove(partner);
        }

        public DbEntityEntry<CompanyDocsViews> Entry(CompanyDocsViews doc)
        {
            return Context.Entry(doc);
        }
        public DbEntityEntry<CompanyPartner> Entry(CompanyPartner partner)
        {
            return Context.Entry(partner);
        }
    }
}
