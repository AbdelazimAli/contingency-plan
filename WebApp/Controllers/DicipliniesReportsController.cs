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
    public class DicipliniesReportsController : BaseReportController
    {
        // GET: DicipliniesReports
        public DicipliniesReportsController()
        {
            
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Language == "ar-EG")
            {
                Icon = "3.jfif";
                ReportModels = new List<ReportViewModel>()
            {
                new ReportViewModel
                {
                    Id = 0,
                    ReportName = "EmployeeDiscplineReport",
                    ReportTitle =" جزاءات الموظفين",
                    Icon = "3.jfif",
                    Language = "ar-EG"
                }
                ,
                  new ReportViewModel
                {
                    Id = 0,
                    ReportName = "EmpDiscplines",
                    ReportTitle =" جزاءات",
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
                    Id = 0,
                    ReportName = "en_EmployeeDiscplineReport",
                    ReportTitle ="Employee Discipline Report",
                    Icon = "3.jfif",
                    Language = "ar-EG"
                }
            };

            }
        }

        public void testing()
        {

        }

       
        protected override void Report_DataSourceDemanded(object sender, EventArgs e)
        {
            var report = (XtraReport)sender;
          
            if (report.Parameters["GroupingBy"] != null)
            {
                var deptName = report.Parameters["Departments"].Value;
                var empIds = report.Parameters["Employees"].Value;
                var DisplineIds = report.Parameters["DisplineType"].Value;

                report.Parameters["mappingDeptName"].Value = string.Join(",", ((IEnumerable<int>)deptName).Select(a => a));

                report.Parameters["mappingDisplineType"].Value = string.Join(",", ((IEnumerable<int>)DisplineIds).Select(a => a));

                report.Parameters["mappingEmployees"].Value = string.Join(",", ((IEnumerable<int>)empIds).Select(a => a));

                if (Language=="ar-EG")
                {
                    var report2 = (EmployeeDiscplineReport)sender;
                    var Details = report2.Parameters["Details"].Value;
                    report2.Parameters["MappingDetails"].Value = string.Join(",", ((IEnumerable<int>)Details).Select(a => a));
                    var map = report2.Parameters["MappingDetails"].Value;
                    var GroupingBy = report2.Parameters["GroupingBy"].Value;
                    if ((string)GroupingBy == "Default")
                    {
                        //report2.GroupHeader3.Visible = false;
                        report2.GroupFooter3.Visible = false;
                        report2.GroupHeader5.Visible = false;
                        report2.GroupFooter4.Visible = false;
                        //report2.xrTableCell6.Visible = false;
                        //report2.xrTableCell18.Visible = false;
                        //report2.GroupHeader4.Visible = true;
                        //report2.GroupFooter2.Visible = true;
                        report2.GroupHeader6.Visible = true;
                        report2.xrTable7.Visible = true;
                        report2.xrTable4.Visible = false;
                        report2.xrTable2.Visible = false;
                        report2.xrTable6.Visible = true;


                    }

                    if ((string)GroupingBy == "EmpName")
                    {
                        report2.GroupHeader5.Visible = true;
                        report2.GroupFooter4.Visible = true;
                        //report2.GroupHeader4.Visible = false;
                        //report2.xrTableCell6.Visible = true;
                        //report2.GroupFooter2.Visible = false
                        //report2.xrTableCell5.Visible = true;
                        //report2.GroupHeader2.Visible = false;
                        //report2.GroupHeader3.Visible = false;
                        report2.GroupFooter3.Visible = false;
                       // report2.xrTableCell18.Visible = false;
                       // report2.xrTableCell18.WidthF = 0;
                        report2.xrTable2.Visible = false;
                        report2.xrTable7.Visible = false;
                        report2.xrTable4.Visible = true;
                        report2.xrTable6.Visible = false;
                        //if (report2.xrCheckBox1.Checked==true)
                        //{
                        //    //report2.xrTable4.Visible = false;
                        //    report2.xrTable5.Visible = false;

                        //}
                        //else
                        //{
                        //   //report2.xrTable4.Visible = true;
                        //   report2.xrTable5.Visible = true;

                        //}

                    }


                    //if ((string)GroupingBy == "ActualDescription")
                    //{
                    //    report2.GroupHeader5.Visible = false;
                    //    report2.GroupFooter4.Visible = false;
                    //    //report2.GroupHeader4.Visible = false;
                    //    //report2.xrTableCell6.Visible = false;
                    //    //report2.GroupFooter2.Visible = false;
                    //   // report2.xrTableCell18.Visible = true;
                    //    //report2.xrTableCell18.WidthF = 168.94f;
                    //    //report2.xrTableCell5.Visible = false;
                    //    //report2.GroupHeader2.Visible = false;
                    //    report2.GroupHeader3.Visible = true;
                    //    report2.GroupFooter3.Visible = true;
                    //    report2.xrTable4.Visible = false;
                    //    report2.xrTable7.Visible = false;
                    //    report2.xrTable2.Visible = true;
                    //    report2.xrTable6.Visible = false;

                    //}
                    {
                        if (((string)report2.Parameters["GroupingBy"].Value == "All" && (string)map == "1") || ((string)report2.Parameters["GroupingBy"].Value == "EmpName" && (string)map == "1")
                            || ((string)report2.Parameters["GroupingBy"].Value == "ActualDescription" && (string)map == "1"))
                        {
                            report2.Detail.Visible = false;
                        }

                        else
                        {
                            report2.Detail.Visible = true;
                        }
                    }


                }
                else
                {
                    var report2 = (en_EmployeeDiscplineReport)sender;
                    var Details = report2.Parameters["Details"].Value;
                    report2.Parameters["MappingDetails"].Value = string.Join(",", ((IEnumerable<int>)Details).Select(a => a));
                    var map = report2.Parameters["MappingDetails"].Value;
                    var GroupingBy = report2.Parameters["GroupingBy"].Value;
                    if ((string)GroupingBy == "Default")
                    {
                      //  report2.GroupHeader3.Visible = false;
                        report2.GroupFooter3.Visible = false;
                        report2.GroupHeader5.Visible = false;
                        report2.GroupFooter4.Visible = false;
                        //report2.xrTableCell6.Visible = false;
                        //report2.xrTableCell18.Visible = false;
                        //report2.GroupHeader4.Visible = true;
                        //report2.GroupFooter2.Visible = true;
                        report2.GroupHeader6.Visible = true;
                        report2.xrTable7.Visible = true;
                        report2.xrTable4.Visible = false;
                        //report2.xrTable2.Visible = false;
                        report2.xrTable6.Visible = true;


                    }

                    if ((string)GroupingBy == "EmpName")
                    {
                        report2.GroupHeader5.Visible = true;
                        report2.GroupFooter4.Visible = true;
                        //report2.GroupHeader4.Visible = false;
                        //report2.xrTableCell6.Visible = true;
                        //report2.GroupFooter2.Visible = false
                        //report2.xrTableCell5.Visible = true;
                        //report2.GroupHeader2.Visible = false;
                       // report2.GroupHeader3.Visible = false;
                        report2.GroupFooter3.Visible = false;
                        // report2.xrTableCell18.Visible = false;
                        // report2.xrTableCell18.WidthF = 0;
                       // report2.xrTable2.Visible = false;
                        report2.xrTable7.Visible = false;
                        report2.xrTable4.Visible = true;
                        report2.xrTable6.Visible = false;


                    }


                    //if ((string)GroupingBy == "ActualDescription")
                    //{
                    //    report2.GroupHeader5.Visible = false;
                    //    report2.GroupFooter4.Visible = false;
                    //    //report2.GroupHeader4.Visible = false;
                    //    //report2.xrTableCell6.Visible = false;
                    //    //report2.GroupFooter2.Visible = false;
                    //   // report2.xrTableCell18.Visible = true;
                    //    //report2.xrTableCell18.WidthF = 168.94f;
                    //    //report2.xrTableCell5.Visible = false;
                    //    //report2.GroupHeader2.Visible = false;
                    //    report2.GroupHeader3.Visible = true;
                    //    report2.GroupFooter3.Visible = true;
                    //    report2.xrTable4.Visible = false;
                    //    report2.xrTable7.Visible = false;
                    //    report2.xrTable2.Visible = true;
                    //    report2.xrTable6.Visible = false;

                    //}
                    {
                        if (((string)report2.Parameters["GroupingBy"].Value == "All" && (string)map == "1") || ((string)report2.Parameters["GroupingBy"].Value == "EmpName" && (string)map == "1")
                            || ((string)report2.Parameters["GroupingBy"].Value == "ActualDescription" && (string)map == "1"))
                        {
                            report2.Detail.Visible = false;
                        }

                        else
                        {
                            report2.Detail.Visible = true;
                        }
                    }
                }

              
            }



        }
    }
}