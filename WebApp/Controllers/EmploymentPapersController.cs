using Db.Persistence.BLL;
using Interface.Core;
using Model.Domain;
using Model.ViewModel.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.Extensions;
using Db.Persistence.Services;
using WebApp.Models;
using Hangfire;
using System.Web;
using WebApp.Helpers;
using Db.Persistence;

namespace WebApp.Controllers
{
    public class EmploymentPapersController : BaseController
    {
        private IHrUnitOfWork _hrUnitOfWork;
        public EmploymentPapersController(IHrUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _hrUnitOfWork = unitOfWork;
        }

        #region Views & Partials
        public ActionResult Index(int EmpID)
        {
            int Gender, JobID, Nationality;
            EmploymentPapers _EmploymentPapers = new EmploymentPapers(_hrUnitOfWork);
            EmploymentPapersIndexViewModel Model = new EmploymentPapersIndexViewModel();


            Model.EmpDocsTypes = _EmploymentPapers.GetPapers(EmpID, CompanyId, User.Identity.GetCulture(), out JobID, out Gender, out Nationality);
            string RequiredDocTypeIDs = _EmploymentPapers.GetRequiredDocTypeIDs(Model.EmpDocsTypes);

            Model.EmpID = EmpID;
            Model.GeneralUrl = Url.Action("UploadPaperPartial", "EmploymentPapers", new
            {
                area = "",
                EmpID = EmpID,
                JobID = JobID,
                Gender = Gender,
                Nationality = Nationality,
                RequiredDocTypeIDs= RequiredDocTypeIDs,

            });

      
           // _EmploymentPapers.UpdateEmp_EmpPapersStatus(EmpID,Model.EmpDocsTypes);
            return View(Model);
        }

        public PartialViewResult UploadPaperPartial(int EmpID, int JobID, int Gender, int Nationality, int DocTypeID = 0, bool HasExpiryDate = false, Guid? Stream_Id = null,string RequiredDocTypeIDs="",bool IsEmpPaper=false)
        {
            int DocumentType = 0;
            bool IsAddNewOthers = true;
            List<SelectListItem> DocTypesList = new List<SelectListItem>();
            EmploymentPapers _EmploymentPapers = new EmploymentPapers(_hrUnitOfWork);

            if (DocTypeID == 0)
            {
                IsAddNewOthers = false;
                DocTypesList = _EmploymentPapers.GetPapers_Others(JobID, Gender, Nationality, CompanyId, User.Identity.GetCulture()).Select(d => new SelectListItem() { Value = d.Id.ToString(), Text = d.Name }).ToList();
            }
            else
            {
                DocumentType = _EmploymentPapers.GetDocumentType_ByDocTypeID(DocTypeID);
            }

            EmploymentPapersUploadVModel Model = new EmploymentPapersUploadVModel();
            Model.RequiredDocTypeIDs = RequiredDocTypeIDs;
            Model.ValidFileExtensions = "'.jpg','.jpeg','.bmp','.png','.gif','.pdf'";/*,'.doc','.docx','.xls','.xlsx'*/
            Model.DocumenType = DocumentType;
            Model.TypeId = DocTypeID;
            Model.DocTypesList = DocTypesList;
            Model.IsAddNewOthers = IsAddNewOthers;
            Model.HasExpiryDate = HasExpiryDate;
            Model.EmpID = EmpID;


            if (Stream_Id != null)
            {
                Model.IsUploaded = true;
                Model.Stream_Id = Stream_Id;

                CompanyDocsViews DocView = _EmploymentPapers.GetCompanyDocsViews_ByStreamID(Stream_Id.Value);
                if (DocView != null)
                {
                    Model.Keyword = DocView.Keyword;
                    Model.name = DocView.name;
                    Model.Description = DocView.Description;
                    Model.ExpireDate_string = DocView.ExpiryDate.ToMyDateString(User.Identity.GetCulture(), "dd/MM/yyyy");
                    Model.File_Type = DocView.file_type;
                    Model.IsEmpPaper = IsEmpPaper;
                }

            }
            //Model.OldModel_Serialized = new JavaScriptSerializer().Serialize(Model);
            FillBasicData(false, false,true, true);

            return PartialView("_UploadPaperPartial", Model);
        }

        #endregion

