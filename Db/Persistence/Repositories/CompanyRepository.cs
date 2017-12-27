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
        public string TrlsTable(string culture, string table)
        {
            return context.Database.SqlQuery<string>("SELECT dbo.fn_TrlsName(M.[Name] + CAST(M.[Sequence] AS nvarchar(5)), '" + culture + "') FROM Menus M WHERE M.Id = (SELECT TOP 1 MenuId FROM PageDivs WHERE TableName = '" + table + "')").FirstOrDefault();
        }

        public IList<CompanyDocsViews> GetDocsViews(string Source, int SourceId)
        {
            return context.CompanyDocsView.Include("DocType").Where(d => d.Source == Source && d.SourceId == SourceId).ToList();
        }

        public IList<CompanyDocsViews> GetDocsViews(string Source, IList<int> SourceIds)
        {
            return context.CompanyDocsView.Where(d => d.Source == Source && SourceIds.Contains(d.SourceId.Value)).ToList();
        }
        public Address GetAddress(int id)
        {
            var address = context.Set<Address>().Find(id);

            if (address == null)
                return new Address();

            return address;
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

        public  CompanyFormViewModel ReadCompany(int id,string culture)
        {
            var query = (from c in context.Companies
                        where c.Id == id
                         join a in context.Addresses on c.AddressId equals a.Id into g2
                         from a in g2.DefaultIfEmpty()
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
                            Address = a == null ? "" : (a.Address1 + (a.Address2 == null ? "" : ", " + a.Address2) + (a.Address3 == null ? "" : ", " + c.Address.Address3)),
                            AddressId = c.AddressId,
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
                              ValueText = (dt.InputType == 3  && code != null ? (t == null ? code.Name : t.Title) : cd.Value)
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
                Country = lang == "ar"? company.Country.NameAr : company.Country.Name,
                Email = company.Email,
                WebSite = company.WebSite,
                Attachement = HrContext.GetDoc("ComoanyLogo", company.Id)
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

        public IQueryable<BranchesViewModel> GetCompanyBranches(int CompanyId)
        {
            var result = context.CompanyBranches
                .Where(branch => branch.CompanyId == CompanyId)
                .OrderBy(branch => branch.BranchNo)
                .Select(branch => new BranchesViewModel
                {
                    Id = branch.Id,
                    BranchNo = branch.BranchNo,
                    Name = branch.Name,
                    Email = branch.Email,
                    Telephone = branch.Telephone,
                    Address = branch.Address.Address1 + (branch.Address.Address2 == null ? "" : ", " + branch.Address.Address2) + (branch.Address.Address3 == null ? "" : ", " + branch.Address.Address3),
                    AddressId = branch.AddressId,
                    CompanyId = branch.CompanyId,
                    CreatedUser = branch.CreatedUser,
                    CreatedTime = branch.CreatedTime,
                    ModifiedUser = branch.ModifiedUser,
                    ModifiedTime = branch.ModifiedTime,

                });

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
                    Address = partner.Address.Address1 + (partner.Address.Address2 == null ? "" : ", " + partner.Address.Address2) + (partner.Address.Address3 == null ? "" : ", " + partner.Address.Address3),
                    AddressId = partner.AddressId,
                    CreatedUser=partner.CreatedUser,
                    CreatedTime = partner.CreatedTime,
                    ModifiedUser = partner.ModifiedUser,
                    ModifiedTime = partner.ModifiedTime,



                });

            return result;
        }

        //public void ReadCompanyLogos()
        //{
        //    // Get logo folder
        //    var logosFolder = System.AppDomain.CurrentDomain.BaseDirectory + "Content\\Logos";
        //    // Get last access time
        //    //var lastWriteFileTime = Directory.GetLastWriteTime(logosFolder);
        //    //var CreationTime = Directory.GetCreationTime(logosFolder);
        //    //if  (lastWriteFileTime < CreationTime || Directory.GetFiles(logosFolder).Count() <= 1) lastWriteFileTime = new System.DateTime(2000, 1, 1);
        //    // Get required file need to write
        //    var files = context.CompanyDocsView.Where(f => f.is_directory == false && f.Source == "Company" && f.DocType.DocumenType == 1).Select(f => new { name = f.name, data = f.file_stream }).ToList();
        //    // loop to write files
        //    foreach (var file in files)
        //    {
        //        File.WriteAllBytes(Path.Combine(logosFolder, file.name), file.data);
        //    }
        //}

        public int CompanyAttachmentsCount(int SourceId)
        {
            return HrContext.GetAttachments("Company", SourceId);
        }

        public void Add(Address address)
        {
            context.Addresses.Add(address);
        }
       
        public void Add(CompanyDocsViews doc)
        {
             context.CompanyDocsView.Add(doc);
        }
        public void Add(RoleMenu RMenue)
        {
            context.RoleMenus.Add(RMenue);
        }

        public void Add(CompanyBranch branch)
        {
            context.CompanyBranches.Add(branch);
        }

        public void Add(CompanyPartner partner)
        {
            context.CompanyPartners.Add(partner);
        }
       

        public void Add(AudiTrail audit)
        {
            context.AuditTrail.Add(audit);
        }

        public void Attach(Address address)
        {
            context.Addresses.Attach(address);
        }

        public void Attach(CompanyDocsViews doc)
        {
            context.CompanyDocsView.Attach(doc);
        }

        public void Attach(CompanyBranch branch)
        {
            context.CompanyBranches.Attach(branch);
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

        void Remove(Address address, int? addressId)
        {
            // record doesn't contain address
            if (addressId == null)
                return;

            // Address not loaded for new records
            if (address == null)
                address = context.Addresses.Find(addressId);

            // remove address
            if (address != null)
            {
                if (Context.Entry(address).State == EntityState.Detached)
                {
                    context.Addresses.Attach(address);
                }
                context.Addresses.Remove(address);
            }
        }

        public void Remove(CompanyBranch branch)
        {
            // delete related address if found
            Remove(branch.Address, branch.AddressId);    

            if (Context.Entry(branch).State == EntityState.Detached)
            {
                context.CompanyBranches.Attach(branch);
            }
            context.CompanyBranches.Remove(branch);
        }

        public void Remove(CompanyPartner partner)
        {
            // delete related address if found
            Remove(partner.Address, partner.AddressId);

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
        public DbEntityEntry<CompanyBranch> Entry(CompanyBranch branch)
        {
            return Context.Entry(branch);
        }
        public DbEntityEntry<CompanyPartner> Entry(CompanyPartner partner)
        {
            return Context.Entry(partner);
        }
        public DbEntityEntry<Address> Entry(Address address)
        {
            return Context.Entry(address);
        }
    }
}
