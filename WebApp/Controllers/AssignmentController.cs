using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Interface.Core;
using WebApp.Extensions;
using Model.Domain;
using Model.ViewModel.Personnel;
using Model.ViewModel;
using System.Data.Entity;
using System.Linq.Dynamic;
using WebApp.Models;
using Model.Domain.Payroll;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace WebApp.Controllers
{
    public class AssignmentController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;

        private string UserName { get; set; }
        private string Language { get; set; }
        private int CompanyId { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
                UserName = requestContext.HttpContext.User.Identity.Name;
            }
        }
        public AssignmentController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region EmpBenefit
        public ActionResult EmpBenefitIndex(int id = 0)
        {
            ViewBag.BeneficiaryId = _hrUnitOfWork.Repository<EmpRelative>().Where(a => a.EmpId == id).Select(p => new { value = p.Id, text = p.Name }).ToList();
            ViewBag.BenefitplanId = _hrUnitOfWork.Repository<BenefitPlan>().Select(p => new { value = p.Id, text = p.PlanName }).ToList();
            ViewBag.benefitId = _hrUnitOfWork.EmployeeRepository.GetEmpBenefit(id, Language, CompanyId).Select(a => new { value = a.Id, text = a.Name });
            ViewBag.EmpId = id;
            return View();
        }
        public ActionResult GetEmpBenefit(int EmpId)
        {
            return Json(_hrUnitOfWork.EmployeeRepository.GetEmpBenefits(EmpId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBenPlanDetails(int BenPlanId)
        {
            var record = _hrUnitOfWork.Repository<BenefitPlan>().FirstOrDefault(a => a.Id == BenPlanId);
            return Json(record,JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBenfitClass(int[] Ids,int BenId)
        {
            //ids=benfitId

            string Msg = "";
            var listBen = _hrUnitOfWork.Repository<Benefit>().Where(a => Ids.Contains( a.Id));
            foreach (var item in listBen)
            {
                int countx = item.MaxFamilyCnt.GetValueOrDefault();
                if (countx == 0)
                    Msg = "";
                else
                {
                    int x = Ids.Where(a => a == item.Id).Count();
                    if (x <= countx)
                        Msg = "";
                    else
                        return Json(Msg = "Error");
                }
            }
            return Json(Msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBenfitPlan(int BenId)
        {
            //ids=benfitId
            var BenefitplanId = _hrUnitOfWork.Repository<BenefitPlan>().Where(a => a.BenefitId == BenId).Select(p => new { value = p.Id, text = p.PlanName }).ToList();
            return Json(BenefitplanId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateEmpBenefit(IEnumerable<EmployeeBenefitViewModel> models, IEnumerable<OptionsViewModel> options, int EmpId)
        {
            var result = new List<EmpBenefit>();
            var datasource = new DataSource<EmployeeBenefitViewModel>();
            var listBen = _hrUnitOfWork.Repository<Benefit>().ToList();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpBenefits",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (EmployeeBenefitViewModel model in models)
                {
                    model.sys = listBen.Where(a => a.Id == model.BenefitId).Select(a => a.BenefitClass).FirstOrDefault();
                    var empBen = new EmpBenefit
                    {
                        BenefitId = model.BenefitId,
                        BenefitPlanId = model.BenefitPlanId,
                        BeneficiaryId = model.BeneficiaryId,
                        EmpId = EmpId,
                        EndDate = model.EndDate,
                        StartDate = model.StartDate,
                        CreatedTime = DateTime.Now,
                        CreatedUser = UserName
                    };

                    if (model.StartDate > model.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }
                    result.Add(empBen);
                    _hrUnitOfWork.EmployeeRepository.Add(empBen);
                }
                datasource.Errors = SaveChanges(Language);
                datasource.Data = (from m in models
                                   join r in result on m.BenefitId equals r.BenefitId into g
                                   from r in g.DefaultIfEmpty()
                                   select new EmployeeBenefitViewModel
                                   {
                                       Id = (r == null ? 0 : r.Id),
                                       BenefitId = r.BenefitId,
                                       BenefitPlanId = r.BenefitPlanId,
                                       BenefitPlanName = m.BenefitPlanName,
                                       EmpId = EmpId,
                                       BeneficiaryId = r.BeneficiaryId,
                                       EndDate = r.EndDate,
                                       StartDate = r.StartDate
                                   }).ToList();
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
        public ActionResult UpdateEmpBenefit(IEnumerable<EmployeeBenefitViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<EmployeeBenefitViewModel>();
            datasource.Data = models;
            var listBen = _hrUnitOfWork.Repository<Benefit>().ToList();
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpBenefits",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (EmployeeBenefitViewModel model in models)
                {
                    model.sys = listBen.Where(a => a.Id == model.BenefitId).Select(a => a.BenefitClass).FirstOrDefault();
                    var empBen = new EmpBenefit()
                    {
                        Id = model.Id,
                        BenefitId = model.BenefitId,
                        BenefitPlanId = model.BenefitPlanId,
                        EmpId = model.EmpId,
                        EndDate = model.EndDate,
                        StartDate = model.StartDate,
                        BeneficiaryId = model.BeneficiaryId,
                        ModifiedTime = DateTime.Now,
                        ModifiedUser = UserName,
                        CreatedTime = model.CreatedTime,
                        CreatedUser = model.CreatedUser
                    };

                  //  AutoMapper(new AutoMapperParm() { ObjectName = "EmpBenefits", Destination = empBen, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Id="EmpId", });

                    if (empBen.StartDate > empBen.EndDate)
                    {
                        ModelState.AddModelError("EndDate", MsgUtils.Instance.Trls("EndDateGthanStartDate"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }
                    _hrUnitOfWork.EmployeeRepository.Attach(empBen);
                    _hrUnitOfWork.EmployeeRepository.Entry(empBen).State = EntityState.Modified;
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
        public ActionResult DeleteEmpBenefit(int Id)
        {
            var datasource = new DataSource<EmployeeBenefitViewModel>();
            var Obj = _hrUnitOfWork.Repository<EmpBenefit>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "EmpBenefits",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.EmployeeRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }

        #endregion

        #region EmpRelative
        public ActionResult EmpRelativeIndex(int id = 0)
        {
            ViewBag.Relations = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Relations", Language).Select(a => new { value = a.CodeId, text = a.Title });
            ViewBag.EmpId = id;
            return View();
        }
        public ActionResult GetEmpRelative(int EmpId)
        {
            return Json(_hrUnitOfWork.EmployeeRepository.GetEmpRelative(EmpId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateEmpRelative(IEnumerable<EmpRelativeViewModel> models, int EmpId)
        {
            var result = new List<EmpRelative>();
            var datasource = new DataSource<EmpRelativeViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpRelatives",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                foreach (EmpRelativeViewModel model in models)
                {

                    var empRelative = new EmpRelative
                    {

                        EmpId = EmpId,
                        Name = model.Name,
                        BirthDate = model.BirthDate,
                        Entry = model.Entry,
                        ExpiryDate = model.ExpiryDate,
                        GateIn = model.GateIn,
                        NationalId = model.NationalId,
                        Relation = model.Relation,
                        PassportNo = model.PassportNo,
                        Telephone = model.Telephone,
                        CreatedTime = DateTime.Now,
                        CreatedUser = UserName
                    };
                    if (model.BirthDate > model.ExpiryDate)
                    {
                        ModelState.AddModelError("ExpiryDate", MsgUtils.Instance.Trls("ExpiryDategreaterBirthDate"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }
                    result.Add(empRelative);
                    _hrUnitOfWork.EmployeeRepository.Add(empRelative);
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }


            datasource.Data = (from m in models
                               join r in result on m.Name equals r.Name into g
                               from r in g.DefaultIfEmpty()
                               select new EmpRelativeViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   EmpId = EmpId,
                                   Name = r.Name,
                                   BirthDate = r.BirthDate,
                                   Entry = r.Entry,
                                   ExpiryDate = r.ExpiryDate,
                                   GateIn = r.GateIn,
                                   NationalId = r.NationalId,
                                   Relation = r.Relation,
                                   PassportNo = r.PassportNo,
                                   Telephone = r.Telephone,
                                   CreatedTime = DateTime.Now,
                                   CreatedUser = User.Identity.Name
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateEmpRelative(IEnumerable<EmpRelativeViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<EmpRelativeViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpRelatives",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
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

                    var empRelative = new EmpRelative();
                    //{
                    //    Id = model.Id,
                    //    EmpId = model.EmpId,
                    //    Name = model.Name,
                    //    BirthDate = model.BirthDate,
                    //    Entry = model.Entry,
                    //    ExpiryDate = model.ExpiryDate,
                    //    GateIn = model.GateIn,
                    //    NationalId = model.NationalId,
                    //    Relation = model.Relation,
                    //    PassportNo = model.PassportNo,
                    //    Telephone = model.Telephone,
                    //    ModifiedTime = DateTime.Now,
                    //    ModifiedUser = UserName,
                    //    CreatedTime = model.CreatedTime,
                    //    CreatedUser = model.CreatedUser
                    //};
                    AutoMapper(new AutoMapperParm() { ObjectName = "EmpRelatives", Destination = empRelative, Source = models.ElementAtOrDefault(i), Version = 0, Options = options.ElementAtOrDefault(i),Id = "EmpId", });

                    if (empRelative.BirthDate > empRelative.ExpiryDate)
                    {
                        ModelState.AddModelError("ExpiryDate", MsgUtils.Instance.Trls("ExpiryDategreaterBirthDate"));
                        datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                        return Json(datasource);
                    }
                    _hrUnitOfWork.EmployeeRepository.Attach(empRelative);
                    _hrUnitOfWork.EmployeeRepository.Entry(empRelative).State = EntityState.Modified;
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
        public ActionResult DeleteEmpRelative(int Id)
        {
            var datasource = new DataSource<EmpRelativeViewModel>();
            var Obj = _hrUnitOfWork.Repository<EmpRelative>().FirstOrDefault(k => k.Id == Id);
            AutoMapper(new Models.AutoMapperParm
            {
                Source = Obj,
                ObjectName = "EmpRelatives",
                Version = Convert.ToByte(Request.Form["Version"]),
                Transtype = TransType.Delete
            });
            _hrUnitOfWork.EmployeeRepository.Remove(Obj);
            datasource.Errors = SaveChanges(Language);
            datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }
        #endregion

        #region Assignment
        public ActionResult GetAssignment(int MenuId, int pageSize, int skip)
        {
            var query = _hrUnitOfWork.EmployeeRepository.GetAssignments(Language);
            string filter = "";
            string Sorting = "";
            string whecls = GetWhereClause(MenuId);
            query = (IQueryable<AssignmentGridViewModel>)Utils.GetFilter(query, ref filter, ref Sorting);
            if (whecls.Length > 0 || filter.Length > 0)
            {
                try
                {
                    if (whecls.Length > 0 && filter.Length == 0)
                        query = query.Where(whecls);
                    else if (filter.Length > 0 && whecls.Length == 0)
                        query = query.Where(filter);
                    else
                        query = query.Where(filter).Where(whecls);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }

            var total = query.Count();

            if (Sorting.Length > 0)
                query = query.OrderBy(Sorting).Skip(skip).Take(pageSize);
            else if(skip > 0)
                query = query.OrderBy(a=>a.Code).Skip(skip).Take(pageSize);
            else
                query = query.Take(pageSize);

            return Json(new { total = total, data = query.ToList() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int Id)
        {
          
            var assignment = _hrUnitOfWork.EmployeeRepository.GetAssignment(Id,Language);
            var oldAssignment = _hrUnitOfWork.EmployeeRepository.Find(a => a.EmpId == Id).OrderByDescending(b => b.EndDate).FirstOrDefault();
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
            int EmpolyeeCompany = _hrUnitOfWork.PeopleRepository.GetEmployment(assignment.EmpId).CompanyId;


            FillViewBag(Language, EmpolyeeCompany,assignment.EmpId,assignment.PositionId,assignment.JobId,assignment.DepartmentId);
            return View(assignment);
        }
        private void FillViewBag(string Lang,int EmpCompany,int EmpId,int? positionid,int? JobId,int? DeptId)
        {
            var personel = _PersonSetup;
            ViewBag.JobDoc = personel != null ? personel.JobDoc : 0;
            ViewBag.AssignFlex = personel != null ? personel.AssignFlex : 0;
            ViewBag.AssignStatus = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Assignment", Lang).Where(a => a.SysCodeId == 1 || a.SysCodeId == 2).Select(a => new { id = a.CodeId, name = a.Title }).ToList();
            ViewBag.LocationId = _hrUnitOfWork.LocationRepository.ReadLocations(Lang, EmpCompany).Where(a => a.IsInternal).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.job = _hrUnitOfWork.JobRepository.ReadJobs(EmpCompany, Lang,0).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.Dept = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId,null, Language);
            ViewBag.Payroll = _hrUnitOfWork.Repository<Payrolls>().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.Position = _hrUnitOfWork.PositionRepository.GetPositions(Lang, EmpCompany).Where(p =>(p.JobId == JobId && p.DeptId == DeptId) && ((p.HiringStatus == 2) || (p.Id == (positionid != null ?positionid.Value : 0)))).Select(a => new { id = a.Id, name = a.Name, HeadCount = a.Headcount, ErrorMes = a.SysResponse }).ToList();
            ViewBag.PeopleGroup = _hrUnitOfWork.PeopleRepository.GetPeoples().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.PayrollGrad = _hrUnitOfWork.JobRepository.GetPayrollGrade();
            ViewBag.CareerPath = _hrUnitOfWork.JobRepository.ReadCareerPaths(EmpCompany).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.CompanyId = EmpCompany;
            ViewBag.ManagerId = _hrUnitOfWork.EmployeeRepository.EmployeeMangers(EmpCompany, Language, positionid).ToList();
        }
        private void RemoveChildren(List<ManagerEmployeeDiagram> array, int id)
        {
            var child = array.Where(a => a.ParentId == id).ToList();
            if (child.Count != 0)
            {
                for (int i = 0; i < child.Count; i++)
                {
                    array.Remove(child[i]);
                    RemoveChildren(array, child[i].Id);
                }
            }
        }

        [HttpPost]
        public ActionResult CorrectDetails(AssignmentFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            var record = _hrUnitOfWork.EmployeeRepository.Find(a => a.Id == model.Id).FirstOrDefault();
            string message = "OK";
            // var currentAssignment = _hrUnitOfWork.Repository<Assignment>().Where(b => b.EmpId == model.EmpId && (b.AssignDate < record.AssignDate)).OrderByDescending(c => c.AssignDate).FirstOrDefault();

            var currentAssignment = _hrUnitOfWork.EmployeeRepository.Find(a => a.Id == model.Id).FirstOrDefault();
            var oldAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.Id != currentAssignment.Id && a.EmpId == model.EmpId && a.AssignDate < model.AssignDate).OrderByDescending(c => c.AssignDate).FirstOrDefault();
            var futureAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.Id != currentAssignment.Id && a.EmpId == model.EmpId && a.AssignDate > model.AssignDate).OrderBy(c => c.AssignDate).FirstOrDefault();

            DateTime EndDate = (record != null ? record.EndDate : new DateTime(2099, 1, 1));
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.PositionRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "AssignmentsForm",
                        TableName = "Assignments",
                        Columns = Models.Utils.GetColumnViews(ModelState),
                        ParentColumn = "CompanyId",
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
            }
            else
                return Json(Models.Utils.ParseFormErrors(ModelState));


            if (oldAssignment != null && oldAssignment.EndDate >= model.AssignDate)
            {
                ModelState.AddModelError("AssignDate", MsgUtils.Instance.Trls("foundoldAssignment"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
                // error: system found old assignment terminated on oldAssignment.EndDate
                // return
            }

            if (futureAssignment != null && futureAssignment.AssignDate <= model.AssignDate)
            {
                ModelState.AddModelError("AssignDate", MsgUtils.Instance.Trls("foundFutureAssignment"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
                // error: system found future assignment will start on futureAssignment.AssignDate
                // return
            }
            Employement Employment = _hrUnitOfWork.Repository<Employement>().Where(a => a.EmpId == model.EmpId && a.Status == 1 && a.CompanyId == CompanyId).FirstOrDefault();

            bool chkEmployment = _hrUnitOfWork.EmployeeRepository.CheckEmployment(Employment, model.EmpId, model.AssignDate);
            if (chkEmployment == false)
            {
                ModelState.AddModelError("AssignDate", MsgUtils.Instance.Trls("haventcontract"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
            if (oldAssignment != null) // Edit
            {
                if (record.AssignDate != model.AssignDate)
                {
                    oldAssignment.ModifiedTime = DateTime.Now;
                    oldAssignment.ModifiedUser = UserName;
                    oldAssignment.EndDate = model.AssignDate.AddDays(-1);
                    _hrUnitOfWork.EmployeeRepository.Attach(oldAssignment);
                    _hrUnitOfWork.EmployeeRepository.Entry(oldAssignment).State = EntityState.Modified;
                }
            }
            var currentJobId = 0;
            var currentDepartmentId = 0;
            if (record.PositionId != model.PositionId && record.PositionId != null)
                ChangeManager(model.EmpId, model.PositionId);

            // Record Update in All conditions
            if (model.EmpTasks == 2)
                MapEligibilityCriteria(model, moreInfo);
            else
            {
                record.PeopleGroups = null;
                record.PayrollGrades = null;
                record.Payrolls = null;
                record.CompanyStuctures = null;
                record.Locations = null;
                record.Jobs = null;
                record.Employments = null;
                record.Positions = null;
            }

            AutoMapper(new AutoMapperParm
            {
                Destination = record,
                Source = model,
                ObjectName = "AssignmentsForm",
                Version = 0,
                Id = "EmpId",
                Options = moreInfo
            });

            if (currentJobId != 0 && currentDepartmentId != 0)
            {
                currentAssignment.JobId = currentJobId;
                currentAssignment.DepartmentId = currentDepartmentId;
            }

            record.ModifiedTime = DateTime.Now;
            record.ModifiedUser = UserName;
            record.EndDate = EndDate;
            record.SysAssignStatus = _hrUnitOfWork.Repository<LookUpUserCode>().Where(a => a.CodeName == "Assignment" && a.CodeId == model.AssignStatus).FirstOrDefault().SysCodeId;
            _hrUnitOfWork.EmployeeRepository.Attach(record);
            _hrUnitOfWork.EmployeeRepository.Entry(record).State = EntityState.Modified;

            errors = SaveChanges(Language);
            if (errors.Count > 0)
                message = errors.First().errors.First().message;
            else
                message += "," + (new JavaScriptSerializer()).Serialize(record);
            return Json(message);
        }
        [HttpPost]
        public ActionResult UpdateDetails(AssignmentFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            DateTime EndDate = new DateTime(2099, 1, 1);
            DateTime Now = DateTime.Now;

            string message = "OK";

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.PositionRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "AssignmentsForm",
                        TableName = "Assignments",
                        Columns = Models.Utils.GetColumnViews(ModelState),
                        ParentColumn = "CompanyId",
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
            }
            else
                return Json(Models.Utils.ParseFormErrors(ModelState));
            var currentJobId = 0;
            var currentDepartmentId = 0;
            var currentAssignment = _hrUnitOfWork.EmployeeRepository.Find(a => a.Id == model.Id).FirstOrDefault();
            Assignment newAssignment = new Assignment();

            if (currentAssignment != null)
            {
                currentJobId = currentAssignment.JobId;
                currentDepartmentId = currentAssignment.DepartmentId;
            }
            // employee has active assignment and u try to insert new in the same period
            if (currentAssignment != null && model.AssignDate >= currentAssignment.AssignDate && model.AssignDate <= currentAssignment.EndDate && currentAssignment.EndDate < EndDate)
            {
                // Employee already has assignment in this period
                ModelState.AddModelError("AssignDate", MsgUtils.Instance.Trls("AlreadyHasAssignmentInPeriod"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            if (currentAssignment == null) // New
                currentAssignment = new Assignment() { AssignDate = model.AssignDate };


            var oldAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.Id != currentAssignment.Id && a.EmpId == model.EmpId && a.AssignDate < model.AssignDate).OrderByDescending(c => c.AssignDate).FirstOrDefault();

            // employee has active assignment and you try to insert new before current period
            if (currentAssignment.Id > 0 && oldAssignment == null && model.AssignDate < currentAssignment.AssignDate)
            {
                ModelState.AddModelError("AssignDate", MsgUtils.Instance.Trls("CantAssignOldDate"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
                // error: You can't assign employee in old date
            }
            else if (oldAssignment != null && oldAssignment.AssignDate >= currentAssignment.AssignDate)
            {
                ModelState.AddModelError("AssignDate", MsgUtils.Instance.Trls("AlreadyHasAssignmentInPeriod"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
                // error: system found future assignment between current assignment and new assignment
            }
            else if (oldAssignment != null && oldAssignment.EndDate >= model.AssignDate)
            {
                ModelState.AddModelError("AssignDate", MsgUtils.Instance.Trls("foundoldAssignment"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
                // error: system found old assignment terminated on oldAssignment.EndDate
            }

            var futureAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.Id != currentAssignment.Id && a.EmpId == model.EmpId && a.AssignDate > model.AssignDate).OrderBy(c => c.AssignDate).FirstOrDefault();
            if (futureAssignment != null && futureAssignment.AssignDate <= model.AssignDate)
            {
                ModelState.AddModelError("AssignDate", MsgUtils.Instance.Trls("foundFutureAssignment"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
                // error: system found future assignment will start on futureAssignment.AssignDate
            }
            Employement Employment = _hrUnitOfWork.Repository<Employement>().Where(a => a.EmpId == model.EmpId && a.Status == 1 && a.CompanyId == CompanyId).FirstOrDefault();
            bool chkEmployment = _hrUnitOfWork.EmployeeRepository.CheckEmployment(Employment, model.EmpId, model.AssignDate);
            if (chkEmployment == false)
            {
                ModelState.AddModelError("AssignDate", MsgUtils.Instance.Trls("haventcontract"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
            var AllSysAssignStatus = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Assignment", Language);

            if (currentAssignment.Id == 0) // New
            {
                //currentAssignment = new Assignment();
                //model.Id = currentAssignment.Id;
                if (model.EmpTasks == 2)
                    MapEligibilityCriteria(model, moreInfo);

                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = currentAssignment,
                    Source = model,
                    ObjectName = "AssignmentsForm",
                    Version = 0,
                    Id = "EmpId",
                    Options = moreInfo
                });

                if (currentJobId != 0 && currentDepartmentId != 0)
                {
                    currentAssignment.JobId = currentJobId;
                    currentAssignment.DepartmentId = currentDepartmentId;
                }
                currentAssignment.EndDate = futureAssignment == null ? EndDate : futureAssignment.AssignDate.AddDays(-1);
                currentAssignment.CreatedTime = Now;
                currentAssignment.CreatedUser = UserName;
                currentAssignment.SysAssignStatus = AllSysAssignStatus.Where(a => a.CodeId == model.AssignStatus).FirstOrDefault().SysCodeId;
                _hrUnitOfWork.EmployeeRepository.Add(currentAssignment);
            }
            else // Edit
            {

                if (currentAssignment.AssignDate == model.AssignDate)
                {
                    // error: you must change assignment date to successfully update assignment
                    // return
                    ModelState.AddModelError("AssignDate", MsgUtils.Instance.Trls("can't updateassignment"));
                    return Json(Models.Utils.ParseFormErrors(ModelState));
                }

                DateTime oldDate = currentAssignment.AssignDate;
                var assignStatus = currentAssignment.AssignStatus;
                if (model.EmpTasks == 2)
                    MapEligibilityCriteria(model, moreInfo);
                else
                {
                    currentAssignment.PeopleGroups = null;
                    currentAssignment.PayrollGrades = null;
                    currentAssignment.Payrolls = null;
                    currentAssignment.CompanyStuctures = null;
                    currentAssignment.Locations = null;
                    currentAssignment.Jobs = null;
                    currentAssignment.Employments = null;
                    currentAssignment.Positions = null;
                }

                if (currentAssignment.PositionId != model.PositionId && currentAssignment.PositionId != null)
                    ChangeManager(model.EmpId, model.PositionId);

                //update assignment
                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = currentAssignment,
                    Source = model,
                    ObjectName = "AssignmentsForm",
                    Version = 0,
                    Id = "EmpId",
                    Options = moreInfo
                });
                if (currentJobId != 0 && currentDepartmentId != 0)
                {
                    currentAssignment.JobId = currentJobId;
                    currentAssignment.DepartmentId = currentDepartmentId;
                }
                currentAssignment.AssignStatus = assignStatus;
                currentAssignment.AssignDate = oldDate;
                currentAssignment.EndDate = model.AssignDate.AddDays(-1); ;
                currentAssignment.ModifiedTime = Now;
                currentAssignment.ModifiedUser = UserName;
                currentAssignment.SysAssignStatus = AllSysAssignStatus.FirstOrDefault(a => a.CodeId == assignStatus).SysCodeId;

                _hrUnitOfWork.EmployeeRepository.Attach(currentAssignment);
                _hrUnitOfWork.EmployeeRepository.Entry(currentAssignment).State = EntityState.Modified;

                //insert new assignment
                AutoMapper(new AutoMapperParm
                {
                    Destination = newAssignment,
                    Source = model,
                    ObjectName = "AssignmentsForm",
                    Version = 0,
                    Id = "EmpId",
                    Options = moreInfo
                });


                newAssignment.EndDate = new DateTime(2099, 1, 1);
                newAssignment.CreatedTime = Now;
                newAssignment.Id = 0;
                newAssignment.CreatedUser = UserName;
                newAssignment.SysAssignStatus = AllSysAssignStatus.Where(a => a.CodeId == model.AssignStatus).FirstOrDefault().SysCodeId;
                _hrUnitOfWork.EmployeeRepository.Add(newAssignment);
            }

            errors = SaveChanges(Language);
            if (errors.Count > 0)
                message = errors.First().errors.First().message;
            else
                message += "," + (new JavaScriptSerializer()).Serialize(currentAssignment.Id == 0 ? currentAssignment : newAssignment);
            return Json(message);
        }

        public ActionResult AssignmentHistory(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult GetHitory(int Id)
        {
            var query = _hrUnitOfWork.EmployeeRepository.GetHistoryAssignments(CompanyId, Language, Id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public void ChangeManager(int EmpId, int? NewPosition)
        {
            var EmpsWitholdManger = _hrUnitOfWork.Repository<Assignment>().Where(a => a.ManagerId == EmpId).ToList();
            if (EmpsWitholdManger.Count != 0)
            {
                var NewManger = _hrUnitOfWork.EmployeeRepository.EmployeeMangers(CompanyId, Language, NewPosition).FirstOrDefault();
                for (int i = 0; i < EmpsWitholdManger.Count; i++)
                {
                    Assignment model = EmpsWitholdManger.ElementAt(i);
                    if (NewManger != null)
                        model.ManagerId = NewManger.id;
                    else
                        model.ManagerId = null;
                    _hrUnitOfWork.EmployeeRepository.Attach(model);
                    _hrUnitOfWork.EmployeeRepository.Entry(model).State = EntityState.Modified;

                }

            }
        }
        public ActionResult CheckManager(int? assid, int DepId)
        {
            var assignment = _hrUnitOfWork.Repository<Assignment>().Where(a => (a.DepartmentId == DepId) && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today)&&(a.CompanyId==CompanyId) && (a.IsDepManager)).Select(b => b).FirstOrDefault();
            var BranchName = _hrUnitOfWork.EmployeeRepository.BranchName(DepId,Language).FirstOrDefault();
            var Sector = _hrUnitOfWork.EmployeeRepository.Sector(DepId,Language).FirstOrDefault();
            if (assignment == null)
            {
                assignment = new Assignment();
                assignment.IsDepManager = true;

            }
            else if (assignment.Id == assid)
                assignment.IsDepManager = true;
            else if (assid != assignment.Id)
                assignment.IsDepManager = false;
            else
                assignment.IsDepManager = false;

            assignment.BranchId = BranchName !=null ? (int?)BranchName.id: null;
            assignment.SectorId = Sector != null ? (int?)Sector.id : null;
            assignment.CreatedUser = BranchName != null ? BranchName.name : null ;

            return Json(assignment, JsonRequestBehavior.AllowGet);
        }
        public void MapEligibilityCriteria(AssignmentFormViewModel model, OptionsViewModel moreInfo)
        {
            model.Locations = model.ILocations == null ? null : string.Join(",", model.ILocations.ToArray());
            model.Jobs = model.IJobs == null ? null : string.Join(",", model.IJobs.ToArray());
            model.Employments = model.IEmployments == null ? null : string.Join(",", model.IEmployments.ToArray());
            model.PeopleGroups = model.IPeopleGroups == null ? null : string.Join(",", model.IPeopleGroups.ToArray());
            model.Payrolls = model.IPayrolls == null ? null : string.Join(",", model.IPayrolls.ToArray());
            model.PayrollGrades = model.IPayrollGrades == null ? null : string.Join(",", model.IPayrollGrades.ToArray());
            model.CompanyStuctures = model.ICompanyStuctures == null ? null : string.Join(",", model.ICompanyStuctures.ToArray());
            model.Positions = model.IPositions == null ? null : string.Join(",", model.IPositions.ToArray());
            moreInfo.VisibleColumns.Add("Locations");
            moreInfo.VisibleColumns.Add("Jobs");
            moreInfo.VisibleColumns.Add("Employments");
            moreInfo.VisibleColumns.Add("PeopleGroups");
            moreInfo.VisibleColumns.Add("Payrolls");
            moreInfo.VisibleColumns.Add("PayrollGrades");
            moreInfo.VisibleColumns.Add("CompanyStuctures");
            moreInfo.VisibleColumns.Add("Positions");
        }
        public ActionResult GetManagers(int PostionId)
        {
            return Json(_hrUnitOfWork.EmployeeRepository.EmployeeMangers(CompanyId, Language, PostionId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPositions(int JobId,int DeptId, int EmpId, string tableName)
        {
            bool hasDocs = _hrUnitOfWork.EmployeeRepository.CheckDocs(CompanyId, JobId, EmpId);
            var Positions = _hrUnitOfWork.PositionRepository.GetPositions(Language, CompanyId).Where(j => (j.JobId == JobId && j.DeptId == DeptId) && (j.HiringStatus == 2)).Select(a => new { id = a.Id, name = a.Name, HeadCount = a.Headcount, ErrorMes = a.SysResponse}).ToList();
            var Result = _hrUnitOfWork.EmployeeRepository.GetFlexDataCheck(tableName, JobId, EmpId);

            var PsotionsObj = new { Positions = Positions, Result = Result != "" ? MsgUtils.Instance.Trls("reuqiredskills") + Result : "", Check = hasDocs };
            return Json(PsotionsObj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FlexDataCheck(string tableName, int SourceId, int EmpId)
        {
            var Result = _hrUnitOfWork.EmployeeRepository.GetFlexDataCheck(tableName, SourceId, EmpId);
            if (Result != "")
                return Json(MsgUtils.Instance.Trls("reuqiredskills") + Result, JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);

        }
        public ActionResult AssignFlexPos(string tableName, int SourceId, int EmpId,int? HeadCount)
        {
            var Result = _hrUnitOfWork.EmployeeRepository.GetFlexDataCheck(tableName, SourceId, EmpId);
            bool AssiGrt = false;
            if (HeadCount != null)
            {
                var AssigCount = _hrUnitOfWork.EmployeeRepository.GetAssignments(Language).Where(a =>a.CompanyId == CompanyId && a.PositionId == SourceId).Count();
                if (AssigCount < HeadCount)
                    AssiGrt = true;
            }
            var Obj = new { AssResult = (Result != "" ? MsgUtils.Instance.Trls("reuqiredskills") + Result : ""), Grt = AssiGrt };
            return Json(Obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PosError(int SourceId,int? HeadCount)
        {
            bool AssiGrt = false;
            if (HeadCount != null)
            {
                var AssigCount = _hrUnitOfWork.EmployeeRepository.GetAssignments(Language).Where(a => a.CompanyId == CompanyId && a.PositionId == SourceId).Count();
                if (AssigCount < HeadCount)
                    AssiGrt = true;
            }
            return Json(AssiGrt,JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAssignmentHistory(int id)
        {
            List<Error> errors = new List<Error>();
            DataSource<AssignHistoryViewModel> Source = new DataSource<AssignHistoryViewModel>();
            Assignment assignment = _hrUnitOfWork.PeopleRepository.FindAssignment(id);
            if (assignment != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = assignment,
                    ObjectName = "AssignmentHistory",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });

                DateTime Enddate = assignment.AssignDate.AddDays(-1);
                int empId = assignment.EmpId;

                _hrUnitOfWork.PeopleRepository.Remove(assignment);

                var PreviousAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == empId && a.EndDate == Enddate).FirstOrDefault();
                if(PreviousAssignment != null)
                {
                    PreviousAssignment.EndDate = new DateTime(2099, 1, 1);
                    _hrUnitOfWork.EmployeeRepository.Attach(PreviousAssignment);
                    _hrUnitOfWork.EmployeeRepository.Entry(PreviousAssignment).State = EntityState.Modified;
                }
            }

            string message = "OK";
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count > 0)
                return Json(Source);
            else
                return Json(message);
        }
    }
    #endregion
}