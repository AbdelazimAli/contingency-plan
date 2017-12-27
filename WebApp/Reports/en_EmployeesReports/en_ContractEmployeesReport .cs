using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using WebApp.Reports.EmployeesReports;

/// <summary>
/// Summary description for InsuranceEmployeesReports
/// </summary>
public class en_ContractEmployeesReport : DevExpress.XtraReports.UI.XtraReport,IBasicInfoReport
{
    private DetailBand Detail;
    public XRTable xrTable4;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell43;
    public XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell30;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell32;
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private PageHeaderBand PageHeader;
    public XRTable xrTable3;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell28;
    public XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell13;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel5;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource5;
    private XRLabel xrLabel4;
    private XRLabel xrLabel2;
    private DevExpress.XtraReports.Parameters.Parameter CompanyName;
    private XRPictureBox companyLogo;
    private PageFooterBand PageFooter;
    private XRLabel xrLabel9;
    private XRPageInfo xrPageInfo2;
    private XRPageInfo xrPageInfo1;
    private XRLabel xrLabel16;
    private DevExpress.XtraReports.Parameters.Parameter User;
    private XRPageBreak xrPageBreak1;
    public GroupHeaderBand GroupHeader1;
    private XRLabel xrLabel3;
    private XRLabel xrLabel1;
    private XRLabel xrLabel17;
    private XRLabel xrLabel18;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private FormattingRule formattingRule1;
    private FormattingRule formattingRule2;
    private FormattingRule formattingRule3;
    private FormattingRule formattingRule4;
    private FormattingRule formattingRule5;
    private XRControlStyle Odding;
    private XRControlStyle GroupCaption3;
    private XRControlStyle xrControlStyle1;
    private XRControlStyle Title;
    private XRControlStyle odd;
    private XRControlStyle xrControlStyle2;
    private XRControlStyle tbl_header_style;
    private XRControlStyle tbl_even_detail;
    private XRControlStyle tbl_odd_detail;
    private XRControlStyle ev;
    private DevExpress.XtraReports.Parameters.Parameter CompanyId;
    private DevExpress.XtraReports.Parameters.Parameter Culture;
    private DevExpress.XtraReports.Parameters.Parameter EmpIds;
    private DevExpress.XtraReports.Parameters.Parameter Gender;
    private DevExpress.XtraReports.Parameters.Parameter DeptIds;
    private DevExpress.XtraReports.Parameters.Parameter JobIds;
    private DevExpress.XtraReports.Parameters.Parameter ContractStatus;
    private DevExpress.XtraReports.Parameters.Parameter ContractType;
    private DevExpress.XtraReports.Parameters.Parameter ConEmpStartDate;
    private DevExpress.XtraReports.Parameters.Parameter ConEmpEndDate;
    private DevExpress.XtraReports.Parameters.Parameter MappingDeptIds;
    private DevExpress.XtraReports.Parameters.Parameter MappingEmpIds;
    private DevExpress.XtraReports.Parameters.Parameter MappingJobIds;
    private DevExpress.XtraReports.Parameters.Parameter MappingContractType;
    private DevExpress.XtraReports.Parameters.Parameter MappingContractStatus;
    private DevExpress.XtraReports.Parameters.Parameter mappingGender;
    private DevExpress.XtraReports.Parameters.Parameter GroupingBy;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell44;
    private WebApp.Reports.en_EmployeesReports.Bullet bullet2;
    private WebApp.Reports.en_EmployeesReports.Bullet bullet1;
    private FormattingRule formattingRule6;
    private XRLabel xrLabel7;
    private XRLabel xrLabel6;
    private WebApp.Reports.en_EmployeesReports.Bullet bullet3;
    private WebApp.Reports.en_EmployeesReports.Bullet bullet4;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell47;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell45;
    private FormattingRule Invisible;

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

