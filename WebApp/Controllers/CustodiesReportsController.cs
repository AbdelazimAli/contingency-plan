using DevExpress.XtraReports.UI;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Models;
using WebApp.Reports.en_EmployeesReports;
using WebApp.Reports.ReportInterfaces;
using WebApp.Reports.SharedForReports;

namespace WebApp.Controllers
{
    public class CustodiesReportsController : BaseReportController
    {
        XtraReport report;
        public CustodiesReportsController()
        {
            dateRange = db.Database.SqlQuery<DateRange>("select top(1) startDate, EndDate from FiscalYears order by startDate desc").FirstOrDefault();
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Icon = "3.jfif";
        }
        protected override void Report_DataSourceDemanded(object sender, EventArgs e)
        {
             report = (XtraReport)sender;
            if (report.Parameters["ReportName"] != null)
            {
                if (report.Parameters["ReportName"].Value.ToString() == "EmployeeCustodiesReport")
                {
                    var empIds = report.Parameters["EmpIds"].Value;
                    var deptIds = report.Parameters["DeptIds"].Value;
                    var CatsId = report.Parameters["categoryIds"].Value;
                    var inUse = report.Parameters["InUse"].Value;

                    report.Parameters["MappingEmpIds"].Value = string.Join(",", ((IEnumerable<int>)empIds).Select(a => a));
                    report.Parameters["MappingDeptIds"].Value = string.Join(",", ((IEnumerable<int>)deptIds).Select(a => a));
                    report.Parameters["MappingCategoryIds"].Value = string.Join(",", ((IEnumerable<int>)CatsId).Select(a => a));
                    report.Parameters["MappingInUse"].Value = string.Join(",", ((IEnumerable<int>)inUse).Select(a => a));

                }
            }

            if (report.Parameters["ReportName"] != null)
            {
                if (report.Parameters["ReportName"].Value.ToString() == "CustodyTrackingReport")
                {
                    var custodyIds = report.Parameters["CustodyIds"].Value;
                    var categoriesIds = report.Parameters["CategoriesIds"].Value;

                    report.Parameters["MappingCustodyIds"].Value = string.Join(",", ((IEnumerable<int>)custodyIds).Select(a => a));
                    report.Parameters["MappingCategoryIds"].Value = string.Join(",", ((IEnumerable<int>)categoriesIds).Select(a => a));
                }
            }


            if (report.Parameters["ReportName"] != null)
            {
                if (report.Parameters["ReportName"].Value.ToString() == "EmployeeCurrentCustody")
                {
                    var empIds = report.Parameters["EmpIds"].Value;
                    var deptIds = report.Parameters["DeptIds"].Value;
                    var CatsId = report.Parameters["categoryIds"].Value;

                    report.Parameters["MappingEmpIds"].Value = string.Join(",", ((IEnumerable<int>)empIds).Select(a => a));
                    report.Parameters["MappingDeptIds"].Value = string.Join(",", ((IEnumerable<int>)deptIds).Select(a => a));
                    report.Parameters["MappingCategoryIds"].Value = string.Join(",", ((IEnumerable<int>)CatsId).Select(a => a));

                }
            }
        }

        protected override void ReceiveBar_Changed(object sender, EventArgs e)
        {
            var bar = (ProgressBar)sender;
            bar.Position = 0;
            if (report.RowCount > 0)
            {
                if (report.GetCurrentColumnValue("ReceiveStatus").ToString() != "")
                {

                    byte status = (byte)report.GetCurrentColumnValue("ReceiveStatus");

                    bar.Position = status;

                }
            }
        }
        protected override void Delivery_Changed(object sender, EventArgs e)
        {
            var bar = (ProgressBar)sender;
            bar.Position = 0;

            if (report.RowCount > 0)
            {
                if (report.GetCurrentColumnValue("deliveryStatus").ToString() != "")
                {

                    byte status = (byte)report.GetCurrentColumnValue("deliveryStatus");

                    bar.Position = status;

                }
            }
        }
    }
}