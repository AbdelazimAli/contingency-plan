using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using WebApp.Reports.EmployeesReports;

/// <summary>
/// Summary description for TerminatedEmpReport
/// </summary>
public class en_TerminatedEmpReport : DevExpress.XtraReports.UI.XtraReport,IBasicInfoReport
{
    private DetailBand Detail;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell30;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell7;
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private PageHeaderBand PageHeader;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell5;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel4;
    private PageFooterBand PageFooter;
    private FormattingRule formattingRule1;
    private FormattingRule formattingRule2;
    private FormattingRule formattingRule3;
    private FormattingRule formattingRule4;
    private FormattingRule formattingRule5;
    private FormattingRule formattingRule6;
    private FormattingRule formattingRule7;
    private XRControlStyle Odding;
    private XRControlStyle GroupCaption3;
    private XRControlStyle xrControlStyle1;
    private XRControlStyle Title;
    private XRControlStyle xrControlStyle4;
    private XRControlStyle xrControlStyle2;
    private XRControlStyle tbl_header_style;
    private XRControlStyle tbl_even_detail;
    private XRControlStyle tbl_odd_detail;
    private CalculatedField StatusImg;
    private DevExpress.XtraReports.Parameters.Parameter from;
    private DevExpress.XtraReports.Parameters.Parameter to;
    private DevExpress.XtraReports.Parameters.Parameter Culture;
    private XRTableCell xrTableCell6;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell36;
    private CalculatedField PeriodYear;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell23;
    private CalculatedField PeriodMonth;
    private CalculatedField PeriodDay;
    private DevExpress.XtraReports.Parameters.Parameter EmpIds;
    private DevExpress.XtraReports.Parameters.Parameter EmpCode;
    private DevExpress.XtraReports.Parameters.Parameter CompanyName;
    private XRLabel xrLabel1;
    private DevExpress.XtraReports.Parameters.Parameter User;
    public XRPictureBox companyLogo;
    private DevExpress.XtraReports.Parameters.Parameter MappingEmpIds;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell27;
    private XRPageInfo xrPageInfo1;
    private XRLabel xrLabel10;
    private XRLabel xrLabel9;
    private XRPageInfo xrPageInfo2;
    private XRLabel xrLabel5;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource2;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell41;
    private DevExpress.XtraReports.Parameters.Parameter CompanyId;
    private DevExpress.XtraReports.Parameters.Parameter mappingGender;
    private DevExpress.XtraReports.Parameters.Parameter Gender;
    private DevExpress.XtraReports.Parameters.Parameter DeptIds;
    private DevExpress.XtraReports.Parameters.Parameter JobIds;
    private DevExpress.XtraReports.Parameters.Parameter MappingDeptIds;
    private DevExpress.XtraReports.Parameters.Parameter MappingJobIds;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private XRTableCell xrTableCell43;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public XRPictureBox CompanyLogo
    {
        get
        {
            return this.companyLogo;
        }

        set
        {
            CompanyLogo = this.companyLogo;
        }
    }

