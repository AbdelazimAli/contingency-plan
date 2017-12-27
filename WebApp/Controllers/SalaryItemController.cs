using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Payroll;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using Model.Domain.Payroll;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class SalaryItemController : BaseController
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
        public SalaryItemController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;

        }
        #region SalaryItems
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetSalaryItem(int MenuId)
        {
            var query = _hrUnitOfWork.PayrollRepository.GetSalaryItems(CompanyId);
            string whereclause = GetWhereClause(MenuId);
            if (whereclause.Length > 0)
            {
                try
                {
                    query = query.Where(whereclause);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                    Models.Utils.LogError(ex.Message);
                    return Json("", JsonRequestBehavior.AllowGet);

                }
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id = 0)
        {
            ViewBag.Account = _hrUnitOfWork.Repository<Account>().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.TstFormula=_hrUnitOfWork.Repository<Formula>().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.Curr = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();


            if (id == 0)
                return View(new SalaryItemFormViewModel());

            var record = _hrUnitOfWork.PayrollRepository.GetSalaryItem(id);
            return record == null ? (ActionResult)HttpNotFound() : View(record);
        }
        public ActionResult Details(SalaryItemFormViewModel model, OptionsViewModel moreInfo)
        {
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LocationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "SalaryItems",
                        TableName = "SalaryItems",
                        ParentColumn = "CompanyId",
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

                SalaryItem record;

                //insert
                if (model.Id == 0)
                {
                    record = new SalaryItem();
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "SalaryItems",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });
                    record.CreatedUser = UserName;
                    record.CreatedTime = DateTime.Now;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    Check(record, model);
                    _hrUnitOfWork.PayrollRepository.Add(record);
                }
                //update
                else
                {
                    record = _hrUnitOfWork.Repository<SalaryItem>().FirstOrDefault(a => a.Id == model.Id);
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "SalaryItems",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    Check(record, model);
                    _hrUnitOfWork.PayrollRepository.Attach(record);
                    _hrUnitOfWork.PayrollRepository.Entry(record).State = EntityState.Modified;
                }

                if (errors.Count > 0) return Json(errors.First().errors.First().message);

                errors = SaveChanges(Language);

                var message = "OK";
                if (errors.Count > 0) message = errors.First().errors.First().message;

                return Json(message);
            }

            return Json(Models.Utils.ParseFormErrors(ModelState));

        }
        public void Check(SalaryItem record, SalaryItemFormViewModel model)
        {
            if (model.ItemType != 2)
                record.Multiple = false;
            if (model.ValueType == 1)
            {
                record.MinValue = null;
                record.MaxValue = null;
                record.InValidValue = null;
            }
            else if (model.ValueType == 2)
            {
                record.MinValue = null;
                record.MaxValue = null;
                record.InValidValue = null;
                record.FormulaId = null;
            }
            if (model.UoMeasure != 1 && model.IsSalaryItem == false)
                record.InputCurr = null;
            if (model.IsSalaryItem == true)
                record.UoMeasure = 1;
            if (model.IsSalaryItem == false)
                record.ItemType = 2;
        }
        public ActionResult Delete(int id)
        {
            var message = "OK";
            DataSource<SalaryItemViewModel> Source = new DataSource<SalaryItemViewModel>();
            SalaryItem record = _hrUnitOfWork.PayrollRepository.GetSalary(id);
            if (record != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = record,
                    ObjectName = "SalaryItem",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });

                _hrUnitOfWork.PayrollRepository.Remove(record);
            }
            Source.Errors = SaveChanges(Language);
            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }
        #endregion

        #region Salary Variables
        public ActionResult SalaryVarIndex()
        {
            ViewBag.PayrollId = _hrUnitOfWork.Repository<Payrolls>().Where(s=>s.CompanyId==CompanyId || s.IsLocal==false).Select(a => new { value = a.Id, text = a.Name }).ToList();
            ViewBag.SalItemId = _hrUnitOfWork.Repository<SalaryItem>().Where(s => s.CompanyId == CompanyId || s.IsLocal == false).Select(a => new { value = a.Id, text = a.Name }).ToList();
            ViewBag.PayPeriodId = _hrUnitOfWork.Repository<SubPeriod>().Select(a => new { value = a.Id, text = a.Name }).ToList();
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult GetSalaryVar()
        {
            var query = _hrUnitOfWork.PayrollRepository.GetSalaryVar();                      
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DetailsOfSalary(string id )
        {
            ViewBag.Curr = _hrUnitOfWork.LookUpRepository.GetCurrencyCode();
            ViewBag.PayPeriodId = _hrUnitOfWork.Repository<SubPeriod>().Where(s=>s.Status == 1).Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.PayrollId = _hrUnitOfWork.Repository<Payrolls>().Where(s => s.CompanyId == CompanyId || s.IsLocal == false).Select(a => new { id = a.Id, name = a.Name }).ToList();
            ViewBag.SalItemId = _hrUnitOfWork.Repository<SalaryItem>().Where(s => (s.CompanyId == CompanyId || s.IsLocal == false) && s.ItemType == 2).Select(a => new { id = a.Id, name = a.Name ,UMeasure = a.UoMeasure}).ToList();
            ViewBag.EmpSalary = _hrUnitOfWork.PeopleRepository.GetActiveEmployees(CompanyId,Language).Select(a => new { value = a.id, text = a.name }).Distinct().ToList();
            Guid newGuid;
            if (id == null) newGuid = new Guid();
            else newGuid = Guid.Parse(id);
            if (id== null)
                return View(new SalaryVarFormViewModel());
            var record = _hrUnitOfWork.PayrollRepository.GetSalaryVar(newGuid);
            return record == null ? (ActionResult)HttpNotFound() : View(record);           
        }
        //GetSubPeriods
        public ActionResult GetSubPeriods(int payrollId)
        {
            var ListOfSubPeriods = _hrUnitOfWork.PayrollRepository.GetSubPeriods(payrollId);
            return Json(ListOfSubPeriods, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult ReadEmpSalaryVar(Guid reference )
        {
            var ListOfEmpSalary = _hrUnitOfWork.PayrollRepository.ReadEmpSalaryVar(reference);
            return Json(ListOfEmpSalary, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ChecksalaryDocs(HttpPostedFileBase File)
        {
            List<Error> source = new List<Error>();
            string[] ExcelExt = { "xlsx", "xls" };
            if (!Array.Exists(ExcelExt, f => f == File.FileName.Split('.')[1]))
            {
                source.Add(new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("NotExcelFile") } } });
                return Json(source);
            }
            string Path = Server.MapPath("/SpecialData/TempFolder");

            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            FileStream stream = null;
            foreach (var Dirfile in Directory.GetFiles(Path, "*.xlsx"))
            {
                FileInfo f = new FileInfo(Dirfile);
                try
                {
                    stream = f.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IOException)
                {
                    source.Add(new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("FileinUse") } } });
                }
                finally
                {
                    if (stream != null)
                        stream.Close();

                    System.IO.File.SetAttributes(Dirfile, FileAttributes.Normal);
                }
                if (source.Count > 0)
                    return Json(source);
                else
                    System.IO.File.Delete(Dirfile);
            }

            File.SaveAs(Server.MapPath("/SpecialData/TempFolder/SalariesData.xlsx"));
            return Json(source);
        }
        public ActionResult ImportSalariesGrid()
        {
            DataSource<SalaryEmpVarViewModel> source = new DataSource<SalaryEmpVarViewModel>();
            source.Data = GetsalaryData();
            return Json(source,JsonRequestBehavior.AllowGet);
        }
        private List<SalaryEmpVarViewModel> GetsalaryData()
        {
            var path = Server.MapPath("/SpecialData/TempFolder/SalariesData.xlsx");
            List<SalaryEmpVarViewModel> MySalries = new List<SalaryEmpVarViewModel>();
            FileInfo inf = new FileInfo(path);
            ExcelPackage pak = new ExcelPackage(inf);
            ExcelWorksheet workSheet = pak.Workbook.Worksheets.FirstOrDefault();
            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {

                var workShee = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column].Count();
                if (workShee != 0)
                {
                    MySalries.Add(new SalaryEmpVarViewModel
                    {
                        EmpId = GetEmpId(workSheet.Cells[rowNumber, 1].Value),
                        Amount = (workSheet.Cells[rowNumber, 2].Value != null ? decimal.Parse(workSheet.Cells[rowNumber, 2].Value.ToString()) : 0),
                        Status = getStatus(workSheet.Cells[rowNumber, 3].Value),
                    });
               }
            }
            return MySalries;
        }
        private int GetEmpId(object val)
        {
            return _hrUnitOfWork.Repository<Person>().Where(a => (string.Format("{0} {1} {2} {3}",a.FirstName,a.Fathername,a.GFathername,a.Familyname) == val.ToString())).Select(a => a.Id).FirstOrDefault();
        }
        private byte getStatus(object val)
        {
            switch (val.ToString())
            {
                case "New":
                    return 0;
                case "Deleted":
                    return 2;
                default:
                    return 1;
            }
        
        }
        [HttpPost]
        public ActionResult SaveSalaryVar(SalaryVarFormViewModel model, OptionsViewModel moreInfo, SalaryEmpVarGridViewModel grid1)
        {
            List<Error> errors = new List<Error>();
            var SalaryVarRecord = _hrUnitOfWork.PayrollRepository.GetSalVar(model.Reference);
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.PositionRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "SalaryVar",
                        TableName = "SalaryVars",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
                        ParentColumn = "PayPeriodId",
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
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
            var multi = _hrUnitOfWork.Repository<SalaryItem>().Where(a => a.Id == model.SalItemId).Select(a => a.Multiple).FirstOrDefault();
            List<SalaryVar> ListOfSalaryVar = new List<SalaryVar>();
            if (SalaryVarRecord == null) // New
            {

                if (grid1.inserted != null)
                {
                    Guid newGUID = Guid.NewGuid();
                    foreach (var item in grid1.inserted)
                    {
                        SalaryVarRecord = new SalaryVar();
                        SalaryVarRecord.Approvedby = UserName;
                        SalaryVarRecord.Curr = model.Curr;
                        SalaryVarRecord.PayPeriodId = model.PayPeriodId;
                        SalaryVarRecord.PayrollId = model.PayrollId;
                        SalaryVarRecord.SalItemId = model.SalItemId;
                        SalaryVarRecord.CreatedTime = DateTime.Now;
                        SalaryVarRecord.CreatedUser = UserName;
                        SalaryVarRecord.Reference = newGUID;
                        SalaryVarRecord.EmpId = item.EmpId;
                        SalaryVarRecord.Amount = item.Amount;
                        SalaryVarRecord.Status = item.Status;
                        ListOfSalaryVar.Add(SalaryVarRecord);
                    }
                    _hrUnitOfWork.PayrollRepository.AddRange(ListOfSalaryVar);

                    if (multi == false)
                    {
                        var empIds = ListOfSalaryVar.Select(a => a.EmpId).ToList();
                        if (empIds.Count != empIds.Distinct().Count())
                        {
                            ModelState.AddModelError("", MsgUtils.Instance.Trls("CantAcceptMultiInPeriod"));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }
                }
                else
                {
                    SalaryVarRecord = new SalaryVar();
                  
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = SalaryVarRecord,
                        Source = model,
                        ObjectName = "SalaryVar",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo
                    });
                    _hrUnitOfWork.PayrollRepository.Add(SalaryVarRecord);
                }
            }
            else // Edit
            {
                //if form is updated only
                if (grid1.inserted == null && grid1.updated == null)
                {
                    var listOfSalaryVar = _hrUnitOfWork.Repository<SalaryVar>().Where(a => a.Reference == model.Reference).ToList();
                    foreach (var salary in listOfSalaryVar)
                    {
                        AutoMapper(new Models.AutoMapperParm
                        {
                            Destination = salary,
                            Source = model,
                            ObjectName = "SalaryVar",
                            Version = Convert.ToByte(Request.Form["Version"]),
                            Options = moreInfo,
                            Transtype = TransType.Update
                        });
                        SalaryVarRecord.ModifiedTime = DateTime.Now;
                        SalaryVarRecord.ModifiedUser = UserName;
                        _hrUnitOfWork.PayrollRepository.Attach(SalaryVarRecord);
                        _hrUnitOfWork.PayrollRepository.Entry(SalaryVarRecord).State = EntityState.Modified;
                    }
                }
                // if insert in grid 
                if (grid1.inserted != null)
                {
                    foreach (var item in grid1.inserted)
                    {
                        SalaryVarRecord = new SalaryVar();
                        SalaryVarRecord.Approvedby = UserName;
                        SalaryVarRecord.Curr = model.Curr;
                        SalaryVarRecord.PayPeriodId = model.PayPeriodId;
                        SalaryVarRecord.PayrollId = model.PayrollId;
                        SalaryVarRecord.SalItemId = model.SalItemId;
                        SalaryVarRecord.CreatedTime = DateTime.Now;
                        SalaryVarRecord.CreatedUser = UserName;
                        SalaryVarRecord.Reference = model.Reference;
                        SalaryVarRecord.EmpId = item.EmpId;
                        SalaryVarRecord.Amount = item.Amount;
                        SalaryVarRecord.Status = item.Status;
                        ListOfSalaryVar.Add(SalaryVarRecord);
                    }
                    _hrUnitOfWork.PayrollRepository.AddRange(ListOfSalaryVar);
                    if (multi == false)
                    {
                        var dbEmpIds = _hrUnitOfWork.Repository<SalaryVar>().Where(a => a.Reference == model.Reference).Select(s => s.EmpId).ToList();
                        var empIds = ListOfSalaryVar.Select(a => a.EmpId).ToList();
                        List<int> chkList = new List<int>();
                        chkList.AddRange(dbEmpIds);
                        chkList.AddRange(empIds);
                        if (chkList.Count != chkList.Distinct().Count())
                        {
                            ModelState.AddModelError("", MsgUtils.Instance.Trls("CantAcceptMultiInPeriod"));
                            return Json(Models.Utils.ParseFormErrors(ModelState));
                        }
                    }
                }
                //if update in grid and form
                if (grid1.updated != null)
                {
                    foreach (var item in grid1.updated)
                    {
                        SalaryVarRecord = _hrUnitOfWork.Repository<SalaryVar>().Where(a => a.Id == item.Id).FirstOrDefault();
                        SalaryVarRecord.Approvedby = UserName;
                        SalaryVarRecord.Curr = model.Curr;
                        SalaryVarRecord.PayPeriodId = model.PayPeriodId;
                        SalaryVarRecord.PayrollId = model.PayrollId;
                        SalaryVarRecord.SalItemId = model.SalItemId;
                        SalaryVarRecord.ModifiedTime = DateTime.Now;
                        SalaryVarRecord.ModifiedUser = UserName;
                        SalaryVarRecord.Reference = model.Reference;
                        SalaryVarRecord.EmpId = item.EmpId;
                        SalaryVarRecord.Amount = item.Amount;
                        SalaryVarRecord.Status = item.Status;
                        _hrUnitOfWork.PayrollRepository.Attach(SalaryVarRecord);
                        _hrUnitOfWork.PayrollRepository.Entry(SalaryVarRecord).State = EntityState.Modified;
                    }

                }
                if (grid1.deleted != null)
                {
                    foreach (var item in grid1.deleted)
                    {
                        SalaryVarRecord = _hrUnitOfWork.Repository<SalaryVar>().Where(a => a.Id == item.Id).FirstOrDefault();
                        SalaryVarRecord.Status = 2;
                        _hrUnitOfWork.PayrollRepository.Attach(SalaryVarRecord);
                        _hrUnitOfWork.PayrollRepository.Entry(SalaryVarRecord).State = EntityState.Modified;

                    }
                }
                if(model.submit == true)
                {
                    var listOfSalary = _hrUnitOfWork.Repository<SalaryVar>().Where(a => a.Reference == model.Reference && a.Status == 0 ).ToList();
                    if(listOfSalary.Count() >0)
                    {
                        for( int i =0; i<listOfSalary.Count; i++)
                        {
                            listOfSalary[i].Status = 1;
                            _hrUnitOfWork.PayrollRepository.Attach(listOfSalary[i]);
                            _hrUnitOfWork.PayrollRepository.Entry(listOfSalary[i]).State = EntityState.Modified;
                        }
                    }

                }
            }
            if (errors.Count > 0) return Json(errors.First().errors.First().message);
            try
            {
                _hrUnitOfWork.Save();

            }
            catch (Exception ex)
            {
                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg);
            }

            return Json("OK,"+((new JavaScriptSerializer()).Serialize(SalaryVarRecord)));
        }

        //SaveCriteria
        public ActionResult SaveCriteria()
        {
            ViewBag.Jobs = _hrUnitOfWork.JobRepository.ReadJobs(CompanyId, Language,0).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.Locations = _hrUnitOfWork.LocationRepository.ReadLocations(Language, CompanyId).Select(a => new { id = a.Id, name = a.LocalName });
            ViewBag.CompanyStuctures = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId, null, Language);
            ViewBag.Payrolls = _hrUnitOfWork.Repository<Payrolls>().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.Positions = _hrUnitOfWork.PositionRepository.GetPositions(Language, CompanyId).Select(a => new { id = a.Id, name = a.Name });
            ViewBag.PeopleGroups = _hrUnitOfWork.PeopleRepository.GetPeoples().Select(a => new { id = a.Id, name = a.Name });
            ViewBag.PayrollGrades = _hrUnitOfWork.JobRepository.GetPayrollGrade();
            return View(new SalaryVarFormViewModel());
        }
        [HttpPost]
        public ActionResult AddToSalaryVarGrid(SalaryVarFormViewModel Model)
        {
            string wel = "CompanyId == " + CompanyId.ToString();
            var assign = _hrUnitOfWork.EmployeeRepository.GetAssignments(Language).Where(wel);
            List<int> EmpIds = new List<int>();
            //1-Add EmpId belongs to specified Jobs
            if(Model.IJobs != null ) 
                EmpIds.AddRange(assign.Where(a => Model.IJobs.Contains(a.JobId != null ? a.JobId.Value : 0)).Select(c => c.EmpId).ToList());
            //2-Add EmpId belongs to specified Postions
            if (Model.IPositions != null)            
                EmpIds.AddRange(assign.Where(a => Model.IPositions.Contains(a.PositionId != null ? a.PositionId.Value : 0)).Select(c => c.EmpId).ToList());
            //3-Add EmpId belongs to specified Company
            if (Model.ICompanyStuctures != null)
                EmpIds.AddRange(assign.Where(a => Model.ICompanyStuctures.Contains(a.CompanyId != null ? a.CompanyId.Value : 0)).Select(c => c.EmpId).ToList());
            //4-Add EmpId belongs to specified Employment
            if (Model.IEmployments != null)
                EmpIds.AddRange(assign.Where(a => Model.IEmployments.Contains(a.PersonType != null ? a.PersonType.Value : 0)).Select(c => c.EmpId).ToList());
            //5-Add EmpId belongs to specified Locations
            if (Model.ILocations != null)
                EmpIds.AddRange(assign.Where(a => Model.ILocations.Contains(a.LocationId != null ? a.LocationId.Value : 0)).Select(c => c.EmpId).ToList());
            //6-Add EmpId belongs to specified PayrollGrades
            if (Model.IPayrollGrades != null)
                EmpIds.AddRange(assign.Where(a => Model.IPayrollGrades.Contains(a.PayGradeId != null ? a.PayGradeId.Value : 0)).Select(c => c.EmpId).ToList());
            //7-Add EmpId belongs to specified PeopleGroups
            if (Model.IPeopleGroups != null)
                EmpIds.AddRange(assign.Where(a => Model.IPeopleGroups.Contains(a.GroupId != null ? a.GroupId.Value : 0)).Select(c => c.EmpId).ToList());
            //8-Add EmpId belongs to specified Payrolls
            if (Model.IPayrolls != null)
                EmpIds.AddRange(assign.Where(a => Model.IPayrolls.Contains(a.PayrollId != null ? a.PayrollId.Value : 0)).Select(c => c.EmpId).ToList());

            List<SalaryEmpVarViewModel> records = new List<SalaryEmpVarViewModel>();
            for (int i = 0; i < EmpIds.Count; i++)
            {
                records.Add(new SalaryEmpVarViewModel
                {
                    EmpId = EmpIds[i]
                });
            }
            return Json(records, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteSalaryVar(string id)
        {
            var message = "OK";
            DataSource<PayrollVarViewModel> Source = new DataSource<PayrollVarViewModel>();
            //  Guid guid = _hrUnitOfWork.Repository<SalaryVar>().Where(a => a.Id == id).Select(a => a.Reference).FirstOrDefault();
            Guid g = Guid.Parse(id);
            var ListOfSalaryVar = _hrUnitOfWork.Repository<SalaryVar>().Where(a => a.Reference == g).ToList();
            foreach (var item in ListOfSalaryVar)
            {
                _hrUnitOfWork.PayrollRepository.Remove(item);

            }
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }


        #endregion

    }
}