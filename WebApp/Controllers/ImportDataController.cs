using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Administration;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApp.Extensions;
using OfficeOpenXml;
using WebApp.Models;
using System.Web.Routing;
using Model.Domain.Payroll;
using OfficeOpenXml.DataValidation.Contracts;
using System.Reflection;
using System.Collections;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApp.Controllers
{
    public class ImportDataController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        UserContext db = new UserContext();
        private ApplicationUserManager _userManager;
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
        public ImportDataController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

        }

        #region Index View
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ImportEmployees()
        {
            ViewBag.ArrData = new List<FormList>() { new FormList() { text = MsgUtils.Instance.Trls("People"), name = "People" }, new FormList() { text = MsgUtils.Instance.Trls("Job"), name = "Job" }, new FormList() { text = MsgUtils.Instance.Trls("Locations"), name = "FormLocation" } };
            return View();
        }
        public ActionResult ImportDataView(ExcelFileImports File)
        {
            string Path = Server.MapPath("/SpecialData/CurrentData/" + File.ObjectName + ".xlsx");
            if (System.IO.File.Exists(Path))
               File.ExistFile = true;

            return View(File);
        }
        #endregion

        #region GlobalFunctions
        private IList ReturnListData(IEnumerable models, ExcelFileImports File, PeopleExportViewModel GridValues, List<FormColumnViewModel> GeneralCol, List<FormColumnViewModel> AssignCol, PropertyInfo[] ModelProps)
        {
            Assembly assenmbly = Assembly.Load("Model");
            var ViewModelType = assenmbly.ExportedTypes.FirstOrDefault(a => a.Name == "Excel" + File.OldObjectName + "ViewModel");
            var ListViewModelType = typeof(List<>).MakeGenericType(ViewModelType);
            IList data = (IList)Activator.CreateInstance(ListViewModelType);

            PropertyInfo ColumnProp;
            PropertyInfo ListColumnProp;
            FormColumnViewModel CoulmnType;
            string[] ArrayList = { "select", "radioset", "autocomplete" };
            IEnumerable<FormList> list;
            FormList ListRecord;
            object NullValue;
            foreach (var item in models)
            {
                var mymodel = Activator.CreateInstance(ViewModelType);

                for (int i = 0; i < ModelProps.Count(); i++)
                {
                    ColumnProp = mymodel.GetType().GetProperty(ModelProps[i].Name);
                    CoulmnType = GeneralCol.FirstOrDefault(a => a.name == ModelProps[i].Name);
                    if (ColumnProp != null)
                    {
                        if (CoulmnType == null)
                            if (File.OldObjectName == "People")
                                CoulmnType = AssignCol.FirstOrDefault(a => a.name == ModelProps[i].Name);

                        if (CoulmnType != null)
                        {
                            if (!ArrayList.Contains(CoulmnType.type) || ModelProps[i].Name == "BirthLocation" )
                            {
                                NullValue = ModelProps[i].GetValue(item);
                                if (ModelProps[i].Name.ToLower().IndexOf("date") >= 0)
                                    ColumnProp.SetValue(mymodel, NullValue != null ? NullValue.ToString().Split(' ')[0] : " ");
                                else
                                    ColumnProp.SetValue(mymodel, Convert.ChangeType(NullValue, TypeCode.String));

                            }
                            else
                            {
                                ListColumnProp = GridValues.GetType().GetProperty(ModelProps[i].Name);
                                if (ListColumnProp != null)
                                {
                                    list = (IEnumerable<FormList>)ListColumnProp.GetValue(GridValues);
                                    NullValue = ModelProps[i].GetValue(item);
                                    ListRecord = NullValue != null ? list.FirstOrDefault(a => ModelProps[i].Name != "Curr"?( a.value == int.Parse(NullValue.ToString())):(a.name == NullValue.ToString())) : null;
                                    ColumnProp.SetValue(mymodel, ListRecord != null ? ListRecord.text : " ");
                                }
                            }

                        }

                    }
                }
                data.Add(mymodel);
            }
            return data;
        }
        private void AddExcelList(PeopleExportViewModel GridValues)
        {
            string Ar = Language.Split('-')[0];
            string True = MsgUtils.Instance.Trls("true");
            string False = MsgUtils.Instance.Trls("false");

            var NationCountry = _hrUnitOfWork.Repository<Country>().ToList();
            GridValues.Nationality = NationCountry.Where(a => a.Nationality != null).Select(a => new FormList { text = Ar == "ar" ? a.NationalityAr : a.Nationality, value = a.Id }).ToList();
            GridValues.BirthLocation = _hrUnitOfWork.Repository<World>().Select(a => new FormList { text = Ar == "ar" ?(a.NameAr!= null? a.NameAr.Replace("\r", "").Replace("\n", ""):"") :(a.Name!= null ? a.Name.Replace("\r", "").Replace("\n", ""):""), value = a.CountryId, id = a.CityId, Icon = a.DistrictId }).ToList();
            GridValues.ToCountry = NationCountry.Select(a => new FormList { text = Ar == "ar" ? a.NameAr : a.Name, value = a.Id }).ToList();
            GridValues.JobId = _hrUnitOfWork.JobRepository.ReadJobs(CompanyId, Language, 0).Select(a => new FormList { text = a.LocalName, value = a.Id }).ToList();
            GridValues.DepartmentId = _hrUnitOfWork.CompanyStructureRepository.GetAllDepartments(CompanyId, null, Language).ToList();
            GridValues.PayrollId = _hrUnitOfWork.Repository<Payrolls>().Select(a => new FormList { text = a.Name, value = a.Id }).ToList();
            GridValues.PositionId = _hrUnitOfWork.PositionRepository.GetPositions(Language, CompanyId).Where(p => p.HiringStatus == 2).Select(a => new FormList { text = a.Name, value = a.Id }).ToList();
            GridValues.PayGradeId = _hrUnitOfWork.Repository<PayrollGrade>().Where(a => (a.StartDate <= DateTime.Today && (a.EndDate == null || a.EndDate >= DateTime.Today))).Select(a => new FormList { text = a.Name, value = a.Id }).ToList();
            GridValues.GroupId = _hrUnitOfWork.PeopleRepository.GetPeoples().Select(a => new FormList { text = a.Name, value = a.Id }).ToList();
            GridValues.CareerId = _hrUnitOfWork.JobRepository.ReadCareerPaths(CompanyId).Select(a => new FormList { text = a.Name, value = a.Id }).ToList();
            GridValues.ManagerId = _hrUnitOfWork.EmployeeRepository.GetActiveEmployees(Language, 0, CompanyId).Distinct().Select(a => new FormList { text = a.Employee, value = a.Id }).ToList();
            GridValues.QualificationId = _hrUnitOfWork.QualificationRepository.GetAll().Select(a => new FormList { text = a.Name, value = a.Id }).ToList();
            GridValues.ProviderId = _hrUnitOfWork.Repository<Provider>().Select(a => new FormList { text = a.Name, value = a.Id }).ToList();
            var Loctions = _hrUnitOfWork.LocationRepository.ReadLocations(Language, CompanyId).Where(a => a.IsInternal);
            GridValues.LocationId = Loctions.Select(a => new FormList { text = a.LocalName, value = a.Id }).ToList();
            GridValues.AssignLocation = Loctions.Select(a => new FormList { text = a.LocalName, value = a.Id }).ToList();
            GridValues.KafeelId = _hrUnitOfWork.LookUpRepository.GetAllKafeels().Select(a => new FormList { text = a.Name, value = a.Id }).ToList();
            GridValues.Curr = _hrUnitOfWork.LookUpRepository.GetCurrency().Select(a => new FormList { text = a.Name, name= a.Code }).ToList();
            GridValues.CareerId = _hrUnitOfWork.JobRepository.ReadCareerPaths(CompanyId).Select(a => new FormList { text = a.Name, value = a.Id }).ToList();
            GridValues.DefaultGradeId = GridValues.PayGradeId;
            GridValues.FromCountry = GridValues.ToCountry;
            GridValues.CustodyCatId = _hrUnitOfWork.CustodyRepository.fillCatCustody(false, Language).ToList();
        }
        private string SaveToExcelPublic(ExcelFileImports File, IList ActiveEmps, List<FormColumnViewModel> GeneralCol, List<FormColumnViewModel> AssignCol, PeopleExportViewModel GridValues, string Path)
        {
            string ExcelEmployees = File.ObjectName;
            string SecondSheet = MsgUtils.Instance.Trls("Assignment");
            string ListShee = MsgUtils.Instance.Trls("ListSheet");
            string List = MsgUtils.Instance.Trls("List");
            string True = MsgUtils.Instance.Trls("true");
            string False = MsgUtils.Instance.Trls("false");
            string message = "Ok" + ExcelEmployees;
            int ListCount = 1;


            if (!Directory.Exists(Server.MapPath(Path)))
                Directory.CreateDirectory(Server.MapPath(Path));

            Path = Server.MapPath(string.Format("{0}/{1}.xlsx", Path, ExcelEmployees));

            try
            {
                using (ExcelPackage pak = new ExcelPackage(new FileInfo(Path)))
                {

                    var AllNames = pak.Workbook.Worksheets.Select(a => a.Name).ToList();

                    for (int i = 0; i < AllNames.Count; i++)
                        pak.Workbook.Worksheets.Delete(AllNames[i]);

                    ExcelWorksheet ListSheet = pak.Workbook.Worksheets.Add(ListShee);


                    ListSheet.DefaultColWidth = 30;

                    ValidationData(GeneralCol, pak, ExcelEmployees, ListSheet, ActiveEmps, GridValues, ref ListCount);
                    if (File.OldObjectName == "People")
                        ValidationData(AssignCol, pak, SecondSheet, ListSheet, ActiveEmps, GridValues, ref ListCount);

                    ListSheet.Hidden = eWorkSheetHidden.VeryHidden;
                    ExcelWorksheet sheet1 = pak.Workbook.Worksheets.Where(a => a.Name == "Sheet1").FirstOrDefault();
                    if (pak.Workbook.Worksheets.Contains(sheet1))
                        pak.Workbook.Worksheets.Delete(sheet1);

                    pak.Save();
                    pak.Dispose();
                }
            }
            catch (Exception ex)
            {
                message = MsgUtils.Instance.Trls("FileinUse");
                message += ex.Message;
            }

            return message;
        }
        [HttpPost]
        public ActionResult AddFields(ExcelFileImports File)
        {
            Assembly assenmbly = Assembly.Load("Model");
            var ViewModelType = assenmbly.ExportedTypes.FirstOrDefault(a => a.Name == "ExcelGrid" + File.OldObjectName + "ViewModel");
            var ListViewModelType = typeof(List<>).MakeGenericType(ViewModelType);
            IList data = (IList)Activator.CreateInstance(ListViewModelType);


            string Path = "~/SpecialData/EmployeesData";
            List<FormColumnViewModel> GeneralCol;

            if (File.PageType == "Grid")
                GeneralCol = _hrUnitOfWork.PagesRepository.GetGridColumnInfo(CompanyId, File.OldObjectName, 0, Language).Where(a => a.ColumnType != "button" && a.isVisible).ToList();
            else
                GeneralCol = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, File.OldObjectName, 0, Language).Where(a => a.type != "hidden" && a.type != "file" && a.type != "multiselect" && a.type != "label" && a.type != "button" && a.isVisible && (string.IsNullOrEmpty(a.HtmlAttribute) || !a.HtmlAttribute.Contains("readonly"))).ToList();

            if (GeneralCol.Count == 0)
                GeneralCol = _hrUnitOfWork.PagesRepository.GetGridColumnInfo(CompanyId, "Import" + File.OldObjectName, 0, Language).Where(a => a.ColumnType != "button" && a.isVisible).ToList();
            var GeneralPPlCodes = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(GeneralCol.FirstOrDefault().PageId, Language);

            List<FormColumnViewModel> EmployCol = new List<FormColumnViewModel>();
            List<FormColumnViewModel> AssignCol = new List<FormColumnViewModel>();
            IEnumerable<FormLookUpCodeVM> EmployCodes = new List<FormLookUpCodeVM>();
            IEnumerable<FormLookUpCodeVM> AssignCodes = new List<FormLookUpCodeVM>();

            if (File.OldObjectName == "People")
            {
                EmployCol = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, "Emp", 0, Language).Where(a => a.type != "hidden" && a.type != "multiselect" && a.type != "label" && a.type != "file" && a.type != "button" && a.isVisible && (string.IsNullOrEmpty(a.HtmlAttribute) || !a.HtmlAttribute.Contains("readonly"))).ToList();
                var Code = EmployCol.Where(a => a.name == "Code").FirstOrDefault();
                AssignCol = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, "AssignmentsForm", 0, Language).Where(a => a.type != "hidden" && a.type != "multiselect" && a.type != "file" && a.type != "label" && a.type != "button" && a.isVisible && (string.IsNullOrEmpty(a.HtmlAttribute) || !a.HtmlAttribute.Contains("readonly"))).ToList();
                EmployCodes = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(EmployCol.FirstOrDefault().PageId, Language);
                AssignCodes = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(AssignCol.FirstOrDefault().PageId, Language);
                GeneralCol.AddRange(EmployCol);
                GeneralCol.Remove(Code);
                GeneralCol.Insert(0, Code);
                AssignCol.Insert(0, Code);
            }

            PeopleExportViewModel GridValues = AddCodes(GeneralPPlCodes, EmployCodes, AssignCodes);
            if (!String.IsNullOrEmpty(File.TimeZone) && File.OldObjectName == "FormLocation")
                GridValues.TimeZone = JsonConvert.DeserializeObject<List<FormList>>(File.TimeZone);
            AddExcelList(GridValues);
            

            data = RepositoryData(File.OldObjectName, File.Id);
            return Json(SaveToExcelPublic(File, data, GeneralCol, AssignCol, GridValues, Path), JsonRequestBehavior.AllowGet);
        }
        private IList RepositoryData(string oldObjectName,int? Id)
        {
            switch (oldObjectName)
            {
                case "People":
                    return _hrUnitOfWork.EmployeeRepository.GetPeopleExcel(Language, CompanyId).ToList();
                case "Job":
                    return _hrUnitOfWork.JobRepository.ReadExcelJobs(CompanyId, Language).ToList();
                case "FormLocation":
                    return _hrUnitOfWork.LocationRepository.ReadExcelLocations(CompanyId, Language).ToList();
                case "LeaveRanges":
                    return _hrUnitOfWork.LeaveRepository.GetLeaveRange(Id.Value).ToList();
                case "Custody":
                    return _hrUnitOfWork.CustodyRepository.ReadExcelCustody(CompanyId, Language).ToList();
                default:
                    return null;
            }
        }
        public void ValidationData(List<FormColumnViewModel> GeneralCol, ExcelPackage pak, string Name, ExcelWorksheet ListSheet, IList ActiveEmps, PeopleExportViewModel GridValues, ref int ListCount)
        {
            PropertyInfo Colprop;
            string List = MsgUtils.Instance.Trls("List");
            string FormulaList = "";
            int Index = 0;
            string AddressValidation = "";
            IExcelDataValidationList AddList;
            string SelectfromList = MsgUtils.Instance.Trls("selectfromList");
            List<string> ListData;
            string SecondSheet = MsgUtils.Instance.Trls("Assignment");
            ExcelWorksheet workSheet = pak.Workbook.Worksheets.Add(Name);
            workSheet.View.RightToLeft = User.Identity.RTL();
            workSheet.DefaultColWidth = 35;
            workSheet.View.FreezePanes(2,1);

            object JobValue = null;
            for (int i = 0; i < GeneralCol.Count; i++)
            {
                Index++;
                AddressValidation = workSheet.Cells[2, Index, 1045000, Index].Address;
                workSheet.DataValidations.RemoveAll(a => a.Address.Address == AddressValidation);

                if (GeneralCol[i].name != "Id")
                {
                    workSheet.Cells[1, Index].Value = GeneralCol[i].label;
                    workSheet.Cells[1, Index].Style.Font.Bold = true;

                    if (GeneralCol[i].name == "Code")
                        workSheet.Cells[AddressValidation].ConditionalFormatting.AddDuplicateValues().Style.Font.Color.Color = System.Drawing.Color.Red;

                    if (GeneralCol[i].required)
                    {
                        workSheet.Cells[1, Index].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.DarkTrellis;
                        workSheet.Cells[1, Index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    }
                    else
                    {
                        workSheet.Cells[1, Index].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        workSheet.Cells[1, Index].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    }

                    if (GeneralCol[i].type == "select" || GeneralCol[i].type == "radioset" || GeneralCol[i].type == "autocomplete" || GeneralCol[i].type == "checkbox")
                    {

                        AddList = workSheet.DataValidations.AddListValidation(AddressValidation);
                        var ListSheetAddress = ListSheet.Cells[2, ListCount].Address;

                        //Get List Values
                        ListData = GetList(GridValues, GeneralCol[i].name, GeneralCol[i].type);
                        if (ListData.Count > 0)
                        {
                            ListSheet.Cells[1, ListCount].Value = List + " " + GeneralCol[i].label;

                            for (int z = 0; z < ListData.Count; z++)
                                ListSheet.Cells[z + 2, ListCount].Value = ListData[z];

                            FormulaList = ListSheet.Cells[2, ListCount, ListData.Count + 2, ListCount].FullAddressAbsolute;
                            AddList.Formula.ExcelFormula = FormulaList;

                            AddList.ShowErrorMessage = true;
                            AddList.Error = SelectfromList;

                            if (GeneralCol[i].required)
                                AddList.AllowBlank = false;

                        }
                        else
                            AddList.Formula.Values.Add("");
                        ListCount++;
                    }

                    for (int j = 0; j < ActiveEmps.Count; j++)
                    {

                        Colprop = ActiveEmps[j].GetType().GetProperty("Id");
                        if (Name != SecondSheet && Colprop != null)
                            workSheet.Cells[j + 2, GeneralCol.Count + 1].Value = Colprop.GetValue(ActiveEmps[j]);

                        if (GeneralCol[i].name == "LocationId" && Name == SecondSheet)
                        {
                            Colprop = ActiveEmps[j].GetType().GetProperty("AssignLocation");
                            if (Colprop != null)
                                workSheet.Cells[j + 2, Index].Value = Colprop.GetValue(ActiveEmps[j]);
                        }
                        else if (GeneralCol[i].name == "Profession" && !string.IsNullOrEmpty(GeneralCol[i].HtmlAttribute))
                        {
                            Colprop = ActiveEmps[j].GetType().GetProperty("EmpProfession");
                            if (Colprop != null)
                                workSheet.Cells[j + 2, Index].Value = Colprop.GetValue(ActiveEmps[j]);
                        }
                        else
                        {
                            var Prop = ActiveEmps[j].GetType().GetProperty(GeneralCol[i].name)?.GetValue(ActiveEmps[j]);
                            if (GeneralCol[i].name == "Code" && Name == SecondSheet)
                            {
                                JobValue = ActiveEmps[j].GetType().GetProperty("JobId").GetValue(ActiveEmps[j]);
                                if (string.IsNullOrEmpty(JobValue?.ToString().Trim()))
                                    Prop = "";
                            }
                            workSheet.Cells[j + 2, Index].Value = Prop;

                            if (GeneralCol[i].name == "Code")
                                workSheet.Cells[j + 2, Index].Style.Locked = true;

                            workSheet.Cells[j + 2, Index].Style.HorizontalAlignment = User.Identity.RTL() ? OfficeOpenXml.Style.ExcelHorizontalAlignment.Right : OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells[j + 2, Index].Style.WrapText = true;
                        }
                    }
                }
            }
            if (Name != SecondSheet)
                workSheet.Column(GeneralCol.Count + 1).Hidden = true;

        }
        private List<string> GetList(PeopleExportViewModel GridValues, string Name, string Type)
        {
            switch (Name)
            {
                case "Gender":
                    return GridValues.Gender.Select(a => a.text).ToList();
                case "QualificationId":
                    return GridValues.QualificationId.Select(a => a.text).ToList();
                case "BirthLocation":
                    return GridValues.BirthLocation.Select(a => a.text).ToList();
                case "Nationality":
                    return GridValues.Nationality.Select(a => a.text).ToList();
                case "MaritalStat":
                    return GridValues.MaritalStat.Select(a => a.text).ToList();
                case "Religion":
                    return GridValues.Religion.Select(a => a.text).ToList();
                case "MedicalStat":
                    return GridValues.MedicalStat.Select(a => a.text).ToList();
                case "BloodClass":
                    return GridValues.BloodClass.Select(a => a.text).ToList();
                case "RecommenReson":
                    return GridValues.RecommenReson.Select(a => a.text).ToList();
                case "SalaryBasis":
                    return GridValues.SalaryBasis.Select(a => a.text).ToList();
                case "PersonType":
                    return GridValues.PersonType.Select(a => a.text).ToList();
                case "AssignStatus":
                    return GridValues.AssignStatus.Select(a => a.text).ToList();
                case "MilitaryStat":
                    return GridValues.MilitaryStat.Select(a => a.text).ToList();
                case "ProviderId":
                    return GridValues.ProviderId.Select(a => a.text).ToList();
                case "LocationId":
                    return GridValues.LocationId.Select(a => a.text).ToList();
                case "KafeelId":
                    return GridValues.KafeelId.Select(a => a.text).ToList();
                case "Curr":
                    return GridValues.Curr.Cast<FormList>().Select(a => a.text).ToList();
                case "ToCountry":
                case "FromCountry":
                    return GridValues.ToCountry.Select(a => a.text).ToList();
                case "JobId":
                    return GridValues.JobId.Select(a => a.text).ToList();
                case "DepartmentId":
                    return GridValues.DepartmentId.Select(a => a.text).ToList();
                case "PayrollId":
                    return GridValues.PayrollId.Select(a => a.text).ToList();
                case "PositionId":
                    return GridValues.PositionId.Select(a => a.text).ToList();
                case "PayGradeId":
                    return GridValues.PayGradeId.Select(a => a.text).ToList();
                case "GroupId":
                    return GridValues.GroupId.Select(a => a.text).ToList();
                case "CareerId":
                    return GridValues.CareerId.Select(a => a.text).ToList();
                case "Performance":
                    return GridValues.Performance.Select(a => a.text).ToList();
                case "ManagerId":
                    return GridValues.ManagerId.Select(a => a.text).ToList();
                case "MilCertGrade":
                    return GridValues.MilCertGrade.Select(a => a.text).ToList();
                case "Rank":
                    return GridValues.Rank.Select(a => a.text).ToList();
                case "EmpTasks":
                    return GridValues.EmpTasks.Select(a => a.text).ToList();
                case "TimeZone":
                    return GridValues.TimeZone.Select(a => a.text).ToList();
                case "CustodyCat":
                    return GridValues.CustodyCatId.Select(a => a.text).ToList();
                default:
                    if (Type == "checkbox")
                        return new List<string>() { MsgUtils.Instance.Trls("false"), MsgUtils.Instance.Trls("true") };
                    else
                        return new List<string>();
            }

        }
        public void ImportToExcelGrid(ExcelWorksheet workSheet, List<FormColumnViewModel> GeneralCold, IEnumerable<FormLookUpCodeVM> GeneralCol, PeopleExportViewModel GridValues, List<FormList> Addresses, int i, object model)
        {
            int tocol = workSheet.Dimension.End.Column;
            var GridValuesProp = GridValues.GetType().GetProperties();
            string True = MsgUtils.Instance.Trls("true");
            PropertyInfo ColumnProp;
            bool Nul = false;
            string Assign = MsgUtils.Instance.Trls("Assignment");
            float? on = null;
            string value = "";
            FormColumnViewModel Column;
            for (int z = 1; z <= tocol; z++)
            {
                value = workSheet.Cells[1, z].Value?.ToString();
                Column = GeneralCold.FirstOrDefault(a => a.label == value);
                if (value != null && Column != null)
                {
                    string ExactValue = Column.name;
                    if (ExactValue == "Profession" && Column.HtmlAttribute != null && Column.HtmlAttribute.Contains("data-emp"))
                        ExactValue = "EmpProfession";
                    else if (ExactValue == "LocationId" && Column.HtmlAttribute != null && Column.HtmlAttribute.Contains("data-emp"))
                        ExactValue = "AssignLocation";

                    string ColumnType = Column.type;
                    Nul = false;
                    ColumnProp = model.GetType().GetProperty(ExactValue);

                    if (ColumnProp != null)
                    {
                        var type = ColumnProp.PropertyType;
                        if (type.FullName.Contains("Nullable`1"))
                            type = Nullable.GetUnderlyingType(type);

                        if (ColumnType == "date" || ColumnType == "time")
                            model.GetType().GetProperty(ExactValue).SetValue(model, BackDateTime(workSheet.Cells[i, z].Value));
                        else if (ExactValue.ToLower().Contains("address") || ExactValue == "BirthLocation" || ExactValue == "TimeZone" || ExactValue =="Curr")
                            model.GetType().GetProperty(ExactValue).SetValue(model, BackAddress(workSheet.Cells[i, z].Value, model, ExactValue, Addresses,GridValues.Curr));
                        else if (Column.CodeName != null && (ColumnType == "select" || ColumnType == "radioset"))
                        {
                            on = LookupValue(Column.CodeName, workSheet.Cells[i, z].Value, GeneralCol);
                            if (Column.required && on == null)
                                on = 0;
                            else if (on == null)
                                Nul = true;
                            model.GetType().GetProperty(ExactValue).SetValue(model, Nul ? null : Convert.ChangeType(on, type));
                        }
                        else if ((ColumnType == "select" || ColumnType == "radioset" || ColumnType == "autocomplete"))
                        {
                            on = (int?)GetDropDownValue(workSheet.Cells[i, z].Value, (IEnumerable<FormList>)GridValuesProp.Where(a => a.Name == ExactValue).FirstOrDefault().GetValue(GridValues));
                            if (Column.required && on == null)
                                on = 0;
                            else if (on == null)
                                Nul = true;
                            model.GetType().GetProperty(ExactValue).SetValue(model, Nul ? null : Convert.ChangeType(on, type));
                        }
                        else if (ColumnType == "number")
                        {
                            on = BackNumber(workSheet.Cells[i, z].Value);
                            if (Column.required && on == null)
                                on = 0;
                            else if (on == null)
                                Nul = true;
                            model.GetType().GetProperty(ExactValue).SetValue(model, Nul ? null : Convert.ChangeType(on, type));
                        }
                        else if (ColumnType == "checkbox")
                            model.GetType().GetProperty(ExactValue).SetValue(model, BackStringData(workSheet.Cells[i, z].Value) == True ? true : false);
                        else
                           model.GetType().GetProperty(ExactValue).SetValue(model, BackStringData(workSheet.Cells[i, z].Value));
                        
                    }
                }

            }
            if (workSheet.Name != Assign)
            {
                if (workSheet.Cells[1, tocol].Value != null)
                    tocol++;
                value = workSheet.Cells[i, tocol].Value?.ToString();
                if (String.IsNullOrEmpty(value))
                    value = "0";
                model.GetType().GetProperty("Id").SetValue(model, int.Parse(value));
            }
        }
        private short? LookupValue(string Codename, object Value, IEnumerable<FormLookUpCodeVM> Codes)
        {
            return (Value != null ? (Codes.FirstOrDefault(a => a.CodeName == Codename && a.name == Value.ToString())?.id) : null);
        }
        private PeopleExportViewModel AddCodes(IEnumerable<FormLookUpCodeVM> PeopleCodes, IEnumerable<FormLookUpCodeVM> EmpCodes, IEnumerable<FormLookUpCodeVM> AssignCodes)
        {
            int[] ids = { 1, 2, 4, 5 };

            return new PeopleExportViewModel()
            {
                Gender = PeopleCodes.Where(a => a.CodeName == "Gender").Select(c => new FormList { text = c.name, value = c.id }).ToList(),
                AssignStatus = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("Assignment", Language).Where(a => a.SysCodeId == 1 || a.SysCodeId == 2).Select(a => new FormList { value = a.CodeId, text = a.Title }).ToList(),
                BloodClass = PeopleCodes.Where(a => a.CodeName == "BloodClass").Select(c => new FormList { text = c.name, value = c.id }).ToList(),
                MaritalStat = PeopleCodes.Where(a => a.CodeName == "MaritalStat").Select(c => new FormList { text = c.name, value = c.id }).ToList(),
                MedicalStat = PeopleCodes.Where(a => a.CodeName == "MedicalStat").Select(c => new FormList { text = c.name, value = c.id }).ToList(),
                MilCertGrade = PeopleCodes.Where(a => a.CodeName == "MilCertGrade").Select(c => new FormList { text = c.name, value = c.id }).ToList(),
                MilitaryStat = new List<FormList>() { new FormList() { value = 1, text = MsgUtils.Instance.Trls("Performed") }, new FormList() { value = 2, text = MsgUtils.Instance.Trls("Exempt") }, new FormList() { value = 3, text = MsgUtils.Instance.Trls("Deferred") }, new FormList() { value = 4, text = MsgUtils.Instance.Trls("Underage") } },
                Performance = _hrUnitOfWork.LookUpRepository.GetLookUpCodes("Performance", Language).Select(c => new FormList { text = c.Title, value = c.CodeId }).ToList(),
                PersonType = EmpCodes?.Count() > 0 ? EmpCodes.Where(a => a.CodeName == "PersonType" && ids.Contains(a.SysCode)).Select(c => new FormList { text = c.name, value = c.id }).ToList() : null,
                Rank = PeopleCodes.Where(a => a.CodeName == "Rank").Select(c => new FormList { text = c.name, value = c.id }).ToList(),
                RecommenReson = PeopleCodes.Where(a => a.CodeName == "RecommenReson").Select(c => new FormList { text = c.name, value = c.id }).ToList(),
                Religion = PeopleCodes.Where(a => a.CodeName == "Religion").Select(c => new FormList { text = c.name, value = c.id }).ToList(),
                SalaryBasis = AssignCodes?.Count() > 0 ? AssignCodes.Where(a => a.CodeName == "SalaryBasis").Select(c => new FormList { text = c.name, value = c.id }).ToList() : null,
                EmpTasks = new List<FormList>() { new FormList() { text = MsgUtils.Instance.Trls("Use eligibility criteria"), value = 2 }, new FormList() { text = MsgUtils.Instance.Trls("Employee whose direct managed"), value = 1 } },
                Frequency = PeopleCodes.Where(a => a.CodeName == "Frequency").Select(c => new FormList { text = c.name, value = c.id }).ToList(),
            };

        }
        private float? GetDropDownValue(object Value, IEnumerable<FormList> List)
        {
            return (List.Count() != 0 ? (Value != null ? (!string.IsNullOrEmpty(Value.ToString().Trim()) ? (List.FirstOrDefault(a => a.text ==  Value.ToString())?.value) : null) : null) : null);
        }
        private DateTime? BackDateTime(object value)
        {
            double v = 0;
            if (double.TryParse(value?.ToString(), out v))
                return DateTime.FromOADate(v);
            else
                return (value != null ? (!string.IsNullOrEmpty(value.ToString().Trim()) ? Convert.ToDateTime(value) : (DateTime?)(null)) : null);
        }
        private string BackStringData(object value)
        {
            return (value != null ? (!string.IsNullOrEmpty(value.ToString().Trim()) ? value.ToString() : "") : "");
        }
        private float? BackNumber(object value)
        {
            return (value != null ? (!string.IsNullOrEmpty(value.ToString().Trim()) ? float.Parse(value.ToString()) : (float?)null) : null);
        }
        private string BackAddress(object value, object item, string Name, IEnumerable<FormList> Items, IEnumerable Curr)
        {
            PropertyInfo propColumn;
            if (value != null)
            {
                if (!string.IsNullOrEmpty(value.ToString().Trim()))
                {
                    value = value.ToString().Replace("\r", "").Replace("\n", "");                   
                    var loc = Name != "Curr" ?Items.FirstOrDefault(a => a.text == value.ToString()):Curr.Cast<FormList>().FirstOrDefault(a => a.text == value.ToString());
                    if (loc != null)
                    {
                        switch (Name)
                        {
                            case "BirthLocation":
                                propColumn = item.GetType().GetProperty("BirthCity");
                                if (propColumn != null)
                                    propColumn.SetValue(item, loc.id);
                                propColumn = item.GetType().GetProperty("BirthCountry");
                                if (propColumn != null)
                                    propColumn.SetValue(item, loc.value);
                                propColumn = item.GetType().GetProperty("BirthDstrct");
                                if (propColumn != null)
                                    propColumn.SetValue(item, loc.Icon);
                                break;
                            case "Address":
                                propColumn = item.GetType().GetProperty("AddressId");
                                if (propColumn != null)
                                    propColumn.SetValue(item, loc.value);
                                break;
                            case "TimeZone":
                                propColumn = item.GetType().GetProperty("TimeZone");
                                if (propColumn != null)
                                    propColumn.SetValue(item, loc.text);
                                return loc.name;
                            case "Curr":
                                return loc.name;
                            default:
                                propColumn = item.GetType().GetProperty("HoAddressId");
                                if (propColumn != null)
                                    propColumn.SetValue(item, loc.value);
                                break;
                        }
                        return loc.text;
                    }else
                        return value.ToString();
                }else
                    return "";
            }else
                return "";
        }
        [HttpPost]
        public ActionResult ImportDataInformation(ExcelFileImports File)
        {
            ErrorsViewModel Obj = new ErrorsViewModel();
            Obj.Errors = new List<Error>();
            if (!File.ErrorData)
            {
                if (File.ExcelFile == null)
                {
                    Obj.Errors.Add(new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("EmptyFile") } } });
                    return Json(Obj);
                }
                    string[] ExcelExt = { "xlsx", "xls" };

                if (File.ExcelFile != null && !Array.Exists(ExcelExt, f => f == File.ExcelFile.FileName.Split('.')[1]))
                {
                    Obj.Errors.Add(new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("NotExcelFile") } } });
                    return Json(Obj);
                }
            }
            string Ar = Language.Split('-')[0];
            List<FormColumnViewModel> GeneralCold;

            if (File.PageType == "Grid")
                GeneralCold = _hrUnitOfWork.PagesRepository.GetGridColumnInfo(CompanyId, File.OldObjectName, 0, Language).Where(a => a.ColumnType != "button" && a.isVisible).ToList();
            else
                GeneralCold = _hrUnitOfWork.PagesRepository.GetExcelColumnInfo(CompanyId, File.OldObjectName, 0, Language).ToList();

            if (GeneralCold.Count == 0)
                GeneralCold = _hrUnitOfWork.PagesRepository.GetGridColumnInfo(CompanyId, "Import"+File.OldObjectName, 0, Language).Where(a => a.ColumnType != "button" && a.isVisible).ToList();

            var GeneralCol = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(GeneralCold.FirstOrDefault().PageId, Language);
            IEnumerable<FormLookUpCodeVM> EmployCol = null;
            IEnumerable<FormLookUpCodeVM> AssignCol = null;
            List<FormColumnViewModel> AssignCold = null;
            string SecondSheet = "";
            string FirstSheet = File.ObjectName;

            //////ImportLookUpCodes///////
            if (File.OldObjectName == "People")
            {
                var EmployCold = _hrUnitOfWork.PagesRepository.GetExcelColumnInfo(CompanyId, "Emp", 0, Language).ToList();
                AssignCold = _hrUnitOfWork.PagesRepository.GetExcelColumnInfo(CompanyId, "AssignmentsForm", 0, Language).ToList();
                EmployCol = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(EmployCold.FirstOrDefault().PageId, Language);
                AssignCol = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(AssignCold.FirstOrDefault().PageId, Language);
                GeneralCold.AddRange(EmployCold);
                SecondSheet = MsgUtils.Instance.Trls("Assignment");
            }

            PeopleExportViewModel GridValues = AddCodes(GeneralCol, EmployCol, AssignCol);
            AddExcelList(GridValues);
            
            var Addresses = _hrUnitOfWork.Repository<Address>().Select(a => new FormList { text = a.Address1 + " " + a.Address2 + " " + a.Address3, value = a.Id }).ToList();
            Addresses.AddRange(GridValues.BirthLocation);
            if (!String.IsNullOrEmpty(File.TimeZone) && File.OldObjectName == "FormLocation")
            {
                GridValues.TimeZone = JsonConvert.DeserializeObject<List<FormList>>(File.TimeZone);
                Addresses.AddRange(GridValues.TimeZone);
            }
            ///////////////////////////
            Obj.Selected = GridValues;
            Assembly assenmbly = Assembly.Load("Model");

            ExcelPackage pak;
            string Path = Server.MapPath("~/SpecialData/CurrentData/" + FirstSheet + ".xlsx");
            if (File.ErrorData)
                pak = new ExcelPackage(new FileInfo(Path));
            else
                pak = new ExcelPackage(File.ExcelFile.InputStream);

            try
            {
                ExcelWorksheet workSheet = pak.Workbook.Worksheets.FirstOrDefault(a => a.Name == FirstSheet);
                if (workSheet == null || workSheet.Dimension == null)
                {
                    if (workSheet == null)
                        Obj.Errors.Add(new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("WrongExcel") } } });
                    else
                        Obj.Errors.Add(new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls("NoDataInExcel") } } });
                    pak.Dispose();
                    return Json(Obj);
                }
                var ViewModelType = assenmbly.ExportedTypes.FirstOrDefault(a => a.Name == "ExcelGrid" + File.OldObjectName + "ViewModel");
                var ListViewModelType = typeof(List<>).MakeGenericType(ViewModelType);
                var data = (IList)Activator.CreateInstance(ListViewModelType);


                for (int i = 1; i < workSheet.Dimension.End.Row; i++)
                {
                    var mymodel = Activator.CreateInstance(ViewModelType);
                    workSheet = pak.Workbook.Worksheets.FirstOrDefault(a => a.Name == FirstSheet);
                    ImportToExcelGrid(workSheet, GeneralCold, GeneralCol, GridValues, Addresses, i + 1, mymodel);
                    data.Add(mymodel);
                }
                if (File.OldObjectName == "People")
                {
                    string LocaloName = "";
                    int Length = 0;
                    string FirstName = "";
                    string Fathername = "";
                    string FamilyName = "";
                    for (int z = 0; z < data.Count; z++)
                    {
                        LocaloName = data[z].GetType().GetProperty("localName").GetValue(data[z])?.ToString();
                        FirstName = data[z].GetType().GetProperty("FirstName").GetValue(data[z])?.ToString();
                        Fathername = data[z].GetType().GetProperty("Fathername").GetValue(data[z])?.ToString();
                        FamilyName = data[z].GetType().GetProperty("Familyname").GetValue(data[z])?.ToString();

                        if ((!string.IsNullOrEmpty(LocaloName)) &&( string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(FamilyName) || string.IsNullOrEmpty(Fathername)))
                        {
                            var Arr = LocaloName.TrimEnd().TrimStart().Split(' ');
                            Length = Arr.Length;
                            data[z].GetType().GetProperty("FirstName").SetValue(data[z], Arr[0]);
                            data[z].GetType().GetProperty("Fathername").SetValue(data[z], Length > 1 ? Arr[1] : "");
                            data[z].GetType().GetProperty("GFathername").SetValue(data[z], Length > 3 ? Arr[2] : "");
                            data[z].GetType().GetProperty("Familyname").SetValue(data[z], Length > 3 ? Arr[3] : (Length > 2 ? Arr[2] : ""));
                        }
                    }

                    workSheet = pak.Workbook.Worksheets.FirstOrDefault(a => a.Name == SecondSheet);
                    string Code = "";
                    string CompareCode = "";
                    for (int i = 2; i <= workSheet.Dimension.End.Row; i++)
                    {
                        Code = workSheet.Cells[i, 1].Value?.ToString();
                        if (!string.IsNullOrEmpty(Code))
                        {
                            for (int z = 0; z < data.Count; z++)
                            {
                                CompareCode = data[z].GetType().GetProperty("Code").GetValue(data[z])?.ToString();
                                if (CompareCode != null && CompareCode.Equals(Code))
                                {
                                    data[z].GetType().GetProperty("AssignError").SetValue(data[z], true);
                                    ImportToExcelGrid(workSheet, AssignCold, GeneralCol, GridValues, Addresses, i, data[z]);
                                    break;
                                }
                            }
                        }
                    }
                }
                
                
                Obj.data = data;
                pak.Dispose();
                if (File.ErrorData)
                    System.IO.File.Delete(Path);
            }
            catch (Exception ex)
            {
                Obj.Errors.Add(new Error { errors = new List<ErrorMessage>() { new ErrorMessage() { message = MsgUtils.Instance.Trls(ex.Message) } } });
                pak.Dispose();
                return Json(Obj);
            }
            ////////////////////////////

            PageDiv ExistImportEmps = _hrUnitOfWork.Repository<PageDiv>().Where(s => s.CompanyId == CompanyId && s.DivType == "Grid" && s.ObjectName == "Import" + File.OldObjectName).FirstOrDefault();

            if (ExistImportEmps == null)
            {
                int pageId = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.CompanyId == CompanyId && a.ObjectName == File.OldObjectName).Select(b => b.Id).FirstOrDefault();

                var FormColumns = _hrUnitOfWork.PageEditorRepository.GetFormColsToGridImport(pageId).ToList();
                var OldCoulmnTitle = _hrUnitOfWork.PageEditorRepository.GetColumnTitles(CompanyId, "Import" + File.OldObjectName, 0, Language).ToList();
                var NewCoulmnTitle = _hrUnitOfWork.PageEditorRepository.GetColumnTitles(CompanyId, File.OldObjectName, 0, Language).Distinct().ToList();
                PageDiv PagDiv = new PageDiv
                {
                    CompanyId = CompanyId,
                    DivType = "Grid",
                    HasCustCols = false,
                    MenuId = File.MenuId,
                    ObjectName = "Import" + File.OldObjectName,
                    TableName = GeneralCold.FirstOrDefault().TableName,
                    Title = "Import" + File.OldObjectName,
                    Version = File.Ver
                };
                if (ExistImportEmps != null)
                    File.Id = ExistImportEmps.Id;
                _hrUnitOfWork.PageEditorRepository.AddPage(PagDiv);
                if (FormColumns.Count != 0)
                    AddFromFormColumns(FormColumns, PagDiv);
                else
                {
                    var GridColumns = _hrUnitOfWork.PageEditorRepository.GetGridColumns(pageId).ToList();
                    AddFromGridColumns(GridColumns, PagDiv);
                }

                if (NewCoulmnTitle.Count != 0)
                    RefreshColumnTitle(OldCoulmnTitle, NewCoulmnTitle, File);
                else
                    AddCoulmnTitle(OldCoulmnTitle, File);


                Obj.Errors = SaveChanges(Language);
            }
            return Json(Obj);
        }
        [HttpPost]
        public ActionResult RefreshColumns(ExcelFileImports File)
        {
            List<Error> Errors = new List<Error>();

            PageDiv ExistImportEmps = _hrUnitOfWork.Repository<PageDiv>().Where(s => s.CompanyId == CompanyId && s.DivType == "Grid" && s.ObjectName == "Import"+File.OldObjectName).FirstOrDefault();

            int pageId = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.CompanyId == CompanyId && a.ObjectName == File.OldObjectName && a.Version == 0).Select(b => b.Id).FirstOrDefault();

            var FormColumns = _hrUnitOfWork.PageEditorRepository.GetFormColsToGridImport(pageId).ToList();
            
            var OldCoulmnTitle = _hrUnitOfWork.PageEditorRepository.GetColumnTitles(CompanyId, "Import"+File.OldObjectName, File.Ver, Language).ToList();
            var NewCoulmnTitle = _hrUnitOfWork.PageEditorRepository.GetColumnTitles(CompanyId, File.OldObjectName, 0, Language).ToList();
            var GridCol = _hrUnitOfWork.PageEditorRepository.GetGridColumns(pageId).ToList();
            if (File.OldObjectName == "People")
            {
                pageId = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.CompanyId == CompanyId && a.ObjectName == "Emp" && a.Version == File.Ver).Select(b => b.Id).FirstOrDefault();
                FormColumns.AddRange(_hrUnitOfWork.PageEditorRepository.GetFormColsToGridImport(pageId).Where(a=>a.ColumnName != "Code").ToList());
                pageId = _hrUnitOfWork.Repository<PageDiv>().Where(a => a.CompanyId == CompanyId && a.ObjectName == "AssignmentsForm" && a.Version == File.Ver).Select(b => b.Id).FirstOrDefault();
                FormColumns.AddRange(_hrUnitOfWork.PageEditorRepository.GetFormColsToGridImport(pageId).ToList());
                NewCoulmnTitle.AddRange(_hrUnitOfWork.PageEditorRepository.GetColumnTitles(CompanyId, "Emp", 0, Language).ToList());
                NewCoulmnTitle.AddRange(_hrUnitOfWork.PageEditorRepository.GetColumnTitles(CompanyId, "AssignmentsForm", 0, Language).ToList());
            }
            if (ExistImportEmps == null)
            {

                PageDiv PagDiv = new PageDiv
                {
                    CompanyId = CompanyId,
                    DivType = "Grid",
                    HasCustCols = false,
                    MenuId = File.MenuId,
                    ObjectName ="Import"+File.OldObjectName,
                    TableName = File.TableName,
                    Title = File.ObjectName,
                    Version = File.Ver
                };
                File.Id = PagDiv.Id;
                _hrUnitOfWork.PageEditorRepository.AddPage(PagDiv);
                if (FormColumns.Count != 0)
                    AddFromFormColumns(FormColumns, PagDiv);
                else
                    AddFromGridColumns(GridCol, PagDiv);

            }
            else
            {
                File.Id = ExistImportEmps.Id;

                var GridCoulmns = _hrUnitOfWork.PageEditorRepository.GetGridColumns(ExistImportEmps.Id).ToList();

                if (FormColumns.Count != 0)
                    RefreshFormColumns(FormColumns, GridCoulmns);
                else
                    RefreshGridColumns(GridCol, GridCoulmns);

            }

            if (NewCoulmnTitle.Count != 0)
                RefreshColumnTitle(OldCoulmnTitle, NewCoulmnTitle,File);
            else
                AddCoulmnTitle(OldCoulmnTitle, File);

            Errors = SaveChanges(Language);

            return Json(Errors);
        }
        private void AddFromGridColumns(List<GridColumn> GridColumns, PageDiv PagDiv)
        {
            foreach (var item in GridColumns)
            {
                if (item.ColumnName != "CreatedUser" && item.ColumnName != "ModifiedUser" && item.ColumnName != "CreatedTime" && item.ColumnName != "ModifiedTime")
                {
                    _hrUnitOfWork.PageEditorRepository.Add(new GridColumn
                    {
                        ColumnName = item.ColumnName,
                        ColumnOrder = item.ColumnOrder,
                        ColumnType = item.ColumnType,
                        Custom = item.Custom,
                        DefaultValue = item.DefaultValue,
                        DefaultWidth = item.DefaultWidth,
                        Grid = PagDiv,
                        GridId = PagDiv.Id,
                        IsUnique = item.IsUnique,
                        isVisible = item.isVisible,
                        Max = item.Max,
                        MaxLength = item.MaxLength,
                        Min = item.Min,
                        MinLength = item.MinLength,
                        Pattern = item.Pattern,
                        PlaceHolder = item.PlaceHolder,
                        Required = item.Required,
                        UniqueColumns = item.UniqueColumns
                    });

                }
            }
        }
        private void AddFromFormColumns(List<FormColumn> FormColumns, PageDiv PagDiv)
        {
            string[] Arr = { "CreatedUser", "ModifiedUser", "CreatedTime", "ModifiedTime", "Id" };
            foreach (var item in FormColumns)
            {
                if (!Array.Exists(Arr, f => f == item.ColumnName))
                {
                    if (item.InputType != "button" && item.InputType != "file" && item.InputType != "multiselect")
                    {

                        _hrUnitOfWork.PageEditorRepository.Add(new GridColumn
                        {
                            ColumnName = item.ColumnName,
                            CodeName = item.CodeName,
                            ColumnOrder = item.ColumnOrder != null ? item.ColumnOrder.Value : (byte)0,
                            DefaultValue = item.DefaultValue,
                            DefaultWidth = 50,
                            Grid = PagDiv,
                            ColumnType = (item.InputType == "checkbox" ? "boolean" : (item.InputType == "autocomplete" ? "number" : item.InputType)),
                            isVisible = item.isVisible,
                            IsUnique = item.IsUnique,
                            Max = item.Max,
                            MaxLength = item.MaxLength,
                            Min = item.Min,
                            InputType = ((item.InputType == "autocomplete" || item.InputType == "select") ? "select" : item.InputType),
                            MinLength = item.MinLength,
                            Pattern = item.Pattern,
                            PlaceHolder = item.PlaceHolder,
                            Required = item.Required,
                            OrgInputType = item.OrgInputType,
                            UniqueColumns = item.UniqueColumns,
                        });
                    }
                }
            }
            if (PagDiv.ObjectName == "ImportPeople")
                AddGridAddress(PagDiv);

            _hrUnitOfWork.PageEditorRepository.Add(new GridColumn
            {
                isVisible = true,
                CodeName = null,
                ColumnName = "Delete",
                ColumnOrder = Convert.ToByte(FormColumns.Count + 1),
                ColumnType = "button",
                DefaultWidth = 50,
                Grid = PagDiv,
                InputType = "button",
                IsUnique = false,
                Max = null,
                DefaultValue = null,
                MaxLength = null,
                Pattern = null,
                Required = false,
                PlaceHolder = null,
                Min = null,
                MinLength = null,
                OrgInputType = null,
                UniqueColumns = null,
                Custom = null
            });
            _hrUnitOfWork.PageEditorRepository.Add(new GridColumn
            {
                isVisible = true,
                CodeName = null,
                ColumnName = "Id",
                ColumnOrder = 0,
                ColumnType = "number",
                DefaultWidth = 50,
                Grid = PagDiv,
                InputType = "none",
                IsUnique = false,
                Max = null,
                DefaultValue = null,
                MaxLength = null,
                Pattern = null,
                Required = true,
                PlaceHolder = null,
                Min = null,
                MinLength = null,
                OrgInputType = "none",
                UniqueColumns = null,
                Custom = null
            });
        }
        private void AddGridAddress(PageDiv Page)
        {
            string[] arr = { "Address2", "Address3", "PostalCode", "HostAddress2", "HostAddress3", "HostPostalCode", "AssignDate", "AssignEndDate", "AssignStatus", "Department", "Job", "Position", "PeopleGroup" };
            for (int i = 0; i < arr.Length; i++)
            {
                _hrUnitOfWork.PageEditorRepository.Add(new GridColumn
                {
                    ColumnName = arr[i],
                    ColumnOrder = 20,
                    ColumnType = (arr[i].Contains("Date") ? "date" : "text"),
                    DefaultWidth = 50,
                    Grid = Page,
                    IsUnique = false,
                    isVisible = true,
                    Required = true,
                    PlaceHolder = arr[i],
                    MaxLength = (arr[i].Contains("Code") ? 15 : (short?)100)
                });
            }
        }
        private void RefreshColumnTitle(List<ColumnTitle> OldTitles, List<ColumnTitle> NewTitle,ExcelFileImports File)
        {
            var obj = OldTitles.FirstOrDefault();
            string key = CompanyId + obj.ObjectName + obj.Version + Language;

            var exist = _hrUnitOfWork.PagesRepository.CacheManager.IsSet(key);
            if (exist)
                _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

            if (OldTitles.Count == 0)
               AddCoulmnTitle(NewTitle,File);
            else
            {
                foreach (var item in NewTitle)
                {
                    ColumnTitle title = OldTitles.FirstOrDefault(a => a.ColumnName == item.ColumnName);

                    if (title != null)
                    {
                        item.Title = title.Title;
                        _hrUnitOfWork.PageEditorRepository.Attach(item);
                        _hrUnitOfWork.PageEditorRepository.Entry(item).State = EntityState.Modified;
                    }
                }
            }
        }
        private void AddCoulmnTitle(List<ColumnTitle> Titles, ExcelFileImports File)
        {
            foreach (var item in Titles)
            {
                _hrUnitOfWork.PageEditorRepository.Add(new ColumnTitle
                {
                    ColumnName = item.ColumnName,
                    CompanyId = CompanyId,
                    Culture = Language,
                    ObjectName = "Import" + File.OldObjectName,
                    Title = item.Title,
                    Version = File.Ver
                });
            }

        }
        private void RefreshFormColumns(List<FormColumn> FormColumns, List<GridColumn> GridCoulmns)
        {
            var obj = _hrUnitOfWork.Repository<PageDiv>().Where(p => p.Id == GridCoulmns.FirstOrDefault().GridId).Select(p => new { p.ObjectName, p.Version }).FirstOrDefault();
            string key = CompanyId + obj.ObjectName + obj.Version + Language;
            var exist = _hrUnitOfWork.PagesRepository.CacheManager.IsSet(key);
            if (exist)
                _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

            foreach (var item in FormColumns)
            {
                var col = GridCoulmns.FirstOrDefault(a => a.ColumnName == item.ColumnName);

                if (col != null)
                {
                    col.DefaultValue = item.DefaultValue;
                    col.isVisible = item.isVisible;
                    col.Pattern = item.Pattern;
                    col.Required = item.Required;
                    col.MinLength = item.MinLength;
                    col.Max = item.Max;
                    col.MaxLength = item.MaxLength;
                    col.Min = item.Min;
                    col.IsUnique = item.IsUnique;
                    col.UniqueColumns = item.UniqueColumns;
                    _hrUnitOfWork.PageEditorRepository.Attach(col);
                    _hrUnitOfWork.PageEditorRepository.Entry(col).State = EntityState.Modified;

                }
            }
        }
        private void RefreshGridColumns(List<GridColumn> OldGridColumns, List<GridColumn> GridCoulmns)
        {
            var obj = _hrUnitOfWork.Repository<PageDiv>().Where(p => p.Id == GridCoulmns.FirstOrDefault().GridId).Select(p => new { p.ObjectName, p.Version }).FirstOrDefault();
            string key = CompanyId + obj.ObjectName + obj.Version + Language;

            var exist = _hrUnitOfWork.PagesRepository.CacheManager.IsSet(key);
            if (exist)
                _hrUnitOfWork.PagesRepository.CacheManager.Remove(key);

            foreach (var item in OldGridColumns)
            {
                var col = GridCoulmns.FirstOrDefault(a => a.ColumnName == item.ColumnName);
                if (col != null)
                {
                    col.DefaultValue = item.DefaultValue;
                    col.isVisible = item.isVisible;
                    col.Pattern = item.Pattern;
                    col.Required = item.Required;
                    col.MinLength = item.MinLength;
                    col.Max = item.Max;
                    col.MaxLength = item.MaxLength;
                    col.Min = item.Min;
                    col.IsUnique = item.IsUnique;
                    col.UniqueColumns = item.UniqueColumns;

                    _hrUnitOfWork.PageEditorRepository.Attach(col);
                    _hrUnitOfWork.PageEditorRepository.Entry(col).State = EntityState.Modified;

                }
            }
        }
        public ActionResult ExistFileError(string Name)
        {
            bool Exist = false;
            string Path = Server.MapPath("/SpecialData/CurrentData/" + Name + ".xlsx");
            if (System.IO.File.Exists(Path))
                Exist = true;
            return Json(Exist, JsonRequestBehavior.AllowGet);
        }
        public Address AddPersonAddress(string AddressValue)
        {
            Address Addres = _hrUnitOfWork.Repository<Address>().FirstOrDefault(a => a.Address1 == AddressValue);
            if (Addres == null)
            {
                Addres = new Address() { Address1 = AddressValue };
                _hrUnitOfWork.CompanyRepository.Add(Addres);
            }
            return Addres;
        }
        #endregion

        #region ImportEmployees
        [HttpPost]
        public ActionResult SavePeopleFile(ExcelFileImports File, IEnumerable<ExcelGridPeopleViewModel> models)
        {

            var GeneralCol = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, File.OldObjectName, 0, Language).Where(a => a.type != "hidden" && a.type != "file" && a.type != "multiselect" && a.type != "label" && a.type != "button" && a.isVisible && (string.IsNullOrEmpty(a.HtmlAttribute) || !a.HtmlAttribute.Contains("readonly"))).ToList();
            var GeneralPPlCodes = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(GeneralCol.FirstOrDefault().PageId, Language);
            var EmployCol = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, "Emp", 0, Language).Where(a => a.type != "hidden" && a.type != "file" && a.type != "multiselect" && a.type != "label" && a.type != "button" && a.isVisible && (string.IsNullOrEmpty(a.HtmlAttribute) || !a.HtmlAttribute.Contains("readonly"))).ToList();
            var Code = EmployCol.Where(a => a.name == "Code").FirstOrDefault();
            var AssignCol = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, "AssignmentsForm", 0, Language).Where(a => a.type != "hidden" && a.type != "file" && a.type != "multiselect" && a.type != "label" && a.type != "button" && a.isVisible && (string.IsNullOrEmpty(a.HtmlAttribute) || !a.HtmlAttribute.Contains("readonly"))).ToList();
            var EmployCodes = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(EmployCol.FirstOrDefault().PageId, Language);
            var AssignCodes = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(AssignCol.FirstOrDefault().PageId, Language);
            GeneralCol.AddRange(EmployCol);
            GeneralCol.Remove(Code);
            GeneralCol.Insert(0, Code);
            AssignCol.Insert(0, Code);
            PeopleExportViewModel GridValues = AddCodes(GeneralPPlCodes, EmployCodes, AssignCodes);
            AddExcelList(GridValues);
            var ModelProps = models.FirstOrDefault().GetType().GetProperties();
            string Path = "~/SpecialData/CurrentData";
            return Json(SaveToExcelPublic(File, ReturnListData(models, File, GridValues, GeneralCol, AssignCol, ModelProps), GeneralCol, AssignCol, GridValues, Path), JsonRequestBehavior.AllowGet);
        }
        private void GeninfValid(ExcelGridPeopleViewModel model, int i, IList<Error> errors)
        {
            if (model.MaritalStat == null || model.MaritalStat.Value == 1 || model.MaritalStat.Value == 3)
            {
                model.TaxFamlyCnt = null;
                model.BnftFamlyCnt = null;
            }
            if (model.MilitaryNo == null || model.MaritalStat.Value != 1)
            {
                model.MilResDate = null;
                model.Rank = null;
                model.MilCertGrade = null;
            }
            if (model.SubscripDate == null)
            {
                model.VarSubAmt = null;
                model.BasicSubAmt = null;
            }
            model.BirthDate = model.BirthDate.Value;
            int _18Year = Convert.ToInt32((DateTime.Today.Subtract(model.BirthDate.Value).TotalDays) / 365);

            if (_18Year < 18 || DateTime.Today > model.BirthDate.Value.AddYears(60))
                errors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "BirthDate", message = MsgUtils.Instance.Trls("BirthDateMustBeInRangeAg") } }, row = short.Parse(i.ToString()) });
            if (model.ExpiryDate != null && model.IssueDate != null)
                if (model.ExpiryDate.Value < model.IssueDate.Value)
                    errors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "ExpiryDate", message = MsgUtils.Instance.Trls("IssueDateGrTExpiryDate") } }, row = short.Parse(i.ToString()) });

            if (model.StartExpDate != null && model.JoinDate != null)
                if (model.JoinDate.Value < model.StartExpDate.Value)
                    errors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "StartExpDate", message = MsgUtils.Instance.Trls("JoinDateGrTStartExpDate") } }, row = short.Parse(i.ToString()) });

        }
        private void EmploymentValidation(ExcelGridPeopleViewModel model, int i, IList<Error> errors)
        {
            var sysCode = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("PersonType", Language).Where(a => a.CodeId == model.PersonType).FirstOrDefault();
            if (sysCode != null)
            {
                if (sysCode.SysCodeId != 5)
                    model.EmpProfession = null;              
            }
            else
                model.EmpProfession = null;

            if (model.PersonType != 0)
            {
                if (model.EndDate != null)
                {

                    if (model.StartDate > model.EndDate.Value)
                        errors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "StartDate", message = MsgUtils.Instance.Trls("EndDateGrTStartDate") } }, row = short.Parse(i.ToString()) });
                    else
                    {
                        double Years = (model.EndDate.Value.Subtract(model.StartDate.Value).TotalDays / 365.25);
                        model.DurInYears = byte.Parse(Years.ToString().Split('.')[0]);
                        model.DurInMonths = Convert.ToByte((Years - model.DurInYears) * 12);
                    }
                }
                else
                {
                    if (model.DurInYears != 0 || model.DurInMonths != 0)
                        model.EndDate = model.StartDate.Value.AddYears(model.DurInYears).AddMonths(model.DurInMonths);
                }

            }
            else
                errors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "PersonType", message = MsgUtils.Instance.Trls("required") } }, row = short.Parse(i.ToString()) });
        }
        private void AssignValidation(ExcelGridPeopleViewModel model, int i, IList<Error> errors, List<AssignmentGridViewModel> assignments)
        {
            var ExistAssign = assignments.Where(a => a.EmpId == model.Id).FirstOrDefault();

            if (model.Id == 0 || ExistAssign == null)
                assignments.Add(new AssignmentGridViewModel() { IsDeptManger = model.IsDepManager, PositionId = model.PositionId, DepartmentId = model.DepartmentId });
            else
            {
                ExistAssign.IsDeptManger = model.IsDepManager;
                ExistAssign.DepartmentId = model.DepartmentId;
                ExistAssign.PositionId = model.PositionId;

            }
            var assignment = _hrUnitOfWork.Repository<Assignment>().Where(a => (a.DepartmentId == model.DepartmentId) && a.EmpId != model.Id && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today) && (a.CompanyId == CompanyId) && (a.IsDepManager)).Select(b => b).FirstOrDefault();
            if (assignment != null && model.IsDepManager)
                errors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "DepartmentId", message = MsgUtils.Instance.Trls("Already Taken before") } }, row = short.Parse(i.ToString()) });
            Position pos = _hrUnitOfWork.Repository<Position>().Where(a => a.Id == model.PositionId && a.CompanyId == CompanyId).FirstOrDefault();

            if (pos != null)
            {
                if (pos.JobId != model.JobId || pos.DeptId != model.DepartmentId)
                    errors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "PositionId", message = MsgUtils.Instance.Trls("NotinJoborDept") } }, row = short.Parse(i.ToString()) });

                if (pos.Headcount != null && pos.SysResponse == 2)
                {
                    var AssigCount = assignments.Where(a => a.PositionId == model.PositionId).Count();
                    if (AssigCount > pos.Headcount)
                        errors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "PositionId", message = MsgUtils.Instance.Trls("OverHeadCount") } }, row = short.Parse(i.ToString()) });
                }
            }
        }
        private void AddOrUpdatePerson(Person per, ExcelGridPeopleViewModel Item, string Type,OptionsViewModel options)
        {
            string name = Item.Title + " " + Item.FirstName + " " + Item.Familyname;
            string oldName = per.Title + " " + per.FirstName + " " + per.Familyname;
            _hrUnitOfWork.PositionRepository.AddLName(Language, oldName, name, Item.localName);
            AutoMapper(new AutoMapperParm()
            {
                ObjectName = "People",
                Destination = per,
                Source = Item,
                Transtype = Type == "Insert" ? TransType.Insert : TransType.Update,
                Options = options

            });
            per.BirthDate = Item.BirthDate.Value;
            per.Gender = Item.Gender.Value;
            if (Item.Address != null && Item.HostAddress != null && Item.Address.Equals(Item.HostAddress))
            {
                per.Address = AddPersonAddress(Item.Address);
                per.HoAddress = per.Address;
            }else {
                if (Item.Address != null && Item.AddressId == null)
                    per.Address = AddPersonAddress(Item.Address);

                if (Item.HostAddress != null && Item.HoAddressId == null)
                    per.HoAddress = AddPersonAddress(Item.HostAddress);
            }

            if (per.Id != 0)
            {
                per.ModifiedTime = DateTime.Now;
                per.ModifiedUser = UserName;
            }
            else
            {
                per.CreatedTime = DateTime.Now;
                per.CreatedUser = UserName;
            }
        }
        private void AddOrUpdateEmployment(Employement emp, ExcelGridPeopleViewModel item, Person per, List<Error> Emperrors,OptionsViewModel options ,int i, string v)
        {
            string message = "OK";
            AutoMapper(new AutoMapperParm()
            {
                ObjectName = "Emp",
                Destination = emp,
                Source = item,
                Transtype = v == "Insert" ? TransType.Insert : TransType.Update,
                Id = "EmpId",
                Options = options
            });
            emp.StartDate = item.StartDate.Value;
            emp.PersonType = item.PersonType.Value;
            emp.ContIssueDate = item.ContIssueDate == null ? DateTime.Now :item.ContIssueDate.Value;
            item.Id = emp.EmpId;
            emp.Profession = item.EmpProfession;
            emp.Person = per;
            emp.CompanyId = CompanyId;
            if (v == "Insert")
            {
                emp.CreatedTime = DateTime.Now;
                emp.CreatedUser = UserName;
                emp.Status = 1;
            }
            else
            {
                message = _hrUnitOfWork.PeopleRepository.CheckCode(emp, Language);
                if (message != "OK")
                    Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "Code", message = message } }, row = short.Parse(i.ToString()) });
                emp.ModifiedTime = DateTime.Now;
                emp.ModifiedUser = UserName;
            }

        }
        private void AddOrUpdateAssignment(ExcelGridPeopleViewModel item, Person per, Employement Emp, List<Error> Emperrors,OptionsViewModel options ,int i)
        {
            Assignment Assign;
            bool chkEmployment = _hrUnitOfWork.EmployeeRepository.CheckEmployment(Emp, item.Id,item.AssignDate);
         short sysAssign  = _hrUnitOfWork.Repository<LookUpUserCode>().Where(a => a.CodeName == "Assignment" && a.CodeId == item.AssignStatus).FirstOrDefault().SysCodeId;
            if (!chkEmployment)
                Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "AssignDate", message = MsgUtils.Instance.Trls("haventcontract") } }, row = short.Parse(i.ToString()) });
            else
            {
                var currentAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == item.Id && (a.AssignDate <= DateTime.Today && a.EndDate >= DateTime.Today && a.CompanyId == CompanyId)).FirstOrDefault();
                DateTime EndDate = (currentAssignment != null ? currentAssignment.EndDate : new DateTime(2099, 1, 1));

                if (item.Id == 0 || currentAssignment == null)
                {
                    Assign = new Assignment();
                    item.Id = Assign.Id;
                    AutoMapper(new AutoMapperParm()
                    {
                        ObjectName = "AssignmentsForm",
                        Destination = Assign,
                        Source = item,
                        Transtype = TransType.Insert,
                        Id = "EmpId",
                        Options = options

                    });

                    item.Id = Assign.EmpId;
                    Assign.LocationId = item.AssignLocation;
                    Assign.Employee = per;
                    Assign.EndDate = EndDate;
                    Assign.CompanyId = CompanyId;
                    Assign.CreatedTime = DateTime.Now;
                    Assign.SysAssignStatus = sysAssign;
                    Assign.CreatedUser = UserName;
                    _hrUnitOfWork.EmployeeRepository.Add(Assign);

                }
                else
                {
                    var oldAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.Id != currentAssignment.Id && a.EmpId == item.Id && a.AssignDate < item.AssignDate).OrderByDescending(c => c.AssignDate).FirstOrDefault();
                    var futureAssignment = _hrUnitOfWork.Repository<Assignment>().Where(a => a.Id != currentAssignment.Id && a.EmpId == item.Id && a.AssignDate > item.AssignDate).OrderBy(c => c.AssignDate).FirstOrDefault();
                    var assign = _hrUnitOfWork.Repository<Assignment>().Where(a => a.EmpId == item.Id).FirstOrDefault();
                    if (oldAssignment != null && oldAssignment.EndDate >= item.AssignDate)
                        Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "AssignDate", message = MsgUtils.Instance.Trls("foundoldAssignment") } }, row = short.Parse(i.ToString()) });
                    if (futureAssignment != null && futureAssignment.AssignDate <= item.AssignDate)
                        Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "AssignDate", message = MsgUtils.Instance.Trls("foundFutureAssignment") } }, row = short.Parse(i.ToString()) });
                    if (oldAssignment != null) // Edit
                    {
                        if (currentAssignment.AssignDate != item.AssignDate)
                        {
                            oldAssignment.ModifiedTime = DateTime.Now;
                            oldAssignment.ModifiedUser = UserName;
                            oldAssignment.EndDate = item.AssignDate.AddDays(-1);
                            _hrUnitOfWork.EmployeeRepository.Attach(oldAssignment);
                            _hrUnitOfWork.EmployeeRepository.Entry(oldAssignment).State = EntityState.Modified;
                        }
                    }
                    item.Id = currentAssignment.Id;
                    AutoMapper(new AutoMapperParm
                    {
                        Destination = currentAssignment,
                        Source = item,
                        ObjectName = "AssignmentsForm",
                        Version = 0,
                        Id = "EmpId",
                        Transtype = TransType.Update,
                        Options = options

                    });
                    item.Id = currentAssignment.EmpId;
                    currentAssignment.ModifiedTime = DateTime.Now;
                    currentAssignment.ModifiedUser = UserName;
                    currentAssignment.EndDate = EndDate;
                    currentAssignment.SysAssignStatus = sysAssign;
                    if (currentAssignment.PositionId != null)
                        currentAssignment.ManagerId = null;
                    _hrUnitOfWork.EmployeeRepository.Attach(currentAssignment);
                    _hrUnitOfWork.EmployeeRepository.Entry(currentAssignment).State = EntityState.Modified;

                }
            }

        }
        private void AddAssignmentError(ExcelGridPeopleViewModel item, List<Error> Emperrors, int i)
        {

            string message = MsgUtils.Instance.Trls("Required");
            if (item.DepartmentId == 0)
                Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "DepartmentId", message = message } }, row = short.Parse(i.ToString()) });
            if (item.JobId == 0)
                Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "JobId", message = message } }, row = short.Parse(i.ToString()) });
            if (item.AssignStatus == 0)
                Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "AssignStatus", message = message } }, row = short.Parse(i.ToString()) });
            if (item.AssignDate == new DateTime())
                Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "AssignDate", message = message } }, row = short.Parse(i.ToString()) });

        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> SaveImportPeople(IEnumerable<ExcelGridPeopleViewModel> models, OptionsViewModel options)
        {

            DataSource<ExcelGridPeopleViewModel> dataSource = new DataSource<ExcelGridPeopleViewModel>();
            dataSource.Errors = new List<Error>();
            List<Error> Emperrors;
            List<ExcelGridPeopleViewModel> data = new List<ExcelGridPeopleViewModel>();
            List<Error> Servererrors = new List<Error>();
            IdentityResult result;

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    Servererrors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "ImportPeople",
                        TableName = "People",
                        Columns = Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });
                }

                var Assignments = _hrUnitOfWork.EmployeeRepository.GetAssignments(Language).Where(a => a.CompanyId == CompanyId).ToList();

                for (var i = 0; i < models.Count(); i++)
                {
                    Error RecordError = Servererrors.FirstOrDefault(a => a.row == i);
                    ExcelGridPeopleViewModel Item = models.ElementAtOrDefault(i);

                    if (RecordError == null)
                    {
                        Emperrors = new List<Error>();

                        GeninfValid(Item, i, Emperrors);

                        Person per = _hrUnitOfWork.Repository<Person>().FirstOrDefault(a => a.FirstName == Item.FirstName && a.Fathername == Item.Fathername && a.GFathername == Item.GFathername && a.Familyname == Item.Familyname);

                        Employement Emp = _hrUnitOfWork.Repository<Employement>().Where(a => a.Code == Item.Code && a.Status == 1 && a.CompanyId == CompanyId).FirstOrDefault();
                        if (Item.Gender == 0)
                            Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "Gender", message = MsgUtils.Instance.Trls("Required") } }, row = short.Parse(i.ToString()) });
                        if (Item.QualificationId == 0)
                            Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "QualificationId", message = MsgUtils.Instance.Trls("Required") } }, row = short.Parse(i.ToString()) });
                        if (Item.PersonType == 0)
                            Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "PersonType", message = MsgUtils.Instance.Trls("Required") } }, row = short.Parse(i.ToString()) });
                        if (Item.StartDate == new DateTime())
                            Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "StartDate", message = MsgUtils.Instance.Trls("Required") } }, row = short.Parse(i.ToString()) });

                        if (Emp == null)
                        {
                            if (per != null)
                                Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "FirstName", message = MsgUtils.Instance.Trls("AlreadyExists") } }, row = short.Parse(i.ToString()) });
                            Item.Id = 0;
                        }
                        else
                        {

                            if (per != null && Emp.EmpId != per.Id)
                                Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "FirstName", message = MsgUtils.Instance.Trls("AlreadyExists") } }, row = short.Parse(i.ToString()) });

                            Item.Id = Emp.EmpId;

                        }

                        if (Emperrors.Count == 0)
                        {
                            if (Item.Id == 0)
                            {
                                if (per == null)
                                    per = new Person();
                                AddOrUpdatePerson(per, Item, "Insert", options);
                                _hrUnitOfWork.PeopleRepository.Add(per);

                                if (!string.IsNullOrEmpty(Item.Code) && Emperrors.Count == 0)
                                {
                                    EmploymentValidation(Item, i, Emperrors);

                                    if (Emperrors.Count == 0)
                                    {
                                        Emp = new Employement();
                                        Item.Id = Emp.Id;
                                        AddOrUpdateEmployment(Emp, Item, per, Emperrors, options, i, "Insert");

                                        _hrUnitOfWork.PeopleRepository.Add(Emp);

                                        if (Item.DepartmentId != 0 && Item.JobId != 0 && Item.AssignStatus != 0 && Item.AssignDate != new DateTime())
                                        {
                                            AssignValidation(Item, i, Emperrors, Assignments);
                                            if (Emperrors.Count == 0)
                                                AddOrUpdateAssignment(Item, per, Emp, Emperrors, options, i);
                                        }
                                        else if (Item.DepartmentId != 0 || Item.JobId != 0 || Item.AssignStatus != 0 || Item.AssignDate != new DateTime() || Item.AssignError)
                                            AddAssignmentError(Item, Emperrors, i);
                                    }
                                }
                            }
                            else
                            {
                                per = _hrUnitOfWork.Repository<Person>().FirstOrDefault(a => a.Id == Item.Id);
                                AddOrUpdatePerson(per, Item, "Update", options);

                                _hrUnitOfWork.PeopleRepository.Attach(per);
                                _hrUnitOfWork.PeopleRepository.Entry(per).State = EntityState.Modified;

                                EmploymentValidation(Item, i, Emperrors);
                                Emp = _hrUnitOfWork.Repository<Employement>().Where(a => a.EmpId == Item.Id && a.Status == 1 && a.CompanyId == CompanyId).OrderByDescending(a => a.StartDate).FirstOrDefault();

                                if (Emperrors.Count == 0)
                                {
                                    if (Emp == null)
                                        Emp = new Employement();
                                    else
                                    {
                                        var PreviousRecord = _hrUnitOfWork.Repository<Employement>().Where(a => a.EmpId == Emp.EmpId && a.Status != 1 && a.CompanyId == CompanyId).LastOrDefault();
                                        var date = (PreviousRecord != null ? PreviousRecord.EndDate : new DateTime(1900, 1, 2));
                                        var assignment = _hrUnitOfWork.Repository<Assignment>().Where(a => (a.AssignDate >= Emp.StartDate && a.AssignDate <= Emp.EndDate) && a.EmpId == Emp.EmpId).FirstOrDefault();

                                        if (Item.StartDate > date)
                                        {
                                            if (assignment != null && Item.EndDate <= assignment.AssignDate)
                                                Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "EndDate", message = MsgUtils.Instance.Trls("cantcorrectemployment") } }, row = short.Parse(i.ToString()) });

                                            if (assignment != null && Item.StartDate > assignment.AssignDate)
                                                Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "StartDate", message = MsgUtils.Instance.Trls("cantcorrectStartemploy") } }, row = short.Parse(i.ToString()) });

                                            Item.Id = Emp.Id;
                                            AddOrUpdateEmployment(Emp, Item, per, Emperrors, options, i, Emp.Id == 0 ? "Insert" : "Update");
                                            Item.Id = Emp.EmpId;

                                            if (PreviousRecord != null)
                                            {
                                                if (Item.StartDate < PreviousRecord.EndDate)
                                                {
                                                    PreviousRecord.EndDate = Item.StartDate.Value.AddDays(-1);
                                                    _hrUnitOfWork.PeopleRepository.Attach(PreviousRecord);
                                                    _hrUnitOfWork.PeopleRepository.Entry(PreviousRecord).State = EntityState.Modified;
                                                }
                                            }
                                        }
                                        else
                                            Emperrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "StartDate", message = MsgUtils.Instance.Trls("StartMustGrtThanPreviousStart") } }, row = short.Parse(i.ToString()) });
                                    }

                                    _hrUnitOfWork.PeopleRepository.Attach(Emp);
                                    _hrUnitOfWork.PeopleRepository.Entry(Emp).State = EntityState.Modified;

                                    if (Item.DepartmentId != 0 && Item.JobId != 0 && Item.AssignStatus != 0 && Item.AssignDate != new DateTime())
                                    {
                                        AssignValidation(Item, i, Emperrors, Assignments);
                                        AddOrUpdateAssignment(Item, per, Emp, Emperrors, options, i);
                                    }
                                    else if (Item.DepartmentId != 0 || Item.JobId != 0 || Item.AssignStatus != 0 || Item.AssignDate != new DateTime() || Item.AssignError)
                                        AddAssignmentError(Item, Emperrors, i);
                                }
                            }
                        }
                        if (Emperrors.Count == 0)
                        {
                            var error = SaveChanges(Language);
                            Emperrors.AddRange(error);
                        }

                        if (Emperrors.Count > 0)
                            data.Add(Item);
                        else
                        {
                            var users = db.Users.Where(a => a.EmpId == Item.Id).ToList();
                            foreach (var item in users)
                            {
                                item.Email = Item.WorkEmail;
                                item.PhoneNumber = Item.WorkTel;
                                result = await _userManager.UpdateAsync(item);
                                if (result.Errors.Count() > 0)
                                    Emperrors.Add(new Error() { errors = new List<ErrorMessage>(new List<ErrorMessage>() { new ErrorMessage() { message = result.Errors.FirstOrDefault() } }) });
                            }
                        }

                        for (int j = 0; j < Emperrors.Count; j++)
                        {
                            Emperrors[j].row = (short)data.IndexOf(Item);
                            string msg = Emperrors[j].errors.FirstOrDefault().message;
                            if (msg.ToLower().IndexOf("address") >= 0)
                            {
                                if (Item.Address.Length > 500)
                                    Emperrors[j].errors.FirstOrDefault().field = "Address";
                                else
                                    Emperrors[j].errors.FirstOrDefault().field = "HostAddress";
                                Emperrors[j].errors.FirstOrDefault().message = MsgUtils.Instance.Trls("AddressError");
                            }
                            dataSource.Errors.Add(Emperrors[j]);
                        }

                        _hrUnitOfWork.EmployeeRepository.RemoveContext();
                    }
                    else
                    {
                        data.Add(Item);
                        RecordError.row = (short)(data.Count - 1);
                        dataSource.Errors.Add(RecordError);
                    }
                }
            }
            else
            {
                dataSource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                dataSource.Data = models;
                return Json(dataSource);
            }
            dataSource.Data = data;
            dataSource.Total = data.Count();
            return Json(dataSource);
        }
        #endregion

        #region ImportJobs
        [HttpPost]
        public ActionResult SaveJobFile(ExcelFileImports File, IEnumerable<ExcelGridJobViewModel> models)
        {
            var GeneralCol = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, File.OldObjectName, 0, Language).Where(a => a.type != "hidden" && a.type != "file" && a.type != "multiselect" && a.type != "label" && a.type != "button" && a.isVisible && (string.IsNullOrEmpty(a.HtmlAttribute) || !a.HtmlAttribute.Contains("readonly"))).ToList();
            var GeneralPPlCodes = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(GeneralCol.FirstOrDefault().PageId, Language);

            PeopleExportViewModel GridValues = AddCodes(GeneralPPlCodes, null, null);
            AddExcelList(GridValues);
            var ModelProps = models.FirstOrDefault().GetType().GetProperties();
            string Path = "~/SpecialData/CurrentData";
            return Json(SaveToExcelPublic(File, ReturnListData(models, File, GridValues, GeneralCol, null, ModelProps), GeneralCol, null, GridValues, Path), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveImportJob(IEnumerable<ExcelGridJobViewModel> models, OptionsViewModel options)
        {
            DataSource<ExcelGridJobViewModel> dataSource = new DataSource<ExcelGridJobViewModel>();
            dataSource.Errors = new List<Error>();
            List<Error> Joberrors;
            List<ExcelGridJobViewModel> data = new List<ExcelGridJobViewModel>();
            List<Error> Servererrors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    Servererrors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "ImportJob",
                        TableName = "Jobs",
                        Columns = Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                }

                for (var i = 0; i < models.Count(); i++)
                {
                    ExcelGridJobViewModel Item = models.ElementAtOrDefault(i);
                    Error RecordError = Servererrors.FirstOrDefault(a => a.row == i);
                    if (RecordError == null)
                    {
                        Joberrors = new List<Error>();
                        var record = _hrUnitOfWork.Repository<Job>().Where(a => a.CompanyId == CompanyId && a.Code == Item.Code).FirstOrDefault();
                        if (record == null)
                        {
                            record = new Job();
                            Item.Id = 0;
                            _hrUnitOfWork.JobRepository.AddLName(Language, record.Name, Item.Name, Item.LName);
                            AutoMapper(new Models.AutoMapperParm
                            {
                                Destination = record,
                                Source = Item,
                                ObjectName = "Job",
                                Version = Convert.ToByte(Request.Form["Version"]),
                                Transtype = TransType.Insert,
                                Options = options
                            });

                            record.CreatedTime = DateTime.Now;
                            record.CreatedUser = UserName;
                            record.CompanyId = Item.IsLocal ? CompanyId : (int?)null;
                            _hrUnitOfWork.JobRepository.Add(record);
                        }
                        else
                        {
                            Item.Id = record.Id;
                            _hrUnitOfWork.JobRepository.AddLName(Language, record.Name, Item.Name, Item.LName);
                            Item.CompanyId = record.CompanyId;
                            AutoMapper(new Models.AutoMapperParm
                            {
                                Destination = record,
                                Source = Item,
                                ObjectName = "Job",
                                Version = Convert.ToByte(Request.Form["Version"]),
                                Transtype = TransType.Update,
                                Options = options
                            });



                            record.ModifiedTime = DateTime.Now;
                            record.ModifiedUser = UserName;
                            record.CompanyId = Item.IsLocal ? CompanyId : (int?)null;
                            _hrUnitOfWork.JobRepository.Attach(record);
                            _hrUnitOfWork.JobRepository.Entry(record).State = EntityState.Modified;
                        }

                        var error = SaveChanges(Language);
                        Joberrors.AddRange(error);


                        if (Joberrors.Count > 0)
                            data.Add(Item);
                        for (int j = 0; j < Joberrors.Count; j++)
                        {
                            Joberrors[j].row = (short)data.IndexOf(Item);
                            dataSource.Errors.Add(Joberrors[j]);
                        }

                        _hrUnitOfWork.EmployeeRepository.RemoveContext();


                    }else
                    {
                        data.Add(Item);
                        RecordError.row = (short)(data.Count - 1);
                        dataSource.Errors.Add(RecordError);
                    }
                }
            }
            else
            {
                dataSource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                dataSource.Data = models;
                return Json(dataSource);
            }
            dataSource.Data = data;
            dataSource.Total = data.Count();
            return Json(dataSource);
        }
        #endregion

        #region Locations
        [HttpPost]
        public ActionResult SaveFormLocationFile(ExcelFileImports File, IEnumerable<ExcelFormLocationViewModel> models)
        {
            var GeneralCol = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, File.OldObjectName, 0, Language).Where(a => a.type != "hidden" && a.type != "file" && a.type != "multiselect" && a.type != "label" && a.type != "button" && a.isVisible && (string.IsNullOrEmpty(a.HtmlAttribute) || !a.HtmlAttribute.Contains("readonly"))).ToList();
            var GeneralPPlCodes = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(GeneralCol.FirstOrDefault().PageId, Language);

            PeopleExportViewModel GridValues = AddCodes(GeneralPPlCodes, null, null);
            AddExcelList(GridValues);
            var ModelProps = models.FirstOrDefault().GetType().GetProperties();
            string Path = "~/SpecialData/CurrentData";
            return Json(SaveToExcelPublic(File, ReturnListData(models, File, GridValues, GeneralCol, null, ModelProps), GeneralCol, null, GridValues, Path), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveImportFormLocation(IEnumerable<ExcelGridFormLocationViewModel> models, OptionsViewModel options)
        {
            DataSource<ExcelGridFormLocationViewModel> dataSource = new DataSource<ExcelGridFormLocationViewModel>();
            dataSource.Errors = new List<Error>();
            List<Error> Joberrors;
            List<ExcelGridFormLocationViewModel> data = new List<ExcelGridFormLocationViewModel>();
            List<Error> Servererrors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    Servererrors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "ImportFormLocation",
                        TableName = "Locations",
                        Columns = Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });
                }


                for (var i = 0; i < models.Count(); i++)
                {

                    ExcelGridFormLocationViewModel Item = models.ElementAtOrDefault(i);
                    Error RecordError = Servererrors.FirstOrDefault(a => a.row == i);
                    if (RecordError == null)
                    {
                        Joberrors = new List<Error>();
                        var loc = _hrUnitOfWork.Repository<Location>().Where(a => a.CompanyId == CompanyId && a.Code == Item.Code).FirstOrDefault();
                        if (loc == null) // New
                        {
                            loc = new Location();
                            Item.Id = 0;
                            _hrUnitOfWork.LocationRepository.AddLName(Language, loc.Name, Item.Name, Item.LName);

                            AutoMapper(new Models.AutoMapperParm
                            {
                                Destination = loc,
                                Source = Item,
                                ObjectName = "FormLocation",
                                Version = Convert.ToByte(Request.Form["Version"]),
                                Options = options,
                                Transtype = TransType.Insert
                            });
                            loc.CreatedTime = DateTime.Now;
                            loc.CreatedUser = UserName;
                            _hrUnitOfWork.LocationRepository.Add(loc);

                        }
                        else // Edit
                        {
                            Item.Id = loc.Id;
                            AutoMapper(new Models.AutoMapperParm
                            {
                                Destination = loc,
                                Source = Item,
                                ObjectName = "FormLocation",
                                Version = Convert.ToByte(Request.Form["Version"]),
                                Options = options,
                                Transtype = TransType.Update
                            });

                            loc.ModifiedTime = DateTime.Now;
                            loc.ModifiedUser = UserName;
                            _hrUnitOfWork.LocationRepository.Attach(loc);
                            _hrUnitOfWork.LocationRepository.Entry(loc).State = EntityState.Modified;
                            _hrUnitOfWork.LocationRepository.AddLName(Language, loc.Name, Item.Name, Item.LName);

                        }
                        var error = SaveChanges(Language);
                        Joberrors.AddRange(error);

                        if (Joberrors.Count > 0)
                            data.Add(Item);

                        for (int j = 0; j < Joberrors.Count; j++)
                        {
                            Joberrors[j].row = (short)data.IndexOf(Item);
                            dataSource.Errors.Add(Joberrors[j]);
                        }

                        _hrUnitOfWork.EmployeeRepository.RemoveContext();
                    }else
                    {
                        data.Add(Item);
                        RecordError.row = (short)(data.Count - 1);
                        dataSource.Errors.Add(RecordError);
                    }
                }

            }
            else
            {
                dataSource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                dataSource.Data = models;
                return Json(dataSource);
            }
            dataSource.Data = data;
            dataSource.Total = data.Count();
            return Json(dataSource);
        }
        #endregion

        #region Custody
        [HttpPost]
        public ActionResult SaveCustodyFile(ExcelFileImports File, IEnumerable<ExcelCustodyViewModel> models)
        {
            var GeneralCol = _hrUnitOfWork.PagesRepository.GetFormColumnInfo(CompanyId, File.OldObjectName, 0, Language).Where(a => a.type != "hidden" && a.type != "file" && a.type != "multiselect" && a.type != "label" && a.type != "button" && a.isVisible && (string.IsNullOrEmpty(a.HtmlAttribute) || !a.HtmlAttribute.Contains("readonly"))).ToList();
            var GeneralPPlCodes = _hrUnitOfWork.PagesRepository.GetFormLookUpCodes(GeneralCol.FirstOrDefault().PageId, Language);

            PeopleExportViewModel GridValues = AddCodes(GeneralPPlCodes, null, null);
            AddExcelList(GridValues);
            var ModelProps = models.FirstOrDefault().GetType().GetProperties();
            string Path = "~/SpecialData/CurrentData";
            return Json(SaveToExcelPublic(File, ReturnListData(models, File, GridValues, GeneralCol, null, ModelProps), GeneralCol, null, GridValues, Path), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveImportCustody(IEnumerable<ExcelGridCustodyViewModel> models, OptionsViewModel options)
        {
            DataSource<ExcelGridCustodyViewModel> dataSource = new DataSource<ExcelGridCustodyViewModel>();
            dataSource.Errors = new List<Error>();
            List<Error> Custodyerrors;
            List<ExcelGridCustodyViewModel> data = new List<ExcelGridCustodyViewModel>();
            List<Error> Servererrors = new List<Error>();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    Servererrors = _hrUnitOfWork.LookUpRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "ImportCustody",
                        TableName = "Custody",
                        Columns = Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });
                }


                for (var i = 0; i < models.Count(); i++)
                {

                    ExcelGridCustodyViewModel Item = models.ElementAtOrDefault(i);
                    Error RecordError = Servererrors.FirstOrDefault(a => a.row == i);
                    if (RecordError == null)
                    {
                        Custodyerrors = new List<Error>();
                        if (Item.CustodyCatId == 0)
                            Custodyerrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "CustodyCatId", message = MsgUtils.Instance.Trls("Required") } }, row = short.Parse(i.ToString()) });
                        if (Item.JobId == 0)
                            Custodyerrors.Add(new Error { errors = new List<ErrorMessage> { new ErrorMessage { field = "JobId", message = MsgUtils.Instance.Trls("Required") } }, row = short.Parse(i.ToString()) });

                        if (Custodyerrors.Count == 0)
                        {
                            var CustodyCategory = _hrUnitOfWork.Repository<CustodyCat>().Where(a => a.Id == Item.CustodyCatId).Select(s => new { prefix = s.Prefix, CodeLen = s.CodeLength == 0 ? 4 : s.CodeLength }).FirstOrDefault();

                            var custody = _hrUnitOfWork.Repository<Custody>().Where(a => a.CompanyId == CompanyId && a.Code == Item.Code).FirstOrDefault();
                            if (custody == null) // New
                            {
                                custody = new Custody();
                                Item.Id = 0;
                                //  _hrUnitOfWork.LocationRepository.AddLName(Language, custody.Name, model.Name, model.LName);

                                AutoMapper(new Models.AutoMapperParm
                                {
                                    Destination = custody,
                                    Source = Item,
                                    ObjectName = "Custody",
                                    Version = Convert.ToByte(Request.Form["Version"]),
                                    Options = options,
                                    Transtype = TransType.Insert
                                });
                                custody.Qty = 1;
                                custody.InUse = false;
                                custody.CompanyId = CompanyId;
                                custody.CustodyCatId = Item.CustodyCatId;
                                custody.CreatedTime = DateTime.Now;
                                custody.CreatedUser = UserName;
                                if (CustodyCategory.prefix == null)
                                {
                                    var GenralSeq = _hrUnitOfWork.Repository<Custody>().Where(a => a.CompanyId == CompanyId).Select(a => a.Sequence).DefaultIfEmpty(0).Max();
                                    var sequence = GenralSeq != 0 ? GenralSeq + 1 : 1;
                                    custody.Sequence = sequence;
                                    custody.Code = sequence.ToString();
                                }
                                else
                                {
                                    var CategorySeq = _hrUnitOfWork.Repository<Custody>().Where(a => a.CustodyCatId == Item.CustodyCatId).Select(a => a.Sequence).DefaultIfEmpty(0).Max();
                                    var sequence = CategorySeq != 0 ? CategorySeq + 1 : 1;
                                    custody.Sequence = sequence;
                                    custody.Code = CustodyCategory.prefix + sequence.ToString().PadLeft((int)CustodyCategory.CodeLen, '0');
                                }
                                _hrUnitOfWork.CustodyRepository.Add(custody);

                            }
                            else // Edit
                            {
                                Item.Id = custody.Id;

                                AutoMapper(new Models.AutoMapperParm
                                {
                                    Destination = custody,
                                    Source = Item,
                                    ObjectName = "Custody",
                                    Version = Convert.ToByte(Request.Form["Version"]),
                                    Options = options,
                                    Transtype = TransType.Update
                                });
                                custody.CustodyCatId = Item.CustodyCatId;
                                custody.ModifiedTime = DateTime.Now;
                                custody.ModifiedUser = UserName;
                                custody.Qty = 1;
                                custody.InUse = false;
                                custody.CompanyId = CompanyId;
                                _hrUnitOfWork.CustodyRepository.Attach(custody);
                                _hrUnitOfWork.CustodyRepository.Entry(custody).State = EntityState.Modified;

                            }

                            var error = SaveChanges(Language);
                            Custodyerrors.AddRange(error);

                            if (Custodyerrors.Count > 0)
                                data.Add(Item);

                            for (int j = 0; j < Custodyerrors.Count; j++)
                            {
                                Custodyerrors[j].row = (short)data.IndexOf(Item);
                                dataSource.Errors.Add(Custodyerrors[j]);
                            }

                            _hrUnitOfWork.EmployeeRepository.RemoveContext();
                        }
                        else
                        {
                            data.Add(Item);
                            for (int x = 0; x < Custodyerrors.Count; x++)
                                dataSource.Errors.Add(Custodyerrors[x]);
                        }
                    }
                    else
                    {
                        data.Add(Item);
                        RecordError.row = (short)(data.Count - 1);
                        dataSource.Errors.Add(RecordError);
                    }
                }

            }
            else
            {
                dataSource.Errors = Models.Utils.ParseErrors(ModelState.Values);
                dataSource.Data = models;
                return Json(dataSource);
            }
            dataSource.Data = data;
            dataSource.Total = data.Count();
            return Json(dataSource);
        }
        #endregion
    }
}