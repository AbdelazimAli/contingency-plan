using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Interface.Core;
using Model.Domain;
using Model.ViewModel.Personnel;
using System.Data.Entity;
using WebApp.Extensions;
using Model.ViewModel;
using System.Linq.Dynamic;
using System.IO;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Routing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WebApp.Controllers
{
    public class JobController : BaseController
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
        public JobController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
           
        }
        // GET: Jop
        [HttpGet]
       
        public ActionResult Index()
        {
            string RoleId = Request.QueryString["RoleId"]?.ToString();
            int MenuId = Request.QueryString["MenuId"] != null ? int.Parse(Request.QueryString["MenuId"].ToString()) : 0;
            if (MenuId != 0)
                ViewBag.Functions = _hrUnitOfWork.MenuRepository.GetUserFunctions(RoleId, MenuId).ToArray();
            return View();
        }
        public ActionResult PeriodIndex()
        {
            return View();
        }
        public ActionResult GetAllJobs(int MenuId)
        {
            var query = _hrUnitOfWork.JobRepository.ReadJobs(CompanyId, Language,0).AsQueryable();
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
      
        void FillViewBag(bool Islocal)
        {
            ViewBag.Frequency = _hrUnitOfWork.CompanyRepository.GetLookUpCode(Language, "Frequency");
            ViewBag.PayrollGrade = _hrUnitOfWork.JobRepository.GetPayrollGrade();
            ViewBag.JobClass = _hrUnitOfWork.JobRepository.GetJobClass(CompanyId);
        }

        [HttpGet]
        public ActionResult Details(int Id=0)
        {
            var Job = _hrUnitOfWork.JobRepository.ReadJob(Id, Language);

            if (Id == 0)
                Job = new JobViewModel();

            FillViewBag(Id==0?false:Job.IsLocal);

            return View(Job);
            
        }


        [HttpPost]
        public ActionResult Details(JobViewModel model, OptionsViewModel moreInfo)
        {
           
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.LocationRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "Job",
                        TableName = "Jobs",
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

                List<JobClass> JclassList = new List<JobClass>();             
                var record = _hrUnitOfWork.JobRepository.JobObject(model.Id);
                if (model.IJobClasses != null && model.IJobClasses.Count() >= 0)
                {
                  
                    foreach (var item in model.IJobClasses)
                    {
                        JobClass J = _hrUnitOfWork.Repository<JobClass>().Where(a => a.Id == item).FirstOrDefault();
                        JclassList.Add(J); 
                    }
                }
             
              if(model.ContractTempl != null)
                    model.ContractTempl = model.ContractTempl.Split('\\').LastOrDefault().Split('.')[0]+".docx";
                // add New
                if (record == null)
                {
                    record = new Job();
                    _hrUnitOfWork.JobRepository.AddLName(Language, record.Name, model.Name, model.LName);

                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Job",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Insert
                    });

                    record.CreatedTime = DateTime.Now;
                    record.CreatedUser = UserName;
                    record.JobClasses = JclassList;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    _hrUnitOfWork.JobRepository.Add(record);
                }
                // Update
                else
                {
                    _hrUnitOfWork.JobRepository.AddLName(Language, record.Name, model.Name, model.LName);
                    moreInfo.VisibleColumns.Add("JobClasses");
                    AutoMapper(new Models.AutoMapperParm
                    {
                        Destination = record,
                        Source = model,
                        ObjectName = "Job",
                        Version = Convert.ToByte(Request.Form["Version"]),
                        Options = moreInfo,
                        Transtype = TransType.Update
                    });
                 
                    if (record.JobClasses == null)
                    {
                        record.JobClasses = JclassList;
                    }
                    else
                    {
                        var Jclassobj = record.JobClasses.Select(a => a).ToList();

                        foreach (var item in JclassList)
                        {
                            if (!Jclassobj.Contains(item))
                            {
                                record.JobClasses.Add(item);

                            }
                        }
                        foreach (var item in Jclassobj)
                        {
                            if (!JclassList.Contains(item))
                                record.JobClasses.Remove(item);
                        }
                    }  
                          
                    record.ModifiedTime = DateTime.Now;
                    record.ModifiedUser = UserName;
                    record.CompanyId = model.IsLocal ? CompanyId : (int?)null;
                    _hrUnitOfWork.JobRepository.Attach(record);
                    _hrUnitOfWork.JobRepository.Entry(record).State = EntityState.Modified;
                }

                
                var Errors = SaveChanges(Language);

                #region Contract File
                if (!string.IsNullOrEmpty(model.ContractTempl))
                {
                    string CompanyPath = Server.MapPath("/SpecialData/Contracts/" + CompanyId.ToString());

                    if (Directory.Exists(CompanyPath))
                    {
                        if (Directory.Exists(string.Format(CompanyPath + "/{0}", record.Id)))
                        {
                            var Files = Directory.GetFiles(CompanyPath.Insert(CompanyPath.Length, "/" + record.Id));
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
                                    return Json(Errors);
                                else
                                    System.IO.File.Delete(Dirfile);

                            }

                        }
                    }
                    else
                        Directory.CreateDirectory(CompanyPath);

                    if (System.IO.File.Exists(Server.MapPath("/SpecialData/TempFolder/" + model.ContractTempl)))
                        System.IO.File.Move(Server.MapPath("/SpecialData/TempFolder/" + model.ContractTempl), Server.MapPath("/SpecialData/Contracts/" + CompanyId.ToString() + "/" + record.Id + "/" + model.ContractTempl));
                }

                #endregion

                var message = "OK,";
                if (Errors.Count > 0)
                    message = Errors.First().errors.First().message;
                else
                {
                    model.Id = record.Id;
                    message += ((new JavaScriptSerializer()).Serialize(model));
                }
                return Json(message);

            }
           
                return Json(Models.Utils.ParseFormErrors(ModelState));
        }


        public ActionResult DeleteJob(int id)
        {
            var message = "OK";
            DataSource<PositionViewModel> Source = new DataSource<PositionViewModel>();

            Job Job = _hrUnitOfWork.JobRepository.Get(id);

            if (Job != null)
            {
                AutoMapper(new Models.AutoMapperParm
                {
                    Source = Job,
                    ObjectName = "Job",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Transtype = TransType.Delete
                });
                _hrUnitOfWork.JobRepository.Remove(Job);
                _hrUnitOfWork.JobRepository.RemoveLName(Language,Job.Name);
            }

            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
                return Json(message);
        }

        #region Contract
        public ActionResult ContractFiles(int Id)
        {
            var result = new List<ChartViewModel>();
            string ListPath = Server.MapPath("/App_Data/ContractsList.mdb");
            if (User.Identity.RTL())
                ListPath = Server.MapPath("/App_Data/arContractsList.mdb");

            if (Id != 0)
            {
                string pathName = Server.MapPath(@"/SpecialData");
                string CompanyPath = Server.MapPath(@"/SpecialData/Contracts/" + CompanyId);
                if (Directory.Exists(pathName))
                {
                    var Files = Directory.GetFiles(pathName);
                    foreach (var item in Files)
                    {
                        FileInfo fl = new FileInfo(item);
                        var f = new ChartViewModel()
                        {
                            category = fl.Name,
                            color = "/SpecialData/" + fl.Name
                        };
                        result.Add(f);
                    }

                }
                if (Directory.Exists(CompanyPath))
                {
                    if (!Directory.Exists(string.Format(CompanyPath + "/{0}", Id)))
                        Directory.CreateDirectory(string.Format(CompanyPath + "/{0}", Id));
                    else
                    {
                        var Files = Directory.GetFiles(string.Format(CompanyPath + "/{0}", Id));
                        foreach (var item in Files)
                        {
                            FileInfo fl = new FileInfo(item);
                            var f = new ChartViewModel()
                            {
                                category = fl.Name,
                                color = string.Format("/SpecialData/Contracts/{0}/{1}/{2}", CompanyId, Id, fl.Name)
                            };
                            result.Add(f);
                        }
                    }
                }
                else
                    Directory.CreateDirectory(CompanyPath);
            }
            if(result.Count != 0)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    string item = Server.MapPath(result.ElementAt(i).color);
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
                }
            }
            return PartialView("_ContractFiles", result);

        }
        [HttpPost]
        public ActionResult CheckDocs(HttpPostedFileBase ContractTempl)
        {
            string Message = "Exist";
            string[] arr = { "docx", "doc" };
            bool MoveOne = true;
            string Temp = "../SpecialData/TempFolder";
            string ListPath = Server.MapPath("/App_Data/ContractsList.mdb");
            if (User.Identity.RTL())
                ListPath = Server.MapPath("/App_Data/arContractsList.mdb");
            if (ContractTempl != null)
            {
                if (!arr.Any(f => ContractTempl.FileName.EndsWith(f, StringComparison.OrdinalIgnoreCase)))
                    MoveOne = false;
                else
                {
                    if (!Directory.Exists(Server.MapPath(Temp)))
                        Directory.CreateDirectory(Server.MapPath(Temp));
                    
                    var Files = Directory.GetFiles(Server.MapPath(Temp));
                    foreach (var item in Files)
                    {
                        System.IO.File.Delete(item);
                    }
                    string Name = ContractTempl.FileName.Split('.')[0] + ".docx";
                    ContractTempl.SaveAs(Server.MapPath(Temp)+"/"+ Name);

                    using (WordprocessingDocument docPKG = WordprocessingDocument.Open(Server.MapPath(Temp) + "/" + Name, true))
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
                }
            }
            else
                Message = "NoData";

            return Json(new { Exist= Message,Move=MoveOne});
        }
        #endregion
    }
}