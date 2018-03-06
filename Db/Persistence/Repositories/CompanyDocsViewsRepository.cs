using Db.Persistence.BLL;
using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Db.Persistence.Repositories
{
    public class CompanyDocsViewsRepository : Repository<CompanyDocsViews>, ICompanyDocsViewsRepository
    {
        public CompanyDocsViewsRepository(DbContext context) : base(context)
        {
        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        public CompanyDocsViews GetByStreamID(Guid Stream_Id)
        {
            try
            {
                return context.CompanyDocsView.SingleOrDefault<CompanyDocsViews>(m => m.stream_id == Stream_Id);
            }
            catch
            {
                return null;
            }
        }

        public void GetFileStream_ByStreamID(Guid? Stream_Id, out byte[] File_Stream, out string File_Type)
        {
            File_Stream = null;
            File_Type = string.Empty;
            try
            {
                var Record = context.CompanyDocsView.Where<CompanyDocsViews>(m => m.stream_id == Stream_Id).Select(a => new { file_stream = a.file_stream, file_type = a.file_type }).FirstOrDefault();
                if (Record != null)
                {
                    File_Stream = Record.file_stream;
                    File_Type = Record.file_type;
                }
            }
            catch
            {

            }
        }

        public List<EmploymentPaper_ToNotify> EmploymentPapersForNotifications()
        {
            try
            {
                DateTime Today = DateTime.Now.Date;

                return (from c in context.CompanyDocsView
                        join d in context.DocTypes on c.TypeId equals d.Id
                        join l in context.LookUpUserCodes on d.DocumenType equals l.SysCodeId
                        where l.CodeName == Constants.SystemCodes.DocType.CodeName && c.Source == Constants.Sources.People &&
                              d.HasExpiryDate == true && c.ExpiryDate != null && DbFunctions.AddDays(c.ExpiryDate, -d.NotifyDays) == Today
                        select new EmploymentPaper_ToNotify
                        {
                            Stream_Id = c.stream_id,
                            PaperFileName = c.name,
                            DocTypeName = d.Name,
                            EmpID = c.SourceId.Value,
                            CompanyId=c.CompanyId,
                            ExpiryDate=c.ExpiryDate,
                            AlreadyNotified= context.NotifyLetters.Any(n => n.EmpId == c.SourceId && n.NotifyDate == Today && n.NotifySource == d.Name)
                        }).Where(a=>a.AlreadyNotified==false).ToList();
            }
            catch
            {
                return new List<EmploymentPaper_ToNotify>();
            }
        }
    }
}
