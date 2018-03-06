using System.Linq;
using System.Web.Mvc;
using Interface.Core;
using Model.Domain;
using Model.ViewModel.Personnel;
using System.Data.Entity;
using WebApp.Extensions;
using System.Collections.Generic;
using Model.ViewModel;
using System;
using WebApp.Models;
using System.IO;
using Model.ViewModel.Payroll;
using Model.Domain.Payroll;
using System.Web.Routing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Model.ViewModel.Administration;

namespace WebApp.Controllers
{
    public class PersonnelController : BaseController
    {

        private IHrUnitOfWork _hrUnitOfWork;
        private UserContext db;
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
        public PersonnelController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
            db = new UserContext();
        }
        #region Personnel
        public ActionResult Index()
        {
            FillViewBag();
            var oldPersonnel = _PersonSetup;
            if (oldPersonnel.Id == -1)
            {
                return View(new PersonSetup() { Id = -1 });
            }
            else
            {
                return oldPersonnel == null ? (ActionResult)HttpNotFound() : View(oldPersonnel);
            }
        }

        void FillViewBag()
        {
            ViewBag.TermSysCode = _hrUnitOfWork.LookUpRepository.GetsyscodeForm("Termination", Language);
            ViewBag.Role = _hrUnitOfWork.LeaveRepository.GetOrgChartRoles(Language).Select(a => new { text = a.text, value = a.value }).ToList();
            ViewBag.CurrencyCode = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            ViewBag.calender = _hrUnitOfWork.Repository<PeriodName>().Where(J=>((J.IsLocal && J.CompanyId == CompanyId) || J.IsLocal == false) && (J.StartDate <= DateTime.Today && (J.EndDate == null || J.EndDate >= DateTime.Today))).Select(a => new { id = a.Id, name = a.Name }).ToList();
            //ViewBag.QualGroups = _hrUnitOfWork.QualificationRepository.GetQualGroups().Select(q => new FormList() { id = q.Id, name = q.Name }).ToList();
        }
        private List<Error> SaveGrid2(WorkFlowRangVM grid1, IEnumerable<KeyValuePair<string, ModelState>> state, PersonSetup workflow)
        {
            List<Error> errors = new List<Error>();

            // Exclude delete models from sever side validations
            if (ServerValidationEnabled)
            {
                var modified = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted")));
                if (modified.Count > 0)
                {
                    errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "WorkflowObject",
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0) return errors;
                }
            }

            // updated records
            if (grid1.updated != null)
            {
                foreach (WorkFlowObjectsViewModel model in grid1.updated)
                {
                    var newworkflow = new Workflow();
                    AutoMapper(new Models.AutoMapperParm { Destination = newworkflow, Source = model, Transtype = TransType.Update });
                    workflow.ModifiedTime = DateTime.Now;
                    workflow.ModifiedUser = UserName;
                    _hrUnitOfWork.LeaveRepository.Attach(newworkflow);
                    _hrUnitOfWork.LeaveRepository.Entry(newworkflow).State = EntityState.Modified;
                }
            }
            var dbWorkflow = _hrUnitOfWork.Repository<Workflow>().Where(a => a.CompanyId == CompanyId).ToList();
            // inserted records
            if (dbWorkflow.Count == 0)
            {
                if (grid1.inserted != null)
                {
                    foreach (WorkFlowObjectsViewModel model in grid1.inserted)
                    {
                        var newworkflow = new Workflow();
                        AutoMapper(new Models.AutoMapperParm { Destination = newworkflow, Source = model, Transtype = TransType.Insert });
                        _hrUnitOfWork.LeaveRepository.Add(newworkflow);
                    }
                }
            }
            else
            {
                //new Inserted record
                if (grid1.inserted != null)
                {
                    foreach (WorkFlowObjectsViewModel model in grid1.inserted)
                    {
                        var newworkflow = new Workflow();
                        AutoMapper(new Models.AutoMapperParm { Destination = newworkflow, Source = model, Transtype = TransType.Insert });
                        _hrUnitOfWork.LeaveRepository.Add(newworkflow);
                    }
                }
            }
            
