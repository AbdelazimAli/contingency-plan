using Interface.Core;
using System.Collections.Generic;
using System.Linq;
using Model.ViewModel.Personnel;
using System;
using Model.Domain;
using Model.ViewModel;
using Db.Persistence.Services;
using System.Web.Mvc;
using Model.Domain.Notifications;
using System.Web;

namespace Db.Persistence.BLL
{
    public class EmploymentPapers
    {
        IHrUnitOfWork hrUnitOfWork;
        public EmploymentPapers(IHrUnitOfWork _hrUnitOfWork)
        {
            hrUnitOfWork = _hrUnitOfWork;
        }

        #region Get Data
        public bool HasExpirationDate(int DocTypeID)
        {
            return hrUnitOfWork.DocTypesRepository.HasExpirationDate(DocTypeID);
        }
        public int GetDocumentType_ByDocTypeID(int DocTypeID)
        {
            return hrUnitOfWork.DocTypesRepository.GetDocumentType_ByDocTypeID(DocTypeID);
        }

        public CompanyDocsViews GetCompanyDocsViews_ByStreamID(Guid StreamID)
        {
            return hrUnitOfWork.CompanyDocsViewsRepository.GetByStreamID(StreamID);
        }

        public void GetCompanyDocsViewsImage(Guid? Stream_Id, out byte[] ImageData, out string File_Type)
        {
            hrUnitOfWork.CompanyDocsViewsRepository.GetFileStream_ByStreamID(Stream_Id, out ImageData, out File_Type);
        }

        public IQueryable<CompanyDocAttrViewModel> ReadDocTypeAttr(string Language, Guid? streamId = null, int typeId = 0)
        {
            return hrUnitOfWork.CompanyRepository.GetDocTypeAttr(typeId, Language, streamId);
        }

        public void SetImageToModel(ref EmploymentPapersUploadVModel Model, HttpContextBase HttpContext)
        {
            try
            {
                // if (Model.Images != null&&!Model.Images.Any(i=>i==null))
                //return;

                if (HttpContext.Request.Files != null && HttpContext.Request.Files.Count > 0)
                {
                    List<HttpPostedFileBase> ImagesList = new List<HttpPostedFileBase>();
                    for (int i = 0; i < HttpContext.Request.Files.Count; i++)
                    {
                        var file = HttpContext.Request.Files[i];
                        if (file.ContentLength > 0)
                            ImagesList.Add(file);
                    }

                    Model.Images = ImagesList.Count() > 0 ? ImagesList.ToArray() : null;
                }


            }
            catch
            {

            }
        }

