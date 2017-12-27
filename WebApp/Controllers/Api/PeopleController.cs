using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;
using System.Web.Http.ModelBinding;
using System.Web;
using Interface.Core;
using Model.ViewModel.Personnel;
using Model.ViewModel;
using Model.Domain;
using System;
using System.Collections;
using Model.Domain.Payroll;
using Model.ViewModel.Administration;

namespace WebApp.Controllers.Api
{
    public class PeopleController : BaseApiController
    {
        private readonly IHrUnitOfWork _hrUnitOfWork;

        public PeopleController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }
        [ResponseType(typeof(TerminationFormViewModel)), HttpGet]
        [Route("api/People/GetEmp")]
        public IHttpActionResult GetEmp()
        {
            string culture = User.Identity.GetLanguage();
            int CompanyId = User.Identity.GetDefaultCompany();
            var empId = User.Identity.GetEmpId();

            empId = 1042;
            var GenderLst = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Gender", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var ReligonLst = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Religion", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var MaritalStatLst = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("MaritalStat", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var MilitaryStatLst = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("MilitaryStat", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var MedicalStatLst = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("MedicalStat", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();//
            var BloodClassLst = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("BloodClass", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var PersonTypeLst = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var RecommenResonLst = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("RecommenReson", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var CurrencyLst = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();

            //Assignment
            var assignment = _hrUnitOfWork.EmployeeRepository.GetAssignment(1042, culture);
            var oldAssignment = _hrUnitOfWork.EmployeeRepository.Find(a => a.EmpId == 1042).OrderByDescending(b => b.EndDate).FirstOrDefault();
            if (oldAssignment != null && assignment == null)
            {
                assignment.DepartmentId = oldAssignment.DepartmentId;
                assignment.LocationId = oldAssignment.LocationId;
                assignment.ManagerId = oldAssignment.ManagerId;
                assignment.NoticePrd = oldAssignment.NoticePrd;
                assignment.PayGradeId = oldAssignment.PayGradeId;
                assignment.PayrollId = oldAssignment.PayrollId;
                assignment.PositionId = oldAssignment.PositionId;
                assignment.ProbationPrd = oldAssignment.ProbationPrd;
                assignment.SalaryBasis = oldAssignment.SalaryBasis;
                assignment.JobId = oldAssignment.JobId;
            }
            int EmpCompany = _hrUnitOfWork.PeopleRepository.GetEmployment(assignment.EmpId).CompanyId;
            var LocationId = _hrUnitOfWork.LocationRepository.ReadLocations(culture, EmpCompany).Where(a => a.IsInternal).Select(a => new { id = a.Id, name = a.LocalName });
            var job = _hrUnitOfWork.JobRepository.ReadJobs(EmpCompany, culture,0).Select(a => new { id = a.Id, name = a.LocalName });
            var Dept = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(EmpCompany,null,culture);
            var Payroll = _hrUnitOfWork.Repository<Payrolls>().Select(a => new { id = a.Id, name = a.Name });
            var Position = _hrUnitOfWork.PositionRepository.GetPositions(culture, EmpCompany).Where(p => (p.HiringStatus == 2) || (p.Id == (assignment.PositionId != null ? assignment.PositionId.Value : 0))).Select(a => new { id = a.Id, name = a.Name, HeadCount = a.Headcount, ErrorMes = a.SysResponse }).ToList();
            var PeopleGroup = _hrUnitOfWork.PeopleRepository.GetPeoples().Select(a => new { id = a.Id, name = a.Name });
            var PayrollGrad = _hrUnitOfWork.JobRepository.GetPayrollGrade();
            var CareerPath = _hrUnitOfWork.JobRepository.ReadCareerPaths(EmpCompany).Select(a => new { id = a.Id, name = a.Name });
            var ManagerId = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(culture, 0, EmpCompany).Where(a => a.Id != assignment.EmpId).Distinct().Select(m => new { name = m.Employee, id = m.Id }).ToList();
            var AssignmentLst = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Assignment", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var PerformanceLst = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Performance", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();
            var SalaryBasisLst = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("SalaryBasis", culture).Select(b => new { id = b.CodeId, name = b.Title }).ToList();

            //ManagerId=ManagerId,CareerPath=CareerPath,PayrollGrad=PayrollGrad,PeopleGroup,PeopleGroup,Position=Position,Payroll=Payroll,Dept=Dept,job=job,LocationId=LocationId
            //Elig Cert

            int[] PrId = { 1, 2 };
            var Hosiptal = _hrUnitOfWork.Repository<Provider>().Where(a => PrId.Contains(a.ProviderType)).Select(a => new { id = a.Id, name = a.Name });
            var Qualification = _hrUnitOfWork.QualificationRepository.GetAll().Select(a => new { id = a.Id, name = a.Name }).ToList();
            var Kafeel = _hrUnitOfWork.LookUpRepository.GetAllKafeels().Select(a => new { id = a.Id, name = a.Name }).ToList();
            var Location = _hrUnitOfWork.Repository<World>().Select(c => new { id = c.CountryId, country = c.CountryId, city = c.CityId, dist = c.DistrictId, name = c.Name }).ToList();

            //PersonTypeObj
            var Person = _hrUnitOfWork.EmployeeRepository.GetPersonTypeAndEmployee(1042);

            //Basic Data
            var PersonObj = _hrUnitOfWork.PeopleRepository.ReadPerson(1042, User.Identity.GetLanguage());

            // employementObj
            var employement = _hrUnitOfWork.PeopleRepository.GetEmployment(1042);

            var Age = DateTime.Now.Year - PersonObj.BirthDate.Year;
            IEnumerable Nationality = null;
            if (culture.Substring(0, 2) == "ar")
            {
                Nationality = _hrUnitOfWork.Repository<Country>().Where(a => a.Nationality != null).Select(a => new { id = a.Id, name = a.NationalityAr }).ToList();
            }
            else
            {
                Nationality = _hrUnitOfWork.Repository<Country>().Where(a => a.Nationality != null).Select(a => new { id = a.Id, name = a.Nationality }).ToList();
            }

            IEnumerable birthLocation = null;
            if (PersonObj.BirthCountry > 0)
            {
                var city = PersonObj.BirthCity ?? 0;
                var dist = PersonObj.BirthDstrct ?? 0;
                var location = _hrUnitOfWork.Repository<World>().FirstOrDefault(c => c.CountryId == PersonObj.BirthCountry
                && c.CityId == city && c.DistrictId == dist);
                if (location != null)
                    birthLocation = (culture.Substring(0, 2) == "ar" ? location.NameAr : location.Name);
            }

            return Ok(new
            {
                PersonObj = PersonObj,
                employementObj = employement,
                GenderLst = GenderLst,
                ReligonLst = ReligonLst,
                MaritalStatLst = MaritalStatLst,
                MilitaryStatLst = MilitaryStatLst,
                MedicalStatLst = MedicalStatLst,
                BloodClassLst = BloodClassLst,
                PersonTypeLst = PersonTypeLst,
                RecommenResonLst = RecommenResonLst,
                Qualification = Qualification,
                Kafeel = Kafeel,
                Location = Location,
                Hosiptal = Hosiptal,
                NationalityLst = Nationality,
                Person = Person,
                Age = Age,
                birthLocation = birthLocation,
                CurrencyLst = CurrencyLst,
                assignment = assignment,
                AssignmentLst = AssignmentLst,
                SalaryBasisLst = SalaryBasisLst,
                PerformanceLst = PerformanceLst,
                ManagerId = ManagerId,
                CareerPath = CareerPath,
                PayrollGrad = PayrollGrad,
                PeopleGroup = PeopleGroup,
                Position = Position,
                Payroll = Payroll,
                Dept = Dept,
                job = job,
                LocationId = LocationId
            });
        }

        [ResponseType(typeof(EmployeeBenefitViewModel)), HttpGet]
        [Route("api/People/GetEmpBenefit")]
        public IHttpActionResult GetEmpBenefit()
        {
            var id = User.Identity.GetEmpId();
            var BeneficiaryLst = _hrUnitOfWork.Repository<EmpRelative>().Where(a => a.EmpId == 1042).Select(p => new { value = p.Id, text = p.Name }).ToList();
          //  var BenefitplanId = _hrUnitOfWork.Repository<BenefitPlan>().Select(p => new { value = p.Id, text = p.PlanName }).ToList();
            var benefitLst = _hrUnitOfWork.EmployeeRepository.GetEmpBenefit(1042, User.Identity.GetLanguage(), User.Identity.GetDefaultCompany()).Select(a => new { value = a.Id, text = a.Name });
            var query = _hrUnitOfWork.EmployeeRepository.GetEmpBenefits(1042).ToList(); ;
            foreach (var item in query)
            {
                if (item.BeneficiaryId == null)
                    item.BeneficiaryName = "Employee";
                else
                    item.BeneficiaryName = BeneficiaryLst.Where(a => a.value == item.BeneficiaryId).Select(a => a.text).FirstOrDefault();
                item.BenefitName = benefitLst.Where(a => a.value == item.BenefitId).Select(a => a.text).FirstOrDefault();
            }
            return Ok(query);
        }
        [ResponseType(typeof(EmpRelativeViewModel)), HttpGet]
        [Route("api/People/GetEmpRelative")]
        public IHttpActionResult GetEmpRelative()
        {
            var Relations = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Relations", User.Identity.GetLanguage()).Select(a => new { value = a.CodeId, text = a.Title });

            var query = _hrUnitOfWork.EmployeeRepository.GetEmpRelative(1042).ToList();
            foreach (var item in query)
            {
                item.RelationName = Relations.Where(a => a.value == item.Relation).Select(a => a.text).FirstOrDefault();
            }

            return Ok(query);
        }
        [ResponseType(typeof(PeopleTrainGridViewModel)), HttpGet]
        [Route("api/People/GetEmpTraining")]
        public IHttpActionResult GetEmpTraining()
        {
            var CourseId = _hrUnitOfWork.TrainingRepository.GetTrainCourse(User.Identity.GetLanguage(), User.Identity.GetDefaultCompany()).Select(p => new { value = p.Id, text = p.LocalName });

            var query = _hrUnitOfWork.PeopleRepository.ReadEmployeeTraining(1042).ToList();
            foreach (var item in query)
            {
                item.CourseName = CourseId.Where(a => a.value == item.CourseId).Select(a => a.text).FirstOrDefault();
            }
            return Ok(query);
        }
        [ResponseType(typeof(EmpQualificationViewModel)), HttpGet]
        [Route("api/People/GetEmpQualification")]
        public IHttpActionResult GetEmpQualification()
        {
            string culture = User.Identity.GetLanguage();
            var Qual = _hrUnitOfWork.PeopleRepository.getQualification("QualCat");
            var School = _hrUnitOfWork.Repository<School>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            var Grade = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Grade", culture).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            List<FormList> Status = new List<FormList>();
            Status.Add(new FormList { id = 1, name = MsgUtils.Instance.Trls("Ingoing") });
            Status.Add(new FormList { id = 2, name = MsgUtils.Instance.Trls("Completed") });
            bool flag = true;
            var query = _hrUnitOfWork.PeopleRepository.ReadQualifications(1042, flag).ToList();
            foreach (var item in query)
            {
                item.SchoolName = School.Where(a => a.value == item.SchoolId).Select(a => a.text).FirstOrDefault();
                item.QualName = Qual.Where(a => a.value == item.QualId).Select(a => a.text).FirstOrDefault();
                item.GradName = Grade.Where(a => a.value == item.Grade).Select(a => a.text).FirstOrDefault();
                item.StutesName = Status.Where(a => a.id == item.Status).Select(a => a.name).FirstOrDefault();
            }
            return Ok(query);
        }//GetEmpCertification
        [ResponseType(typeof(EmpQualificationViewModel)), HttpGet]
        [Route("api/People/GetEmpCertification")]
        public IHttpActionResult GetEmpCertification()
        {
            string culture = User.Identity.GetLanguage();
            var Qual =_hrUnitOfWork.PeopleRepository.getCertification("QualCat"); ;
            var School = _hrUnitOfWork.Repository<School>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            var Grade = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Grade", culture).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            var Awarding = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Awarding", culture).Select(a => new { value = a.CodeId, text = a.Title }).ToList(); ;

            List<GridListViewModel> Status = new List<GridListViewModel>();
            Status.Add(new GridListViewModel { value = 1, text = MsgUtils.Instance.Trls("Ingoing") });
            Status.Add(new GridListViewModel { value = 2, text = MsgUtils.Instance.Trls("Completed") });
           
            bool flag = false;
            var query = _hrUnitOfWork.PeopleRepository.ReadQualifications(1042, flag).ToList();
            foreach (var item in query)
            {
                item.SchoolName = School.Where(a => a.value == item.SchoolId).Select(a => a.text).FirstOrDefault();
                item.QualName = Qual.Where(a => a.value == item.QualId).Select(a => a.text).FirstOrDefault();
                item.GradName = Grade.Where(a => a.value == item.Grade).Select(a => a.text).FirstOrDefault();
                item.StutesName = Status.Where(a => a.value == item.Status).Select(a => a.text).FirstOrDefault();
                item.AwardingName = Awarding.Where(a => a.value == item.Awarding).Select(a => a.text).FirstOrDefault();
            }
            return Ok(query);
        }
    }
}
