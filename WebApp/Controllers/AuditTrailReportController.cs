using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class AuditTrailReportController : BaseReportController
    {
        XtraReport report;
        public AuditTrailReportController()
        {

        }
        protected override void Report_DataSourceDemanded(object sender, EventArgs e)
        {
            report = (XtraReport)sender;
            if (report.Parameters["ReportName"] != null)
            {
                if (report.Parameters["ReportName"].Value.ToString() == "UserAuditTrail")
                {
                    var sourceIds = report.Parameters["SourceId"].Value;
                    var objectNames = report.Parameters["ObjectName"].Value;
                    var modifiedUsers = report.Parameters["ModifiedUser"].Value;
                    var actTypes = report.Parameters["ActType"].Value;

                    report.Parameters["MappingSourceId"].Value = string.Join(",", ((IEnumerable<string>)sourceIds).Select(a => a));
                    report.Parameters["MappingObjectName"].Value = string.Join(",", ((IEnumerable<string>)objectNames).Select(a => a));
                    report.Parameters["MappingModifiedUser"].Value = string.Join(",", ((IEnumerable<string>)modifiedUsers).Select(a => a));
                    report.Parameters["MappingActType"].Value = string.Join(",", ((IEnumerable<int>)actTypes).Select(a => a));

                }
            }
            if (report.Parameters["ReportName"] != null)
            {
                if (report.Parameters["ReportName"].Value.ToString()== "UsersLoginHistory")
                {
                    var userNames = report.Parameters["UsersNames"].Value;

                    report.Parameters["MappingUsersNames"].Value = string.Join(",", ((IEnumerable<string>)userNames).Select(a => a));
                }
            }
        }

    }
}