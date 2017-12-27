using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Reports.EmployeesReports
{
    interface IEmployeeDataReport
    {
         XRPictureBox EmpStatusPicBox { get; set; }
    }
}
