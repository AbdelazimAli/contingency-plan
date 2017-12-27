using Db.Persistence.Services;
using Interface.Core;
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
using System.Linq.Expressions;
using System.Text;

namespace Db.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        public ICacheManager CacheManager { get; private set; }

        public Repository(DbContext context)
        {
            Context = context;
            CacheManager = new CacheManager();
        }

        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void Attach(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
        }

        public bool IsExist(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Count(predicate) > 0;
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public TEntity Get(int? id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public void Remove(int? id)
        {
            var entity = Context.Set<TEntity>().Find(id);
            if (entity != null) Remove(entity);
        }
        public void Remove(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Set<TEntity>().Attach(entity);
            }
            Context.Set<TEntity>().Remove(entity);
        }

        public PersonSetup GetPersonSetup(int company)
        {
            string key = "PersonSetup" + company;
            var exist = CacheManager.IsSet(key);
            PersonSetup setup;
            if (!exist)
            {
                setup = ((HrContext)Context).PersonSetup.Where(a => a.Id == company).FirstOrDefault();
                if (setup == null) setup = new PersonSetup() { Id = -1 };
                CacheManager.Set(key, setup, 0);
            }
            else
                setup = CacheManager.Get<PersonSetup>(key);

            return setup;
        }

        public void SetPersonSetup(PersonSetup setup)
        {
            string key = "PersonSetup" + setup.Id;
            if (CacheManager.IsSet(key))
                CacheManager.Remove(key);

            CacheManager.Set(key, setup, 0);
        }
        public void RemoveLName(string Culture,string Name)
        {
            var record = Context.Set<NameTbl>().Where(a => a.Culture == Culture && a.Name == Name).FirstOrDefault();
            if (record != null)
                Context.Set<NameTbl>().Remove(record);
        }
        public void AddLName(string culture, string oldName,string newName, string lname)
        {
            // no change 1
            if (!string.IsNullOrEmpty(lname))
            {
                //insertNew 2
                if(oldName==null  && lname!= null)
                {
                    Context.Set<NameTbl>().Add(new NameTbl
                    {
                        Culture = culture,
                        Name = newName,
                        Title = lname
                    });
                }
               
                else
                {
                    var record = Context.Set<NameTbl>().FirstOrDefault(a => a.Culture == culture && (a.Name == oldName || a.Name == newName));
                    //change culture
                    if(record==null)
                    {
                        Context.Set<NameTbl>().Add(new NameTbl
                        {
                            Culture = culture,
                            Name = newName,
                            Title = lname
                        });
                    }
                    
                    else
                    {
                        //no Changes 3 select only
                        if (record.Name == newName && record.Title == lname)
                            return;
                        // select then update 5
                        else if (record.Name==newName && record.Title != lname)
                        {
                            record.Title = lname;
                            Context.Set<NameTbl>().Attach(record);
                            Context.Entry(record).State = EntityState.Modified;
                        }

                        // update Key 6,4
                        else
                        {

                            Context.Set<NameTbl>().Attach(record);
                            Context.Entry(record).State = EntityState.Deleted;
                            Context.Set<NameTbl>().Add(new NameTbl
                            {
                                Culture = culture,
                                Name = newName,
                                Title = lname
                            });
                        }
                    }
                   
                }
            }
        }

        DbEntityEntry<TEntity> IRepository<TEntity>.Entry(TEntity entity)
        {
            return Context.Entry(entity);
        }

   //     public static Expression<Func<TEntity, bool>> IsCurrent<TEntity>()
   //where TEntity : IValidFromTo
   //     {
   //         return e => (e.ValidFrom == null || e.ValidFrom <= DateTime.Now) &&
   //                     (e.ValidTo == null || e.ValidTo >= DateTime.Now);
   //     }

        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties
                .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy?.Invoke(query).ToList() ?? query.ToList();
        }

        public IEnumerable GetLookUpCode(string culture, string code)
        {
            var list = Context.Set<LookUpTitles>()
                .Where(c => c.Culture == culture && c.CodeName == code)
                .Select(c => new { id = c.CodeId, name = c.Title })
                .ToList();

            if (list.Count() == 0)
            {
                list = Context.Set<LookUpCode>()
                .Where(c => c.CodeName == code)
                .Select(c => new { id = c.CodeId, name = c.Name })
                .ToList();
            }

            return list;
        }
        public IEnumerable GetGridLookUpCode(string culture, string code)
        {
            var list = Context.Set<LookUpTitles>()
                .Where(c => c.Culture == culture && c.CodeName == code)
                .Select(c => new { value = c.CodeId, text = c.Title })
                .ToList();

            if (list.Count() == 0)
            {
                list = Context.Set<LookUpCode>()
                .Where(c => c.CodeName == code)
                .Select(c => new { value = c.CodeId, text = c.Name })
                .ToList();
            }

            return list;
        }

        public WfTrans AddWorkFlow(WfViewModel wf, string culture)
        {
            var Db = Context as HrContext;
            var record = (from w in Db.RequestWf
                        where w.Source == wf.Source && w.SourceId == wf.SourceId
                        join t in Db.WfTrans on w.Id equals t.WFlowId into g
                        from t in g.Where(a => a.DocumentId == wf.DocumentId).DefaultIfEmpty()
                        select new AddWfViewModel
                        {
                            WorkFlowId = w.Id,
                            Id = t.Id,
                            Order = t.Order,
                            Sequence = t.Sequence,
                            RoleId = t.RoleId,
                            CodeId = t.CodeId,
                            ApprovalStatus = t.ApprovalStatus,
                            AuthBranch = t.AuthBranch,
                            AuthDept = t.AuthDept,
                            AuthEmp = t.AuthEmp,
                            AuthPosition = t.AuthPosition,
                            HeirType = w.HeirType,
                            Hierarchy = w.Hierarchy,
                            NofApprovals = w.NofApprovals,
                            Source = w.Source,
                            SourceId = w.SourceId
                        }).OrderByDescending(o => o.Id).FirstOrDefault();

            if (record == null)
            {
                wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "UndefinedWorkFlow");
                return null;
            }

            wf.WFlowId = record.WorkFlowId;

            // 1- New 2-Submit 3-Employee Review 4-Managre Review 5-Accept 6-Approved 7-Cancel before approved 8-Cancel after approved 9-Rejected 
            int Order = (record.Order == null ? 0 : record.Order.Value );
            if (wf.ApprovalStatus == 2) Order = 0;

            if (record.ApprovalStatus > 6)
            {
                wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "CanceledRequest");
                return null;
            }

            if ((wf.ApprovalStatus == 3 || wf.ApprovalStatus == 4) && wf.ManagerId == null && wf.BackToEmployee == false)
            {
                wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "MustToEmpOrManager");
                return null;
            }

            var employee = Db.Assignments.Where(a => a.EmpId == wf.RequesterEmpId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Select(a => new { a.CompanyId, a.DepartmentId, a.PositionId, a.ManagerId, a.BranchId, a.SectorId}).FirstOrDefault();
            if (employee == null)
            {
                wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "NotActiveEmp");
                return null; // return if requestor employee is not active
            }

            var trans = new WfTrans
            {
                WFlowId = record.WorkFlowId,
                ApprovalStatus = wf.ApprovalStatus,
                DocumentId = wf.DocumentId,
                CreatedUser = wf.CreatedUser,
                CreatedTime = DateTime.Now,
                BranchId = employee.BranchId,
                DeptId = employee.DepartmentId,
                EmpId = wf.RequesterEmpId,
                PositionId = employee.PositionId,
                SectorId = employee.SectorId,
                CompanyId = employee.CompanyId,
                Source = record.Source,
                SourceId = record.SourceId,
                Sequence = (short)(record.Sequence == null ? 1 : record.Sequence + 1)
            };

            if (wf.ApprovalStatus == 3) // Employee Review
            {
                trans.CodeId = 1; // Employee
                trans.AuthEmp = wf.RequesterEmpId;
                trans.Order = 1;
                return trans;
            }

            if (wf.ApprovalStatus == 4) // Manager Review
            {
                trans.CodeId = wf.ManagerId == 0 ? 2 : (short?)null;
                AssignmentVM manager;
                if (wf.ManagerId.Value == 0) // direct manager
                    manager = Db.Assignments.Where(a => a.DepartmentId == employee.DepartmentId && a.IsDepManager && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Select(a => new AssignmentVM { BranchId = a.BranchId, EmpId = a.EmpId, PositionId = a.PositionId, DepartmentId = a.DepartmentId }).FirstOrDefault();
                else
                    manager = Db.Assignments.Where(a => a.EmpId == wf.ManagerId.Value && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Select(a => new AssignmentVM { BranchId = a.BranchId, EmpId = a.EmpId, PositionId = a.PositionId, DepartmentId = a.DepartmentId }).FirstOrDefault();

                if (manager != null)
                {
                    trans.AuthEmp = manager.EmpId;
                    trans.AuthDept = manager.DepartmentId;
                    trans.AuthPosition = manager.PositionId;
                    trans.AuthBranch = manager.BranchId;
                    return trans;
                }

                return null;
            }

            // Cancel or Reject
            if (wf.ApprovalStatus > 6)
            {
                trans.Order = (byte)Order;
                trans.CodeId = record.CodeId;
                trans.AuthDept = record.AuthDept;
                trans.AuthPosition = record.AuthPosition;
                trans.AuthEmp = record.AuthEmp;
                trans.AuthBranch = record.AuthBranch;
                trans.ApprovalStatus = wf.ApprovalStatus;
                return trans;
            }

            bool SkipIamManager = false;

            // Hierarchy type 1-Org Chart  2-Org Chart Hierarchy  3-Position Hierarchy  4-Employee-Manager
            if (record.HeirType == 1) // 1-Org Chart
            {
                while (true) // loop until update trans or return null
                {
                    Order++;
                    var next = Db.WfRoles.Where(a => a.WFlowId == record.WorkFlowId && a.Order == Order).Select(a => new { a.RoleId, a.CodeId }).FirstOrDefault();
                    if (next == null)
                    { 
                        if (record.Order == null && !SkipIamManager) wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "EmptyWorkflow");
                        return null;
                    }

                    // handle company structure trans
                    trans.CodeId = next.CodeId;
                    trans.Order = (byte)Order;

                    if (next.RoleId != null)
                    {
                        trans.RoleId = next.RoleId;
                        return trans;
                    }

                    if (next.CodeId == null) return null; // Bug in WF defination

                    var code = Db.LookUpUserCodes.FirstOrDefault(a => a.CodeName == "Roles" && a.CodeId == next.CodeId);

                    AssignmentVM assignment = null;
                    switch (code.SysCodeId)
                    {
                        case 1:  // 1-Employee 
                            assignment = new AssignmentVM
                            {
                                EmpId = wf.RequesterEmpId,
                                DepartmentId = employee.DepartmentId,
                                PositionId = employee.PositionId,
                                BranchId = employee.BranchId
                            };
                            break;
                        case 2: // 2-Direct Manager
                            assignment = Db.Assignments.Where(a => a.DepartmentId == employee.DepartmentId && a.IsDepManager && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Select(a => new AssignmentVM { BranchId = a.BranchId, EmpId = a.EmpId, PositionId = a.PositionId, DepartmentId = a.DepartmentId }).FirstOrDefault();
                            if (assignment == null || assignment.EmpId == wf.RequesterEmpId)
                                assignment = Db.Database.SqlQuery<AssignmentVM>("select m.EmpId, m.BranchId, m.PositionId, m.DepartmentId from Assignments m where (GETDATE() between m.AssignDate and m.EndDate) and m.IsDepManager = 1 and m.DepartmentId = (select top 1 B.Id from CompanyStructures A, CompanyStructures B where A.Id = " + employee.DepartmentId + " and B.Id != " + employee.DepartmentId + " and A.CompanyId = B.CompanyId and A.Sort LIKE B.Sort + '%' order by LEN(B.Sort) desc)").FirstOrDefault();
                            break;
                        case 3: //3-Department Head
                            assignment = Db.Database.SqlQuery<AssignmentVM>("select m.EmpId, m.BranchId, m.PositionId, m.DepartmentId from Assignments m where (GETDATE() between m.AssignDate and m.EndDate) and m.IsDepManager = 1 and m.DepartmentId = (select top 1 B.Id from CompanyStructures A, CompanyStructures B where A.Id = " + employee.DepartmentId + " and B.Id != " + employee.DepartmentId + " and A.CompanyId = B.CompanyId and A.Sort LIKE B.Sort + '%' and (B.NodeType in (select C.CodeId from LookUpUserCodes C where C.CodeName = 'CompanyStructure' and C.SysCodeId = 4)) order by LEN(B.Sort) desc)").FirstOrDefault();
                            break;
                        case 4: //4-Main Department Head
                            assignment = Db.Database.SqlQuery<AssignmentVM>("select m.EmpId, m.BranchId, m.PositionId, m.DepartmentId from Assignments m where (GETDATE() between m.AssignDate and m.EndDate) and m.IsDepManager = 1 and m.DepartmentId = (select top 1 B.Id from CompanyStructures A, CompanyStructures B where A.Id = " + employee.DepartmentId + " and B.Id != " + employee.DepartmentId + " and A.CompanyId = B.CompanyId and A.Sort LIKE B.Sort + '%' and (B.NodeType in (select C.CodeId from LookUpUserCodes C where C.CodeName = 'CompanyStructure' and C.SysCodeId = 4)) order by LEN(B.Sort))").FirstOrDefault();
                            break;
                        case 5: //5-Sector Head
                            assignment = Db.Database.SqlQuery<AssignmentVM>("select m.EmpId, m.BranchId, m.PositionId, m.DepartmentId from Assignments m where (GETDATE() between m.AssignDate and m.EndDate) and m.IsDepManager = 1 and m.DepartmentId = (select top 1 B.Id from CompanyStructures A, CompanyStructures B where A.Id = " + employee.DepartmentId + " and B.Id != " + employee.DepartmentId + " and A.CompanyId = B.CompanyId and A.Sort LIKE B.Sort + '%' and (B.NodeType in (select C.CodeId from LookUpUserCodes C where C.CodeName = 'CompanyStructure' and C.SysCodeId = 3)) order by LEN(B.Sort)) desc").FirstOrDefault();
                            break;
                        case 6: //6-Branch Manager
                            assignment = Db.Database.SqlQuery<AssignmentVM>("select m.EmpId, m.BranchId, m.PositionId, m.DepartmentId from Assignments m where (GETDATE() between m.AssignDate and m.EndDate) and m.IsDepManager = 1 and m.DepartmentId = (select top 1 B.Id from CompanyStructures A, CompanyStructures B where A.Id = " + employee.DepartmentId + " and B.Id != " + employee.DepartmentId + " and A.CompanyId = B.CompanyId and A.Sort LIKE B.Sort + '%' and (B.NodeType in (select C.CodeId from LookUpUserCodes C where C.CodeName = 'CompanyStructure' and C.SysCodeId = 2)) order by LEN(B.Sort) desc)").FirstOrDefault();
                            break;
                        case 7: //7-Main Branch Head
                            assignment = Db.Database.SqlQuery<AssignmentVM>("select m.EmpId, m.BranchId, m.PositionId, m.DepartmentId from Assignments m where (GETDATE() between m.AssignDate and m.EndDate) and m.IsDepManager = 1 and m.DepartmentId = (select top 1 B.Id from CompanyStructures A, CompanyStructures B where A.Id = " + employee.DepartmentId + " and B.Id != " + employee.DepartmentId + " and A.CompanyId = B.CompanyId and A.Sort LIKE B.Sort + '%' and (B.NodeType in (select C.CodeId from LookUpUserCodes C where C.CodeName = 'CompanyStructure' and C.SysCodeId = 2)) order by LEN(B.Sort))").FirstOrDefault();
                            break;
                        default:
                            wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "CodeIdDeleted"); // next.CodeId
                            return null;
                    }

                    if (assignment != null)
                    {
                        if (wf.RequesterEmpId == assignment.EmpId)
                        {
                            SkipIamManager = true;
                            continue;
                        }

                        trans.AuthDept = assignment.DepartmentId;
                        trans.AuthPosition = assignment.PositionId;
                        trans.AuthEmp = assignment.EmpId;
                        trans.AuthBranch = assignment.BranchId;
                        return trans;
                    }

                    SkipIamManager = false;
                }
            } else if (record.HeirType == 2 && record.NofApprovals > Order) {  // 2-Org Chart Hierarchy
                int? dept = employee.DepartmentId;
                if (wf.ApprovalStatus == 2) // first record of approval get direct manager
                {
                    trans.CodeId = 2;
                    trans.Order = 1;
                }
                else // Accept
                {
                    dept = record.AuthDept == null ? employee.DepartmentId : record.AuthDept;
                    Order++;
                    trans.Order = (byte)Order;
                    if (Order > record.NofApprovals) dept = null;
                }

                while (dept != null)
                {
                    var manager = Db.Assignments.Where(a => a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today && a.IsDepManager && a.DepartmentId == dept).Select(a => new { a.EmpId, a.DepartmentId, a.BranchId, a.PositionId }).FirstOrDefault();
                    if (manager != null)
                    {
                        if (wf.RequesterEmpId != manager.EmpId)
                        {
                            trans.AuthDept = manager.DepartmentId;
                            trans.AuthEmp = manager.EmpId;
                            trans.AuthBranch = manager.BranchId;
                            trans.AuthPosition = manager.PositionId;
                            return trans;
                        }
                    }
                    dept = Db.CompanyStructures.Where(a => a.Id == dept).Select(a => a.ParentId).FirstOrDefault();
                }
            } else if (record.HeirType == 3 && record.NofApprovals > Order) { // 3-Position Hierarchy
                if (employee.PositionId == null)
                {
                    wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "EmpPosIfPosHierarchy");
                    return null;
                }

                int? pos = employee.PositionId;
                bool second = false;

                if (wf.ApprovalStatus == 2) // Parent Position
                {
                    trans.Order = 1;
                    trans.CodeId = 2;
                }
                else // Accept
                {
                    Order++;
                    trans.Order = (byte)Order;
                    if (Order <= record.NofApprovals)
                        pos = record.AuthPosition == null ? employee.PositionId : record.AuthPosition;
                    else
                        pos = null; // stop don't continue
                }

                while (pos != null)
                {
                    if (record.Hierarchy == null)
                    {
                        var position = Db.Positions.Where(a => a.Id == pos).Select(a => a.Supervisor).FirstOrDefault();
                        if (position == null)
                        {
                            if (second)
                                wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "CantFindActiveSupervisor");
                            else
                                wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "EmpSupervisorIfPosHierarchy");
                            return null;
                        }

                        pos = position;
                    }
                    else
                    {
                        var position = Db.DiagramNodes.Where(a => a.DiagramId == record.Hierarchy && a.ChildId == pos).Select(a => a.ParentId).FirstOrDefault();
                        if (position == 0)
                        {
                            if (second)
                                wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "CantFindActiveParentPos");
                            else
                                wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "CantFindEmpPos");
                            return null;
                        }

                        pos = position;
                    }

                    second = true;

                    // check if position has active assignments
                    var manager = Db.Assignments.Where(a => a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today && a.PositionId == pos).Select(a => new { a.EmpId, a.DepartmentId, a.BranchId, a.PositionId, a.AssignDate }).OrderBy(a => a.AssignDate).FirstOrDefault();
                    if (manager != null)
                    {
                        if (wf.RequesterEmpId == manager.EmpId)
                        {
                            SkipIamManager = true;
                            continue;
                        }

                        trans.AuthDept = manager.DepartmentId;
                        trans.AuthEmp = manager.EmpId;
                        trans.AuthBranch = manager.BranchId;
                        trans.AuthPosition = manager.PositionId;
                        return trans;
                    }

                    SkipIamManager = false;
                }
            } else if (record.HeirType == 4 && record.NofApprovals > Order && employee.ManagerId != null) { // 4-Employee-Manager
                //if (employee.ManagerId == null)
                //{
                //    wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "EmpDoesntHaveManager");
                //    return null;
                //}

                int? emp = employee.ManagerId;

                if (wf.ApprovalStatus == 2)
                {
                    trans.CodeId = 2;
                    trans.Order = 1;
                }
                else
                {
                    Order++;
                    trans.Order = (byte)Order;
                    if (Order <= record.NofApprovals)
                        emp = record.AuthEmp == null ? employee.ManagerId : record.AuthEmp;
                    else
                        emp = null; // stop don't continue
                }

                // loop until find manager
                while (emp != null)
                {
                    // get last assignment record for manager
                    var manager = Db.Assignments.Where(a => a.EmpId == emp).Select(a => new { a.Id,a.AssignDate,a.EndDate, a.ManagerId }).OrderByDescending(a => a.Id).FirstOrDefault();

                    // if manager is not active foreword document to his manager
                    if (!(manager.AssignDate <= DateTime.Today && manager.EndDate >= DateTime.Today))
                    {
                        // Employee manager is not active, and he doesn't have manager
                        if (manager.ManagerId == null)
                        {
                            wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "EmpManagerNotActive");
                            return null;
                        }

                        emp = manager.ManagerId;
                    }
                    else // is active
                    {
                        var assignment = Db.Assignments.Where(a => a.EmpId == emp && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Select(a => new { a.DepartmentId, a.PositionId, a.EmpId, a.BranchId }).FirstOrDefault();

                        if (wf.RequesterEmpId == assignment.EmpId)
                        {
                            SkipIamManager = true;
                            continue;
                        }

                        trans.AuthDept = assignment.DepartmentId;
                        trans.AuthPosition = assignment.PositionId;
                        trans.AuthEmp = assignment.EmpId;
                        trans.AuthBranch = assignment.BranchId;
                        return trans;
                    }

                    SkipIamManager = false;
                }
            }

            // Handle HR transactions
            if (record.HeirType != 1)
            {
                Order = Order - (record.NofApprovals != null ? record.NofApprovals.Value : 0);
                if (Order <= 0) Order = 0;
                if (wf.ApprovalStatus == 2)
                    Order = 1; // submit
                else if (wf.ApprovalStatus == 5)
                    Order++; // Accept

                WfRole nextRole = null;
                do
                {
                    nextRole = Db.WfRoles.FirstOrDefault(a => a.WFlowId == record.WorkFlowId && a.Order == Order);
                    if (nextRole != null && nextRole.RoleId != null && nextRole.RoleId != record.RoleId)
                    {
                        if(record.NofApprovals != null) Order += record.NofApprovals.Value;
                        trans.Order = (byte)Order;
                        trans.RoleId = nextRole.RoleId;
                        return trans;
                    }
                    Order++;
                } while (nextRole != null);
            }

            if (record.Order == null) wf.WorkFlowStatus = MsgUtils.Instance.Trls(culture, "EmptyWorkflow");
            return null;
        }

        public WfTrans GetWorkFlow(string Source, int SourceId, int DocumentId)
        {
            var Db = Context as HrContext;
            return (from w in Db.RequestWf
                          where w.Source == Source && w.SourceId == SourceId
                          join t in Db.WfTrans on w.Id equals t.WFlowId into g
                          from t in g.Where(a => a.DocumentId == DocumentId).DefaultIfEmpty()
                          orderby t.Id
                          select t).LastOrDefault();
        }

        public IList<Model.ViewModel.Personnel.RolesViewModel> GetOrgChartRoles(string culture)
        {
            var pos = Context.Set<LookUpUserCode>()
                .Where(c => c.CodeName == "Roles")
                .Select(c => new Model.ViewModel.Personnel.RolesViewModel { RoleId = null, CodeId = c.CodeId, text = HrContext.GetLookUpUserCode("Roles", c.CodeId, culture) })
                .Union(Context.Set<Role>().Select(r => new Model.ViewModel.Personnel.RolesViewModel { RoleId = r.Id, CodeId = null, text = r.Name }))
                .ToList();

            return pos;
        }

        public IQueryable<FlexDataViewModel> GetFlexData(int companyId, string objectName, byte version, string culture,  int SourceId)
        {
            var query = (// where fc.PageId == HrContext.GetPageId(companyId, objectName, version) && fc.isVisible == true
                         from p in Context.Set<PageDiv>()
                         where p.CompanyId == companyId && p.ObjectName == objectName && p.Version == version
                         join fc in Context.Set<FlexColumn>() on p.Id equals fc.PageId
                         where fc.isVisible
                         join fd in Context.Set<FlexData>() on new { fc.PageId, fc.ColumnName } equals new { fd.PageId, fd.ColumnName } into g1
                         from fd in g1.Where(b => b.SourceId == SourceId).DefaultIfEmpty()
                         join code in Context.Set<LookUpCode>() on fd.ValueId equals code.Id into g2
                         from code in g2.DefaultIfEmpty()
                         join t in Context.Set<LookUpTitles>() on new { code.CodeName, code.CodeId } equals new { t.CodeName, t.CodeId } into g3
                         from t in g3.Where(a => a.Culture == culture).DefaultIfEmpty()
                         select new FlexDataViewModel
                         {
                             ColumnName = fc.ColumnName,
                             Title = HrContext.GetColumnTitle(companyId, culture, objectName, version, fc.ColumnName) ?? fc.ColumnName,
                             Id = fd.Id,
                             PageId = fc.PageId,
                             SourceId = SourceId,
                             Value = fd.Value,
                             ValueId = fd.ValueId,
                             CodeName = fc.CodeName,
                             InputType = fc.InputType,
                             IsUnique = fc.IsUnique,
                             Max = fc.Max,
                             Min = fc.Min,
                             Pattern = fc.Pattern,
                             PlaceHolder = fc.PlaceHolder,
                             Required = fc.Required,
                             UniqueColumns = fc.UniqueColumns,
                             ValueText = (fc.InputType == 3 && code != null ? (t == null ? code.Name : t.Title) : fd.Value),
                             ObjectName = objectName,
                             TableName = p.TableName,
                             Version = version
                         }).ToList();

            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culture);
            foreach (var r in query)
                if (r.InputType == 4 && !String.IsNullOrEmpty(r.ValueText)) r.ValueText = Convert.ToDateTime(r.ValueText).ToString("d", ci);
                else if (r.InputType == 6 && !String.IsNullOrEmpty(r.ValueText)) r.ValueText = Convert.ToDateTime(r.ValueText).ToShortTimeString();

            return query.AsQueryable();
        }

        public IQueryable<AuditViewModel> GetLog(int companyId, string[] objects, byte version, string culture, string Id)
        {
            var Db = Context as HrContext;
            var query = from a in Db.AuditTrail
                        where a.CompanyId == companyId && objects.Contains(a.ObjectName) && a.Version == version && a.SourceId == Id
                        join c in Db.Companies on a.CompanyId equals c.Id
                        join t in Db.ColumnTitles on new { a.CompanyId, a.ObjectName, a.Version, a.ColumnName }
                        equals new { t.CompanyId, t.ObjectName, t.Version, t.ColumnName } into g
                        from t in g.Where(b => b.Culture == culture).DefaultIfEmpty()
                        select new AuditViewModel
                        {
                            ColumnName = t == null ? a.ColumnName : t.Title,
                            ValueAfter = a.ValueAfter,
                            ValueBefore = a.ValueBefore,
                            ModifiedUser = a.ModifiedUser,
                            ModifiedTime = a.ModifiedTime,
                            ObjectName = a.ObjectName,
                            Company = c.Name
                        };

            return query;
        }
        public int GetIntResultFromSql(string Sql)
        {
            return ((Db.HrContext)Context).Database.SqlQuery<int>(Sql, "").Single();
        }

        //Check
        public List<Error> CheckForm(CheckParm parm)
        {
            var formcolumns = ((Db.HrContext)Context).FormsColumns
               .Where(info => info.Section.FieldSet.Page.CompanyId == parm.CompanyId && info.Section.FieldSet.Page.ObjectName == parm.ObjectName && info.Section.FieldSet.Page.Version == 0)
               .ToList();

            var query = from info in formcolumns
                        join column in parm.Columns on info.ColumnName equals column.Name
                        orderby info.ColumnOrder, column.Cell
                        select new ColumnInfoViewModel
                        {
                            Row = column.Row,
                            Cell = column.Cell,
                            Value = column.Value,
                            ColumnName = info.ColumnName,
                            ColumnType = info.ColumnType,
                            Required = info.Required,
                            Max = info.Max,
                            Min = info.Min,
                            MaxLength = info.MaxLength,
                            MinLength = info.MinLength,
                            Pattern = info.Pattern,
                            IsUnique = info.IsUnique,
                            UniqueColumns = info.UniqueColumns
                        };

            return CheckPage(query.ToList(), parm);
        }

        public List<Error> CheckGrid(CheckParm parm)
        {
            var gridcolumns = ((Db.HrContext)Context).GridColumns
                .Where(info => info.Grid.CompanyId == parm.CompanyId && info.Grid.ObjectName == parm.ObjectName && info.Grid.Version == 0)
                .ToList();

            var query = from info in gridcolumns
                        join column in parm.Columns on info.ColumnName equals column.Name
                        orderby info.ColumnOrder, column.Cell
                        select new ColumnInfoViewModel
                        {
                            Row = column.Row,
                            Cell = column.Cell,
                            Value = column.Value,
                            ColumnName = info.ColumnName,
                            ColumnType = info.ColumnType,
                            Required = info.Required,
                            Max = info.Max,
                            Min = info.Min,
                            MaxLength = info.MaxLength,
                            MinLength = info.MinLength,
                            Pattern = info.Pattern,
                            IsUnique = info.IsUnique,
                            UniqueColumns = info.UniqueColumns
                        };

            return CheckPage(query.ToList(), parm);
        }

        public List<Error> CheckPage(IEnumerable<ColumnInfoViewModel> query, CheckParm parm)
        {
            string id = "0", parentId = "0", errors = "";
            short previousRow = -1;
            int previousId = 0;

            var Errors = new List<Error>();
            List<ErrorMessage> errorsList = null;

            foreach (var record in query) // foreach cell
            {
                errors = "";

                if (record.ColumnName == "Id") id = record.Value;
                else if (record.ColumnName == parm.ParentColumn) parentId = record.Value;

                if (record.Required && string.IsNullOrEmpty(record.Value))
                    errors += MsgUtils.Instance.Trls(parm.Culture, "Required") + ";"; // use ; inseated of new line <br/>

                if ((record.Max != null || record.Min != null) && record.ColumnType == "number" && record.Value.Length > 0)
                {
                    decimal val = 0;
                    if (!decimal.TryParse(record.Value, out val))
                        errors += MsgUtils.Instance.Trls(parm.Culture, "IsNotValidNumber") + ";";
                    else
                    {
                        if (record.Max != 0 && val > record.Max)
                            errors += MsgUtils.Instance.Trls(parm.Culture, "CantExceed") + " " + record.Max + ";";
                        if (record.Min != 0 && val < record.Min)
                            errors += MsgUtils.Instance.Trls(parm.Culture, "CantBeLessThan") + " " + record.Min + ";";
                    }
                }

                if (record.MaxLength > 0 && (record.ColumnType == "string" || record.ColumnType == "text" || record.ColumnType == "email" || record.ColumnType == "url") && record.Value.Length > record.MaxLength && record.Required)
                    errors += MsgUtils.Instance.Trls(parm.Culture, "IsExceedMax") + " " + record.MaxLength + ";";

                if (record.MinLength > 0 && (record.ColumnType == "string" || record.ColumnType == "text" || record.ColumnType == "email" || record.ColumnType == "url") && record.Value.Length < record.MinLength && record.Required)
                    errors += MsgUtils.Instance.Trls(parm.Culture, "LengthCantBeLessThan") + " " + record.MinLength + ";";

                if (!string.IsNullOrEmpty(record.Pattern) && !string.IsNullOrEmpty(record.Value))
                {
                    var reg = new System.Text.RegularExpressions.Regex(record.Pattern.Replace("\\\\", "\\"));
                    if (!reg.IsMatch(record.Value))
                        errors += MsgUtils.Instance.Trls(parm.Culture, "IsNotMatchPattern") + ";";
                }

                if (record.IsUnique)
                {
                    // create sql statement
                    if (string.IsNullOrEmpty(parm.TableName)) parm.TableName = parm.ObjectName;
                    StringBuilder sql = new StringBuilder("Select Count(0) From " + parm.TableName + " Where");
                    // used for update only
                    if (id == "")
                        errors += "Id must be first column in the model";

                    if (!(id == "0" && string.IsNullOrEmpty(id)))
                        sql.Append(" Id <> '" + id + "' And");

                    // for child rows
                    if (parm.ParentColumn != null)
                    {
                        if (parentId == "")
                            errors += parm.ParentColumn + " must be directly after Id in the model";

                        sql.Append(" " + parm.ParentColumn + " = '" + parentId + "' And");
                    }

                    // main unique column
                    if (record.ColumnName.ToUpper().Contains("DATE"))
                    {
                        DateTime d = DateTime.MinValue;
                        DateTime.TryParse(record.Value, out d);
                        if (d > DateTime.MinValue) sql.Append(" " + record.ColumnName + " = '" + d.ToString("yyyy/MM/dd") + "'");
                    }
                    else sql.Append(" " + record.ColumnName + " = '" + record.Value + "'");

                    if (record.UniqueColumns != null)
                    {
                        string[] filters = record.UniqueColumns.Split(',');

                        // basic filter columns
                        for (var i = 0; i < filters.Length; i++)
                        {
                            string filter = filters[i].Trim();
                            var row = parm.Columns.FirstOrDefault(col => col.Row == record.Row && col.Name == filter);
                            if (row != null)
                            {
                                sql.Append(" And");
                                sql.Append(" " + filter + " = '" + row.Value + "'");
                            }
                        }
                    }

                    if (parm.TableName == "Currencies") sql = sql.Replace("Id", "Code");
                    else if (parm.TableName == "Lookupcode") sql = sql.Replace(" Id", " CodeName");
                    else if (parm.TableName == "LookUpUserCodes") sql = sql.Replace(" Id", " CodeName");

                    if (errors.Length == 0 && GetIntResultFromSql(sql.ToString()) > 0)
                        errors += /*record.Title + (record.UniqueColumns == null? "": ", " + record.UniqueColumns) +*/ MsgUtils.Instance.Trls(parm.Culture, "AlreadyExists") + ";";
                }

                // Add previous error
                if (previousRow != record.Row && previousRow != -1 && errorsList.Count() > 0)
                {
                    var page = parm.Columns.FirstOrDefault(col => col.Row == previousRow && col.Name == "Page");
                    Errors.Add(new Error() { id = previousId, row = previousRow, errors = errorsList, page = (page == null ? (short)0 : short.Parse(page.Value)) });
                }

                // Start for each row
                if (previousRow != record.Row)
                {
                    previousRow = record.Row;
                    previousId = (id.Length > 0 ? 1 : int.Parse(id));
                    errorsList = new List<ErrorMessage>();
                }

                // for each cell
                if (errors.Length > 0) // found errors       
                    errorsList.Add(new ErrorMessage() { field = record.ColumnName, message = errors });
            }

            // add last row error
            if (errorsList != null && errorsList.Count() > 0)
            {
                var page = parm.Columns.FirstOrDefault(col => col.Row == previousRow && col.Name == "Page");
                Errors.Add(new Error() { id = previousId, row = previousRow, errors = errorsList, page = ( (page == null || string.IsNullOrEmpty(page.Value)) ? (short)0 : short.Parse(page.Value)) });
            }

            return Errors;
        }

        public List<Error> Check(CheckParm parm)
        {
            return CheckGrid(parm);
        }

        public List<string> GetAutoCompleteColumns(string objectName, int compnayId, byte version)
        {
            List<string> columns = ((Db.HrContext)Context).FormsColumns
                .Where(fc => fc.Section.FieldSet.PageId == HrContext.GetPageId(compnayId, objectName, version) && fc.InputType == "autocomplete")
                .Select(fc => fc.ColumnName).ToList();

            return columns;
        }

        public void AddTrail(AddTrailViewModel trailVM)
        {
            AudiTrail trail = new AudiTrail
            {
                ColumnName = trailVM.ColumnName,
                CompanyId = trailVM.CompanyId,
                ModifiedTime = DateTime.Now,
                ModifiedUser = trailVM.UserName,
                ObjectName = trailVM.ObjectName,
                SourceId = trailVM.SourceId,
                ValueAfter = trailVM.ValueAfter,
                Version = trailVM.Version,
                ValueBefore = trailVM.ValueBefore,
                Transtype = trailVM.Transtype
            };
            ((Db.HrContext)Context).AuditTrail.Add(trail);
        }

        public void RequestRangeFilter(byte range, int companyId, out DateTime? Start, out DateTime? End)
        {
            DateTime Today = DateTime.Today;
            Start = null;
            End =  Today;

            switch (range)
            {
                case 1:     //--1 today
                    Start = Today; End = Today; break;
                case 2:     //--2 yesterday
                    Start = Today.AddDays(-1); End = Today.AddDays(-1); break;
                case 3:     //--3 last 7 days
                    Start = Today.AddDays(-6); break;
                case 4:     //--4 Last 14 Days"
                    Start = Today.AddDays(-13);  break;
                case 5:     //--5 Last 30 Days"
                    Start = Today.AddDays(-30); break;
                case 6:     //--6 This Week
                case 7:     //--7 Last Week
                    PersonSetup personnel = GetPersonSetup(companyId);
                    byte weekend = (personnel?.Weekend2 ?? personnel?.Weekend1) ?? 0;
                    int diff = ((int)Today.DayOfWeek - weekend);// ((weekend == 6 ? 0 : weekend + 1)));
                    if (diff < 0) diff += 7;
                        Start = Today.AddDays(-1 * diff).Date;
                        End = Today.AddDays(6 - (diff)).Date;
                    if (range == 7)
                    {
                        Start = Start.Value.AddDays(-7);
                        End = End.Value.AddDays(-7);
                    }
                    break;
                case 8:     //--8 This Month
                    Start = new DateTime(Today.Year, Today.Month, 1);
                    End = new DateTime(Today.Year, Today.Month, Start.Value.AddMonths(1).AddDays(-1).Day);
                    break;
                case 9:     //--9 Last Month
                    DateTime fThisMonth = new DateTime(Today.Year, Today.Month, 1);
                    Start = fThisMonth.AddMonths(-1);
                    End = fThisMonth.AddDays(-1);
                    break;
                default:    //-- 10 All Time
                    Start = null;
                    End = null;
                    break;
            }
        }
    }
}