            return errors;
        }
        public ActionResult CreatePersonnel(PersonnelViewModel model, OptionsViewModel moreInfo, WorkFlowRangVM grid1)
        {
            List<Error> errors = new List<Error>();
            if (model.ContractTempl != null)
                model.ContractTempl = model.ContractTempl.Split('\\').LastOrDefault().Split('.')[0] + ".docx";

            if (ModelState.IsValid)
            {
                var record = _PersonSetup;
                if (record.Id == -1)
                {
                    record = new PersonSetup();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "PersonnelSetting",
                        Options = moreInfo,
                        Transtype= TransType.Insert
                    });
                    record.Id = CompanyId;
                    _PersonSetup = record;
                    _hrUnitOfWork.LookUpRepository.Add(record);
                }
                else
                {
                    record.TaskPeriod = null;
                    record.BudgetPeriod = null;

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "PersonnelSetting",
                        Options = moreInfo,
                        Id = "CompanyId",
                        Transtype = TransType.Update
                    });
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    _PersonSetup = record;
                    _hrUnitOfWork.LookUpRepository.Attach(record);
                    _hrUnitOfWork.LookUpRepository.Entry(record).State = EntityState.Modified;
                }
                errors = SaveGrid2(grid1, ModelState.Where(a => a.Key.Contains("grid1")),record);
                var Errors = SaveChanges(Language);
                #region ContractFile
                if (!string.IsNullOrEmpty(model.ContractTempl))
                {
                    if (!Directory.Exists(Server.MapPath("/SpecialData/Contracts/" + CompanyId.ToString())))
                        Directory.CreateDirectory(Server.MapPath("/SpecialData/Contracts/" + CompanyId.ToString()));
                    else
                    {
                        var Files = Directory.GetFiles(Server.MapPath("/SpecialData/Contracts/" + CompanyId.ToString()), "*.*");

                        FileStream stream = null;
                        foreach (var Dirfile in Files)
                        {
                            FileInfo f = new FileInfo(Dirfile);
                            try
                            {
                                stream = f.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                            }
                            catch (IOException)
                            {
                                Errors.Add(new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("FileinUse") } } });
                            }

                            finally
                            {
                                if (stream != null)
                                    stream.Close();

                                System.IO.File.SetAttributes(Dirfile, FileAttributes.Normal);
                            }
                            if (Errors.Count > 0)
                            {
                                ModelState.AddModelError("ContractTempl", MsgUtils.Instance.Trls("FileinUse"));
                                return Json(Utils.ParseFormErrors(ModelState));
                            }
                            else
                                System.IO.File.Delete(Dirfile);
                        }
                    }
                    if (System.IO.File.Exists(Server.MapPath("/SpecialData/TempFolder/" + model.ContractTempl)))
                        System.IO.File.Move(Server.MapPath("/SpecialData/TempFolder/" + model.ContractTempl), Server.MapPath(string.Format("/SpecialData/Contracts/{0}/{1}", CompanyId.ToString(), model.ContractTempl)));
                }
                #endregion

                var message = "OK,";
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                Utils.MaxPassTrials = record.MaxPassTrials;
                Utils.WaitingInMinutes = record.WaitingInMinutes;
                return Json(message);
            }
            return Json(Utils.ParseFormErrors(ModelState));
        }
        public ActionResult ReadRequestWf(int PersonnelId)
        {
            return Json(_hrUnitOfWork.LeaveRepository.GetWfRole(PersonnelId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult WorkFlowIndex(int TerminateId, string Source)
        {

            //ViewBag.RoleCode = _hrUnitOfWork.LeaveRepository.GetOrgChartRoles(Language).Select(a => new { text = a.text, value = a.value }).ToList();
            //ViewBag.Role = db.Roles.Select(r => new { value = r.Id, text = r.Name });
            //ViewBag.Hierarchy = _hrUnitOfWork.Repository<Diagram>().Select(a => new { id = a.Id, name = a.Name });
            //// string source = "Termination";
            //ViewBag.sourceId = TerminateId;
            //ViewBag.sourceTxt = Source;

            var reqestwf = _hrUnitOfWork.LeaveRepository.ReadRequestWF(TerminateId, Source, Language);
            if (reqestwf == null)
                return View(new RequestWfFormViewModel());
            else
                return reqestwf == null ? (ActionResult)HttpNotFound() : View(reqestwf);
        }

        //ReadWorkFlow
        public ActionResult ReadWorkFlow()
        {
            return Json(_hrUnitOfWork.LeaveRepository.ReadWorkFlowObjects(CompanyId,Language), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Holidays
        public ActionResult Calendar()
        {
            var weekend = _PersonSetup;
            ViewBag.weekend = new { weekend.Weekend1, weekend.Weekend2 };

            return View();
        }

        //read grids
        public ActionResult GetStanderedHolidays()
        {
            return Json(_hrUnitOfWork.LeaveRepository.ReadStanderedHolidays(CompanyId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomHolidays()
        {
            return Json(_hrUnitOfWork.LeaveRepository.ReadCustomHolidays(CompanyId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveCalendar(WeekendViewModel model, HolidayVM grid1, HolidayVM grid2)
        {
            string message = "OK";
            var personnel = _hrUnitOfWork.Repository<PersonSetup>().FirstOrDefault(p => p.Id == CompanyId);
            if (personnel == null)
            {
                ModelState.AddModelError("", MsgUtils.Instance.Trls("NoPersonnelSettings"));
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }

            personnel.Weekend1 = model.Weekend1;
            personnel.Weekend2 = model.Weekend2;
            personnel.ModifiedUser = UserName;
            personnel.ModifiedTime = DateTime.Now;

            _hrUnitOfWork.LookUpRepository.Attach(personnel);
            _hrUnitOfWork.LookUpRepository.Entry(personnel).State = EntityState.Modified;

            //Save Standerd Holiday
            var errors = SaveStandredGrid(grid1, ModelState.Where(a => a.Key.Contains("grid1")), true);
            if (errors.Count > 0)
            {
                message = errors.First().errors.First().message;
                return Json(message);
            }

            //Save Custom Holiday
            errors = SaveStandredGrid(grid2, ModelState.Where(a => a.Key.Contains("grid2")), false);
            if (errors.Count > 0)
            {
                message = errors.First().errors.First().message;
                return Json(message);
            }

            errors = SaveChanges(Language);
            if (errors.Count > 0)
                message = errors.First().errors.First().message;

            return Json(message);
        }

        private List<Error> SaveStandredGrid(HolidayVM grid1, IEnumerable<KeyValuePair<string, ModelState>> state, bool isStanderd)
        {
            List<Error> errors = new List<Error>();
            string objectName = isStanderd ? "StandardHolidays" : "CustomHolidays";
            var AllHolidays = _hrUnitOfWork.Repository<Holiday>().Where(h => h.Standard == isStanderd && (!h.IsLocal || h.IsLocal && h.CompanyId == CompanyId)).ToList();

            // Deleted
            if (grid1.deleted != null)
            {
                foreach (HolidayViewModel model in grid1.deleted)
                {
                    var holiday = AllHolidays.Where(h => h.Id == model.Id).FirstOrDefault();
                    _hrUnitOfWork.LeaveRepository.Remove(holiday);
                }
            }

            // Exclude delete models from sever side validations
            if (ServerValidationEnabled)
            {
                var modified = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted")));
                if (modified.Count > 0)
                {
                    errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = objectName,
                        ParentColumn = "CompanyId",
                        Columns = Models.Utils.GetModifiedRows(state.Where(a => !a.Key.Contains("deleted"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0) return errors;
                }
            }

            // updated records
            if (grid1.updated != null)
            {
                foreach (HolidayViewModel model in grid1.updated)
                {
                    var holiday = AllHolidays.Where(h => h.Id == model.Id).FirstOrDefault();
                    AutoMapper(new Models.AutoMapperParm { ObjectName = objectName, Destination = holiday, Source = model, Transtype = TransType.Update });
                    holiday.CompanyId = model.IsLocal ? (model.CompanyId == null ? CompanyId : model.CompanyId) : (int?)null;
                    holiday.ModifiedTime = DateTime.Now;
                    holiday.ModifiedUser = UserName;
                    holiday.Standard = isStanderd;

                    _hrUnitOfWork.LeaveRepository.Attach(holiday);
                    _hrUnitOfWork.LeaveRepository.Entry(holiday).State = EntityState.Modified;
                }
            }

            // inserted records
            if (grid1.inserted != null)
            {
                foreach (HolidayViewModel model in grid1.inserted)
                {
                    var holiday = new Holiday();
                    AutoMapper(new Models.AutoMapperParm { ObjectName = objectName, Destination = holiday, Source = model, Transtype = TransType.Insert });
                    holiday.CreatedTime = DateTime.Now;
                    holiday.CreatedUser = UserName;
                    holiday.Standard = isStanderd;
                    holiday.CompanyId = model.IsLocal ? CompanyId : (int?)null;

                    _hrUnitOfWork.LeaveRepository.Add(holiday);
                }
            }

            return errors;
        }

        #endregion

        #region LeavePlan
        public ActionResult LeavePlanIndex()
        {
            ViewBag.Calender = _hrUnitOfWork.LeaveRepository.GetHolidays(CompanyId);
            return View();
        }

        public ActionResult GetDeptLvPlanv(int DeptId)
        {
            var leavePlan = _hrUnitOfWork.CompanyStructureRepository.GetDeptLeavePlan(CompanyId, DeptId, Language);
            return Json(leavePlan, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadDeptJobLvPlan(int DeptId, DateTime? FromDate, DateTime? ToDate)
        {
            var jobPlan = _hrUnitOfWork.CompanyStructureRepository.GetJobLeavePlan(CompanyId, DeptId, FromDate, ToDate, Language);
            return Json(jobPlan, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeDept(int DeptId, int Week)
        {
            JobPercentChartVM jobPercentChartVM = _hrUnitOfWork.CompanyStructureRepository.GetJobsPercentage(DeptId, Week, CompanyId, Language);
            return Json(jobPercentChartVM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckPrevLeaves(IEnumerable<DeptLeavePlanViewModel> models)
        {
            int Requestcount = _hrUnitOfWork.CompanyStructureRepository.CheckLeaveRequests(CompanyId, models);
            if (Requestcount > 0)
                return Json(MsgUtils.Instance.Trls("Find Request"));

            return Json("OK");
        }

        [HttpPost]
        public ActionResult SaveLeavePlan(IEnumerable<DeptLeavePlanViewModel> models) 
        {
            var datasource = new DataSource<DeptLeavePlanViewModel>();
            datasource.Data = models;
            var result = new List<DeptJobLeavePlan>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "LeavePlanDept",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        if (datasource.Errors.Count() > 0)
                            return Json(datasource.Errors.FirstOrDefault().errors.FirstOrDefault().message);
                    }
                }

                if (models != null)
                {
                    var deptId = models.FirstOrDefault().DeptId;
                    var oldDeptPlans = _hrUnitOfWork.Repository<DeptJobLeavePlan>().Where(p => p.CompanyId == CompanyId && p.ToDate >= DateTime.Today.Date && p.DeptId == deptId).ToList();

                    foreach (DeptLeavePlanViewModel model in models)
                    {
                        if (model.Id == 0) //Add
                        {
                            DeptJobLeavePlan record = new DeptJobLeavePlan();
                            AutoMapper(new AutoMapperParm() { ObjectName = "LeavePlanDept", Destination = record, Source = model, Transtype = TransType.Insert });
                            record.CompanyId = CompanyId;
                            record.CreatedUser = UserName;
                            record.CreatedTime = DateTime.Now;
                            _hrUnitOfWork.CompanyStructureRepository.Add(record);
                        }
                        else //Update
                        {
                            DeptJobLeavePlan updatedRec = oldDeptPlans.FirstOrDefault(p => p.Id == model.Id);
                            if (updatedRec == null) //Add
                            {
                                updatedRec = new DeptJobLeavePlan();
                                AutoMapper(new AutoMapperParm() { ObjectName = "LeavePlanDept", Destination = updatedRec, Source = model,  Transtype = TransType.Insert });
                                updatedRec.CompanyId = CompanyId;
                                updatedRec.CreatedUser = UserName;
                                updatedRec.CreatedTime = DateTime.Now;
                                _hrUnitOfWork.CompanyStructureRepository.Add(updatedRec);
                            }
                            else //update
                            {
                                AutoMapper(new AutoMapperParm() { ObjectName = "LeavePlanDept", Destination = updatedRec, Source = model,  Transtype = TransType.Update });
                                updatedRec.ModifiedUser = UserName;
                                updatedRec.ModifiedTime = DateTime.Now;
                                _hrUnitOfWork.CompanyStructureRepository.Attach(updatedRec);
                                _hrUnitOfWork.CompanyStructureRepository.Entry(updatedRec).State = EntityState.Modified;
                            }
                        }
                        
                    }
                    datasource.Errors = SaveChanges(Language);

                }
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            var message = "OK";
            if (datasource.Errors.Count() > 0)
                return Json(datasource.Errors.FirstOrDefault().errors.FirstOrDefault().message);
            
            else
                return Json(message);
        }
        
        public ActionResult DeleteDeptLvPlan(int Id)
        {
            var datasource = new DataSource<DeptLeavePlanViewModel>();
            var plan = _hrUnitOfWork.Repository<DeptJobLeavePlan>().FirstOrDefault(p => p.Id == Id);

            IEnumerable<DeptJobLeavePlan> deptJobs = _hrUnitOfWork.Repository<DeptJobLeavePlan>().Where(p => p.DeptId == plan.DeptId && p.FromDate == plan.FromDate && p.ToDate == plan.ToDate).ToList();

            foreach (var dept in deptJobs)
            {
                AutoMapper(new Models.AutoMapperParm { Source = dept, ObjectName = "LeavePlanDept",  Transtype = TransType.Delete });

                if (dept != null) _hrUnitOfWork.CompanyStructureRepository.Remove(dept);
            }

            datasource.Errors = SaveChanges(Language);
           // datasource.Total = 1;

            if (datasource.Errors.Count > 0)
                return Json(datasource);
            else
                return Json("OK");
        }

       #endregion

        #region TermDuration
        public ActionResult CreateTermDur(IEnumerable<TermDurationViewModel> models)
        {
            var result = new List<TermDuration>();
            var datasource = new DataSource<TermDurationViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "TermDurations",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }


                foreach (TermDurationViewModel model in models)
                {

                    var record = new TermDuration
                    {
                        CompanyId = model.CompanyId,
                        Percent1 = model.Percent1,
                        Percent2 = model.Percent2,
                        WorkDuration = model.WorkDuration,
                        TermSysCode = model.TermSysCode,
                        FirstPeriod = model.FirstPeriod,

                    };
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "TermDurations",
                        Transtype = TransType.Insert
                    });
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    result.Add(record);
                    _hrUnitOfWork.TerminationRepository.Add(record);
                }

                datasource.Errors = SaveChanges(Language);
            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }


            datasource.Data = (from m in models
                               join r in result on m.CompanyId equals r.CompanyId into g
                               from r in g.DefaultIfEmpty()
                               select new TermDurationViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   CompanyId = m.CompanyId,
                                   Percent1 = m.Percent1,
                                   Percent2 = m.Percent2,
                                   WorkDuration = m.WorkDuration,
                                   TermSysCode = m.TermSysCode,
                                   FirstPeriod = m.FirstPeriod,

                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult ReadTermDur(int TermSysCode, int CompanyId)
        {
            return Json(_hrUnitOfWork.TerminationRepository.ReadTermDur(CompanyId, TermSysCode), JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateTermDur(IEnumerable<TermDurationViewModel> models)
        {
            var datasource = new DataSource<TermDurationViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "TermDurations",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                
                foreach (TermDurationViewModel model in models)
                {
                    var record = HrUnitOfWork.Repository<TermDuration>().Where(a => a.Id == model.Id).FirstOrDefault();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Job",
                        Transtype = TransType.Update
                    });
                    record.ModifiedUser = UserName;
                    record.ModifiedTime = DateTime.Now;
                    _hrUnitOfWork.TerminationRepository.Attach(record);
                    _hrUnitOfWork.TerminationRepository.Entry(record).State = EntityState.Modified;
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
        public ActionResult DeleteTermDur(IEnumerable<TermDurationViewModel> models)
        {
            var datasource = new DataSource<TermDurationViewModel>();


            foreach (TermDurationViewModel model in models)
            {
                var record = new TermDuration
                {
                    Id = model.Id
                };
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = record,
                    ObjectName = "TermDurations",
                    Transtype = TransType.Delete
                });

                _hrUnitOfWork.TerminationRepository.Remove(record);
            }

            datasource.Errors = SaveChanges(Language);
            datasource.Total = models.Count();


            datasource.Data = models;

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult ContractFiles()
        {
            var result = new List<ChartViewModel>();
            string pathName = Server.MapPath(@"/SpecialData");
            string ListPath = Server.MapPath("/App_Data/ContractsList.mdb");
            if (User.Identity.RTL())
                ListPath = Server.MapPath("/App_Data/arContractsList.mdb");
            if (Directory.Exists(pathName))
            {
                var Files = Directory.GetFiles(pathName);
                foreach (var item in Files)
                {
                    FileInfo fl = new FileInfo(item);
                    using (WordprocessingDocument docPKG = WordprocessingDocument.Open(item, true))
                    {
                        MailMerge mymerge = docPKG.MainDocumentPart.DocumentSettingsPart.Settings.Elements<MailMerge>().FirstOrDefault();
                        if (mymerge != null)
                        {
                            mymerge.ConnectString = new ConnectString() { Val = "Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source=" + ListPath + ";Mode=Read;Extended Properties=\"\";Jet OLEDB:System database=\"\";Jet OLEDB:Registry Path=\"\";Jet OLEDB:Engine Type=6;Jet OLEDB:Database Locking Mode=0;Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Global Bulk Transactions=1;Jet OLEDB:New Database Password=\"\";Jet OLEDB:Create System Database=False;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:SFP=False;Jet OLEDB:Support Complex Data=False;Jet OLEDB:Bypass UserInfo Validation=False;Jet OLEDB:Limited DB Caching=False;Jet OLEDB:Bypass ChoiceField Validation=False" };

                            var Relation = docPKG.MainDocumentPart.DocumentSettingsPart.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/mailMergeSource", new Uri(ListPath));
                            mymerge.DataSourceReference = new DataSourceReference() { Id = Relation.Id };
                        }
                        docPKG.Save();
                        docPKG.Dispose();
                    }
                    var f = new ChartViewModel()
                    {
                        category = fl.Name,
                        color = "/SpecialData/" + fl.Name
                    };
                    result.Add(f);
                }

            }
            return PartialView("_ContractFiles", result);
        }
        public ActionResult Upcoming()
        {
            var holidays = _hrUnitOfWork.LeaveRepository.GetUpcomingHolidays(CompanyId);
            var leaves = _hrUnitOfWork.LeaveRepository.GetUpcomingLeaves(CompanyId, Language);

            return View(new { holidays, leaves });
        }
        #endregion

        #region Payroll Setup
        public ActionResult PayrollDetails()
        {
            var today = DateTime.Today;
            ViewBag.Account = _hrUnitOfWork.Repository<Account>().Select(a => new { id = a.Id, name = a.Name, isActive = (a.StartDate <= today && (a.EndDate == null || a.EndDate >= today)) }).ToList();
            ViewBag.SalaryItem = _hrUnitOfWork.Repository<SalaryItem>().Select(a => new { id = a.Id, name = a.Name, isActive = (a.StartDate <= today && (a.EndDate == null || a.EndDate >= today)) }).ToList();
            var oldPayroll = _hrUnitOfWork.Repository<PayrollSetup>().FirstOrDefault(p => p.Id == CompanyId);
            if (oldPayroll == null)
            {
                return View(new PayrollSetup());
            }
            else
            {
                return oldPayroll == null ? (ActionResult)HttpNotFound() : View(oldPayroll);
            }
        }
        public ActionResult CreatePayroll(Model.ViewModel.Payroll.PayrollFormSetupViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            var datasource = new DataSource<PayrollFormSetupViewModel>();
            if (ModelState.IsValid)
            {
                var record = _hrUnitOfWork.Repository<PayrollSetup>().FirstOrDefault(p => p.Id == CompanyId);
                if (model.submit == false)
                {
                    if (record == null)
                    {
                        record = new PayrollSetup();
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = record,
                            Source = model,
                            ObjectName = "PayrollSetup",
                            Options = moreInfo,
                            Id = "CompanyId",
                            Transtype=TransType.Insert
                        });
                        record.Id = CompanyId;
                        ChkGradeName(model, record);
                        _hrUnitOfWork.PayrollRepository.Add(record);
                    }
                    else
                    {
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = record,
                            Source = model,
                            ObjectName = "PayrollSetup",
                            Options = moreInfo,
                            Id = "CompanyId",
                            Transtype = TransType.Update
                        });
                        record.ModifiedTime = DateTime.Now;
                        record.ModifiedUser = UserName;
                        ChkGradeName(model, record);
                        _hrUnitOfWork.PayrollRepository.Attach(record);
                        _hrUnitOfWork.PayrollRepository.Entry(record).State = EntityState.Modified;
                    }
                }
                else
                {
                    var query = _hrUnitOfWork.Repository<PayrollGrade>().Where(a => a.CompanyId == CompanyId).Select(a => a.Name).ToList();
                    var Code = _hrUnitOfWork.Repository<PayrollGrade>().DefaultIfEmpty().Max(a => a == null ? 0 : a.Code);
                    string[] GroupValues = model.Group != null ? model.Group.Split(',') : new string[0];
                    string[] GradeValues = model.Grade != null ? model.Grade.Split(',') : new string[0];
                    string[] SubGradesValues = model.SubGrade != null ? model.SubGrade.Split(',') : new string[0];
                    if (GroupValues.Count() > 0 && GradeValues.Count() > 0 && SubGradesValues.Count() > 0)
                    {
                        GenerateGrades(query, Code, GroupValues, GradeValues, SubGradesValues);
                    }
                    else if (GroupValues.Count() <= 0 && GradeValues.Count() > 0 && SubGradesValues.Count() > 0)
                    {
                        GenerateGrades(query, Code, GradeValues, SubGradesValues);
                    }
                    else if (GroupValues.Count() > 0 && GradeValues.Count() > 0 && SubGradesValues.Count() <= 0)
                    {
                        GenerateGrade(query, Code, GroupValues, GradeValues);
                    }
                    else
                    {
                        ModelState.AddModelError("", MsgUtils.Instance.Trls("InsertGrades"));
                        return Json(Models.Utils.ParseFormErrors(ModelState));
                    }

                }
              
                var Errors = SaveChanges(Language);
                var message = "OK,";
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;

                return Json(message);
            }
            return Json(Utils.ParseFormErrors(ModelState));
        }
        public void GenerateGrades(List<string> query, int Code, string[] grades, string[] subgrades)
        {
            foreach (var Grade in grades)
            {
                foreach (var SubGrade in subgrades)
                {
                    var name = Grade + "." + SubGrade;
                    if (!query.Contains(name))
                    {
                        PayrollGrade grade = new PayrollGrade()
                        {
                            CompanyId = CompanyId,
                            CreatedUser = UserName,
                            CreatedTime = DateTime.Now,
                            Name = Grade + "." + SubGrade,
                            Grade = Grade,
                            SubGrade = SubGrade,
                            Code = ++Code
                        };
                        _hrUnitOfWork.PayrollRepository.Add(grade);
                    }
                }
            }
        }
        public void GenerateGrades(List<string> query, int Code, string[] groups, string[] grades, string[] subgrades)
        {
            foreach (var Group in groups)
            {
                foreach (var Grade in grades)
                {
                    foreach (var SubGrade in subgrades)
                    {
                        var name = Group + "." + Grade + "." + SubGrade;
                        if (!query.Contains(name))
                        {
                            PayrollGrade grade = new PayrollGrade()
                            {
                                CompanyId = CompanyId,
                                CreatedUser = UserName,
                                CreatedTime = DateTime.Now,
                                Name = Group + "." + Grade + "." + SubGrade,
                                Group = Group,
                                Grade = Grade,
                                SubGrade = SubGrade,
                                Code = ++Code
                            };
                            _hrUnitOfWork.PayrollRepository.Add(grade);

                        }
                    }

                }
            }
        }
        public void GenerateGrade(List<string> query, int Code, string[] groups, string[] grades)
        {
            foreach (var Group in groups)
            {
                foreach (var Grade in grades)
                {
                    var name = Group + "." + Grade;
                    if (!query.Contains(name))
                    {
                        PayrollGrade grade = new PayrollGrade()
                        {
                            CompanyId = CompanyId,
                            CreatedUser = UserName,
                            CreatedTime = DateTime.Now,
                            Name = Group + "." + Grade,
                            Group = Group,
                            Grade = Grade,
                            Code = ++Code
                        };
                        _hrUnitOfWork.PayrollRepository.Add(grade);
                    }
                }

            }
        }
        public void ChkGradeName(PayrollFormSetupViewModel model , PayrollSetup record)
        {
            if (model.GradeName == 1)
            {
                record.Spiltter = null;
                record.Grade = null;
                record.Group = null;
                record.SubGrade = null;
            }
            if (model.MultiCurr == false)
            {
                record.DiffDebitAcct = null;
                record.DiffCreditAcct = null;
            }
        }

        #endregion

        #region Account SetUp
        //AccountSetUp
        public ActionResult AccountSetUp()
        {
            ViewBag.Spiltter = _hrUnitOfWork.Repository<AccountSetup>().Where(a => a.AccType == 1).Select(a => a.Spiltter).FirstOrDefault();
            return View();
        }
        //ReadAccountSetUp
        public ActionResult ReadAccountSetUp(int AccTypeId)
        {
            var query = _hrUnitOfWork.PayrollRepository.readAccountSetup(AccTypeId, CompanyId);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSpiltter(int AcctypeId)
        {
            var spiltter = _hrUnitOfWork.Repository<AccountSetup>().Where(a => a.AccType == AcctypeId).Select(a => a.Spiltter).FirstOrDefault();
            return Json(spiltter, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CreateAccountSetUp(IEnumerable<AccountSetupViewModel> models, byte AccTypeId , string Spiltter)
        {
            var result = new List<AccountSetup>();
            var datasource = new DataSource<AccountSetupViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            var company = CompanyId;
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.MenuRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "AccountSetup",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        ParentColumn = "CompanyId",
                        Culture = Language,
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                foreach (AccountSetupViewModel model in models)
                {

                    var account = new AccountSetup();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = account,
                        Source = model,
                        ObjectName = "AccountSetup",
                        Transtype = TransType.Insert
                    });


                    result.Add(account);
                    _hrUnitOfWork.PayrollRepository.Add(account);
                }
                var listOfAccounts = _hrUnitOfWork.Repository<AccountSetup>().Where(a => a.AccType == AccTypeId).ToList();
                if(listOfAccounts.Count > 0)
                {
                    foreach (var record in listOfAccounts)
                    {
                       if(record.Spiltter != Spiltter)
                        {
                            record.Spiltter = Spiltter;                           
                            _hrUnitOfWork.PayrollRepository.Attach(record);
                            _hrUnitOfWork.PayrollRepository.Entry(record).State = EntityState.Modified;
                        }
                    }
                               
                }
                datasource.Errors = SaveChanges(Language);

            }
            else
            {
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);
            }

            datasource.Data = (from m in models
                               join r in result on m.Id equals r.Id
                               select new AccountSetupViewModel
                               {
                                  Id = r.Id,
                                  CompanyId = m.CompanyId,
                                  Seq =m.Seq,
                                  Segment = m.Segment,
                                  SegLength = m.SegLength                                 
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateAccountSetUp(IEnumerable<AccountSetupViewModel> models, byte AccTypeId, string Spiltter, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<AccountSetupViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            var company = CompanyId;
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.PageEditorRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "AccountSetup",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        ParentColumn = "CompanyId",
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Id);
                var db_accountSetUp = _hrUnitOfWork.Repository<AccountSetup>().Where(a => ids.Contains(a.Id)).ToList();
                for (var i = 0; i < models.Count(); i++)
                {
                    var model = models.ElementAtOrDefault(i);
                    var accountSetup = db_accountSetUp.FirstOrDefault(a => a.Id == model.Id);
                    AutoMapper(new AutoMapperParm() { ObjectName = "AccountSetup", Destination = accountSetup, Source = model, Version = 0, Options = options.ElementAtOrDefault(i) ,Transtype=TransType.Update});
                    _hrUnitOfWork.PayrollRepository.Attach(accountSetup);
                    _hrUnitOfWork.PayrollRepository.Entry(accountSetup).State = EntityState.Modified;
                }
                var listOfAccounts = _hrUnitOfWork.Repository<AccountSetup>().Where(a => a.AccType ==AccTypeId).ToList();
                if (listOfAccounts.Count > 0)
                {
                    foreach (var record in listOfAccounts)
                    {
                        if (record.Spiltter != Spiltter)
                        {
                            record.Spiltter = Spiltter;
                            _hrUnitOfWork.PayrollRepository.Attach(record);
                            _hrUnitOfWork.PayrollRepository.Entry(record).State = EntityState.Modified;
                        }
                    }

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
        public ActionResult DeleteAccountSetUp(IEnumerable<AccountSetupViewModel> models)
        {
            var datasource = new DataSource<AccountSetupViewModel>();
            foreach (AccountSetupViewModel model in models)
            {
                var record = new AccountSetup
                {
                    Id = model.Id
                };
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = record,
                    ObjectName = "AccountSetup",
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.PayrollRepository.Remove(record);
            }

            datasource.Errors = SaveChanges(Language);
            datasource.Total = models.Count();
            datasource.Data = models;

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        #endregion

        #region PurgeData
        [Authorize(Roles = "Admin")]
        public ActionResult Purge()
        {          
            return View();
        }
        
        [HttpPost]
        public ActionResult DropTable(string table, bool hasCompany, bool hasEmployee)
        {
            string sql = "DELETE FROM " + table;
            if (hasCompany)
                sql += " WHERE CompanyId = " + CompanyId;
            else if (hasEmployee)
            {
                string empId = "EmpId";
                if (table == "MsgEmployees") empId = "FromEmpId";
                sql += " WHERE " + empId + " IN (SELECT DISTINCT A.EmpId FROM Assignments A WHERE A.CompanyId = " + CompanyId + ")";
            }
            else
            {
                string result;
                if (table == "BenefitServs") _hrUnitOfWork.CompanyRepository.ExecuteSql("delete from BenefitServs where ParentId Is not NULL");
                if (table == "LeaveTrans")
                {
                    var sqls = new string[]
                    {
                        "delete from LeaveTrans WHERE CompanyId = " + CompanyId,
                        "delete from LeaveAdjusts WHERE EmpId IN (SELECT DISTINCT A.EmpId FROM Assignments A WHERE A.CompanyId = " + CompanyId + ")",
                        "delete from LeavePostings WHERE EmpId IN (SELECT DISTINCT A.EmpId FROM Assignments A WHERE A.CompanyId = " + CompanyId + ")",
                        "delete from LeaveRequests WHERE EmpId IN (SELECT DISTINCT A.EmpId FROM Assignments A WHERE A.CompanyId = " + CompanyId + ")"
                    };

                    return Json(_hrUnitOfWork.CompanyRepository.ExecuteSqlTrans(sqls));
                }
                if (table == "DeptBudgets")
                {
                    result = _hrUnitOfWork.CompanyRepository.ExecuteSql("delete from DeptBudgets WHERE DeptId IN (SELECT CS.Id FROM CompanyStructures CS WHERE CS.CompanyId = " + CompanyId + ")");
                    return Json(result);
                }
                if (table == "People")
                {
                    result = _hrUnitOfWork.CompanyRepository.ExecuteSql("delete from People");
                    return Json(result);
                }
                if (table == "AspNetRoles")
                {
                    result = _hrUnitOfWork.CompanyRepository.ExecuteSql("delete from AspNetRoles where Name not in ('Employee','Users','Developer','Manager','Admin','Administrators','Configuration')");
                    return Json(result);
                }
                if (table == "AspNetUsers")
                {
                    result = _hrUnitOfWork.CompanyRepository.ExecuteSql("delete from AspNetUsers where UserName not in('Admin')");
                    return Json(result);
                }
                if (table == "Companies")
                {
                    var sqls = new string[]
                    {
                        "Delete from RoleMenuFunctions where RoleMenuFunctions.RoleMenu_MenuId in (select MenuId from RoleMenus where MenuId in (select  Id as ID from Menus Where Menus.CompanyId = " + CompanyId + "))",
                        "Delete from PageDivs where MenuId in (select Id from Menus Where Menus.CompanyId = " + CompanyId + ")",
                        "Delete From Menus where CompanyId = " + CompanyId,
                        "Delete from RoleMenus where MenuId in (select Id from Menus Where Menus.CompanyId = " + CompanyId + ")",
                        "Delete from PersonSetup where Id = " + CompanyId,
                        "Delete from PayrollSetup where Id = " + CompanyId,
                        "Delete From Companies where Id = " + CompanyId
                    };

                    return Json(_hrUnitOfWork.CompanyRepository.ExecuteSqlTrans(sqls));
                }
            }

            return Json(_hrUnitOfWork.CompanyRepository.ExecuteSql(sql));
        }

        //string FriendlyMessage(string msg)
        //{
        //    int i;
        //    if (int.TryParse(msg, out i))
        //        return msg;
        //    else
        //        return _hrUnitOfWork.HandleDbExceptions()
        //}

        #endregion

        #region RepeatInput
        //RepeatInputIndex
        public ActionResult RepeatInputIndex(string Depts , int year)
        {
            ViewBag.Depts = Depts;
            ViewBag.Year = year;
            return View();
        }
        //DaysTab
        public ActionResult DaysTab(string Depts,int Year, byte Version = 0)
        { 
            return View(new DeptPlanFormDaysViewModel());
        }

        #endregion




    }
}