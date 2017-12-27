using HR_uploader.Helpers;
using Interface.Core;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Controllers.Api;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WebApp.Models;
using Model.Domain;
using Model.ViewModel;
using System.Data.Entity;
using System.IO;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Routing;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using Model.ViewModel.Personnel;

namespace WebApp.Controllers.NewApi
{


    public class UploadedFileVm
    {
        public UploadedFileVm()
        {
            Files = new List<byte[]>();
            FileDetails = new List<string>();
        }
        public IEnumerable<byte[]> Files { get; set; }
        public string Source { get; set; }
        public int TaskId { get; set; }
        public int CompanyId { get; set; }
        public string Language { get; set; }
        public List<string> FileDetails { get; set; }


        //IEnumerable<HttpPostedFileBase> files, string Source, int TaskId
    }

    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MobileTasksController : BaseApiController
    {
        
        protected IHrUnitOfWork hrUnitOfWork { get; private set; }

        public MobileTasksController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            hrUnitOfWork = unitOfWork;
        }
        // //
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("newApi/MobileTasks/getAllTasks")]
        public IHttpActionResult getAllTasks(int emp_id)
        {
            var tasks = hrUnitOfWork.CheckListRepository.ReadEmployeeTasksGrid(emp_id, "ar-EG");
            return Ok(tasks);
        }
        // //
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("newApi/MobileTasks/getTask")]
        public IHttpActionResult getTaskById(int task_id)
        {
            var task = hrUnitOfWork.CheckListRepository.GetEmpTask(task_id);
            return Ok(task);
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.Route("newApi/MobileTasks/PostFile")]
        public IHttpActionResult UploadFile(UploadedFileVm model)
        {

            var username = HttpContext.Current.User.Identity.Name;
            var i = 0;
            if (model.Files.Count() > 0)
            {
                foreach (var item in model.Files)
                {
                    //FileStream fileStream = new FileStream(HttpContext.Current.Server.MapPath(serverMapPath + model.FileDetails[i]), FileMode.Create, FileAccess.ReadWrite);
                    //fileStream.Write(item, 0, item.Length);
                    //fileStream.Close();

                    CompanyDocsViews doc = new CompanyDocsViews()
                    {
                        CompanyId = model.CompanyId,
                        //name =  fileStream.Name,
                        name = model.FileDetails[i],
                        CreatedUser = username,
                        Source = model.Source,
                        SourceId = model.TaskId,
                        file_stream = item
                    };

                    hrUnitOfWork.CompanyRepository.Add(doc);
                    i++;
                }
            }
            //else
            //{
            //    CompanyDocsViews doc = new CompanyDocsViews()
            //    {
            //        CompanyId = model.CompanyId,
            //        //name = model.FileDetails[i],
            //        name = string.Empty,
            //        CreatedUser = username,
            //        Source = model.Source,
            //        SourceId = model.TaskId
            //    };

            //    hrUnitOfWork.CompanyRepository.Add(doc);
            //}
            EmpTask oldTask = hrUnitOfWork.CheckListRepository.GetEmpTask(model.TaskId);

            //hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "EndTime", ValueBefore = oldTask.EndTime.ToString(), ValueAfter = DateTime.Now.ToString(), ObjectName = "EmpTasksForm",  CompanyId = model.CompanyId, SourceId = oldTask.Id.ToString(), UserName = username });
            //hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "Status", ValueBefore = oldTask.Status.ToString(), ValueAfter = "2", ObjectName = "EmpTasksForm",  CompanyId = model.CompanyId, SourceId = oldTask.Id.ToString(), UserName = username });
           
            if(oldTask.StartTime > DateTime.Now)
            {
                oldTask.StartTime = DateTime.Now;
            }

            oldTask.EndTime = DateTime.Now;
            oldTask.Status = 2;
            TimeSpan duration = oldTask.EndTime.Value - oldTask.StartTime.Value;



            switch (oldTask.Unit)
            {
                case 0://Minutes
                    oldTask.Duration = Convert.ToSByte(duration.Minutes);
                    break;
                case 1: //Hours
                    oldTask.Duration = Convert.ToSByte(duration.Hours);
                    break;
                case 2: //Days
                    oldTask.Duration = Convert.ToSByte(duration.Days);
                    break;
                case 3: //Weeks
                    oldTask.Duration = Convert.ToSByte(duration.Days / 7);
                    break;
                case 4: //Months
                    oldTask.Duration = Convert.ToSByte(duration.Days / 365.25 * 12);
                    break;
            }
            //hrUnitOfWork.LeaveRepository.AddTrail(new AddTrailViewModel { ColumnName = "Status", ValueBefore = "", ValueAfter = oldTask.Duration.ToString(), ObjectName = "EmpTasksForm", Version = version, CompanyId = model.CompanyId, SourceId = oldTask.Id.ToString(), UserName = UserName });

            hrUnitOfWork.CheckListRepository.AssignNextTask(oldTask);
            oldTask.ModifiedTime = DateTime.Now;
            oldTask.ModifiedUser = username;

            var sc = SaveChanges(model.Language);
            return Ok(sc);
        }


        // //
        //[System.Web.Http.HttpPost]
        //[System.Web.Http.AllowAnonymous]
        //[System.Web.Http.Route("newApi/MobileTasks/PostFile")]
        //public Task<HttpResponseMessage> PostFile()
        //{
        //    HttpRequestMessage request = Request;
        //    if (!request.Content.IsMimeMultipartContent())
        //    { throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType); }
        //    string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/uploads");
        //    var provider = new MultipartFormDataStreamProvider(root);
        //    var task = request.Content.ReadAsMultipartAsync(provider).ContinueWith<HttpResponseMessage>(o =>
        //    {
        //        //string file1 = provider.BodyPartFileNames.First().Value;
        //        return new HttpResponseMessage()
        //        {
        //            Content = new StringContent("file uploadedd.")
        //        };
        //    });
        //    return task;
        //}


        // //
        //public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> files, string id)
        //{
        //    HttpServerUtilityBase server = null ;
        //    string Path = server.MapPath("/SpecialData/HrLetters/Uploaded/" + CompanyId);

        //    string Name = "";
        //    if (!Directory.Exists(Path))
        //        Directory.CreateDirectory(Path);

        //    if (id != null || files.Count() != 0)
        //    {
        //        var File = files.FirstOrDefault();
        //        Name = File.FileName.Split('.')[0] + ".docx";
        //        File.SaveAs(Path.Insert(Path.Length, "/" + Name));
        //    }
        //    return Json(new { FileName = Name }, "text/plain");
        //}


        // GET: Tasks
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[System.Web.Http.Route("newApi/Tasks/Upload")]
        //[System.Web.Http.HttpPost]
        //public JsonResult Upload(string Source, string SourceId, HttpPostedFileBase File)
        //{
        //    var resultList = new List<ViewDataUploadFilesResult>();
        //    File.SaveAs()


        //    var CompanyDocs = hrUnitOfWork.Repository<CompanyDocsViews>()
        //        .Where(d => d.Source == Source && d.SourceId == sid).Select(a => a.name).ToList();
        //    bool Exist = false;
        //    string name = files.files.FirstOrDefault().name;
        //    if (CompanyDocs.Contains(name))
        //    {
        //        string checkName = name;
        //        Exist = true;
        //        for (int i = 1; i <= CompanyDocs.Count; i++)
        //        {
        //            checkName = string.Format("copy{0}_{1}", i, name);
        //            if (!CompanyDocs.Contains(checkName))
        //            {
        //                name = checkName;
        //                break;
        //            }
        //        }
        //    }


        //    foreach (var item in files.files)
        //    {

        //        CompanyDocsViews doc = new CompanyDocsViews()
        //        {
        //            CompanyId = (Source == "Company" ? sid : CompanyId),
        //            name = Exist ? name : item.name,
        //            CreatedUser = username,
        //            Source = Source,
        //            SourceId = sid,
        //            file_stream = System.IO.File.ReadAllBytes(Server.MapPath(serverMapPath + item.name))
        //        };

        //        names.Add(doc.name);
        //        _hrUnitOfWork.CompanyRepository.Add(doc);
        //    }

        //    var errors = SaveChanges(Language);

        //    var model = _hrUnitOfWork.Repository<CompanyDocsViews>()
        //        .Where(d => d.Source == Source && d.SourceId == sid && names.Contains(d.name))
        //        .Select(d => new
        //        {
        //            sid = d.stream_id,
        //            time = d.creation_time,
        //            name = d.name
        //        }).ToList();

        //    var result = from m in model
        //                 join file in files.files on m.name equals Exist ? name : file.name
        //                 select (new ViewDataUploadFilesResult
        //                 {
        //                     sid = m.sid,
        //                     name = Exist ? name : file.name,
        //                     deleteType = file.deleteType,
        //                     deleteUrl = DeleteURL + m.sid,
        //                     size = file.size,
        //                     thumbnailUrl = file.thumbnailUrl,
        //                     type = file.type,
        //                     url = file.url,
        //                     last_access_time = m.time.Value.ToString("dd/MM/yyyy hh:mm"),
        //                     creation_time = m.time.Value.ToString("dd/MM/yyyy hh:mm"),
        //                     CreatedUser = username
        //                 });

        //    return Json(new JsonFiles(result.ToList()), JsonRequestBehavior.AllowGet);
        //}
    }
}