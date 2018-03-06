using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel.MissionRequest;
using Model.ViewModel.Notification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Persistence.Repositories
{
    public class MissionRepository:Repository<ErrandRequest>, IMissionRepository
    {
        public MissionRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public IQueryable<ErrandIndexRequestViewModel> ReadMissionRequest(int companyId, byte Tab, byte Range, DateTime? Start, DateTime? End, string culture)
        {
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, companyId, out Start, out End);
    
            DateTime today = DateTime.Today.Date;

            var q1 = from l in context.ErrandRequests
                     where l.CompanyId == companyId
                     select l;

            if (Range != 10) // Allow date range
                q1 = q1.Where(l => Start <= l.StartDate && l.EndDate <= End);

            if (Tab == 1) //Pending
                q1 = q1.Where(l => l.ApprovalStatus < 6);
            else if (Tab == 2) //Approved
                q1 = q1.Where(l => l.ApprovalStatus == 6 && today <= l.EndDate);
            else if (Tab == 3) //Rejected
                q1 = q1.Where(l => l.ApprovalStatus == 9 && today <= l.EndDate);
            else if (Tab == 4) //Archive
                q1 = q1.Where(l => today > l.EndDate);

            var query =
                (from l in q1
                 join a in context.Assignments on l.EmpId equals a.EmpId
                 where (a.CompanyId == companyId && a.AssignDate <= l.StartDate && a.EndDate >= l.StartDate)
                 join p in context.People on l.EmpId equals p.Id
                 join wft in context.WF_TRANS on new { p1 = "ErrandRequest" , p2 = l.CompanyId, p3 = l.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                 from wft in g.DefaultIfEmpty()
                 join ap in context.People on wft.AuthEmp equals ap.Id into g1
                 from ap in g1.DefaultIfEmpty()
                 join role in context.Roles on wft.RoleId equals role.Id into g4
                 from role in g4.DefaultIfEmpty()
                 select new ErrandIndexRequestViewModel
                 {
                     Id = l.Id,
                     Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                     Manager = HrContext.TrlsName(l.Manager.Title + " " + l.Manager.FirstName + " " + l.Manager.Familyname, culture),
                     Site = a.BranchId != null ? HrContext.TrlsName(l.Site.Name, culture) : " ",
                     EmpId = l.EmpId,
                     CompanyId = l.CompanyId,
                     EndDate = l.EndDate.ToString(),
                     StartDate = l.StartDate.ToString(),
                     Subject = l.Subject,
                     MultiDays = l.MultiDays,
                     ApprovalStatus = l.ApprovalStatus,
                     RoleId = wft.RoleId.ToString(),
                     DeptId = wft.DeptId,
                     PositionId = wft.PositionId,
                     AuthBranch = wft.AuthBranch,
                     AuthDept = wft.AuthDept,
                     AuthEmp = wft.AuthEmp,
                     AuthEmpName = HrContext.TrlsName(ap.Title + " " + ap.FirstName + " " + ap.Familyname, culture),
                     AuthPosName = role == null ? "" : role.Name,
                     AuthPosition = wft.AuthPosition,
                     BranchId = wft.BranchId,
                     WorkflowTime = wft.CreatedTime,
                     Status = l.ApprovalStatus
                 }).ToList().Select(e => new ErrandIndexRequestViewModel
                 {
                     Id = e.Id,
                     Employee = e.Employee,
                     Manager = e.Manager,
                     Site = e.Site,
                     EmpId = e.EmpId,
                     CompanyId = e.CompanyId,
                     EndDate = e.MultiDays == false ? Convert.ToDateTime(e.EndDate).ToString("f") : Convert.ToDateTime(e.EndDate).ToString("d MMM yyyy"),
                     StartDate =e.MultiDays == false ? Convert.ToDateTime(e.StartDate).ToString("f") :Convert.ToDateTime(e.StartDate).ToString("d MMM yyyy") ,
                     Subject = e.Subject,
                     MultiDays = e.MultiDays,
                     ApprovalStatus = e.ApprovalStatus,
                     AuthBranch = e.AuthBranch,
                     AuthEmpName = e.AuthEmpName,
                     AuthEmp = e.AuthEmp,
                     AuthPosition = e.AuthPosition,
                     DeptId = e.DeptId,
                     RoleId = e.RoleId,
                     PositionId = e.PositionId,
                     BranchId = e.BranchId,
                     WorkflowTime = e.WorkflowTime,
                     AuthPosName = e.AuthPosName,
                     Status = e.Status,
                     AuthDept = e.AuthDept                     
                 }).OrderByDescending(s=>s.EndDate);

            return query.AsQueryable();
        }
        public IQueryable<ErrandIndexRequestViewModel> ReadMissionRequestArchieve(int companyId,byte Range, DateTime? Start, DateTime? End, string culture)
        {
            //10- All, 0-Custom
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, companyId, out Start, out End);

            DateTime Today = DateTime.Today.Date;
            var q1 = from l in context.ErrandRequests
                     where l.CompanyId == companyId
                     select l;

            if (Range != 10) // Allow date range
                q1 = q1.Where(l => Start <= l.StartDate && l.EndDate <= End);


            var query =
           (from l in q1
            join a in context.Assignments on l.EmpId equals a.EmpId
            where (a.CompanyId == companyId && a.AssignDate <= l.StartDate && a.EndDate >= l.StartDate && l.ApprovalStatus >= 6)
            join p in context.People on l.EmpId equals p.Id
            join wft in context.WF_TRANS on new { p1 = "ErrandRequest", p2 = l.CompanyId, p3 = l.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
            from wft in g.DefaultIfEmpty()
            join ap in context.People on wft.AuthEmp equals ap.Id into g1
            from ap in g1.DefaultIfEmpty()
            join role in context.Roles on wft.RoleId equals role.Id into g4
            from role in g4.DefaultIfEmpty()
            select new ErrandIndexRequestViewModel
            {
                Id = l.Id,
                Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                Manager = HrContext.TrlsName(l.Manager.Title + " " + l.Manager.FirstName + " " + l.Manager.Familyname, culture),
                Site = a.BranchId != null ? HrContext.TrlsName(l.Site.Name, culture) : " ",
                EmpId = l.EmpId,
                CompanyId = l.CompanyId,
                EndDate = l.EndDate.ToString(),
                StartDate = l.StartDate.ToString(),
                Subject = l.Subject,
                MultiDays = l.MultiDays,
                ApprovalStatus = l.ApprovalStatus,
                RoleId = wft.RoleId.ToString(),
                DeptId = wft.DeptId,
                PositionId = wft.PositionId,
                AuthBranch = wft.AuthBranch,
                AuthDept = wft.AuthDept,
                AuthEmp = wft.AuthEmp,
                AuthEmpName = HrContext.TrlsName(ap.Title + " " + ap.FirstName + " " + ap.Familyname, culture),
                AuthPosName = role == null ? "" : role.Name,
                AuthPosition = wft.AuthPosition,
                BranchId = wft.BranchId,
                WorkflowTime = wft.CreatedTime,
                Status = l.ApprovalStatus
            }).ToList().Select(e => new ErrandIndexRequestViewModel
            {
                Id = e.Id,
                Employee = e.Employee,
                Manager = e.Manager,
                Site = e.Site,
                EmpId = e.EmpId,
                CompanyId = e.CompanyId,
                EndDate = e.MultiDays == false ? Convert.ToDateTime(e.EndDate).ToString("f") : Convert.ToDateTime(e.EndDate).ToString("d MMM yyyy"),
                StartDate = e.MultiDays == false ? Convert.ToDateTime(e.StartDate).ToString("f") : Convert.ToDateTime(e.StartDate).ToString("d MMM yyyy"),
                Subject = e.Subject,
                MultiDays = e.MultiDays,
                ApprovalStatus = e.ApprovalStatus,
                AuthBranch = e.AuthBranch,
                AuthEmpName = e.AuthEmpName,
                AuthEmp = e.AuthEmp,
                AuthPosition = e.AuthPosition,
                DeptId = e.DeptId,
                RoleId = e.RoleId,
                PositionId = e.PositionId,
                BranchId = e.BranchId,
                WorkflowTime = e.WorkflowTime,
                AuthPosName = e.AuthPosName,
                Status = e.Status,
                AuthDept = e.AuthDept
            }).OrderByDescending(s => s.EndDate);

            return query.AsQueryable();
        }
        public ErrandFormRequestViewModel ReadMissionRequest(int Id)
        {
            var record = (from e in context.ErrandRequests
                          where e.Id == Id
                          select new ErrandFormRequestViewModel
                          {
                              Id = e.Id,
                              StartTime = e.StartDate.ToString(),
                              EndTime = e.EndDate.ToString(),
                              EndDate = e.EndDate,
                              StartDate = e.StartDate,
                              ManagerId = e.ManagerId,
                              ApprovalStatus = e.ApprovalStatus,
                              MultiDays = e.MultiDays,
                              Reason = e.Reason,
                              Subject = e.Subject,
                              ErrandType = e.ErrandType,                              
                              EmpId = e.EmpId,
                              SiteId = e.SiteId                              
                          }).FirstOrDefault();

            record.StartTime = DateTime.Parse(record.StartTime).ToString("HH:mm");
            record.EndTime = DateTime.Parse(record.EndTime).ToString("HH:mm");
            return record;
        }

        public SiteInfoViewModel GetFullSiteInfo(int SiteId,int ErrandType,int EmpId, string culture)
        {
            var query = (from l in context.Sites
                         where l.Id == SiteId
                         join c in context.Cities on l.CityId equals c.Id
                         join d in context.Districts on l.DistrictId equals d.Id
                         join p in context.People on EmpId equals p.Id
                         join a in context.Assignments on EmpId equals a.EmpId 
                         join b in context.Branches on a.BranchId equals b.Id
                         select new SiteInfoViewModel
                         {
                             City = culture.Substring(0, 2) == "ar" ? c.NameAr : c.Name,
                             District = culture.Substring(0, 2) == "ar" ? d.NameAr : d.Name,
                             Address = l.Address1,
                             Longitude = l.Longitude,
                             Latitude = l.Latitude,
                             dest_Latitude = (ErrandType == 1 ? p.Latitude : b.Latitude),
                             dest_Longitude = (ErrandType == 1 ? p.Longitude :b.Latitude)
                         }).FirstOrDefault();
            return query;
        }
        public ErrandRequest GetMissionByiD(int? Id)
        {
            return context.ErrandRequests.Find(Id);
        }
        public CloseMissionCiewModel CloseMission (int id)
        {

            var query = (from m in context.ErrandRequests
                        where m.Id == id
                        select new CloseMissionCiewModel
                        {
                            Expenses = m.Expenses,
                            Notes = m.Notes
                        }).FirstOrDefault();
            return query;
        }

        public byte[] GetBytes(int Id)
        {
           var ArrayOfBytes = context.CompanyDocsView.Where(m => m.Source== "ErrandRequest" && m.SourceId == Id).Select(a => a.file_stream).FirstOrDefault();
            return ArrayOfBytes;
            
        }

    }
}
