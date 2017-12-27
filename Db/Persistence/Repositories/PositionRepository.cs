using Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Interface.Core.Repositories;
using Model.ViewModel.Personnel;
using System.Data.Entity.Infrastructure;

namespace Db.Persistence.Repositories
{
    class PositionRepository : Repository<Position>, IPositionRepository
    {
        public PositionRepository(DbContext context) : base(context)
        {
        }
        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        public IQueryable<PositionViewModel> GetPositions(string Culture,int CompanyId)
        {
            var Postions = from P in context.Positions
                           where (P.StartDate <= DateTime.Today && (P.EndDate == null || P.EndDate >= DateTime.Today )) && P.CompanyId== CompanyId
                           select new PositionViewModel
                           {
                                Code=P.Code,
                                Name=HrContext.TrlsName(P.Name,Culture),
                                DeptName=P.Department.Name,
                                JobName=P.Job.Name,
                                Id=P.Id,
                                JobId=P.JobId,
                                Supervisor = P.Supervisor,
                                HiringStatus =P.HiringStatus,
                                Seasonal=P.Seasonal,
                                DeptId = P.DeptId,
                                Headcount = P.Headcount,
                                SysResponse=P.SysResponse
                           };
            return Postions;
        }
        public PositionViewModel ReadPosition(int id , string culure)
        {
            var Position = (from P in context.Positions
                           where P.Id == id
                            select new PositionViewModel
                           {
                               Code = P.Code,
                               Name = P.Name,
                               LName=HrContext.TrlsName(P.Name,culure),
                               DeptId = P.DeptId,
                               JobId = P.JobId,
                               Id = P.Id,
                               Headcount=P.Headcount,
                               SysResponse=P.SysResponse,
                               //PositionType = P.PositionType,
                               HiringStatus = P.HiringStatus,
                               Seasonal = P.Seasonal,
                               Supervisor=P.Supervisor,
                               ModifiedTime=P.ModifiedTime,
                               ModifiedUser=P.ModifiedUser,
                               OverlapPeriod=P.OverlapPeriod,
                               PayrollId=P.PayrollId,
                               ProbationPeriod=P.ProbationPeriod,
                               CreatedTime=P.CreatedTime,
                               CreatedUser=P.CreatedUser,
                               EndDate=P.EndDate,
                                Relief=P.Relief,
                                SalaryBasis=P.SalaryBasis,
                                SeasonDay=P.SeasonDay,
                                SeasonMonth=P.SeasonMonth,
                                StartDate=P.StartDate,
                                Successor=P.Successor
                           }).FirstOrDefault();
            return Position;
        }

        public IQueryable<SubperiodViewModel> GetPeriods(int BudgetId , int PositionId)
        {
            var periods = (from b in context.Budgets
                            where b.Id == BudgetId
                            join c in context.SubPeriods on b.PeriodId equals c.PeriodId
                            join pb in context.PositionBudgets on c.Id equals pb.SubPeriodId into g
                           from pb in g.Where(a=>a.PositionId == PositionId).DefaultIfEmpty()
                            
                            select new SubperiodViewModel
                            {
                               Id = pb == null ? 0 : pb.Id,
                               SubPeriodId = c.Id,
                               Name= c.Name,
                               Amount = pb.BudgetAmount
                            }); 
                          
                return periods;
        }
        public void Add(PosBudget Posbudget)
        {
            context.PositionBudgets.Add(Posbudget);
        }

