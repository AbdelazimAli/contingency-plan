using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel.Personnel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Db.Persistence.Repositories
{
    public class ComplaintRepository :Repository<ComplainRequest>, IComplaintRepository
    {
        public ComplaintRepository(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        public IEnumerable<ComplaintIndexViewModel> GetComplaintRequest(int companyId,string culture)
        {
            var result = (from c in context.ComplainRequests
                         where c.CompanyId == companyId
                         join p in context.People on c.EmpId equals p.Id
                         join wft in context.WF_TRANS on new { p1 = "Complaint" + c.Against, p2 = c.CompanyId, p3 = c.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                         from wft in g.DefaultIfEmpty()
                         join ap in context.People on wft.AuthEmp equals ap.Id into g1
                         from ap in g1.DefaultIfEmpty()
                         join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                         from apos in g2.DefaultIfEmpty()
                         join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                         from dep in g3.DefaultIfEmpty()
                         join role in context.Roles on wft.RoleId equals role.Id into g4
                         from role in g4.DefaultIfEmpty()
                         select new ComplaintIndexViewModel
                         {
                            Id = c.Id,
                            Against = c.Against,
                            EmpId = c.EmpId,
                            RequestDate = c.RequestDate,
                            Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                            ComplainType = c.ComplainType,
                            Description = c.Description ,
                            CompanyId = companyId, 
                            ApprovalStatus = c.ApprovalStatus,
                            RoleId = wft.RoleId.ToString(),
                            DeptId = wft.DeptId,
                            PositionId = wft.PositionId,
                            AuthBranch = wft.AuthBranch,
                            AuthDept = wft.AuthDept,
                            AuthDeptName = HrContext.TrlsName(dep.Name, culture),
                            AuthEmp = wft.AuthEmp,
                            AuthEmpName = HrContext.TrlsName(ap.Title + " " + ap.FirstName + " " + ap.Familyname, culture),
                            AuthPosition = wft.AuthPosition,
                             AuthPosName = role == null ? HrContext.TrlsName(apos.Name, culture) : role.Name,
                             BranchId = wft.BranchId,
                            SectorId = wft.SectorId,
                            AginstName="",
                            ApproveName="",
                            ComplaintTypeName="",
                         }).ToList();

            return result;
        }
        public ComplaintRequestViewModel ReadComplaint(int id)
        {
            var res = (from c in context.ComplainRequests
                       where c.Id == id
                       select new ComplaintRequestViewModel
                       {
                           Id = c.Id,
                           Against = c.Against,
                           ApprovalStatus = c.ApprovalStatus,
                           ComplainType = c.ComplainType,
                           Description=c.Description,
                           EmpId = c.EmpId,
                           Attachments = HrContext.GetAttachments("ComplainRequests", c.Id),
                           CreatedTime = c.CreatedTime,
                           ModifiedTime = c.ModifiedTime,
                           ModifiedUser = c.ModifiedUser,
                           CreatedUser = c.CreatedUser
                       }).FirstOrDefault();
            return res;
        }

        #region FolowUp

        public IQueryable<ComplaintIndexViewModel> GetComplaintReqFollowUp(int companyId, string culture)
        { 
            var query = from c in context.ComplainRequests
                        where c.CompanyId == companyId && c.ApprovalStatus < 6 && c.ApprovalStatus > 1 
                        join p in context.People on c.EmpId equals p.Id
                        join wft in context.WF_TRANS on new { p1 = "Complaint" + c.Against, p2 = c.CompanyId, p3 = c.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                        from wft in g.DefaultIfEmpty()
                        join ap in context.People on wft.AuthEmp equals ap.Id into g1
                        from ap in g1.DefaultIfEmpty()
                        join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                        from apos in g2.DefaultIfEmpty()
                        join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                        from dep in g3.DefaultIfEmpty()
                        join role in context.Roles on wft.RoleId equals role.Id into g4
                        from role in g4.DefaultIfEmpty()
                        select new ComplaintIndexViewModel
                        {
                            Id = c.Id,
                            Against = c.Against,
                            EmpId = c.EmpId,
                            RequestDate = c.RequestDate,
                            Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                            ComplainType = c.ComplainType,
                            Description = c.Description,
                            CompanyId = c.CompanyId,
                            ApprovalStatus = c.ApprovalStatus,
                            HasImage = p.HasImage,
                            RoleId = wft.RoleId.ToString(),
                            DeptId = wft.DeptId,
                            PositionId = wft.PositionId,
                            AuthBranch = wft.AuthBranch,
                            AuthDept = wft.AuthDept,
                            AuthDeptName = HrContext.TrlsName(dep.Name, culture),
                            AuthEmp = wft.AuthEmp,
                            AuthEmpName = HrContext.TrlsName(ap.Title + " " + ap.FirstName + " " + ap.Familyname, culture),
                            AuthPosition = wft.AuthPosition,
                            AuthPosName = role == null ? HrContext.TrlsName(apos.Name, culture) : role.Name,
                            BranchId = wft.BranchId,
                            SectorId = wft.SectorId
                        };

            return query;
        }
        public ComplaintRequestViewModel GetRequest(int requestId, string culture)
        {
            ComplaintRequestViewModel Request = (from req in context.ComplainRequests
                                         where req.Id == requestId
                                         join p in context.People on req.EmpId equals p.Id
                                         select new ComplaintRequestViewModel()
                                         {
                                             Id = req.Id,
                                             RequestDate = req.RequestDate,
                                             EmpId = req.EmpId,
                                             Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),                         
                                             ApprovalStatus = req.ApprovalStatus,                                         
                                             RejectReason = req.RejectReason,
                                             RejectDesc = req.RejectDesc,
                                             CancelReason = req.CancelReason,
                                             CancelDesc = req.CancelDesc,
                                             Attachments = HrContext.GetAttachments("ComplaintRequest", req.Id),
                                             CreatedTime = req.CreatedTime,
                                             CreatedUser = req.CreatedUser,
                                             ModifiedTime = req.ModifiedTime,
                                             ModifiedUser = req.ModifiedUser,
                                             ComplainType = req.ComplainType,
                                             Description =  req.Description,
                                             Against = req.Against
                                         }).FirstOrDefault();
                      
            return Request;
        }

        #endregion


    }
}
