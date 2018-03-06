using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
  public  interface IDocTypesRepository : IRepository<DocType>
    {
         IQueryable<DocTypeFormViewModel> GetPapers(int JobID, int Gender, int Nationality, int CompanyId, string Lang, List<int> Include_SystemCodes, List<int> Exclude_SystemCodes);
        bool HasExpirationDate(int DocTypeID);
        int GetDocumentType_ByDocTypeID(int DocTypeID);
    }
}
