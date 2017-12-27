using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Extensions;
using System.Linq.Dynamic;
using System.Drawing;
using System.Web.Routing;
using DevExpress.XtraReports.UI;
using System.Threading;
using WebApp.Reports.EmployeesReports;
using WebApp.Reports.ReportInterfaces;

namespace WebApp.Controllers
{

    public class BaseReportController : Controller
    {
        protected UserContext db;
        protected string userName { get; set; }
        protected int CompanyId { get; set; }
        public string companyName { get; set; }
        protected string Language { get; set; }
        protected string Icon { get; set; }
        protected IList<ReportViewModel> ReportModels;

        public class DateRange
        {
            public  DateTime startDate { get; set; }
            public  DateTime EndDate { get; set; }
        }
        public DateRange dateRange { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {           
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                userName = requestContext.HttpContext.User.Identity.Name;
                CompanyId = requestContext.HttpContext.User.Identity.GetDefaultCompany();
                Language = requestContext.HttpContext.User.Identity.GetLanguage();
                Session["Language"] = Language;
                if (Language == "ar-EG")
                {
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar");
                }
                var company = db.Companies.Where(a => a.Id == CompanyId).Select(a => new { a.Name,a.CountryId }).FirstOrDefault();

                
                companyName = db.Companies.Where(a => a.Id == CompanyId).Select(a => a.Name ).FirstOrDefault();

            }
         
        }
        public BaseReportController()
        {
            db = new UserContext();
            Icon = "7.png";
            ReportModels = new List<ReportViewModel>();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (Request.QueryString["MenuId"] != null) Session["MenuId"] = Request.QueryString["MenuId"];
            if (Request.QueryString["RoleId"] != null) Session["RoleId"] = Request.QueryString["RoleId"];
        }

        protected override void OnException(ExceptionContext filterContext)
        {

            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
        }

        private XtraReport GetReport(int ReportId, string name)
        {
            var report = new XtraReport();
            //defensive code for checking if the report is created or not
            if (Type.GetType(name) != null)
            {
                 report = (XtraReport)Activator.CreateInstance(Type.GetType(name));

                var hreport = db.HReports.Find(ReportId);

                if (hreport != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(hreport.ReportData, 0, hreport.ReportData.Length);
                        ms.Flush();
                        report.LoadLayoutFromXml(ms);
                    }
                }
            }
            return report;
        }

        private XtraReport GetReportbyName(string name)
        {
            var report = (XtraReport)Activator.CreateInstance(Type.GetType(name));
            var hreport = db.HReports.Where(r => r.OrgReportId == null && r.ReportName == name && r.Language == Language).FirstOrDefault();
            if (hreport != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(hreport.ReportData, 0, hreport.ReportData.Length);
                    ms.Flush();
                    report.LoadLayoutFromXml(ms);

                }     
            }
           
