using Model.ViewModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Model.ViewModel.Personnel
{
   public class ExcelFileImports
    {
        public HttpPostedFileBase ExcelFile { get; set; }
        public byte Ver { get; set; }
        public int MenuId { get; set; }
        public string OldObjectName { get; set; }
        public string ObjectName { get; set; }
        public string TableName { get; set; }
        public string PathName { get; set; }
        public bool ErrorData { get; set; }
        public string TimeZone { get; set; }
        public string PageType { get; set; }
        public int? Id { get; set; }
        public bool ExistFile { get; set; } = false;
        public int DataLevel { get; set; } = 0;
        public int Read { get; set; } = 0;
    }
}
