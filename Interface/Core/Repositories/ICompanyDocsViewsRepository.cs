using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
    public interface ICompanyDocsViewsRepository : IRepository<CompanyDocsViews>
    {
        CompanyDocsViews GetByStreamID(Guid Stream_Id);
        void GetFileStream_ByStreamID(Guid? Stream_Id, out byte[] File_Stream, out string File_Type);
        List<EmploymentPaper_ToNotify> EmploymentPapersForNotifications();
    }
}
