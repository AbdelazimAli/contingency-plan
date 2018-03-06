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
    class BenefitsRepository :Repository<Benefit>,IBenefitsRepository
    {
        public BenefitsRepository(DbContext context) : base(context)
        {

        }

        private HrContext context
        {
            get
            {
                return Context as HrContext;
            }
        }

        #region Benefit
        public IQueryable<BenefitViewModel> GetBenefits(string culture,int CompanyId)
        {
            var result = from B in context.Benefits
                         where (B.CompanyId == CompanyId || B.IsLocal == false) && (B.StartDate <= DateTime.Today && (B.EndDate == null || B.EndDate >= DateTime.Today))
                         select new BenefitViewModel
                         {
                             Id = B.Id,
                             Name = B.Name,
                             Code = B.Code,
                             LocalName = HrContext.TrlsName(B.Name, culture),
                             StartDate = B.StartDate,
                             EndDate = B.EndDate,
                         };
            return result;
        }

        public BenefitFormViewModel ReadBenefit(int Id, string culture)
        {
            var obj = context.Benefits.Where(a => a.Id == Id).Select(a => new { a, LocalName = HrContext.TrlsName(a.Name, culture) }).FirstOrDefault();
            var mod = new BenefitFormViewModel()
            {
                Id = obj.a.Id,
                Name = obj.a.Name,
                StartDate = obj.a.StartDate,
                IsLocal = obj.a.IsLocal,
                Code = obj.a.Code,
                EndDate = obj.a.EndDate,
                Coverage= obj.a.Coverage,
                Description= obj.a.Description,
                EmpAccural= obj.a.EmpAccural,
                MaxFamilyCnt= obj.a.MaxFamilyCnt,
                MonthFees= obj.a.MonthFees,
                WaitMonth= obj.a.WaitMonth,
                LocalName = obj.LocalName,
                CreatedTime = obj.a.CreatedTime,
                CreatedUser = obj.a.CreatedUser,
                ModifiedTime = obj.a.ModifiedTime,
                ModifiedUser = obj.a.ModifiedUser,
                CalenderId=obj.a.CalenderId,
                BenefitClass=obj.a.BenefitClass,
                IPayrolls = obj.a.Payrolls == null ? null : obj.a.Payrolls.Split(',').Select(int.Parse).ToList(),
                ICompanyStuctures = obj.a.CompanyStuctures == null ? null : obj.a.CompanyStuctures.Split(',').Select(int.Parse).ToList(),
                IEmployments = obj.a.Employments == null ? null : obj.a.Employments.Split(',').Select(int.Parse).ToList(),
                IPayrollGrades = obj.a.PayrollGrades == null ? null : obj.a.PayrollGrades.Split(',').Select(int.Parse).ToList(),
                IPositions = obj.a.Positions == null ? null : obj.a.Positions.Split(',').Select(int.Parse).ToList(),
                IBranches = obj.a.Branches == null ? null : obj.a.Branches.Split(',').Select(int.Parse).ToList(),
                IJobs = obj.a.Jobs == null ? null : obj.a.Jobs.Split(',').Select(int.Parse).ToList(),
                IPeopleGroups = obj.a.PeopleGroups == null ? null : obj.a.PeopleGroups.Split(',').Select(int.Parse).ToList(),
                PlanLimit = obj.a.PlanLimit

            };
            return mod;
        }
        #endregion

        #region BenefitPlan
        public IEnumerable<BenefitPlanViewModel> ReadBenefitPlan(int BenefitId)
        {
            var result = (from Bp in context.BenefitPlans
                         where Bp.BenefitId == BenefitId
                         select new BenefitPlanViewModel
                         {
                             Id = Bp.Id,
                             BenefitId = Bp.BenefitId,
                             PlanName = Bp.PlanName,
                             CompPercent = Bp.CompPercent,
                             EmpAmount = Bp.EmpAmount,
                             EmpPercent = Bp.EmpPercent,
                             CoverAmount = Bp.CoverAmount,
                             CompAmount = Bp.CompAmount,
                             CreatedTime = Bp.CreatedTime,
                             CreatedUser = Bp.CreatedUser,
                             ModifiedTime = Bp.ModifiedTime,
                             ModifiedUser = Bp.ModifiedUser,
                             BenefitServs = context.BenefitServPlans.Where(a => a.BenefitPlanId == Bp.Id).Select(a => a.BenefitServ.Name).ToList()
                            
                         }).ToList();
            return result;
        }

        public void AddRange(List<BenefitServPlans> benefitServPlans)
        {
            context.BenefitServPlans.AddRange(benefitServPlans);        
        }

        public void Add(BenefitPlan benefitPlan)
        {
            context.BenefitPlans.Add(benefitPlan);
        }
        public void Attach(BenefitPlan benefitPlan)
        {
            context.BenefitPlans.Attach(benefitPlan);
        }
        public DbEntityEntry<BenefitPlan> Entry(BenefitPlan benefitPlan)
        {
            return Context.Entry(benefitPlan);
        }
        public void Remove(BenefitPlan benefitPlan)
        {
            if (Context.Entry(benefitPlan).State == EntityState.Detached)
            {
                context.BenefitPlans.Attach(benefitPlan);
            }
            context.BenefitPlans.Remove(benefitPlan);
        }
        public IQueryable<FunctionViewModel> GetBenefitServs()
        {
            var Functions = from f in context.BenefitServs
                            where f.IsGroup == true
                            select new FunctionViewModel
                            {
                                Id = f.Id,
                                Name = f.Name
                            };

            return Functions;
        }

        public void BenefitServs(IEnumerable<string> BenfitServName, int planId)
        {
            var ListBenPlan = context.BenefitServPlans.Where(a => a.BenefitPlanId == planId).ToList();
            var list = context.BenefitServPlans.ToList();
            if (BenfitServName != null)
            {
                var servObj = context.BenefitServs.Where(a => BenfitServName.Contains(a.Name)).ToList();
                foreach (var item in servObj)
                {
                    if (list.FirstOrDefault(a => a.BenefitServId == item.Id && a.BenefitPlanId == planId) == null)
                    {
                        var benfSerPlan = new BenefitServPlans();
                        benfSerPlan.BenefitServId = item.Id;
                        benfSerPlan.BenefitPlanId = planId;
                        context.BenefitServPlans.Add(benfSerPlan);
                    }
                }
                foreach (var item in ListBenPlan)
                {
                    if (!servObj.Contains(item.BenefitServ))
                        context.BenefitServPlans.Remove(item);
                }
            }
            else if (BenfitServName == null && ListBenPlan.Count() > 0)
                foreach (var item in ListBenPlan)
                {
                    context.BenefitServPlans.Remove(item);
                }

        }


        #endregion

        #region BenefitServ
        public IQueryable<BenfitServiceViewModel> GetBenefitServ(bool IsGroup , int? ParentId)
        {
            var query = from Bs in context.BenefitServs
                        where Bs.IsGroup == IsGroup && Bs.ParentId==ParentId
                        select new BenfitServiceViewModel
                        {
                            Id = Bs.Id,
                            Name = Bs.Name,
                            Code = Bs.Code,
                            IsGroup =Bs.IsGroup,
                            CompPercent=Bs.CompPercent,
                            Cost=Bs.Cost,
                            Curr=Bs.Curr,
                            EmpPercent=Bs.EmpPercent,
                            EndDate=Bs.EndDate,
                            ParentId=Bs.ParentId,
                            StartDate=Bs.StartDate,
                            BenefitId =Bs.BenefitId
                        };
            return query;
        }

        public void Add(BenefitServ benefitServ)
        {
            context.BenefitServs.Add(benefitServ);
        }
        public void Attach(BenefitServ benefitServ)
        {
            context.BenefitServs.Attach(benefitServ);
        }
        public DbEntityEntry<BenefitServ> Entry(BenefitServ benefitServ)
        {
            return Context.Entry(benefitServ);
        }
        public void Remove(BenefitServ benefitServ)
        {
            if (Context.Entry(benefitServ).State == EntityState.Detached)
            {
                context.BenefitServs.Attach(benefitServ);
            }
            context.BenefitServs.Remove(benefitServ);
        }

        #endregion
    }
}

