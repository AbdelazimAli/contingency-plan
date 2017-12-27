using HR_uploader.Helpers;
using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class FileUploadController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        FilesHelper filesHelper;
        string tempPath = "~/uploadercash/";
        string serverMapPath = "~/Files/uploadercash/";
        string UrlBase = "/Files/uploadercash/";
        string DeleteURL = "/FileUpload/DeleteFile/?file=";
        string DeleteType = "GET";

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
        [HttpGet]
        public string GetCompanyLogo(int Id, string Name)
        {
            var query = _hrUnitOfWork.Repository<CompanyDocsViews>()
                .Where(d => d.CompanyId == Id && d.TypeId == 1)
                .Select(d => new { name = d.name })
                .ToList();

            if (query.Count == 0 || Name == query[0].name)
                return "0";
            else
                return query[0].name;
        }
        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }
        public FileUploadController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot, UrlBase, tempPath, serverMapPath);
            _hrUnitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Show()
        {
            JsonFiles ListOfFiles = filesHelper.GetFileList();
            var model = new Models.FilesViewModel()
            {
                Files = ListOfFiles.files
            };

            return View(model);
        }

        public ActionResult Edit(string Source, string SourceId)
        {
            UserContext db = new UserContext();
            ViewBag.Source = Source;
            ViewBag.SourceId = SourceId;
            ViewBag.DocType = _hrUnitOfWork.LookUpRepository.GetDocTypes(CompanyId, User.Identity.GetCulture()).Select(d => new { id = d.Id, name = d.LocalName });
            ViewBag.CodeName = _hrUnitOfWork.Repository<LookUpCode>().Select(l => new { value = l.CodeName, text = l.CodeName }).Distinct();
            ViewBag.Upload = db.Users.Where(a => a.UserName == UserName).FirstOrDefault().UploadDocs;
            return View();
        }

        [HttpPost]
        public JsonResult UploadOneFile(string Source, string SourceId)
        {
            var file = HttpContext.Request.Files[0];
            if (file == null)
                return Json("No files found");

            int sid = Convert.ToInt32(SourceId);
            var stream = ReadFully(file.InputStream);
            WebImage fullsize = null;
            WebImage thumbs= null;

            if (Source == "EmployeePic")
            {
                fullsize = new WebImage(stream).Resize(180, 180);
                thumbs = new WebImage(stream).Resize(32, 32);
            }
            else if (Source == "CompanyLogo")
            {
                fullsize = new WebImage(stream).Resize(396,130);
                thumbs = new WebImage(stream).Resize(80, 80);
            }
            else if (file.ContentType != "application/pdf")
            {
                fullsize = new WebImage(stream).Resize(1240, 1754); //   1240, 1754    2480, 3508
                thumbs = new WebImage(stream).Resize(124, 175);
            }

           
       
            CompanyDocsViews doc = new CompanyDocsViews()
            {
                CompanyId = (Source == "Company" ? sid : CompanyId),
                name = file.FileName,
                CreatedUser = UserName,
                Source = Source,
                SourceId = sid,
                file_stream = fullsize != null ? fullsize.GetBytes(): stream,
                thumbs = thumbs != null ? thumbs.GetBytes() : null
            };

            _hrUnitOfWork.CompanyRepository.Add(doc);

            var errors = SaveChanges(Language);
            string error = errors.Count > 0 ? errors.First().errors.First().message : "OK";
            return Json(error);
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }


        public JsonResult Upload(string Source, string SourceId)
        {
            //if (Request.QueryString["Source"] != null && Request.QueryString["SourceId"] != null)
            //{
            //    Source = Request.QueryString["Source"]?.ToString();
            //    SourceId = Request.QueryString["SourceId"]?.ToString();
            //}
            var resultList = new List<ViewDataUploadFilesResult>();

            filesHelper.UploadAndShowResults(HttpContext, resultList);
            if (!resultList.Any())
                return Json("Error");


            JsonFiles files = new JsonFiles(resultList);
            var username = UserName;
            var names = new List<string>();
            int sid = Convert.ToInt32(SourceId);
            var CompanyDocs = _hrUnitOfWork.Repository<CompanyDocsViews>()
                .Where(d => d.Source == Source && d.SourceId == sid).Select(a => a.name).ToList();

            bool Exist = false;
            string name = files.files.FirstOrDefault().name;
            if (CompanyDocs.Contains(name))
            {
                string checkName = name;
                Exist = true;
                for (int i = 1; i <= CompanyDocs.Count; i++)
                {
                    checkName = string.Format("copy{0}_{1}", i, name);
                    if (!CompanyDocs.Contains(checkName))
                    {
                        name = checkName;
                        break;
                    }
                }
            }


            foreach (var item in files.files)
            {

                CompanyDocsViews doc = new CompanyDocsViews()
                {
                    CompanyId = (Source == "Company" ? sid : CompanyId),
                    name = Exist ? name : item.name,
                    CreatedUser = username,
                    Source = Source,
                    SourceId = sid,
                    file_stream = System.IO.File.ReadAllBytes(Server.MapPath(serverMapPath + item.name))
                };

                names.Add(doc.name);
                _hrUnitOfWork.CompanyRepository.Add(doc);
            }

            var errors = SaveChanges(Language);

            var model = _hrUnitOfWork.Repository<CompanyDocsViews>()
                .Where(d => d.Source == Source && d.SourceId == sid && names.Contains(d.name))
                .Select(d => new
                {
                    sid = d.stream_id,
                    time = d.creation_time,
                    name = d.name
                }).ToList();

            var result = from m in model
                         join file in files.files on m.name equals Exist ? name : file.name
                         select (new ViewDataUploadFilesResult
                         {
                             sid = m.sid,
                             name = Exist ? name : file.name,
                             deleteType = file.deleteType,
                             deleteUrl = DeleteURL + m.sid,
                             size = file.size,
                             thumbnailUrl = file.thumbnailUrl,
                             type = file.type,
                             url = file.url,
                             last_access_time = m.time.Value.ToString("dd/MM/yyyy hh:mm"),
                             creation_time = m.time.Value.ToString("dd/MM/yyyy hh:mm"),
                             CreatedUser = username,
                             DocName=m.name
                         });

            return Json(new JsonFiles(result.ToList()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult exist(int Id, string filename)
        {
            return Json(new { name = filename, count = _hrUnitOfWork.Repository<CompanyDocsViews>().Count(d => d.CompanyId == Id && d.name == filename) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFileList(string Source, string SourceId)
        {
            int sid = 0;
            if (Source.Length == 0 || SourceId.Length == 0 || !int.TryParse(SourceId, out sid))
                return Json("Missing Source & Source Id");


            emptycash();
            var comdocs = _hrUnitOfWork.CompanyRepository.GetDocsViews(Source, sid);
            var r = new List<ViewDataUploadFilesResult>();

            string fullPath = Path.Combine(StorageRoot);
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo dir = new DirectoryInfo(fullPath);
                foreach (var item in comdocs)
                {

                    string filefullpath = Server.MapPath(serverMapPath + item.name);
                    System.IO.File.WriteAllBytes(filefullpath, item.file_stream);
                    string[] imageArray = item.name.Split('.');
                    if (imageArray.Length != 0)
                    {
                        string extension = imageArray[imageArray.Length - 1].ToLower();
                        if (extension != "jpg" && extension != "png" && extension != "jpeg" && extension != "Bmp" && extension != "gif") //Do not create thumb if file is not an image
                        {

                        }
                        else
                        {
                            var ThumbfullPath = Server.MapPath(serverMapPath) + "thumbs/";
                            string fileThumb = item.name + ".80x80.jpg";
                            var ThumbfullPath2 = ThumbfullPath + fileThumb;
                            using (MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(filefullpath)))
                            {
                                var thumbnail = new WebImage(stream).Resize(80, 80);
                                thumbnail.Save(ThumbfullPath2, "jpg");
                            }
                        }
                    }

                    FileInfo file = new FileInfo(filefullpath);
                    int SizeInt = unchecked((int)file.Length);

                    r.Add(filesHelper.UploadResult(file.Name, SizeInt, file.FullName, item.stream_id));
                }
            }

            var model = _hrUnitOfWork.CompanyRepository.GetCompanyDocsViews(Source, Convert.ToInt32(SourceId));

            // for selfservice user hide files for access level = 0
            if (User.Identity.IsSelfServiceUser()) model = model.Where(a => a.AccessLevel > 0);


            var result = from m in model
                         join file in r on m.stream_id equals file.sid
                         orderby m.creation_time.Value
                         select (new ViewDataUploadFilesResult
                         {
                             sid = file.sid,
                             name = file.name,
                             deleteType = file.deleteType,
                             deleteUrl = file.deleteUrl,
                             size = file.size,
                             thumbnailUrl = file.thumbnailUrl,
                             type = file.type,
                             url = m.AccessLevel == 1 && User.Identity.IsSelfServiceUser() ? "" : file.url,
                             Description = m.Description,
                             DocType = m.DocType,
                             ExpiryDate = (m.ExpiryDate == null ? "" : m.ExpiryDate.Value.ToString("dd/MM/yyyy")),
                             last_access_time = (m.last_access_time == null ? "" : m.last_access_time.Value.ToString("dd/MM/yyyy hh:mm")),
                             creation_time = m.creation_time.Value.ToString("dd/MM/yyyy hh:mm"),
                             CreatedUser = m.CreatedUser,
                             ModifiedUser = m.ModifiedUser,
                             DocName = m.DocName,
                             Keyword = m.Keyword

                         });

            return Json(new JsonFiles(result.ToList()), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateDocument(Model.ViewModel.CompanyDocViewModel doc)
        {
            if (ModelState.IsValid)
            {
                string message = "OK,";
                CompanyDocsViews row = new CompanyDocsViews();
                AutoMapper(new AutoMapperParm() { Destination = row, Source = doc , ObjectName = "CompanyDocuments", Version = Convert.ToByte(Request.Form["Version"]) });
                row.TypeId = doc.DocType;
                row.ModifiedUser = doc.ModifiedUser;               
                _hrUnitOfWork.CompanyRepository.Attach(row);
                _hrUnitOfWork.CompanyRepository.Entry(row).State = System.Data.Entity.EntityState.Modified;
                var errors = SaveChanges(Language);
                if (errors.Count() > 0)
                    message = errors.First().errors.First().message;
                else
                    message += (new JavaScriptSerializer()).Serialize(doc);

                return Json(message);
            }
            return Json(Models.Utils.ParseFormErrors(ModelState));
        }

        [HttpGet]
        public JsonResult DeleteFile(Guid file)
        {
            CompanyDocsViews doc = _hrUnitOfWork.Repository<CompanyDocsViews>().Single(d => d.stream_id == file);
            _hrUnitOfWork.CompanyRepository.Remove(doc);
            SaveChanges(Language);
            filesHelper.DeleteFile(doc.name);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMultiFiles(List<Guid> files)
        {
            List<CompanyDocsViews> docs = _hrUnitOfWork.Repository<CompanyDocsViews>().Where(d => files.Contains(d.stream_id)).ToList();
            foreach (var doc in docs)
            {
                _hrUnitOfWork.CompanyRepository.Remove(doc);
                filesHelper.DeleteFile(doc.name);
            }
            SaveChanges(Language);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult emptycash()
        {
            try
            {
                foreach (var item in filesHelper.GetFileList().files)
                {
                    filesHelper.DeleteFile(item.name);
                }
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);

            }
  
        }

        public ActionResult ReadDocTypeAttr(Guid streamId, int typeId = 0)
        {
            return Json(_hrUnitOfWork.CompanyRepository.GetDocTypeAttr(streamId, typeId, Language), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDocTypeAttr(IEnumerable<CompanyDocAttrViewModel> models, IEnumerable<OptionsViewModel> options)
        {
            var datasource = new DataSource<CompanyDocAttrViewModel>();
            datasource.Data = models;
            datasource.Total = models.Count();

            if (ModelState.IsValid)
            {
                if (ServerValidationEnabled)
                {
                    var errors = _hrUnitOfWork.CompanyRepository.Check(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CompanyDocAttrs",
                        Columns = Models.Utils.GetModifiedRows(ModelState.Where(a => a.Key.Contains("models"))),
                        Culture = Language
                    });

                    if (errors.Count() > 0)
                    {
                        datasource.Errors = errors;
                        return Json(datasource);
                    }
                }

                var oldCompDocAttr = _hrUnitOfWork.Repository<CompanyDocAttr>().Where(cd => cd.StreamId == models.FirstOrDefault().StreamId);
                foreach (CompanyDocAttrViewModel model in models)
                {
                    //--CompanyDocAttr
                    var companyDocAttr = oldCompDocAttr.FirstOrDefault(cd => cd.AttributeId == model.Id && cd.StreamId == model.StreamId);
                    //Don't save in CompanyDocAttr table if not exists and value == null 
                    if (model.Insert && model.Value != null) //Add  
                    {
                        companyDocAttr = new CompanyDocAttr();
                        companyDocAttr.AttributeId = model.Id;
                        companyDocAttr.Value = getValue(model);
                        companyDocAttr.ValueId = model.ValueId;
                        companyDocAttr.StreamId = model.StreamId;

                        _hrUnitOfWork.LookUpRepository.Add(companyDocAttr);
                    }
                    else if (oldCompDocAttr != null) //Update
                    {
                        companyDocAttr.Value = getValue(model);
                        companyDocAttr.ValueId = model.ValueId;

                        _hrUnitOfWork.LookUpRepository.Attach(companyDocAttr);
                        _hrUnitOfWork.LookUpRepository.Entry(companyDocAttr).State = EntityState.Modified;
                    }

                }
                datasource.Errors = SaveChanges(Language);
            }
            else
                datasource.Errors = Models.Utils.ParseErrors(ModelState.Values);

            if (datasource.Errors.Count() > 0)
                return Json(datasource);
            else
                return Json(datasource.Data);
        }

        private string getValue(CompanyDocAttrViewModel model)
        {
            string value;
            switch(model.InputType)
            {
                //case 3:  //select
                //    value = model.ValueText;
                //    break;
                case 4:  //date
                    value = DateTime.Parse(model.ValueText).ToString("yyyy/MM/dd");
                    break;
                case 5:  //time
                    value =  model.ValueText;
                    break;
                case 6:  //datetime
                    value = DateTime.Parse(model.ValueText).ToString("yyyy/MM/dd hh:mm");
                    break;
                default:
                    value = model.Value;
                    break;
            }
            return value;
        }

        public ActionResult GetLookUpCodesLists(int typeId)
        {
            return Json(_hrUnitOfWork.CompanyRepository.GetLookUpCodesLists(typeId, Language), JsonRequestBehavior.AllowGet);
        }
    }
}





