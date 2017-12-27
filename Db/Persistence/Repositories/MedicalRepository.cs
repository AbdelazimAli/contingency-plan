
using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Persistence.Repositories
{
    public class MedicalRepository :Repository<BenefitRequest>,IMedicalRepository
    {
        public MedicalRepository(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        #region Medical Request
        //Get Medical Request (Index Grid)
        public IQueryable<MedicalIndexViewModel> GetMedicalRequest(int companyId, string culture)
        {
            var result = from c in context.BenefitRequests
                         where c.CompanyId == companyId
                         join n in context.BenefitServs on c.ServiceId equals n.Id
                         join prov in context.Providers on c.ProviderId equals prov.Id
                         join p in context.People on c.EmpId equals p.Id
                         join wft in context.WF_TRANS on new { p1 = "Medical", p2 = c.CompanyId, p3 = c.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                         from wft in g.DefaultIfEmpty()
                         join ap in context.People on wft.AuthEmp equals ap.Id into g1
                         from ap in g1.DefaultIfEmpty()
                         join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                         from apos in g2.DefaultIfEmpty()
                         join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                         from dep in g3.DefaultIfEmpty()
                         join role in context.Roles on wft.RoleId equals role.Id into g4
                         from role in g4.DefaultIfEmpty()
                         select new MedicalIndexViewModel
                         {
                             Id = c.Id,
                             EmpId = c.EmpId,
                             RequestDate = c.RequestDate,
                             ApprovalStatusName= "",
                             Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
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
                             Service = n.Name,
                             Provider = prov.Name                             
                         };
            return result;
        }

        //Read Medical Request
        public MedicalRequestViewModel ReadMedical(int id)
        {
            var res = (from c in context.BenefitRequests
                       where c.Id == id
                       select new MedicalRequestViewModel
                       {
                           Id = c.Id,
                           ApprovalStatus = c.ApprovalStatus,
                           Description = c.Description,
                           EmpId = c.EmpId,
                           Attachments = HrContext.GetAttachments("MedicalRequests", c.Id),
                           CreatedTime = c.CreatedTime,
                           CompanyCost =c.CompanyCost,
                           EmpCost =c.EmpCost,
                           CurrRate = c.CurrRate,
                           RequestDate = c.RequestDate,
                           ServiceId = c.ServiceId,
                           ServCost = c.ServCost,
                           ProviderId = c.ProviderId,
                           BenefitId =c.BenefitId,
                           Curr = c.Curr,
                           BenefitPlanId = c.BenefitPlanId,                        
                           BeneficiaryId =c.BeneficiaryId,
                           ModifiedTime = c.ModifiedTime,
                           ModifiedUser = c.ModifiedUser,
                           CreatedUser = c.CreatedUser,
                       }).FirstOrDefault();
            return res;
        }
        //Get Beneficiary
        public IEnumerable<FormList> GetBeneficiary(int id)
        {
            var res = (from c in context.EmpRelative
                       where c.EmpId == id
                       select new FormList
                       {
                           id = c.Id,
                           name = c.Name
                       }).ToList();
            return res;
        }
        
        public IEnumerable<FormList> FillDDLBenefit(string Culture)
        {
            var ListOfBenefit = (from b in context.Benefits
                                 select new FormList
                                 {
                                     id = b.Id,
                                     name = HrContext.TrlsName(b.Name,Culture)
                                 }).ToList();
            return ListOfBenefit;
        }

        //Get All service()
        public IEnumerable<FormList> GetAllservice(int EmpId, int BenefitId, int? BeneficiaryId)
        {
            var today = DateTime.Today;
            var benfeciary = BeneficiaryId == 0 ? null : BeneficiaryId;
            var list = (from eb in context.EmpBenefits
                        where eb.EmpId == EmpId && eb.BeneficiaryId == benfeciary
                        join gs in context.BenefitServPlans on eb.BenefitPlanId equals gs.BenefitPlanId
                        join s in context.BenefitServs on gs.BenefitServ.Id equals s.ParentId
                        where s.Parent.BenefitId == BenefitId
                        select new FormList
                        {
                            id = s.Id,
                            name = s.Name,
                            isActive = s.StartDate <= today && (s.EndDate == null || s.EndDate >= today),
                        }).Distinct();
            return list.ToList();
        }

        // Get All Providers
        public IEnumerable<FormList> GetAllProvider()
        {
            var res = (from c in context.Providers
                       select new FormList
                       {
                           id = c.Id,
                           name = c.Name
                       }).ToList();
            return res;
        }
        //Get AllBeneficiary
        public IEnumerable<FormList> GetAllBeneficiary()
        {
            var res = (from c in context.EmpRelative
                       select new FormList
                       {
                           id = c.Id,
                           name = c.Name
                       }).ToList();
            return res;
        }

        public SetCurrencyInfoViewModel SetCurrency(int serviceId)
        {
            var test = (from s in context.BenefitServs
                        where s.Id == serviceId
                        join p in context.BenefitServs on s.ParentId equals p.Id
                        select new SetCurrencyInfoViewModel
                        {
                            Cost = s.Cost,
                            Curr = s.Curr,
                            EmpPercent =p.EmpPercent,
                            CompPercent = p.CompPercent,
                            rate = context.Currencies.Where(a => a.Code == s.Curr).Select(a => a.MidRate).FirstOrDefault(),
                            ParentId= p.Id                             
                        }).FirstOrDefault();

            
            return test;
        }

        //GetBenefitPlanId
        public int GetBenefitPlanId(int serviceId, int empId, int? beneficiaryId)
        {
            var today = DateTime.Today;
            var beneficiary = beneficiaryId == 0 ? null : beneficiaryId;

            var PlannId = (from eb in context.EmpBenefits
                           where eb.EmpId == empId && eb.BeneficiaryId == beneficiary
                           join bs in context.BenefitServPlans on new { p1 = eb.BenefitPlanId, p2 = serviceId }  equals new { p1 = bs.BenefitPlanId, p2 = bs.BenefitServId  }
                           select eb.BenefitPlanId).FirstOrDefault();

            return PlannId;
        }

        public int GetMedicalPeriodId(int planId, DateTime issusDate)
        {
            var periodid = (from bp in context.BenefitPlans
                            where bp.Id == planId
                           join p in context.Periods on bp.Benefit.CalenderId equals p.CalendarId
                           join s in context.SubPeriods on p.Id equals s.PeriodId
                           where s.StartDate <= issusDate && s.EndDate >= issusDate
                           select s.Id).FirstOrDefault();

            return periodid;
        }

        //Get BenfitId
        public List<int> GetPeriodId(int serviceId, int empId, int? beneficiaryId)
        {
            var parentId = context.BenefitServs.Where(a => a.Id == serviceId).Select(a => a.ParentId).FirstOrDefault();
            var today = DateTime.Today;
            var beneficiary = beneficiaryId == 0 ? null : beneficiaryId;

            var BenefitId = (from eb in context.EmpBenefits
                           where eb.EmpId == empId && eb.BeneficiaryId == beneficiary && eb.StartDate <= today && (eb.EndDate == null || eb.EndDate >= today)
                           join gs in context.BenefitServPlans on eb.BenefitPlanId equals gs.BenefitPlanId
                           join s in context.BenefitServs on gs.BenefitServ.Id equals parentId
                           select eb.BenefitId).FirstOrDefault();

            var periodId = (from b in context.Benefits
                            where b.Id == BenefitId
                            join c in context.PeriodNames on b.CalenderId equals c.Id
                            join p in context.Periods on c.Id equals p.CalendarId
                            select p.Id).ToList();

            return periodId;
        }
        #endregion


        #region Medical Follow Up
        public IQueryable<MedicalIndexViewModel> GetMedicalReqFollowUp(int companyId, string culture)
        {
            var query = from c in context.BenefitRequests
                        where c.CompanyId == companyId && c.ApprovalStatus < 6 && c.ApprovalStatus >1
                        join n in context.BenefitServs on c.ServiceId equals n.Id
                        join prov in context.Providers on c.ProviderId equals prov.Id
                        join p in context.People on c.EmpId equals p.Id
                        join wft in context.WF_TRANS on new { p1 = "Medical", p2 = c.CompanyId, p3 = c.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                        from wft in g.DefaultIfEmpty()
                        join ap in context.People on wft.AuthEmp equals ap.Id into g1
                        from ap in g1.DefaultIfEmpty()
                        join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                        from apos in g2.DefaultIfEmpty()
                        join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                        from dep in g3.DefaultIfEmpty()
                        join role in context.Roles on wft.RoleId equals role.Id into g4
                        from role in g4.DefaultIfEmpty()
                        select new MedicalIndexViewModel
                        {
                            Id = c.Id,
                            EmpId = c.EmpId,
                            RequestDate = c.RequestDate,
                            Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
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
                            SectorId = wft.SectorId,
                            Service = n.Name,
                            Provider = prov.Name
                        };

            return query;
        }
        public BenefitRequestFollowUp GetRequest(int requestId, string culture)
        {
            BenefitRequestFollowUp Request = (from req in context.BenefitRequests
                                                 where req.Id == requestId
                                                 join p in context.People on req.EmpId equals p.Id
                                                 select new BenefitRequestFollowUp()
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
                                                     Attachments = HrContext.GetAttachments("MedicalRequests", req.Id),
                                                     CreatedTime = req.CreatedTime,
                                                     CreatedUser = req.CreatedUser,
                                                     ModifiedTime = req.ModifiedTime,
                                                     ModifiedUser = req.ModifiedUser,
                                                     Description = req.Description,
                                                     CompanyCost =req.CompanyCost,
                                                     Curr =req.Curr,
                                                     EmpCost =req.EmpCost,
                                                     CurrRate =req.CurrRate,
                                                     ServCost =req.ServCost, 
                                                     ServiceId =req.ServiceId,
                                                     ProviderId =req.ProviderId, 
                                                     BeneficiaryId =req.BeneficiaryId,
                                                     ExpiryDate =req.ExpiryDate,
                                                     ServEndDate =req.ServEndDate,
                                                     ServStartDate =req.ServStartDate,
                                                     IssueDate =req.IssueDate,
                                                     BenefitPlanId =req.BenefitPlanId,
                                                     BenefitId =req.BenefitId,
                                                     PaidBy = req.PaidBy                                                                                                                                                                                                            
                                                 }).FirstOrDefault();

            return Request;
        }
        #endregion

        #region Accepted Medical Request
        public IQueryable<MedicalIndexViewModel> GetApprovedMedicalReq(int companyId, string culture)
        {
            return from l in context.BenefitRequests
                   where l.CompanyId == companyId && l.ApprovalStatus == 6
                   join n in context.BenefitServs on l.ServiceId equals n.Id
                   join prov in context.Providers on l.ProviderId equals prov.Id
                   join p in context.People on l.EmpId equals p.Id
                   join wft in context.WF_TRANS on new { p1 = "Medical", p2 = l.CompanyId, p3 = l.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                   from wft in g.DefaultIfEmpty()
                   join ap in context.People on wft.AuthEmp equals ap.Id into g1
                   from ap in g1.DefaultIfEmpty()
                   join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                   from apos in g2.DefaultIfEmpty()
                   join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                   from dep in g3.DefaultIfEmpty()
                   join role in context.Roles on wft.RoleId equals role.Id into g4
                   from role in g4.DefaultIfEmpty()
                   select new MedicalIndexViewModel
                   {
                       Id = l.Id,
                       RequestDate = l.RequestDate,
                       Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                       ApprovalStatus = l.ApprovalStatus,
                       CompanyId = l.CompanyId,
                       EmpId = l.EmpId,
                       RoleId = wft.RoleId.ToString(),
                       DeptId = wft.DeptId,
                       PositionId = wft.PositionId,
                       AuthBranch = wft.AuthBranch,
                       AuthDept = wft.AuthDept,
                       AuthEmp = wft.AuthEmp,
                       AuthPosition = wft.AuthPosition,
                       BranchId = wft.BranchId,
                       SectorId = wft.SectorId,
                       HasImage = p.HasImage,
                       Service = n.Name,
                       Provider = prov.Name
                   };
        }
        #endregion


        // ReadMedicalService
        public IEnumerable<EmpServiceViewModel> ReadMedicalService(int empId)
        {
            var listOfPlans = (from mr in context.BenefitRequests
                               where mr.EmpId == empId && mr.ApprovalStatus == 6
                               join bp in context.BenefitPlans on mr.BenefitPlanId equals bp.Id
                               join b in context.Benefits on bp.BenefitId equals b.Id
                               join p in context.Periods on mr.SubPeriodId equals p.Id
                               group mr by new { p.Name, mr.BeneficiaryId.Value, bp.PlanName, bp.CoverAmount ,mr.EmpId ,mr.SubPeriodId ,b.PlanLimit} into g
                               select new EmpServiceViewModel
                               {
                                   BeneficiaryId = g.Key.Value,
                                   Period =g.Key.Name,
                                   PlanName =g.Key.PlanName,
                                   EmpId =g.Key.EmpId,
                                   PeriodId =g.Key.SubPeriodId,
                                   PlanLimit =g.Key.PlanLimit,
                                   TotalCost =g.Key.PlanLimit == 1 ? g.Sum(a=>a.CompanyCost):(g.Key.PlanLimit==2 ? g.Sum(a=>a.EmpCost): g.Sum(a=>a.ServCost)),
                                   TotalCompCost =g.Sum(a=>a.CompanyCost),
                                   TotalEmpCost =g.Sum(a=>a.EmpCost),
                                   Balance =  g.Key.CoverAmount,
                                   Id = g.Sum(a=>a.Id)
                               });


            return listOfPlans.ToList();
                              
           
        }
       // ReadBenfitService
        public IEnumerable<BenfitServiceReqViewModel> ReadBenfitService(int PeriodId, int BenfitId, int EmpId)
        {
            //Service belong to Employee
            if(BenfitId ==0)
            {

                var q = (from m in context.BenefitRequests
                         where m.SubPeriodId == PeriodId && m.EmpId == EmpId && m.BeneficiaryId == null && m.ApprovalStatus == 6
                         join S in context.BenefitServs on m.ServiceId equals S.Id
                         select new BenfitServiceReqViewModel
                         {
                             ServiceName = S.Name,
                             CompanyCost = m.CompanyCost,
                             ProviderId = m.ProviderId,
                             EmpCost = m.EmpCost,
                             RequestDate = m.RequestDate,
                             ExpiryDate = m.ExpiryDate,
                             ServEndDate = m.ServEndDate,
                             IssueDate = m.IssueDate,
                             ServCost = m.ServCost,
                             ServStartDate = m.ServStartDate
                         }).ToList();
                return q;               

            }else
            //Employee Relative Beneficiary of service
            {
                var Ben = (from m in context.BenefitRequests
                         where m.SubPeriodId == PeriodId && m.EmpId == EmpId && m.BeneficiaryId == BenfitId && m.ApprovalStatus == 6
                         join S in context.BenefitServs on m.ServiceId equals S.Id
                         select new BenfitServiceReqViewModel
                         {
                             ServiceName = S.Name,
                             CompanyCost = m.CompanyCost,
                             ProviderId = m.ProviderId,
                             EmpCost = m.EmpCost,
                             RequestDate = m.RequestDate,
                             ExpiryDate = m.ExpiryDate,
                             ServEndDate = m.ServEndDate,
                             IssueDate = m.IssueDate,
                             ServCost = m.ServCost,
                             ServStartDate = m.ServStartDate
                         }).ToList();
                return Ben;

            }

          
        }

    }
}
