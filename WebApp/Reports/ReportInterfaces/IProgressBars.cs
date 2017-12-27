using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Reports.en_EmployeesReports;

namespace WebApp.Reports.ReportInterfaces
{
    interface IProgressBars
    {
         ProgressBar ReceiveProgress{ get; set; }
         ProgressBar DeliveryProgress{ get; set; }
    }
}
