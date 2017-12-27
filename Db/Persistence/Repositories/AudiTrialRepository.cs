using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Administration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Db.Persistence.Repositories
{
   class AudiTrialRepository : Repository<Model.Domain.AudiTrail>, IAudiTrialRepository
    {
        public AudiTrialRepository (DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        public string CopyLanguage(string source, string destintion, int CompanyId ,string culture)
        {
            string msg = "OK";
            var msgList = context.MsgTbl.Where(a => a.Culture == destintion).FirstOrDefault();
            var colTitlesList = context.ColumnTitles.Where(a => a.Culture == destintion).FirstOrDefault();
            var namesList = context.NamesTbl.Where(a => a.Culture == destintion).FirstOrDefault();
            if (msgList == null || colTitlesList == null|| namesList == null)
            {

                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Copy msg table
                        var sql = "INSERT INTO MsgTbl (Culture, [Name], Meaning, JavaScript) SELECT '" + destintion + "', M.[Name], M.Meaning, M.JavaScript FROM MsgTbl M WHERE M.Culture = '" + source + "'";
                        context.Database.ExecuteSqlCommand(sql);

                        //copy column titles
                        sql = "INSERT INTO ColumnTitles (CompanyId, Culture, ObjectName, [Version], ColumnName, Title) SELECT " + CompanyId + ", '" + destintion + "', ObjectName, [Version], ColumnName, Title FROM ColumnTitles WHERE (CompanyId = " + CompanyId + ") AND (Culture = '" + source + "')";
                        context.Database.ExecuteSqlCommand(sql);
                        //copy names table
                        sql = "INSERT INTO NamesTbl (Culture, [Name], Title) SELECT '" + destintion + "', [Name], Title FROM NamesTbl WHERE (Culture = '" + source + "')";
                        context.Database.ExecuteSqlCommand(sql);
                        // Update all
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        msg += ", " + MsgUtils.Instance.Trls(culture, "FailToCopyLanguage");
                        dbContextTransaction.Rollback();
                    }
                    finally
                    {
                        dbContextTransaction.Dispose();
                    }
                }

                return msg;
            }
            else
            {
              return  msg = "Error," + MsgUtils.Instance.Trls(culture, "dublicateCulture");
            }
        }

        //ReadAudiTrials
         public IQueryable<AuditViewModel> ReadAudiTrials(string culture,int companyId)
        {
            var AudiTrials = from a in context.AuditTrail
                             where a.CompanyId == companyId
                             join p in context.PageDiv on new { a.CompanyId, a.ObjectName, a.Version } equals new { p.CompanyId, p.ObjectName, p.Version }
                             join m in context.Menus on p.MenuId equals m.Id
                             join t in context.ColumnTitles on new { p1 = a.CompanyId, p2 = culture, p3 = a.ObjectName, p4 = a.Version, p5 = a.ColumnName }
                             equals new { p1 = t.CompanyId, p2 = t.Culture, p3 = t.ObjectName, p4 = t.Version, p5 = t.ColumnName } into g
                             from t in g.DefaultIfEmpty()
                             join n in context.NamesTbl on new { p1 = culture, p2 = m.Name + m.Sequence } equals new { p1 = n.Culture, p2 = n.Name } into g2
                             from n in g2.DefaultIfEmpty()
                             orderby a.Id
                             select new AuditViewModel
                             {
                                 Id = a.Id,
                                 ObjectName = a.ObjectName,
                                 SourceId = a.SourceId,
                                 ColumnName = t == null ? a.ColumnName : t.Title,
                                 ValueAfter = a.ValueAfter,
                                 ValueBefore = a.ValueBefore,
                                 ModifiedUser = a.ModifiedUser,
                                 ModifiedTime = a.ModifiedTime,
                                 CompanyId = a.CompanyId,
                                 URL = m.Url,
                                 DivType = p.DivType,
                                 Version = p.Version,
                                 MenuId = p.MenuId,
                                 MenuName = n == null ? m.Name : n.Title
                             };

            return AudiTrials;
        }
        
      
        public IEnumerable<string> ReadLanguage()
        {
            var query = context.Languages.Select(s => s.LanguageCulture).ToList();
            return query;
        }

        //ReadMsgs
        public IQueryable<MsgTblViewModel> ReadMsgs(string culture)
        {
            var Msg = from m in context.MsgTbl
                             where m.Culture == culture
                             select new MsgTblViewModel
                             {

                                 Culture = m.Culture,
                                 JavaScript = m.JavaScript,
                                 Meaning = m.Meaning,
                                 Name = m.Name,
                                 Id = m.SequenceId                                                  
                             };
            return Msg;
        }
        public IQueryable<LanguageGridViewModel> ReadLang()
        {
            var query = from l in context.Languages
                        select new LanguageGridViewModel
                        {
                            Id = l.Id,
                            Name = l.Name,
                            IsEnabled = l.IsEnabled,
                            LanguageCulture = l.LanguageCulture,
                            DefaultCurrencyId = l.DefaultCurrencyId,
                            FlagImageFileName = l.FlagImageFileName,
                            Rtl = l.Rtl,
                            DisplayOrder = l.DisplayOrder,
                            UniqueSeoCode = l.UniqueSeoCode
                        };
            return query;
        }
        public void Add(MsgTbl msg)
        {
            context.MsgTbl.Add(msg);
        }
        public void Add(NameTbl name)
        {
            context.NamesTbl.Add(name);
        }
        public void Attach(MsgTbl msg)
        {
            context.MsgTbl.Attach(msg);
        }
        public void Remove(MsgTbl msg)
        {
            if (Context.Entry(msg).State == EntityState.Detached)
            {
                context.MsgTbl.Attach(msg);
            }
            context.MsgTbl.Remove(msg);
        }
        public DbEntityEntry<MsgTbl> Entry(MsgTbl msg)
        {
            return Context.Entry(msg);
        }

    }
}
