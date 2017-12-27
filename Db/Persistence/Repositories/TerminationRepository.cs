using Interface.Core.Repositories;
using Model.Domain;
using System;
using System.Linq;
using System.Data.Entity;
using Model.ViewModel.Personnel;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;

namespace Db.Persistence.Repositories
{
    class TerminationRepository : Repository<Termination>, ITerminationRpository
    {

        public TerminationRepository(DbContext context) : base(context)
        {

        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public void Add(TermDuration termDuration)
        {
            context.TermDurations.Add(termDuration);
        }
        public void Attach(TermDuration termDuration)
        {
            context.TermDurations.Attach(termDuration);
        }
        public DbEntityEntry<TermDuration> Entry(TermDuration termDuration)
        {
            return Context.Entry(termDuration);
        }
        public void Remove(TermDuration termDuration)
        {
            if (Context.Entry(termDuration).State == EntityState.Detached)
            {
                context.TermDurations.Attach(termDuration);
            }
            context.TermDurations.Remove(termDuration);
        }


        public IQueryable<TermDurationViewModel> ReadTermDur(int companyId, int TermSysCode)
        {
            var res = from R in context.TermDurations
                      where R.CompanyId == companyId && R.TermSysCode == TermSysCode
                      select new TermDurationViewModel
                      {
                          Id=R.Id,
                          TermSysCode=R.TermSysCode,
                          CompanyId=R.CompanyId,
                          FirstPeriod=R.FirstPeriod,
                          Percent1=R.Percent1,
                          Percent2=R.Percent2,
                          WorkDuration=R.WorkDuration
                      };
            return res;
        }



        public IQueryable<TerminationGridViewModel> ReadTermRequests(int companyId, string culture)
        {
            var requests= from tr in context.Terminations
                    where tr.CompanyId == companyId
                    join wft in context.WF_TRANS on new { p1 = "Termination", p2 = tr.CompanyId, p3 = tr.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                    from wft in g.DefaultIfEmpty()
                    join ap in context.People on wft.AuthEmp equals ap.Id into g1
                    from ap in g1.DefaultIfEmpty()
                    join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                    from apos in g2.DefaultIfEmpty()
                    join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                    join role in context.Roles on wft.RoleId equals role.Id into g4
                    from role in g4.DefaultIfEmpty()
                    from dep in g3.DefaultIfEmpty()
                   select new TerminationGridViewModel
                   {
                       Id = tr.Id,
                       EmpId = tr.EmpId,
                       CompanyId = tr.CompanyId,
                       AssignStatus = HrContext.GetLookUpUserCode("Assignment", tr.AssignStatus, culture),
                       RequestDate = tr.RequestDate,
                       Employee = HrContext.TrlsName(tr.Employee.Title + " " + tr.Employee.FirstName + " " + tr.Employee.Familyname, culture),
                       JoinedDate = tr.ServStartDate,
                       PersonType = HrContext.GetLookUpUserCode("PersonType", tr.PersonType, culture),
                       BonusInMonths = tr.BonusInMonths,
                       ServYear = tr.ServYear,
                       ApprovalStatus = tr.ApprovalStatus,
                       TermReason = HrContext.GetLookUpUserCode("Termination", tr.TermReason, culture),
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
                       HasImage = tr.Employee.HasImage
                   };
            return requests;
        }
        public IQueryable<TerminationGridViewModel> ReadTermFollowUp(int companyId, string culture)
        {
            var followUp = from tr in context.Terminations
                           where tr.CompanyId == companyId && tr.ApprovalStatus < 6 && tr.ApprovalStatus > 1
                           join wft in context.WF_TRANS on new { p1 = "Termination", p2 = tr.CompanyId, p3 = tr.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                           from wft in g.DefaultIfEmpty()
                           join ap in context.People on wft.AuthEmp equals ap.Id into g1
                           from ap in g1.DefaultIfEmpty()
                           join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                           from apos in g2.DefaultIfEmpty()
                           join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                           join role in context.Roles on wft.RoleId equals role.Id into g4
                           from role in g4.DefaultIfEmpty()
                           from dep in g3.DefaultIfEmpty()
                           select new TerminationGridViewModel
                           {
                               Id = tr.Id,
                               RequestDate = tr.RequestDate,
                               ApprovalStatus = tr.ApprovalStatus,
                               EmpId = tr.EmpId,
                               Employee = HrContext.TrlsName(tr.Employee.Title + " " + tr.Employee.FirstName + " " + tr.Employee.Familyname, culture),
                               PlanedDate = tr.PlanedDate,
                               TermReason = HrContext.GetLookUpUserCode("Termination", tr.TermReason, culture),
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
                               HasImage = tr.Employee.HasImage
                           };
            return followUp;
        }

        public MobileTerminationFormViewModel ReadEmpTermination(int EmpId, string culture, int companyId)
        {
            var query = from ter in context.Terminations
                        join a in context.Assignments on ter.EmpId equals a.EmpId
                        where ter.CompanyId == companyId && ter.EmpId == EmpId && ter.RequestDate >= a.AssignDate && ter.RequestDate <= a.EndDate
                        orderby ter.RequestDate descending
                        select new MobileTerminationFormViewModel
                        {
                            ActualDate=ter.ActualDate,
                            ApprovalStatus=ter.ApprovalStatus,
                            AssignStatus=ter.AssignStatus,
                            BonusInMonths=ter.BonusInMonths,
                            CancelDesc=ter.CancelDesc,
                            CancelReason=ter.CancelReason,
                            EmpId=ter.EmpId,
                            Employee= HrContext.TrlsName(ter.Employee.Title + " " + ter.Employee.FirstName + " " + ter.Employee.Familyname, culture),
                            Id=ter.Id,
                            Job= HrContext.TrlsName(a.Job.Name , culture),
                            DepartmentId=a.DepartmentId,
                            Department= HrContext.TrlsName(a.Department.Name, culture),
                            TermReason=ter.TermReason,
                            Terminated=ter.Terminated,     
                            LastAccDate=ter.LastAccDate,
                            LastAdjustDate=ter.LastAdjustDate,
                            ServYear=ter.ServYear,
                            ServStartDate=ter.ServStartDate,
                            RequestDate=ter.RequestDate,
                            RejectReason=ter.RejectReason,
                            PersonType=ter.PersonType,
                            RejectDesc=ter.RejectDesc,
                            ReasonDesc=ter.ReasonDesc,
                            PlanedDate=ter.PlanedDate,               
                        };
            return query.FirstOrDefault();
            
        }
        public Dictionary<string, string> ReadMailEmpTerm(string Language, int Id)
        {
            DateTime Today = DateTime.Today.Date;

            var query = (from r in context.Terminations
                         where r.Id == Id
                         join a in context.Assignments on r.EmpId equals a.EmpId into g
                         from a in g.Where(x => x.CompanyId == r.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                         join m in context.People on a.ManagerId equals m.Id into g1
                         from m in g1.DefaultIfEmpty()
                         select new
                         {
                             EmployeeName = HrContext.TrlsName(r.Employee.Title + " " + r.Employee.FirstName + " " + r.Employee.Familyname, Language),
                             Job = HrContext.TrlsName(a.Job.Name, Language),
                             PlannedEndDate = r.PlanedDate.Value.ToString(),
                             RequestDate = r.RequestDate.ToString(),
                             Department = HrContext.TrlsName(a.Department.Name, Language),
                             DirectManager =  HrContext.TrlsName(m.Title + " " + m.FirstName + " " + m.Familyname, Language),
                         }).FirstOrDefault();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (query != null)
            {
                var ObjProps = query.GetType().GetProperties();
                for (int i = 0; i < ObjProps.Length; i++)
                {
                    var p = ObjProps[i].GetValue(query);
                    dic.Add(ObjProps[i].Name, p != null ? p.ToString() : " ");
                }
            }
            return dic;
        }
        
        public TerminationFormViewModel ReadTermination(int id,string culture)
        {
            if (id == 0)
                return new TerminationFormViewModel {
                    ServStartDate = DateTime.Now,
                    PlanedDate = DateTime.Now
                };

            return (from r in context.Terminations
                    where r.Id == id
                    select new TerminationFormViewModel
                    {
                        Id = r.Id,
                        ActualDate = r.ActualDate,
                        JoinedDate = r.ServStartDate,
                        ApprovalStatus = r.ApprovalStatus,
                        AssignStatus = r.AssignStatus,
                        EmpId = r.EmpId,
                        ServStartDate = r.ServStartDate,
                        Employee = HrContext.TrlsName(r.Employee.Title + " " + r.Employee.FirstName + " " + r.Employee.Familyname, culture),
                        LastAccDate = r.LastAccDate,
                        LastAdjustDate = r.LastAdjustDate,
                        PersonType = r.PersonType,
                        PlanedDate = r.PlanedDate == null ? DateTime.Now : r.PlanedDate,
                        BonusInMonths = r.BonusInMonths,
                        ServYear = r.ServYear,
                        TermReason = r.TermReason,
                        Terminated = r.Terminated,
                        CancelDesc = r.CancelDesc,
                        CancelReason = r.CancelReason,
                        Attachments = HrContext.GetAttachments("TermRequestForm", r.Id),
                        ReasonDesc = r.ReasonDesc,
                        RejectDesc = r.RejectDesc,
                        RejectReason = r.RejectReason,
                        RequestDate = r.RequestDate,


                    }).FirstOrDefault();
        }

        public IQueryable<TerminationGridViewModel> ReadTermsApproved(int companyId,string culture)
        {
            var approved = from tr in context.Terminations
                           where tr.CompanyId == companyId && tr.ApprovalStatus == 6 && tr.Terminated == false
                           select new TerminationGridViewModel
                           {
                               Id = tr.Id,
                               EmpId = tr.EmpId,
                               AssignStatus = HrContext.GetLookUpUserCode("Assignment", tr.AssignStatus, culture),
                               RequestDate = tr.RequestDate,
                               Employee = HrContext.TrlsName(tr.Employee.Title + " " + tr.Employee.FirstName + " " + tr.Employee.Familyname, culture),
                               JoinedDate = tr.ServStartDate,
                               PersonType = HrContext.GetLookUpUserCode("PersonType", tr.PersonType, culture),
                               BonusInMonths = tr.BonusInMonths,
                               ServYear = tr.ServYear,
                               TermReason = HrContext.GetLookUpUserCode("Termination", tr.TermReason, culture),
                               HasImage = tr.Employee.HasImage
                           };

            return approved;
        }

        public void DeleteRequest(int Id, int SourceId,string culture)
        {
            Termination request = context.Terminations.Find(Id);
            var wf = context.WF_TRANS.Where(t => t.Source == "Termination" && t.SourceId == SourceId && t.DocumentId == Id).FirstOrDefault();

            if (request.ApprovalStatus == 1)
                context.Terminations.Remove(request);
        }
    }
}
