using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Persistence.Repositories
{
    class LeaveRepository : Repository<LeaveRequest>, ILeaveRepository
    {
        public LeaveRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        #region LeaveAction
        public IQueryable<LeaveActionViewModel> GetLeaveAction(string culture, int CompanyId)
        {
            var result = from PT in context.LeaveAdjusts
                         where PT.CompanyId == CompanyId
                         join c in context.LeaveTypes on PT.TypeId equals c.Id
                         join p in context.People on PT.EmpId equals p.Id
                         join r in context.Periods on PT.PeriodId equals r.Id
                         select new LeaveActionViewModel
                         {
                             Id = PT.Id,
                             EmpId = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             PeriodId = r.Name,
                             TypeId = c.Name,
                             NofDays = PT.NofDays,
                             EmpStatus = HrContext.GetEmpStatus(p.Id),
                             Image = HrContext.GetDoc("EmployeePic", p.Id),
                             Gender = p.Gender,
                             empId = p.Id,
                             TransType = PT.TransType
                         };

            return result;
        }

        public LeaveAdjust GetLeaveAction(int? id)
        {
            return context.LeaveAdjusts.Find(id);
        }
        public void Add(LeaveAdjust leaveAction)
        {
            context.LeaveAdjusts.Add(leaveAction);
        }
        public void Attach(LeaveAdjust leaveAction)
        {
            context.LeaveAdjusts.Attach(leaveAction);
        }
        public DbEntityEntry<LeaveAdjust> Entry(LeaveAdjust leaveAction)
        {
            return Context.Entry(leaveAction);
        }
        public void Remove(LeaveAdjust leaveAction)
        {
            if (Context.Entry(leaveAction).State == EntityState.Detached)
            {
                context.LeaveAdjusts.Attach(leaveAction);
            }
            context.LeaveAdjusts.Remove(leaveAction);
        }

        public LeaveActionFormViewModel ReadleaveAction(int Id)
        {
            var result = (from obj in context.LeaveAdjusts
                          where obj.Id == Id
                          select new LeaveActionFormViewModel()
                          {
                              Id = obj.Id,
                              ActionDate = obj.AdjustDate,
                              EmpId = obj.EmpId,
                              NofDays = obj.NofDays,
                              PeriodId = obj.PeriodId,
                              TypeId = obj.TypeId,
                              TransType = obj.TransType,
                              CreatedTime = obj.CreatedTime,
                              CreatedUser = obj.CreatedUser,
                              Attachments = HrContext.GetAttachments("LeaveAction", obj.Id),
                          }).FirstOrDefault();

            return result;
        }



        #endregion

        public IQueryable<WorkFlowGridViewModel> ReadWorkFlow(string source, int sourceId, int requestId, int companyId, string culture)
        {
            var query = from wf in context.RequestWf
                        where wf.Source == source && wf.SourceId == sourceId
                        join t in context.WfTrans on wf.Id equals t.WFlowId into g
                        from t in g.Where(a => a.DocumentId == requestId).DefaultIfEmpty()
                        join l in context.LeaveTypes on wf.SourceId equals l.Id into g1
                        from l in g1.DefaultIfEmpty()
                        join e in context.People on t.EmpId equals e.Id into g2
                        from e in g2.DefaultIfEmpty()
                        join p in context.Positions on t.PositionId equals p.Id into g3
                        from p in g3.DefaultIfEmpty()
                        join r in context.Roles on t.RoleId equals r.Id into g4
                        from r in g4.DefaultIfEmpty()
                        join d in context.CompanyStructures on t.DeptId equals d.Id into g5
                        from d in g5.DefaultIfEmpty()
                        select new WorkFlowGridViewModel
                        {
                            Id = t.Id,
                            Source = wf.Source,
                            SourceId = wf.SourceId,
                            DocumentId = t.DocumentId,
                            WorkFlowId = wf.Id,
                            ApprovalStatus = t.ApprovalStatus,
                            Role = HrContext.TrlsName(r.Name, culture),
                            Employee = HrContext.TrlsName(e.Title + " " + e.FirstName + " " + e.Familyname, culture),
                            Position = HrContext.TrlsName(p.Name, culture),
                            Department = HrContext.TrlsName(d.Name, culture),


                        };

            return query;
        }

        public IQueryable GetAccrualLeaveTypes(int companyId, string culture)
        {
            return context.LeaveTypes.Where(t =>
                        t.HasAccrualPlan && ((t.IsLocal && t.CompanyId == companyId) || t.IsLocal == false)
                        && (t.StartDate <= DateTime.Today && (t.EndDate == null || t.EndDate >= DateTime.Today)))
                        .Select(t => new { id = t.Id, name = HrContext.TrlsName(t.Name, culture) });
        }
        public IQueryable GetAcuralGridLeaveTypes(int companyId, string culture)
        {
            return context.LeaveTypes.Where(t =>
                        t.HasAccrualPlan && ((t.IsLocal && t.CompanyId == companyId) || t.IsLocal == false)
                        && (t.StartDate <= DateTime.Today && (t.EndDate == null || t.EndDate >= DateTime.Today)))
                        .Select(t => new { value = t.Id, text = HrContext.TrlsName(t.Name, culture) });
        }
        public IEnumerable GetAcuralRestLeaveTypes(int companyId, string culture)
        {
            return context.LeaveTypes.Where(t => t.HasAccrualPlan && ((t.IsLocal && t.CompanyId == companyId) || t.IsLocal == false)
                        && (t.StartDate <= DateTime.Today && (t.EndDate == null || t.EndDate >= DateTime.Today)) && t.AbsenceType == 6)
                        .Select(t => new { id = t.Id, name = HrContext.TrlsName(t.Name, culture) }).ToList();
        }
        public IQueryable<PeriodListViewModel> GetOpenedLeavePeriods()
        {
            return (from p in context.Periods
                    where p.Status == 1 
                    join l in context.LeaveTypes on p.CalendarId equals l.CalendarId
                    select new PeriodListViewModel { id = p.Id, name = p.Name, typeId = l.Id });
        }

        public Dictionary<string, string> ReadMailEmpLeave(string Language, int Id)
        {
            DateTime Today = DateTime.Today.Date;
            float balance;

            var query = (from l in context.LeaveRequests
                         where l.Id == Id
                         join e in context.Employements on l.EmpId equals e.EmpId into g1
                         from e in g1.Where(s => s.Status == 1).DefaultIfEmpty()
                         join a in context.Assignments on l.EmpId equals a.EmpId into g
                         from a in g.Where(x => x.CompanyId == l.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                         join re in context.People on l.ReplaceEmpId equals re.Id into g2
                         from re in g2.DefaultIfEmpty()
                         join s in context.Assignments on l.ReplaceEmpId equals s.EmpId into g3
                         from s in g3.Where(x => x.CompanyId == l.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                         select new
                         {
                             EmployeeName = HrContext.TrlsName(l.Person.Title + " " + l.Person.FirstName + " " + l.Person.Familyname, Language),
                             Department = HrContext.TrlsName(a.Department.Name, Language),
                             Job = HrContext.TrlsName(a.Job.Name, Language),
                             EmploymentDate = e.StartDate.ToString(),
                             RequestDate = l.RequestDate,
                             LeaveType = HrContext.TrlsName(l.LeaveType.Name, Language),
                             StartDate = l.StartDate.ToString(),
                             EndDate = l.EndDate.ToString(),
                             Day = l.ReturnDate != null ? l.ReturnDate.Value.ToString() : " ",
                             ReturnDate = l.ReturnDate != null ? l.ReturnDate.Value.ToString() : " ",
                             Notes = l.ReasonDesc,
                             ReplaceEmployeeName = l.ReplaceEmpId != null ? HrContext.TrlsName(re.Title + " " + re.FirstName + " " + re.Familyname, Language) : " ",
                             ReplaceEmployeeJob = l.ReplaceEmpId != null ? HrContext.TrlsName(s.Job.Name, Language) : " ",
                             Balance = "0",
                             DirectManager = a.ManagerId != null ? context.People.Where(m => m.Id == a.ManagerId).Select(f => HrContext.TrlsName(f.Title + " " + f.FirstName + " " + f.Familyname, Language)).FirstOrDefault() : " ",
                             TypeId = l.TypeId,
                             Absence = l.LeaveType.AbsenceType,
                             PeriodId = l.PeriodId,
                             EmpId = l.EmpId,
                             AllowNegBal = l.LeaveType.AllowNegBal,
                             Percantage = l.LeaveType.Percentage,
                             MaxDaysInPeriod = l.LeaveType.MaxDaysInPeriod,
                             LeftBalance = "0",
                             NofDays = l.NofDays.ToString(),
                             RequestDay = "",
                             StartDay = l.StartDate,
                             EndDay = l.EndDate,

                         }).FirstOrDefault();

            balance = GetMaxAllowedDays(query.TypeId, query.Absence, query.PeriodId, query.EmpId, query.AllowNegBal, query.Percantage, query.MaxDaysInPeriod, out balance);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (query != null)
            {
                var ObjProps = query.GetType().GetProperties();
                for (int i = 0; i < ObjProps.Length; i++)
                {

                    var p = ObjProps[i].GetValue(query);
                    if (ObjProps[i].Name == "Balance")
                        p = balance.ToString();
                    else if (ObjProps[i].Name == "LeftBalance")
                        p = ((balance) - float.Parse(query.NofDays)).ToString();
                    else if (ObjProps[i].Name == "RequestDate")
                        p = query.RequestDate.ToString("yyyy-MM-dd");
                    else if (ObjProps[i].Name == "RequestDay")
                        p = MsgUtils.Instance.Trls(Language, query.RequestDate.DayOfWeek.ToString());
                    else if (ObjProps[i].Name == "StartDay")
                        p = MsgUtils.Instance.Trls(Language, query.StartDay.DayOfWeek.ToString());
                    else if (ObjProps[i].Name == "EndDay")
                        p = MsgUtils.Instance.Trls(Language, query.EndDay.DayOfWeek.ToString());
                    else if (ObjProps[i].Name == "Day")
                        p = query.Day != null ? MsgUtils.Instance.Trls(Language, Convert.ToDateTime(query.Day).DayOfWeek.ToString()) : " ";
                    dic.Add(ObjProps[i].Name, p != null ? p.ToString() : " ");
                }
            }
            return dic;
        }

        #region Leave Trans
        public IQueryable<LeaveTransViewModel> GetLeaveTrans(int TypeId, int PeriodId, int EmpId, string culture)
        {
            var query = from t in context.LeaveTrans
                        where t.TypeId == TypeId && t.PeriodId == PeriodId && t.EmpId == EmpId
                        orderby t.TransDate, t.TransType
                        select new LeaveTransViewModel
                        {
                            Id = t.Id,
                            CreditQty = (t.TransFlag == 1 ? t.TransQty : 0),
                            DebitQty = (t.TransFlag == -1 ? t.TransQty : 0),
                            Balance = (t.TransQty * t.TransFlag),
                            Employee = HrContext.TrlsName(t.Employee.Title + " " + t.Employee.FirstName + " " + t.Employee.Familyname, culture),
                            LeaveType = HrContext.TrlsName(t.LeaveType.Name, culture),
                            Period = t.Period.Name,
                            TransDate = t.TransDate,
                            TransType = t.TransType
                        };

            return query;
        }

        public IQueryable<LeaveTransSummary> GetLeaveTransSummary(int YearId, int companyId, string culture)
        {
            //var query = (from t in context.LeaveTrans
            //             where t.CompanyId == companyId && t.Period.YearId == YearId
            //             join a in context.Assignments on t.EmpId equals a.EmpId
            //             where (a.AssignDate <= t.TransDate && a.EndDate >= t.TransDate)
            //             group new { t, a } by new
            //             {
            //                 TypeId = t.TypeId,
            //                 PeriodId = t.PeriodId,
            //                 EmpId = t.EmpId,
            //                 Title = t.Employee.Title,
            //                 FirstName = t.Employee.FirstName,
            //                 Familyname = t.Employee.Familyname,
            //                 LeaveName = t.LeaveType.Name,
            //                 DeptName = a.Department.Name,
            //                 PeriodN = t.Period.Name,
            //                 Code = a.Code,
            //                 DeptId = a.DepartmentId,
            //                 BranchId = a.BranchId,
            //             } into g
            //             orderby new { g.Key.EmpId, g.Key.TypeId }
            //             select new LeaveTransSummary
            //             {
            //                 Balance = g.Sum(x => x.t.TransFlag * x.t.TransQty),
            //                 Employee = HrContext.TrlsName(g.Key.Title + " " + g.Key.FirstName + " " + g.Key.Familyname, culture),
            //                 LeaveType = HrContext.TrlsName(g.Key.LeaveName, culture),
            //                 Dept = HrContext.TrlsName(g.Key.DeptName, culture),
            //                 TypeId = g.Key.TypeId,
            //                 Period = g.Key.PeriodN,
            //                 EmpId = g.Key.EmpId,
            //                 EmpCode = g.Key.Code,
            //                 DeptId = g.Key.DeptId,
            //                 BranchId = g.Key.BranchId,
            //             });

            var query = context.Database.SqlQuery<LeaveTransSummary>("SELECT LeaveTrans.CompanyId, LeaveTrans.PeriodId, LeaveTrans.EmpId, LeaveTrans.EmpId Id, LeaveTrans.TypeId, [Periods].[Name] [Period], dbo.fn_TrlsName(ISNULL(People.Title, '') + ' ' + People.FirstName + ' ' + People.Familyname, '" + culture + "') Employee, dbo.fn_TrlsName(LeaveTypes.[Name], '" + culture + "') LeaveType, Assignments.Code EmpCode, Assignments.DepartmentId DeptId, Assignments.BranchId, dbo.fn_TrlsName(CompanyStructures.[Name], '" + culture + "') Dept, Round(SUM(LeaveTrans.TransFlag * LeaveTrans.TransQty),2) Balance FROM LeaveTrans INNER JOIN LeaveTypes ON LeaveTrans.TypeId = LeaveTypes.Id INNER JOIN Assignments ON (LeaveTrans.EmpId = Assignments.EmpId AND Convert(date, GetDate()) BETWEEN Assignments.AssignDate AND Assignments.EndDate) INNER JOIN CompanyStructures ON Assignments.DepartmentId = CompanyStructures.Id INNER JOIN People ON LeaveTrans.EmpId = People.Id INNER JOIN [Periods] ON LeaveTrans.PeriodId = [Periods].Id WHERE LeaveTrans.CompanyId = " + companyId + " AND [Periods].YearId = " + YearId + " GROUP BY LeaveTrans.CompanyId, LeaveTrans.PeriodId, LeaveTrans.EmpId, LeaveTrans.TypeId, People.Title, People.FirstName, People.Familyname, [Periods].[Name], LeaveTypes.[Name], Assignments.Code, Assignments.DepartmentId, Assignments.BranchId, CompanyStructures.[Name] ORDER BY LeaveTrans.EmpId, LeaveTrans.TypeId").AsQueryable<LeaveTransSummary>();
            return query;
        }

        public string AddAssignOrdersLeaveTrans(AssignOrder assignOrder, string User, string culture)
        {
            var leavetype = context.LeaveTypes.Where(a => a.Id == assignOrder.LeaveTypeId).Select(a => new { a.AbsenceType, a.CalendarId }).FirstOrDefault();
            string error;
            var periodId = GetLeaveRequestPeriod(leavetype.CalendarId, assignOrder.AssignDate, culture, out error);
            if (error != "OK")
                return error;

            var Trans = context.LeaveTrans.Add(new LeaveTrans
            {
                CompanyId = assignOrder.CompanyId,
                EmpId = assignOrder.EmpId,
                TypeId = assignOrder.LeaveTypeId.GetValueOrDefault(),
                PeriodId = periodId,
                TransFlag = 1,
                TransDate = assignOrder.AssignDate,
                TransQty = assignOrder.CalcMethod == 2 ? (assignOrder.Duration == 2 ? 0.25f : assignOrder.Duration == 3 ? 0.5f : 1f) : 0,
                TransType = 3, ///3-Time Compensation  تعويضات الوقت 
                AbsenceType = leavetype.AbsenceType,
                ExpiryDate = assignOrder.ExpiryDate,
                CreatedUser = User,
                CreatedTime = DateTime.Now

            });

            return "";
        }

        public void AddAcceptLeaveTrans(LeaveRequest request, string user)
        {
            var absenceType = context.LeaveTypes.Where(a => a.Id == request.TypeId).Select(a => a.AbsenceType).FirstOrDefault();

            context.LeaveTrans.Add(new LeaveTrans
            {
                PeriodId = request.PeriodId,
                CompanyId = request.CompanyId,
                CreatedTime = DateTime.Now,
                CreatedUser = user,
                EmpId = request.EmpId,
                TransDate = request.ActualStartDate.Value,
                TransFlag = -1,
                TypeId = request.TypeId,
                TransQty = request.ActualNofDays.Value,
                TransType = 11,
                AbsenceType = absenceType
            });
        }

        public void AddBreakLeaveTrans(LeaveRequest request, float diffDays, string user)
        {
            var absenceType = context.LeaveTypes.Where(a => a.Id == request.TypeId).Select(a => a.AbsenceType).FirstOrDefault();

            context.LeaveTrans.Add(new LeaveTrans
            {
                PeriodId = request.PeriodId,
                CompanyId = request.CompanyId,
                CreatedTime = DateTime.Now,
                CreatedUser = user,
                EmpId = request.EmpId,
                TransDate = request.ReturnDate.GetValueOrDefault(),
                TransFlag = 1,
                TypeId = request.TypeId,
                TransQty = diffDays,
                TransType = 22, //Break Leave
                AbsenceType = absenceType
            });
        }

        public string AddCancelLeaveTrans(LeaveRequest request, string user, string culture)
        {
            var period = context.Periods.Find(request.PeriodId);
            if (period == null)
                return MsgUtils.Instance.Trls(culture, "PeriodIsNotDefined");

            if (period.Status == 2)
                return MsgUtils.Instance.Trls(culture, "PeriodIsClosed");

            if (period.EndDate == new DateTime(2999, 1, 1) && request.EndDate.Year < DateTime.Today.Year)
                return MsgUtils.Instance.Trls(culture, "YouCantCancelLeaveInPrevYear");

            //var status = context.SubPeriods.Where(a => a.PeriodId == period.Id && a.StartDate <= request.StartDate && a.EndDate >= request.StartDate)
            //   .Select(a => a.Status).FirstOrDefault();

            //if (status == 2)
            //    return MsgUtils.Instance.Trls(culture, "SubPeriodIsClosed");

            var absenceType = context.LeaveTypes.Where(a => a.Id == request.TypeId).Select(a => a.AbsenceType).FirstOrDefault();

            context.LeaveTrans.Add(new LeaveTrans
            {
                AbsenceType = absenceType,
                PeriodId = request.PeriodId,
                CompanyId = request.CompanyId,
                CreatedTime = DateTime.Now,
                CreatedUser = user,
                EmpId = request.EmpId,
                TransDate = request.ActualStartDate.Value,
                TransFlag = 1,
                TypeId = request.TypeId,
                TransQty = request.NofDays,
                TransType = 21 // Cancel Leave
            });

            return "";
        }

        public IList<FormList> GetLeavePeriods(int LeaveId)
        {
            return (from l in context.LeaveTypes
                    where l.Id == LeaveId
                    join p in context.Periods on l.CalendarId equals p.CalendarId
                    where p.Status == 1
                    select new FormList
                    {
                        id = p.Id,
                        name = p.Name
                    }).ToList();
        }

        public void AddEditLeaveTrans(LeaveRequest request, DateTime startDate)
        {
            int absenceType = context.LeaveTypes.Where(a => a.Id == request.TypeId).Select(a => a.AbsenceType).FirstOrDefault();
            LeaveTrans trans = context.LeaveTrans.Where(t => t.CompanyId == request.CompanyId && t.EmpId == request.EmpId
                && t.AbsenceType == absenceType && t.TypeId == request.TypeId && t.PeriodId == request.PeriodId && t.TransFlag == -1
                && DbFunctions.TruncateTime(t.TransDate) == startDate).FirstOrDefault();

            if (trans != null)
            {
                trans.TransDate = request.ActualStartDate.GetValueOrDefault();

                context.LeaveTrans.Attach(trans);
                context.Entry(trans).State = EntityState.Modified;
            }
        }

        #endregion

        #region Posting

        public string CalcLeaveBalance(out IEnumerable<LeaveBalanceGridViewModel> Grids, int TypeId,
            int PeriodId, int SubPeriodId, int CompanyId, string culture)
        {
            LeaveType leaveType = GetLeaveType(TypeId);
            Period prevPeriod = context.Periods.Find(PeriodId);
            bool ApplyPostingRules = true; // check sub period

            var newPeriod = context.Periods.Where(p => p.CalendarId == leaveType.CalendarId && p.StartDate > prevPeriod.StartDate && p.Status == 1).OrderBy(p => p.StartDate).FirstOrDefault();
            var newPeriodId = newPeriod?.Id ?? 0;
            var subPeriods = context.SubPeriods.Where(s => s.PeriodId == PeriodId || s.PeriodId == newPeriodId).OrderBy(s => s.StartDate).ToList();

            //var subPeriods = context.SubPeriods.Where(s => (s.PeriodId == PeriodId || s.PeriodId == newPeriodId) && s.Status == 1).OrderBy(s => s.StartDate).ToList();
            //var newSubPeriod = context.SubPeriods.Where(s => s.Id == SubPeriodId && s.PeriodId == PeriodId && s.Status == 1).FirstOrDefault();

            var newSubPeriod = subPeriods.Find(s => s.Id == SubPeriodId);
            var prevSubPeriod = newSubPeriod == null ?
                context.SubPeriods.Where(a => a.PeriodId == PeriodId).OrderByDescending(a => a.StartDate).FirstOrDefault() :
                context.SubPeriods.FirstOrDefault(s => s.StartDate < newSubPeriod.StartDate);

            if (newPeriod == null && newSubPeriod == null)
            {
                Grids = null;
                return MsgUtils.Instance.Trls(culture, "FiscalYearnotfound"); // check new
            }
            else if (newSubPeriod != null) //new subPeriod - Add Balance
            {
                ApplyPostingRules = false;
                newPeriod = prevPeriod;
            }
            else if (newSubPeriod == null && newPeriod != null)
            {
                newSubPeriod = subPeriods.Where(s => s.PeriodId == newPeriod.Id).FirstOrDefault();
                if (newSubPeriod.Status != 1)
                    newSubPeriod = null;
            }

            DateTime PeriodStartDate = newPeriod.StartDate;
            DateTime PrevStartDate = newPeriod.StartDate.AddYears(-1);

            IEnumerable<EmpLeaveDays> list = GetLeaveDays(leaveType, PeriodId, newSubPeriod?.StartDate ?? PeriodStartDate, CompanyId, culture);
            if (list.Count() == 0)
            {
                Grids = null;
                return MsgUtils.Instance.Trls(culture, "EmployeesDoesntExist");
            }

            //SubPeriod oldSubPeriod = subPeriods.Where(s => s.PeriodId == newSubPeriod.PeriodId && s.Status == 1 && s.StartDate < newSubPeriod.StartDate).OrderByDescending(s => s.StartDate).FirstOrDefault();

            LeaveBalanceGridViewModel grid = new LeaveBalanceGridViewModel()
            {
                CompanyId = CompanyId,
                Period = prevPeriod.Name,
                PeriodId = PeriodId,
                NewPeriod = newSubPeriod?.Name ?? newPeriod.Name,
                NewPeriodId = newPeriod.Id,
                NewPeriodDate = newPeriod.StartDate.ToString("dd/MM/yyyy"),
                TypeId = leaveType.Id,
                OpenBalance = newSubPeriod == null ? 0 : leaveType.NofDays ?? 0,
                StartDate = PeriodStartDate,
                NewSubPeriodId = newSubPeriod?.Id,
                AbsenceType = leaveType.AbsenceType,
                SubStartDate = newSubPeriod?.StartDate,
                SubEndDate = newSubPeriod?.EndDate
            };

            if (leaveType.PercentOfActive)
                Grids = GetLeaveBalanceGrid(list, grid, leaveType, PrevStartDate, PeriodStartDate, culture);
            else
                Grids = GetLeaveBalanceGrid(list, grid, leaveType, prevSubPeriod?.StartDate ?? PrevStartDate, newSubPeriod?.StartDate ?? PeriodStartDate, culture);

            //Posting
            PostingLeave(Grids, leaveType, PeriodId, ApplyPostingRules, culture);

            return "Success";
        }

        private IList<EmpLeaveDays> GetLeaveDays(LeaveType leave, int selectedPeriod, DateTime StartPeriod, int companyId, string culture)
        {
            string startdate, endate;
            switch (leave.WorkServMethod)
            {
                case 2:
                    startdate = "ISNULL(P.StartExpDate,P.JoinDate) StartDate"; // start experience date
                    endate = " AND ISNULL(P.StartExpDate,P.JoinDate) <= '" + StartPeriod.ToString("yyyy/MM/dd") + "'";
                    break;
                case 3:
                    startdate = "E.StartDate StartDate"; // last employment date
                    endate = " AND E.StartDate <= '" + StartPeriod.ToString("yyyy/MM/dd") + "'";
                    break;
                case 4:
                    startdate = "A.AssignDate StartDate"; // last assignment date
                    endate = " AND A.AssignDate <= '" + StartPeriod.ToString("yyyy/MM/dd") + "'";
                    break;
                default:
                    startdate = "P.JoinDate StartDate"; // From Join date is default
                    endate = " AND P.JoinDate <= '" + StartPeriod.ToString("yyyy/MM/dd") + "'";
                    break;
            }

            string sql = "SELECT P.Id EmpId, A.Code EmpCode, dbo.fn_TrlsName(ISNULL(P.Title, '') + ' ' + P.FirstName + ' ' + P.Familyname, '" + culture + "') Name, " + startdate + ", P.BirthDate,IsNull((SELECT SUM(LT.TransQty * LT.TransFlag) FROM LeaveTrans LT WHERE LT.TypeId = LT.TypeId AND LT.PeriodId = " + selectedPeriod + " AND LT.EmpId = P.Id),0) Balance FROM LeaveTypes T, Assignments A, Employements E, People P LEFT OUTER JOIN LeavePostings LP ON (LP.PeriodId = " + selectedPeriod + " AND LP.EmpId = P.Id AND LP.Posted = 1) WHERE E.CompanyId = " + companyId + " And A.EmpId = E.EmpId And A.EmpId = P.Id And T.Id = " + leave.Id + endate + " AND LP.Id IS NULL AND (Convert(date, GETDATE()) Between A.AssignDate And A.EndDate) AND A.SysAssignStatus = 1 AND E.Status = 1 AND (Convert(date, GETDATE()) Between T.StartDate And ISNULL(T.EndDate, '2099/01/01')) AND ISNULL(T.Gender, P.Gender) = P.Gender AND ISNULL(T.Religion, ISNULL(P.Religion, 0)) = ISNULL(P.Religion, 0) AND ISNULL(T.MaritalStat, ISNULL(P.MaritalStat, 0)) = ISNULL(P.MaritalStat, 0) AND ISNULL(T.Nationality, ISNULL(P.Nationality, 0)) = ISNULL(P.Nationality, 0) AND ISNULL(T.MilitaryStat, ISNULL(P.MilitaryStat, 0)) = ISNULL(P.MilitaryStat, 0) AND (CASE WHEN LEN(T.Employments) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Employments, ',') WHERE VALUE = E.PersonType), 0) ELSE E.PersonType END) = E.PersonType AND (CASE WHEN LEN(T.Jobs) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Jobs, ',') WHERE VALUE = A.JobId), 0) ELSE A.JobId END) = A.JobId AND (CASE WHEN LEN(T.CompanyStuctures) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.CompanyStuctures, ',') WHERE VALUE = A.DepartmentId), 0) ELSE A.DepartmentId END) = A.DepartmentId AND (CASE WHEN LEN(T.Branches) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Branches, ',') WHERE VALUE = ISNULL(A.BranchId, 0)), -1) ELSE ISNULL(A.BranchId, 0) END) = ISNULL(A.BranchId, 0) AND (CASE WHEN LEN(T.Positions) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Positions, ',') WHERE VALUE = ISNULL(A.PositionId, 0)), -1) ELSE ISNULL(A.PositionId, 0) END) = ISNULL(A.PositionId, 0) AND (CASE WHEN LEN(T.PeopleGroups) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.PeopleGroups, ',') WHERE VALUE = ISNULL(A.GroupId, 0)), -1) ELSE ISNULL(A.GroupId, 0) END) = ISNULL(A.GroupId, 0) AND (CASE WHEN LEN(T.Payrolls) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Payrolls, ',') WHERE VALUE = ISNULL(A.PayrollId, 0)), -1) ELSE ISNULL(A.PayrollId, 0) END) = ISNULL(A.PayrollId, 0) AND (CASE WHEN LEN(T.PayrollGrades) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.PayrollGrades, ',') WHERE VALUE = ISNULL(A.PayGradeId, 0)), -1) ELSE ISNULL(A.PayGradeId, 0) END) = ISNULL(A.PayGradeId, 0)";
            return context.Database.SqlQuery<EmpLeaveDays>(sql).ToList();
        }

        private IList<EmpActivePeriods> GetEmployments(IEnumerable<int> Ids, DateTime StartPeriod)
        {
            string persontype = "C.SysCodeId = 1"; // Regulare employees only;
            //switch (WorkServMethod)
            //{
            //    case 5:
            //        persontype = "C.SysCodeId = 1"; // Regulare employees only
            //        break;
            //    default:
            //        persontype = "C.SysCodeId IN (1,2,4,5)"; // All employees type
            //        break;
            //}

            string employees = string.Join(",", Ids);

            string sql = "SELECT E.EmpId, SUM(DATEDIFF(month, E.StartDate, ISNULL(E.EndDate, '" + StartPeriod.ToString("yyyy/MM/dd") + "')) - (CASE WHEN DATEPART(day, E.StartDate) >= 15 THEN 1 ELSE 0 END)) Months FROM Employements E WHERE E.EmpId IN (" + employees + ") AND E.StartDate <= '" + StartPeriod.ToString("yyyy/MM/dd") + "' AND E.Status < 3 AND E.PersonType IN (SELECT C.CodeId FROM LookUpUserCodes C WHERE C.CodeName = 'PersonType' AND " + persontype + ") GROUP BY E.EmpId";
            return context.Database.SqlQuery<EmpActivePeriods>(sql).ToList();
        }

        private IList<EmpActivePeriods> GetAllAssignments(IEnumerable<int> Ids, DateTime StartPeriod)
        {
            string employees = string.Join(",", Ids);
            string sql = "SELECT A.EmpId, SUM(DATEDIFF(month, A.AssignDate, (CASE WHEN A.EndDate > '" + StartPeriod.ToString("yyyy/MM/dd") + "' THEN '" + StartPeriod.ToString("yyyy/MM/dd") + "' ELSE A.EndDate END)) - (CASE WHEN DATEPART(day, A.AssignDate) >= 15 THEN 1 ELSE 0 END)) Months FROM Assignments A WHERE A.EmpId IN (" + employees + ") AND A.AssignDate <= '" + StartPeriod.ToString("yyyy/MM/dd") + "' AND A.SysAssignStatus = 1 GROUP BY A.EmpId";
            return context.Database.SqlQuery<EmpActivePeriods>(sql).ToList();
        }

        private IList<EmpActivePeriods> GetAssignments(IEnumerable<int> Ids, DateTime PrevStartPeriod, DateTime StartPeriod)
        {
            string employees = string.Join(",", Ids);
            string sql = "SELECT A.EmpId, SUM(DATEDIFF(day, (CASE WHEN A.AssignDate <= '" + PrevStartPeriod.ToString("yyyy/MM/dd") + "' THEN '" + PrevStartPeriod.ToString("yyyy/MM/dd") + "' ELSE A.AssignDate END), (CASE WHEN A.EndDate > '" + StartPeriod.ToString("yyyy/MM/dd") + "' THEN '" + StartPeriod.ToString("yyyy/MM/dd") + "' ELSE A.EndDate END))) Months FROM Assignments A WHERE A.EmpId IN (" + employees + ") AND A.AssignDate <= '" + StartPeriod.ToString("yyyy/MM/dd") + "' AND A.EndDate > '" + PrevStartPeriod.ToString("yyyy/MM/dd") + "' AND A.SysAssignStatus = 1 GROUP BY A.EmpId";
            return context.Database.SqlQuery<EmpActivePeriods>(sql).ToList();
        }

        private IEnumerable<LeaveBalanceGridViewModel> GetLeaveBalanceGrid(IEnumerable<EmpLeaveDays> list, LeaveBalanceGridViewModel grid, LeaveType leaveType, DateTime PrevStartPeriod, DateTime StartPeriod, string culture)
        {
            List<LeaveBalanceGridViewModel> grids = new List<LeaveBalanceGridViewModel>();
            IList<LeaveRange> ranges = null;
            string trls_years = MsgUtils.Instance.Trls(culture, "years");
            string trls_months = MsgUtils.Instance.Trls(culture, "months");

            if (leaveType.AccBalDays == 2) // Working Service Duration 
            {
                ranges = context.LeaveRanges.Where(r => r.LeaveTypeId == leaveType.Id).Select(r => r).ToList();
                foreach (var range in ranges)
                {
                    if (leaveType.MonthOrYear == 2) // Year
                    { // Convert years to months
                        range.FromPeriod = (short)(range.FromPeriod * 12);
                        range.ToPeriod = (short)(range.ToPeriod * 12);
                    }
                }
            }

            List<int> Ids = list.Select(l => l.EmpId).ToList();

            IList<EmpActivePeriods> EmpActualActivePeriods = new List<EmpActivePeriods>();
            IList<EmpActivePeriods> EmpAllActivePeriods = new List<EmpActivePeriods>();


            // Percent of active periods
            if (leaveType.PercentOfActive)
                EmpActualActivePeriods = GetAssignments(Ids, PrevStartPeriod, StartPeriod);

            if (leaveType.WorkServMethod == 5)  // Employment Periods
                EmpAllActivePeriods = GetEmployments(Ids, StartPeriod);
            else if (leaveType.WorkServMethod == 6)  // Active Assignments Periods
                EmpAllActivePeriods = GetAllAssignments(Ids, StartPeriod);

            var empList = context.People.Where(p => Ids.Contains(p.Id)).Select(p => new { p.Id, Name = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture) }).ToList();

            foreach (var item in list)
            {
                string empName = empList.FirstOrDefault(e => e.Id == item.EmpId).Name;
                int empAge = GetMonthsDiff(item.BirthDate, StartPeriod);
                int workDuration = (leaveType.WorkServMethod == null || leaveType.WorkServMethod < 4 ? GetMonthsDiff(item.StartDate, StartPeriod) : EmpAllActivePeriods.FirstOrDefault(e => e.EmpId == item.EmpId)?.Months ?? 0);

                grid = new LeaveBalanceGridViewModel()
                {
                    EmpId = item.EmpId,
                    EmpCode = item.EmpCode,
                    Employee = empName,
                    Name = grid.Name,
                    TypeId = grid.TypeId,
                    CompanyId = grid.CompanyId,
                    Period = grid.Period,
                    PeriodId = grid.PeriodId,
                    NewPeriod = grid.NewPeriod,
                    NewPeriodId = grid.NewPeriodId,
                    NewPeriodDate = grid.NewPeriodDate,
                    NewSubPeriodId = grid.NewSubPeriodId,
                    MonthWorkDuration = workDuration,
                    WorkDuration = GetAgeText(workDuration, culture, trls_years, trls_months),
                    StartDate = item.StartDate,
                    Age = empAge,
                    AgeText = GetAgeText(empAge, culture, trls_years, trls_months),
                    CurrBalance = (float)item.Balance,
                    OpenBalance = grid.NewSubPeriodId == null ? 0 : leaveType.NofDays ?? 0,
                    PostBal = Convert.ToSingle(item.Balance),
                    Compensations = 0,
                    AbsenceType = grid.AbsenceType,
                    SubStartDate = grid.SubStartDate,
                    SubEndDate = grid.SubEndDate,
                };

                if (grid.NewSubPeriodId != null)
                {
                    // get sub period length in months
                    float subPeriodLength = grid.SubEndDate.Value.Month - grid.SubStartDate.Value.Month + 1;

                    // compare employee age with the specific age (50Years) in months
                    float age = empAge - Convert.ToInt32(leaveType.Age50 * 12);
                    var AgeOverlapedServiceDuration = leaveType.AccBalDays == 2 && leaveType.Balanace50 && ((leaveType.PercentOfActive && age > 0 && age < 12) || (!leaveType.PercentOfActive && age < 0 && -age < subPeriodLength));
                    float part1 = 0, part2 = 0, part3 = 0, first = 0, second = 0, third = 0;

                    if (leaveType.AccBalDays == 2) // Working Service Duration
                    {
                        // get matched record according to employee experiance
                        var firstPeriod = new LeaveRange();

                        if (leaveType.PercentOfActive && leaveType.MonthOrYear == 2)
                            firstPeriod = ranges.Where(r => r.FromPeriod <= grid.MonthWorkDuration && r.ToPeriod >= grid.MonthWorkDuration).Select(r => new LeaveRange { FromPeriod = r.FromPeriod, ToPeriod = r.ToPeriod, NofDays = r.NofDays }).FirstOrDefault();
                        else
                            firstPeriod = ranges.Where(r => r.FromPeriod <= grid.MonthWorkDuration && r.ToPeriod > grid.MonthWorkDuration).Select(r => new LeaveRange { FromPeriod = r.FromPeriod, ToPeriod = r.ToPeriod, NofDays = r.NofDays }).FirstOrDefault();

                        if (firstPeriod != null)
                        {
                            grid.OpenBalance = firstPeriod.NofDays; // get direct result from employee

                            // check period interactions
                            if (leaveType.MonthOrYear == 2) // Year: only check interactions for years
                            {
                                int milestone = 0;
                                float secondPeriod = 0;

                                // second period after first period (egypt case)
                                if (!leaveType.PercentOfActive)
                                {
                                    secondPeriod = ranges.Where(r => r.FromPeriod == firstPeriod.ToPeriod).Select(r => r.NofDays).FirstOrDefault();
                                    if (secondPeriod > 0) // found interaction
                                        milestone = firstPeriod.ToPeriod;
                                }
                                else // lypia case: second period before first period
                                {
                                    secondPeriod = ranges.Where(r => r.ToPeriod == firstPeriod.FromPeriod).Select(r => r.NofDays).FirstOrDefault();
                                    if (secondPeriod > 0) // found interaction
                                        milestone = firstPeriod.FromPeriod;
                                }

                                if (milestone > 0) // found interaction
                                {
                                    // Compare employee experiance with milsestone
                                    float months = grid.MonthWorkDuration - milestone;
                                    // Egypt start balance
                                    // if months >= 0 then employee exceed milestone, no effect
                                    // else if months >= -12 then employee is far than milestone. no effect
                                    // else if months between -1 and -11 then employee get two percents no of No of days
                                    if (!leaveType.PercentOfActive)
                                    {
                                        months = -months; // simplify compare 
                                        if (months > 0 && months < subPeriodLength)
                                        {
                                            part1 = months;
                                            part2 = subPeriodLength - months;
                                            first = firstPeriod.NofDays;
                                            second = secondPeriod;
                                            grid.OpenBalance = firstPeriod.NofDays * (part1 / subPeriodLength) +
                                                secondPeriod * (part2 / subPeriodLength);
                                        }
                                    }
                                    else
                                    {
                                        // Lypia due balance (calculation is based on 12 months because all subperiod get equal balance)
                                        // if months > 0 && months < 12 then employee get two percents no of No of days
                                        // if months >= 12 employee far from milestone
                                        // if months <= 0 employee is before milestone
                                        if (months > 0 && months < 12)
                                        {
                                            part2 = months;
                                            part1 = 12 - months;
                                            second = firstPeriod.NofDays;
                                            first = secondPeriod;
                                            grid.OpenBalance = firstPeriod.NofDays * (part2 / 12) +
                                                    secondPeriod * (part1 / 12);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            grid.OpenBalance = ranges.OrderByDescending(r => r.FromPeriod).Select(r => r.NofDays).FirstOrDefault();
                        }

                    }

                    // Check leave balance change in specific age
                    if (leaveType.Balanace50)
                    {
                        // get age text
                        grid.AgeText = grid.AgeText; //GetAgeText(grid.Age, culture, trls_years, trls_months);

                        // Egypt start balance
                        // if age >= 0 then employee exceed 50years so he get full No of days
                        // else if age >= -12 then employee is young so don't affect
                        // else if age between -1 and -11 then employee get two percents no of No of days
                        if (!leaveType.PercentOfActive)
                        {
                            age = -age; // simplify compare 
                            if (age <= 0)
                                grid.OpenBalance = leaveType.NofDays50.Value;
                            else if (age > 0 && age < subPeriodLength)
                            {
                                part3 = subPeriodLength - age;
                                third = leaveType.NofDays50.Value;
                                grid.OpenBalance = grid.OpenBalance * (age / subPeriodLength) +
                                    leaveType.NofDays50.Value * ((subPeriodLength - age) / subPeriodLength);
                            }
                        }
                        else
                        {
                            // Lypya due balance (calculation is based on 12 months because all subperiod get equal balance)
                            // if age > 0 && age < 12 then employee get to percents no of year
                            // if age >= 12 employee get full 50years balance
                            // if age <= 0 employee is young so don't affect
                            if (age > 0 && age < 12)
                            {
                                part3 = age;
                                third = leaveType.NofDays50.Value;
                                grid.OpenBalance = grid.OpenBalance * ((12 - age) / 12) +
                                        leaveType.NofDays50.Value * (age / 12);
                            }
                            else if (age >= 12)
                                grid.OpenBalance = leaveType.NofDays50.Value;
                        }
                    }

                    if (AgeOverlapedServiceDuration && (part1 > 0 || part2 > 0))
                    {
                        if (leaveType.PercentOfActive) subPeriodLength = 12;
                        if (subPeriodLength - part3 <= part1)
                        {
                            part1 = subPeriodLength - part3;
                            part2 = 0;
                        }
                        else part2 = subPeriodLength - part3 - part1;

                        grid.OpenBalance = first * part1 / subPeriodLength + second * part2 / subPeriodLength + third * part3 / subPeriodLength;
                    }

                    // Percent of actual active period
                    if (leaveType.PercentOfActive)
                    {
                        int days = EmpActualActivePeriods.Where(e => e.EmpId == grid.EmpId).Select(e => e.Months).FirstOrDefault();
                        if (days > 365) days = 365;
                        grid.OpenBalance = grid.OpenBalance * days / 365;
                    }
                }

                grid.OpenBalance = (float)Math.Round(grid.OpenBalance, 2);
                grid.Total = grid.OpenBalance + grid.PostBal;
                grids.Add(grid);
            }

            return grids;
        }

        private void PostingLeave(IEnumerable<LeaveBalanceGridViewModel> Grids, LeaveType leaveType, int PrevPeriodId, bool ApplyPostingRules, string culture)
        {
            IEnumerable<MinDaysViewModel> MinDays = null;

            if (leaveType.MinLeaveDays > 0)
                MinDays = context.LeaveTrans.Where(t => (t.LeaveType.AbsenceType == leaveType.AbsenceType) && t.PeriodId == PrevPeriodId && (t.TransType == 5 || t.TransType == 11 || t.TransType == 12)).GroupBy(t => t.EmpId).Select(g => new MinDaysViewModel { EmpId = g.Key, MinLeaveDays = g.Sum(a => a.TransQty * a.TransFlag), IncludContinu = g.Max(a => a.TransQty) });

            foreach (var item in Grids)
            {
                //if (item.PostBal < 0)
                //{
                //    item.OpenBalance += item.PostBal;
                //    item.PostBal = 0;
                //}

                if (ApplyPostingRules == false || leaveType.PostOpt == 0) //PostOpt : 0.No Posting
                {
                    if (item.PostBal > 0) item.PostBal = 0;
                    // if (item.PostBal == 0) item.Status = "";
                    item.PostAction = 0; //No Action

                    //for - balance(AllowNegBal) or add balance(not posting)
                    if (ApplyPostingRules && item.PostBal >= 0)
                        item.Total = item.OpenBalance;
                    else
                        item.Total = item.OpenBalance + item.CurrBalance;

                }
                else
                {
                    if (leaveType.PostOpt == 1) //PostOpt : 1.Post all days
                    {
                        item.PostAction = 1; //Success
                    }
                    else
                    {
                        float? limit = (leaveType.PostOpt == 3 ? (leaveType.MaxPercent * item.OpenBalance) : leaveType.MaxNofDays);
                        if (limit < item.PostBal)
                        {
                            if (leaveType.DiffDaysOpt == 1)
                            {
                                item.PostAction = 2; // Add Time compensation trans
                                item.Reason = MsgUtils.Instance.Trls(culture, "Time compensation days") + " = " + (item.PostBal - limit);
                                item.Compensations = item.PostBal - limit.Value;
                                item.PostBal = limit.Value;
                                item.Total = item.OpenBalance + item.PostBal;
                            }
                            else
                            {
                                item.PostAction = 3; //Fail
                                item.Reason = MsgUtils.Instance.Trls(culture, "PostedBalExceedMinLimit");
                                item.PostBal = limit.Value;
                                item.Total = item.OpenBalance + item.PostBal;
                            }

                        }
                        else
                            item.PostAction = 1; //Success
                    }

                    if (item.PostBal > 0 && leaveType.MinLeaveDays > 0)
                    {
                        var Min = MinDays.FirstOrDefault(e => e.EmpId == item.EmpId);
                        if (Min == null || ((Min.MinLeaveDays > leaveType.MinLeaveDays.Value) || (leaveType.IncludContinu.Value > 0 && leaveType.IncludContinu > Min.IncludContinu)))
                        {
                            item.PostBal = 0;
                            item.PostAction = 3; //Fail
                            item.Total = item.OpenBalance + item.PostBal;
                            item.Reason = MsgUtils.Instance.Trls(culture, "EmpNotRecieveMinPeriod");
                        }
                    }
                }
            }

        }

        private int GetMonthsDiff(DateTime StartDate, DateTime StartPeriod)
        {
            if (StartDate.Day > 15)
                StartDate = new DateTime(StartDate.Year, (StartDate.AddMonths(1).Month), 1);

            int months = Convert.ToInt32(StartPeriod.Subtract(StartDate).Days / 365.25 * 12);
            return months;
        }

        private string GetAgeText(int age, string culture, string trls_years, string trls_months)
        {
            int years = age / 12;
            int months = age % 12;

            string ageText = (years > 0 ? years + " " + trls_years : "");
            if (months > 0)
                ageText += (years > 0 ? ", " : "") + months + " " + trls_months;

            return ageText;
        }

        #endregion

        #region Calculations
        public int GetLeaveRequestPeriod(int calendar, DateTime startDate, string culture, out string message)
        {
            message = "OK";
            var period = context.Periods.Where(a => a.CalendarId == calendar && a.StartDate <= startDate && a.EndDate >= startDate)
                 .Select(a => new { PeriodId = a.Id, Status = a.Status }).FirstOrDefault();

            if (period == null)
            {
                message = MsgUtils.Instance.Trls(culture, "PeriodIsNotDefined");
                return -1;
            }

            if (period.Status == 2)
            {
                message = MsgUtils.Instance.Trls(culture, "PeriodIsClosed");
                return -1;
            }

            //var status = context.SubPeriods.Where(a => a.PeriodId == period.PeriodId && a.StartDate <= startDate && a.EndDate >= startDate)
            //    .Select(a => a.Status).FirstOrDefault();

            //if (status == 2)
            //{
            //    message = MsgUtils.Instance.Trls(culture, "SubPeriodIsClosed");
            //    return -1;
            //}

            return period.PeriodId;
        }
        public int GetLeaveRequestPeriod(int calendar, DateTime startDate, string culture)
        {
            var period = context.Periods.Where(a => a.CalendarId == calendar && a.StartDate <= startDate && a.EndDate >= startDate)
                 .Select(a => new { PeriodId = a.Id, Status = a.Status }).FirstOrDefault();

            if (period == null)
            {

                return -1;
            }

            if (period.Status == 2)
            {
                return -1;
            }

            //var status = context.SubPeriods.Where(a => a.PeriodId == period.PeriodId && a.StartDate <= startDate && a.EndDate >= startDate)
            //    .Select(a => a.Status).FirstOrDefault();

            //if (status == 2)
            //{
            //    message = MsgUtils.Instance.Trls(culture, "SubPeriodIsClosed");
            //    return -1;
            //}

            return period.PeriodId;
        }



        private float GetMaxAllowedDays(int leaveType, short absenceType, int periodId, int empId, bool AllowNegBal, float? Percentage, short? MaxDaysInPeriod, out float balance)
        {
            balance = context.LeaveTrans
                .Where(a => a.TypeId == leaveType && a.PeriodId == periodId && a.EmpId == empId)
                .Select(a => a.TransQty * a.TransFlag).DefaultIfEmpty().Sum();

            //if leave dosn't Have Accural Balance
            if (MaxDaysInPeriod != null && MaxDaysInPeriod.Value > 0)
            {
                balance = MaxDaysInPeriod.Value - Math.Abs(balance);
                return balance;
            }

            float addBalance = 0;

            if (AllowNegBal && Percentage != null)
            {
                addBalance = context.LeaveTrans
                .Where(a => a.TypeId == leaveType && a.PeriodId == periodId && a.EmpId == empId && a.TransType == 2) //2. Accrual Balance استحقاق الرصيد
                .Select(a => a.TransQty).DefaultIfEmpty().Sum();

                addBalance = addBalance * Percentage.Value;
            }
            return balance + addBalance;
        }

        private IEnumerable<RequestValidationViewModel> GetReservedDays(int leaveType, int periodId, List<int> empIds, int requestId)
        {
            var days = context.LeaveRequests
                .Where(a => a.Id != requestId && a.TypeId == leaveType && a.PeriodId == periodId && empIds.Contains(a.EmpId) && a.ApprovalStatus < 6 && a.ApprovalStatus != 1)
                .Select(a => new { a.EmpId, a.NofDays }).GroupBy(a => a.EmpId).Select(a => new RequestValidationViewModel { EmpId = a.Key, ReservedDays = a.Sum(s => s.NofDays) }).ToList();

            return days;
        }
        #endregion

        //Check Employee Assginment Status
        public string CheckAssignStatus(int empId, int typeId, out LeaveType leaveType, string culture)
        {
            leaveType = GetLeaveType(typeId);
            if (leaveType.AssignStatus != null)
            {
                var future = context.Assignments.Where(a => a.EmpId == empId && (a.AssignDate > DateTime.Today.Date)).FirstOrDefault();
                if (future != null) return MsgUtils.Instance.Trls(culture, "AlreadyHaveFutureAssignment");
            }
            return null;
        }

        #region Leave Request
        public IList<DropDownList> GetEmpLeaveTypes(int empId, int compnayId, string culture)
        {
            string sql = "SELECT T.Id, dbo.fn_TrlsName(T.Name, '" + culture + "') Name FROM LeaveTypes T, Assignments A, Employements E, People P WHERE A.EmpId = E.EmpId And A.EmpId = P.Id And A.EmpId = " + empId + " AND (CONVERT(date, getdate()) Between A.AssignDate And A.EndDate) AND E.Status = 1 AND (GETDATE() Between T.StartDate And ISNULL(T.EndDate, '2099/01/01')) AND ((T.IsLocal = 1 And T.CompanyId = " + compnayId + ") Or t.IsLocal = 0) AND ISNULL(T.Gender, P.Gender) = P.Gender AND ISNULL(T.Religion, ISNULL(P.Religion, 0)) = ISNULL(P.Religion, 0) AND ISNULL(T.MaritalStat, ISNULL(P.MaritalStat, 0)) = ISNULL(P.MaritalStat, 0) AND ISNULL(T.Nationality, ISNULL(P.Nationality, 0)) = ISNULL(P.Nationality, 0) AND ISNULL(T.MilitaryStat, ISNULL(P.MilitaryStat, 0)) = ISNULL(P.MilitaryStat, 0) AND (CASE WHEN LEN(T.Employments) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Employments, ',') WHERE VALUE = E.PersonType), 0) ELSE E.PersonType END) = E.PersonType AND (CASE WHEN LEN(T.Jobs) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Jobs, ',') WHERE VALUE = A.JobId), 0) ELSE A.JobId END) = A.JobId AND (CASE WHEN LEN(T.CompanyStuctures) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.CompanyStuctures, ',') WHERE VALUE = A.DepartmentId), 0) ELSE A.DepartmentId END) = A.DepartmentId AND (CASE WHEN LEN(T.Branches) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Branches, ',') WHERE VALUE = ISNULL(A.BranchId, 0)), -1) ELSE ISNULL(A.BranchId, 0) END) = ISNULL(A.BranchId, 0) AND (CASE WHEN LEN(T.Positions) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Positions, ',') WHERE VALUE = ISNULL(A.PositionId, 0)), -1) ELSE ISNULL(A.PositionId, 0) END) = ISNULL(A.PositionId, 0) AND (CASE WHEN LEN(T.PeopleGroups) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.PeopleGroups, ',') WHERE VALUE = ISNULL(A.GroupId, 0)), -1) ELSE ISNULL(A.GroupId, 0) END) = ISNULL(A.GroupId, 0) AND (CASE WHEN LEN(T.Payrolls) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Payrolls, ',') WHERE VALUE = ISNULL(A.PayrollId, 0)), -1) ELSE ISNULL(A.PayrollId, 0) END) = ISNULL(A.PayrollId, 0) AND (CASE WHEN LEN(T.PayrollGrades) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.PayrollGrades, ',') WHERE VALUE = ISNULL(A.PayGradeId, 0)), -1) ELSE ISNULL(A.PayGradeId, 0) END) = ISNULL(A.PayGradeId, 0)";
            return context.Database.SqlQuery<DropDownList>(sql).ToList();
        }

        public IQueryable<LeaveRequestViewModel> ReadLeaveRequests(int companyId, string culture)
        {
            DateTime today = DateTime.Today;
            return from l in context.LeaveRequests
                   where l.CompanyId == companyId
                   join a in context.Assignments on l.EmpId equals a.EmpId
                   where (a.CompanyId == companyId && a.AssignDate <= l.StartDate && a.EndDate >= l.StartDate)
                   join p in context.People on l.EmpId equals p.Id
                   join lt in context.LeaveTypes on l.TypeId equals lt.Id
                   select new LeaveRequestViewModel
                   {
                       Id = l.Id,
                       RequestDate = l.RequestDate,
                       Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                       Type = HrContext.TrlsName(lt.Name, culture),
                       TypeId = l.TypeId,
                       StartDate = l.StartDate,
                       ReturnDate = l.ReturnDate,
                       NofDays = l.NofDays,
                       ApprovalStatus = l.ApprovalStatus,
                       EmpId = l.EmpId,
                       CompanyId = l.CompanyId,
                       DeptId = a.DepartmentId,
                       BranchId = a.BranchId,
                       HasImage = p.HasImage,
                       EndDate = l.EndDate,
                       ReplaceEmpId = l.ReplaceEmpId,
                       //last edit by Abdelazim
                       ReqReason = l.ReqReason,
                       ReasonDesc = l.ReasonDesc,
                       RejectReason = l.RejectReason,
                       RejectDesc = l.RejectDesc,
                       ReqStatus = l.ReqStatus,
                       CancelReason = l.CancelReason,
                       CancelDesc = l.CancelDesc,
                       ActualEndDate = l.ActualEndDate,
                       ActualStartDate=l.ActualStartDate,
                       ActualNofDays = l.ActualNofDays,
                       DayFraction = l.DayFraction,
                       isStarted = today >= (l.ActualStartDate ?? l.StartDate),
                       isBreaked = (DbFunctions.TruncateTime(l.EndDate)  != DbFunctions.TruncateTime(l.ActualEndDate)) && (DbFunctions.TruncateTime(l.StartDate) == DbFunctions.TruncateTime(l.ActualStartDate))
                   };
        }

        public IQueryable<LeaveRequestViewModel> ReadLeaveRequestArchive(int companyId, byte Range, string Depts, DateTime? Start, DateTime? End, string culture)
        {
            //10- All, 0-Custom
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, companyId, out Start, out End);
            List<int> deptLst = new List<int>();
            if (!String.IsNullOrEmpty(Depts))
                deptLst = Depts.Split(',').Select(a => int.Parse(a)).ToList();
            else
                Depts = "";

            DateTime Today = DateTime.Today.Date;
            var q1 = from l in context.LeaveRequests
                     where l.CompanyId == companyId && ((l.ApprovalStatus == 6 && Today > l.ActualEndDate) || (l.ApprovalStatus > 6 && Today > (l.ActualStartDate ?? l.StartDate)))
                     select l;

            if (Range != 10) // Allow date range
                q1 = q1.Where(l => Start <= l.StartDate & l.StartDate <= End);

            IQueryable<LeaveRequestViewModel> query =
                from l in q1
                join a in context.Assignments on l.EmpId equals a.EmpId
                where (a.CompanyId == companyId && a.AssignDate <= l.StartDate && a.EndDate >= l.StartDate && (Depts == "" || deptLst.Contains(a.DepartmentId)))
                join p in context.People on l.EmpId equals p.Id
                select new LeaveRequestViewModel
                {
                    Id = l.Id,
                    RequestDate = l.RequestDate,
                    Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                    TypeId = l.TypeId,
                    StartDate = l.ActualStartDate ?? l.StartDate,
                    ReturnDate = l.ReturnDate,
                    NofDays = l.ActualNofDays ?? l.NofDays,
                    ApprovalStatus = l.ApprovalStatus,
                    EmpId = l.EmpId,
                    CompanyId = l.CompanyId,
                    HasImage = p.HasImage,
                    Notes = l.ApprovalStatus == 9 && l.RejectReason != null ? HrContext.GetLookUpCode("LeaveRejectReason", l.RejectReason.Value, culture) : l.RejectDesc,
                    AttUrl = HrContext.GetDoc("LeaveRequest", l.Id)
                };

            return query.OrderByDescending(a=>a.StartDate);
        }

        public IQueryable<LeaveRequestViewModel> ReadLeaveRequestTabs(int companyId, byte Tab, byte Range, string Depts, DateTime? Start, DateTime? End, string culture)
        {
            //10- All, 0-Custom
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, companyId, out Start, out End);
            List<int> deptLst = new List<int>();
            if (!String.IsNullOrEmpty(Depts))
                deptLst = Depts.Split(',').Select(a => int.Parse(a)).ToList();
            else
                Depts = "";

            DateTime Today = DateTime.Today.Date;
            var q1 = from l in context.LeaveRequests
                     where l.CompanyId == companyId
                     select l;

            if (Range != 10) // Allow date range
                q1 = q1.Where(l => Start <= l.StartDate && l.StartDate <= End);

            if (Tab == 1) //Pending
                q1 = q1.Where(l => l.ApprovalStatus < 6);
            else if (Tab == 2) //Approved
                q1 = q1.Where(l => l.ApprovalStatus == 6 && Today <= l.ActualEndDate);
            else if (Tab == 3) //Rejected
                q1 = q1.Where(l => l.ApprovalStatus == 9 && Today <= l.StartDate);
            else if (Tab == 4) //Archive
                q1 = q1.Where(l => (l.ApprovalStatus == 6 && Today > l.ActualEndDate) || ( l.ApprovalStatus > 6 && Today > (l.ActualStartDate ?? l.StartDate)));

            IQueryable<LeaveRequestViewModel> query =
                from l in q1
                join a in context.Assignments on l.EmpId equals a.EmpId
                where (a.CompanyId == companyId && a.AssignDate <= l.StartDate && a.EndDate >= l.StartDate && (Depts == "" || deptLst.Contains(a.DepartmentId)))
                join p in context.People on l.EmpId equals p.Id
                select new LeaveRequestViewModel
                {
                    Id = l.Id,
                    RequestDate = l.RequestDate,
                    Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                    TypeId = l.TypeId,
                    StartDate = l.ActualStartDate ?? l.StartDate,
                    ReturnDate = l.ReturnDate,
                    NofDays = l.ActualNofDays ?? l.NofDays,
                    ApprovalStatus = l.ApprovalStatus,
                    EmpId = l.EmpId,
                    CompanyId = l.CompanyId,
                    Code = a.Code,
                    JobId = a.JobId,
                    JobName = HrContext.TrlsName(a.Job.Name, culture),
                    DeptId = a.DepartmentId,
                    DeptName = HrContext.TrlsName(a.Department.Name, culture),
                    BranchId = a.BranchId,
                    BranchName = HrContext.TrlsName(a.Branch.Name, culture),
                    HasImage = p.HasImage,
                    Notes = l.ApprovalStatus == 9 && l.RejectReason != null ? HrContext.GetLookUpCode("LeaveRejectReason", l.RejectReason.Value, culture) : l.RejectDesc,
                    AttUrl = HrContext.GetDoc("LeaveRequest", l.Id)
                };

            return query.OrderByDescending(a => a.StartDate);
        }

        public RequestWfFormViewModel ReadRequestWF(int sourceId, string source, string lang)
        {
            var request = from r in context.RequestWf
                          where r.Source == source && r.SourceId == sourceId
                          select new RequestWfFormViewModel
                          {
                              Source = r.Source,
                              HeirType = r.HeirType,
                              Hierarchy = r.Hierarchy,
                              Id = r.Id,
                              NofApprovals = r.NofApprovals,
                              SourceId = r.SourceId,
                              TimeOutAction = r.TimeOutAction,
                              TimeOutDays = r.TimeOutDays,
                              ForceUpload = r.ForceUpload,
                              Roles = context.LookUpUserCodes.Where(c => c.CodeName == "Roles").Select(c => new Model.ViewModel.Personnel.RolesViewModel { RoleId = null, CodeId = c.CodeId, text = HrContext.GetLookUpUserCode("Roles", c.CodeId, lang) }).Union(context.Roles.Select(a => new Model.ViewModel.Personnel.RolesViewModel { RoleId = a.Id, CodeId = null, text = a.Name })).ToList(),
                              Diagrams = context.Diagrams.Select(a => new FormDropDown { id = a.Id, name = a.Name }).ToList()
                          };

            return request.FirstOrDefault();
        }

        public LeaveReqViewModel GetRequest(int requestId, string culture)
        {
            if (requestId == 0)
                return new LeaveReqViewModel();

            LeaveReqViewModel Request = (from req in context.LeaveRequests
                                         where req.Id == requestId
                                         join p in context.People on req.EmpId equals p.Id
                                         join l in context.LeaveTypes on req.TypeId equals l.Id
                                         select new LeaveReqViewModel()
                                         {
                                             Id = req.Id,
                                             CompanyId = req.CompanyId,
                                             RequestDate = req.RequestDate,
                                             EmpId = req.EmpId,
                                             Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                                             TypeId = req.TypeId,
                                             AbsenceType = l.AbsenceType,
                                             LeaveType = HrContext.TrlsName(l.Name, culture),
                                             ReqStatus = req.ReqStatus,
                                             ApprovalStatus = req.ApprovalStatus,
                                             ReqReason = req.ReqReason,
                                             ReasonDesc = req.ReasonDesc,
                                             StartDate = req.StartDate,
                                             EndDate = req.EndDate,
                                             ReturnDate = req.ReturnDate,
                                             NofDays = req.NofDays,
                                             ReplaceEmpId = req.ReplaceEmpId,
                                             AuthbyEmpId = req.AuthbyEmpId,
                                             DayFraction = req.DayFraction,
                                             ActualEndDate = req.ActualEndDate,
                                             ActualNofDays = req.ActualNofDays,
                                             ActualStartDate = req.ActualStartDate,
                                             BalanceBefore = req.BalanceBefore,
                                             PeriodId = req.PeriodId,
                                             RejectReason = req.RejectReason,
                                             RejectDesc = req.RejectDesc,
                                             CancelReason = req.CancelReason,
                                             CancelDesc = req.CancelDesc,
                                             Stars = req.Stars,
                                             Attachments = HrContext.GetAttachments("LeaveRequest", req.Id),
                                             ForceUpload = HrContext.ForceUpload("Leave", req.TypeId),
                                             CreatedTime = req.CreatedTime,
                                             CreatedUser = req.CreatedUser,
                                             ModifiedTime = req.ModifiedTime,
                                             ModifiedUser = req.ModifiedUser,
                                             AllowNegBal = l.AllowNegBal,
                                             MaxDaysInPeriod = l.MaxDaysInPeriod,
                                             Percentage = l.Percentage
                                         }).FirstOrDefault();

            float balance;
            Request.ReservedDays = GetReservedDays(Request.TypeId, Request.PeriodId, new List<int> { Request.EmpId }, requestId).Where(r => r.EmpId == Request.EmpId).Select(r => r.ReservedDays).FirstOrDefault() ?? 0;
            Request.AllowedDays = GetMaxAllowedDays(Request.TypeId, Request.AbsenceType, Request.PeriodId, Request.EmpId, Request.AllowNegBal, Request.Percentage, Request.MaxDaysInPeriod, out balance) - Request.ReservedDays;
            float balBefore = balance, balAfter = balance;
            if (Request.ApprovalStatus == 6)
                balBefore = balance + (Request.ActualNofDays ?? Request.NofDays);
            else if (Request.ApprovalStatus < 6)
                balAfter = (Request.ApprovalStatus <= 3 ? balance - Request.NofDays : balance - (Request.ActualNofDays ?? Request.NofDays));

            Request.BalBefore = balBefore;
            Request.BalAfter = balAfter;

            return Request;
        }

        public RequestValidationViewModel CheckLeaveRequest(int TypeId, int EmpId, DateTime StartDate, DateTime EndDate, float NofDays, string culture, int RequestId, bool isSSUser, int companyId, int? replaceEmp = null)
        {
            LeaveType type = new LeaveType();
            string assignError = CheckAssignStatus(EmpId, TypeId, out type, culture);

            RequestValidationViewModel requestVal = new RequestValidationViewModel() { ExDayOff = type.ExDayOff, ExHolidays = type.ExHolidays, MaxDays = type.MaxDays };
            StringBuilder message = new StringBuilder();

            //if future assignment && leave type chanege assignment -> prevent
            if (!string.IsNullOrEmpty(assignError))
            {
                requestVal.Message = assignError;
                return requestVal;
            }

            #region Replacement Employee
            //Check if the employee is Replacement Employee in this date
            var replaceFor = IsReplacement(EmpId, StartDate, EndDate, companyId, culture);
            if (replaceFor.Count > 0)
                message.Append(MsgUtils.Instance.Trls(culture, "YouAreReplacementInThisDateFor").Replace("{0}", string.Join(",", replaceFor)) + "<br>");

            //Check if the Repalcement Employee is Avilable
            if (replaceEmp != null)
            {
                var replaceAvilabe = HavePervRequests(new List<int> { replaceEmp.GetValueOrDefault() }, 0, StartDate, EndDate, companyId, true);
                if (replaceAvilabe.Count > 0)
                    message.Append(MsgUtils.Instance.Trls(culture, "ReplaceEmployeeInLeave") + "<br>");
            }
            #endregion
            
            if (NofDays <= 0)
                message.Append(MsgUtils.Instance.Trls(culture, "CantBeLessThan") + " 0<br>");
            else if (!type.AllowFraction && Math.Truncate(NofDays) != NofDays) //if NofDays is float and leave don't allow fraction
                message.Append(MsgUtils.Instance.Trls(culture, "LeaveNotAllowFraction") + "<br>");

            //Waiting Monthes
            var today = DateTime.Today.Date;
            if (type.WaitingMonth != null && type.WaitingMonth.Value > 0)
            {
                DateTime empDate;
                switch (type.WorkServMethod)
                {
                    case 2: // start experience date
                        empDate = context.People.Where(p => p.Id == EmpId).Select(p => p.StartExpDate).FirstOrDefault().Value;
                        break;
                    case 3: // last employment date
                        empDate = context.Employements.Where(e => e.EmpId == EmpId && e.Status == 1).Select(e => e.StartDate).FirstOrDefault();
                        break;
                    case 4: // last assignment date
                        empDate = context.Assignments.Where(a => a.EmpId == EmpId && (a.AssignDate <= today && a.EndDate >= today)).Select(a => a.AssignDate).FirstOrDefault();
                        break;
                    default: // From Join date is default
                        empDate = context.People.Where(p => p.Id == EmpId).Select(p => p.JoinDate).FirstOrDefault().GetValueOrDefault();
                        break;
                }

                empDate = empDate.AddMonths(type.WaitingMonth.Value);
                if (StartDate.Date.CompareTo(empDate.Date) < 0)
                    message.Append(MsgUtils.Instance.Trls(culture, "WaitingMonthes") + " " + empDate.ToString("yyyy/MM/dd") + "<br>");
            }

            //Has Request
            var prevReqs = HavePervRequests(new List<int> { EmpId }, RequestId, StartDate, EndDate, companyId);
            if (prevReqs.Count > 0)
                message.Append(MsgUtils.Instance.Trls(culture, "DoublicateStartDate") + "<br>");


            //Allowed Dayes and Balance
            int periodId = GetLeaveBalance(ref requestVal, new ReqDaysParamVM { type = type, EmpId = EmpId, RequestId = RequestId, StartDate = StartDate, culture = culture });

            #region  Dept Leave Plan -- Stars & Percent
            if (type.IncLeavePlan)
            {
                GetStarsParamVM param = new GetStarsParamVM { EmpId = EmpId, StartDate = StartDate, EndDate = EndDate, RequestId = RequestId, ExDayOff = type.ExDayOff, ExHolidays = type.ExHolidays, PeriodId = periodId, ComapnyId = companyId };
                int Stars, EmpStars;
                List<string> errorMsgs = CheckLeavePlan(param, culture, out Stars, out EmpStars);

                requestVal.Stars = Stars;
                requestVal.EmpStars = EmpStars;
                if (errorMsgs.Count > 0)
                {
                    if (isSSUser) message.Append(String.Join(" <br>", errorMsgs));
                    else requestVal.Warning = String.Join(" <br>", errorMsgs);
                }
            }
            #endregion

            // for edit in leave operationd
            int approvalStatus = 1;
            if (RequestId > 0)
            {
                approvalStatus = context.LeaveRequests.Where(a => a.Id == RequestId).Select(a => a.ApprovalStatus).FirstOrDefault();
                if (approvalStatus == 0) approvalStatus = 1;
            }
            
            if (approvalStatus < 6)
            {
                if (NofDays > requestVal.AllowedDays) message.Append(MsgUtils.Instance.Trls(culture, "AllowedDays") + "<br>");
                if (NofDays > type.MaxDays) message.Append(MsgUtils.Instance.Trls(culture, "CantGreaterThan") + " " + type.MaxDays + "<br>");
            }

            requestVal.BalAfter = requestVal.BalBefore.GetValueOrDefault() - NofDays;
            requestVal.Message = message.Length > 0 ? message.ToString().Substring(0, message.ToString().Length - 4) : "";

            return requestVal;
        }

        public LeavePlanStarsVM GetStars(GetStarsParamVM param)
        {
            LeavePlanStarsVM result = new LeavePlanStarsVM();
            DateTime StartDate = param.StartDate, EndDate = param.EndDate;

            List<DeptLeavePlanViewModel> leavePlans =
                (from p in context.DeptJobLeavePlans
                 where p.CompanyId == param.ComapnyId &&
                    (p.FromDate <= StartDate && p.ToDate >= StartDate || p.FromDate <= EndDate && p.ToDate >= EndDate
                    || StartDate <= p.FromDate && EndDate >= p.FromDate || StartDate <= p.ToDate && EndDate >= p.ToDate)
                 join a in context.Assignments on new { a1 = p.DeptId, a2 = p.JobId } equals new { a1 = a.DepartmentId, a2 = a.JobId }
                 where a.EmpId == param.EmpId && a.AssignDate <= StartDate && a.EndDate >= StartDate
                 select new DeptLeavePlanViewModel
                 {
                     DeptId = a.DepartmentId,
                     JobId = a.JobId,
                     FromDate = p.FromDate,
                     ToDate = p.ToDate,
                     Stars = p.Stars,
                     MinAllowPercent = p.MinAllowPercent
                 }).ToList();

            result.Stars = 0;

            if (leavePlans.Count > 0)
            {
                var holidays = GetHolidays(param.ComapnyId);

                for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
                {
                    var plan = leavePlans.Where(s => s.FromDate <= date && s.ToDate >= date).FirstOrDefault();
                    if (plan != null)
                    {
                        if (param.ExDayOff && (holidays.weekend1 == (byte)date.DayOfWeek || holidays.weekend2 == (byte)date.DayOfWeek))
                            continue;
                        if (param.ExHolidays && (holidays.CustomHolidays.Contains(date) || (holidays.StanderdHolidays.Where(s => s.SDay == date.Day && s.SMonth == date.Month)).Count() > 0))
                            continue;

                        result.Stars += plan.Stars;
                    }
                }

                result.Plans = leavePlans.Where(a => a.MinAllowPercent != 0).ToList();
                result.DeptId = leavePlans.FirstOrDefault().DeptId;
                result.JobId = leavePlans.FirstOrDefault().JobId;
            }

            result.EmpStars = context.LeaveRequests.Where(r => r.Id != param.RequestId && r.EmpId == param.EmpId && r.PeriodId == param.PeriodId && r.ApprovalStatus <= 6 && r.Stars != null && r.Stars > 0).Sum(r => r.Stars) ?? 0;

            return result;
        }

        public List<string> CheckLeavePlan(GetStarsParamVM param, string culture, out int Stars, out int EmpStars)
        {
            List<string> errorMsg = new List<string>();
            var leavePlans = GetStars(param);

            Stars = leavePlans.Stars;
            EmpStars = Stars + leavePlans.EmpStars;

            if (Stars > 0 && EmpStars > 5)
            {
                errorMsg.Add(MsgUtils.Instance.Trls(culture, "UnavilableStars"));
            }

            ///Dpet Job Percentage
            // leavePlans.Plans -> contains leave plan records where MinAllowPercent != 0
            if (leavePlans.Plans != null)
            {
                if (leavePlans.Plans.Where(a => a.MinAllowPercent == 100).Count() > 0)
                {
                    errorMsg.Add(MsgUtils.Instance.Trls(culture, "LeavePlanPreventRequests"));
                }
                else
                {
                    foreach (var plan in leavePlans.Plans)
                    {
                        //active emloyees in request period join approved requests in plan period
                        var sdate = (plan.FromDate <= param.StartDate ? param.StartDate : plan.FromDate);
                        var edate = (plan.ToDate >= param.EndDate ? param.EndDate : plan.ToDate);
                        var MinCapacity = plan.MinAllowPercent;
                        var query = (from a in context.Assignments
                                      where a.CompanyId == param.ComapnyId && a.AssignDate <= param.StartDate && a.EndDate >= param.StartDate && a.DepartmentId == leavePlans.DeptId && a.JobId == leavePlans.JobId && a.SysAssignStatus == 1
                                      join r in context.LeaveRequests on a.EmpId equals r.EmpId into lj
                                      from r in lj.Where(l => l.Id != param.RequestId && (l.ApprovalStatus == 5 || l.ApprovalStatus == 6) && (((l.ActualStartDate ?? l.StartDate) >= sdate && (l.ActualStartDate ?? l.StartDate) <= edate) || ((l.ActualEndDate ?? l.EndDate) >= sdate && (l.ActualEndDate ?? l.EndDate) <= edate) || ((l.ActualStartDate ?? l.StartDate) <= sdate && (l.ActualEndDate ?? l.EndDate) >= sdate) || ((l.ActualStartDate ?? l.StartDate) <= edate && (l.ActualEndDate ?? l.EndDate) >= edate))).DefaultIfEmpty()
                                      select new { a.DepartmentId, a.JobId, ActiveEmps = a.EmpId, LeaveReq = r == null ? 0 : 1 } into gb
                                      group gb by new { gb.DepartmentId, gb.JobId } into g
                                      select new { sum = (g.Sum(p => p.LeaveReq)), count = g.Count() });

                        var actualCount = query.FirstOrDefault();
                        float CurrentCapacity = (1 - (float)(actualCount.sum + 1) / (float)actualCount.count) * 100;  // (LeavesCount * 100) / TotalCount 
                        if (CurrentCapacity < MinCapacity)
                        {
                            errorMsg.Add(MsgUtils.Instance.Trls(culture, "UnavilablePercent") + (100 - plan.MinAllowPercent) + "%");
                        }
                    }
                }
            }

            return errorMsg;
        }

        public int GetLeaveBalance(ref RequestValidationViewModel requestVal, ReqDaysParamVM param)
        {
            //Allowed Days
            int PeriodId;
            string msg;
            var type = param.type;
            float AllowedDays, balance = 0, ReservedDays = 0;
            PeriodId = GetLeaveRequestPeriod(type.CalendarId, param.StartDate, param.culture, out msg);
            if (msg != "OK") // only check valid period
            {
                requestVal.Message = msg;
                return 0;
            }

            ReservedDays = GetReservedDays(type.Id, PeriodId, new List<int> { param.EmpId }, param.RequestId).Where(r => r.EmpId == param.EmpId).Select(r => r.ReservedDays).FirstOrDefault() ?? 0;
            AllowedDays = GetMaxAllowedDays(type.Id, type.AbsenceType, PeriodId, param.EmpId, type.AllowNegBal, type.Percentage, type.MaxDaysInPeriod, out balance) - ReservedDays;

            requestVal.ReservedDays = ReservedDays;
            requestVal.AllowedDays = AllowedDays;
            requestVal.BalBefore = balance;
            //Add by Abdelazim
            requestVal.EmpId = param.EmpId;

            return PeriodId;
        }

        public IQueryable GetLeaveTypesList(int companyId, string culture)
        {
            return context.LeaveTypes.Where(l => (l.CompanyId == companyId) || l.IsLocal == false)
                .Select(l => new { id = l.Id, name = HrContext.TrlsName(l.Name, culture), depts = l.CompanyStuctures, isActive = (l.StartDate <= DateTime.Today && (l.EndDate == null || l.EndDate >= DateTime.Today)) });
        }

        public CalenderViewModel GetHolidays(int compId)
        {
            var personnel = GetPersonSetup(compId);
            var standard = ReadStanderedHolidays(compId).ToList();
            var custom = ReadCustomHolidays(compId).Where(c => c.HoliDate != null).Select(c => c.HoliDate.Value).ToList();

            CalenderViewModel calender = new CalenderViewModel() { CustomHolidays = new List<DateTime>(), StanderdHolidays = new List<HolidayViewModel>() };
            calender.weekend1 = personnel?.Weekend1;
            calender.weekend2 = personnel?.Weekend2;
            calender.WorkStartTime = personnel?.StartTime;

            if (personnel?.Frequency == 1) //Day
                calender.WorkHours = personnel?.WorkHours;
            else if (personnel?.StartTime != null && personnel?.EndTime != null)
                calender.WorkHours = Convert.ToSByte((personnel.EndTime - personnel.StartTime).Value.Hours);
            else
                calender.WorkHours = 8;

            foreach (var item in standard)
            {
                calender.StanderdHolidays.Add(item);
            }

            foreach (DateTime item in custom)
            {
                calender.CustomHolidays.Add(item);
            }
            return calender;
        }

        public List<RequestValidationViewModel> HavePervRequests(List<int> EmpIds, int Id, DateTime StartDate, DateTime EndDate, int companyId, bool isReplace = false)
        {
            StartDate = StartDate.Date;
            EndDate = EndDate.Date;

            return context.LeaveRequests
                            .Where(r => r.Id != Id && EmpIds.Contains(r.EmpId)
                                && (r.ApprovalStatus < 6 && (isReplace ? r.ApprovalStatus != 1 : true) && (r.StartDate <= StartDate && r.EndDate >= StartDate || r.StartDate <= EndDate && r.EndDate >= EndDate || StartDate <= r.StartDate && EndDate >= r.StartDate || StartDate <= r.EndDate && EndDate >= r.EndDate)
                               || (r.ApprovalStatus == 6 && (r.ActualStartDate <= StartDate && r.ActualEndDate >= StartDate || r.ActualStartDate <= EndDate && r.ActualEndDate >= EndDate || StartDate <= r.ActualStartDate && EndDate >= r.ActualStartDate || StartDate <= r.ActualEndDate && EndDate >= r.ActualEndDate))
                               ))
                            .Select(r => new RequestValidationViewModel { EmpId = r.EmpId })
                        .ToList();
        }

        public List<string> IsReplacement(int EmpId, DateTime StartDate, DateTime EndDate, int CompanyId, string culture)
        {
            return (from r in context.LeaveRequests
                    where r.CompanyId == CompanyId && r.ReplaceEmpId == EmpId
                    && (r.ApprovalStatus < 6 && r.ApprovalStatus != 1 && (r.StartDate <= StartDate && r.EndDate >= StartDate || r.StartDate <= EndDate && r.EndDate >= EndDate || StartDate <= r.StartDate && EndDate >= r.StartDate || StartDate <= r.EndDate && EndDate >= r.EndDate)
                    || (r.ApprovalStatus == 6 && (r.ActualStartDate <= StartDate && r.ActualEndDate >= StartDate || r.ActualStartDate <= EndDate && r.ActualEndDate >= EndDate || StartDate <= r.ActualStartDate && EndDate >= r.ActualStartDate || StartDate <= r.ActualEndDate && EndDate >= r.ActualEndDate)))
                    join p in context.People on r.EmpId equals p.Id
                    select HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture)).ToList();
        }

        //Replacement Employee
        public IQueryable<FormList> GetReplaceEmpList(int empId, string culture)
        {
            string active = " And Convert(date, GETDATE()) between A.AssignDate And A.EndDate AND A.SysAssignStatus = 1 ";
            string sql = "SELECT P.Id id, dbo.fn_TrlsName(ISNULL(P.Title, '') + ' ' + P.FirstName + ' ' + P.Familyname, '" + culture + "') name, [dbo].[fn_GetDoc]('EmployeePic', p.Id) PicUrl, p.Gender, dbo.fn_GetEmpStatus(P.Id) Icon FROM Assignments A, People P WHERE A.EmpId != " + empId + " And A.EmpId = P.Id " + active + " And A.DepartmentId In (SELECT RD.Id FROM Assignments A, CompanyStructures ED, CompanyStructures RD WHERE A.EmpId = " + empId + " And A.DepartmentId = ED.Id And (ED.ParentId = RD.Id OR RD.Sort LIKE (SUBSTRING(ED.Sort, 0, Len(ED.Sort) - 4)) + '_____' ))";

            return context.Database.SqlQuery<FormList>(sql).AsQueryable();
        }

        //EmpTasks Grid
        public IQueryable<EmpTasksViewModel> ReadLeaveEmpTasks(int EmpId, int subPeriodId, string culture)
        {
            var query = context.EmpTasks.Where(t => t.EmpId == EmpId && t.SubPeriodId == subPeriodId).Select(t => new EmpTasksViewModel
            {
                Id = t.Id,
                TaskNo = t.TaskNo,
                AssignedTime = t.AssignedTime,
                Status = t.Status,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                Priority = t.Priority,
                TaskCategory = HrContext.GetLookUpCode("EmpTaskCat", t.TaskCat.Value, culture),
            });

            return query;
        }

        //Prev Leaves Grid
        public IQueryable<LeaveTransViewModel> ReadEmpLeaveTrans(int empId, DateTime startDate, int companyId, string culture)
        {
            //var query = from t in context.LeaveTrans
            //            where t.CompanyId == companyId && t.EmpId == empId && t.Period.StartDate <= startDate && t.Period.EndDate >= startDate && t.LeaveType.HasAccrualPlan
            //            orderby t.TransDate, t.TransType
            //            select new LeaveTransViewModel
            //            {
            //                Id = t.Id,
            //                CreditQty = (t.TransFlag == 1 ? t.TransQty : 0),
            //                DebitQty = (t.TransFlag == -1 ? t.TransQty : 0),
            //                LeaveType = HrContext.TrlsName(t.LeaveType.Name, culture),
            //                Period = t.Period.Name,
            //                TransDate = t.TransDate,
            //                TransType = t.TransType,
            //                TransTypeName = HrContext.GetLookUpCode("TransType", t.TransType, culture),
            //                Balance = t.TransQty * t.TransFlag
            //            };
            //return query;
            string sql = $"SELECT LT.Id, dbo.fn_TrlsName(L.Name, '{culture}') LeaveType, LT.TransType, LT.TransDate, (CASE WHEN LT.TransFlag = 1 THEN LT.TransQty ELSE 0 END) CreditQty, (CASE WHEN LT.TransFlag = -1 THEN LT.TransQty ELSE 0 END) DebitQty, CAST(SUM(LT.TransQty * LT.TransFlag) OVER(PARTITION BY LT.typeid ORDER BY LT.TransDate, LT.TransType, LT.Id) AS real) Balance, dbo.fn_GetLookUpCode('TransType', LT.TransType, '{culture}') TransTypeName FROM LeaveTrans LT, [Periods] P, LeaveTypes L WHERE LT.CompanyId = {companyId} AND LT.EmpId = {empId} AND LT.TypeId = L.Id AND P.Id = LT.PeriodId AND '{startDate.Date.ToString("yyyy-MM-dd")}' BETWEEN P.StartDate and P.EndDate";
            return context.Database.SqlQuery<LeaveTransViewModel>(sql).AsQueryable();
        }

        ////ProgressBar
        public IEnumerable<LeaveTransCounts> AnnualLeavesProgress(int empId, DateTime startDate, string culture)
        {
            var query = (from t in context.LeaveTrans
                         where t.EmpId == empId
                         join p in context.Periods on t.PeriodId equals p.Id
                         where startDate >= p.StartDate && startDate <= p.EndDate
                         join l in context.LeaveTypes on t.TypeId equals l.Id
                         where l.HasAccrualPlan
                         select new
                         {
                             AbsenceCode = l.AbsenceType,
                             Balance = (t == null ? 0 : t.TransFlag * t.TransQty),
                             //MaxDays = l.HasAccrualPlan ? 0 : (l.MaxDaysInPeriod ?? 0), //Not HasAccrualPlan Blance
                             TotalDays = (t == null ? 0 : (t.TransFlag == -1 || t.TransType == 5 ? t.TransQty * t.TransFlag * -1 : 0)),
                             //HasAccrualPlan = l.HasAccrualPlan,
                             TypeId = l.Id,
                             LeaveName = HrContext.TrlsName(l.Name, culture),
                             CalendarId = l.CalendarId
                         }).GroupBy(d => new { d.TypeId }).Select(g => new LeaveTransCounts
                         {
                             TypeId = g.Key.TypeId,
                             Name = g.FirstOrDefault().LeaveName,
                             Balance = g.Sum(s => s.Balance),
                             Days = g.Sum(s => s.TotalDays), //Days = leave days
                             //HasAccrualPlan = g.FirstOrDefault().HasAccrualPlan,
                         }).ToList();

            return query;
        }

        #endregion

        #region GroupLeave
        private IEnumerable<RequestValidationViewModel> EmpsMaxAllowedDays(int leaveType, short absenceType, int periodId, List<int> empIds, bool AllowNegBal, float? Percentage, short? MaxDaysInPeriod, int companyId)
        {
            var query = (from e in context.People
                         where empIds.Contains(e.Id)
                         join t in context.LeaveTrans on e.Id equals t.EmpId into g1
                         from t in g1.Where(t => t.CompanyId == companyId && t.TypeId == leaveType && t.PeriodId == periodId).DefaultIfEmpty()
                         join l in context.LeaveTypes on t.TypeId equals l.Id into g2
                         from l in g2.DefaultIfEmpty()
                         select new
                         {
                             EmpId = e.Id,
                             Balance = t == null ? 0 : t.TransFlag * t.TransQty,
                             MaxDays = l == null ? (MaxDaysInPeriod ?? 0) : (l.HasAccrualPlan ? 0 : (l.MaxDaysInPeriod ?? 0)),
                             HasAccrualPlan = l.HasAccrualPlan,
                             TypeId = leaveType,
                         }).GroupBy(g => new { g.EmpId, g.TypeId, g.MaxDays }).Select(g => new RequestValidationViewModel
                         {
                             EmpId = g.Key.EmpId,
                             AllowedDays = g.Sum(s => s.Balance) + g.Key.MaxDays
                         }).ToList();

            if (AllowNegBal && Percentage != null)
            {
                var addBalance = context.LeaveTrans
                .Where(a => a.TypeId == leaveType && a.PeriodId == periodId && empIds.Contains(a.EmpId) && a.TransType == 2) //2. Accrual Balance استحقاق الرصيد
                .Select(a => new { EmpId = a.EmpId, addBalance = a.TransQty })
                .GroupBy(a => a.EmpId).Select(a => new { EmpId = a.Key, Balance = (a.Sum(s => s.addBalance) * Percentage.Value) });

                return query.Select(a => new RequestValidationViewModel { EmpId = a.EmpId, AllowedDays = a.AllowedDays + (addBalance.Where(b => b.EmpId == a.EmpId).Select(b => b.Balance).FirstOrDefault()) }).ToList();
            }

            return query;
        }

        public List<ErrorMessage> CheckGroupLeave(LeaveReqViewModel request, LeaveType type, string culture, out int PeriodId)
        {
            string msg;
            List<ErrorMessage> errors = new List<ErrorMessage>();

            PeriodId = GetLeaveRequestPeriod(type.CalendarId, request.StartDate, culture, out msg);
            if (msg != "OK") // only check valid period
                errors.Add(new ErrorMessage { field = "StartDate", message = msg });

            if (request.NofDays > type.MaxDays)
                errors.Add(new ErrorMessage { field = "NofDays", message = MsgUtils.Instance.Trls(culture, "CantGreaterThan") + " " + type.MaxDays });

            if (type.MustAddCause && request.ReqReason == null)
                errors.Add(new ErrorMessage { field = "ReqReason", message = MsgUtils.Instance.Trls(culture, "Required") });

            return errors;
        }
        public IQueryable<GroupLeaveViewModel> CheckGroupLeaveGrid(LeaveReqViewModel request, int[] Depts, int companyId, string culture, out List<ErrorMessage> errors)
        {
            if (Depts == null) Depts = new int[0];

            List<FormList> empList = (from a in context.Assignments
                                      where a.CompanyId == companyId && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today) && Depts.Contains(a.DepartmentId)
                                      join p in context.People on a.EmpId equals p.Id
                                      select new FormList { id = p.Id, name = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture) })
                                 .ToList();

            List<int> EmpIds = empList.Select(e => e.id).ToList();

            List<GroupLeaveViewModel> Grid = new List<GroupLeaveViewModel>();

            LeaveType type = GetLeaveType(request.TypeId);
            int PeriodId;
            errors = CheckGroupLeave(request, type, culture, out PeriodId);
            if (errors.Count > 0) return null;

            ///1.Check if any Employee not have Balance
            var ReservedDays = GetReservedDays(request.TypeId, PeriodId, EmpIds, 0);
            var AllowedDays = EmpsMaxAllowedDays(request.TypeId, type.AbsenceType, PeriodId, EmpIds, type.AllowNegBal, type.Percentage, type.MaxDaysInPeriod, companyId);

            ///2.Check if any Employee have request
            var pervReqs = HavePervRequests(EmpIds, 0, request.StartDate, request.EndDate, companyId);

            foreach (var item in empList)
            {
                string reason = "";
                byte reasonCode = 0; //0. success, 1.balance, 2.have Request, 3.balance & have Request
                bool valid = true;

                ///1.
                var balance = AllowedDays.Where(b => b.EmpId == item.id).Select(b => b.AllowedDays - (ReservedDays.Where(r => r.EmpId == item.id).Select(r => r.ReservedDays).FirstOrDefault() ?? 0)).FirstOrDefault();
                if (request.NofDays > balance) { valid = false; reasonCode = 1; reason += MsgUtils.Instance.Trls(culture, "AllowedDays"); }

                ///2.
                var empPervReqs = pervReqs.Where(r => r.EmpId == item.id);
                if (empPervReqs.Count() > 0) { valid = false; reasonCode = 2; reason += (reason.Length > 0 ? ", " : "") + MsgUtils.Instance.Trls(culture, "DoublicateStartDate"); }

                if (empPervReqs.Count() > 0 && request.NofDays > balance) reasonCode = 3;

                Grid.Add(new GroupLeaveViewModel
                {
                    Id = item.id,
                    EmpId = item.id,
                    Approve = valid,
                    Status = (valid ? MsgUtils.Instance.Trls(culture, "Success") : MsgUtils.Instance.Trls(culture, "Faild")),
                    Reason = reason,
                    Employee = item.name,
                    ReasonCode = reasonCode,
                    Success = valid,
                    NofErrors = valid ? 0 : 1
                });
            }

            return Grid.Select(g => new GroupLeaveViewModel { Id = g.Id, EmpId = g.EmpId, ReasonCode = g.ReasonCode, Success = g.Success, Approve = g.Approve, Status = g.Status, Reason = g.Reason, Employee = g.Employee, NofErrors = Grid.Select(s => s.NofErrors).Sum() }).AsQueryable();
        }

        public void Add(GroupLeave GroupLeave)
        {
            context.GroupLeaves.Add(GroupLeave);
        }
        public void Add(GroupLeaveLog GroupLeaveLog)
        {
            context.GroupLeaveLogs.Add(GroupLeaveLog);
        }

        #endregion

        #region Upcoming 
        public IEnumerable<HolidayViewModel> GetUpcomingHolidays(int companyId)
        {
            var query = context.Holidays.Where(h => (!h.IsLocal || (h.IsLocal && h.CompanyId == companyId))
            && (h.HoliDate >= DateTime.Today || (h.SMonth > DateTime.Today.Month || (h.SMonth == DateTime.Today.Month && h.SDay >= DateTime.Today.Day))))
            .Select(h => new
            {
                Id = h.Id,
                Name = h.Name,
                SDay = h.SDay,
                SMonth = h.SMonth,
                HoliDate = h.HoliDate,
                Standard = h.Standard
            })
            .ToList();

            return query.Select(h => new HolidayViewModel
            {
                Id = h.Id,
                Name = h.Name,
                HoliDate = h.Standard ? new DateTime(DateTime.Today.Year, h.SMonth.Value, h.SDay.Value) : h.HoliDate,
                Standard = h.Standard
            })
            .OrderBy(h => h.HoliDate);
        }

        public IEnumerable GetUpcomingLeaves(int companyId, string culture)
        {
            DateTime Tomorrow = DateTime.Today.AddDays(1).Date;
            var query = (from l in context.LeaveRequests
                         where l.CompanyId == companyId && l.ApprovalStatus == 6 && (l.ActualStartDate <= DateTime.Today && l.ActualEndDate >= DateTime.Today)
                         join p in context.People on l.EmpId equals p.Id
                         select new
                         {
                             Id = l.Id,
                             Day = 1, //Today
                             EmpId = l.EmpId,
                             Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             Image = HrContext.GetDoc("EmployeePic", l.Id),
                             Gender = p.Gender,
                             LeaveType = HrContext.TrlsName(l.LeaveType.Name, culture),
                             NofDays = l.NofDays,
                             ActualStartDate = l.ActualStartDate
                         }).Union(from l in context.LeaveRequests
                                  where l.CompanyId == companyId && l.ApprovalStatus == 6 && (l.ActualStartDate <= Tomorrow && l.ActualEndDate >= Tomorrow)
                                  join p in context.People on l.EmpId equals p.Id
                                  select new
                                  {
                                      Id = l.Id,
                                      Day = 2, //Tomorrow
                                      EmpId = l.EmpId,
                                      Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                                      Image = HrContext.GetDoc("EmployeePic", l.Id),
                                      Gender = p.Gender,
                                      LeaveType = HrContext.TrlsName(l.LeaveType.Name, culture),
                                      NofDays = l.NofDays,
                                      ActualStartDate = l.ActualStartDate
                                  }).OrderBy(l => l.ActualStartDate).ToList();

            return query;
        }
        #endregion

        #region Holidays
        public IQueryable<HolidayViewModel> ReadStanderedHolidays(int companyId)
        {
            return context.Holidays.Where(h => h.Standard && (!h.IsLocal || h.IsLocal && h.CompanyId == companyId)).Select(h =>
                new HolidayViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    IsLocal = h.IsLocal,
                    SDay = h.SDay,
                    SMonth = h.SMonth,
                    CompanyId = h.CompanyId,
                    Standard = h.Standard,
                    CreatedTime = h.CreatedTime,
                    CreatedUser = h.CreatedUser,
                    ModifiedTime = h.ModifiedTime,
                    ModifiedUser = h.ModifiedUser
                });
        }

        public IQueryable<HolidayViewModel> ReadCustomHolidays(int companyId)
        {
            return context.Holidays.Where(h => !h.Standard && (!h.IsLocal || h.IsLocal && h.CompanyId == companyId)).Select(h =>
                 new HolidayViewModel
                 {
                     Id = h.Id,
                     Name = h.Name,
                     IsLocal = h.IsLocal,
                     HoliDate = h.HoliDate,
                     CompanyId = h.CompanyId,
                     Standard = h.Standard,
                     CreatedTime = h.CreatedTime,
                     CreatedUser = h.CreatedUser,
                     ModifiedTime = h.ModifiedTime,
                     ModifiedUser = h.ModifiedUser
                 });
        }

        public void RemoveLeave(LeaveRequest model)
        {
            if (model.ApprovalStatus == 1)
                context.LeaveRequests.Remove(model);
            else
                context.LeaveRequests.Attach(model);
        }
        public void Add(Holiday holiday)
        {
            context.Holidays.Add(holiday);
        }

        public void Attach(Holiday holiday)
        {
            context.Holidays.Attach(holiday);
        }

        public DbEntityEntry<Holiday> Entry(Holiday holiday)
        {
            return Context.Entry(holiday);
        }

        public void Remove(Holiday holiday)
        {
            if (Context.Entry(holiday).State == EntityState.Detached)
            {
                context.Holidays.Attach(holiday);
            }
            context.Holidays.Remove(holiday);
        }

        #endregion

        #region LeaveTypes
        public LeaveTypeFormViewModel ReadleaveType(int Id, string culture)
        {
            var obj = (from a in context.LeaveTypes
                       where a.Id == Id
                       join c in context.PeriodNames on a.CalendarId equals c.Id into g
                       from c in g.DefaultIfEmpty()
                       select new { a, c, LocalName = HrContext.TrlsName(a.Name, culture) }).FirstOrDefault();

            var mod = new LeaveTypeFormViewModel()
            {
                Id = obj.a.Id,
                AbsenceType = obj.a.AbsenceType,
                Name = obj.a.Name,
                Religion = obj.a.Religion,
                AccBalDays = obj.a.AccBalDays,
                Age50 = obj.a.Age50,
                AllowFraction = obj.a.AllowFraction,
                Balanace50 = obj.a.Balanace50,
                Code = obj.a.Code,
                CompanyId = obj.a.CompanyId,
                DiffDaysOpt = obj.a.DiffDaysOpt,
                EndDate = obj.a.EndDate,
                ExDayOff = obj.a.ExDayOff,
                ExHolidays = obj.a.ExHolidays,
                IncLeavePlan = obj.a.IncLeavePlan,
                Percentage = obj.a.Percentage * 100,
                Gender = obj.a.Gender,
                MaritalStat = obj.a.MaritalStat,
                MaxDays = obj.a.MaxDays,
                MaxNofDays = obj.a.MaxNofDays,
                MilitaryStat = obj.a.MilitaryStat,
                IsLocal = obj.a.IsLocal,
                MustAddCause = obj.a.MustAddCause,
                Nationality = obj.a.Nationality,
                NofDays = obj.a.NofDays,
                NofDays50 = obj.a.NofDays50,
                AllowNegBal = obj.a.AllowNegBal,
                PostOpt = obj.a.PostOpt,
                StartDate = obj.a.StartDate,
                //StartDay = obj.a.StartDay,
                //StartMonth = obj.a.StartMonth,
                VerifyFraction = obj.a.VerifyFraction,
                WaitingMonth = obj.a.WaitingMonth,
                // YearStartDt = obj.a.YearStartDt,
                LocalName = obj.LocalName,
                FrequencyId = obj.a.HasAccrualPlan ? obj.c.SubPeriodCount : (byte)0,
                OpenCalendarId = obj.a.HasAccrualPlan ? 0 : obj.a.CalendarId,
                CalendarId = obj.a.CalendarId,
                HasAccrualPlan = obj.a.HasAccrualPlan,
                MaxDaysInPeriod = obj.a.MaxDaysInPeriod,
                CreatedTime = obj.a.CreatedTime,
                CreatedUser = obj.a.CreatedUser,
                ModifiedTime = obj.a.ModifiedTime,
                ModifiedUser = obj.a.ModifiedUser,
                AssignStatus = obj.a.AssignStatus,
                ChangAssignStat = obj.a.ChangAssignStat,
                AutoChangStat = obj.a.AutoChangStat,
                MaxPercent = obj.a.MaxPercent * 100,
                IncludContinu = obj.a.IncludContinu,
                MinLeaveDays = obj.a.MinLeaveDays,
                WorkServMethod = obj.a.WorkServMethod,
                PercentOfActive = obj.a.PercentOfActive,
                MonthOrYear = obj.a.MonthOrYear,
                ExWorkflow = obj.a.ExWorkflow,
                IPayrolls = obj.a.Payrolls == null ? null : obj.a.Payrolls.Split(',').Select(int.Parse).ToList(),
                ICompanyStuctures = obj.a.CompanyStuctures == null ? null : obj.a.CompanyStuctures.Split(',').Select(int.Parse).ToList(),
                IEmployments = obj.a.Employments == null ? null : obj.a.Employments.Split(',').Select(int.Parse).ToList(),
                IPayrollGrades = obj.a.PayrollGrades == null ? null : obj.a.PayrollGrades.Split(',').Select(int.Parse).ToList(),
                IPositions = obj.a.Positions == null ? null : obj.a.Positions.Split(',').Select(int.Parse).ToList(),
                IBranches = obj.a.Branches == null ? null : obj.a.Branches.Split(',').Select(int.Parse).ToList(),
                IJobs = obj.a.Jobs == null ? null : obj.a.Jobs.Split(',').Select(int.Parse).ToList(),
                IPeopleGroups = obj.a.PeopleGroups == null ? null : obj.a.PeopleGroups.Split(',').Select(int.Parse).ToList()
            };
            return mod;
        }
        public IQueryable<WfRoleViewModel> GetWfRole(int RId)
        {
            var result = from R in context.WfRoles
                         where R.WFlowId == RId
                         select new WfRoleViewModel
                         {
                             Id = R.Id,
                             CodeId = R.CodeId,
                             RoleId = R.RoleId,
                             Role = (R.RoleId == null ? R.CodeId.ToString() : R.RoleId.ToString()),
                             Order = R.Order,
                             WFlowId = R.WFlowId
                         };
            return result;
        }
        #region WorkFlow Grid Objects

        //read grid 
        public IEnumerable<WorkFlowObjectsViewModel> ReadWorkFlowObjects(int companyId,string culture)
        {
            var query = from l in context.LookUpCodes
                        where l.CodeName == "WorkFlow"
                        join w in context.Workflows on l.Name equals w.Source into g
                        from j in g.Where(a=>a.CompanyId == companyId).DefaultIfEmpty()
                        select new WorkFlowObjectsViewModel()
                        {
                            IsRequired = j == null ? true : j.IsRequired,
                            DbSource = HrContext.GetLookUpCode(l.CodeName,l.CodeId,culture),
                            Source = l.Name,
                            Id = j == null ? 0 : j.Id ,
                            CompanyId = companyId                           
                        };                     
            return query.ToList();
        }


        #endregion

        public void Add(RequestWf requestWf)
        {
            context.RequestWf.Add(requestWf);
        }
        public void Attach(RequestWf requestWf)
        {
            context.RequestWf.Attach(requestWf);
        }
        public DbEntityEntry<RequestWf> Entry(RequestWf requestWf)
        {
            return Context.Entry(requestWf);
        }
        public void Remove(RequestWf requestWf)
        {
            if (Context.Entry(requestWf).State == EntityState.Detached)
            {
                context.RequestWf.Attach(requestWf);
            }
            context.RequestWf.Remove(requestWf);
        }

        public IQueryable<LeaveTypeViewModel> GetLeaveTypes(int companyId, string culture)
        {
            // .Where(a => ((a.IsLocal && a.CompanyId == companyId) || a.IsLocal == false) && (a.StartDate <= DateTime.Today && (a.EndDate >= DateTime.Today || a.EndDate == null)));
            var result = from l in context.LeaveTypes
                         where ((l.IsLocal && l.CompanyId == companyId) || l.IsLocal == false) && (l.StartDate <= DateTime.Today && (l.EndDate == null || l.EndDate >= DateTime.Today))
                         select new LeaveTypeViewModel
                         {
                             Id = l.Id,
                             Name = l.Name,
                             Code = l.Code,
                             LocalName = HrContext.TrlsName(l.Name, culture),
                             StartDate = l.StartDate,
                             EndDate = l.EndDate,
                             CompanyId = l.CompanyId,
                             IsLocal = l.IsLocal,
                             HasAccrualPlan = l.HasAccrualPlan,
                             AbsenceType = l.AbsenceType
                         };
            return result;
        }

        public LeaveType GetLeaveType(int? id)
        {
            return context.LeaveTypes.Find(id);
        }

        public Period GetLperiod(int? id)
        {
            return context.Periods.Find(id);
        }
        public void Remove(LeaveType leaveType)
        {
            var request = context.RequestWf.Where(q => q.SourceId == leaveType.Id).ToList();

            if (request.Count() >= 0)
            {
                foreach (var item in request)
                {
                    if (Context.Entry(item).State == EntityState.Detached)
                    {
                        context.RequestWf.Attach(item);
                    }
                    context.RequestWf.Remove(item);
                }
            }
            if (Context.Entry(leaveType).State == EntityState.Detached)
            {
                context.LeaveTypes.Attach(leaveType);
            }
            context.LeaveTypes.Remove(leaveType);



        }
        public IQueryable<ExcelGridLeaveRangesViewModel> GetLeaveRange(int leaveTypeId)
        {
            var result = from l in context.LeaveRanges
                         where l.LeaveTypeId == leaveTypeId
                         orderby l.FromPeriod
                         select new ExcelGridLeaveRangesViewModel
                         {
                             Id = l.Id,
                             FromPeriod = l.FromPeriod,
                             NofDays = l.NofDays,
                             ToPeriod = l.ToPeriod,
                             LeaveTypeId = l.LeaveTypeId,
                             CreatedTime = l.CreatedTime,
                             CreatedUser = l.CreatedUser,
                             ModifiedTime = l.ModifiedTime,
                             ModifiedUser = l.ModifiedUser
                         };
            return result;
        }
        public void Add(LeaveType LeaveType)
        {
            context.LeaveTypes.Add(LeaveType);
        }
        public void Attach(LeaveType LeaveType)
        {
            context.LeaveTypes.Attach(LeaveType);
        }
        public DbEntityEntry<LeaveType> Entry(LeaveType LeaveType)
        {
            return Context.Entry(LeaveType);
        }
        public void Add(LeaveRange LeaveRange)
        {
            context.LeaveRanges.Add(LeaveRange);
        }
        public void Attach(LeaveRange LeaveRange)
        {
            context.LeaveRanges.Attach(LeaveRange);
        }
        public DbEntityEntry<LeaveRange> Entry(LeaveRange LeaveRange)
        {
            return Context.Entry(LeaveRange);
        }
        public void Remove(LeaveRange leaveRange)
        {
            if (Context.Entry(leaveRange).State == EntityState.Detached)
            {
                context.LeaveRanges.Attach(leaveRange);
            }
            context.LeaveRanges.Remove(leaveRange);
        }
        #endregion

        #region Follow Up 
        public IQueryable<LeaveReqGridViewModel> GetLeaveReqFollowUp(int companyId, string culture)
        { // Where(w => w.CodeId != 1)
            var query = (from l in context.LeaveRequests
                         where l.CompanyId == companyId && l.ApprovalStatus > 1 && l.ApprovalStatus < 6
                         join p in context.People on l.EmpId equals p.Id
                         join lt in context.LeaveTypes on l.TypeId equals lt.Id
                         join wft in context.WF_TRANS on new { p1 = "Leave", p2 = l.TypeId, p3 = l.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                         from wft in g.DefaultIfEmpty()
                         join ap in context.People on wft.AuthEmp equals ap.Id into g1
                         from ap in g1.DefaultIfEmpty()
                         join apos in context.Positions on wft.AuthPosition equals apos.Id into g2
                         from apos in g2.DefaultIfEmpty()
                         join dep in context.CompanyStructures on wft.AuthDept equals dep.Id into g3
                         from dep in g3.DefaultIfEmpty()
                         join role in context.Roles on wft.RoleId equals role.Id into g4
                         from role in g4.DefaultIfEmpty()
                         select new LeaveReqGridViewModel
                         {
                             Id = l.Id,
                             RequestDate = l.RequestDate,
                             Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             LeaveType = HrContext.TrlsName(lt.Name, culture),
                             StartDate = l.StartDate,
                             EndDate = l.EndDate,
                             NofDays = l.NofDays,
                             ActualStartDate = l.ActualStartDate,
                             ActualEndDate = l.ActualEndDate,
                             ActualNofDays = l.ActualNofDays,
                             ApprovalStatus = l.ApprovalStatus.ToString(),
                             CompanyId = l.CompanyId,
                             ReqReason = HrContext.GetLookUpCode("LeaveReason", l.ReqReason.Value, culture),
                             EmpId = l.EmpId,
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
                             HasImage = p.HasImage,
                             EmpStars = context.LeaveRequests.Where(r => r.EmpId == l.EmpId && r.StartDate.Year == l.StartDate.Year && r.ApprovalStatus <= 6).Select(r => r.Stars).Sum(r => r) ?? 0,
                             Attachement = HrContext.GetDoc("EmployeePic", p.Id),
                             Gender = p.Gender
                         }).OrderBy(o => new { o.EmpStars, o.StartDate });

            return query;
        }

        public IQueryable<LeaveReqGridViewModel> GetApprovedLeaveReq(int companyId, string culture)
        {
            DateTime today = DateTime.Today;

            return from l in context.LeaveRequests
                   where l.CompanyId == companyId && l.ApprovalStatus == 6
                   join p in context.People on l.EmpId equals p.Id
                   join lt in context.LeaveTypes on l.TypeId equals lt.Id
                   select new LeaveReqGridViewModel
                   {
                       Id = l.Id,
                       RequestDate = l.RequestDate,
                       Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                       LeaveType = HrContext.TrlsName(lt.Name, culture),
                       StartDate = l.StartDate,
                       EndDate = l.EndDate,
                       NofDays = l.NofDays,
                       ActualStartDate = l.ActualStartDate,
                       ActualEndDate = l.ActualEndDate,
                       ActualNofDays = l.ActualNofDays,
                       ApprovalStatus = l.ApprovalStatus.ToString(),
                       CompanyId = l.CompanyId,
                       ReqReason = HrContext.GetLookUpCode("LeaveReason", l.ReqReason.Value, culture),
                       EmpId = l.EmpId,
                       HasImage = p.HasImage,
                       isStarted = today >= (l.ActualStartDate ?? l.StartDate),
                       isBreaked = l.EndDate != l.ActualEndDate && l.StartDate == l.ActualStartDate,
                       Gender = p.Gender,
                       Attachement = HrContext.GetDoc("EmployeePic", p.Id)
                   };
        }

        #endregion

        #region Leave Operations

        public LeaveOpViewModel GetLeaveOpReq(int id, string culture)
        {
            LeaveOpViewModel Request = (from req in context.LeaveRequests
                                        where req.Id == id
                                        join p in context.People on req.EmpId equals p.Id
                                        join l in context.LeaveTypes on req.TypeId equals l.Id
                                        select new LeaveOpViewModel()
                                        {
                                            Id = req.Id,
                                            CompanyId = req.CompanyId,
                                            RequestDate = req.RequestDate,
                                            EmpId = req.EmpId,
                                            Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                                            TypeId = req.TypeId,
                                            LeaveType = HrContext.TrlsName(l.Name, culture),
                                            EndDate = req.EndDate,
                                            NofDays = req.NofDays,
                                            StartDate = req.StartDate,
                                            ActualStartDate = req.ActualStartDate,
                                            ActualEndDate = req.ActualEndDate,
                                            //StartTolerance = req.StartTolerance,
                                            Stars = req.Stars,
                                            Attachments = HrContext.GetAttachments("LeaveRequest", req.Id),
                                            PeriodId = req.PeriodId,
                                            Period = req.Period,
                                            CreatedTime = req.CreatedTime,
                                            CreatedUser = req.CreatedUser,
                                            ModifiedTime = req.ModifiedTime,
                                            ModifiedUser = req.ModifiedUser,
                                            //leave info
                                            AllowNegBal = l.AllowNegBal,
                                            AbsenceType = l.AbsenceType,
                                            Percentage = l.Percentage,
                                            MaxDaysInPeriod = l.MaxDaysInPeriod
                                        }).FirstOrDefault();

            float balance;

            GetMaxAllowedDays(Request.TypeId, Request.AbsenceType, Request.PeriodId, Request.EmpId, Request.AllowNegBal, Request.Percentage, Request.MaxDaysInPeriod, out balance);
            Request.BalAfterCancel = balance + Request.NofDays;
            Request.Balance = balance;

            return Request;
        }

        #endregion

        #region First Open Balance

        public IQueryable<LeaveTransOpenBalanceViewModel> GetLeaveFirstTrans(int companyId, string culture, int LeaveType, DateTime FiscalYear, string Departments)
        {
            var deptIds = new List<int>();
            if (!String.IsNullOrEmpty(Departments))
                deptIds = Departments.Split(',').Select(a => Convert.ToInt32(a)).ToList();
            else
                Departments = "";
            //Get  YearId ;
            int? YearId = context.FiscalYears.Where(s => s.Name == FiscalYear.Year.ToString()).Select(a => a.Id).SingleOrDefault();
            //var CalendarId = context.LeaveTypes.Where(a => a.Id == LeaveType).Select(a => a.CalendarId).FirstOrDefault();
            var PeriodId = (from l in context.LeaveTypes
                            where l.Id == LeaveType
                            join p in context.Periods on new { c1 = l.CalendarId, c2 = YearId } equals new { c1 = p.CalendarId, c2 = p.YearId }
                            select p.Id).FirstOrDefault();

            var query = (from e in context.People
                         join a in context.Assignments on e.Id equals a.EmpId
                         where (a.CompanyId == companyId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today && (Departments == "" || deptIds.Contains(a.DepartmentId)))
                         join t in context.LeaveTrans on a.EmpId equals t.EmpId into g
                         from t in g.Where(p => p.CompanyId == companyId && p.TypeId == LeaveType && p.PeriodId == PeriodId && p.TransType == 0).DefaultIfEmpty()
                         select new
                         {
                             Code = a.Code,
                             DeptId = a.DepartmentId,
                             transQty = t == null ? 0 : t.TransQty,
                             EmpId = a.EmpId,
                             flag = t == null || t.TransQty == 0 ? true : false,
                             Employee = HrContext.TrlsName(e.Title + " " + e.FirstName + " " + e.Familyname, culture),
                             LeaveType = t.TypeId

                         }).GroupBy(a => new { a.EmpId, a.Code, a.DeptId, a.Employee, a.LeaveType, a.flag }).Select(a => new LeaveTransOpenBalanceViewModel
                         {
                             EmpId = a.Key.EmpId,
                             Code = a.Key.Code,
                             Employee = a.Key.Employee,
                             transQty = a.Sum(s => s.transQty),
                             Department = a.Key.DeptId,
                             TypeId = LeaveType,
                             flag = a.Key.flag,
                             PeriodId = PeriodId
                         });
            return query;
        }
        public string CheckPeriods(DateTime Period,int LeaveId,string Culture)
        {
            var result = "OK";
            int? YearId = context.FiscalYears.Where(s => s.Name == Period.Year.ToString()).Select(a => a.Id).SingleOrDefault();
            var PeriodStatus = (from l in context.LeaveTypes
                            where l.Id == LeaveId
                            join p in context.Periods on new { c1 = l.CalendarId, c2 = YearId } equals new { c1 = p.CalendarId, c2 = p.YearId }
                            select p.Status).FirstOrDefault();
            if (PeriodStatus == 0)
                result = "NotDefinedPeriod," + MsgUtils.Instance.Trls(Culture, "ThisDateNotDefined");
            else if (PeriodStatus == 2)
            {
                result = "ClosedPeriod," + MsgUtils.Instance.Trls(Culture, "ThisPeriodClosed");
            }

            return result;
        }

        #endregion

        #region Money Balance
        //GetLeaveMoneyTrans
        public IQueryable<LeaveMoneyAdjustViewModel> GetLeaveMoneyTrans(int companyId, string culture)
        {
            var query = from a in context.LeaveAdjusts
                        where a.CompanyId == companyId && a.TransType == 12
                        join p in context.People on a.EmpId equals p.Id
                        join s in context.Assignments on p.Id equals s.EmpId
                        where (s.CompanyId == companyId && s.AssignDate <= a.AdjustDate && s.EndDate >= a.AdjustDate)
                        join c in context.CompanyStructures on s.DepartmentId equals c.Id
                        select new LeaveMoneyAdjustViewModel
                        {
                            Id = a.Id,
                            EmpId = a.EmpId,
                            Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                            AdjustDate = a.AdjustDate,
                            Description = a.Description,
                            WorkingDate = a.WorkingDate,
                            ExpiryDate = a.ExpiryDate,
                            NofDays = a.NofDays,
                            PeriodId = a.PeriodId,
                            TransType = a.TransType,
                            TypeId = a.TypeId,
                            Department = HrContext.TrlsName(c.Name, culture),
                            Code = s.Code,
                            CompanyId = companyId
                        };

            return query.OrderByDescending(q => q.AdjustDate);

        }

        #endregion
        #region Credit Balance
        public IQueryable<LeaveMoneyAdjustViewModel> GetLeaveCreaditBalance(int companyId, string culture)
        {
            var query = from a in context.LeaveAdjusts
                        where a.CompanyId == companyId && (a.TransType == 4 || a.TransType == 13)
                        join p in context.People on a.EmpId equals p.Id
                        join s in context.Assignments on p.Id equals s.EmpId
                        where (s.CompanyId == companyId && s.AssignDate <= a.AdjustDate && s.EndDate >= a.AdjustDate)
                        join c in context.CompanyStructures on s.DepartmentId equals c.Id
                        select new LeaveMoneyAdjustViewModel
                        {
                            Id = a.Id,
                            EmpId = a.EmpId,
                            Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                            AdjustDate = a.AdjustDate,
                            Description = a.Description,
                            WorkingDate = a.WorkingDate,
                            ExpiryDate = a.ExpiryDate,
                            NofDays = a.NofDays,
                            PeriodId = a.PeriodId,
                            TransType = a.TransType,
                            TypeId = a.TypeId,
                            Department = HrContext.TrlsName(c.Name, culture),
                            Code = s.Code,
                            CompanyId = companyId
                        };

            return query.OrderByDescending(q => q.AdjustDate);
        }
        //GetLeaveRest

        #endregion
        #region Rest Balance
        public IQueryable<LeaveMoneyAdjustViewModel> GetLeaveRest(int companyId, string culture)
        {
            var query = from a in context.LeaveAdjusts
                        where a.CompanyId == companyId && a.TransType == 3
                        join p in context.People on a.EmpId equals p.Id
                        join s in context.Assignments on p.Id equals s.EmpId
                        where (s.CompanyId == companyId && s.AssignDate <= a.AdjustDate && s.EndDate >= a.AdjustDate)
                        join c in context.CompanyStructures on s.DepartmentId equals c.Id
                        select new LeaveMoneyAdjustViewModel
                        {
                            Id = a.Id,
                            EmpId = a.EmpId,
                            Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                            AdjustDate = a.AdjustDate,
                            Description = a.Description,
                            WorkingDate = a.WorkingDate,
                            ExpiryDate = a.ExpiryDate,
                            NofDays = a.NofDays,
                            PeriodId = a.PeriodId,
                            TransType = a.TransType,
                            TypeId = a.TypeId,
                            Department = HrContext.TrlsName(c.Name, culture),
                            Code = s.Code,
                            CompanyId = companyId
                        };

            return query.OrderByDescending(q => q.AdjustDate);
        }

        public IQueryable<FormList> GetDeptEmployees(int companyId, string culture, string Departments)
        {
            var deptIds = Departments.Split(',').Select(a => Convert.ToInt32(a));
            var query = from e in context.People
                        join a in context.Assignments on e.Id equals a.EmpId
                        where (a.CompanyId == companyId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today && deptIds.Contains(a.DepartmentId))
                        select new FormList
                        {
                            id = a.EmpId,
                            name = HrContext.TrlsName(e.Title + " " + e.FirstName + " " + e.Familyname, culture)
                        };
            return query;
        }
        #endregion


        public void CancelLeaveRequests(int empId, string userName, int companyId, byte version, string culture)
        {
            string[] ApprovalStatus = { "New", "Submit", "Employee review", "Manager Review", "Accept", "Approved", "Cancel before approved", "Cancel after approved", "Rejected" };

            var LeaveRequests = context.LeaveRequests.Where(a => a.EmpId == empId && (a.ApprovalStatus < 6 || (a.ApprovalStatus == 6 && a.ActualStartDate <= DateTime.Today))).ToList();
            foreach (var item in LeaveRequests)
            {
                AddTrailViewModel trail = new AddTrailViewModel() { ObjectName = "LeaveReqFollowUpForm", ColumnName = "ApprovalStatus", SourceId = item.Id.ToString(), CompanyId = companyId, Version = version, UserName = userName };
                if (item.ApprovalStatus < 6)
                {
                    trail.ValueAfter = MsgUtils.Instance.Trls(culture, ApprovalStatus[6]);
                    trail.ValueBefore = MsgUtils.Instance.Trls(culture, ApprovalStatus[item.ApprovalStatus - 1]);
                    AddTrail(trail);
                    item.ApprovalStatus = 7;
                }
                else
                {
                    trail.ValueAfter = MsgUtils.Instance.Trls(culture, ApprovalStatus[7]);
                    trail.ValueBefore = MsgUtils.Instance.Trls(culture, ApprovalStatus[5]);
                    AddTrail(trail);
                    item.ApprovalStatus = 8;
                }

                item.ModifiedTime = DateTime.Now;
                item.ModifiedUser = userName;

                context.LeaveRequests.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
        }

        public bool DeleteRequest(LeaveRequest request, string culture)
        {
            if (request != null && request.ApprovalStatus == 1)
            {
                context.LeaveRequests.Remove(request);
                return true;
            }
            return false;
            //else if (request.EmpId == request.AuthbyEmpId)
            //{
            //    request.IsDeleted = true;
            //    context.LeaveRequests.Attach(request);
            //    context.Entry(request).State = EntityState.Modified;
            //}
            //else
            //    return MsgUtils.Instance.Trls(culture, "CantDelete");

            //return "";
        }

        public void Add(LeaveTrans trans)
        {
            context.LeaveTrans.Add(trans);
        }
        public void Remove(LeaveTrans LeaveTrans)
        {
            if (Context.Entry(LeaveTrans).State == EntityState.Detached)
            {
                context.LeaveTrans.Attach(LeaveTrans);
            }
            context.LeaveTrans.Remove(LeaveTrans);
        }

        public void Add(LeavePosting posting)
        {
            context.PostingLeave.Add(posting);
        }
        public void Attach(LeavePosting leavePosting)
        {
            context.PostingLeave.Attach(leavePosting);
        }

        public DbEntityEntry<LeavePosting> Entry(LeavePosting leavePosting)
        {
            return Context.Entry(leavePosting);
        }
        public void Add(Workflow workflow)
        {
            context.Workflows.Add(workflow);
        }
        public void Attach(Workflow workflow)
        {
            context.Workflows.Attach(workflow);
        }

        public DbEntityEntry<Workflow> Entry(Workflow workflow)
        {
            return Context.Entry(workflow);
        }
        public void Add(WfRole Wfrole)
        {
            context.WfRoles.Add(Wfrole);
        }
        public void Attach(WfRole Wfrole)
        {
            context.WfRoles.Attach(Wfrole);
        }
        public DbEntityEntry<WfRole> Entry(WfRole Wfrole)
        {
            return Context.Entry(Wfrole);
        }
        public void Remove(WfRole Wfrole)
        {
            if (Context.Entry(Wfrole).State == EntityState.Detached)
            {
                context.WfRoles.Attach(Wfrole);
            }
            context.WfRoles.Remove(Wfrole);
        }

        public void Add(WfTrans WfTrans)
        {
            context.WfTrans.Add(WfTrans);
        }

        #region Dashboard
        public IEnumerable<ChartViewModel> LeaveStatistics(byte range, int companyId, string culture)
        {
            string[] ApprovalStatus = { "New", "Submit", "EmployeeReview", "ManagerReview", "Accept", "Approved", "Cancel before approved", "Cancel after approved", "Rejected" };
            string[] Colors = { "", "#42a7ff", "#ededed", "#797979", "#d54c7e", "#B0D877", " #797979", "#ededed", "#42a7ff", "#ededed" };

            DateTime startRange, endRange;
            RangeFilter(range, out startRange, out endRange);

            var query = context.LeaveRequests.Where(l => l.CompanyId == companyId && l.ApprovalStatus != 1 //&& l.StartDate.Year == DateTime.Now.Year)
                && (l.StartDate >= startRange && l.StartDate <= endRange))
                .GroupBy(l => new { l.StartDate.Year, l.ApprovalStatus })
                .Select(l => new
                {
                    Status = l.Key.ApprovalStatus,
                    Count = l.Count()
                }).ToList();

            return query.Select(l => new ChartViewModel { Id = (int)l.Status, category = MsgUtils.Instance.Trls(culture, ApprovalStatus[l.Status - 1]), value = l.Count, color = Colors[l.Status - 1] }).ToList();
        }

        public IEnumerable<LeaveReqViewModel> LeaveStatisticsGrid(byte range, byte status, int companyId, string culture)
        {
            string[] ApprovalStatus = { "New", "Submit", "EmployeeReview", "ManagerReview", "Accept", "Approved", "Cancel before approved", "Cancel after approved", "Rejected" };

            DateTime startRange, endRange;
            RangeFilter(range, out startRange, out endRange);

            var query = (from l in context.LeaveRequests
                         where l.CompanyId == companyId && l.ApprovalStatus == status && (l.StartDate >= startRange && l.StartDate <= endRange)
                         join p in context.People on l.EmpId equals p.Id
                         select new LeaveReqViewModel
                         {
                             Id = l.Id,
                             Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                             LeaveType = HrContext.TrlsName(l.LeaveType.Name, culture),
                             ApprovalStatus = l.ApprovalStatus,
                         }).ToList();

            return query.Select(l => new LeaveReqViewModel { Id = l.Id, Approval = MsgUtils.Instance.Trls(culture, ApprovalStatus[l.ApprovalStatus - 1]), Employee = l.Employee, LeaveType = l.LeaveType }).ToList();
        }

        public IEnumerable<LeaveTransCounts> LeavesCounts(int empId, int companyId, string culture)
        {
            var query = (from t in context.LeaveTrans
                         where t.CompanyId == companyId && t.EmpId == empId
                         join p in context.Periods on t.PeriodId equals p.Id
                         where p.Status == 1
                         join l in context.LeaveTypes on t.TypeId equals l.Id
                         where (l.HasAccrualPlan || l.MaxDaysInPeriod != 0) && (l.AbsenceType == 1 || l.AbsenceType == 3 || l.AbsenceType == 8) // 1.Annual, 3.Illness, 8.Casual
                         select new
                         {
                             AbsenceCode = l.AbsenceType,
                             Balance = t.TransFlag * t.TransQty,
                             MaxDays = l.HasAccrualPlan ? 0 : (l.MaxDaysInPeriod ?? 0),
                             TotalDays = t.TransFlag == -1 || t.TransType == 5 ? t.TransQty * t.TransFlag * -1 : 0,
                             HasAccrualPlan = l.HasAccrualPlan,
                             TypeId = l.Id,
                             PeriodId = p.Id,
                             LeaveName = HrContext.TrlsName(l.Name, culture) + " - " + p.Name
                         }).GroupBy(d => new { d.PeriodId, d.AbsenceCode, d.TypeId }).Select(g => new LeaveTransCounts
                         {
                             AbsenceCode = g.Key.AbsenceCode,
                             TypeId = g.Key.TypeId,
                             Name = g.FirstOrDefault().LeaveName,
                             Balance = g.Sum(s => s.Balance) + g.FirstOrDefault().MaxDays,
                             Days = g.Sum(s => s.TotalDays),
                             PeriodId = g.FirstOrDefault().PeriodId
                         }).ToList();

            return query;
        }

        public IQueryable<LeaveReqViewModel> PeriodLeaveGrid(int empId, int leaveTypeId, int periodId, int companyId, string culture)
        {
            var query = context.LeaveRequests
                .Where(r => r.CompanyId == companyId && r.EmpId == empId && r.TypeId == leaveTypeId && r.PeriodId == periodId && r.ApprovalStatus == 6)
                .Select(r => new LeaveReqViewModel
                {
                    Id = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    NofDays = r.NofDays,
                    LeaveType = HrContext.TrlsName(r.LeaveType.Name, culture) + " - " + r.Period.Name,
                });

            return query;
        }

        public void RangeFilter(byte range, out DateTime startRange, out DateTime endRange)
        {
            DateTime Today = DateTime.Today;

            //Default -- 1. Today
            startRange = Today;
            endRange = Today;

            switch (range)
            {
                case 2: //Last 3 Days
                    startRange = Today.AddDays(-2);
                    endRange = Today;
                    break;
                case 3: //Last Week
                    startRange = Today.AddDays(-6);
                    endRange = Today;
                    break;

                case 4: //Last Month
                    startRange = new DateTime(Today.Year, Today.Month, 1);
                    endRange = new DateTime(Today.Year, Today.Month, DateTime.DaysInMonth(Today.Year, Today.Month));
                    break;
                case 5: //Last 3 Months
                    startRange = new DateTime(Today.Year, Today.AddMonths(-2).Month, 1);
                    endRange = new DateTime(Today.Year, Today.Month, DateTime.DaysInMonth(Today.Year, Today.Month));
                    break;
                case 6: //Last 6 Months
                    startRange = new DateTime(Today.Year, Today.AddMonths(-5).Month, 1);
                    endRange = new DateTime(Today.Year, Today.Month, DateTime.DaysInMonth(Today.Year, Today.Month));
                    break;
                case 7:
                    startRange = new DateTime(Today.Year, 1, 1);
                    endRange = new DateTime(Today.Year, 12, DateTime.DaysInMonth(Today.Year, 12));
                    break;
                default: //1. Today
                    break;
            }
        }


        #endregion
        #region API
        public ValidationMessages CheckLeaveRequestApi(int TypeId, int EmpId, DateTime StartDate, DateTime EndDate,float NofDays, string culture, int RequestId, bool isSSUser, int companyId, int? replaceEmp = null)
        {
            ValidationMessages message = new ValidationMessages();
            message.IsError = false;
            // List<string> Errors = new List<string>();
            LeaveType type = new LeaveType();
            var request = context.LeaveRequests.FirstOrDefault(r => r.Id == RequestId);
            #region assign
            string assignError = CheckAssignStatus(EmpId, TypeId, out type, culture);
            RequestValidationViewModel requestVal = new RequestValidationViewModel() { ExDayOff = type.ExDayOff, ExHolidays = type.ExHolidays, MaxDays = type.MaxDays };

            if (!string.IsNullOrEmpty(assignError))
            {
                message.AssignError = assignError;
                message.IsError = true;
                return message;
            }
            #endregion

            #region Check Workflow
            //WfViewModel wf = new WfViewModel()
            //{
            //    Source = "Leave",
            //    SourceId = type.Id,
            //    DocumentId = RequestId,
            //    RequesterEmpId = EmpId,
            //    ApprovalStatus = 2,
            //};
            //var wfTrans = AddWorkFlow(wf, culture);
            //if (wfTrans == null && wf.WorkFlowStatus != "Success")
            //{
            //    message.NoWorkFlowError= MsgUtils.Instance.Trls(culture, "NoWorkFlowerror");
            //}
            #endregion

            #region Replacement Employee
            //Check if the employee is Replacement Employee in this date
            var replaceFor = IsReplacement(EmpId, StartDate, EndDate, companyId, culture);
            if (replaceFor.Count > 0)
            {
                message.IsReplacementError = MsgUtils.Instance.Trls(culture, "YouAreReplacementInThisDateFor").Replace("{0}", string.Join(",", replaceFor));
                message.IsError = true;
            }
            //Check if the Repalcement Employee is Avilable
            if (replaceEmp != null)
            {
                var replaceAvilabe = HavePervRequests(new List<int> { replaceEmp.GetValueOrDefault() }, 0, StartDate, EndDate, companyId, true);
                if (replaceAvilabe.Count > 0)
                {
                    message.ReplacementError = MsgUtils.Instance.Trls(culture, "ReplaceEmployeeInLeave");
                    message.IsError = true;
                }
            }
            #endregion

            #region WaitingMonth
            if (type.WaitingMonth != null && type.WaitingMonth.Value > 0)
            {
                DateTime empDate;
                switch (type.WorkServMethod)
                {
                    case 2: // start experience date
                        empDate = context.People.Where(p => p.Id == EmpId).Select(p => p.StartExpDate).FirstOrDefault().Value;
                        break;
                    case 3: // last employment date
                        empDate = context.Employements.Where(e => e.EmpId == EmpId && e.Status == 1).Select(e => e.StartDate).FirstOrDefault();
                        break;
                    case 4: // last assignment date
                        empDate = context.Assignments.Where(a => a.EmpId == EmpId && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today)).Select(a => a.AssignDate).FirstOrDefault();
                        break;
                    default: // From Join date is default
                        empDate = context.People.Where(p => p.Id == EmpId).Select(p => p.JoinDate).FirstOrDefault().GetValueOrDefault();
                        break;
                }

                empDate = empDate.AddMonths(type.WaitingMonth.Value);
                if (StartDate.Date.CompareTo(empDate.Date) < 0)
                {
                    message.WaitingError = MsgUtils.Instance.Trls(culture, "WaitingMonthes");
                    message.WaitingMonth = empDate;
                    message.IsError = true;
                }
            }
            #endregion

            #region HasARequest
            var prevReqs = HavePervRequests(new List<int> { EmpId }, RequestId, StartDate, EndDate, companyId);
            if (prevReqs.Count > 0)
            {
                message.HasRequestError = MsgUtils.Instance.Trls(culture, "DoublicateStartDate");
                message.IsError = true;
            }

            #endregion

            #region  Dept Leave Plan -- Stars

            string msg;
            int PeriodId = GetLeaveRequestPeriod(type.CalendarId, StartDate, culture, out msg);
            var currentDate = request == null ? DateTime.Now.Date : request.StartDate;
            int CheckedId = GetLeaveRequestPeriod(type.CalendarId, currentDate, culture, out msg);
            var period = (from p in context.Periods where p.Id == CheckedId select new {p.StartDate,p.EndDate }).FirstOrDefault();
            if (StartDate.Date > period.EndDate.Date || StartDate.Date < period.StartDate.Date)
            {
                message.IsError = true;
                message.PeriodError = MsgUtils.Instance.Trls(culture, "ExceedPeriodLimit");

            } 
            if (msg != "OK") // only check valid period
            {
                message.StarsError = msg;
                message.IsError = true;
            }

            if (type.IncLeavePlan)
            {
                GetStarsParamVM param = new GetStarsParamVM { EmpId = EmpId, StartDate = StartDate, EndDate = EndDate, RequestId = RequestId, ExDayOff = type.ExDayOff, ExHolidays = type.ExHolidays, PeriodId = PeriodId, ComapnyId = companyId };
                int Stars, EmpStars;
                List<string> errorMsgs = CheckLeavePlan(param, culture, out Stars, out EmpStars);

                message.Stars = Stars;
                //message.EmpStars = EmpStars;
                if (errorMsgs.Count > 0)
                {
                    message.StarsError = String.Join(" ", errorMsgs);
                    message.IsError = true;
                    //message.Percentage = (float)plan.MinAllowPercent;
                }
            }
            #endregion

            #region  for edit leave operation 
            int approvalStatus = 1;
            if (RequestId > 0)
                approvalStatus = context.LeaveRequests.FirstOrDefault(r => r.Id == RequestId)?.ApprovalStatus ?? 1;
            
            if (approvalStatus < 6)
            {
                if (NofDays > requestVal.AllowedDays) 
                    message.AllowedDaysError= MsgUtils.Instance.Trls(culture, "AllowedDays");
                if (NofDays > type.MaxDays) 
                    message.CantGreaterError =MsgUtils.Instance.Trls(culture, "CantGreaterThan");
            }
            requestVal.BalAfter = requestVal.BalBefore.GetValueOrDefault() - NofDays;
            


            #endregion
            return message;
        }
        public Dictionary<string, string> ReadMailEmpAssign(string Language, int Id)
        {
            DateTime Today = DateTime.Today.Date;

            var query = (from r in context.AssignOrders
                         where r.Id == Id
                         join a in context.Assignments on r.EmpId equals a.EmpId into g
                         from a in g.Where(x => x.CompanyId == r.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                         join m in context.Assignments on r.ManagerId equals m.EmpId into g1
                         from m in g1.Where(x => x.CompanyId == r.CompanyId && x.AssignDate <= Today && x.EndDate >= Today).DefaultIfEmpty()
                         select new
                         {
                             AssignManager = HrContext.TrlsName(r.Manager.Title + " " + r.Manager.FirstName + " " + r.Manager.Familyname, Language),
                             ManagerJob = HrContext.TrlsName(m.Job.Name,Language),
                             ManagerDepartment = HrContext.TrlsName(m.Department.Name, Language),
                             AssignEmployee = HrContext.TrlsName(r.Emp.Title + " " + r.Emp.FirstName + " " + r.Emp.Familyname, Language),
                             AssignCode = r.Id.ToString(),
                             EmployeeJob = HrContext.TrlsName(a.Job.Name, Language),
                             EmployeeDepartment = HrContext.TrlsName(a.Department.Name, Language),
                             AssignDay =" ",
                             AssignDate = r.AssignDate,
                             AssignTask = r.TaskDesc,
                             Compensation =  r.CalcMethod == 1 ? HrContext.TrlsMsg("Monetary",Language): HrContext.TrlsMsg("Time compensation", Language),
                             CompensationDay =  " ",
                             CompensationDate = r.ExpiryDate,
                         }).FirstOrDefault();

            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (query != null)
            {
                var ObjProps = query.GetType().GetProperties();
                for (int i = 0; i < ObjProps.Length; i++)
                {
                    var p = ObjProps[i].GetValue(query);
                    if (ObjProps[i].Name == "CompensationDay")
                        p = query.CompensationDate != null ? MsgUtils.Instance.Trls(Language, query.CompensationDate?.DayOfWeek.ToString()) : " ";
                    else if (ObjProps[i].Name == "AssignDay")
                        p = MsgUtils.Instance.Trls(Language, query.AssignDate.DayOfWeek.ToString());
                    else if (ObjProps[i].Name == "AssignDate")
                        p = query.AssignDate.ToString("yyyy-MM-dd");
                    else if (ObjProps[i].Name == "CompensationDate")
                        p = query.CompensationDate?.ToString("yyyy-MM-dd");
                    dic.Add(ObjProps[i].Name, p != null ? p.ToString() : " ");
                }
            }
            return dic;
        }
        
        #region AssignOrder
        public AssignOrder GetAssignOrderByiD(int? Id)
        {
            return context.AssignOrders.Find(Id);
        }
        public void AddAssignOrder(AssignOrder order)
        {
            context.AssignOrders.Add(order);
        }
        public DbEntityEntry<AssignOrder> EntryAssignOrder(AssignOrder AssignOrder)
        {
            return Context.Entry(AssignOrder);
        }
        public void AttachAssignOrder(AssignOrder assignOrder)
        {
            context.AssignOrders.Attach(assignOrder);
        }
        //For Mobile
        public IQueryable<AssignOrderViewModel> ReadAssignOrders(int companyId, string culture)
        {
            IQueryable<AssignOrderViewModel> query =
                from l in context.AssignOrders
                join a in context.Assignments on l.EmpId equals a.EmpId
                where (a.CompanyId == companyId  &&a.AssignDate <= l.AssignDate && a.EndDate >= l.AssignDate)
                join p in context.People on l.EmpId equals p.Id
                join wft in context.WF_TRANS on new { p1 = "AssignOrder" + l.CalcMethod, p2 = l.CompanyId, p3 = l.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                from wft in g.DefaultIfEmpty()
                join ap in context.People on wft.AuthEmp equals ap.Id into g1
                from ap in g1.DefaultIfEmpty()
                join role in context.Roles on wft.RoleId equals role.Id into g4
                from role in g4.DefaultIfEmpty()
                select new AssignOrderViewModel
                {
                    Id = l.Id,
                    Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                    AssignDate = l.AssignDate,
                    ExpiryDate = l.ExpiryDate,
                    CalcMethod = l.CalcMethod,
                    CreatedTime = l.CreatedTime,
                    CreatedUser = l.CreatedUser,
                    Duration = l.Duration,
                    LeaveTypeId = l.LeaveTypeId,
                    ManagerId = l.ManagerId,
                    ModifiedTime = l.ModifiedTime,
                    ModifiedUser = l.ModifiedUser,
                    TaskDesc = l.TaskDesc,
                    //WFlowId
                    ApprovalStatus = l.ApprovalStatus,
                    EmpId = l.EmpId,
                    CompanyId = l.CompanyId,
                    RoleId = wft.RoleId.ToString(),
                    DeptId = wft.DeptId,
                    PositionId = wft.PositionId,
                    AuthBranch = wft.AuthBranch,
                    AuthDept = wft.AuthDept,
                    AuthEmp = wft.AuthEmp,
                    AuthPosName = role == null ? "" : role.Name,
                    AuthPosition = wft.AuthPosition,
                    LeaveType = HrContext.TrlsName(l.LeaveType.Name, culture),
                    Manager = HrContext.TrlsName(l.Manager.Title + " " + l.Manager.FirstName + " " + l.Manager.Familyname, culture),
                    BranchId = wft.BranchId,
                    Status = l.ApprovalStatus,
                    WorkflowTime = wft.CreatedTime,
                };

            return query.OrderByDescending(a => a.AssignDate);
        }
        public IQueryable<AssignOrderViewModel> ReadAssignOrders(int companyId, byte Tab, byte Range, string Depts, DateTime? Start, DateTime? End, string culture)
        {
            //10- All, 0-Custom
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, companyId, out Start, out End);
            List<int> deptLst = new List<int>();
            if (!String.IsNullOrEmpty(Depts))
                deptLst = Depts.Split(',').Select(a => int.Parse(a)).ToList();
            else
                Depts = "";

            DateTime today = DateTime.Today.Date;

            var q1 = from l in context.AssignOrders
                     where l.CompanyId == companyId
                     select l;

            if (Range != 10) // Allow date range
                q1 = q1.Where(l => Start <= l.AssignDate && l.AssignDate <= End);

            if (Tab == 1) //Pending
                q1 = q1.Where(l => l.ApprovalStatus < 6);
            else if (Tab == 2) //Approved
                q1 = q1.Where(l => l.ApprovalStatus == 6 && today <= l.AssignDate);
            else if (Tab == 3) //Rejected
                q1 = q1.Where(l => l.ApprovalStatus == 9 && today <= l.AssignDate);
            else if (Tab == 4) //Archive
                q1 = q1.Where(l => today > l.AssignDate);

            IQueryable<AssignOrderViewModel> query =
                from l in q1
                join a in context.Assignments on l.EmpId equals a.EmpId
                where (a.CompanyId == companyId && a.AssignDate <= l.AssignDate && a.EndDate >= l.AssignDate && (Depts == "" || deptLst.Contains(a.DepartmentId)))
                join p in context.People on l.EmpId equals p.Id
                join wft in context.WF_TRANS on new { p1 = "AssignOrder" + l.CalcMethod, p2 = l.CompanyId, p3 = l.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                from wft in g.DefaultIfEmpty()
                join ap in context.People on wft.AuthEmp equals ap.Id into g1
                from ap in g1.DefaultIfEmpty()
                join role in context.Roles on wft.RoleId equals role.Id into g4
                from role in g4.DefaultIfEmpty()
                select new AssignOrderViewModel
                {
                    Id = l.Id,
                    Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                    EmpId = l.EmpId,
                    AssignDate = l.AssignDate,
                    ExpiryDate = l.ExpiryDate,
                    CalcMethod = l.CalcMethod,
                    Duration = l.Duration,
                    LeaveTypeId = l.LeaveTypeId,
                    Manager = HrContext.TrlsName(l.Manager.Title + " " + l.Manager.FirstName + " " + l.Manager.Familyname, culture),
                    ManagerId = l.ManagerId,
                    TaskDesc = l.TaskDesc,
                    ApprovalStatus = l.ApprovalStatus,
                    CompanyId = l.CompanyId,
                    RoleId = wft.RoleId.ToString(),
                    DeptId = wft.DeptId,
                    PositionId = wft.PositionId,
                    AuthBranch = wft.AuthBranch,
                    AuthDept = wft.AuthDept,
                    AuthEmp = wft.AuthEmp,
                    AuthEmpName = HrContext.TrlsName(ap.Title + " " + ap.FirstName + " " + ap.Familyname, culture),
                    AuthPosName = role == null ? "" : role.Name,
                    AuthPosition = wft.AuthPosition,
                    LeaveType = HrContext.TrlsName(l.LeaveType.Name, culture),
                    BranchId = wft.BranchId,
                    WorkflowTime = wft.CreatedTime,
                    Status = l.ApprovalStatus
                };

            return query.OrderByDescending(a => a.AssignDate);
        }

        public IQueryable<AssignOrderViewModel> ReadAssignOrdersArchieve(int companyId, byte Range, string Depts, DateTime? Start, DateTime? End, string culture)
        {
            //10- All, 0-Custom
            if (Range != 10 && Range != 0) RequestRangeFilter(Range, companyId, out Start, out End);
            List<int> deptLst = new List<int>();
            if (!String.IsNullOrEmpty(Depts))
                deptLst = Depts.Split(',').Select(a => int.Parse(a)).ToList();
            else
                Depts = "";

            DateTime Today = DateTime.Today.Date;
            var q1 = from l in context.AssignOrders
                     where l.CompanyId == companyId
                     select l;

            if (Range != 10) // Allow date range
                q1 = q1.Where(l => Start <= l.AssignDate && l.AssignDate <= End);

            IQueryable<AssignOrderViewModel> query =
                from l in q1
                join a in context.Assignments on l.EmpId equals a.EmpId
                where (a.CompanyId == companyId && a.AssignDate <= l.AssignDate && a.EndDate >= l.AssignDate && l.ApprovalStatus >= 6 && (Depts == "" || deptLst.Contains(a.DepartmentId)))
                join p in context.People on l.EmpId equals p.Id
                join wft in context.WF_TRANS on new { p1 = "AssignOrder" + l.CalcMethod, p2 = l.CompanyId, p3 = l.Id } equals new { p1 = wft.Source, p2 = wft.SourceId, p3 = wft.DocumentId } into g
                from wft in g.DefaultIfEmpty()
                join ap in context.People on wft.AuthEmp equals ap.Id into g1
                from ap in g1.DefaultIfEmpty()
                join role in context.Roles on wft.RoleId equals role.Id into g4
                from role in g4.DefaultIfEmpty()
                select new AssignOrderViewModel
                {
                    Id = l.Id,
                    Employee = HrContext.TrlsName(p.Title + " " + p.FirstName + " " + p.Familyname, culture),
                    EmpId = l.EmpId,
                    AssignDate = l.AssignDate,
                    ExpiryDate = l.ExpiryDate,
                    CalcMethod = l.CalcMethod,
                    Duration = l.Duration,
                    LeaveTypeId = l.LeaveTypeId,
                    Manager = HrContext.TrlsName(l.Manager.Title + " " + l.Manager.FirstName + " " + l.Manager.Familyname, culture),
                    ManagerId = l.ManagerId,
                    TaskDesc = l.TaskDesc,
                    ApprovalStatus = l.ApprovalStatus,
                    CompanyId = l.CompanyId,
                    RoleId = wft.RoleId.ToString(),
                    DeptId = wft.DeptId,
                    PositionId = wft.PositionId,
                    AuthBranch = wft.AuthBranch,
                    AuthDept = wft.AuthDept,
                    AuthEmp = wft.AuthEmp,
                    AuthEmpName = HrContext.TrlsName(ap.Title + " " + ap.FirstName + " " + ap.Familyname, culture),
                    AuthPosName = role == null ? "" : role.Name,
                    AuthPosition = wft.AuthPosition,
                    LeaveType = HrContext.TrlsName(l.LeaveType.Name, culture),
                    BranchId = wft.BranchId,
                    WorkflowTime = wft.CreatedTime,
                    Status= l.ApprovalStatus
                };
            return query.OrderByDescending(a=>a.AssignDate);
        }

        public IQueryable<DateTime> GetEmpAssignDates(int companyId, int EmpId)
        {
            var query = from a in context.AssignOrders
                        join l in context.Assignments on a.EmpId equals l.EmpId
                        where a.CompanyId == companyId && a.EmpId == EmpId && l.AssignDate <= a.AssignDate && l.EndDate >= a.AssignDate
                        select a.AssignDate;
            return query;
        }
        public int GetLastEmpCalcsMethod(int companyId, int EmpId)
        {
            var query = from a in context.AssignOrders
                        join l in context.Assignments on a.EmpId equals l.EmpId
                        where a.CompanyId == companyId && a.EmpId == EmpId && l.AssignDate <= a.AssignDate && l.EndDate >= a.AssignDate
                        orderby a.AssignDate descending
                        select a.CalcMethod;

            return query.FirstOrDefault();
        }
        public PeopleGridViewModel GetFullEmpInfo(int companyId, int EmpId,string culture)
        {
            var today = DateTime.Today.Date;
            var query = from a in context.Assignments
                        where a.CompanyId == companyId && a.EmpId == EmpId && a.AssignDate <= today && a.EndDate >= today
                        select new PeopleGridViewModel
                        {
                            Job = HrContext.TrlsName(a.Job.Name, culture),
                            DepartmentId = a.DepartmentId,
                            Department = HrContext.TrlsName(a.Department.Name, culture),
                            Code=a.Code
                        };
            return query.FirstOrDefault();                 
        }
        public IQueryable<AssignOrderViewModel> GetEmpAssignData(int companyId, int EmpId,string culture)
        {
            IQueryable<AssignOrderViewModel> query =
                from l in context.AssignOrders where l.ApprovalStatus == 6
                join a in context.Assignments on l.EmpId equals a.EmpId
                where (a.CompanyId == companyId && a.EmpId == EmpId && a.AssignDate <= l.AssignDate && a.EndDate >= l.AssignDate)
                join p in context.People on l.EmpId equals p.Id
                orderby l.AssignDate
                select new AssignOrderViewModel
                {
                    Id = l.Id,
                    AssignDate = l.AssignDate,
                    ExpiryDate = l.ExpiryDate,
                    CalcMethod = l.CalcMethod,
                    CreatedTime = l.CreatedTime,
                    CreatedUser = l.CreatedUser,
                    Duration = l.Duration,
                    NofDays = l.CalcMethod == 1 ?(l.Duration == 2 ? 0.25f : l.Duration == 3 ? 0.5f : 1f):0,
                    TimeNofDays = l.CalcMethod == 2 ? (l.Duration == 2 ? 0.25f : l.Duration == 3 ? 0.5f : 1f) : 0,
                    LeaveTypeId = l.LeaveTypeId,
                    Manager= HrContext.TrlsName(l.Manager.Title + " " + l.Manager.FirstName + " " + l.Manager.Familyname, culture),
                    ManagerId = l.ManagerId,
                    ModifiedTime = l.ModifiedTime,
                    ModifiedUser = l.ModifiedUser,
                    TaskDesc = l.TaskDesc,
                    Month = l.AssignDate.Month,
                    Year = l.AssignDate.Year+"/"+l.AssignDate.Month
                };
           // IQueryable<AssignOrderViewModel> orderedMonth = query.OrderByDescending(ord => ord.Month);
            
            return query;
        }
        #endregion

        public IList<DropDownList> GetSpacificLeaveTypes(int companyId, string culture,int empId)
        {

            string sql = "SELECT T.Id, dbo.fn_TrlsName(T.Name, '" + culture + "') Name FROM LeaveTypes T, Assignments A, Employements E, People P WHERE A.EmpId = E.EmpId And A.EmpId = P.Id And A.EmpId = " + empId + " AND (CONVERT(date, getdate()) Between A.AssignDate And A.EndDate) AND E.Status = 1 AND (GETDATE() Between T.StartDate And ISNULL(T.EndDate, '2099/01/01')) AND (T.AbsenceType in (1,6,8)) AND ((T.IsLocal = 1 And T.CompanyId = " + companyId + ") Or t.IsLocal = 0) AND ISNULL(T.Gender, P.Gender) = P.Gender AND ISNULL(T.Religion, ISNULL(P.Religion, 0)) = ISNULL(P.Religion, 0) AND ISNULL(T.MaritalStat, ISNULL(P.MaritalStat, 0)) = ISNULL(P.MaritalStat, 0) AND ISNULL(T.Nationality, ISNULL(P.Nationality, 0)) = ISNULL(P.Nationality, 0) AND ISNULL(T.MilitaryStat, ISNULL(P.MilitaryStat, 0)) = ISNULL(P.MilitaryStat, 0) AND (CASE WHEN LEN(T.Employments) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Employments, ',') WHERE VALUE = E.PersonType), 0) ELSE E.PersonType END) = E.PersonType AND (CASE WHEN LEN(T.Jobs) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Jobs, ',') WHERE VALUE = A.JobId), 0) ELSE A.JobId END) = A.JobId AND (CASE WHEN LEN(T.CompanyStuctures) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.CompanyStuctures, ',') WHERE VALUE = A.DepartmentId), 0) ELSE A.DepartmentId END) = A.DepartmentId AND (CASE WHEN LEN(T.Branches) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Branches, ',') WHERE VALUE = ISNULL(A.BranchId, 0)), -1) ELSE ISNULL(A.BranchId, 0) END) = ISNULL(A.BranchId, 0) AND (CASE WHEN LEN(T.Positions) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Positions, ',') WHERE VALUE = ISNULL(A.PositionId, 0)), -1) ELSE ISNULL(A.PositionId, 0) END) = ISNULL(A.PositionId, 0) AND (CASE WHEN LEN(T.PeopleGroups) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.PeopleGroups, ',') WHERE VALUE = ISNULL(A.GroupId, 0)), -1) ELSE ISNULL(A.GroupId, 0) END) = ISNULL(A.GroupId, 0) AND (CASE WHEN LEN(T.Payrolls) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.Payrolls, ',') WHERE VALUE = ISNULL(A.PayrollId, 0)), -1) ELSE ISNULL(A.PayrollId, 0) END) = ISNULL(A.PayrollId, 0) AND (CASE WHEN LEN(T.PayrollGrades) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(T.PayrollGrades, ',') WHERE VALUE = ISNULL(A.PayGradeId, 0)), -1) ELSE ISNULL(A.PayGradeId, 0) END) = ISNULL(A.PayGradeId, 0)";
            return context.Database.SqlQuery<DropDownList>(sql).ToList();
            //var res = from LT in context.LeaveTypes
            //          where (LT.AbsenceType == 1 || LT.AbsenceType == 6) && (LT.CompanyId == companyId)
            //          select new
            //          {
            //              id = LT.Id,
            //              name = HrContext.TrlsName(LT.Name, culture),
            //              isActive = (LT.StartDate <= DateTime.Today && (LT.EndDate == null || LT.EndDate >= DateTime.Today))
            //          };
            //return res.ToList();
        }

        public bool DeleteAssignOrder(int id, string culture)
        {
            var model = GetAssignOrderByiD(id);
            var returned = context.AssignOrders.Remove(model);
            if (returned != null)
            {
                return true;
            }
            return false;
        }



        #endregion
    }
}
