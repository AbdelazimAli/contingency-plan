using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Model.ViewModel.Personnel
{
    public class EmploymentPapersUploadVModel
    {
        /// <summary>
        /// Sample : "'.jpg','.jpeg','.bmp','.png','.gif','.pdf'"
        /// </summary>
        public string ValidFileExtensions { set; get; }
        public string ValidFileExt_ForAcceptAttribute
        {
            get
            {
                try
                {
                    return ValidFileExtensions.Replace("'", "");
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public int EmpID { set; get; }

        [Required(ErrorMessage = "File name is required")]
        public string name { set; get; }

        public int TypeId { set; get; }
        public List<SelectListItem> DocTypesList { set; get; }

        public int DocumenType { set; get; }

        public string SelectedDocType
        {
            get
            {
                try
                {
                    if (TypeId > 0 && DocTypesList.Count > 0)
                        return DocTypesList.Find(m => m.Value == TypeId.ToString()).Text;

                    return string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public string Description { set; get; }


        [Required(ErrorMessage = "ExpireDate is required")]
        [RegularExpression(pattern: @"\d{2}/\d{2}/\d{4}", ErrorMessage = "Invalid Date")]
        public string ExpireDate_string { set; get; }

        public DateTime? ExpiryDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(ExpireDate_string);
                }
                catch
                {
                    return null;
                }
            }
        }
        public string Keyword { set; get; }

        public bool IsAddNewOthers { set; get; }
        public bool HasExpiryDate { set; get; }

        public HttpPostedFileBase[] Images { set; get; }
      public  byte[] ImageStream { set; get; }
        public string ContentType { set; get; }
        public bool IsUploaded { set; get; }
        public Guid? Stream_Id { set; get; }
        public string File_Type { set; get; }
        public bool IsEmpPaper { set; get; }
        public bool? IsImage
        {
            get
            {

                if (!string.IsNullOrEmpty(File_Type))
                {
                    if (File_Type.ToLower().Contains("image"))
                        return true;
                    else
                        return false;
                }
                else
                    return null;
            }
        }
        public string BatchGridData { set; get; }
        public List<CompanyDocAttrViewModel> BatchGridData_List
        {
            get
            {
                if (!string.IsNullOrEmpty(BatchGridData))
                    return new JavaScriptSerializer().Deserialize<List<CompanyDocAttrViewModel>>(BatchGridData);

                return new List<CompanyDocAttrViewModel>();
            }
        }

        public string OldModel_Serialized { set; get; }

        public string BatchGridData_Old { set; get; }
        public List<CompanyDocAttrViewModel> BatchGridData_List_Old
        {
            get
            {
                if (!string.IsNullOrEmpty(BatchGridData_Old))
                    return new JavaScriptSerializer().Deserialize<List<CompanyDocAttrViewModel>>(BatchGridData_Old);

                return new List<CompanyDocAttrViewModel>();
            }
        }

        public string RequiredDocTypeIDs { set; get; }
        public List<int> RequiredDocTypeIDsList
        {
            get
            {
                List<int> IDsList = new List<int>();
                try
                {
                    
                    if (!string.IsNullOrEmpty(RequiredDocTypeIDs))
                        IDsList = RequiredDocTypeIDs.Split(',').Select(int.Parse).ToList();
                }
                catch
                {
                }
                return IDsList;
            }
        }
    }

    public class BatchGridRecord
    {
        public int Id { set; get; }
        public string Value { set; get; }
    }






}
