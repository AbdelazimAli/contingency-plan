using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Model.Domain.Payroll;

namespace Interface.Core.Repositories
{
    public interface ILookUpRepository : IRepository<LookUpCode>
    {
        void AddLookUpCode(LookUpCode Look);
        IEnumerable GetCurrencyCodeGrid();
        IEnumerable<LookUpViewModel> GetLookUp(string culture);
        string CheckLookUpCode(string code, short id, string culture);

        IList<SysCodeViewModel> GetsyscodeForm(string CodeName, string culture);
         IQueryable<KafeelViewModel> GetAllKafeels();
        DocType DocTypeObject(int id);

        DocType GetDocType(int? id);
        void Add(Kafeel Kafeel);
        IEnumerable GetCode();
        DbEntityEntry<Kafeel> Entry(Kafeel Kafeel);
        void Attach(Kafeel Kafeel);
        void Remove(Kafeel kafeel);
        IQueryable<HospitalViewModel> GetAllHospitals(int ProvidType);
        void Add(Provider Provider);
        DbEntityEntry<Provider> Entry(Provider Provider);
        void Attach(Provider Provider);
        void Remove(Provider Provider);
        IEnumerable BudgetPeriod();
        IQueryable<CountryViewModel> GetCountry();
        IQueryable<CityViewModel> GetCity(int Id);
        IQueryable<DistrictViewModel> GetDistrict(int Id);
        IQueryable<CurrencyViewModel> GetCurrency();
        IQueryable<JobClassViewModel> GetJobClasses(int CompanyId);
        IQueryable GetJobs(int companyId);
        DocTypeFormViewModel ReadDocType(int Id, string culture);
        void Add(Country country);
        void Add(City city);
        void Add(PersonSetup Personnel);
        void Add(JobClass job);
        void Add(DocType doctype);
        void Add(DocTypeAttr docattr);
        void Add(CompanyDocAttr companyDocAttr);
        void Attach(PersonSetup Personnel);
        void Attach(DocType doctype);
        void Attach(JobClass job);
        void Attach(Country country);
        void Attach(City city);
        void Attach(DocTypeAttr docattr);
        void Attach(CompanyDocAttr companyDocAttr);
        void Remove(Country country);
       // void Remove(LookUpCode codes);
        void Remove(City city);
        void Remove(DocType doctype);
        void Remove(JobClass job);
        void Remove(DocTypeAttr docattr);
        DbEntityEntry<Country> Entry(Country country);
        DbEntityEntry<PersonSetup> Entry(PersonSetup personnel);
        DbEntityEntry<LookUpUserCode> Entry(LookUpUserCode lookupusercode);
        DbEntityEntry<DocType> Entry(DocType doctype);
        DbEntityEntry<City> Entry(City city);
        DbEntityEntry<CompanyDocAttr> Entry(CompanyDocAttr companyDocAttr);
        void Add(District district);
        void Attach(District district);
        void Remove(District district);
        DbEntityEntry<District> Entry(District district);
        DbEntityEntry<JobClass> Entry(JobClass job);
        DbEntityEntry<DocTypeAttr> Entry(DocTypeAttr docattr);
        void Add(Currency currency);
       void Attach(Currency currency);
        void Remove(Currency currency);
        DbEntityEntry<Currency> Entry(Currency currency);
        IQueryable<LookupCodesViewModel> GetLookUpCodes(string Id, string culture);
        string DeleteLookUpCode(LookUpViewModel model, string culture);
        void UpdateLookUpCode(IEnumerable<LookUpViewModel> models);
      //  void UpdateLookUpCode(IEnumerable<LookupCodesViewModel> models,string culture);
        void UpdateLookUpUserCode(IEnumerable<LookupUserCodeViewModel> models, string culture, string name);
        IEnumerable GetCurrencyCode();
        void Add(LookUpTitles lookuptitles);
        void Add(LookUpUserCode lookupusercode);

        void Attach(LookUpTitles lookuptitles);
        void Attach(LookUpUserCode lookupusercode);

        void Remove(LookUpTitles lookuptitles);
        void Remove(LookUpUserCode UserCode);

        DbEntityEntry<LookUpTitles> Entry(LookUpTitles lookuptitles);
        void RemoveRange(IEnumerable<LookUpTitles> entities);
        void RemoveRange(IEnumerable<LookUpUserCode> entities);
        // IQueryable<LookUpViewModel> GetLookUpUserCode();
        IEnumerable<LookUpViewModel> GetLookUpUserCode(string culture);
        IQueryable<LookupUserCodeViewModel> GetLookUpUserCodes(string Id, string culture);
        IEnumerable GetsyscodeId(string CodeName);
        IEnumerable GetCodeName();
        IQueryable<DocTypeViewModel> GetDocTypes(int companyId, string culture);
        IEnumerable<DocTypeAttrViewModel> GetDocAttr(int Id,string culture);
        void UpdateLookUpCodes(IEnumerable<LookupCodesViewModel> models, string culture, string name);
        void DeleteLookUpCodes(IEnumerable<LookupCodesViewModel> models);
        IEnumerable<LookupCodesViewModel> GetFlexLookUpCodesLists(int companyId, string objectName, byte version, string culture);
    }

}
