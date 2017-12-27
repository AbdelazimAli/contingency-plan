using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class RequestsReportsController : BaseReportController
    {
        XtraReport report;

        public RequestsReportsController()
        {

        }

        protected override void Report_DataSourceDemanded(object sender, EventArgs e)
        {
            report = (XtraReport)sender;
            if (report.Parameters["ReportName"] != null)
            {
                if (report.Parameters["ReportName"].Value.ToString() == "LeaveRequestsReport")
                {

                    var deptIds = report.Parameters["deptIds"].Value;
                    var empIds = report.Parameters["empIds"].Value;
                    var holidaysIds = report.Parameters["HolidayType"].Value;

                    report.Parameters["MappingDeptIds"].Value = string.Join(",", ((IEnumerable<int>)deptIds).Select(a => a));
                    report.Parameters["MappingEmpIds"].Value = string.Join(",", ((IEnumerable<int>)empIds).Select(a => a));
                    report.Parameters["MappingHolidayType"].Value = string.Join(",", ((IEnumerable<int>)holidaysIds).Select(a => a));

                }

            }

            if (report.Parameters["ReportName"] != null)
            {
                if (report.Parameters["ReportName"].Value.ToString() == "TerminationRequestsReport")
                {
                    var deptIds = report.Parameters["DeptId"].Value;
                    var empIds = report.Parameters["EmpIds"].Value;

                    report.Parameters["MappingDeptIds"].Value = string.Join(",", ((IEnumerable<int>)deptIds).Select(a => a));
                    report.Parameters["MappingEmpIds"].Value = string.Join(",", ((IEnumerable<int>)empIds).Select(a => a));
                }
            }

        }
    }
}