        public string GetRequiredDocTypeIDs(List<Paper_UploadStatus> PapersList)
        {
            try
            {
                //var list = string.Join(",", PapersList.Where(a => a.IsRequired).Select(a => a.Paper.Id).ToArray());
                return PapersList.Where(a => a.IsRequired).Select(a => a.Paper.Id.ToString()).Aggregate<string>((x1, x2) => x1 + "," + x2).ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region Show Papers

        public IQueryable<DocTypeFormViewModel> GetPapers_Others(int JobID, int Gender, int Nationality, int CompanyID, string Lang)
        {
            try
            {
                List<int> Exclude_SystemCodes = new List<int> { (int)Constants.SystemCodes.DocType.DocTypeEnum.Employment_Papers };
                return hrUnitOfWork.DocTypesRepository.GetPapers(JobID, Gender, Nationality, CompanyID, Lang, new List<int>(), Exclude_SystemCodes);
            }
            catch
            {
                return new List<DocTypeFormViewModel>().AsQueryable();
            }
        }

        public List<Paper_UploadStatus> GetPapers(int EmpID, int CompanyID, string Lang, out int JobID, out int Gender, out int Nationality)
        {
            JobID = 0; Gender = 0; Nationality = 0;
            try
            {
                hrUnitOfWork.EmployeeRepository.GetAssignment(EmpID, out JobID, out Gender, out Nationality);

                List<int> Include_SystemCodes = new List<int> { (int)Constants.SystemCodes.DocType.DocTypeEnum.Employment_Papers };
                List<DocTypeFormViewModel> PapersList = hrUnitOfWork.DocTypesRepository.GetPapers(JobID, Gender, Nationality, CompanyID, Lang, Include_SystemCodes, new List<int>()).ToList();
                return SpecifyWhichPaperUploaded(EmpID, PapersList, Lang);
            }
            catch
            {
                return new List<Paper_UploadStatus>();
            }
        }

        private List<Paper_UploadStatus> SpecifyWhichPaperUploaded(int EmpID, List<DocTypeFormViewModel> PapersList, string Lang)
        {
            List<StreamID_DocTypeFormViewModel> AllUploadedDocsList = hrUnitOfWork.CompanyRepository.GetDocsViews_Uploaded(Constants.Sources.People, EmpID, Constants.SystemCodes.DocType.CodeName/*, (int)Constants.SystemCodes.DocType.DocTypeEnum.Employment_Papers*/, Lang);
            List<Paper_UploadStatus> Result = new List<Paper_UploadStatus>();
            try
            {
                PapersList.ForEach(p =>
                {
                    Guid? Stream_Id = null;
                    bool IsUploaded = false;
                    StreamID_DocTypeFormViewModel UploadedRecord = AllUploadedDocsList.FirstOrDefault(a => a.DocTypeFormViewModel.Id == p.Id);
                    if (/*AllUploadedDocsList.Any(a => a.Id == p.Id)*/UploadedRecord != null)
                    {
                        IsUploaded = true;
                        Stream_Id = UploadedRecord.Stream_Id;
                    }
                    Result.Add(new Paper_UploadStatus()
                    {
                        Stream_Id = Stream_Id,
                        IsEmpPaper = true,
                        IsUploaded = IsUploaded,
                        Paper = p,
                        IsRequired =(p.RequiredOpt != (int)Constants.Enumerations.RequiredOptEnum.Not_Required)
                    });
                });

                List<StreamID_DocTypeFormViewModel> UploadedDocs_NotEmpPapers = AllUploadedDocsList.Where(a => !PapersList.Select(x => x.Id).Contains(a.DocTypeFormViewModel.Id)).ToList();
                UploadedDocs_NotEmpPapers.ForEach(a =>
                {
                    Result.Add(new Paper_UploadStatus()
                    {
                        Stream_Id = a.Stream_Id,
                        IsEmpPaper = false,
                        IsUploaded = true,
                        Paper = a.DocTypeFormViewModel,
                        IsRequired =false //(a.DocTypeFormViewModel.RequiredOpt != (int)Constants.Enumerations.RequiredOptEnum.Not_Required)
                    });
                });
            }
            catch (Exception ex)
            {
            }
            return Result.OrderBy(a => a.IsUploaded).OrderByDescending(a => a.IsRequired).ToList();
        }
        #endregion

        #region Save & Remove

        //public void UpdateEmp_EmpPapersStatus(int EmpID,List<Paper_UploadStatus> EmpDocsTypes)
        //{
        //    try
        //    {
        //        double Total = EmpDocsTypes.Count(a => a.IsRequired && a.IsEmpPaper);
        //        double Uploaded = EmpDocsTypes.Count(a => a.IsRequired && a.IsEmpPaper && a.IsUploaded);

        //        // double Percentage = (Uploaded / Total) * 100;
        //        string PaperStatus = string.Format("{0} / {1}", Uploaded, Total);
        //        //Update employee record 
        //        var person = hrUnitOfWork.PeopleRepository.GetPerson(EmpID);
        //        if (person != null)
        //            person.PaperStatus = PaperStatus;

        //        hrUnitOfWork.Save();
        //    }
        //    catch
        //    {

        //    }
        //}

   


        public bool IsValid(ModelStateDictionary ModelState, EmploymentPapersUploadVModel Model, string Culture, out string ErrorMessage)
        {
            if (!Model.HasExpiryDate)
            {
                ModelState.Remove("ExpireDate_string");
            }

            bool Result = true;
            ErrorMessage = string.Empty;
            if (!ModelState.IsValid)
            {
                Result = CreateErrorMessage(MsgUtils.Instance.Trls(Culture, Constants.MessagesKeys.InvalidData), ref ErrorMessage);
            }

            if (Model.EmpID == 0)
            {
                Result = CreateErrorMessage(MsgUtils.Instance.Trls(Culture, "No employee found"), ref ErrorMessage);
            }

            if (!string.IsNullOrEmpty(Model.ExpireDate_string))
            {
                DateTime ExpireDate = Model.ExpireDate_string.ToMyDateTime(Culture);

                if (ExpireDate < DateTime.Now.Date)
                    Result = CreateErrorMessage(MsgUtils.Instance.Trls(Culture, "Invalid Expire Date"), ref ErrorMessage);
            }

            string KendoErrorMessage;
            if (!Constants.KendoGridValidation.IsValid(Model.BatchGridData_List, Culture, out KendoErrorMessage))
            {
                Result = CreateErrorMessage(KendoErrorMessage, ref ErrorMessage);
            }

            if (Model.Images == null && Model.Stream_Id == null)
            {
                Result = CreateErrorMessage(MsgUtils.Instance.Trls(Culture, "Please insert image"), ref ErrorMessage);
            }

            if (!ImageProcesses.IsValidExtension(Model.Images, Model.ValidFileExtensions))
            {
                Result = CreateErrorMessage(MsgUtils.Instance.Trls(Culture, "Invalid file extension"), ref ErrorMessage);
            }

            return Result;
        }

        private bool CreateErrorMessage(string Message, ref string ErrorMessage)
        {

            if (!string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage += " , ";

            ErrorMessage += Message;

            return false;
        }

        public bool Save(EmploymentPapersUploadVModel Model, int CompanyId, string UserName, string Culture, out string ErrorMessage, out int CurrentEmpStatus, out int OldEmpStatus)
        {
            OldEmpStatus = 0;
            CurrentEmpStatus = 0;
            ErrorMessage = string.Empty;
            List<Error> errors = new List<Error>();
            try
            {

                CompanyDocsViews doc = Save_CompanyDocsViews(Model, UserName, Culture, CompanyId, out CurrentEmpStatus, out OldEmpStatus);
                SaveGrid_Section(Model, Culture, doc);
                errors = hrUnitOfWork.SaveChanges();

                if (errors.Count() > 0)
                {
                    ErrorMessage = errors.First().errors.First().message;
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            // }

        }

        private CompanyDocsViews Save_CompanyDocsViews(EmploymentPapersUploadVModel Model, string UserName, string Culture, int CompanyId, out int CurrentEmpStatus, out int OldEmpStatus)
        {
            OldEmpStatus = 0;
            CurrentEmpStatus = 0;

            CompanyDocsViews doc = null;
            if (Model.Stream_Id != null)
            {
                doc = hrUnitOfWork.CompanyDocsViewsRepository.GetByStreamID(Model.Stream_Id.Value);
                doc.ModifiedUser = UserName;
                FillCommonData_CompanyDocsViews(ref doc, Model, Culture);
            }
            else
            {
                doc = new CompanyDocsViews();
                doc.TypeId = Model.TypeId;
                doc.CompanyId = CompanyId;
                doc.CreatedUser = UserName;
                doc.Source = Constants.Sources.People;
                doc.SourceId = Model.EmpID;
                FillCommonData_CompanyDocsViews(ref doc, Model, Culture);
            }


            CurrentEmpStatus = UpdatePersonStatus(Model.EmpID, Model.RequiredDocTypeIDsList, Model.TypeId, null, out OldEmpStatus);
            return doc;
        }
        public int UpdatePersonStatus(int EmpID, List<int> RequiredDocTypeIDsList, int? DocTypeID_AddEditeMode, int? DocTypeID_DeleteMode, out int OldEmpStatus)
        {
            int CurrentEmpStatus = 0;
            OldEmpStatus = 0;
            try
            {
                bool IsAllReqPapersUploaded = hrUnitOfWork.CompanyRepository.IsAllRequiredPapersUploaded(Constants.Sources.People, EmpID, RequiredDocTypeIDsList, DocTypeID_AddEditeMode, DocTypeID_DeleteMode);
                Person per = hrUnitOfWork.Repository<Person>().SingleOrDefault<Person>(a => a.Id == EmpID);

                if (per != null)
                {
                    OldEmpStatus = Convert.ToInt32(per.Status);
                    if (IsAllReqPapersUploaded)
                    {
                        if (per.Status < PersonStatus.Assignment)
                            per.Status = PersonStatus.Assignment;
                    }
                    else
                    {
                        if (per.Status == PersonStatus.Assignment)
                            per.Status = PersonStatus.Papers;
                    }
                    CurrentEmpStatus = Convert.ToInt32(per.Status);
                }
            }
            catch
            {
            }
            return CurrentEmpStatus;
        }
        private void FillCommonData_CompanyDocsViews(ref CompanyDocsViews doc, EmploymentPapersUploadVModel Model, string Culture)
        {
            //byte[] ImageStream = null;

            //if (Model.Images != null)
            //    ImageStream =  ImageProcesses.ReadFully(Model.Images.InputStream);

            doc.name = Model.name;
            doc.Keyword = Model.Keyword;
            doc.Description = Model.Description;
            doc.ExpiryDate = (!string.IsNullOrEmpty(Model.ExpireDate_string)) ? (DateTime?)Model.ExpireDate_string.ToMyDateTime(Culture) : null;
            if (Model.ImageStream != null)
            {
                doc.file_stream = Model.ImageStream;
                doc.file_type = Model.ContentType;
            }

        }
        private void SaveGrid_Section(EmploymentPapersUploadVModel Model, string Culture, CompanyDocsViews doc)
        {
            IList<CompanyDocAttr> oldCompDocAttr = null;
            if (Model.Stream_Id != null)
                oldCompDocAttr = hrUnitOfWork.CompanyDocAttrRepository.GetDocAttrByStreamId(Model.Stream_Id.Value);

            foreach (CompanyDocAttrViewModel UpdatedAttr in Model.BatchGridData_List)
            {
                //if (UpdatedAttr. !string.IsNullOrEmpty(UpdatedAttr.Value))
                //{
                    if (!UpdatedAttr.Insert && oldCompDocAttr != null)
                    {
                        var docAttr = oldCompDocAttr.Where(a => a.AttributeId == UpdatedAttr.Id).FirstOrDefault();
                        docAttr.Value = Constants.CompanyDocAttr.getValue(UpdatedAttr, Culture);
                        docAttr.ValueId = UpdatedAttr.ValueId;
                    }
                    else
                    {
                        var docAttr = new CompanyDocAttr();
                        docAttr.AttributeId = UpdatedAttr.Id;
                        docAttr.Value = Constants.CompanyDocAttr.getValue(UpdatedAttr, Culture);
                        docAttr.ValueId = UpdatedAttr.ValueId;
                        doc.CompanyDocAttrs.Add(docAttr);
                    }
                //}
            }

            if (Model.Stream_Id == null)
                hrUnitOfWork.CompanyDocsViewsRepository.Add(doc);
        }
        public bool RemoveDocument(Guid Stream_Id, string RequiredDocTypeIDs, out string ErrorMessage, out int CurrentEmpStatus, out int OldEmpStatus)
        {
            OldEmpStatus = 0;
            CurrentEmpStatus = 0;
            ErrorMessage = string.Empty;
            try
            {
                CompanyDocsViews doc = hrUnitOfWork.CompanyDocsViewsRepository.GetByStreamID(Stream_Id);
                hrUnitOfWork.CompanyRepository.Remove(doc);

                List<int> RequiredDocTypeIDsList = GetListFromCommaSeperatedString(RequiredDocTypeIDs);
                CurrentEmpStatus = UpdatePersonStatus(doc.SourceId.Value, RequiredDocTypeIDsList, null, doc.TypeId, out OldEmpStatus);


                List<Error> errors = hrUnitOfWork.SaveChanges();
                if (errors.Count() > 0)
                {
                    ErrorMessage = errors.First().errors.First().message;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }

        }

        private List<int> GetListFromCommaSeperatedString(string CommaSeperated)
        {
            List<int> IDsList = new List<int>();
            try
            {

                if (!string.IsNullOrEmpty(CommaSeperated))
                    IDsList = CommaSeperated.Split(',').Select(int.Parse).ToList();
            }
            catch
            {
            }
            return IDsList;
        }
        #endregion

        #region Notifications
        public static void RunNotificationsAlgorithm(string ConnectionString,string Language)
        {
            HrUnitOfWork unitofwork = new HrUnitOfWork(new HrContextFactory(ConnectionString));

            try
            {
                DateTime Today = DateTime.Now.Date;
                List<EmploymentPaper_ToNotify> EmpPapers_ToNotify = unitofwork.CompanyDocsViewsRepository.EmploymentPapersForNotifications();

                string EmpIDs = (EmpPapers_ToNotify.Any()) ? EmpPapers_ToNotify.Select(a => a.EmpID.ToString()).Aggregate<string>((x1, x2) => x1 + "," + x2).ToString() : "";
                List<FormDropDown> EmpsLangs = unitofwork.MeetingRepository.GetUsersLang(EmpIDs);

                List<NotifyLetter> NotifyLettersList = new List<NotifyLetter>();
                EmpPapers_ToNotify.ForEach(e =>
                {

                    FormDropDown EmpLang = EmpsLangs.Where(a => a.id == e.EmpID).FirstOrDefault();

                    string Lang = "";
                    if (EmpLang != null)
                        Lang = EmpLang.name;

                    NotifyLetter NL = new NotifyLetter()
                    {
                        CompanyId = e.CompanyId.Value,
                        EmpId = e.EmpID,
                        NotifyDate = Today,
                        NotifySource = MsgUtils.Instance.Trls(Lang, e.DocTypeName),
                        SourceId = e.Stream_Id.ToString(),
                        Sent = true,
                        EventDate = e.ExpiryDate.Value,
                        Description = MsgUtils.Instance.Trls(Lang, "you must renew") + " " + e.PaperFileName + " " + MsgUtils.Instance.Trls(Lang, "Before") + " " + e.ExpiryDate.Value.ToMyDateString(Lang, "yyyy-MM-dd")

                    };
                    //unitofwork.NotifyLetterRepository.Add(NL);
                    NotifyLettersList.Add(NL);
                });

                string ErrorMessage;
                AddNotifyLetters AddNotifyLetters = new AddNotifyLetters(unitofwork, NotifyLettersList, Language);
                bool Result = AddNotifyLetters.Run(out ErrorMessage);
                //unitofwork.SaveChanges();

            }

            catch (Exception ex)
            {
                unitofwork.HandleDbExceptions(ex);
            }
            finally
            {
            }
        }

        public void UpdateTimeofTask(int Id, string Language)
        {
            try
            {
                SchedualTask task = hrUnitOfWork.Repository<SchedualTask>().Where(a => a.EventId == Id).FirstOrDefault();
                var Now = DateTime.Now;
                task.LastEndDate = Now;
                task.LastStartDate = Now;
                task.LastSuccessDate = Now;
                task.Enabled = true;
                hrUnitOfWork.SaveChanges(Language);
            }
            catch
            {
            }
        }
        #endregion



    }



    public class Paper_UploadStatus
    {
        public Guid? Stream_Id { set; get; }
        public DocTypeFormViewModel Paper { set; get; }
        public bool IsUploaded { set; get; }
        public bool IsRequired { set; get; }
        public bool IsEmpPaper { set; get; }

        //Just For Test until design is Completed
        public string RequiredColor
        {
            get
            {
                return IsRequired ? "required" : "optional";
            }
        }

        public string IsEmpPaperClass
        {
            get
            {
                return !IsEmpPaper ? "Others" : "";
            }
        }
        public string UploadedText
        {
            get
            {
                return IsUploaded ? "Done" : "Empty";
            }
        }

        public string Uploaded_StreamID_QueryString
        {
            get
            {
                if (IsUploaded)
                    return "&Stream_Id=" + this.Stream_Id;

                return string.Empty;
            }
        }

        public string EmpPaperText
        {
            get
            {
                return IsEmpPaper ? "Employee Paper" : "Other Paper";
            }
        }
    }


}
