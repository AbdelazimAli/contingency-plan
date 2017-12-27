using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Core.Repositories
{
    public interface IMedicalRepository :IRepository<BenefitRequest>
    {
        IQueryable<MedicalIndexViewModel> GetMedicalRequest(int companyId, string culture);
        MedicalRequestViewModel ReadMedical(int id);
        IEnumerable<FormList> FillDDLBenefit(string Culture);
        IEnumerable<FormList> GetBeneficiary(int id);
        IEnumerable<FormList> GetAllservice(int EmpId, int BenefitId, int? BeneficiaryId);
        IEnumerable<FormList> GetAllProvider();
        IEnumerable<FormList> GetAllBeneficiary();
        SetCurrencyInfoViewModel SetCurrency(int serviceId);
        IQueryable<MedicalIndexViewModel> GetMedicalReqFollowUp(int companyId, string culture);
        BenefitRequestFollowUp GetRequest(int requestId, string culture);
        IQueryable<MedicalIndexViewModel> GetApprovedMedicalReq(int companyId, string culture);
        IEnumerable<EmpServiceViewModel> ReadMedicalService(int EmpId);
        IEnumerable<BenfitServiceReqViewModel> ReadBenfitService(int PeriodId, int BenfitId, int EmpId);
        int GetBenefitPlanId(int id, int empId, int? beneficiaryId);
        List<int> GetPeriodId(int id, int empId, int? beneficiaryId);
        int GetMedicalPeriodId(int serviceId, DateTime issusDate);
    }
}
