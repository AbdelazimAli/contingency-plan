using Interface.Core;
using Model.Domain;
using Model.ViewModel;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Extensions;

namespace WebApp.Controllers
{
    public class CompanyStructureController : BaseController
    {
        // GET: CompanyStructure
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
        public CompanyStructureController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            ViewBag.Job = _hrUnitOfWork.JobRepository.ReadJobs(CompanyId, Language,0).Select(a => new { value = a.Id, text = a.LocalName });
            ViewBag.StructType = _hrUnitOfWork.LookUpRepository.GetLookUpUserCodes("CompanyStructure", Language).Select(a => new { id = a.CodeId, name = a.Title });
            return View();
        }
        public ActionResult Diagram()
        {
            var nodes = _hrUnitOfWork.CompanyStructureRepository.GetDiagram(CompanyId,Language);
            var all = nodes.ToList();
            var result = new List<CompanyDiagramViewModel>();

            foreach (var node in all.Where(a => a.ParentId == null))
            {
                result.Add(new CompanyDiagramViewModel
                {
                    Id = node.Id,
                    Employee=node.Employee,
                    Image=node.Image,
                    Name = node.Name,
                    ParentId = node.ParentId,
                    Code = node.Code,
                    colorSchema= node.colorSchema,
                    Children = AddNodes(all, node.Id),
                    HasImage = node.HasImage
                });
            }

            return View(result);
        }
        public ActionResult GetFirstmodel()
        {
            int Id = _hrUnitOfWork.Repository<CompanyStructure>().Where(a=>a.CompanyId == CompanyId).Select(a => a.Id).FirstOrDefault();
            var compstru = new CompanyStructureViewModel();
            if (Id > 0)
            compstru = _hrUnitOfWork.CompanyStructureRepository.GetStructure(Id,Language);
            return Json(compstru,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(CompanyStructureViewModel model,OptionsViewModel moreInfo)
        {
            var Message = "OK";
            List<Error> errors = new List<Error>();
            if (ModelState.IsValid)
            {

                if (ServerValidationEnabled)
                {
                    errors = _hrUnitOfWork.CompanyStructureRepository.CheckForm(new CheckParm
                    {
                        CompanyId = CompanyId,
                        ObjectName = "CompanyStructure",
                        TableName = "CompanyStructures",
                        Columns = Models.Utils.GetColumnViews(ModelState.Where(a => !a.Key.Contains('.'))),
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
            }else
            {
                return Json(Models.Utils.ParseFormErrors(ModelState));
            }
            CompanyStructure compStruc;
            if (model.Id == 0) // New
            {
                 compStruc = new CompanyStructure();
                _hrUnitOfWork.PositionRepository.AddLName(Language, compStruc.Name, model.Name, model.LocalName);

                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = compStruc,
                    Source = model,
                    ObjectName = "CompanyStructure",
                    Version = Convert.ToByte(Request.Form["Version"]),
                    Options = moreInfo,
                    Transtype = TransType.Insert
                });
                compStruc.CompanyId = CompanyId;
                compStruc.CreatedTime = DateTime.Now;
                compStruc.CreatedUser = UserName;
                _hrUnitOfWork.CompanyStructureRepository.Add(compStruc);
            }
            else // Edit
            {
                 compStruc = _hrUnitOfWork.Repository<CompanyStructure>().Where(a => a.Id == model.Id).FirstOrDefault();
                _hrUnitOfWork.PositionRepository.AddLName(Language, compStruc.Name, model.Name, model.LocalName);

                AutoMapper(new Models.AutoMapperParm
                {
                    Destination = compStruc,
                    Source = model,
                    ObjectName = "CompanyStructure",
                    Version = 0,
                    Options = moreInfo,
                    Transtype = TransType.Update
                });
                compStruc.ModifiedTime = DateTime.Now;
                compStruc.ModifiedUser = UserName;
                _hrUnitOfWork.CompanyStructureRepository.Attach(compStruc);
                _hrUnitOfWork.CompanyStructureRepository.Entry(compStruc).State = EntityState.Modified;
            }

            try
            {
                var err = SaveChanges(Language);
                if (err.Count() > 0)
                {
                    foreach (var item in err)
                    {
                        Message = item.errors.Select(a => a.message).FirstOrDefault();
                    }
                    return Json(Message);
                }

            }
            catch (Exception ex)
            {
                var msg = _hrUnitOfWork.HandleDbExceptions(ex, Language);
                if (msg.Length > 0)
                    return Json(msg);
            }

            return Json(Message);
        }

        //public ActionResult CheckLeavRequest(IEnumerable<DeptLeavePlanViewModel> model)
        //{
        //    string message = "OK";
        //    foreach (var item in model)
        //    {
        //        int Requestcount = _hrUnitOfWork.CompanyStructureRepository.CheckLeaveRequests(item.FromDate, item.ToDate);
        //        if (Requestcount > 0)
        //            message="Warning";
        //    }
        //    return Json(message,JsonRequestBehavior.AllowGet);
        //}
        private IList<CompanyDiagramViewModel> AddNodes (IList<CompanyDiagramViewModel> all, int ParentId)
        {
            return all.Where(a => a.ParentId == ParentId).Select(a => new CompanyDiagramViewModel
            {
                Id = a.Id,
                Employee = a.Employee,
                Image = a.Image,
                Name = a.Name,
                ParentId = a.ParentId,
                Code = a.Code,
                colorSchema =a.colorSchema,
                Children = AddNodes(all, a.Id),
                HasImage = a.HasImage
            }).ToList();
        }    
        public ActionResult GetModel(int? Id)
        {
            CompanyStructureViewModel compstru;
            if (Id != 0)
                compstru = _hrUnitOfWork.CompanyStructureRepository.GetStructure(Id,Language);
            else
                compstru = new CompanyStructureViewModel();
            return Json(compstru,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string Sort)
        {
            var Message = "OK";
            DataSource<CompanyStructureViewModel> Source = new DataSource<CompanyStructureViewModel>();
            var companyId = CompanyId;
            var list =  _hrUnitOfWork.Repository<CompanyStructure>().Where(a => a.CompanyId == companyId && a.Sort.StartsWith(Sort)).ToList();
            foreach (var item in list)
            {
                    _hrUnitOfWork.MenuRepository.RemoveLName(Language, item.Name);               
            }
            _hrUnitOfWork.CompanyStructureRepository.RemoveRange(list);
            Source.Errors = SaveChanges(Language);

            if (Source.Errors.Count() > 0)
                return Json(Source);
            else
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
    }
    
}