    public en_ContractEmployeesReport()
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
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter5 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter6 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter7 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter8 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(en_ContractEmployeesReport));
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery2 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter9 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter10 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter11 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter12 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter13 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter14 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings1 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings1 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings2 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings2 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings3 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings3 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings4 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings4 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.formattingRule6 = new DevExpress.XtraReports.UI.FormattingRule();
            this.Invisible = new DevExpress.XtraReports.UI.FormattingRule();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRule2 = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRule3 = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRule4 = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRule5 = new DevExpress.XtraReports.UI.FormattingRule();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.sqlDataSource5 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.CompanyName = new DevExpress.XtraReports.Parameters.Parameter();
            this.companyLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.User = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.Odding = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupCaption3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_header_style = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_even_detail = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_odd_detail = new DevExpress.XtraReports.UI.XRControlStyle();
            this.ev = new DevExpress.XtraReports.UI.XRControlStyle();
            this.CompanyId = new DevExpress.XtraReports.Parameters.Parameter();
            this.Culture = new DevExpress.XtraReports.Parameters.Parameter();
            this.EmpIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.Gender = new DevExpress.XtraReports.Parameters.Parameter();
            this.DeptIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.JobIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.ContractStatus = new DevExpress.XtraReports.Parameters.Parameter();
            this.ContractType = new DevExpress.XtraReports.Parameters.Parameter();
            this.ConEmpStartDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.ConEmpEndDate = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingDeptIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingEmpIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingJobIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingContractType = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingContractStatus = new DevExpress.XtraReports.Parameters.Parameter();
            this.mappingGender = new DevExpress.XtraReports.Parameters.Parameter();
            this.GroupingBy = new DevExpress.XtraReports.Parameters.Parameter();
            this.bullet2 = new WebApp.Reports.en_EmployeesReports.Bullet();
            this.bullet1 = new WebApp.Reports.en_EmployeesReports.Bullet();
            this.bullet3 = new WebApp.Reports.en_EmployeesReports.Bullet();
            this.bullet4 = new WebApp.Reports.en_EmployeesReports.Bullet();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "HrContext";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.MetaSerializable = "<Meta X=\"20\" Y=\"20\" Width=\"100\" Height=\"607\" />";
            storedProcQuery1.Name = "SP_ContractsReport";
            queryParameter1.Name = "@CompanyId";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("[Parameters.CompanyId]", typeof(int));
            queryParameter2.Name = "@Culture";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("[Parameters.Culture]", typeof(string));
            queryParameter3.Name = "@EmpIds";
            queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter3.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingEmpIds]", typeof(string));
            queryParameter4.Name = "@DeptIds";
            queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter4.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingDeptIds]", typeof(string));
            queryParameter5.Name = "@JobIds";
            queryParameter5.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter5.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingJobIds]", typeof(string));
            queryParameter6.Name = "@Gender";
            queryParameter6.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter6.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingGender]", typeof(string));
            queryParameter7.Name = "@ContractStatus";
            queryParameter7.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter7.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingContractStatus]", typeof(string));
            queryParameter8.Name = "@ContractType";
            queryParameter8.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter8.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingContractType]", typeof(string));
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.Parameters.Add(queryParameter5);
            storedProcQuery1.Parameters.Add(queryParameter6);
            storedProcQuery1.Parameters.Add(queryParameter7);
            storedProcQuery1.Parameters.Add(queryParameter8);
            storedProcQuery1.StoredProcName = "SP_ContractsReport";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.xrTable2});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 63.5F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("EmpCode", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("EmpName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("AssignStatus", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ContractType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("AssignDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("EmpStartDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("EmpEndDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("PositionName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Salary", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("DeptName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("EmpName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("AssignStatus", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("ContractType", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("AssignDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("EmpEndDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("EmpStartDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("PositionName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("Salary", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("JobName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("JobName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable4
            // 
            this.xrTable4.BorderColor = System.Drawing.Color.Silver;
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.Dpi = 254F;
            this.xrTable4.EvenStyleName = "tbl_odd_detail";
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(24.99998F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.OddStyleName = "tbl_even_detail";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTable4.SizeF = new System.Drawing.SizeF(2912.74F, 63.5F);
            this.xrTable4.StylePriority.UseBorderColor = false;
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell33,
            this.xrTableCell36,
            this.xrTableCell34,
            this.xrTableCell35,
            this.xrTableCell48,
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell39,
            this.xrTableCell40,
            this.xrTableCell41,
            this.xrTableCell42,
            this.xrTableCell43});
            this.xrTableRow4.Dpi = 254F;
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 0.5679012345679012D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.EmpCode")});
            this.xrTableCell33.Dpi = 254F;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.StylePriority.UseBackColor = false;
            this.xrTableCell33.StylePriority.UseTextAlignment = false;
            this.xrTableCell33.Text = "xrTableCell17";
            this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell33.Weight = 0.69596963797464273D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell36.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell36.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.bullet2});
            this.xrTableCell36.Dpi = 254F;
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.StylePriority.UseBackColor = false;
            this.xrTableCell36.StylePriority.UseBorders = false;
            this.xrTableCell36.StylePriority.UseTextAlignment = false;
            this.xrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell36.Weight = 0.33125233373518614D;
            // 
            // formattingRule6
            // 
            this.formattingRule6.Condition = "[EmpEndDate]= Today()";
            this.formattingRule6.DataMember = "SP_ContractsReport";
            this.formattingRule6.Formatting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.formattingRule6.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.formattingRule6.Name = "formattingRule6";
            // 
            // Invisible
            // 
            this.Invisible.Condition = "[DataSource.RowCount]==0";
            this.Invisible.DataMember = "SP_ContractsReport";
            this.Invisible.Formatting.Visible = DevExpress.Utils.DefaultBoolean.False;
            this.Invisible.Name = "Invisible";
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell34.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.EmpName")});
            this.xrTableCell34.Dpi = 254F;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.StylePriority.UseBackColor = false;
            this.xrTableCell34.StylePriority.UseBorders = false;
            this.xrTableCell34.StylePriority.UseTextAlignment = false;
            this.xrTableCell34.Text = "xrTableCell18";
            this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell34.Weight = 1.9365236114982227D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.JobName")});
            this.xrTableCell35.Dpi = 254F;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.StylePriority.UseBackColor = false;
            this.xrTableCell35.StylePriority.UseTextAlignment = false;
            this.xrTableCell35.Text = "xrTableCell19";
            this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell35.Weight = 1.5018369103098168D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.BackColor = System.Drawing.Color.White;
            this.xrTableCell48.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell48.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell48.Dpi = 254F;
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.StylePriority.UseBackColor = false;
            this.xrTableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell48.Weight = 0.029168692607500324D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.PositionName")});
            this.xrTableCell37.Dpi = 254F;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.StylePriority.UseTextAlignment = false;
            this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell37.Weight = 1.73976292413562D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.ContractType")});
            this.xrTableCell38.Dpi = 254F;
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.StylePriority.UseTextAlignment = false;
            this.xrTableCell38.Text = "xrTableCell25";
            this.xrTableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell38.Weight = 1.3879415361690162D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.EmpStartDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell39.Dpi = 254F;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.StylePriority.UseTextAlignment = false;
            this.xrTableCell39.Text = "xrTableCell26";
            this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell39.Weight = 1.4670241664693009D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.EmpEndDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell40.Dpi = 254F;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.StylePriority.UseTextAlignment = false;
            this.xrTableCell40.Text = "xrTableCell29";
            this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell40.Weight = 1.4126156683382556D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.Salary")});
            this.xrTableCell41.Dpi = 254F;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.StylePriority.UseTextAlignment = false;
            this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell41.Weight = 1.3926369937317087D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.AssignStatus")});
            this.xrTableCell42.Dpi = 254F;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StylePriority.UseTextAlignment = false;
            this.xrTableCell42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell42.Weight = 2.2209280672909069D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.AssignDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell43.Dpi = 254F;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.StylePriority.UseTextAlignment = false;
            this.xrTableCell43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell43.Weight = 2.0353929965167517D;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderColor = System.Drawing.Color.Silver;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Dpi = 254F;
            this.xrTable2.EvenStyleName = "tbl_odd_detail";
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(10.00002F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.OddStyleName = "tbl_even_detail";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(2942.222F, 63.5F);
            this.xrTable2.StylePriority.UseBorderColor = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17,
            this.xrTableCell44,
            this.xrTableCell18,
            this.xrTableCell19,
            this.xrTableCell21,
            this.xrTableCell47,
            this.xrTableCell23,
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell29,
            this.xrTableCell30,
            this.xrTableCell31,
            this.xrTableCell32});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 0.5679012345679012D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.EmpCode")});
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseBackColor = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = "xrTableCell17";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell17.Weight = 0.6394035098792521D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell44.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell44.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.bullet1});
            this.xrTableCell44.Dpi = 254F;
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.StylePriority.UseBackColor = false;
            this.xrTableCell44.StylePriority.UseBorders = false;
            this.xrTableCell44.StylePriority.UseTextAlignment = false;
            this.xrTableCell44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell44.Weight = 0.36421436435361765D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.EmpName")});
            this.xrTableCell18.Dpi = 254F;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseBackColor = false;
            this.xrTableCell18.StylePriority.UseBorders = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = "xrTableCell18";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell18.Weight = 1.9805805683058297D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.JobName")});
            this.xrTableCell19.Dpi = 254F;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseBackColor = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.Text = "xrTableCell19";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell19.Weight = 1.4636202881760334D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.DeptName")});
            this.xrTableCell21.Dpi = 254F;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseBackColor = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Text = "xrTableCell21";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell21.Weight = 1.8733565057350292D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.BackColor = System.Drawing.Color.White;
            this.xrTableCell47.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell47.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell47.Dpi = 254F;
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.StylePriority.UseBackColor = false;
            this.xrTableCell47.StylePriority.UseTextAlignment = false;
            this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell47.Weight = 0.028032479764186217D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.PositionName")});
            this.xrTableCell23.Dpi = 254F;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell23.Weight = 1.3590663404005892D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.ContractType")});
            this.xrTableCell25.Dpi = 254F;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.Text = "xrTableCell25";
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell25.Weight = 1.4670253366003827D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.EmpStartDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell26.Dpi = 254F;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.Text = "xrTableCell26";
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell26.Weight = 1.4126179979913307D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.EmpEndDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell29.Dpi = 254F;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseTextAlignment = false;
            this.xrTableCell29.Text = "xrTableCell29";
            this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell29.Weight = 1.3899940807556521D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.Salary")});
            this.xrTableCell30.Dpi = 254F;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.StylePriority.UseTextAlignment = false;
            this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell30.Weight = 1.2781985468593535D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.AssignStatus")});
            this.xrTableCell31.Dpi = 254F;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.StylePriority.UseTextAlignment = false;
            this.xrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell31.Weight = 1.560606296467923D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.AssignDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell32.Dpi = 254F;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.StylePriority.UseTextAlignment = false;
            this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell32.Weight = 1.4978202331175545D;
            // 
            // formattingRule1
            // 
            this.formattingRule1.Condition = "[Status]==0";
            this.formattingRule1.DataMember = "SP_ContractsReport";
            this.formattingRule1.Formatting.BackColor = System.Drawing.Color.White;
            this.formattingRule1.Formatting.BorderColor = System.Drawing.Color.White;
            this.formattingRule1.Formatting.BorderWidth = 10F;
            this.formattingRule1.Name = "formattingRule1";
            // 
            // formattingRule2
            // 
            this.formattingRule2.Condition = "[Status]==1";
            this.formattingRule2.DataMember = "SP_ContractsReport";
            this.formattingRule2.Formatting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.formattingRule2.Formatting.BorderColor = System.Drawing.Color.White;
            this.formattingRule2.Formatting.BorderWidth = 10F;
            this.formattingRule2.Formatting.ForeColor = System.Drawing.Color.Green;
            this.formattingRule2.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.formattingRule2.Name = "formattingRule2";
            // 
            // formattingRule3
            // 
            this.formattingRule3.Condition = "[Status]==2";
            this.formattingRule3.DataMember = "SP_ContractsReport";
            this.formattingRule3.Formatting.BackColor = System.Drawing.Color.Yellow;
            this.formattingRule3.Formatting.BorderColor = System.Drawing.Color.White;
            this.formattingRule3.Formatting.BorderWidth = 10F;
            this.formattingRule3.Formatting.ForeColor = System.Drawing.Color.Yellow;
            this.formattingRule3.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.formattingRule3.Name = "formattingRule3";
            // 
            // formattingRule4
            // 
            this.formattingRule4.Condition = "[Status]==3";
            this.formattingRule4.DataMember = "SP_ContractsReport";
            this.formattingRule4.Formatting.BackColor = System.Drawing.Color.Red;
            this.formattingRule4.Formatting.BorderWidth = 10F;
            this.formattingRule4.Name = "formattingRule4";
            // 
            // formattingRule5
            // 
            this.formattingRule5.Condition = "[Status]==12";
            this.formattingRule5.DataMember = "SP_ContractsReport";
            this.formattingRule5.Formatting.BackColor = System.Drawing.Color.Gray;
            this.formattingRule5.Formatting.BorderColor = System.Drawing.Color.White;
            this.formattingRule5.Formatting.BorderWidth = 10F;
            this.formattingRule5.Name = "formattingRule5";
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
            this.xrTable3,
            this.xrTable1});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 114.44F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.StyleName = "tbl_header_style";
            // 
            // xrTable3
            // 
            this.xrTable3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTable3.BorderColor = System.Drawing.Color.Silver;
            this.xrTable3.Dpi = 254F;
            this.xrTable3.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(25F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTable3.SizeF = new System.Drawing.SizeF(2912.74F, 114.44F);
            this.xrTable3.StylePriority.UseFont = false;
            this.xrTable3.StylePriority.UseTextAlignment = false;
            this.xrTable3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell7,
            this.xrTableCell9,
            this.xrTableCell46,
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell20,
            this.xrTableCell22,
            this.xrTableCell24,
            this.xrTableCell27,
            this.xrTableCell28});
            this.xrTableRow3.Dpi = 254F;
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 11.5D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Code";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 0.045517411214475306D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.InteractiveSorting.FieldName = "PositionName";
            this.xrTableCell7.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Employee Name";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell7.Weight = 0.14831584188165253D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.InteractiveSorting.FieldName = "JobName";
            this.xrTableCell9.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Job";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.098252615731765663D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell46.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell46.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell46.Dpi = 254F;
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.StylePriority.UseBorderColor = false;
            this.xrTableCell46.StylePriority.UseTextAlignment = false;
            this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell46.Weight = 0.0016405886905691211D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.InteractiveSorting.FieldName = "PositionName";
            this.xrTableCell15.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "Position";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell15.Weight = 0.11401972622162801D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.InteractiveSorting.FieldName = "ContractType";
            this.xrTableCell16.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.Text = "Contract Type";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell16.Weight = 0.090773499728962481D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Dpi = 254F;
            this.xrTableCell20.InteractiveSorting.FieldName = "EmpStartDate";
            this.xrTableCell20.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Text = "Start Date";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell20.Weight = 0.095945595020021285D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Dpi = 254F;
            this.xrTableCell22.InteractiveSorting.FieldName = "EmpEndDate";
            this.xrTableCell22.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "End Date";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell22.Weight = 0.092387121880215153D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Dpi = 254F;
            this.xrTableCell24.InteractiveSorting.FieldName = "Salary";
            this.xrTableCell24.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.Text = "Salary";
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell24.Weight = 0.091080343709771333D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Dpi = 254F;
            this.xrTableCell27.InteractiveSorting.FieldName = "AssignStatus";
            this.xrTableCell27.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.Text = "Assignment Status";
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell27.Weight = 0.14525209823861746D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Dpi = 254F;
            this.xrTableCell28.InteractiveSorting.FieldName = "AssignDate";
            this.xrTableCell28.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.Text = "Assignment Date";
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell28.Weight = 0.13311753250004152D;
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTable1.BorderColor = System.Drawing.Color.Silver;
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.KeepTogether = true;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(2942.222F, 114.44F);
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell14,
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell1,
            this.xrTableCell45,
            this.xrTableCell6,
            this.xrTableCell8,
            this.xrTableCell10,
            this.xrTableCell2,
            this.xrTableCell12,
            this.xrTableCell11,
            this.xrTableCell13});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 11.5D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "Code";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell14.Weight = 0.0419536251262804D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.InteractiveSorting.FieldName = "EmpName";
            this.xrTableCell4.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Employee Name";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.15385069641118904D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.InteractiveSorting.FieldName = "JobName";
            this.xrTableCell5.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "Job";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell5.Weight = 0.0960335825613147D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.InteractiveSorting.FieldName = "DeptName";
            this.xrTableCell1.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Department";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.12286238302515209D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell45.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell45.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell45.Dpi = 254F;
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.StylePriority.UseBorderColor = false;
            this.xrTableCell45.StylePriority.UseTextAlignment = false;
            this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell45.Weight = 0.0018947878948211913D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.InteractiveSorting.FieldName = "PositionName";
            this.xrTableCell6.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Position";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell6.Weight = 0.089173342240995518D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.InteractiveSorting.FieldName = "ContractType";
            this.xrTableCell8.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "Contract Type";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell8.Weight = 0.096257030004671437D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.InteractiveSorting.FieldName = "EmpStartDate";
            this.xrTableCell10.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Start Date";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 0.092686981623613246D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.InteractiveSorting.FieldName = "EmpEndDate";
            this.xrTableCell2.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "End Date";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.091202811884803073D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.InteractiveSorting.FieldName = "Salary";
            this.xrTableCell12.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell12.KeepTogether = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "Salary";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell12.Weight = 0.0838674404835455D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.InteractiveSorting.FieldName = "AssignStatus";
            this.xrTableCell11.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "Assignment Status";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 0.10239716920472983D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.InteractiveSorting.FieldName = "AssignDate";
            this.xrTableCell13.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "Assignment Date";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell13.Weight = 0.098277488464678972D;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel2,
            this.companyLogo});
            this.ReportHeader.Dpi = 254F;
            this.ReportHeader.HeightF = 346.9162F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel5
            // 
            this.xrLabel5.AutoWidth = true;
            this.xrLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.xrLabel5.CanShrink = true;
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.sqlDataSource5, "sp_ReportTitle.Column1")});
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(48.22396F, 228.5516F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
            this.xrLabel5.SizeF = new System.Drawing.SizeF(2855.243F, 93.36461F);
            this.xrLabel5.StylePriority.UseBackColor = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseForeColor = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // sqlDataSource5
            // 
            this.sqlDataSource5.ConnectionName = "HrContext";
            this.sqlDataSource5.Name = "sqlDataSource5";
            storedProcQuery2.Name = "sp_ReportTitle";
            queryParameter9.Name = "@EmpIds";
            queryParameter9.Type = typeof(string);
            queryParameter10.Name = "@JobIds";
            queryParameter10.Type = typeof(string);
            queryParameter11.Name = "@deptIds";
            queryParameter11.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter11.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingDeptIds]", typeof(string));
            queryParameter12.Name = "@culture";
            queryParameter12.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter12.Value = new DevExpress.DataAccess.Expression("[Parameters.Culture]", typeof(string));
            queryParameter13.Name = "@NationIds";
            queryParameter13.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter13.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingContractStatus]", typeof(string));
            queryParameter14.Name = "@ContractsIds";
            queryParameter14.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter14.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingContractType]", typeof(string));
            storedProcQuery2.Parameters.Add(queryParameter9);
            storedProcQuery2.Parameters.Add(queryParameter10);
            storedProcQuery2.Parameters.Add(queryParameter11);
            storedProcQuery2.Parameters.Add(queryParameter12);
            storedProcQuery2.Parameters.Add(queryParameter13);
            storedProcQuery2.Parameters.Add(queryParameter14);
            storedProcQuery2.StoredProcName = "sp_ReportTitle";
            this.sqlDataSource5.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery2});
            this.sqlDataSource5.ResultSchemaSerializable = "PERhdGFTZXQgTmFtZT0ic3FsRGF0YVNvdXJjZTUiPjxWaWV3IE5hbWU9InNwX1JlcG9ydFRpdGxlIj48R" +
    "mllbGQgTmFtZT0iQ29sdW1uMSIgVHlwZT0iU3RyaW5nIiAvPjwvVmlldz48L0RhdGFTZXQ+";
            // 
            // xrLabel4
            // 
            this.xrLabel4.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(706.2429F, 38.99767F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(1178.278F, 161.0023F);
            this.xrLabel4.StylePriority.UseBorderColor = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseForeColor = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Contract Employees Report";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.CompanyName, "Text", "")});
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(2065.389F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(897.7432F, 96.95645F);
            this.xrLabel2.StylePriority.UseBorderColor = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseForeColor = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // CompanyName
            // 
            this.CompanyName.Description = "CompanyName";
            this.CompanyName.Name = "CompanyName";
            this.CompanyName.Visible = false;
            // 
            // companyLogo
            // 
            this.companyLogo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", null, "GetCompanyLogo.file_stream")});
            this.companyLogo.Dpi = 254F;
            this.companyLogo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.companyLogo.Name = "companyLogo";
            this.companyLogo.SizeF = new System.Drawing.SizeF(400F, 200F);
            this.companyLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel7,
            this.xrLabel6,
            this.bullet3,
            this.bullet4,
            this.xrLabel9,
            this.xrPageInfo2,
            this.xrPageInfo1,
            this.xrLabel16,
            this.xrPageBreak1});
            this.PageFooter.Dpi = 254F;
            this.PageFooter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.PageFooter.HeightF = 78.29507F;
            this.PageFooter.Name = "PageFooter";
            this.PageFooter.StylePriority.UseForeColor = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Dpi = 254F;
            this.xrLabel7.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.ForeColor = System.Drawing.Color.Black;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(1316.301F, 9.763831F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(541.9323F, 58.42F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseForeColor = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Today is the end of the contract";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Dpi = 254F;
            this.xrLabel6.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.ForeColor = System.Drawing.Color.Black;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(2112.33F, 7.763968F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(313.0586F, 58.41999F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseForeColor = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Contract in date";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.ForeColor = System.Drawing.Color.Black;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(739.3311F, 9.763889F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(137.5833F, 58.41999F);
            this.xrLabel9.StylePriority.UseForeColor = false;
            this.xrLabel9.Text = "User:";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Dpi = 254F;
            this.xrPageInfo2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.ForeColor = System.Drawing.Color.Black;
            this.xrPageInfo2.Format = "{0:yyyy-MM-dd h:mm tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 3.763875F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(664.6506F, 58.42F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseForeColor = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Dpi = 254F;
            this.xrPageInfo1.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.ForeColor = System.Drawing.Color.Black;
            this.xrPageInfo1.Format = "Page{0} Of {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(2685.09F, 3.763875F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseForeColor = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel16
            // 
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.User, "Text", "")});
            this.xrLabel16.Dpi = 254F;
            this.xrLabel16.ForeColor = System.Drawing.Color.Black;
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(839.8726F, 9.763889F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            this.xrLabel16.StylePriority.UseForeColor = false;
            // 
            // User
            // 
            this.User.Description = "User";
            this.User.Name = "User";
            this.User.Visible = false;
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.Dpi = 254F;
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1.763889F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel1,
            this.xrLabel17,
            this.xrLabel18});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.Expanded = false;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("DeptName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 67.02778F;
            this.GroupHeader1.KeepTogether = true;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.Silver;
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.DeptName")});
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 7F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(404.8208F, 58.41999F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_ContractsReport.EmpName")});
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(724.3311F, 8.607786F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(54.68061F, 58.41999F);
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0}    ";
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Count;
            xrSummary1.IgnoreNullValues = true;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel1.Summary = xrSummary1;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Dpi = 254F;
            this.xrLabel17.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(691.2429F, 8.607786F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(59.97227F, 58.41999F);
            this.xrLabel17.StylePriority.UseForeColor = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = ":";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel18
            // 
            this.xrLabel18.Dpi = 254F;
            this.xrLabel18.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(414.8208F, 8.607786F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(284.782F, 58.41999F);
            this.xrLabel18.StylePriority.UseForeColor = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "Employees Number";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
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
            this.GroupCaption3.BorderColor = System.Drawing.Color.White;
            this.GroupCaption3.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
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
            this.Title.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.Title.Name = "Title";
            // 
            // odd
            // 
            this.odd.BackColor = System.Drawing.Color.Silver;
            this.odd.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.odd.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.odd.Name = "odd";
            this.odd.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
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
            this.tbl_header_style.BackColor = System.Drawing.Color.Silver;
            this.tbl_header_style.BorderColor = System.Drawing.Color.Black;
            this.tbl_header_style.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.tbl_header_style.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_header_style.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_header_style.ForeColor = System.Drawing.Color.Black;
            this.tbl_header_style.Name = "tbl_header_style";
            this.tbl_header_style.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 254F);
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
            this.tbl_odd_detail.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_odd_detail.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_odd_detail.ForeColor = System.Drawing.Color.Black;
            this.tbl_odd_detail.Name = "tbl_odd_detail";
            this.tbl_odd_detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 3, 3, 254F);
            this.tbl_odd_detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // ev
            // 
            this.ev.BackColor = System.Drawing.Color.White;
            this.ev.Name = "ev";
            this.ev.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            // 
            // CompanyId
            // 
            this.CompanyId.Description = "CompanyId";
            this.CompanyId.Name = "CompanyId";
            this.CompanyId.Type = typeof(int);
            this.CompanyId.ValueInfo = "0";
            this.CompanyId.Visible = false;
            // 
            // Culture
            // 
            this.Culture.Description = "Culture";
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(null, "ar-EG"));
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(null, "en-GB"));
            this.Culture.LookUpSettings = staticListLookUpSettings1;
            this.Culture.Name = "Culture";
            this.Culture.ValueInfo = "en_GB";
            this.Culture.Visible = false;
            // 
            // EmpIds
            // 
            this.EmpIds.Description = "Employee Name";
            dynamicListLookUpSettings1.DataAdapter = null;
            dynamicListLookUpSettings1.DataMember = "SP_ContractsReport";
            dynamicListLookUpSettings1.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings1.DisplayMember = "EmpName";
            dynamicListLookUpSettings1.ValueMember = "Id";
            this.EmpIds.LookUpSettings = dynamicListLookUpSettings1;
            this.EmpIds.MultiValue = true;
            this.EmpIds.Name = "EmpIds";
            this.EmpIds.Type = typeof(int);
            // 
            // Gender
            // 
            this.Gender.Description = "Gender";
            staticListLookUpSettings2.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(1, "Male"));
            staticListLookUpSettings2.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(2, "Female"));
            this.Gender.LookUpSettings = staticListLookUpSettings2;
            this.Gender.MultiValue = true;
            this.Gender.Name = "Gender";
            this.Gender.Type = typeof(int);
            // 
            // DeptIds
            // 
            this.DeptIds.Description = "Department";
            dynamicListLookUpSettings2.DataAdapter = null;
            dynamicListLookUpSettings2.DataMember = "SP_ContractsReport";
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
            this.JobIds.Description = "Job";
            dynamicListLookUpSettings3.DataAdapter = null;
            dynamicListLookUpSettings3.DataMember = "SP_ContractsReport";
            dynamicListLookUpSettings3.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings3.DisplayMember = "JobName";
            dynamicListLookUpSettings3.ValueMember = "JobId";
            this.JobIds.LookUpSettings = dynamicListLookUpSettings3;
            this.JobIds.MultiValue = true;
            this.JobIds.Name = "JobIds";
            this.JobIds.Type = typeof(int);
            // 
            // ContractStatus
            // 
            this.ContractStatus.Description = "Contract Status";
            staticListLookUpSettings3.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(0, "New"));
            staticListLookUpSettings3.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(1, "Renew"));
            this.ContractStatus.LookUpSettings = staticListLookUpSettings3;
            this.ContractStatus.MultiValue = true;
            this.ContractStatus.Name = "ContractStatus";
            this.ContractStatus.Type = typeof(int);
            // 
            // ContractType
            // 
            this.ContractType.Description = "Contract Type";
            dynamicListLookUpSettings4.DataAdapter = null;
            dynamicListLookUpSettings4.DataMember = "SP_ContractsReport";
            dynamicListLookUpSettings4.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings4.DisplayMember = "ContractType";
            dynamicListLookUpSettings4.ValueMember = "ContractType";
            this.ContractType.LookUpSettings = dynamicListLookUpSettings4;
            this.ContractType.MultiValue = true;
            this.ContractType.Name = "ContractType";
            // 
            // ConEmpStartDate
            // 
            this.ConEmpStartDate.Description = "Employment End Date From";
            this.ConEmpStartDate.Name = "ConEmpStartDate";
            this.ConEmpStartDate.Type = typeof(System.DateTime);
            // 
            // ConEmpEndDate
            // 
            this.ConEmpEndDate.Description = "Employee End Date";
            this.ConEmpEndDate.Name = "ConEmpEndDate";
            this.ConEmpEndDate.Type = typeof(System.DateTime);
            // 
            // MappingDeptIds
            // 
            this.MappingDeptIds.Description = "MappingDeptIds";
            this.MappingDeptIds.Name = "MappingDeptIds";
            this.MappingDeptIds.Visible = false;
            // 
            // MappingEmpIds
            // 
            this.MappingEmpIds.Description = "MappingEmpIds";
            this.MappingEmpIds.Name = "MappingEmpIds";
            this.MappingEmpIds.Visible = false;
            // 
            // MappingJobIds
            // 
            this.MappingJobIds.Description = "MappingJobIds";
            this.MappingJobIds.Name = "MappingJobIds";
            this.MappingJobIds.Visible = false;
            // 
            // MappingContractType
            // 
            this.MappingContractType.Description = "MappingContractType";
            this.MappingContractType.Name = "MappingContractType";
            this.MappingContractType.Visible = false;
            // 
            // MappingContractStatus
            // 
            this.MappingContractStatus.Description = "MappingContractStatus";
            this.MappingContractStatus.Name = "MappingContractStatus";
            this.MappingContractStatus.Visible = false;
            // 
            // mappingGender
            // 
            this.mappingGender.Description = "mappingGender";
            this.mappingGender.Name = "mappingGender";
            this.mappingGender.Visible = false;
            // 
            // GroupingBy
            // 
            this.GroupingBy.Description = "Grouping By";
            staticListLookUpSettings4.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("Default", "Default"));
            staticListLookUpSettings4.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("Dept", "Department"));
            this.GroupingBy.LookUpSettings = staticListLookUpSettings4;
            this.GroupingBy.Name = "GroupingBy";
            this.GroupingBy.ValueInfo = "Default";
            // 
            // bullet2
            // 
            this.bullet2.BackColor = System.Drawing.Color.Transparent;
            this.bullet2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.bullet2.BulletBackColor = WebApp.Reports.en_EmployeesReports.BulletColor.green;
            this.bullet2.Dpi = 254F;
            this.bullet2.EvenStyleName = "tbl_odd_detail";
            this.bullet2.ForeColor = System.Drawing.Color.Green;
            this.bullet2.FormattingRules.Add(this.formattingRule6);
            this.bullet2.FormattingRules.Add(this.Invisible);
            this.bullet2.LocationFloat = new DevExpress.Utils.PointFloat(7.520744F, 15.75F);
            this.bullet2.Name = "bullet2";
            this.bullet2.OddStyleName = "tbl_even_detail";
            this.bullet2.SizeF = new System.Drawing.SizeF(31.37183F, 31.75F);
            this.bullet2.StylePriority.UseBackColor = false;
            this.bullet2.StylePriority.UseBorders = false;
            this.bullet2.StylePriority.UseForeColor = false;
            // 
            // bullet1
            // 
            this.bullet1.BackColor = System.Drawing.Color.Transparent;
            this.bullet1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.bullet1.BulletBackColor = WebApp.Reports.en_EmployeesReports.BulletColor.green;
            this.bullet1.Dpi = 254F;
            this.bullet1.EvenStyleName = "tbl_odd_detail";
            this.bullet1.ForeColor = System.Drawing.Color.Green;
            this.bullet1.FormattingRules.Add(this.formattingRule6);
            this.bullet1.FormattingRules.Add(this.Invisible);
            this.bullet1.LocationFloat = new DevExpress.Utils.PointFloat(8.701245F, 16.99998F);
            this.bullet1.Name = "bullet1";
            this.bullet1.OddStyleName = "tbl_even_detail";
            this.bullet1.SizeF = new System.Drawing.SizeF(31.37183F, 31.75F);
            this.bullet1.StylePriority.UseBackColor = false;
            this.bullet1.StylePriority.UseBorders = false;
            this.bullet1.StylePriority.UseForeColor = false;
            // 
            // bullet3
            // 
            this.bullet3.BackColor = System.Drawing.Color.Transparent;
            this.bullet3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.bullet3.BulletBackColor = WebApp.Reports.en_EmployeesReports.BulletColor.green;
            this.bullet3.Dpi = 254F;
            this.bullet3.EvenStyleName = "tbl_odd_detail";
            this.bullet3.ForeColor = System.Drawing.Color.Green;
            this.bullet3.LocationFloat = new DevExpress.Utils.PointFloat(2080.959F, 24.99998F);
            this.bullet3.Name = "bullet3";
            this.bullet3.OddStyleName = "tbl_even_detail";
            this.bullet3.SizeF = new System.Drawing.SizeF(31.37158F, 31.75F);
            this.bullet3.StylePriority.UseBackColor = false;
            this.bullet3.StylePriority.UseBorders = false;
            this.bullet3.StylePriority.UseForeColor = false;
            // 
            // bullet4
            // 
            this.bullet4.BackColor = System.Drawing.Color.Transparent;
            this.bullet4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.bullet4.BulletBackColor = WebApp.Reports.en_EmployeesReports.BulletColor.green;
            this.bullet4.Dpi = 254F;
            this.bullet4.EvenStyleName = "tbl_odd_detail";
            this.bullet4.ForeColor = System.Drawing.Color.Red;
            this.bullet4.LocationFloat = new DevExpress.Utils.PointFloat(1284.929F, 24.99998F);
            this.bullet4.Name = "bullet4";
            this.bullet4.OddStyleName = "tbl_even_detail";
            this.bullet4.SizeF = new System.Drawing.SizeF(31.37158F, 31.75F);
            this.bullet4.StylePriority.UseBackColor = false;
            this.bullet4.StylePriority.UseBorders = false;
            this.bullet4.StylePriority.UseForeColor = false;
            // 
            // en_ContractEmployeesReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.ReportHeader,
            this.PageFooter,
            this.GroupHeader1});
            this.Bookmark = "الهيكل التوظيفى";
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource5,
            this.sqlDataSource1});
            this.DataMember = "SP_ContractsReport";
            this.DataSource = this.sqlDataSource1;
            this.Dpi = 254F;
            this.FilterString = "[EmpEndDate] >= ?ConEmpStartDate And [EmpEndDate] <= ?ConEmpEndDate";
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1,
            this.formattingRule2,
            this.formattingRule3,
            this.formattingRule4,
            this.formattingRule5,
            this.formattingRule6,
            this.Invisible});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(3, 3, 0, 0);
            this.PageHeight = 2100;
            this.PageWidth = 2970;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.CompanyId,
            this.User,
            this.CompanyName,
            this.Culture,
            this.EmpIds,
            this.Gender,
            this.DeptIds,
            this.JobIds,
            this.ContractStatus,
            this.ContractType,
            this.ConEmpStartDate,
            this.ConEmpEndDate,
            this.MappingDeptIds,
            this.MappingEmpIds,
            this.MappingJobIds,
            this.MappingContractType,
            this.MappingContractStatus,
            this.mappingGender,
            this.GroupingBy});
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.SnapGridSize = 25F;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Odding,
            this.GroupCaption3,
            this.xrControlStyle1,
            this.Title,
            this.odd,
            this.xrControlStyle2,
            this.tbl_header_style,
            this.tbl_even_detail,
            this.tbl_odd_detail,
            this.ev});
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

  
}