            return report;
        }

        public ActionResult ReportsList()
        {
            var name = RouteData.Values["controller"].ToString();
            ViewBag.Controller = name;

            // 1- Get default reports from menu table
            List<ReportViewModel> mreports = new List<ReportViewModel>();
            if (Request.QueryString["MenuId"] != null)
            {
                var menuid = 0;
                var role = Request.QueryString["RoleId"] == null ? Session["RoleId"].ToString() : Request.QueryString["RoleId"].ToString();

                if (Request.QueryString["MenuId"] != null)
                    int.TryParse(Request.QueryString["MenuId"].ToString(), out menuid);
                if (menuid > 0)
                {
                    mreports = (from m in db.Menus
                                where m.ParentId == menuid
                                join rm in db.RoleMenus on new { role = role, menu = m.Id } equals new { role = rm.RoleId, menu = rm.MenuId }
                                join n in db.NamesTbl on new { lang = Language, name = m.Name + m.Sequence } equals new { lang = n.Culture, name = n.Name } into g
                                from n in g.DefaultIfEmpty()
                                select new ReportViewModel
                                {
                                    Id = m.Id,
                                    ReportName = m.Name,
                                    ReportTitle = n == null ? m.Name : n.Title,
                                    Language = Language,
                                    Icon = m.NodeType == 1 ? "10.jfif" : (m.Icon == null || m.Icon == "" ? "1.jfif" : m.Icon),
                                    NodeType = m.NodeType,
                                    Url = m.Url
                                }).ToList();
                                           }
            }
           
            // 2- Retrieve reports from database
            if (Request.QueryString["MenuName"] != null)
            {
                var menuname = Request.QueryString["MenuName"].ToString();
                Session["MenuName"] = menuname;
                if (Request.QueryString["myurl"] != null && Request.QueryString["myurl"].ToString().Contains("/"))
                    ViewBag.Controller = Request.QueryString["myurl"].ToString().Split('/')[1];

                var dbreports = db.HReports.Where(r => r.MenuName == menuname && r.Language == Language)
                        .Select(r => new ReportViewModel
                        {
                            Id = r.Id,
                            ReportName = r.ReportName,
                            ReportTitle = r.ReportTitle,
                            Language = r.Language,
                            Icon = r.Icon,
                            NodeType = 2
                        }).ToList();

                var prefix = Language.Substring(0, 2) == "ar" ? "" : "en_";

                // Merge step 1 and 2
                var treports = (from m in mreports
                                join r in dbreports on m.ReportName equals r.ReportName into g
                                from r in g.DefaultIfEmpty()
                                select new ReportViewModel
                                {
                                    Id = r == null ? m.Id : r.Id,
                                    ReportName = prefix + m.ReportName,
                                    ReportTitle = r == null ? m.ReportTitle : r.ReportTitle,
                                    Language = r == null ? m.Language : r.Language,
                                    Icon = r == null ? m.Icon : r.Icon,
                                    NodeType = r == null ? m.NodeType : r.NodeType
                                }).ToList();
                //----
        


                return View(treports);
            }

            return View(mreports);
        }

   
        public  ActionResult ReportViewMode(int id, string name, string icon, string controller)
        {
            var design = new ReportDesignViewModel();
            if (icon != null) Session["Icon"] = icon;

            design.Icon = icon;
            design.ReportId = id;
            design.ReportName = name;
            design.Controller = controller;

            if (id > 0)
                design.Report = GetReport(id, name);
            else
                design.Report = GetReportbyName(name);

            //Handling report header
            if (design.Report.Parameters["CompanyName"] != null)
            {
                design.Report.Parameters["CompanyName"].Value = companyName;
            }
            design.Report.Parameters["CompanyId"].Value = CompanyId;

            //Handling report footer
            design.Report.Parameters["User"].Value = userName;
            //daterange = db.Database.SqlQuery<DateRange>("select top(1) startDate, EndDate from FiscalYears order by startDate desc").FirstOrDefault();

            //Financial Date
           
            if (design.Report.Parameters["ConEmpStartDate"] != null)
            {
                design.Report.Parameters["ConEmpStartDate"].Value = dateRange.startDate;
            }
            if (design.Report.Parameters["ConEmpEndDate"] != null)
            { 
                design.Report.Parameters["ConEmpEndDate"].Value = dateRange.EndDate;
            }
            
            ///////Today Date
            if (design.Report.Parameters["EmpEndDate"] != null)
            {
                design.Report.Parameters["EmpEndDate"].ValueInfo = DateTime.Now.Date.ToShortDateString();
            }
            if (design.Report.Parameters["AssignEndDate"] != null)
            {
                design.Report.Parameters["AssignEndDate"].ValueInfo = DateTime.Now.Date.ToShortDateString();
            }
           
           
            // Todo: Moved
            
            if (!(!(design.ReportName == "InsuranceEmployeesReports") ^ !(design.ReportName == "InsuranceEmployeesGrouping") ^ (!(design.ReportName== "en_InsuranceEmployeesReports")^!(design.ReportName== "en_InsuranceEmployeesReportsGrouping")) ))
            {
                if ((!(design.ReportName == "InsuranceEmployeesDoc")))
                {
                    design.Report.DataSourceDemanded += Report_DataSourceDemanded;

                }

            }

            if (design.Report is IBasicInfoReport)
            {
                ((IBasicInfoReport)design.Report).CompanyLogo.BeforePrint += XrPictureBox8_BeforePrint;
            }


            if (design.Report is IEmployeeDataReport)
            {
                ((IEmployeeDataReport)design.Report).EmpStatusPicBox.BeforePrint += xrPictureBox2_BeforePrint;
            }

            if (design.Report is IProgressBars)
            {
                ((IProgressBars)design.Report).ReceiveProgress.BeforePrint += ReceiveBar_Changed;
                ((IProgressBars)design.Report).DeliveryProgress.BeforePrint += Delivery_Changed;
            }

            return View(design);
        }

        public ActionResult ReportDesignMode(int id, string name, string icon, string controller)
        {
            var design = new ReportDesignViewModel();
            //---
            if (icon != null) Session["Icon"] = icon;

            design.Icon = icon;
            design.ReportId = id;
            design.ReportName = name;
            design.Controller = controller;

            if (id > 0)
                design.Report = GetReport(id, name);
            else
                design.Report = GetReportbyName(name);

            return View(design);
        }
        //handling Company Logo Image
        private void XrPictureBox8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var root = AppDomain.CurrentDomain.BaseDirectory;

            ((XRPictureBox)sender).Image = new Bitmap(root + "\\Content\\Logos\\3.png");
        }
        //handling empStatus picture box
        protected virtual void xrPictureBox2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
        //handling data source demand
        protected virtual void Report_DataSourceDemanded(object sender, EventArgs e)
        {


        }

        //handling Custody progress receiveBar
        protected virtual void ReceiveBar_Changed(object sender, EventArgs e)
        {

            
        }
        //handling Custody progress delivery bar
        protected virtual void Delivery_Changed(object sender, EventArgs e)
        {


        }
    }
}