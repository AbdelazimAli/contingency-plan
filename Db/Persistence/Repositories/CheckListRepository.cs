using Interface.Core.Repositories;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Persistence.Repositories
{
    class CheckListRepository : Repository<CheckList>, ICheckListRepository
    {
        public CheckListRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }
        public EmpChkListViewModel ReadEmpList(int Id)
        {
            return (from El in context.EmpChkLists
                    where El.Id == Id
                    select new EmpChkListViewModel
                    {
                        Id=El.Id,
                        CreatedTime=El.CreatedTime,
                        CreatedUser=El.CreatedUser,
                        Description=El.Description,
                        EmpId=El.EmpId,
                        ListEndDate=El.ListEndDate,
                        ListId=El.ListId,
                        ListStartDate=El.ListStartDate,
                        ListType=El.ListType,
                        ModifiedTime=El.ModifiedTime,
                        ModifiedUser=El.ModifiedUser,
                        Name=El.Name,
                        Status=El.Status ,
                        ManagerId = El.ManagerId                     
                    }).FirstOrDefault();
        }
        public string ChkBeforeEmployment(int companyId, string UserName, int EmpId, string Culture)
        {
            string error = "OK";
            var Personnel = GetPersonSetup(companyId);
            var Empchecklist = GetEmploymentEmpCheckLists(companyId, EmpId);
            if (Empchecklist != null && Empchecklist.Status == 0)
            {
                //System found pending employment checkList ,please complete list and try again
                // يوجد قائمه مرجعيه للتعيين لهذا الموظف غير مكتمله
               return error ="SystemPendingEmployment," + MsgUtils.Instance.Trls(Culture, "SystemPendingEmployment");

            }

            if (Empchecklist == null && Personnel.AutoEmployment == true)
            {
                var checkList = GetEmploymentCheckList(companyId);
                if (checkList != null)
                {

                    var Empchlst = AddEmpChlst(checkList, UserName, EmpId,companyId);
                    context.EmpChkLists.Add(Empchlst);
                    var CheckListTasks = ReadCheckListTask(checkList.Id).ToList();
                    AddEmpTask(CheckListTasks, UserName, Empchlst);
                    //System created new employment list ,please complete list then try again
                    //تم نسخ قائمه مرجعيه للتعيين , من فضلك أكمل القائمة وحاول مرة أخري
                   return error = "SystemCopyEmployment," + MsgUtils.Instance.Trls(Culture, "SystemCopyEmployment");

                }
            }
            var flexColumns =( from d in context.DocTypes
                              where d.RequiredOpt == 1 && (!d.IsLocal || (d.IsLocal && d.CompanyId == companyId))
                              join cd in context.CompanyDocsView on d.Id equals cd.TypeId into g
                              from t in g.Where(a => a.Source == "People" && a.SourceId == EmpId).DefaultIfEmpty()
                              where t.TypeId == null
                              select new { d.Id,title=HrContext.TrlsName(d.Name,Culture) }).ToList();

            if (flexColumns.Count != 0 && Personnel.EmploymentDoc == 2)
            {
                //System found necessary documents,please upload document Then try again
                //يوجد مستندات غير مكتمله ضروريه للتعييين 
                error = "SystemErrorDocuments," + MsgUtils.Instance.Trls(Culture, "SystemErrorDocuments")+ string.Join(",", flexColumns.Select(f => f.title).ToArray());

            }
           else if (flexColumns.Count !=0 && Personnel.EmploymentDoc == 1)
            {
                //System found necessary documents
                error = "SystemWarningDocuments," + MsgUtils.Instance.Trls(Culture, "SystemWarningDocuments") + string.Join(",", flexColumns.Select(f => f.title).ToArray());

            }


            return error;
        }
        public EmpChkList AddEmpChlst(CheckList checklist, string UserName, int EmpId,int companyId)
        {
            EmpChkList Emplist = new EmpChkList()
            {
                CreatedTime = DateTime.Now,
                Name = checklist.Name,
                EmpId = EmpId,
                CreatedUser = UserName,
                Description = checklist.Description,
                ListEndDate = checklist.EndDate,
                ListId = checklist.Id,
                ListStartDate = checklist.StartDate,
                ListType = checklist.ListType,
                Status = 0,
                CompanyId = companyId,
            };
            return Emplist;
        }
        public EmpChkList GetEmploymentEmpCheckLists(int company, int EmpId)
        {
            var query = (from c in context.EmpChkLists
                         where c.ListType == 1 && c.EmpId == EmpId
                         select c).FirstOrDefault();
            return query;
        }
        public CheckList GetEmploymentCheckList(int company)
        {
            var setup = GetPersonSetup(company);
            CheckList query = setup.AutoEmployment ? (from c in context.CheckLists
                         where c.CompanyId == company && c.ListType == 1 && c.Default
                         select c).FirstOrDefault() : null;
            return query;
        }
        public IQueryable<EmpTaskViewModel> ReadEmpListTask(int EmpList)
        {
            return from et in context.EmpTasks
                   where et.EmpListId == EmpList
                   select new EmpTaskViewModel
                   {
                        Id=et.Id,
                        EmpId=et.EmpId,
                        EmpListId=et.EmpListId,
                        AssignedTime=et.AssignedTime,
                        Description=et.Description,
                        Duration=et.Duration,
                        EndTime=et.EndTime,
                        ExpectDur=et.ExpectDur,
                        Priority=et.Priority,
                        Required=et.Required,
                        ManagerId=et.ManagerId,
                        StartTime=et.StartTime,
                        Status=et.Status,
                        TaskCat=et.TaskCat,
                        TaskNo=et.TaskNo,
                        Unit=et.Unit
                   };
        }

        #region Tasks SS
        public IEnumerable ReadTasksSubPeriods(int managerId)
        {
            return context.EmpTasks.Where(t => t.ManagerId == managerId && t.SubPeriodId != null).Select(t => new
            {
                id = t.SubPeriodId,
                name = t.SubPeriod.Name,
                periodId = t.SubPeriod.PeriodId
            }).Distinct().ToList();
        }
        public IEnumerable ReadTasksPeriods(int managerId, int companyId, string culture, out string message)
        {
            int? subPeriod;
            message = GetTaskSubPeriod(companyId, DateTime.Now, culture, out subPeriod);
            if(!String.IsNullOrEmpty(message)) return null;

            var selectedPeriod = context.SubPeriods.Where(s => s.Id == subPeriod).Select(s => new { s.Id, s.PeriodId }).FirstOrDefault();

            return (from t in context.EmpTasks where t.ManagerId == managerId
                    join p in context.Periods on t.SubPeriod.PeriodId equals p.Id
                    select new { id = p.Id, name = p.Name, selected = selectedPeriod.PeriodId == p.Id }).Distinct().ToList();
        }

        public IQueryable<EmpTasksViewModel> ReadManagerTasks(int managerId, int? periodId, int? subPeriodId, int companyId, string culture)
        {
            string sql = "select R.EmpId [Id], dbo.fn_TrlsName(E.Title + ' ' + E.FirstName + ' ' + E.FamilyName, '" + culture + "') [Employee], R.PeriodName [Period], R.PeriodId" + (subPeriodId == null ? "" : ", R.SubPeriodId") + " from (select T.EmpId," + (subPeriodId == null ? " P.[Name]" : " S.[Name] + ' - ' + P.[Name]") + " PeriodName, P.Id PeriodId, S.Id SubPeriodId, row_number() over (partition by T.EmpId order by T.AssignedTime desc) RNK from EmpTasks T, [Periods] P, SubPeriods S where T.ManagerId = " + managerId + " And T.SubPeriodId = S.Id And S.PeriodId = P.Id And P.Id = ISNULL(" + (periodId == null ? "null" : periodId.ToString()) + ", P.Id) And S.Id = ISNULL(" + (subPeriodId == null ? "null" : subPeriodId.ToString()) + ", S.Id) ) R, People E where R.RNK = 1 And R.EmpId = E.Id";
            return context.Database.SqlQuery<EmpTasksViewModel>(sql).AsQueryable();
        }

        public IQueryable<EmpTasksViewModel> ReadManagerEmpTasks(int managerId, int empId, int? periodId, int? subPeriodId, string culture)
        {
            var tasks = context.EmpTasks.Where(t => t.ManagerId == managerId && t.EmpId == empId && (subPeriodId == null ? true : subPeriodId == t.SubPeriodId) && (periodId == null ? true : periodId == t.SubPeriod.PeriodId))
            .Select(t => new EmpTasksViewModel
            {
                Id = t.Id,
                EmpList = t.EmpChklist.Name,
                EmpId = t.EmpId,
                Employee = HrContext.TrlsName(t.Employee.Title + " " + t.Employee.FirstName + " " + t.Employee.Familyname, culture),
                ManagerId = t.ManagerId,
                Description = t.Description,
                Priority = t.Priority,
                Status = t.Status,
                TaskCategory = HrContext.GetLookUpCode("EmpTaskCat", t.TaskCat.Value, culture),
                TaskNo = t.TaskNo,
                AssignedTime = t.AssignedTime,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                SubPeriodId = t.SubPeriodId,
                SubPeriod = t.SubPeriod.Name
            });
            return tasks;
        }
        public IEnumerable GetManagerEmpList(int managerId, int? positionId, int companyId, string culture)
        {
            var EmpTasks = context.Assignments.Where(a => a.CompanyId == companyId && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today) && a.EmpId == managerId).Select(a => a.EmpTasks).FirstOrDefault();

            if (EmpTasks == 2) //2-Use eligibility criteria
            {
                string sql = "SELECT P.Id, dbo.fn_TrlsName(ISNULL(P.Title, '') + ' ' + P.FirstName +' ' + P.Familyname , '" + culture + "') Name , (CASE WHEN P.HasImage = 1 THEN CAST(P.Id as nvarchar(10)) + '.jpeg' ELSE 'noimage.jpg' END) PicUrl, dbo.fn_GetEmpStatus(P.Id) Icon FROM Assignments M, Assignments A, Employements E, People P WHERE M.EmpId = " + managerId + " AND M.CompanyId = " + companyId + " And (GETDATE() Between M.AssignDate And M.EndDate) And A.EmpId = E.EmpId And A.EmpId = P.Id AND A.CompanyId = " + companyId + " AND (GETDATE() Between A.AssignDate And A.EndDate) AND E.Status = 1 AND (CASE WHEN LEN(M.Employments) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(M.Employments, ',') WHERE VALUE = E.PersonType), 0) ELSE E.PersonType END) = E.PersonType AND (CASE WHEN LEN(M.Jobs) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(M.Jobs, ',') WHERE VALUE = A.JobId), 0) ELSE A.JobId END) = A.JobId AND (CASE WHEN LEN(M.CompanyStuctures) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(M.CompanyStuctures, ',') WHERE VALUE = A.DepartmentId), 0) ELSE A.DepartmentId END) = A.DepartmentId AND (CASE WHEN LEN(M.Locations) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(M.Locations, ',') WHERE VALUE = ISNULL(A.LocationId, 0)), -1) ELSE ISNULL(A.LocationId, 0) END) = ISNULL(A.LocationId, 0) AND (CASE WHEN LEN(M.Positions) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(M.Positions, ',') WHERE VALUE = ISNULL(A.PositionId, 0)), -1) ELSE ISNULL(A.PositionId, 0) END) = ISNULL(A.PositionId, 0) AND (CASE WHEN LEN(M.PeopleGroups) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(M.PeopleGroups, ',') WHERE VALUE = ISNULL(A.GroupId, 0)), -1) ELSE ISNULL(A.GroupId, 0) END) = ISNULL(A.GroupId, 0) AND (CASE WHEN LEN(M.Payrolls) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(M.Payrolls, ',') WHERE VALUE = ISNULL(A.PayrollId, 0)), -1) ELSE ISNULL(A.PayrollId, 0) END) = ISNULL(A.PayrollId, 0) AND (CASE WHEN LEN(M.PayrollGrades) > 0 THEN ISNULL((SELECT VALUE FROM STRING_SPLIT(M.PayrollGrades, ',') WHERE VALUE = ISNULL(A.PayGradeId, 0)), -1) ELSE ISNULL(A.PayGradeId, 0) END) = ISNULL(A.PayGradeId, 0)";
                return context.Database.SqlQuery<DropDownList>(sql).Select(a => new { id = a.Id, name = a.Name, a.PicUrl, a.Icon }).ToList();
            }
            else //1-Employee whose direct managed
            {
                var emps = context.Assignments
                .Where(a => a.CompanyId == companyId && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today)
                         && a.ManagerId == managerId)
                .Union(context.Assignments
                    .Where(a => a.CompanyId == companyId && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today)
                                && positionId != null && a.Position.Supervisor == positionId))
                    .Select(a => new { id = a.EmpId, name = HrContext.TrlsName(a.Employee.Title + " " + a.Employee.FirstName + " " + a.Employee.Familyname, culture), PicUrl = (a.Employee.HasImage ? a.Employee.Id + ".jpeg" : "noimage.jpg"), Icon = HrContext.GetEmpStatus(a.Employee.Id) });

                return emps;
            }
        }    
        //Manager Chart
        public IEnumerable EmployeeProgress(int managerId, int? periodId, int? subPeriod, string culture)
        {  //0-new, 3-Canceled
            var query = context.EmpTasks.Where(t => t.ManagerId == managerId && t.EmpId != null && t.SubPeriodId != null && t.Status != 0 && t.Status != 3
                         && (subPeriod == null ? true : t.SubPeriodId == subPeriod) && (periodId == null ? true : t.SubPeriod.PeriodId == periodId))
                         .GroupBy(t => new { t.EmpId, t.Status, Period = (subPeriod == null ? t.SubPeriod.PeriodId : t.SubPeriodId )})
                         .Select(g => new ChartViewModel
                         {
                             EmpId = g.Key.EmpId.Value,
                             myGroup = g.Key.Period.ToString(),
                             category = g.Key.Status.ToString(),
                             value = g.Count(),
                             Year = g.FirstOrDefault().SubPeriod.PeriodId //period to filter
                         }).ToList();

            var total = query.GroupBy(t => new { t.EmpId, t.Year }).Select(t => new { t.Key.EmpId, t.Key.Year, total = t.Sum(s => s.value)});
            query = query.Select(t => new ChartViewModel
            {
                EmpId = t.EmpId,
                myGroup = t.myGroup,
                category = t.category,
                value =Convert.ToInt32(((float)t.value / total.Where(a => a.EmpId == t.EmpId && a.Year == t.Year).FirstOrDefault().total) * 100),
                Year = t.Year
            }).ToList();

            return GetStatus(query, culture);
        }

        public EmpTask GetEmpTask(int Id)
        {
            return context.EmpTasks.Find(Id);
        }

        public EmpTasksViewModel GetManagerEmpTask(int Id)
        {
            EmpTasksViewModel task = context.EmpTasks.Where(t => t.Id == Id)
                .Select(t => new EmpTasksViewModel
                {
                    Id = t.Id,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    TaskCat = t.TaskCat,
                    TaskNo = t.TaskNo,
                    AssignedTime = t.AssignedTime,
                    Attachments = HrContext.GetAttachments("EmpTasksForm", t.Id),
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Duration = t.Duration,
                    EmpListId = t.EmpListId,
                    ExpectDur = t.ExpectDur,
                    Unit = t.Unit,
                    Required = t.Required,
                    EmpId = t.EmpId
                }).FirstOrDefault();

            return task;
        }

        public IEnumerable ReadEmployeeTasks(int CompanyId,int empId, string culture)
        {
            var tasks = context.EmpTasks.Where(t => t.EmpId == empId && t.Status == 1 && t.CompanyId == CompanyId).OrderBy(t => t.Priority).Select(t => new NavBarItemVM
            {
                Id = t.Id,
                From = HrContext.GetLookUpCode("EmpTaskCat", t.TaskCat.Value, culture),
                Message = t.Description,
                MoreInfo = HrContext.TrlsName(t.EmpChklist.Employee.Title + " " + t.EmpChklist.Employee.FirstName + " " + t.EmpChklist.Employee.Familyname, culture),
                PicUrl = (t.Manager.HasImage ? t.ManagerId + ".jpeg" : "noimage.jpg"),
            }).ToList();

            return tasks.Take(tasks.Count > 5 ? 5 : tasks.Count);
        }

        public IQueryable<EmpTasksViewModel> ReadEmployeeTasksGrid(int empId, string culture)
        {
            var tasks = context.EmpTasks.Where(t => t.EmpId == empId && t.Status != 0).OrderBy(t => new { t.Status, t.Priority }).Select(t => new EmpTasksViewModel
            {
                Id = t.Id,
                EmpList = HrContext.TrlsName(t.EmpChklist.Name, culture),
                EmpListId = t.EmpListId,
                Manager = HrContext.TrlsName(t.Manager.Title + " " + t.Manager.FirstName + " " + t.Manager.Familyname, culture),
                Description = t.Description,
                Priority = t.Priority,
                Status = t.Status,
                TaskCategory = HrContext.GetLookUpCode("EmpTaskCat", t.TaskCat.Value, culture),
                TaskNo = t.TaskNo,
                AssignedTime = t.AssignedTime,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                Duration = t.Duration,
                ExpectDur = t.ExpectDur,
                Unit = t.Unit,
                Required = t.Required,
                SubPeriodId = t.SubPeriodId,
                SubPeriod = t.SubPeriod.Name
            });
            return tasks;
        }
        public EmpTasksViewModel GetEmployeeTask(int Id, string culture) {
            EmpTasksViewModel task = context.EmpTasks.Where(t => t.Id == Id)
                .Select(t => new EmpTasksViewModel
                {
                    Id = t.Id,
                    EmpList = t.EmpChklist.Name,
                    Manager = HrContext.TrlsName(t.Manager.Title + " " + t.Manager.FirstName + " " + t.Manager.Familyname, culture),
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    TaskCategory = HrContext.GetLookUpCode("EmpTaskCat", t.TaskCat.Value, culture),
                    TaskNo = t.TaskNo,
                    AssignedTime = t.AssignedTime,
                    Attachments = HrContext.GetAttachments("EmpTasksForm", t.Id),
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Duration = t.Duration,
                    EmpListId = t.EmpListId,
                    ExpectDur = t.ExpectDur,
                    Unit = t.Unit,
                    Required = t.Required,
                    Employee = HrContext.TrlsName(t.EmpChklist.Employee.Title + " " + t.EmpChklist.Employee.FirstName + " " + t.EmpChklist.Employee.Familyname, culture),
                }).FirstOrDefault();

            return task;
        }

        public void AssignNextTask(EmpTask currentTask)
        {
            EmpTask nextTask = context.EmpTasks
                .Where(t => t.Status == 0 && t.EmpId == currentTask.EmpId)
                .OrderBy(t => new { t.Priority, t.AssignedTime }).FirstOrDefault();

            if (nextTask != null)
            {
                nextTask.Status = 1;

                context.EmpTasks.Attach(nextTask);
                context.Entry(nextTask).State = EntityState.Modified;
            }
        }

        //Employee Charts
        public IEnumerable EmpTasksByPerid(int empId, int companyId, string culture)
        {
            int? subPeriod;
            GetTaskSubPeriod(companyId, DateTime.Now, culture, out subPeriod);

            var query = (from s in context.SubPeriods where s.Id == subPeriod
                         join t in context.EmpTasks on s.PeriodId equals t.SubPeriod.PeriodId
                         where t.EmpId == empId 
                         group t by t.Status into g
                         select new ChartViewModel {
                             category = g.Key.ToString(),
                             value = g.Count()
                         }).ToList();

            return GetStatus(query, culture);
        }
        public IEnumerable EmpTasksBySubPeriod(int empId, int companyId, string culture)
        {
            int? subPeriod;
            GetTaskSubPeriod(companyId, DateTime.Now, culture, out subPeriod);

            var query = (from s in context.SubPeriods where s.Id == subPeriod
                         join t in context.EmpTasks on s.PeriodId equals t.SubPeriod.PeriodId
                         where t.EmpId == empId
                         group t by new { t.Status, SubPeriod = t.SubPeriod.Name } into g
                         select new ChartViewModel
                         {
                             myGroup = g.Key.SubPeriod,
                             category = g.Key.Status.ToString(),
                             value = g.Count(),
                         }).ToList();

            return GetStatus(query, culture);
        }
        public IEnumerable ManagerEmployeeTask(int  MangerId, int companyId, string culture)
        {
            int? subPeriod;
            GetTaskSubPeriod(companyId, DateTime.Now, culture, out subPeriod);
            var query = (from e in context.EmpTasks
                         where e.ManagerId==MangerId
                         join s in context.SubPeriods on e.SubPeriod.PeriodId equals s.PeriodId
                         where s.Id == subPeriod
                         join p in context.People on e.EmpId equals p.Id
                         group e by new { e.Status, e.EmpId, p.Title, p.Familyname, p.FirstName } into j
                         select new ChartViewModel
                         {
                             category = j.Key.Status.ToString(),
                             EmpId = j.Key.EmpId.Value,
                             value = j.Count(),
                              myGroup= HrContext.TrlsName(j.Key.Title + " " + j.Key.FirstName + " " + j.Key.Familyname, culture),
                         }).ToList();
            //  return query;
            return GetStatus(query, culture);
        }

        private IEnumerable GetStatus(IEnumerable<ChartViewModel> query, string culture)
        {
            foreach (var item in query)
            {
                switch (item.category)
                {
                    case "1":
                        item.category = MsgUtils.Instance.Trls(culture, "AssignTo");
                        item.color = "#42a7ff";
                        break;
                    case "2":
                        item.category = MsgUtils.Instance.Trls(culture, "Done");
                        item.color = "#B0D877";
                        break;
                    case "3":
                        item.category = MsgUtils.Instance.Trls(culture, "Canceled");
                        item.color = "#ededed";
                        break;
                    case "4":
                        item.category = MsgUtils.Instance.Trls(culture, "NotDone");
                        item.color = "#d54c7e";
                        break;
                }
            }
            return query;
        }
        public string GetTaskSubPeriod(int CompanyId, DateTime? AssignTime, string Culture, out int? subPeriodId)
        {
            subPeriodId = 0;
            int? CalenderId = GetPersonSetup(CompanyId).TaskPeriodId;

            if (CalenderId == null)
                return MsgUtils.Instance.Trls(Culture, "SelectCalender");
            else
            {
                int? InPeriodId = GetPeriodId(CalenderId, AssignTime);
                if (InPeriodId == null)
                    return MsgUtils.Instance.Trls(Culture, "NotInCalender");
                else
                    subPeriodId = InPeriodId;
            }
            return "";
        }

        #endregion

        public DataSource<EmpTaskViewModel> AddSubperiods(int CompanyId,DateTime? AssignTime,int Id,short Count,string Culture)
        {
            DataSource<EmpTaskViewModel> DataSource = new DataSource<EmpTaskViewModel>();
            DataSource.Errors = new List<Error>();

            int? CalenderId = GetPersonSetup(CompanyId).TaskPeriodId;
            if (CalenderId == null)
                DataSource.Errors.Add(new Error { id = Id, row = Count, errors = new List<ErrorMessage>() { new ErrorMessage { field = "AssignedTime", message = MsgUtils.Instance.Trls(Culture, "SelectCalender") } } });
            else
            {
                int? InPeriodId = GetPeriodId(CalenderId, AssignTime);
                if (InPeriodId == null)
                {
                    DataSource.Errors.Add(new Error { id = Id, row = Count, errors = new List<ErrorMessage>() { new ErrorMessage { field = "AssignedTime", message = MsgUtils.Instance.Trls(Culture, "NotInCalender") } } });
                }
                else
                {
                    DataSource.Total = InPeriodId != null ? InPeriodId.Value :0;
                }
            }
            return DataSource;
        }
        public void AddEmpTask( List<CheckListTaskViewModel> chlist,string UserName,EmpChkList Emplist)
        {
            foreach (var item in chlist)
            {
                EmpTask emTask = new EmpTask()
                {
                    Unit = item.Unit,
                    CreatedTime = DateTime.Now,
                    CreatedUser = UserName,
                    Description = item.Description,
                    EmpId = item.EmpId,
                    ExpectDur = item.ExpectDur,
                    TaskCat = item.TaskCat,
                    TaskNo = item.TaskNo,
                    Status = 0,
                    Priority = item.Priority,
                    Required = item.Required,
                    EmpChklist = Emplist

                };
                if (item.Priority == 0)
                {
                    emTask.Status = 1;
                    emTask.AssignedTime = DateTime.Now;
                }
                context.EmpTasks.Add(emTask);
            }
        }
        public EmpChkList AddEmpChlst(CheckList checklist, string UserName, int? EmpId,int CompanyId)
        {
            EmpChkList Emplist = new EmpChkList();

            Emplist.CreatedTime = DateTime.Now;
            Emplist.Name = checklist.Name;
            Emplist.EmpId = EmpId;
            Emplist.CreatedUser = UserName;
            Emplist.Description = checklist.Description;
            Emplist.ListEndDate = checklist.EndDate;
            Emplist.ListId = checklist.Id;
            Emplist.ListStartDate = checklist.StartDate;
            Emplist.ListType = checklist.ListType;
            Emplist.Status = 0;
            Emplist.CompanyId = CompanyId;

            return Emplist;
        }
        public IQueryable<EmpChkListViewModel> GetEmpCheckLists(string culture, int companyId)
        {
            var EmpList = from el in context.EmpChkLists
                          where el.CompanyId == companyId
                          join p in context.People on el.EmpId equals p.Id into g
                          from Ecl in g.DefaultIfEmpty()
                          select new EmpChkListViewModel
                          {
                              Id = el.Id,
                              EmpId = el.EmpId,
                              ListEndDate = el.ListEndDate,
                              ListStartDate = el.ListStartDate,
                              PicUrl = (Ecl.HasImage ? "/SpecialData/Photos/"+companyId+"/" +el.EmpId.Value + ".jpeg" : "/SpecialData/Photos/noimage.jpg"),
                              ListType = el.ListType,
                              Name = el.Name,
                              Employee = HrContext.TrlsName(Ecl.Title + " " + Ecl.FirstName + " " + Ecl.Familyname, culture),
                              Status = el.Status,
                              CreatedUser = el.CreatedUser,
                              ManagerId=el.ManagerId,
                              Count= context.EmpTasks.Where(a => a.EmpListId == el.Id).Count(),
                              PrograssBar = context.EmpTasks.Where(a => a.EmpListId == el.Id && a.Status==2).Count() 
                          };
            return EmpList;
        }
        public CheckList GetTermCheckLists(int company)
        {
            var autotermiation = GetPersonSetup(company).AutoTermiation;
            CheckList query = autotermiation ? (from c in context.CheckLists
                        where c.CompanyId == company && c.ListType == 3 && c.Default
                        select c).FirstOrDefault() : null;

            return query;
        }
        public IQueryable<CheckListViewModel> GetCheckLists(string culture,int company)
        {

            var result = from l in context.CheckLists
                         where ((l.IsLocal && l.CompanyId == company) || l.IsLocal == false) && (l.StartDate <= DateTime.Today && (l.EndDate == null || l.EndDate >= DateTime.Today))
                         select new CheckListViewModel
                         {
                             Id = l.Id,
                             Name = l.Name,
                             Default=l.Default,
                             Description=l.Description,
                             Duration=l.Duration,
                             LocalName = HrContext.TrlsName(l.Name, culture),
                             StartDate = l.StartDate,
                             EndDate = l.EndDate,
                         };
            return result;
        }
        public CheckListFormViewModel ReadCheckList(int id, string culture)
        {
            var Job = (from J in context.CheckLists
                       where J.Id == id
                       select new CheckListFormViewModel
                       {
                           Id = J.Id, 
                           Name = J.Name,
                           IsLocal = J.IsLocal,
                           LocalName = HrContext.TrlsName(J.Name, culture),
                           StartDate = J.StartDate,
                           EndDate = J.EndDate,
                           Duration=J.Duration,
                           CompanyId=J.CompanyId,
                           Default=J.Default,
                           Description=J.Description,
                           ListType=J.ListType,
                           ModifiedTime = J.ModifiedTime,
                           ModifiedUser = J.ModifiedUser,
                           CreatedTime = J.CreatedTime,
                           CreatedUser = J.CreatedUser,
                       }).FirstOrDefault();

            return Job;
        }
        public void Add(ChecklistTask checklistTask)
        {
            context.ChecklistTasks.Add(checklistTask);
        }
        public void Attach(ChecklistTask checklistTask)
        {
            context.ChecklistTasks.Attach(checklistTask);
        }
        public DbEntityEntry<ChecklistTask> Entry(ChecklistTask checklistTask)
        {
            return Context.Entry(checklistTask);
        }
        public void Remove(ChecklistTask checklistTask)
        {
            if (Context.Entry(checklistTask).State == EntityState.Detached)
            {
                context.ChecklistTasks.Attach(checklistTask);
            }
            context.ChecklistTasks.Remove(checklistTask);
        }
        public IQueryable<CheckListTaskViewModel> ReadCheckListTask(int ListId)
        {
            var result = from T in context.ChecklistTasks
                         where T.ListId == ListId
                         select new CheckListTaskViewModel
                         {
                             Id = T.Id,
                             EmpId=T.EmpId,
                             Priority=T.Priority,
                             Required=T.Required,
                             TaskCat=T.TaskCat,
                             TaskNo=T.TaskNo,
                             Unit=T.Unit,
                             ListId=T.ListId,
                             Description=T.Description,
                             ExpectDur =T.ExpectDur,
                             CreatedTime=T.CreatedTime,
                             CreatedUser=T.CreatedUser,
                             ModifiedUser=T.ModifiedUser,
                             ModifiedTime=T.ModifiedTime
                             
                             
                         };
            return result;
        }
        public int? GetPeriodId(int? calenderId, DateTime? AssignTime)
        {
            var per = context.Periods.Where(a => a.CalendarId == calenderId).ToList();
            int? inper = null;
            foreach (var item in per)
            {
                var sub = context.SubPeriods.Where(a => a.PeriodId == item.Id && a.StartDate <= AssignTime && a.EndDate >= AssignTime).FirstOrDefault();
                if (sub != null)
                {
                    inper = sub.Id;
                    break;
                }
            }
            if (inper == null)
                return null;
            else
                return inper;
        }
        public void Add(EmpChkList checklistTask)
        {
            context.EmpChkLists.Add(checklistTask);
        }

        public void Add(EmpTask checklistTask)
        {
            context.EmpTasks.Add(checklistTask);
        }

        public void Attach(EmpTask checklistTask)
        {
            context.EmpTasks.Attach(checklistTask);
        }

        public void Attach(EmpChkList checklistTask)
        {
            context.EmpChkLists.Attach(checklistTask);
        }

        public DbEntityEntry<EmpTask> Entry(EmpTask checklistTask)
        {
            return Context.Entry(checklistTask);
        }

        public DbEntityEntry<EmpChkList> Entry(EmpChkList checklistTask)
        {
            return Context.Entry(checklistTask);
        }

        public void Remove(EmpChkList checklistTask)
        {
            if (Context.Entry(checklistTask).State == EntityState.Detached)
            {
                context.EmpChkLists.Attach(checklistTask);
            }
            context.EmpChkLists.Remove(checklistTask);
        }

        public void Remove(EmpTask checklistTask)
        {
            if (Context.Entry(checklistTask).State == EntityState.Detached)
            {
                context.EmpTasks.Attach(checklistTask);
            }
            context.EmpTasks.Remove(checklistTask);
        }

       

       
    }
}

