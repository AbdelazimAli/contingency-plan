using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
    public interface IBenefitsRepository : IRepository<Benefit>

    {
        IQueryable<BenefitViewModel> GetBenefits(string culture, int CompanyId);
        BenefitFormViewModel ReadBenefit(int Id, string culture);
        IEnumerable<BenefitPlanViewModel> ReadBenefitPlan(int BenefitId);
        void Add(BenefitPlan benefitPlan);
        void Attach(BenefitPlan benefitPlan);
        DbEntityEntry<BenefitPlan> Entry(BenefitPlan benefitPlan);
        void Remove(BenefitPlan benefitPlan);
        IQueryable<FunctionViewModel> GetBenefitServs();
        void BenefitServs(IEnumerable<string> BenfitServName, int planId);
        IQueryable<BenfitServiceViewModel> GetBenefitServ(bool IsGroup, int? ParentId);
        void Add(BenefitServ benefitServ);
        void Remove(BenefitServ benefitServ);
        void Attach(BenefitServ benefitServ);
        DbEntityEntry<BenefitServ> Entry(BenefitServ benefitServ);
        void AddRange(List<BenefitServPlans> benefitServPlans);
    }
}