    public en_TerminatedEmpReport()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter5 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter6 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings1 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings1 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings2 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings3 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery2 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter7 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter8 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter9 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter10 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter11 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter12 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter13 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(en_TerminatedEmpReport));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.sqlDataSource2 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.companyLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.CompanyName = new DevExpress.XtraReports.Parameters.Parameter();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.User = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRule2 = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRule3 = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRule4 = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRule5 = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRule6 = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRule7 = new DevExpress.XtraReports.UI.FormattingRule();
            this.Odding = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupCaption3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle4 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_header_style = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_even_detail = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_odd_detail = new DevExpress.XtraReports.UI.XRControlStyle();
            this.StatusImg = new DevExpress.XtraReports.UI.CalculatedField();
            this.from = new DevExpress.XtraReports.Parameters.Parameter();
            this.to = new DevExpress.XtraReports.Parameters.Parameter();
            this.Culture = new DevExpress.XtraReports.Parameters.Parameter();
            this.PeriodYear = new DevExpress.XtraReports.UI.CalculatedField();
            this.PeriodMonth = new DevExpress.XtraReports.UI.CalculatedField();
            this.PeriodDay = new DevExpress.XtraReports.UI.CalculatedField();
            this.EmpIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.EmpCode = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingEmpIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CompanyId = new DevExpress.XtraReports.Parameters.Parameter();
            this.mappingGender = new DevExpress.XtraReports.Parameters.Parameter();
            this.Gender = new DevExpress.XtraReports.Parameters.Parameter();
            this.DeptIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.JobIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingDeptIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingJobIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 64.5F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("DeptName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ServStartDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ServStartDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("EmpName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("EmpName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("JobName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("JobName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("DeptName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("ServStartDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ServStartDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("ContractType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ContractType", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("ActualDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ActualDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("PeriodYear", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("PeriodYear", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("PeriodMonth", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("PeriodMonth", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("PeriodDay", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("PeriodDay", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderColor = System.Drawing.Color.Silver;
            this.xrTable2.BorderWidth = 1F;
            this.xrTable2.Dpi = 254F;
            this.xrTable2.EvenStyleName = "tbl_odd_detail";
            this.xrTable2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(4.768372E-07F, 1F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.OddStyleName = "tbl_even_detail";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(2963F, 63.5F);
            this.xrTable2.StylePriority.UseBorderColor = false;
            this.xrTable2.StylePriority.UseBorderWidth = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18,
            this.xrTableCell19,
            this.xrTableCell21,
            this.xrTableCell42,
            this.xrTableCell23,
            this.xrTableCell20,
            this.xrTableCell43,
            this.xrTableCell28,
            this.xrTableCell29,
            this.xrTableCell30,
            this.xrTableCell32,
            this.xrTableCell9,
            this.xrTableCell33,
            this.xrTableCell34,
            this.xrTableCell7});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 11.5D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Dpi = 254F;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell18.Summary = xrSummary1;
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell18.Weight = 0.14553009948453644D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.EmpCode")});
            this.xrTableCell19.Dpi = 254F;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell19.Weight = 0.17852307308046364D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.EmpName")});
            this.xrTableCell21.Dpi = 254F;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell21.Weight = 0.4075774350211423D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.JobName")});
            this.xrTableCell23.Dpi = 254F;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell23.Weight = 0.24694727887631193D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.BookmarkParent = this.xrTableCell12;
            this.xrTableCell20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.DeptName"),
            new DevExpress.XtraReports.UI.XRBinding("Bookmark", null, "SP_TerminatedEmp.DeptName")});
            this.xrTableCell20.Dpi = 254F;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell20.Weight = 0.402203482075027D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.InteractiveSorting.FieldName = "DeptName";
            this.xrTableCell12.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.RowSpan = 2;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "Department";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell12.Weight = 0.40220349506766034D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.ServStartDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell28.Dpi = 254F;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell28.Weight = 0.30648671948916267D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.ContractType")});
            this.xrTableCell29.Dpi = 254F;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseTextAlignment = false;
            this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell29.Weight = 0.3247858183465156D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.ActualDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell30.Dpi = 254F;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.StylePriority.UseTextAlignment = false;
            this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell30.Weight = 0.41071007080952682D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.BookmarkParent = this.xrTableCell15;
            this.xrTableCell32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.PeriodDay"),
            new DevExpress.XtraReports.UI.XRBinding("Bookmark", null, "EmployeeDetailsReport.DeptName")});
            this.xrTableCell32.Dpi = 254F;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.StylePriority.UseTextAlignment = false;
            this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell32.Weight = 0.22080520249654734D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.InteractiveSorting.FieldName = "ContractType";
            this.xrTableCell15.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.RowSpan = 2;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "Contract Type";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell15.Weight = 0.32478583053274013D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.PeriodMonth")});
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell9.Weight = 0.23094529471249242D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.PeriodYear")});
            this.xrTableCell33.Dpi = 254F;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.StylePriority.UseTextAlignment = false;
            this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell33.Weight = 0.22322470828707763D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.Reason")});
            this.xrTableCell34.Dpi = 254F;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.StylePriority.UseTextAlignment = false;
            this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell34.Weight = 0.42979460723290142D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.ReasonDesc")});
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            this.xrTableCell7.Summary = xrSummary2;
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell7.Weight = 0.40202246290029137D;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 170.8879F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.StyleName = "tbl_header_style";
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(4.768372E-07F, 12.55651F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow3});
            this.xrTable1.SizeF = new System.Drawing.SizeF(2963F, 158.3314F);
            this.xrTable1.StyleName = "tbl_header_style";
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell6,
            this.xrTableCell10,
            this.xrTableCell37,
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell40,
            this.xrTableCell13,
            this.xrTableCell15,
            this.xrTableCell14,
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell5});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.StylePriority.UseTextAlignment = false;
            this.xrTableRow1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableRow1.Weight = 11.5D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.RowSpan = 2;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "ID";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.1455300477161745D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.RowSpan = 2;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Code";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell6.Weight = 0.1785232208536145D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.InteractiveSorting.FieldName = "EmpName";
            this.xrTableCell10.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.RowSpan = 2;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Employee Name";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 0.40757741477186038D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.InteractiveSorting.FieldName = "JobName";
            this.xrTableCell11.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.RowSpan = 2;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "Job";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 0.24694699903941048D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.InteractiveSorting.FieldName = "ServStartDate";
            this.xrTableCell13.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.RowSpan = 2;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "Start Date";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell13.Weight = 0.30648671501664904D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.InteractiveSorting.FieldName = "ActualDate";
            this.xrTableCell14.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.RowSpan = 2;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "Actual Termination Date";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell14.Weight = 0.41071005476668571D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.Text = "Serving Years";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell16.Weight = 0.67497522560367251D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.CanGrow = false;
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.RowSpan = 2;
            this.xrTableCell17.StylePriority.UseFont = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = "Reason ";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell17.Weight = 0.4297949451175358D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.RowSpan = 2;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "Reason Description";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell5.Weight = 0.40202218621402758D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell38,
            this.xrTableCell39,
            this.xrTableCell8,
            this.xrTableCell22,
            this.xrTableCell41,
            this.xrTableCell24,
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell3,
            this.xrTableCell27,
            this.xrTableCell31,
            this.xrTableCell35,
            this.xrTableCell36});
            this.xrTableRow3.Dpi = 254F;
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 11.5D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "xrTableCell1";
            this.xrTableCell1.Weight = 0.14553004785417675D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.Weight = 0.17852322111461749D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.Dpi = 254F;
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.Weight = 0.40757742393776852D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "xrTableCell8";
            this.xrTableCell8.Weight = 0.24694700155666718D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Dpi = 254F;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "xrTableCell22";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell22.Weight = 0.40220349510432352D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Dpi = 254F;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.Text = "xrTableCell24";
            this.xrTableCell24.Weight = 0.30648671498338981D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Dpi = 254F;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Text = "xrTableCell25";
            this.xrTableCell25.Weight = 0.32478583071096057D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Dpi = 254F;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Text = "xrTableCell26";
            this.xrTableCell26.Weight = 0.4107100552311363D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.InteractiveSorting.FieldName = "PeriodDay";
            this.xrTableCell3.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "Day";
            this.xrTableCell3.Weight = 0.22080519762702355D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Dpi = 254F;
            this.xrTableCell27.InteractiveSorting.FieldName = "PeriodMonth";
            this.xrTableCell27.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Text = "Month";
            this.xrTableCell27.Weight = 0.23094567509307959D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.Dpi = 254F;
            this.xrTableCell31.InteractiveSorting.FieldName = "PeriodYear";
            this.xrTableCell31.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.Text = "Year";
            this.xrTableCell31.Weight = 0.2232243528279006D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.CanGrow = false;
            this.xrTableCell35.Dpi = 254F;
            this.xrTableCell35.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.StylePriority.UseFont = false;
            this.xrTableCell35.Weight = 0.42979499174648839D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.Dpi = 254F;
            this.xrTableCell36.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.StylePriority.UseFont = false;
            this.xrTableCell36.Text = "xrTableCell36";
            this.xrTableCell36.Weight = 0.4020221168195125D;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.companyLogo,
            this.xrLabel4,
            this.xrLabel1});
            this.ReportHeader.Dpi = 254F;
            this.ReportHeader.HeightF = 322.2362F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel5
            // 
            this.xrLabel5.AutoWidth = true;
            this.xrLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.xrLabel5.CanShrink = true;
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.sqlDataSource2, "sp_ReportTitle.Column1")});
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 214.455F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 13, 13, 13, 254F);
            this.xrLabel5.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.No;
            this.xrLabel5.SizeF = new System.Drawing.SizeF(2963F, 93.36455F);
            this.xrLabel5.StylePriority.UseBackColor = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseForeColor = false;
            this.xrLabel5.StylePriority.UsePadding = false;
            // 
            // sqlDataSource2
            // 
            this.sqlDataSource2.ConnectionName = "HrContext";
            this.sqlDataSource2.Name = "sqlDataSource2";
            storedProcQuery1.Name = "sp_ReportTitle";
            queryParameter1.Name = "@EmpIds";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingEmpIds]", typeof(string));
            queryParameter2.Name = "@JobIds";
            queryParameter2.Type = typeof(string);
            queryParameter3.Name = "@deptIds";
            queryParameter3.Type = typeof(string);
            queryParameter4.Name = "@culture";
            queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter4.Value = new DevExpress.DataAccess.Expression("[Parameters.Culture]", typeof(string));
            queryParameter5.Name = "@NationIds";
            queryParameter5.Type = typeof(string);
            queryParameter6.Name = "@ContractsIds";
            queryParameter6.Type = typeof(string);
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.Parameters.Add(queryParameter5);
            storedProcQuery1.Parameters.Add(queryParameter6);
            storedProcQuery1.StoredProcName = "sp_ReportTitle";
            this.sqlDataSource2.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.sqlDataSource2.ResultSchemaSerializable = "PERhdGFTZXQgTmFtZT0ic3FsRGF0YVNvdXJjZTIiPjxWaWV3IE5hbWU9InNwX1JlcG9ydFRpdGxlIj48R" +
    "mllbGQgTmFtZT0iQ29sdW1uMSIgVHlwZT0iU3RyaW5nIiAvPjwvVmlldz48L0RhdGFTZXQ+";
            // 
            // companyLogo
            // 
            this.companyLogo.Dpi = 254F;
            this.companyLogo.LocationFloat = new DevExpress.Utils.PointFloat(2563F, 0F);
            this.companyLogo.Name = "companyLogo";
            this.companyLogo.SizeF = new System.Drawing.SizeF(400F, 200F);
            this.companyLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // xrLabel4
            // 
            this.xrLabel4.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(825.6727F, 38.99773F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(1178.278F, 161.0023F);
            this.xrLabel4.StylePriority.UseBorderColor = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseForeColor = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Terminated Employees Report";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.CompanyName, "Text", "")});
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(3.219899F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(758.4722F, 139.5589F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // CompanyName
            // 
            this.CompanyName.Description = "CompanyName";
            this.CompanyName.Name = "CompanyName";
            this.CompanyName.Visible = false;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrLabel10,
            this.xrLabel9,
            this.xrPageInfo2});
            this.PageFooter.Dpi = 254F;
            this.PageFooter.HeightF = 90.17F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Dpi = 254F;
            this.xrPageInfo1.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.Format = "Page{0}of {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(2534.375F, 31.75F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(428.625F, 58.42F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel10
            // 
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.User, "Text", "")});
            this.xrLabel10.Dpi = 254F;
            this.xrLabel10.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(945.1709F, 31.75F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "xrLabel10";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // User
            // 
            this.User.Description = "User";
            this.User.Name = "User";
            this.User.Visible = false;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(807.5875F, 31.75F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(137.5833F, 58.41999F);
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "User :";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Dpi = 254F;
            this.xrPageInfo2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.Format = "{0:yyyy-MM-dd h:mm tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 31.75F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(664.6508F, 58.42F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // formattingRule1
            // 
            this.formattingRule1.Condition = "[EmpStatus] == 0";
            this.formattingRule1.DataMember = "EmployeeDetailsReport";
            this.formattingRule1.Formatting.BackColor = System.Drawing.Color.White;
            this.formattingRule1.Formatting.BorderColor = System.Drawing.Color.White;
            this.formattingRule1.Formatting.BorderWidth = 10F;
            this.formattingRule1.Name = "formattingRule1";
            // 
            // formattingRule2
            // 
            this.formattingRule2.Condition = "[EmpStatus] == 1";
            this.formattingRule2.DataMember = "EmployeeDetailsReport";
            this.formattingRule2.Formatting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.formattingRule2.Formatting.BorderColor = System.Drawing.Color.White;
            this.formattingRule2.Formatting.BorderWidth = 10F;
            this.formattingRule2.Name = "formattingRule2";
            // 
            // formattingRule3
            // 
            this.formattingRule3.Condition = "[EmpStatus] == 2";
            this.formattingRule3.DataMember = "EmployeeDetailsReport";
            this.formattingRule3.Formatting.BackColor = System.Drawing.Color.Yellow;
            this.formattingRule3.Formatting.BorderColor = System.Drawing.Color.White;
            this.formattingRule3.Formatting.BorderWidth = 10F;
            this.formattingRule3.Name = "formattingRule3";
            // 
            // formattingRule4
            // 
            this.formattingRule4.Condition = "[EmpStatus] == 3";
            this.formattingRule4.DataMember = "EmployeeDetailsReport";
            this.formattingRule4.Formatting.BackColor = System.Drawing.Color.Red;
            this.formattingRule4.Formatting.BorderColor = System.Drawing.Color.White;
            this.formattingRule4.Formatting.BorderWidth = 10F;
            this.formattingRule4.Name = "formattingRule4";
            // 
            // formattingRule5
            // 
            this.formattingRule5.Condition = "[EmpStatus] == 12";
            this.formattingRule5.DataMember = "EmployeeDetailsReport";
            this.formattingRule5.Formatting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.formattingRule5.Formatting.BorderColor = System.Drawing.Color.White;
            this.formattingRule5.Formatting.BorderWidth = 10F;
            this.formattingRule5.Name = "formattingRule5";
            // 
            // formattingRule6
            // 
            this.formattingRule6.Condition = "[EmpStatus]==1   Or ([EmpStatus] != 0   And  [EmpStatus] != 3   And  [EmpStatus] " +
    "!= 12   And [EmpStatus] != 2 )";
            this.formattingRule6.DataMember = "EmployeeDetailsReport";
            this.formattingRule6.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.formattingRule6.Name = "formattingRule6";
            // 
            // formattingRule7
            // 
            this.formattingRule7.Condition = "[CompanyId] == 4172 And [Source]==\'Company\'  And [TypeId]=1";
            this.formattingRule7.DataMember = "CompanyDocs.CompanyDocsCompanyDocuments";
            this.formattingRule7.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.formattingRule7.Name = "formattingRule7";
            // 
            // Odding
            // 
            this.Odding.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Odding.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.Odding.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Odding.Name = "Odding";
            this.Odding.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            // 
            // GroupCaption3
            // 
            this.GroupCaption3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.GroupCaption3.BorderColor = System.Drawing.Color.Silver;
            this.GroupCaption3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.GroupCaption3.BorderWidth = 2F;
            this.GroupCaption3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.GroupCaption3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.GroupCaption3.Name = "GroupCaption3";
            this.GroupCaption3.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 5, 0, 0, 254F);
            this.GroupCaption3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.xrControlStyle1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrControlStyle1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrControlStyle1.Name = "xrControlStyle1";
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Tahoma", 14F);
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.Title.Name = "Title";
            // 
            // xrControlStyle4
            // 
            this.xrControlStyle4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrControlStyle4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrControlStyle4.Name = "xrControlStyle4";
            this.xrControlStyle4.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            // 
            // xrControlStyle2
            // 
            this.xrControlStyle2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrControlStyle2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrControlStyle2.Name = "xrControlStyle2";
            this.xrControlStyle2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            // 
            // tbl_header_style
            // 
            this.tbl_header_style.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tbl_header_style.BorderColor = System.Drawing.Color.Silver;
            this.tbl_header_style.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.tbl_header_style.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_header_style.BorderWidth = 1F;
            this.tbl_header_style.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_header_style.ForeColor = System.Drawing.Color.Black;
            this.tbl_header_style.Name = "tbl_header_style";
            this.tbl_header_style.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 13, 13, 13, 254F);
            this.tbl_header_style.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // tbl_even_detail
            // 
            this.tbl_even_detail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tbl_even_detail.BorderColor = System.Drawing.Color.Black;
            this.tbl_even_detail.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.tbl_even_detail.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_even_detail.BorderWidth = 1F;
            this.tbl_even_detail.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_even_detail.ForeColor = System.Drawing.Color.Black;
            this.tbl_even_detail.Name = "tbl_even_detail";
            this.tbl_even_detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 13, 13, 13, 254F);
            this.tbl_even_detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // tbl_odd_detail
            // 
            this.tbl_odd_detail.BackColor = System.Drawing.Color.White;
            this.tbl_odd_detail.BorderColor = System.Drawing.Color.Black;
            this.tbl_odd_detail.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.tbl_odd_detail.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_odd_detail.BorderWidth = 1F;
            this.tbl_odd_detail.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_odd_detail.ForeColor = System.Drawing.Color.Black;
            this.tbl_odd_detail.Name = "tbl_odd_detail";
            this.tbl_odd_detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 13, 13, 13, 254F);
            this.tbl_odd_detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // StatusImg
            // 
            this.StatusImg.DataMember = "EmployeeDetailsReport";
            this.StatusImg.Expression = "\'~/Content/Icons/\' + ToStr([EmpStatus]) + \'.png\'";
            this.StatusImg.Name = "StatusImg";
            // 
            // from
            // 
            this.from.Description = "Termination Date From";
            this.from.Name = "from";
            this.from.Type = typeof(System.DateTime);
            this.from.ValueInfo = "1999-01-01";
            // 
            // to
            // 
            this.to.Description = "Termination Date To";
            this.to.Name = "to";
            this.to.Type = typeof(System.DateTime);
            this.to.ValueInfo = "2099-01-01";
            // 
            // Culture
            // 
            this.Culture.Description = "Culture";
            this.Culture.Name = "Culture";
            this.Culture.ValueInfo = "en_GB";
            this.Culture.Visible = false;
            // 
            // PeriodYear
            // 
            this.PeriodYear.DataMember = "SP_TerminatedEmp";
            this.PeriodYear.Expression = "Abs(ToInt([ServYear]))";
            this.PeriodYear.Name = "PeriodYear";
            // 
            // PeriodMonth
            // 
            this.PeriodMonth.DataMember = "SP_TerminatedEmp";
            this.PeriodMonth.Expression = "(DateDiffMonth([ServStartDate],[ActualDate])%12)";
            this.PeriodMonth.Name = "PeriodMonth";
            // 
            // PeriodDay
            // 
            this.PeriodDay.DataMember = "SP_TerminatedEmp";
            this.PeriodDay.Expression = "(DateDiffDay([ServStartDate],[ActualDate])%365)%30";
            this.PeriodDay.FieldType = DevExpress.XtraReports.UI.FieldType.Int32;
            this.PeriodDay.Name = "PeriodDay";
            // 
            // EmpIds
            // 
            this.EmpIds.Description = "Employee Name";
            dynamicListLookUpSettings1.DataAdapter = null;
            dynamicListLookUpSettings1.DataMember = "SP_TerminatedEmp";
            dynamicListLookUpSettings1.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings1.DisplayMember = "EmpName";
            dynamicListLookUpSettings1.ValueMember = "Id";
            this.EmpIds.LookUpSettings = dynamicListLookUpSettings1;
            this.EmpIds.MultiValue = true;
            this.EmpIds.Name = "EmpIds";
            this.EmpIds.Type = typeof(int);
            // 
            // EmpCode
            // 
            this.EmpCode.Description = "Employee Code";
            this.EmpCode.Name = "EmpCode";
            // 
            // MappingEmpIds
            // 
            this.MappingEmpIds.Description = "MappingEmpIds";
            this.MappingEmpIds.Name = "MappingEmpIds";
            this.MappingEmpIds.Visible = false;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.Dpi = 254F;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.RowSpan = 2;
            this.xrTableCell37.StylePriority.UseTextAlignment = false;
            this.xrTableCell37.Text = "Gender";
            this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell37.Weight = 0.22347423080068746D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.Dpi = 254F;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.Text = "xrTableCell39";
            this.xrTableCell39.Weight = 0.2234742288362426D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.Dpi = 254F;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.RowSpan = 2;
            this.xrTableCell40.StylePriority.UseTextAlignment = false;
            this.xrTableCell40.Text = "Direct Manager";
            this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell40.Weight = 0.3583034358829037D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.Dpi = 254F;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.StylePriority.UseTextAlignment = false;
            this.xrTableCell41.Text = "xrTableCell41";
            this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell41.Weight = 0.3583034360354142D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.Gender")});
            this.xrTableCell42.Dpi = 254F;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StylePriority.UseTextAlignment = false;
            this.xrTableCell42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell42.Weight = 0.22347405269080375D;
            // 
            // CompanyId
            // 
            this.CompanyId.Description = "CompanyId";
            this.CompanyId.Name = "CompanyId";
            this.CompanyId.Type = typeof(int);
            this.CompanyId.ValueInfo = "0";
            this.CompanyId.Visible = false;
            // 
            // mappingGender
            // 
            this.mappingGender.Description = "mappingGender";
            this.mappingGender.Name = "mappingGender";
            this.mappingGender.Visible = false;
            // 
            // Gender
            // 
            this.Gender.Description = "Gender";
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(1, "Male"));
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(2, "Female"));
            this.Gender.LookUpSettings = staticListLookUpSettings1;
            this.Gender.MultiValue = true;
            this.Gender.Name = "Gender";
            this.Gender.Type = typeof(int);
            // 
            // DeptIds
            // 
            this.DeptIds.Description = "Department Name";
            dynamicListLookUpSettings2.DataAdapter = null;
            dynamicListLookUpSettings2.DataMember = "SP_TerminatedEmp";
            dynamicListLookUpSettings2.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings2.DisplayMember = "DeptName";
            dynamicListLookUpSettings2.ValueMember = "DeptId";
            this.DeptIds.LookUpSettings = dynamicListLookUpSettings2;
            this.DeptIds.MultiValue = true;
            this.DeptIds.Name = "DeptIds";
            this.DeptIds.Type = typeof(int);
            // 
            // JobIds
            // 
            this.JobIds.Description = "Job Name";
            dynamicListLookUpSettings3.DataAdapter = null;
            dynamicListLookUpSettings3.DataMember = "SP_TerminatedEmp";
            dynamicListLookUpSettings3.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings3.DisplayMember = "JobName";
            dynamicListLookUpSettings3.ValueMember = "JobId";
            this.JobIds.LookUpSettings = dynamicListLookUpSettings3;
            this.JobIds.MultiValue = true;
            this.JobIds.Name = "JobIds";
            this.JobIds.Type = typeof(int);
            // 
            // MappingDeptIds
            // 
            this.MappingDeptIds.Description = "MappingDeptIds";
            this.MappingDeptIds.Name = "MappingDeptIds";
            this.MappingDeptIds.Visible = false;
            // 
            // MappingJobIds
            // 
            this.MappingJobIds.Description = "MappingJobIds";
            this.MappingJobIds.Name = "MappingJobIds";
            this.MappingJobIds.Visible = false;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "HrContext";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery2.MetaSerializable = "<Meta X=\"20\" Y=\"20\" Width=\"100\" Height=\"507\" />";
            storedProcQuery2.Name = "SP_TerminatedEmp";
            queryParameter7.Name = "@CompanyId";
            queryParameter7.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter7.Value = new DevExpress.DataAccess.Expression("[Parameters.CompanyId]", typeof(int));
            queryParameter8.Name = "@Culture";
            queryParameter8.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter8.Value = new DevExpress.DataAccess.Expression("[Parameters.Culture]", typeof(string));
            queryParameter9.Name = "@EmpIds";
            queryParameter9.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter9.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingEmpIds]", typeof(string));
            queryParameter10.Name = "@EmpCode";
            queryParameter10.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter10.Value = new DevExpress.DataAccess.Expression("[Parameters.EmpCode]", typeof(string));
            queryParameter11.Name = "@DeptIds";
            queryParameter11.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter11.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingDeptIds]", typeof(string));
            queryParameter12.Name = "@JobIds";
            queryParameter12.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter12.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingJobIds]", typeof(string));
            queryParameter13.Name = "@Gender";
            queryParameter13.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter13.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingGender]", typeof(string));
            storedProcQuery2.Parameters.Add(queryParameter7);
            storedProcQuery2.Parameters.Add(queryParameter8);
            storedProcQuery2.Parameters.Add(queryParameter9);
            storedProcQuery2.Parameters.Add(queryParameter10);
            storedProcQuery2.Parameters.Add(queryParameter11);
            storedProcQuery2.Parameters.Add(queryParameter12);
            storedProcQuery2.Parameters.Add(queryParameter13);
            storedProcQuery2.StoredProcName = "SP_TerminatedEmp";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery2});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_TerminatedEmp.MangerName")});
            this.xrTableCell43.Dpi = 254F;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StylePriority.UseTextAlignment = false;
            this.xrTableCell43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell43.Weight = 0.35830344531571917D;
            // 
            // en_TerminatedEmpReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.ReportHeader,
            this.PageFooter});
            this.Bookmark = "Company Structure";
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.StatusImg,
            this.PeriodYear,
            this.PeriodMonth,
            this.PeriodDay});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource2,
            this.sqlDataSource1});
            this.DataMember = "SP_TerminatedEmp";
            this.DataSource = this.sqlDataSource1;
            this.Dpi = 254F;
            this.FilterString = "[ActualDate] Between(?from, ?to)";
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1,
            this.formattingRule2,
            this.formattingRule3,
            this.formattingRule4,
            this.formattingRule5,
            this.formattingRule6,
            this.formattingRule7});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(3, 3, 0, 0);
            this.PageHeight = 2100;
            this.PageWidth = 2970;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.from,
            this.to,
            this.Culture,
            this.EmpIds,
            this.EmpCode,
            this.CompanyName,
            this.User,
            this.MappingEmpIds,
            this.CompanyId,
            this.mappingGender,
            this.Gender,
            this.DeptIds,
            this.JobIds,
            this.MappingDeptIds,
            this.MappingJobIds});
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.SnapGridSize = 25F;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Odding,
            this.GroupCaption3,
            this.xrControlStyle1,
            this.Title,
            this.xrControlStyle4,
            this.xrControlStyle2,
            this.tbl_header_style,
            this.tbl_even_detail,
            this.tbl_odd_detail});
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
