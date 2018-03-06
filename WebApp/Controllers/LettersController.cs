using Interface.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.ViewModel.Personnel;
using System.IO;
using Model.Domain;
using Model.ViewModel;
using WebApp.Extensions;
using System.Data.Entity;
using WebApp.Models;
using System.Web.Routing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Model.ViewModel.Administration;
using System.Diagnostics;

namespace WebApp.Controllers
{
    public class LettersController : BaseController
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
        public LettersController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region Letters
        // GET: Letters
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetLettersData()
        {
            var let = _hrUnitOfWork.HrLettersRepository.GetAll().Select(a => new HrlettersViewModel
            {
               
                Id = a.Id,
                LetterTempl = a.LetterTempl,             
                Name = a.Name
            });
            return Json(let, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateLetter(IEnumerable<HrlettersViewModel> models)
        {
            var result = new List<HRLetter>();
            var datasource = new DataSource<HrlettersViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "HrLetters",
                        Columns = Models.Utils.GetModifiedRows(ModelState),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }


                foreach (HrlettersViewModel model in models)
                {

                    var Letter = new HRLetter
                    {
                        Name = model.Name,
                        LetterTempl = model.LetterTempl,
                        CreatedTime = DateTime.Now,
                        CreatedUser = UserName
                    };
                    if (model.LetterTempl != null)
                        SaveFile(Letter.Name, model.LetterTempl);
                    result.Add(Letter);
                    _hrUnitOfWork.HrLettersRepository.Add(Letter);
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
                               select new HrlettersViewModel
                               {
                                   Id = (r == null ? 0 : r.Id),
                                   Name = m.Name,
                                   LetterTempl = m.LetterTempl
                               }).ToList();

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }
        public ActionResult UpdateLetter(IEnumerable<HrlettersViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<HrlettersViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "HrLetters",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Id);
                var db_HrLetter = _hrUnitOfWork.Repository<HRLetter>().Where(a => ids.Contains(a.Id)).ToList();

                for (var i = 0; i < models.Count(); i++)
                {
                    var Letter = db_HrLetter.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    var item = models.ElementAtOrDefault(i);
                    AutoMapper(new AutoMapperParm() { ObjectName = "HrLetters", Destination = Letter, Source = item, Version = 0, Options = options.ElementAtOrDefault(i), Transtype = TransType.Update });
                    Letter.ModifiedTime = DateTime.Now;
                    Letter.ModifiedUser = UserName;
                    if (item.LetterTempl != null)
                        SaveFile(item.Name, item.LetterTempl);
                    _hrUnitOfWork.HrLettersRepository.Attach(Letter);
                    _hrUnitOfWork.HrLettersRepository.Entry(Letter).State = EntityState.Modified;
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
        public ActionResult DeleteLetter(int id)
        {
            var datasource = new DataSource<HrlettersViewModel>();

            var Letter = _hrUnitOfWork.Repository<HRLetter>().FirstOrDefault(a => a.Id == id);

            AutoMapper(new Models.AutoMapperParm
            {
                Source = Letter,
                ObjectName = "HrLetters",
                Transtype = TransType.Delete
            });

            _hrUnitOfWork.HrLettersRepository.Remove(Letter);


            datasource.Errors = SaveChanges(Language);



            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
            {
                DeleteFiles(Letter);
                return Json(datasource);
            }
        }
        void SaveFile(string Name, string Temp)
        {
           
            string Path = Server.MapPath("/SpecialData/HrLetters/Uploaded/"+ Temp);
            string NewPath = Server.MapPath("/SpecialData/HrLetters/Uploaded/" +Name + "_" + Temp);
            if (System.IO.File.Exists(Path))
            {
                if (System.IO.File.Exists(NewPath))
                    System.IO.File.Delete(NewPath);

                System.IO.File.Move(Path, NewPath);

            }
               

        }
        void DeleteFiles(HRLetter model)
        {
            string Path = Server.MapPath("/SpecialData/HrLetters/Uploaded");

            string Name = model.Name + "_" + model.LetterTempl;
            if (System.IO.File.Exists(Path.Insert(Path.Length, "/" + Name)))
                System.IO.File.Delete(Path.Insert(Path.Length, "/" + Name));

        }
        public ActionResult LettersFiles()
        {
            var result = new List<ChartViewModel>();
            string pathName = Server.MapPath(@"../SpecialData/HrLetters");
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
                        color = "/SpecialData/HrLetters/" + fl.Name
                    };
                    result.Add(f);
                }

            }
            return PartialView("_ContractFiles", result);
        }
        public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> files, string id)
        {

            string Path = Server.MapPath("/SpecialData/HrLetters/Uploaded");

            string Name = "";
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);


            if (id != null || files.Count() != 0)
            {
                var File = files.FirstOrDefault();

                Name = File.FileName.Split('.')[0] + ".docx";
                File.SaveAs(Path.Insert(Path.Length, "/" + Name));

            }
            return Json(new { FileName = Name }, "text/plain");
        }
        #endregion

        #region MailMerge
        [HttpPost]
        public ActionResult MergeLetters(int EmpId, string FileName)
        {
            MailMergeViewmodel mail = new MailMergeViewmodel();
            List<string> ArryList;
            if (User.Identity.RTL())
                ArryList = new List<string>() { "اسم_الموظف", "الرقم_القومى", "تاريخ_اصدار_البطاقة", "عنوان_الموظف", "رقم_جواز_السفر", "تاريخ_اصدار_الجواز", "AddressingNo", "مدة_العقد", "تاريخ_بداية_التعيين", "«تاريخ_نهاية_التعيين", "الوظيفة", "الراتب", "البدلات", "العملة", "عدد_تذاكر_السفر", "قيمة_تذكرة_السفر", "من_دولة", "الى_دولة", "مدة_الاجازة", "يوم_التعيين", "SuggestedJob", "الفرع", "رقم_المحمول", "تليفون_المنزل", "الكفيل", "الادارة", "تاريخ_التكليف", "الموقع_الوظيفى", "حالة_التعيين", "الدرجة_المالية", "المسار_الوظيفى", "مدة_تعيين_الموظف", "مدة_التحاق_الموظف", "الجنسية", "اسم_الشركة", "نوع_الموظف", "مدة_الخبرة", "اخر_مؤهل_دراسى", "تاريخ_الميلاد", "الحالة_الاجتماعية", "الديانة", "الحالة_العسكرية", "تاريخ_تحرير_العقد", "يوم_تاريخ_تحرير_العقد»", "كورسات_التدريب", "المؤهلات" };
            else
                ArryList = new List<string>() { "EmployeeName", "NationalId", "NationalIdDate", "EmployeeAddress", "PassportNo", "PassportIssueDate", "AddressingNo", "ContractPeriod", "EmploymentStartDate", "EmploymentEndDate", "Job", "Salary", "Allowances", "Currency", "TicketCnt", "TicketAmt", "FromCountry", "ToCountry", "SeasonHolidayPeriod", "EmploymentDay", "SuggestedJob", "Branch", "Mobile", "HomeTelephone", "Kafel", "Department", "AssignDate", "Position", "AssignStatus", "PayrollGrad", "CareerPath", "JoinWorkingPeriod", "EmployWorkingPeriod", "Nationality", "CompanyName", "Gender", "ExpPeriod", "LastQual", "BirthDate", "MaritalStatus", "Religion", "MilitaryStatus", "ContractDate", "ContractDay", "TrainingCrs", "Qualifications" };


            string[] EmpData = _hrUnitOfWork.EmployeeRepository.GetEmpMergeData(EmpId, CompanyId, Language);
            string job = EmpData[20];
            string File = "";
            bool ExistFile = false;
            string Path = "";
            string CopyPath = "";

            if (FileName == "0")
            {
                Path = Server.MapPath(string.Format("../SpecialData/Contracts/{0}/{1}", CompanyId.ToString(), job));
                if (Directory.Exists(Path))
                {
                    var Files = Directory.GetFiles(Path);
                    File = Files.FirstOrDefault();
                    if (File != null)
                    {
                        ExistFile = true;
                        File = File.Split('\\').LastOrDefault();
                    }
                }

                if (ExistFile)
                    Path = string.Format("{0}/{1}", Path, File);
                else
                {
                    string CompanyContract = _PersonSetup.ContractTempl;
                    string PersonnelPath = Server.MapPath(string.Format("../SpecialData/Contracts/{0}/{1}", CompanyId.ToString(), CompanyContract));

                    if (CompanyContract != null && System.IO.File.Exists(PersonnelPath))
                    {
                        Path = PersonnelPath;
                        ExistFile = true;
                    }
                }
                if (!ExistFile)
                {
                    mail.Path = "";
                    mail.Exist = ExistFile;
                    return Json(mail);
                }
                CopyPath = "/SpecialData/TempFolder/Contract.docx";
            }
            else
            {
                Path = Server.MapPath("/SpecialData/HrLetters/Uploaded");

                if (Directory.Exists(Path))
                {
                    if (System.IO.File.Exists(Path.Insert(Path.Length, "/" + FileName)))
                    {
                        ExistFile = true;
                        Path = Server.MapPath("/SpecialData/HrLetters/Uploaded/"+ FileName);
                    }
                }

                if (!ExistFile)
                {
                    mail.Path = "";
                    mail.Exist = ExistFile;
                    return Json(mail);
                }
                CopyPath = "/SpecialData/TempFolder/Letter.docx";
            }

            if (!Directory.Exists(Server.MapPath("/SpecialData/TempFolder")))
                Directory.CreateDirectory(Server.MapPath("/SpecialData/TempFolder"));

            try
            {
                if (System.IO.File.Exists(Server.MapPath(CopyPath)))
                    System.IO.File.Delete(Server.MapPath(CopyPath));
                System.IO.File.Copy(Path, Server.MapPath(CopyPath));
            }
            catch (Exception ex)
            {
                mail.Path = "InUse";
            }

            if (mail.Path == "InUse")
                return Json(mail);

            using (WordprocessingDocument docPKG = WordprocessingDocument.Open(Server.MapPath(CopyPath), true))
            {

                var xx = docPKG.MainDocumentPart.Document.Descendants<Text>();
                int cc = -1;
                string FieldName = "";

                foreach (var item in xx)
                {
                    cc++;
                    int Lastindex = 0;
                    FieldName = "";
                    if (item.Text.Contains("«") && item.Text.Contains("»"))
                    {
                        int index = ArryList.FindIndex(f => f == item.Text.Replace("»", "").Replace("«", ""));
                        if (index >= 0)
                            item.Text = EmpData[index];
                    }
                    else if (item.Text.Contains("«"))
                    {
                        Lastindex = xx.ToList().FindIndex(cc, a => a.Text.Contains("»"));
                        for (int y = cc; y <= Lastindex; y++)
                        {
                            FieldName += xx.ElementAt(y).Text;
                            xx.ElementAt(y).Text = "";
                        }
                        int index = ArryList.FindIndex(f => f == FieldName.Replace("»", "").Replace("«", ""));
                        if (index >= 0)
                            item.Text = EmpData[index];
                    }

                }
                docPKG.MainDocumentPart.Document.Save();

            }
            mail.Path = CopyPath;
            mail.Exist = ExistFile;
            return Json(mail);

        }
        #endregion

        #region EmpLetters
        public ActionResult EmpLetters()
        {
            return View();
        }
        public ActionResult GetEmpLettersData()
        {
            return Json(_hrUnitOfWork.HrLettersRepository.EmpLetters(CompanyId, Language), JsonRequestBehavior.AllowGet);
        }
        void SaveEmpFile(string OldFile, string Language, string Temp)
        {
            string Path = Server.MapPath("/SpecialData/HrLetters/Uploaded/" + Temp);
            if (!Directory.Exists(Server.MapPath("/SpecialData/HrLetters/Uploaded/" + Language)))
                Directory.CreateDirectory(Server.MapPath("/SpecialData/HrLetters/Uploaded/" + Language));

            string NewPath = Server.MapPath("/SpecialData/HrLetters/Uploaded/" + Language + "/" + Temp);
            string OldFilePath = Server.MapPath("/SpecialData/HrLetters/Uploaded/" + Language + "/" + OldFile);
            if (System.IO.File.Exists(Path))
            {
                if (!string.IsNullOrEmpty(OldFile) && System.IO.File.Exists(OldFilePath))
                    System.IO.File.Delete(OldFilePath);

                if (System.IO.File.Exists(NewPath))
                    System.IO.File.Delete(NewPath);

                System.IO.File.Move(Path, NewPath);
            }
          
        }
        public ActionResult UpdateEmpLetter(IEnumerable<HrlettersViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<HrlettersViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();
            var result = new List<PagePrint>();

            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "EmpHrLetters",
                        TableName = "PagePrints",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }
                var ids = models.Select(a => a.Id);
                var db_HrLetter = _hrUnitOfWork.Repository<PagePrint>().Where(a => ids.Contains(a.Id)).ToList();

                for (var i = 0; i < models.Count(); i++)
                {
                    var item = models.ElementAtOrDefault(i);

                    PagePrint Letter = db_HrLetter.FirstOrDefault(a => a.Id == models.ElementAtOrDefault(i).Id);
                    if (Letter == null)
                    {
                        Letter = new PagePrint();
                        item.Id = Letter.Id;
                    }
                    string OldFile = Letter.LetterTempl; 
                    AutoMapper(new AutoMapperParm() { ObjectName = "EmpHrLetters", Destination = Letter, Source = item, Version = 0, Options = options.ElementAtOrDefault(i), Transtype = item.Id > 0 ? TransType.Update : TransType.Insert });
                    if (item.LetterTempl != null)
                    {
                        SaveEmpFile(OldFile, Letter.Culture, item.LetterTempl);
                        DownloadLetter(Letter.ObjectName, Letter.Culture, Letter.LetterTempl);
                    }
                    if (item.Id > 0)
                    {
                        Letter.ModifiedTime = DateTime.Now;
                        Letter.ModifiedUser = UserName;

                        _hrUnitOfWork.HrLettersRepository.Attach(Letter);
                        _hrUnitOfWork.HrLettersRepository.Entry(Letter).State = EntityState.Modified;
                    }
                    else
                        _hrUnitOfWork.HrLettersRepository.Add(Letter);

                    result.Add(Letter);
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
            {
                datasource.Data = (from m in models
                                   join r in result on m.LetterTempl equals r.LetterTempl
                                   select new HrlettersViewModel
                                   {
                                       Id = r.Id,
                                       Name = m.Name,
                                       LetterTempl = m.LetterTempl,
                                       Culture = m.Culture,
                                       Language = m.Language,
                                       ObjectName = m.ObjectName,
                                      Version=m.Version 
                                   }).ToList();
                return Json(datasource.Data);
            }
        }

        [HttpPost]
        public ActionResult DownloadLetter(string ObjectName, string Language, string LetterName)
        {
            string Path = "/SpecialData/HrLetters/Uploaded/"+ Language + "/" + LetterName;
            string pathName = Server.MapPath(Path);

            string ListPath = Server.MapPath("/App_Data/" + string.Format("{0}{1}",Language,ObjectName) + ".mdb");

            if (!System.IO.File.Exists(ListPath))
                ListPath = Server.MapPath("/App_Data/" + string.Format("{0}{1}", "en-GB", ObjectName) + ".mdb");
            
            MailMergeViewmodel mail = new MailMergeViewmodel();

            if (System.IO.File.Exists(pathName))
            {

                FileInfo fl = new FileInfo(pathName);
                using (WordprocessingDocument docPKG = WordprocessingDocument.Open(pathName, true))
                {
                    MailMerge mymerge = docPKG.MainDocumentPart.DocumentSettingsPart.Settings.Elements<MailMerge>().FirstOrDefault();
                    var Relation = docPKG.MainDocumentPart.DocumentSettingsPart.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/mailMergeSource", new Uri(ListPath));
                    string Connectionval = "Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source=" + ListPath + ";Mode=Read;Extended Properties=\"\";Jet OLEDB:System database=\"\";Jet OLEDB:Registry Path=\"\";Jet OLEDB:Engine Type=6;Jet OLEDB:Database Locking Mode=0;Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Global Bulk Transactions=1;Jet OLEDB:New Database Password=\"\";Jet OLEDB:Create System Database=False;Jet OLEDB:Encrypt Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;Jet OLEDB:SFP=False;Jet OLEDB:Support Complex Data=False;Jet OLEDB:Bypass UserInfo Validation=False;Jet OLEDB:Limited DB Caching=False;Jet OLEDB:Bypass ChoiceField Validation=False";

                    if (mymerge != null)
                    {
                        mymerge.ConnectString = new ConnectString() { Val = Connectionval };
                        mymerge.DataSourceReference = new DataSourceReference() { Id = Relation.Id };
                    }
                    else
                    {
                        MailMerge m = new MailMerge();
                        string xmlnsw = @"http://schemas.openxmlformats.org/wordprocessingml/2006/main";
                        m.MainDocumentType = new MainDocumentType();
                        m.MainDocumentType.SetAttribute(new DocumentFormat.OpenXml.OpenXmlAttribute("val", xmlnsw, "formatLetters"));
                        m.LinkToQuery = new LinkToQuery();
                        m.DataType = new DataType();
                        m.DataType.SetAttribute(new DocumentFormat.OpenXml.OpenXmlAttribute("val", xmlnsw, "native"));
                        m.ConnectString = new ConnectString();
                        m.ConnectString.SetAttribute(new DocumentFormat.OpenXml.OpenXmlAttribute("val", xmlnsw, Connectionval));
                        m.Query = new Query();
                        m.Query.SetAttribute(new DocumentFormat.OpenXml.OpenXmlAttribute("val", xmlnsw, "SELECT * FROM `Office Address List` "));
                        m.ViewMergedData = new ViewMergedData();
                        m.DataSourceObject = new DataSourceObject();
                        m.DataSourceObject.UdlConnectionString = new UdlConnectionString();
                        m.DataSourceObject.UdlConnectionString.SetAttribute(new DocumentFormat.OpenXml.OpenXmlAttribute("val", xmlnsw, Connectionval));
                        m.DataSourceObject.DataSourceTableName = new DataSourceTableName();
                        m.DataSourceObject.DataSourceTableName.SetAttribute(new DocumentFormat.OpenXml.OpenXmlAttribute("val", xmlnsw, "Office Address List"));
                        m.DataSourceObject.ColumnDelimiter = new ColumnDelimiter();
                        m.DataSourceObject.ColumnDelimiter.SetAttribute(new DocumentFormat.OpenXml.OpenXmlAttribute("val", xmlnsw, "9"));
                        if (m.DataSourceReference == null)
                        {
                            m.DataSourceReference = new DataSourceReference();
                            m.DataSourceReference.Id = Relation.Id;
                        }
                        m.ViewMergedData.Val = true;

                        docPKG.MainDocumentPart.DocumentSettingsPart.Settings.InsertAt<MailMerge>(m, 0);
                    }

                    docPKG.Save();
                    docPKG.Dispose();
                }
                mail.Path = Path;
                mail.Exist = true;
            }
            else
            {
                mail.Path = "";
                mail.Exist = false;

            }
            return Json(mail);
        }
        [HttpPost]
        public ActionResult MergeEmp(string objectName,int Id,int EmpId)
        {
           
            return Json(MergeData(objectName,Id,EmpId,Language));
        }

        public MailMergeViewmodel MergeData(string objectName, int Id, int EmpId,string NLanguage)
        {
            MailMergeViewmodel mail = new MailMergeViewmodel();
            //mail.Exist = false;
            string FilePath = "";

            // set default language for employee lang or user lang
            Language = Language == null ? NLanguage : Language;

            //setup temp folder on server
            string CopyPath = "/SpecialData/TempFolder";
            string NoPage = MsgUtils.Instance.Trls("NoPage",Language);
            string ServerCopyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SpecialData\\TempFolder");

            if (Id != 0 && objectName != null)
            {
                MailMergeArrayViewModel model = BackListDataAndFields(objectName, Id, EmpId);

                string FileName = _hrUnitOfWork.Repository<PagePrint>().Where(a => a.ObjectName == objectName && a.CompanyId == 0 && a.Culture == Language && a.Version == 0).FirstOrDefault()?.LetterTempl;

                if (!string.IsNullOrEmpty(FileName))
                {
                    FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"SpecialData\\HrLetters\\Uploaded\\" + Language + "\\" + FileName);
                    if (!Directory.Exists(ServerCopyPath))
                        Directory.CreateDirectory(ServerCopyPath);
                    else
                    {
                        //Empty Exist files in TempFolder
                        string[] files = Directory.GetFiles(ServerCopyPath);
                        for (int i = 0; i < files.Length; i++)
                            System.IO.File.Delete(files[i]);
                    }

                    ServerCopyPath += "\\" + FileName;
                    try
                    {
                        if (System.IO.File.Exists(ServerCopyPath))
                            System.IO.File.Delete(ServerCopyPath);
                        if (System.IO.File.Exists(FilePath))
                        {
                            mail.Exist = true;
                            System.IO.File.Copy(FilePath, ServerCopyPath);
                        }
                        else
                            mail.Error = NoPage;
                    }
                    catch (Exception ex)
                    {
                        mail.Error = MsgUtils.Instance.Trls("InUse",Language);
                    }

                    if (!mail.Exist)
                        return mail;
                    else
                    {
                        using (WordprocessingDocument docPKG = WordprocessingDocument.Open(ServerCopyPath, true))
                        {
                            var AllText = docPKG.MainDocumentPart.Document.Descendants<Text>();
                            int cc = -1;
                            string FieldName = "";
                            string value = "";
                            string key = "";
                            foreach (var item in AllText)
                            {
                                cc++;
                                int Lastindex = 0;
                                FieldName = "";
                                if (item.Text.Contains("«") && item.Text.Contains("»"))
                                {
                                    value = item.Text.Replace("»", "").Replace("«", "");
                                    if (model.Fields.ContainsValue(value))
                                    {
                                        key = model.Fields.FirstOrDefault(x => x.Value == value).Key;
                                        item.Text = model.Data.FirstOrDefault(x => x.Key == key).Value;
                                    }
                                }
                                else if (item.Text.Contains("«"))
                                {
                                    Lastindex = AllText.ToList().FindIndex(cc, a => a.Text.Contains("»"));
                                    for (int y = cc; y <= Lastindex; y++)
                                    {
                                        FieldName += AllText.ElementAt(y).Text;
                                        AllText.ElementAt(y).Text = "";
                                    }
                                    item.Text.Replace("»", "").Replace("«", "");
                                    value = FieldName.Replace("»", "").Replace("«", "");
                                    if (model.Fields.ContainsValue(value))
                                    {
                                        key = model.Fields.FirstOrDefault(x => x.Value == value).Key;
                                        item.Text = model.Data.FirstOrDefault(x => x.Key == key).Value;
                                    }
                                }

                            }
                            docPKG.MainDocumentPart.DocumentSettingsPart.Settings.Elements<MailMerge>().FirstOrDefault().Remove();
                            docPKG.MainDocumentPart.Document.Save();
                            docPKG.Dispose();
                        }
                        mail.Path = CopyPath+"/"+FileName;
                        mail.Exist = true;
                        mail.ServerFilePath = ServerCopyPath;
                        
                    }

                }
                else
                    mail.Error = NoPage;
            }
            else
            {
                if (EmpId == 0)
                    mail.Error = MsgUtils.Instance.Trls("NoEmp",Language);
                else
                    mail.Error = NoPage;
            }
            return mail;
        }
        public MailMergeArrayViewModel BackListDataAndFields(string ObjectName, int Id, int EmpId)
        {
            MailMergeArrayViewModel model = new MailMergeArrayViewModel();

            model.Fields = _hrUnitOfWork.Repository<Mailtoken>().Where(a => a.Culture == Language && a.ObjectName == ObjectName).Select(c => new { Name = c.Name, Meaning = c.Meaning }).ToDictionary(m => m.Name, m => m.Meaning);

            switch (ObjectName)
            {
                case "RecieveCustody":
                    model.Data = _hrUnitOfWork.CustodyRepository.ReadMailEmpCustody(Language, Id);
                    break;
                case "LeaveRequest":
                    model.Data = _hrUnitOfWork.LeaveRepository.ReadMailEmpLeave(Language, Id);
                    break;
                case "TermRequestForm":
                    model.Data = _hrUnitOfWork.TerminationRepository.ReadMailEmpTerm(Language, Id);
                    break;
                case "AssignOrders":
                    model.Data = _hrUnitOfWork.LeaveRepository.ReadMailEmpAssign(Language, Id);
                    break;
                case "ContractFinish":
                    model.Data = _hrUnitOfWork.EmployeeRepository.ReadMailEmpContractFinish(Language, Id);
                    break;
                case "RenewContract":
                    model.Data = _hrUnitOfWork.EmployeeRepository.ReadMailEmpNewContract(Language, Id);
                    break;
                case "BorrowPapers":
                    model.Data = _hrUnitOfWork.CustodyRepository.ReadMailEmpBorrowDoc(Language, Id);
                    break;
                default:
                    break;
            }
            return model;
        }
        #endregion

    }
}