        #region Get Sub Data
        public FileContentResult GetFile(Guid? Stream_Id)
        {
            EmploymentPapers _EmploymentPapers = new EmploymentPapers(_hrUnitOfWork);
            byte[] fileData;
            string File_Type;
            _EmploymentPapers.GetCompanyDocsViewsImage(Stream_Id, out fileData, out File_Type);

            if (fileData != null)
                return File(fileData, File_Type);
            else
                return null;
        }

      
        public JsonResult HasExpirationDate(int DocTypeID)
        {
            bool HasExpirationDate = false;
            try
            {
                EmploymentPapers _EmploymentPapers = new EmploymentPapers(_hrUnitOfWork);
                HasExpirationDate = _EmploymentPapers.HasExpirationDate(DocTypeID);
            }
            catch
            {
            }
            return Json(new { HasExpirationDate = HasExpirationDate });
        }

        [HttpGet]
        public JsonResult ReadDocTypeAttr(Guid? streamId = null, int typeId = 0)
        {
            EmploymentPapers _EmploymentPapers = new EmploymentPapers(_hrUnitOfWork);
            return Json(_EmploymentPapers.ReadDocTypeAttr(Language, streamId, typeId), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save & Remove
        public JsonResult SaveDoc(EmploymentPapersUploadVModel Model)
        { 
            EmploymentPapers _EmploymentPapers = new EmploymentPapers(HrUnitOfWork);
            _EmploymentPapers.SetImageToModel(ref Model, HttpContext);

            string ValidationErrorMessage;
            if (!_EmploymentPapers.IsValid(ModelState, Model, Culture, out ValidationErrorMessage))
                return Json(new { Result = false, Message = ValidationErrorMessage });

            if (Model.Images != null)
            {
                string contentType;
                Model.ImageStream = ITextSharpProcesses.CompineFilesIntoPDF(Model.Images, out contentType);
                Model.ContentType = contentType;
            }

            int CurrentEmpStatus, OldEmpStatus;
            string ErrorMessage;
            bool ProcessDone = _EmploymentPapers.Save(Model, CompanyId, User.Identity.Name, User.Identity.GetCulture(), out ErrorMessage,out CurrentEmpStatus,out OldEmpStatus);
           
            if(ProcessDone)
            return Json(new { Result = ProcessDone, Message = MsgUtils.Instance.Trls("SaveProcessSuceeded"), CurrentEmpStatus= CurrentEmpStatus, OldEmpStatus= OldEmpStatus });
            else
                return Json(new { Result = ProcessDone, Message = ErrorMessage });

        }


        [HttpGet]
        public JsonResult RemoveDocument(Guid Stream_Id,string RequiredDocTypeIDs)
        {
            string ErrorMessage;
            int CurrentEmpStatus, OldEmpStatus;
            EmploymentPapers _EmploymentPapers = new EmploymentPapers(HrUnitOfWork);
            bool IsRemoved = _EmploymentPapers.RemoveDocument(Stream_Id, RequiredDocTypeIDs, out ErrorMessage,out CurrentEmpStatus,out OldEmpStatus);

            if (IsRemoved)
                return Json(new { Result = IsRemoved, Message = MsgUtils.Instance.Trls("DeleteSuceeded"), CurrentEmpStatus = CurrentEmpStatus, OldEmpStatus = OldEmpStatus }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Result = IsRemoved, Message = ErrorMessage }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Notifications
        public JsonResult RunNotificationsAlgorithm(int Id, int Period, bool Enable, bool OnStopError, bool Invoke)
        {
            string msg = "Enabled";
            if (Enable)
            {

                try
                {
                    int UNHour,   UNMinute;
                    DateTimeServices.GetTimeUniversal(9, 0,User.Identity.GetTimeZone(), out UNHour, out UNMinute);
                    RecurringJob.AddOrUpdate(Id.ToString(), () => EmploymentPapers.RunNotificationsAlgorithm(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString,Language), Cron.Daily(UNHour,UNMinute));
                }
                catch
                {
                    if (OnStopError)
                        RecurringJob.RemoveIfExists(Id.ToString());
                }


                if (!Invoke)
                {
                    RecurringJob.Trigger(Id.ToString());
                    EmploymentPapers _EmploymentPapers = new EmploymentPapers(HrUnitOfWork);
                    _EmploymentPapers.UpdateTimeofTask(Id,Language);
                }
            }
            else
                msg = "Disabled";
            return Json(msg, JsonRequestBehavior.AllowGet);

           
        }
      

    
        #endregion

    }


}