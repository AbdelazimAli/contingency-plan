using HR_uploader.Helpers;
using Interface.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;


namespace WebApp.Controllers.NewApi
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UtilitiesController : BaseApiController
    {
        protected IHrUnitOfWork hrUnitOfWork { get; private set; }
        FilesHelper filesHelper;
        string tempPath = "~/uploadercash/";
        string serverMapPath = "~/Files/uploadercash/";
        string UrlBase = "/Files/uploadercash/";
        string DeleteURL = "/FileUpload/DeleteFile/?file=";
        string DeleteType = "GET";
        private string StorageRoot
        {
            get { return Path.Combine(HostingEnvironment.MapPath(serverMapPath)); }
        }
        public UtilitiesController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            hrUnitOfWork = unitOfWork;
            filesHelper = new FilesHelper(DeleteURL, DeleteType, StorageRoot, UrlBase, tempPath, serverMapPath);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Utilities/GetEmployeeCustody")]
        public IHttpActionResult GetEmployeeCustody(EmployeeCustody model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var res = hrUnitOfWork.CustodyRepository.GetEmpCustody(model.EmpId, model.CompanyId, model.Culture).Select(c => new
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Serial = c.SerialNo,
                Category = c.CustodyCatId,
                Disposal = (c.Disposal) ? "Consumed" : "Permanent",
                Quantity = c.Qty
            }).ToList();
            if (res.Count() == 0)
            {
                return NotFound();
            }
            return Ok(res);

        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("newApi/Utilities/GetEmployeeDocs")]
        public IHttpActionResult GetEmployeeDocs(EmployeeDocs model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var comdocs = hrUnitOfWork.CompanyRepository.GetDocsViews(model.Source, model.SourceId);
            var r = new List<ViewDataUploadFilesResult>();
            string fullPath = Path.Combine(StorageRoot);
            if (Directory.Exists(fullPath))
            {
                DirectoryInfo dir = new DirectoryInfo(fullPath);
                foreach (var item in comdocs)
                {
                    string filefullpath = HttpContext.Current.Server.MapPath(serverMapPath + item.name);

                    if (!File.Exists(filefullpath))
                    {
                        File.WriteAllBytes(filefullpath, item.file_stream);
                    }
                    var ThumbfullPath = HttpContext.Current.Server.MapPath(serverMapPath) + "thumbs/";
                    string fileThumb = item.name + ".80x80.jpg";
                    var ThumbfullPath2 = ThumbfullPath + fileThumb;
                    if (!File.Exists(ThumbfullPath2))
                    {
                        if (item.file_type.ToLower() == "jpeg" || item.file_type.ToLower() == "jpg" || item.file_type.ToLower() == "png" || item.file_type.ToLower() == "gif" || item.file_type.ToLower() == "bmp")
                        {
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

            var res = hrUnitOfWork.CompanyRepository.GetCompanyDocsViews(model.Source, model.SourceId).Where(a => a.AccessLevel == 2);

            var result = from m in res
                         join file in r on m.stream_id equals file.sid
                         orderby m.creation_time.Value
                         select (new
                         {
                             name = file.name,
                             thumbnailUrl = file.thumbnailUrl,
                             type = file.type,
                             url = file.url,
                             size=file.size,
                             Description = m.Description,
                             

                         });

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }

    }

    public class EmployeeDocs
    {
        public string Source { get; set; }
        public int SourceId { get; set; }
    }

    public class EmployeeCustody
    {
        public int EmpId { get; set; }
        public int CompanyId { get; set; }
        public string Culture { get; set; }
    }
}
