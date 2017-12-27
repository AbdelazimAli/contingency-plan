using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraReports.UI;

namespace WebApp.Models
{
    public class ReportDesignViewModel
    {
        public XtraReport Report { get; set; }
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string Icon { get; set; }
        public string Controller { get; set; }
        public byte[] ReportData { get; set; }
    }
}