        public void Attach(PosBudget Posbudget)
        {
            context.PositionBudgets.Attach(Posbudget);
        }
        public DbEntityEntry<PosBudget> Entry(PosBudget PositionBud)
        {
            return Context.Entry(PositionBud);
        }
        public void Remove(PosBudget PosBd)
        {
            if (Context.Entry(PosBd).State == EntityState.Detached)
            {
                context.PositionBudgets.Attach(PosBd);
            }
            context.PositionBudgets.Remove(PosBd);
        }
        public void Attach(DiagramNode  Node)
        {
            context.DiagramNodes.Attach(Node);
        }
        public DbEntityEntry<DiagramNode> Entry(DiagramNode Node)
        {
            return Context.Entry( Node);
        }
        public void Remove(DiagramNode Node)
        {
            if (Context.Entry(Node).State == EntityState.Detached)
            {
                context.DiagramNodes.Attach(Node);
            }
            context.DiagramNodes.Remove(Node);
        }
        public void Add(DiagramNode di)
        {
            context.DiagramNodes.Add(di);
        }
        public void Attach(Diagram Node)
        {
            context.Diagrams.Attach(Node);
        }
        public DbEntityEntry<Diagram> Entry(Diagram Node)
        {
            return Context.Entry(Node);
        }
        public void Remove(Diagram Node)
        {
            if (Context.Entry(Node).State == EntityState.Detached)
            {
                context.Diagrams.Attach(Node);
            }
            context.Diagrams.Remove(Node);
        }
        public void Add(Diagram di)
        {
            context.Diagrams.Add(di);
        }

       
        public IList<PositionDiagram> GetDiagramNodes(string Culture, int value)
        {
            var res = (from dn in context.DiagramNodes
                      join d in context.Diagrams on dn.DiagramId equals d.Id 
                      where d.Id==value
                      join p in context.Positions on dn.ChildId equals p.Id
                      select new PositionDiagram
                      {
                          Id = dn.ChildId,
                          Name= HrContext.TrlsName(p.Name, Culture),
                          colorSchema = d.Color,
                          NoofHolder = context.Assignments.Where(a => a.PositionId == dn.ChildId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Count(),
                          HeadCount = p.Headcount ,
                          ParentId = dn.ParentId,
                          Relief=p.Relief,
                          Employee = context.Assignments.Where(a => a.PositionId == dn.ChildId && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Select(b => HrContext.TrlsName(b.Employee.Title + " " + b.Employee.FirstName + " " + b.Employee.Familyname, Culture)).Take(3).ToList()
                      }).ToList();


            return res;
        }
        public IList<PositionDiagram> GetDiagram(string Culture, int CompanyId)
        {
            var query = (from p in context.Positions
                         where (p.StartDate <= DateTime.Today && (p.EndDate == null || p.EndDate >= DateTime.Today)) && p.CompanyId == CompanyId
                        
                         select new PositionDiagram
                         {
                             Id = p.Id,
                             Name = HrContext.TrlsName(p.Name, Culture),
                             HeadCount = p.Headcount,
                             ParentId = p.Supervisor,
                             Employee = context.Assignments.Where(a => a.PositionId == p.Id && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Select(b => HrContext.TrlsName(b.Employee.Title + " " + b.Employee.FirstName + " " + b.Employee.Familyname, Culture)).ToList(),
                             NoofHolder = context.Assignments.Where(a => a.PositionId == p.Id && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Count(),
                             Relief=p.Relief,
                             colorSchema = (p.HiringStatus == 1 ? "#00ffff" : (p.HiringStatus == 2 ? "#008000" : (p.HiringStatus == 3 ? "#f5f5f5" : "#808080")))
                         });

            return query.ToList();
        }
        public PositionDiagram GetRelief(int id, string culture)
        {
            var query = (from p in context.Positions
                         where p.Id == id
                         select new PositionDiagram
                         {
                             Id = p.Id,
                             ParentId = null,
                             Name = HrContext.TrlsName(p.Name, culture),
                             colorSchema = "#ff1587",
                             Employee = context.Assignments.Where(a => a.PositionId == p.Id && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Select(b => HrContext.TrlsName(b.Employee.Title + " " + b.Employee.FirstName + " " + b.Employee.Familyname, culture)).Take(3).ToList(),
                             HeadCount = p.Headcount,
                             NoofHolder = context.Assignments.Where(a => a.PositionId == p.Id && a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today).Count(),
                             Relief = null
                         }).FirstOrDefault();
            return query;
        }



    }
}
