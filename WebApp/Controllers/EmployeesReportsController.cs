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
using System.Drawing.Printing;
using WebApp.Reports.EmployeesReports;
using DevExpress.XtraReports.Parameters;

namespace WebApp.Controllers
{
    public class EmployeesReportsController : BaseReportController
    {

        XtraReport report;

        public EmployeesReportsController()
        {
            dateRange = db.Database.SqlQuery<DateRange>("select top(1) startDate, EndDate from FiscalYears order by startDate desc").FirstOrDefault();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
        protected override void Report_DataSourceDemanded(object sender, EventArgs e)
        {
            report = (XtraReport)sender;
            if (report.Parameters["ReportName"] != null)
            {
                if (report.Parameters["ReportName"].Value.ToString() == "LeavesSchedule")
                {
                    var deptIds = report.Parameters["DeptIdss"].Value;
                    var jobIds = report.Parameters["JobIdss"].Value;
                    var empIds = report.Parameters["EmpIdss"].Value;

                    report.Parameters["MappingDeptIdss"].Value = string.Join(",", ((IEnumerable<int>)deptIds).Select(a => a));

                    report.Parameters["MappingJobIdss"].Value = string.Join(",", ((IEnumerable<int>)jobIds).Select(a => a));

                    report.Parameters["MappingEmpIdss"].Value = string.Join(",", ((IEnumerable<int>)empIds).Select(a => a));
                }
            }
            if (report.Parameters["Job"] != null)
            {
                if (report.Parameters["Job"] != null && report.Parameters["Gender"] != null && report.Parameters["Nationality"] != null && report.Parameters["ContractType"] != null && report.Parameters["deptName"] != null)
                {
                    var jobs = report.Parameters["Job"].Value;
                    var gender = report.Parameters["Gender"].Value;
                    var Nationalities = report.Parameters["Nationality"].Value;
                    var ContractType = report.Parameters["ContractType"].Value;
                    var deptName = report.Parameters["deptName"].Value;

                    if (report.Parameters["FromAge"].Value == null) report.Parameters["FromAge"].Value = 0;

                    if (report.Parameters["EmpEndDateDetails"] != null)
                    {
                        if (report.Parameters["EmpEndDateDetails"].Value.ToString() == "01/01/0001 12:00:00 ص" || report.Parameters["EmpEndDateDetails"].Value.ToString() == "01/01/0001 00:00:00")
                        {
                            report.Parameters["EmpEndDateDetails"].Value = "2099-01-01";
                        }

                    }

                    if (report.Parameters["AssignEndDateEmp"] != null)
                    {
                        if (report.Parameters["AssignEndDateEmp"].Value.ToString() == "01/01/0001 12:00:00 ص" || report.Parameters["AssignEndDateEmp"].Value.ToString() == "01/01/0001 00:00:00")
                        {
                            report.Parameters["AssignEndDateEmp"].Value = "2099-01-01";
                        }

                    }
                    report.Parameters["mappingJob"].Value = string.Join(",", ((IEnumerable<int>)jobs).Select(a => a));

                    report.Parameters["mappingGender"].Value = string.Join(",", ((IEnumerable<int>)gender).Select(a => a));

                    report.Parameters["mappingNationality"].Value = string.Join(",", ((IEnumerable<int>)Nationalities).Select(a => a));

                    report.Parameters["mappingContractType"].Value = string.Join(",", ((IEnumerable<int>)ContractType).Select(a => a));

                    report.Parameters["mappingDeptName"].Value = string.Join(",", ((IEnumerable<int>)deptName).Select(a => a));
                    if (report.Parameters["insured"] != null)
                    {
                        var Ins = report.Parameters["insured"].Value;
                        report.Parameters["MappingInsured"].Value = string.Join(",", ((IEnumerable<int>)Ins).Select(a => a));
                    }
                }

            }

            if (report.Parameters["MappingJobIds"] != null)
            {
                if (report.Parameters["EmpIds"] != null && report.Parameters["DeptIds"] != null && report.Parameters["JobIds"] != null)
                {
                    var EmpId = report.Parameters["EmpIds"].Value;
                    var DeptId = report.Parameters["DeptIds"].Value;
                    var JobId = report.Parameters["JobIds"].Value;
                    if (report.Parameters["Gender"] != null)
                    {
                        var gender = report.Parameters["Gender"].Value;
                        report.Parameters["mappingGender"].Value = string.Join(",", ((IEnumerable<int>)gender).Select(a => a));
                    }
                    if (report.Parameters["Nationality"] != null)
                    {
                        var Nationality = report.Parameters["Nationality"].Value;
                        report.Parameters["mappingNationality"].Value = string.Join(",", ((IEnumerable<int>)Nationality).Select(a => a));
                    }
                    if (report.Parameters["ContractStatus"] != null)
                    {
                        var ContractStatus = report.Parameters["ContractStatus"].Value;
                        report.Parameters["MappingContractStatus"].Value = string.Join(",", ((IEnumerable<int>)ContractStatus).Select(a => a));
                    }
                    if (report.Parameters["ContractType"] != null)
                    {
                        var ContractType = report.Parameters["ContractType"].Value;
                        report.Parameters["MappingContractType"].Value = string.Join(",", ((IEnumerable<int>)ContractType).Select(a => a));
                    }

                    report.Parameters["MappingEmpIds"].Value = string.Join(",", ((IEnumerable<int>)EmpId).Select(a => a));
                    report.Parameters["MappingJobIds"].Value = string.Join(",", ((IEnumerable<int>)JobId).Select(a => a));
                    report.Parameters["MappingDeptIds"].Value = string.Join(",", ((IEnumerable<int>)DeptId).Select(a => a));

                }
                if (report.Parameters["CourseID"] != null && report.Parameters["TrainingEvent"] != null && report.Parameters["EmpCode"] != null)
                {
                    var CourseId = report.Parameters["CourseID"].Value;
                    var eventName = report.Parameters["TrainingEvent"].Value;
                    var EmpCodes = report.Parameters["EmpCode"].Value;

                    report.Parameters["MappingTrainEvent"].Value = string.Join(",", ((IEnumerable<int>)eventName).Select(a => a));
                    report.Parameters["MappingCourseId"].Value = string.Join(",", ((IEnumerable<int>)CourseId).Select(a => a));
                    report.Parameters["MappingEmpCode"].Value = string.Join(",", ((IEnumerable<int>)EmpCodes).Select(a => a));

                }

            }

            if (report.Parameters["MappingEmpName"] != null)
            {
                var EmpNames = report.Parameters["EmpName"].Value;

                report.Parameters["MappingEmpName"].Value = string.Join(",", ((IEnumerable<int>)EmpNames).Select(a => a));
            }

            if (report.Parameters["GroupingBy"] != null)
            {
                if (report.Parameters["EmpIds"] != null)
                {
                    var EmpId = report.Parameters["EmpIds"].Value;
                    report.Parameters["MappingEmpIds"].Value = string.Join(",", ((IEnumerable<int>)EmpId).Select(a => a));
                }
                if (report.Parameters["DeptIds"] != null)
                {
                    var DeptId = report.Parameters["DeptIds"].Value;
                    report.Parameters["MappingDeptIds"].Value = string.Join(",", ((IEnumerable<int>)DeptId).Select(a => a));
                }
                if (report.Parameters["JobIds"] != null)
                {
                    var JobId = report.Parameters["JobIds"].Value;
                    report.Parameters["MappingJobIds"].Value = string.Join(",", ((IEnumerable<int>)JobId).Select(a => a));
                }
                if (report.Parameters["Gender"] != null)
                {
                    var gender = report.Parameters["Gender"].Value;
                    report.Parameters["mappingGender"].Value = string.Join(",", ((IEnumerable<int>)gender).Select(a => a));
                }
                if (report.Parameters["Nationality"] != null)
                {
                    var nationality = report.Parameters["Nationality"].Value;
                    report.Parameters["mappingNationality"].Value = string.Join(",", ((IEnumerable<int>)nationality).Select(a => a));
                }
                if (report.Parameters["insured"] != null)
                {
                    var Ins = report.Parameters["insured"].Value;
                    report.Parameters["MappingInsured"].Value = string.Join(",", ((IEnumerable<int>)Ins).Select(a => a));
                }


                var xx = report.Band;

                if (Language == "ar-EG")
                {
                    if (xx.ToString() == "NewEmployeesReport")
                    {
                        var report2 = (NewEmployeesReport)sender;
                        var GroupingBy = report2.Parameters["GroupingBy"].Value;
                        if ((string)GroupingBy == "Default")
                        {
                            report2.GroupHeader1.Visible = false;
                            report2.xrTable4.Visible = false;
                            report2.xrTable1.Visible = true;
                            report2.xrTable2.Visible = true;
                            report2.xrTable3.Visible = false;


                        }

                        if ((string)GroupingBy == "Dept")
                        {
                            report2.GroupHeader1.Visible = true;
                            report2.xrTable4.Visible = true;
                            report2.xrTable1.Visible = false;
                            report2.xrTable2.Visible = false;
                            report2.xrTable3.Visible = true;

                        }
                    }
                    else if (xx.ToString() == "InsuranceEmployeesReport")
                    {
                        var report_ins = (InsuranceEmployeesReport)sender;
                        var GroupingBy_ins = report_ins.Parameters["GroupingBy"].Value;
                        if ((string)GroupingBy_ins == "Default")
                        {
                            report_ins.GroupHeader1.Visible = false;
                            report_ins.xrTable4.Visible = true;
                            report_ins.xrTable1.Visible = false;
                            report_ins.xrTable2.Visible = false;
                            report_ins.xrTable3.Visible = true;


                        }

                        if ((string)GroupingBy_ins == "Dept")
                        {
                            report_ins.GroupHeader1.Visible = true;
                            report_ins.xrTable4.Visible = false;
                            report_ins.xrTable1.Visible = true;
                            report_ins.xrTable2.Visible = true;
                            report_ins.xrTable3.Visible = false;

                        }
                    }
                    else if (xx.ToString() == "ContractEmployeesReport")
                    {
                        var report_Contract = (ContractEmployeesReport)sender;
                        var GroupingBy_Contract = report_Contract.Parameters["GroupingBy"].Value;
                        if ((string)GroupingBy_Contract == "Default")
                        {
                            report_Contract.GroupHeader1.Visible = false;
                            report_Contract.xrTable4.Visible = false;
                            report_Contract.xrTable1.Visible = false;
                            report_Contract.xrTable2.Visible = true;
                            report_Contract.xrTable3.Visible = true;


                        }

                        if ((string)GroupingBy_Contract == "Dept")
                        {
                            report_Contract.GroupHeader1.Visible = true;
                            report_Contract.xrTable4.Visible = true;
                            report_Contract.xrTable1.Visible = true;
                            report_Contract.xrTable2.Visible = false;
                            report_Contract.xrTable3.Visible = false;

                        }
                    }
                }
                else
                {
                    if (xx.ToString() == "en_NewEmployeesReport")
                    {
                        var report2 = (en_NewEmployeesReport)sender;
                        var GroupingBy = report2.Parameters["GroupingBy"].Value;
                        if ((string)GroupingBy == "Default")
                        {
                            report2.GroupHeader1.Visible = false;
                            report2.xrTable4.Visible = false;
                            report2.xrTable1.Visible = true;
                            report2.xrTable2.Visible = true;
                            report2.xrTable3.Visible = false;


                        }

                        if ((string)GroupingBy == "Dept")
                        {
                            report2.GroupHeader1.Visible = true;
                            report2.xrTable4.Visible = true;
                            report2.xrTable1.Visible = false;
                            report2.xrTable2.Visible = false;
                            report2.xrTable3.Visible = true;

                        }
                    }
                    else if (xx.ToString() == "en_InsuranceEmployeesReport")

                    {
                        var report_ins = (en_InsuranceEmployeesReport)sender;
                        var GroupingBy = report_ins.Parameters["GroupingBy"].Value;
                        if ((string)GroupingBy == "Default")
                        {
                            report_ins.GroupHeader1.Visible = false;
                            report_ins.xrTable4.Visible = true;
                            report_ins.xrTable1.Visible = true;
                            report_ins.xrTable2.Visible = false;
                            report_ins.xrTable3.Visible = false;


                        }

                        if ((string)GroupingBy == "Dept")
                        {
                            report_ins.GroupHeader1.Visible = true;
                            report_ins.xrTable4.Visible = false;
                            report_ins.xrTable1.Visible = false;
                            report_ins.xrTable2.Visible = true;
                            report_ins.xrTable3.Visible = true;

                        }
                    }
                    else if (xx.ToString() == "en_ContractEmployeesReport")
                    {
                        var report_Contract = (en_ContractEmployeesReport)sender;
                        var GroupingBy_Contract = report_Contract.Parameters["GroupingBy"].Value;
                        if ((string)GroupingBy_Contract == "Default")
                        {
                            report_Contract.GroupHeader1.Visible = false;
                            report_Contract.xrTable4.Visible = true;
                            report_Contract.xrTable1.Visible = false;
                            report_Contract.xrTable2.Visible = false;
                            report_Contract.xrTable3.Visible = true;


                        }

                        if ((string)GroupingBy_Contract == "Dept")
                        {
                            report_Contract.GroupHeader1.Visible = true;
                            report_Contract.xrTable4.Visible = false;
                            report_Contract.xrTable1.Visible = true;
                            report_Contract.xrTable2.Visible = true;
                            report_Contract.xrTable3.Visible = false;

                        }


                    }
                }
            }
        }
        protected override void xrPictureBox2_BeforePrint(object sender, PrintEventArgs e)
        {
            int? status = (int?)report.GetCurrentColumnValue("EmpStatus");
            var Pic = (XRPictureBox)sender;
            var root = AppDomain.CurrentDomain.BaseDirectory;
            switch (status)
            {
                case 12:
                    Pic.Image = new Bitmap(root + "Content\\Icons\\12.png");

                    break;
                case 0:
                    Pic.Image = new Bitmap(root + "Content\\Icons\\0.png");
                    break;
                case 1:
                    Pic.Image = new Bitmap(root + "Content\\Icons\\1.png");
                    break;
                case 2:
                    Pic.Image = new Bitmap(root + "Content\\Icons\\2.png");
                    break;
                default:

                    Pic.Visible = false;
                    break;
            }

        }
    }
}