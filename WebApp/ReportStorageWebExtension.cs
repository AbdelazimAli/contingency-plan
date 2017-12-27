using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;
using WebApp.Models;
using Microsoft.AspNet.Identity;
using System.Web;


namespace WebApp
{
    public class ReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        private UserContext db;

        public ReportStorageWebExtension()
        {
            db = new UserContext();
        }

        public override bool CanSetData(string url)
        {
            // Check if the URL is available in the report storage.
            return db.HReports.Find(int.Parse(url)) != null;
            // return base.CanSetData(url);
        }
        public override bool IsValidUrl(string url)
        {
            // Check if the specified URL is valid for the current report storage.
            // In this example, a URL should be a string containing a numeric value that is used as a data row primary key.
            int n;
            return int.TryParse(url, out n);
            // return base.IsValidUrl(url);
        }

        public override byte[] GetData(string url)
        {
            // Get the report data from the storage.
            var hreport = db.HReports.Find(int.Parse(url));
            if (hreport == null) return null;
            return hreport.ReportData;
            
            //return base.GetData(url);
        }

        public override Dictionary<string, string> GetUrls()
        {
            // Get URLs and display names for all reports available in the storage.
            string language = HttpContext.Current.Session["Language"] == null ? "ar-EG" : HttpContext.Current.Session["Language"].ToString();
            var list = db.HReports.Where(r => r.Language == language).Select(r => new { ReportID = r.Id, DisplayName = (r.ReportTitle == null ? r.ReportName : r.ReportTitle) }).ToList();
            return list.ToDictionary(d => d.ReportID.ToString(), d => d.DisplayName);

            //return base.GetUrls();
        }

        public override void SetData(DevExpress.XtraReports.UI.XtraReport report, string url)
        {
            // Write a report to the storage under the specified URL.
            var hreport = db.HReports.Find(int.Parse(url));

            if (hreport != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    hreport.ReportData = ms.GetBuffer();
                }

                db.HReports.Attach(hreport);
                db.Entry(hreport).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            //base.SetData(report, url);
        }

        public override string SetNewData(DevExpress.XtraReports.UI.XtraReport report, string defaultUrl)
        {
            // Save a report to the storage under a new URL. 
            // The defaultUrl parameter contains the report display name specified by a user.
            string reportname = report.ToString();
            string language = "ar-EG";
            string menuname = "OtherReports";
            string icon = "7.png";

            var orgReport = db.HReports.Where(r => r.ReportName == reportname && r.OrgReportId == null).Select(r => new { Id = r.Id, Icon = r.Icon, MenuName = r.MenuName, Language = r.Language }).FirstOrDefault();
            if (orgReport != null)
            { // get info from original report
                menuname = orgReport.MenuName;
                icon = orgReport.Icon;
                language = orgReport.Language;
            }
            else
            { // read info from sessions
                if (HttpContext.Current.Session["Language"] != null) language = HttpContext.Current.Session["Language"].ToString();
                if (HttpContext.Current.Session["Icon"] != null) icon = HttpContext.Current.Session["Icon"].ToString();
                if (HttpContext.Current.Session["MenuName"] != null) menuname = HttpContext.Current.Session["MenuName"].ToString();
                //int menuId = 0;
                //if (HttpContext.Current.Session["MenuId"] != null) int.TryParse(HttpContext.Current.Session["MenuId"].ToString(), out menuId);
                //if (menuId > 0) menuname = db.Menus.Where(m => m.Id == menuId).Select(m => m.Name).FirstOrDefault();
            }

           
            var hreport = new HReport
            {
                CreatedTime = DateTime.Now,
                CreatedUser = HttpContext.Current.User.Identity.Name,
                MenuName = menuname,
                OrgReportId = (orgReport == null ? (int?)null : orgReport.Id),
                Icon = icon,
                ReportName = reportname,
                ReportTitle = defaultUrl,
                Language = language
            };
           
            using (MemoryStream ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);   
                hreport.ReportData = ms.GetBuffer();
            }

            db.HReports.Add(hreport);
            db.SaveChanges();

            return hreport.Id.ToString();
        }
    }
}
