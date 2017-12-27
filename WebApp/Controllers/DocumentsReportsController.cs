using DevExpress.XtraReports.UI;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp.Controllers
{
    public class DocumentsReportsController : BaseReportController
    {

        public DocumentsReportsController()
        {
            dateRange = db.Database.SqlQuery<DateRange>("select top(1) startDate, EndDate from FiscalYears order by startDate desc").FirstOrDefault();
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Language == "ar-EG")
            {
                // Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar");

                Icon = "3.jfif";
                ReportModels = new List<ReportViewModel>()
                {
                    new ReportViewModel
                    {
                        Id = 0,
                        ReportName = "EmployeeDocumens",
                        ReportTitle = "بيانات اوراق الموظفين",
                        Icon = "3.jfif",
                        Language = "ar-EG"
                    }

                };
            }
            else
            {
                Icon = "3.jfif";
                ReportModels = new List<ReportViewModel>()
                {
                    new ReportViewModel
                    {
                        Id=0,
                        ReportName="en_EmployeeDocumens",
                         ReportTitle = "Employee Documents Data",
                        Icon = "3.jfif",
                        Language = "en-GB"
                    }
                };
            }
        }
        protected override void Report_DataSourceDemanded(object sender, EventArgs e)
        {
            var report = (XtraReport)sender;

            if (report.Parameters["PaperStatus"] != null)
            {
                var gender = report.Parameters["Gender"].Value;
                var deptName = report.Parameters["DeptName"].Value;
                var empName = report.Parameters["EmpName"].Value;

                report.Parameters["mappingGender"].Value = string.Join(",", ((IEnumerable<int>)gender).Select(a => a));

                report.Parameters["mappingDeptName"].Value = string.Join(",", ((IEnumerable<int>)deptName).Select(a => a));

                report.Parameters["mappingEmpName"].Value = string.Join(",", ((IEnumerable<int>)empName).Select(a => a));

            }
            //var c=report.Parameters["AssignEndDate"].Value;
            if (report.Parameters["ReportName"]!=null)
            {
                if (report.Parameters["ReportName"].Value.ToString() == "BorrowedDocs")
                {
                    if (report.Parameters["AssignEndDate"] != null)
                    {
                        if (report.Parameters["AssignEndDate"].Value.ToString() == "01/01/0001 12:00:00 ص" || report.Parameters["AssignEndDate"].Value.ToString() == "01/01/0001 00:00:00")
                        {
                            report.Parameters["AssignEndDate"].Value = "2099-01-01";
                        }
                    }
                    var Docs = report.Parameters["DocsIds"].Value;
                    report.Parameters["MappingDocIds"].Value = string.Join(",", ((IEnumerable<int>)Docs).Select(a => a));
                }
            }
           
        }
    }
}