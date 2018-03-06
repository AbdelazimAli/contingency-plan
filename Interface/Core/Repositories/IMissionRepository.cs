using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using Interface.Core.Repositories;
using Model.ViewModel.MissionRequest;

namespace Interface.Core.Repositories
{
    public interface IMissionRepository:IRepository<ErrandRequest>
    {
        IQueryable<ErrandIndexRequestViewModel> ReadMissionRequest(int companyId, byte Tab, byte Range, DateTime? Start, DateTime? End, string culture);
        IQueryable<ErrandIndexRequestViewModel> ReadMissionRequestArchieve(int companyId, byte Range, DateTime? Start, DateTime? End, string culture);
        ErrandRequest GetMissionByiD(int? Id);
        ErrandFormRequestViewModel ReadMissionRequest(int Id);
        SiteInfoViewModel GetFullSiteInfo(int SiteId,int ErrandType,int EmpId, string culture);
        CloseMissionCiewModel CloseMission(int id);
        byte[] GetBytes(int Id);
    }
}
