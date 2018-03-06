using Db.Persistence.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class EmploymentPapersIndexViewModel
    {
        public List<Paper_UploadStatus> EmpDocsTypes { set; get; }
        public int EmpID { set; get; }
        public string GeneralUrl { set; get; }
    }
}