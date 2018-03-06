using Db.Persistence;
using Interface.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PeopleController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        public PeopleController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        public ActionResult GetMissAttach(int Id, int Gender, int? Nationality)
        {
            var result = _hrUnitOfWork.PeopleRepository.GetMissingAttachments(CompanyId, Id, Language, Gender, Nationality);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateProgress(int Id, bool IsDoc)
        {
            string value = IsDoc ? _hrUnitOfWork.PeopleRepository.GetAttachmentsCount(Id) : _hrUnitOfWork.PeopleRepository.GetProfileCount(Id, CompanyId, Convert.ToByte(Request.QueryString["Version"])).ToString();
            return Json(new { value }, JsonRequestBehavior.AllowGet);
        }

        #region PeopleGroup by Shaddad      
        public ActionResult PeopleIndex()
        {
            return View();
        }
        public ActionResult ReadPeopleGroups()
        {
            return Json(_hrUnitOfWork.PeopleRepository.GetPeoples(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreatePeopleGroups(IEnumerable<PeopleGroupViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var result = new List<PeopleGroup>();

            var datasource = new DataSource<PeopleGroupViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PeopleGroups",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var sequence = _hrUnitOfWork.Repository<PeopleGroup>().Select(a => a.Code).DefaultIfEmpty(0).Max();
                var MaxCode = sequence == 0 ? 1 : sequence + 1;

                for (var i = 0; i < models.Count(); i++)
                {
                    var page = new PeopleGroup();
                    AutoMapper(new AutoMapperParm()
                    {
                        ObjectName = "PeopleGroups",
                        Destination = page,
                        Source = models.ElementAtOrDefault(i),
                        Options = options.ElementAtOrDefault(i),
                        Transtype = TransType.Insert
                    });

                    page.CreatedTime = DateTime.Now;
                    page.CreatedUser = UserName;
                    page.Code = MaxCode++;
                    result.Add(page);
                    _hrUnitOfWork.PeopleRepository.Add(page);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on p.Name equals r.Name
                               select new PeopleGroupViewModel
                               {
                                   Id = r.Id,
                                   Code = r.Code,
                                   Name = p.Name
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdatePeopleGroups(IEnumerable<PeopleGroupViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<PeopleGroupViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PeopleGroups",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Id);
                var db_PeopleGroup = _hrUnitOfWork.Repository<PeopleGroup>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {

                    var rec = db_PeopleGroup.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "PeopleGroups", Destination = rec, Source = models.ElementAtOrDefault(i), Options = options.ElementAtOrDefault(i), Transtype = TransType.Update });
                    rec.ModifiedTime = DateTime.Now;
                    rec.ModifiedUser = UserName;
                    _hrUnitOfWork.PeopleRepository.Attach(rec);
                    _hrUnitOfWork.PeopleRepository.Entry(rec).State = EntityState.Modified;
                }
                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);

        }
        public ActionResult DeletePeopleGroups(int Id)
        {
            var datasource = new DataSource<PeopleGroupViewModel>();
            var Obj = _hrUnitOfWork.Repository<PeopleGroup>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "PeopleGroups",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.PeopleRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }

        #endregion

        #region Person by Mamdouh
        [OutputCache(VaryByParam = "*", Duration = 60)]
        public ActionResult Index()
        {
            ViewBag.QualificationId = _hrUnitOfWork.Repository<Qualification>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            ViewBag.PersonType = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", Language).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            return PartialView();
        }

        public ActionResult GetPeople(int MenuId, int tab, int pageSize, int skip)
        {
            if (tab == 2)
                return ApplyFilter<PeopleGridViewModel>(_hrUnitOfWork.EmployeeRepository.GetWaitingEmployee(CompanyId, Language), a => a.Code, MenuId, pageSize, skip);
            else if (tab == 3)
                return ApplyFilter<PeopleGridViewModel>(_hrUnitOfWork.EmployeeRepository.GetTerminatedEmployee(CompanyId, Language), a => a.Code, MenuId, pageSize, skip);
            else
                return ApplyFilter<PeopleGridViewModel>(_hrUnitOfWork.EmployeeRepository.GetCurrentEmployee(CompanyId, Language), a => a.Code, MenuId, pageSize, skip);

        }

        public ActionResult Details(int id = 0)
        {
            if (id == 0)
                return View(new PeoplesViewModel());

            var person = _hrUnitOfWork.PeopleRepository.ReadPerson(id, Language);
            person.Age = DateTime.Now.Year - person.BirthDate.Year;
            person.ExpYear = DateTime.Now.Year - (person.StartExpDate != null ? person.StartExpDate.Value.Year : person.JoinDate != null ? person.JoinDate.Value.Year : DateTime.Today.Year);
            // int attachments;
            person.Docs = _hrUnitOfWork.PeopleRepository.GetAttachmentsCount(id);
            person.profileProgress = _hrUnitOfWork.PeopleRepository.GetProfileCount(id, CompanyId, Convert.ToByte(Request.QueryString["Version"]));

            return person == null ? (ActionResult)HttpNotFound() : PartialView(person);
        }

        [HttpGet]
        public JsonResult GetAttachmentsCount(int EmpID)
        {
            try
            {
                string Docs = _hrUnitOfWork.PeopleRepository.GetAttachmentsCount(EmpID);
                return Json(new {Result=true, Docs= Docs },JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new {Result=false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult FillDropdownlists(int id, int? BirthCountry, int? CountryId, int? BirthCity, int? BirthDstrct, int? CityId, int? DistrictId)
        {
            try
            {
                var QualificationId = _hrUnitOfWork.QualificationRepository.GetAll().Select(a => new { id = a.Id, name = a.Name }).ToList();

                int[] PrId = { 1, 2 };
                var ProviderId = _hrUnitOfWork.Repository<Provider>().Where(a => PrId.Contains(a.ProviderType)).Select(a => new { id = a.Id, name = a.Name }).ToList();

                var Nationality = Language.Substring(0, 2) == "ar" ? _hrUnitOfWork.Repository<Country>().Where(a => a.Nationality != null).Select(a => new { id = a.Id, name = a.NationalityAr }).ToList() :
                _hrUnitOfWork.Repository<Country>().Where(a => a.Nationality != null).Select(a => new { id = a.Id, name = a.Nationality }).ToList();

                // set birth location
                string BirthLocation = "", Location = "";
                if (BirthCountry > 0)
                {
                    var city = BirthCity ?? 0;
                    var dist = BirthDstrct ?? 0;
                    var location = _hrUnitOfWork.Repository<World>().FirstOrDefault(c => c.CountryId == BirthCountry
                    && c.CityId == city && c.DistrictId == dist);
                    BirthLocation = location != null ? Language.Substring(0, 2) == "ar" ? location.NameAr : location.Name : "";
                }

                // set title location
                if (CountryId > 0)
                {
                    var city = CityId ?? 0;
                    var dist = DistrictId ?? 0;
                    var location = _hrUnitOfWork.Repository<World>().FirstOrDefault(c => c.CountryId == CountryId
                    && c.CityId == city && c.DistrictId == dist);
                    Location = location != null ? Language.Substring(0, 2) == "ar" ? location.NameAr : location.Name : "";
                }

                return Json(new
                {
                    Result = true,
                    QualificationId = QualificationId,
                    ProviderId = ProviderId,
                    Nationality = Nationality,
                    BirthLocation = BirthLocation,
                    Location = Location
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SavePeople(PeoplesViewModel model, OptionsViewModel moreInfo, string LocalName, bool clear)
        {
            List<Error> errors = new List<Error>();
            string message = "OK";

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "People",
                        TableName = "People",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        foreach (var e in errors)
                        {
                            foreach (var errorMsg in e.errors)
                            {
                                ModelState.AddModelError(errorMsg.field, errorMsg.message);
                            }
                        }
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                }

                var record = _hrUnitOfWork.Repository<Person>().FirstOrDefault(j => j.Id == model.Id);
                if (model.SubscripDate == null)
                {
                    model.VarSubAmt = null;
                    model.BasicSubAmt = null;
                }

                if (record == null) //Add
                {
                    record = new Person();
                    string name = model.Title + " " + model.FirstName + " " + model.Familyname;
                    string oldName = record.Title + " " + record.FirstName + " " + record.Familyname;
                    _hrUnitOfWork.PositionRepository.AddLName(Language, oldName, name, LocalName);

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "People",
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });

                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    if (record.BirthCity == 0) record.BirthCity = null;
                    if (record.BirthDstrct == 0) record.BirthDstrct = null;
                    if (record.BirthCountry == 0) record.BirthCountry = null;
                    if (record.CityId == 0) record.CityId = null;
                    if (record.DistrictId == 0) record.DistrictId = null;
                    if (record.CountryId == 0) record.CountryId = null;

                    _hrUnitOfWork.PeopleRepository.Add(record);
                }
                else //update
                {
                    string name = model.Title + " " + model.FirstName + " " + model.Familyname;
                    string oldName = record.Title + " " + record.FirstName + " " + record.Familyname;
                    _hrUnitOfWork.PositionRepository.AddLName(Language, oldName, name, LocalName);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "People",
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });

                    record.BirthCountry = model.BirthCountry;
                    record.BirthCity = model.BirthCity;
                    record.BirthDstrct = model.BirthDstrct;
                    record.CountryId = model.CountryId;
                    record.CityId = model.CityId;
                    record.DistrictId = model.DistrictId;
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    if (record.MilitaryStat != 1)
                    {
                        record.MilResDate = null;
                        record.MilCertGrade = null;
                        record.Rank = null;
                    }
                }

                //##Save FlexData
                //bool isAdd = record.Id == 0;
                List<Error> Errors = new List<Error>();
                //if (isAdd) {
                //    Errors = SaveChanges(Language);
                //    if (Errors.Count > 0)
                //    {
                //        message = Errors.First().errors.First().message;
                //        return Json(message);
                //    }
                //}
                //SaveFlexData(moreInfo?.flexData, record.Id);
                if (record.Status < PersonStatus.Contract)
                {
                    record.Status = PersonStatus.Contract;
                    model.Status = record.Status;
                }
                Errors = SaveChanges(Language);
                //##End Save FlexData


                //if (record.HasImage && model.Id == 0)
                //{
                //    var chkImage = _hrUnitOfWork.Repository<CompanyDocsViews>().Where(a => a.CompanyId == CompanyId && a.SourceId == 0 && a.Source == "Employee").FirstOrDefault();
                //    if (chkImage != null)
                //    {
                //        chkImage.SourceId = record.Id;
                //        _hrUnitOfWork.CompanyRepository.Attach(chkImage);
                //        _hrUnitOfWork.CompanyRepository.Entry(chkImage).State = EntityState.Modified;
                //        _hrUnitOfWork.Save();
                //    }
                //}

                // int attachments;
                if (!clear)
                {
                    model.Id = record.Id;
                    model.Docs = _hrUnitOfWork.PeopleRepository.GetAttachmentsCount(model.Id/*, out attachments*/);
                    //model.Attachments = attachments;
                }
                else model = new PeoplesViewModel();

                message = "OK," + ((new JavaScriptSerializer()).Serialize(model));

                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }
            else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
        }

        public ActionResult _Profile()
        {
            FillBasicData(false, false, true, false);
            return PartialView(new PeoplesViewModel());
        }

        //DeletePeople                                                     
        public ActionResult DeletePeople(int id)
        {
            DataSource<PeoplesViewModel> Source = new DataSource<PeoplesViewModel>();
            Person person = _hrUnitOfWork.PeopleRepository.GetPerson(id);
            if (person != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = person,
                    ObjectName = "People",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.PeopleRepository.Remove(person);
                string name = person.Title + " " + person.FirstName + " " + person.Familyname;
                _hrUnitOfWork.PeopleRepository.RemoveLName(Language, name);
            }
            string message = "OK";
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else
                return Json(message);
        }

        // Developer Button Delate All Employees
        public ActionResult DelateAllEmployees()
        {
            var message = "OK";
            var ListOfAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.CompanyId == CompanyId).ToList();
            if (ListOfAssignment.Count > 0)
                _hrUnitOfWork.PeopleRepository.RemoveRange(ListOfAssignment);
            var ListofPerson = _hrUnitOfWork.Repository<Person>().ToList();
            _hrUnitOfWork.PeopleRepository.RemoveRange(ListofPerson);
            var ListOfEmployment = _hrUnitOfWork.Repository<Employement>().Where(a => a.CompanyId == CompanyId).ToList();
            if (ListOfEmployment.Count > 0)
                _hrUnitOfWork.PeopleRepository.RemoveRange(ListOfEmployment);
            var errors = SaveChanges(User.Identity.GetLanguage());
            if (errors.Count() > 0)
                message = errors.First().errors.First().message;
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Employement by Mamdouh
        public ActionResult ChkBeforeEmployment(int EmpId)
        {
            var chk = _hrUnitOfWork.CheckListRepository.ChkBeforeEmployment(CompanyId, UserName, EmpId, Language);
            return Json(chk, JsonRequestBehavior.AllowGet);
        }

        //Employment Details
        public ActionResult EmpDetails(int Id)
        {
            var employement = _hrUnitOfWork.PeopleRepository.GetEmployment(Id);
            if (employement == null) new EmployementViewModel();

            var GenEmpCode = _PersonSetup.GenEmpCode;
            ViewBag.GenEmpCode = GenEmpCode > 0 ? GenEmpCode : 2;
            ViewBag.Id = Id;

            var result = _hrUnitOfWork.Repository<Employement>().Where(a => a.CompanyId == CompanyId).Max(a => a.Sequence);
            employement.Sequence = result != null ? result + 1 : 1;
            ViewBag.nationalId = GenEmpCode == 3 ? _hrUnitOfWork.Repository<Person>().Where(a => a.Id == Id).Select(s => s.NationalId).FirstOrDefault() : "";

            employement.NotifyButtonLabel = MsgUtils.Instance.Trls("SendNotifyLetter", Culture);
            //EmployementViewModel
            return PartialView(employement);
        }

        [HttpGet]
        public JsonResult FillEmpDropdownlists()
        {
            try
            {
                var Currency = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
                var Jobs = _hrUnitOfWork.JobRepository.GetAllJobs(CompanyId, Culture, 0).Select(a => new { id = a.Id, name = a.LocalName }).ToList();
                var Locations = Language.Substring(0, 2) == "ar" ? _hrUnitOfWork.Repository<Country>().Select(c => new { id = c.Id, name = c.NameAr }).ToList() : _hrUnitOfWork.Repository<Country>().Select(c => new { id = c.Id, name = c.Name }).ToList();
                byte[] ids = { 1, 2, 4, 5 };
                var PersonType = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", Language).Select(a => new { id = a.CodeId, name = a.Title, SysCodeId = a.SysCodeId }).ToList().Where(a => ids.Contains(a.SysCodeId)).ToList();

                return Json(new
                {
                    Result = true,
                    Currency = Currency,
                    Jobs = Jobs,
                    Locations = Locations,
                    PersonType = PersonType
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SendNotifyLetter(int ID = 0)
        {
            if (ID == 0)
            {
                return Json(new { Result = false });
            }

            string ErrorMessage;
            bool Result = HangFireJobs.ExtendContractMethod(_hrUnitOfWork, Language, out ErrorMessage);

            return Json(new { Result = Result, Message = ErrorMessage });
        }

        public ActionResult CorrectEmpDetails(EmployementViewModel Emp, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            string message = "OK";
            DateTime EndDate = Emp.StartDate;
            var oldStartDate = Emp.StartDate;
            var oldEndDate = Emp.EndDate;
            if (!ModelState.IsValid)
                return Json(Models.Utils.ParseFormErrors(ModelState));
            // old employment record
            var record = _hrUnitOfWork.Repository<Employement>().Where(a => a.EmpId == Emp.EmpId && a.Status == 1 && a.CompanyId == CompanyId).OrderByDescending(a => a.StartDate).FirstOrDefault();
            var oldCode = record != null ? record.Code : "";
            var PreviousRecord = _hrUnitOfWork.Repository<Employement>().Where(a => a.EmpId == Emp.EmpId && a.Status != 1 && a.CompanyId == CompanyId).OrderByDescending(a => a.StartDate).FirstOrDefault();
            var date = (PreviousRecord != null ? PreviousRecord.EndDate : new DateTime(1900, 1, 2));
            var ContractIssueDate = Emp.ContIssueDate;
            var GenEmpCode = _PersonSetup.GenEmpCode;
            var assign = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == Emp.EmpId).FirstOrDefault();
            if (assign != null && Emp.PersonType == 5)
            {
                ModelState.AddModelError("", MsgUtils.Instance.Trls("TerminateFirst"));
                return Json(Models.Utils.ParseFormErrors(ModelState));

            }
            if (Emp.StartDate > date)
            {
                if (record != null)
                {
                    if (Emp.EndDate == null || (record.DurInYears != Emp.DurInYears || record.DurInMonths != Emp.DurInMonths))
                    {
                        int? year = Emp.DurInYears;
                        int? month = Emp.DurInMonths;
                        EndDate = EndDate.AddYears(year.Value).AddMonths(month.Value);
                    }
                    var assignment = _hrUnitOfWork.Repository<Assignment>().Where(a => (a.AssignDate >= record.StartDate && a.AssignDate <= record.EndDate) && a.EmpId == record.EmpId).FirstOrDefault();
                    if (assignment != null && Emp.EndDate <= assignment.AssignDate)
                    {
                        ModelState.AddModelError("", MsgUtils.Instance.Trls("cantcorrectemployment"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    if (assignment != null && Emp.StartDate > assignment.AssignDate)
                    {
                        ModelState.AddModelError("", MsgUtils.Instance.Trls("cantcorrectStartemploy"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    if (Emp.SysCodeId != 1)
                    {
                        Emp.AutoRenew = false;
                        Emp.RemindarDays = null;
                    }

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = Emp,
                        ObjectName = "Emp",
                        Options = moreInfo,
                        Id = "EmpId",
                        Transtype = TransType.Update
                    });

                    if (Emp.Code != oldCode)
                        message = _hrUnitOfWork.PeopleRepository.CheckCode(record, Language);

                    //if (Emp.EndDate == null && (Emp.DurInYears != 0 || Emp.DurInMonths != 0))
                    //{
                    //    int year = Emp.DurInYears;
                    //    int month = Emp.DurInMonths;
                    //  record.EndDate = EndDate.AddYears(year).AddMonths(month);
                    //}
                    //else
                    //    record.EndDate = EndDate;

                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.PersonType = Emp.PersonType;
                    record.StartDate = Emp.StartDate;
                    record.FromCountry = Emp.FromCountry;
                    record.ToCountry = Emp.ToCountry;
                    record.ContIssueDate = ContractIssueDate;
                    record.CompanyId = CompanyId;

                    if (record.EndDate != null && record.EndDate < record.StartDate)
                    {
                        ModelState.AddModelError("StartDate", MsgUtils.Instance.Trls("EndDateMustGrtThanStart"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }
                    record.Status = 1;
                    record.Code = Emp.Code;
                    _hrUnitOfWork.PeopleRepository.Attach(record);
                    _hrUnitOfWork.PeopleRepository.Entry(record).State = EntityState.Modified;
                    if (PreviousRecord != null)
                    {
                        if (Emp.StartDate < PreviousRecord.EndDate)
                        {
                            PreviousRecord.EndDate = Emp.StartDate.AddDays(-1);
                            _hrUnitOfWork.PeopleRepository.Attach(PreviousRecord);
                            _hrUnitOfWork.PeopleRepository.Entry(PreviousRecord).State = EntityState.Modified;
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("StartDate", MsgUtils.Instance.Trls("StartMustGrtThanPreviousStart"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
            if (message == "OK")
            {
                var Errors = SaveChanges(Language);

                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;
                else
                {
                    Emp.SysCodeId = _hrUnitOfWork.Repository<LookUpUserCode>().Where(a => (a.CodeName == "PersonType") && (a.CodeId == Emp.PersonType)).Select(s => s.SysCodeId).FirstOrDefault();
                    message += "," + ((new JavaScriptSerializer()).Serialize(Emp));
                }
            }

            return Json(message);

        }
        public ActionResult UpdateEmpDetails(EmployementViewModel Emp, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            string message = "OK";
            DateTime EndDate = Emp.StartDate;
            var OEndDate = Emp.EndDate;
            int companyId = User.Identity.GetDefaultCompany();
            if (!ModelState.IsValid)
                return Json(Models.Utils.ParseFormErrors(ModelState));

            var record = _hrUnitOfWork.Repository<Employement>().Where(a => a.EmpId == Emp.EmpId && a.Status == 1 && a.CompanyId == companyId).OrderByDescending(a => a.StartDate).FirstOrDefault();
            var oldSequence = record != null ? record.Sequence : Emp.Sequence;
            var oldStartDate = record != null ? record.StartDate : new DateTime(1900, 1, 2);
            var oldEndDate = record != null ? record.EndDate : new DateTime(2099, 1, 2);
            var oldPersonType = record != null ? record.PersonType : 1;
            var oldCode = record != null ? record.Code : string.Empty;
            var oldSalary = record != null ? record.Salary : null;
            var oldAllowances = record != null ? record.Allowances : null;
            var oldTicketCnt = record != null ? record.TicketCnt : null;
            var oldTicketAmt = record != null ? record.TicketAmt : null;
            var oldToCountry = record != null ? record.ToCountry : null;
            var oldFromCountry = record != null ? record.FromCountry : null;
            var oldJobDesc = record != null ? record.JobDesc : null;
            var oldBenefitDesc = record != null ? record.BenefitDesc : null;
            var oldSpecialCond = record != null ? record.SpecialCond : null;
            var EDate = _hrUnitOfWork.Repository<Employement>().Where(a => a.EmpId == Emp.EmpId && a.Status != 1 && a.CompanyId == companyId).Select(a => a.EndDate).OrderByDescending(a => a).FirstOrDefault();
            var date = (EDate != null ? EDate : oldStartDate.AddDays(-1));
            var assign = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == Emp.EmpId).FirstOrDefault();

            var status = record == null ? PersonStatus.Contract : 0;

            //var ActiveEmployment = 
            if (assign != null && Emp.PersonType == 5)
            {
                ModelState.AddModelError("", MsgUtils.Instance.Trls("TerminateFirst"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            if (record != null)
                if (Emp.StartDate < record.EndDate)
                {
                    ModelState.AddModelError("StartDate", MsgUtils.Instance.Trls("startLessEnd"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }

            Employement newEmployment = new Employement();
            if (Emp.SysCodeId != 1)
            {
                Emp.AutoRenew = false;
                Emp.RemindarDays = null;
            }

            if (Emp.StartDate != oldStartDate)
            {
                if (Emp.StartDate > date)
                {
                    if (record != null)
                    {
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = record,
                            Source = Emp,
                            ObjectName = "Emp",
                            Options = moreInfo,
                            Id = "EmpId",
                            Transtype = TransType.Update
                        });
                        if (Emp.Code != oldCode)
                            message = _hrUnitOfWork.PeopleRepository.CheckCode(record, Language);
                        record.ModifiedTime = DateTime.Now;
                        record.ModifiedUser = UserName;
                        record.Status = 2;
                        record.StartDate = oldStartDate;
                        record.PersonType = (short)oldPersonType;
                        record.Code = oldCode;
                        record.CompanyId = CompanyId;
                        if (oldEndDate == null)
                            record.EndDate = Emp.StartDate.AddDays(-1);
                        else if (oldEndDate != null && Emp.StartDate >= record.EndDate)
                            record.EndDate = Emp.StartDate.AddDays(-1);
                        else
                            record.EndDate = oldEndDate;
                        record.Sequence = oldSequence;
                        record.Salary = oldSalary;
                        record.Allowances = oldAllowances;
                        record.TicketCnt = oldTicketCnt;
                        record.SpecialCond = oldSpecialCond;
                        record.ToCountry = oldToCountry;
                        record.FromCountry = oldFromCountry;
                        record.JobDesc = oldJobDesc;
                        record.TicketAmt = oldTicketAmt;
                        _hrUnitOfWork.PeopleRepository.Attach(record);
                        _hrUnitOfWork.PeopleRepository.Entry(record).State = EntityState.Modified;
                    }
                    var Chkmsg = _hrUnitOfWork.CheckListRepository.ChkBeforeEmployment(CompanyId, UserName, Emp.EmpId, Language);
                    Emp.Error = Chkmsg;
                    var result = Chkmsg.Split(',');

                    if (result[0] == "OK" || result[0] == "SystemWarningDocuments")
                    {
                        //if (Emp.EndDate == null && (Emp.DurInYears != 0 || Emp.DurInMonths != 0))
                        //{
                        //    int year = Emp.DurInYears;
                        //    int month = Emp.DurInMonths;
                        //    EndDate = EndDate.AddYears(year).AddMonths(month);
                        //}
                        // new employement record
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = newEmployment,
                            Source = Emp,
                            Options = moreInfo,
                            Id = "EmpId",
                            Transtype = TransType.Insert
                        });
                        Emp.Id = newEmployment.Id;
                        if (OEndDate == null && (Emp.DurInYears != 0 || Emp.DurInMonths != 0))
                        {
                            newEmployment.EndDate = EndDate;
                        }
                        else
                        {
                            newEmployment.EndDate = Emp.EndDate;
                        }
                        newEmployment.CompanyId = CompanyId;
                        newEmployment.CreatedTime = DateTime.Now;
                        newEmployment.CreatedUser = UserName;
                        newEmployment.Status = 1;
                        newEmployment.Sequence = oldSequence;
                        if (Emp.Code != oldCode)
                        {
                            message = _hrUnitOfWork.PeopleRepository.CheckCode(newEmployment, Language);
                        }
                        _hrUnitOfWork.PeopleRepository.Add(newEmployment);
                    }
                    else
                    {

                        if (result[0] == "SystemErrorDocuments")
                        {
                            ModelState.AddModelError("PersonType", Chkmsg.Replace(result[0] + ",", ""));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                        else if (result[0] == "SystemCopyEmployment")
                        {
                            _hrUnitOfWork.SaveChanges();
                            ModelState.AddModelError("PersonType", Chkmsg.Replace(result[0] + ",", ""));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                        else
                        {
                            ModelState.AddModelError("PersonType", Chkmsg.Replace(result[0] + ",", ""));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("StartDate", MsgUtils.Instance.Trls("StartMustGrtThanPreviousStart"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }
            }
            else
            {
                ModelState.AddModelError("StartDate", MsgUtils.Instance.Trls("UpdateStartDateFirst"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            if (message == "OK")
            {
                if (status == PersonStatus.Contract)
                {
                    var person = _hrUnitOfWork.PeopleRepository.GetPerson(Emp.EmpId);
                    person.Status = PersonStatus.Papers;
                }

                var Errors = SaveChanges(Language);
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;
                else
                {
                    Emp.Id = newEmployment.Id;
                    Emp.SysCodeId = _hrUnitOfWork.Repository<LookUpUserCode>().Where(a => (a.CodeName == "PersonType") && (a.CodeId == Emp.PersonType)).Select(s => s.SysCodeId).FirstOrDefault();
                    message += "," + ((new JavaScriptSerializer()).Serialize(Emp));
                }
            }
            return Json(message);
        }
        #endregion

        #region Qualifications and Certificates by Mamdouh
        public ActionResult Qualifications(int id)
        {
            ViewBag.QualId = _hrUnitOfWork.PeopleRepository.getQualification("QualCat");
            ViewBag.CertID = _hrUnitOfWork.PeopleRepository.getCertification("QualCat");
            ViewBag.School = _hrUnitOfWork.Repository<School>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            ViewBag.Grade = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Grade", Language).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            ViewBag.Awarding = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Awarding", Language).Select(a => new { value = a.CodeId, text = a.Title }).ToList(); ;
            TempData["Id"] = id;
            return View();
        }
        //Read Qualification
        public ActionResult ReadQualification(int Id)
        {
            bool flag = true;
            return Json(_hrUnitOfWork.PeopleRepository.ReadQualifications(Id, flag), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadCertification(int Id)
        {
            bool flag = false;
            return Json(_hrUnitOfWork.PeopleRepository.ReadQualifications(Id, flag), JsonRequestBehavior.AllowGet);
        }
        //CreateQualification
        public ActionResult CreateQualification(IEnumerable<EmpQualificationViewModel> models, OptionsViewModel moreInfo)
        {
            var result = new List<PeopleQual>();
            var datasource = new DataSource<EmpQualificationViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Qualification",
                        TableName = "PeopleQuals",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (EmpQualificationViewModel q in models)
                {
                    var qual = new PeopleQual();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = qual,
                        Source = q,
                        Options = moreInfo,
                        Id = "EmpId",
                        Transtype = TransType.Insert
                    });
                    qual.EmpId = q.EmpId;
                    if (qual.StartDate > qual.FinishDate)
                    {
                        ModelState.AddModelError("FinishDate", MsgUtils.Instance.Trls("FinishMustGrtThanStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }
                    if (qual.GainDate > qual.ExpiryDate)
                    {
                        ModelState.AddModelError("ExpiryDate", MsgUtils.Instance.Trls("ExpiryMustGrtThanStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }

                    result.Add(qual);
                    _hrUnitOfWork.PeopleRepository.Add(qual);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }
            datasource.Data = (from q in models
                               join r in result on q.EmpId equals r.EmpId
                               select new EmpQualificationViewModel
                               {
                                   Id = r.Id,
                                   EmpId = q.EmpId,
                                   Status = q.Status,
                                   FinishDate = q.FinishDate,
                                   Grade = q.Grade,
                                   QualId = q.QualId,
                                   GradYear = q.GradYear,
                                   Notes = q.Notes,
                                   SchoolId = q.SchoolId,
                                   Score = q.Score,
                                   StartDate = q.StartDate,
                                   Title = q.Title,
                                   Awarding = q.Awarding,
                                   Cost = q.Cost,
                                   ExpiryDate = q.ExpiryDate,
                                   GainDate = q.GainDate,
                                   SerialNo = q.SerialNo
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        //UpdateQualification
        public ActionResult UpdateQualification(IEnumerable<EmpQualificationViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<EmpQualificationViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Qualification",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                for (var i = 0; i < models.Count(); i++)
                {
                    var qual = new PeopleQual();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        ObjectName = "Qualification",
                        Destination = qual,
                        Source = models.ElementAtOrDefault(i),
                        Options = options.ElementAtOrDefault(i),
                        Id = "EmpId",
                        Transtype = TransType.Update
                    });
                    if (qual.StartDate > qual.FinishDate)
                    {
                        ModelState.AddModelError("FinishDate", MsgUtils.Instance.Trls("FinishMustGrtThanStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }
                    if (qual.GainDate > qual.ExpiryDate)
                    {
                        ModelState.AddModelError("ExpiryDate", MsgUtils.Instance.Trls("ExpiryMustGrtThanStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }
                    qual.ModifiedUser = UserName;
                    qual.ModifiedTime = DateTime.Now;
                    _hrUnitOfWork.PeopleRepository.Attach(qual);
                    _hrUnitOfWork.PeopleRepository.Entry(qual).State = EntityState.Modified;
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        //DeleteQualification
        public ActionResult DeleteQualification(int Id)
        {
            var datasource = new DataSource<EmpQualificationViewModel>();

            var Obj = _hrUnitOfWork.Repository<PeopleQual>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "Qualification",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.PeopleRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }

        #endregion

        #region EmployeeTraining by Mamdouh

        public ActionResult EmployeeTraining(int id)
        {
            ViewBag.CourseId = _hrUnitOfWork.TrainingRepository.GetTrainCourse(Language, CompanyId).Select(p => new { value = p.Id, text = p.LocalName });
            TempData["Id"] = id;
            return View();
        }
        // Read Training
        public ActionResult ReadEmployeeTraining(int Id)
        {
            //  int Id = (int)TempData.Peek("Id");
            return Json(_hrUnitOfWork.PeopleRepository.ReadEmployeeTraining(Id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateEmpTraining(IEnumerable<PeopleTrainGridViewModel> models, OptionsViewModel moreInfo)
        {
            var result = new List<PeopleTraining>();
            var datasource = new DataSource<PeopleTrainGridViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PeopleTraining",
                        TableName = "PeopleTrain",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (PeopleTrainGridViewModel p in models)
                {
                    var training = new PeopleTraining();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = training,
                        Source = p,
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    training.CantLeave = p.CantLeaveDate;
                    training.EmpId = p.PersonId;
                    if (training.CourseSDate > training.CourseEDate)
                    {
                        ModelState.AddModelError("CourseEDate", MsgUtils.Instance.Trls("EndMustGrtThanStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }

                    result.Add(training);
                    _hrUnitOfWork.PeopleRepository.Add(training);
                }

                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from p in models
                               join r in result on p.PersonId equals r.EmpId
                               select new PeopleTrainGridViewModel
                               {
                                   Id = r.Id,
                                   PersonId = p.PersonId,
                                   CourseSDate = p.CourseSDate,
                                   CourseId = p.CourseId,
                                   Adwarding = p.Adwarding,
                                   ActualHours = p.ActualHours,
                                   CantLeaveDate = p.CantLeaveDate,
                                   CourseEDate = p.CourseEDate,
                                   CourseTitle = p.CourseTitle,
                                   Internal = p.Internal,
                                   Notes = p.Notes,
                                   Status = p.Status

                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult DeleteEmpTraining(int Id)
        {
            var datasource = new DataSource<PeopleTrainGridViewModel>();

            var Obj = _hrUnitOfWork.Repository<PeopleTraining>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "PeopleTraining",
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.PeopleRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        public ActionResult UpdateEmpTraining(IEnumerable<PeopleTrainGridViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<PeopleTrainGridViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "PeopleTraining",
                        TableName = "PeopleTrain",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                for (int i = 0; i < models.Count(); i++)
                {
                    var training = new PeopleTraining();

                    AutoMapper(new AutoMapperParm()
                    {
                        ObjectName = "PeopleTraining",
                        Destination = training,
                        Source = models.ElementAtOrDefault(i),
                        Options = options.ElementAtOrDefault(i),
                        Id = "PersonId",
                        Transtype = TransType.Update
                    });

                    if (training.CourseSDate >= training.CourseEDate)
                    {
                        ModelState.AddModelError("CourseEDate", MsgUtils.Instance.Trls("EndMustGrtThanStart"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }
                    training.CantLeave = models.ElementAtOrDefault(i).CantLeaveDate;
                    training.EmpId = models.ElementAtOrDefault(i).PersonId;
                    _hrUnitOfWork.PeopleRepository.Attach(training);
                    _hrUnitOfWork.PeopleRepository.Entry(training).State = EntityState.Modified;
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        #endregion

        #region EmploymentHistory by Mamdouh
        public ActionResult EmployementHistory(int id)
        {
            ViewBag.PersonType = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", Language).Select(a => new { value = a.CodeId, text = a.Title }).ToList();
            ViewBag.Status = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Status", Language).Select(a => new { value = a.CodeId, text = a.Title });
            ViewBag.Id = id;
            return View();
        }
        public ActionResult GetHitory(int Id)
        {
            return Json(_hrUnitOfWork.PeopleRepository.GetHistoryEmployement(Id), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteEmploymentHistory(int id)
        {
            List<Error> errors = new List<Error>();
            DataSource<EmployementViewModel> Source = new DataSource<EmployementViewModel>();
            Employement employment = _hrUnitOfWork.PeopleRepository.FindEmployment(id);

            if (employment != null)
            {
                var EndDate = employment.EndDate == null ? new DateTime(2099, 1, 1) : employment.EndDate;
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = employment,
                    ObjectName = "EmployementHistory",
                    Transtype = TransType.Delete
                });

                var assignstatus = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == employment.EmpId && ((a.AssignDate >= employment.StartDate && a.AssignDate <= EndDate) || (a.EndDate >= employment.StartDate && a.EndDate <= EndDate))).Select(a => a.AssignStatus).FirstOrDefault();
                if (assignstatus == 0)
                    _hrUnitOfWork.PeopleRepository.Remove(employment);
                else
                { // can't delete deployment if any assignment found
                  //byte sysCodeId = _hrUnitOfWork.Repository<LookUpUserCode>().Where(a => a.CodeName == "Assignment" && a.CodeId == assignstatus).Select(a => a.SysCodeId).FirstOrDefault();
                  //if (sysCodeId == 1)
                  //{
                    errors.Add(new Error() { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("cantdeleteAssignmet") } } });
                    Source.Errors = errors;
                    return Json(Source);
                    //}
                    //else
                    //{
                    //    _hrUnitOfWork.PeopleRepository.Remove(employment);
                    //}
                }

                var PreviousEmployment = _hrUnitOfWork.Repository<Employement>()
                  .Where(a => a.CompanyId == CompanyId && a.EmpId == employment.EmpId && a.Status == 2)
                  .OrderByDescending(a => a.StartDate)
                  .FirstOrDefault();
                if (PreviousEmployment != null && PreviousEmployment.EndDate != EndDate && PreviousEmployment.Status != 3)
                {
                    PreviousEmployment.Status = 1;
                    _hrUnitOfWork.PeopleRepository.Attach(PreviousEmployment);
                    _hrUnitOfWork.PeopleRepository.Entry(PreviousEmployment).State = EntityState.Modified;
                }
            }

            string message = "OK";
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else
                return Json(message);
        }
        #endregion

        #region MedicalServices
        //MedicalServices
        public ActionResult MedicalService(int id)
        {
            ViewBag.Id = id;
            ViewBag.BeneficiaryId = _hrUnitOfWork.Repository<EmpRelative>().Where(a => a.EmpId == id).Select(p => new { value = p.Id, text = p.Name }).ToList();
            ViewBag.Providers = _hrUnitOfWork.Repository<Provider>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            return View();
        }
        //  ReadMedicalService
        public ActionResult ReadMedicalService(int id)
        {
            var query = _hrUnitOfWork.MedicalRepository.ReadMedicalService(id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReadBenfitService(int PeriodId, int BeneficiaryId, int EmpId)
        {
            var query = _hrUnitOfWork.MedicalRepository.ReadBenfitService(PeriodId, BeneficiaryId, EmpId);
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Take Photo From Webcam
        public PartialViewResult TakePhotoPartial(int EmpID = 0)
        {
            TakePhotoVModel Model = new TakePhotoVModel();

            var stream = _hrUnitOfWork.Repository<CompanyDocsViews>().Where(a => a.Source == Db.Persistence.Constants.Sources.EmployeePic && a.SourceId == EmpID).Select(a => a.file_stream).FirstOrDefault();
            if (stream != null)
                Model.ImageData = Convert.ToBase64String(stream);

            Model.EmpID = EmpID;
            return PartialView("_TakePhotoPartial", Model);
        }



        #endregion
        #region Image
        public ActionResult Pic(int EmpId)
        {
            return View(EmpId);
        }
        public ActionResult ConvertFile(string Pic, int Id)
        {
            using (FileStream fs = new FileStream(Server.MapPath(@"../Content/Photos/default.png"), FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(Pic);
                    bw.Write(data);
                    bw.Close();
                }
                fs.Close();
            }

            string PathName = "../SpecialData/Photos/" + CompanyId.ToString();
            if (!Directory.Exists(Server.MapPath(PathName)))
                Directory.CreateDirectory(Server.MapPath(PathName));

            WebImage nweb = new WebImage(Server.MapPath(@"../Content/Photos/default.png"));
            nweb.Resize(530, 350);
            nweb.Save(Server.MapPath(string.Format(PathName + "/{0}", Id.ToString())), "jpeg");
            return Json(string.Format(PathName + "/{0}", Id.ToString()) + ".jpeg");
        }
        public ActionResult CropPic(string Source, int SourceId = 0)
        {
            var model = new { Source = Source, SourceId = SourceId };
            return View(model);
        }

        [HttpPost]
        public ActionResult _Upload(IEnumerable<HttpPostedFileBase> files)
        {

            string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" };

            if (files == null || !files.Any()) return Json(new { success = false, errorMessage = MsgUtils.Instance.Trls("No file uploaded") });
            var file = files.FirstOrDefault();  // get ONE only
            if (file == null || !IsImage(file, _imageFileExtensions)) return Json(new { success = false, errorMessage = MsgUtils.Instance.Trls("File is of wrong format.") });
            if (file.ContentLength <= 0) return Json(new { success = false, errorMessage = MsgUtils.Instance.Trls("File cannot be zero length.") });
            var webPath = GetTempSavedFilePath(file);

            return Json(new { success = true, fileName = webPath.Replace("\\", "/") }); // success
        }

        [HttpPost]
        public ActionResult RotateImage(string fileName)
        {
            try
            {
                var tempFolder = Server.MapPath("~/Content/Photos/TempFolder");
                var fn = Path.Combine(tempFolder, Path.GetFileName(fileName));
                var img = new WebImage(fn);
                var rotated = img.RotateRight();
                System.IO.File.Delete(fn);
                rotated.Save(fn);
                return Json("Ok");
            }
            catch (Exception ex)
            {
                return Json(MsgUtils.Instance.Trls(ex.Message));
            }

        }
        private string SaveBitMapImage(string Path)
        {
            var Bitm = new Bitmap(Server.MapPath(Path));
            using (MemoryStream m = new MemoryStream())
            {
                Bitm.Save(m, ImageFormat.Png);
            }
            return Path;

        }
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {

            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].MimeType == mimeType)
                    return encoders[i];
            }
            return null;
        }
        private bool IsImage(HttpPostedFileBase file, string[] arr)
        {
            if (file == null) return false;
            return file.ContentType.Contains("image") ||
                arr.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
        private string GetTempSavedFilePath(HttpPostedFileBase file)
        {
            // Define destination
            var tempfolder = "/Content/Photos/TempFolder";
            var serverPath = HttpContext.Server.MapPath(tempfolder);
            if (Directory.Exists(serverPath) == false)
            {
                Directory.CreateDirectory(serverPath);
            }

            // Generate unique file name
            var fileName = Path.GetFileName(file.FileName);
            fileName = SaveTemporaryAvatarFileImage(file, serverPath, fileName);

            // Clean up old files after every save
            CleanUpTempFolder(1);
            return Path.Combine(tempfolder, fileName);
        }
        private static string SaveTemporaryAvatarFileImage(HttpPostedFileBase file, string serverPath, string fileName)
        {
            var img = new WebImage(file.InputStream);
            var ratio = img.Height / (double)img.Width;
            img.Resize(600, (int)(600 * ratio));

            var fullFileName = Path.Combine(serverPath, fileName);
            if (System.IO.File.Exists(fullFileName))
            {
                System.IO.File.Delete(fullFileName);
            }

            img.Save(fullFileName);
            return Path.GetFileName(img.FileName);
        }
        private void CleanUpTempFolder(int hoursOld)
        {
            try
            {
                var currentUtcNow = DateTime.UtcNow;
                var serverPath = HttpContext.Server.MapPath("/Temp");
                if (!Directory.Exists(serverPath)) return;
                var fileEntries = Directory.GetFiles(serverPath);
                foreach (var fileEntry in fileEntries)
                {
                    var fileCreationTime = System.IO.File.GetCreationTimeUtc(fileEntry);
                    var res = currentUtcNow - fileCreationTime;
                    if (res.TotalHours > hoursOld)
                    {
                        System.IO.File.Delete(fileEntry);
                    }
                }
            }
            catch
            {
                // Deliberately empty.
            }
        }
        //public ActionResult ChangePicId(int Id)
        //{
        //    string path = Server.MapPath(@"../SpecialData/Photos/" + CompanyId.ToString());
        //    if (Directory.Exists(path))
        //    {
        //        if (System.IO.File.Exists(path + "/0.jpeg"))
        //            System.IO.File.Move(path + "/0.jpeg", string.Format("{0}/{1}.jpeg", path, Id));
        //    }
        //    return Json("Ok", JsonRequestBehavior.AllowGet);
        //}
        #endregion

    }
}
