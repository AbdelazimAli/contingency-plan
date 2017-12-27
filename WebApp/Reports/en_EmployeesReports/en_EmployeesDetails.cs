using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using WebApp.Reports.EmployeesReports;
using WebApp.Models;
using Interface.Core;
using Model.Domain;
using System.Linq;
using static WebApp.Controllers.BaseReportController;


/// <summary>
/// Summary description for EmployeesDetails
/// </summary>
public class en_EmployeesDetails : DevExpress.XtraReports.UI.XtraReport, IBasicInfoReport
{
    private DetailBand Detail;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell47;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell49;
    private XRTableCell xrTableCell50;
    private XRTableCell xrTableCell51;
    private XRTableCell xrTableCell52;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell54;
    private XRTableCell xrTableCell55;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell58;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell60;
    private XRTableCell xrTableCell61;
    private XRTableCell xrTableCell62;
    private XRTableCell xrTableCell63;
    private XRTableCell xrTableCell64;
    private XRTableCell xrTableCell65;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell67;
    private XRTableCell xrTableCell68;
    private XRTableCell xrTableCell69;
    private XRTableCell xrTableCell70;
    private XRTableCell xrTableCell71;
    private XRTableCell xrTableCell72;
    private XRTableCell xrTableCell73;
    private XRTableCell xrTableCell74;
    private XRTableCell xrTableCell75;
    private XRTableCell xrTableCell76;
    private XRTableCell xrTableCell77;
    private XRTableCell xrTableCell78;
    private XRTableCell xrTableCell79;
    private XRTableCell xrTableCell80;
    private XRTableCell xrTableCell81;
    private XRTableCell xrTableCell82;
    private XRTableCell xrTableCell94;
    private XRTableCell xrTableCell83;
    private XRTableCell xrTableCell84;
    private XRTableCell xrTableCell85;
    private XRTableCell xrTableCell86;
    private XRTableCell xrTableCell87;
    private XRTableCell xrTableCell88;
    private XRTableCell xrTableCell89;
    private XRTableCell xrTableCell90;
    private XRTableCell xrTableCell91;
    private XRTableCell xrTableCell95;
    private WebApp.Reports.en_EmployeesReports.Bullet bullet1;
    private FormattingRule Active;
    private FormattingRule NotEmployment;
    private FormattingRule Suspend;
    private FormattingRule Terminated;
    private FormattingRule Invisible;
    private XRTableCell xrTableCell92;
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private PageHeaderBand PageHeader;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell93;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell30;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell44;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell2;
    private ReportHeaderBand ReportHeader;
    private DevExpress.XtraReports.Parameters.Parameter AssignEndDateEmp;
    private DevExpress.XtraReports.Parameters.Parameter EmpEndDateDetails;
    private XRLabel xrLabel2;
    private DevExpress.XtraReports.Parameters.Parameter CompanyName;
    private XRPictureBox companyLogo;
    private PageFooterBand PageFooter;
    private DevExpress.XtraReports.Parameters.Parameter User;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private FormattingRule formattingRule2;
    private XRControlStyle Odding;
    private XRControlStyle GroupCaption3;
    private XRControlStyle xrControlStyle1;
    private XRControlStyle Title;
    private XRControlStyle xrControlStyle4;
    private XRControlStyle xrControlStyle2;
    private XRControlStyle tbl_header_style;
    private XRControlStyle tbl_even_detail;
    private XRControlStyle tbl_odd_detail;
    private CalculatedField التأمين;
    private DevExpress.XtraReports.Parameters.Parameter Gender;
    private DevExpress.XtraReports.Parameters.Parameter FromAge;
    private DevExpress.XtraReports.Parameters.Parameter ToAge;
    private DevExpress.XtraReports.Parameters.Parameter WorkingPeriodFrom;
    private DevExpress.XtraReports.Parameters.Parameter WorkingPeriodTo;
    private DevExpress.XtraReports.Parameters.Parameter insured;
    private DevExpress.XtraReports.Parameters.Parameter Nationality;
    private DevExpress.XtraReports.Parameters.Parameter deptName;
    private DevExpress.XtraReports.Parameters.Parameter Job;
    private DevExpress.XtraReports.Parameters.Parameter Culture;
    private DevExpress.XtraReports.Parameters.Parameter CompanyId;
    private DevExpress.XtraReports.Parameters.Parameter mappingGender;
    private DevExpress.XtraReports.Parameters.Parameter mappingContractType;
    private DevExpress.XtraReports.Parameters.Parameter mappingJob;
    private DevExpress.XtraReports.Parameters.Parameter mappingNationality;
    private DevExpress.XtraReports.Parameters.Parameter mappingDeptName;
    private DevExpress.XtraReports.Parameters.Parameter ContractType;
    private DevExpress.XtraReports.Parameters.Parameter MappingInsured;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource2;
    private XRLabel xrLabel8;
    private XRLabel xrLabel5;
    private XRLabel xrLabel1;
    private XRLabel xrLabel10;
    private XRLabel xrLabel11;
    private XRLabel xrLabel4;
    private XRPageInfo xrPageInfo2;
    private WebApp.Reports.en_EmployeesReports.Bullet bullet4;
    private WebApp.Reports.en_EmployeesReports.Bullet bullet3;
    private XRLabel xrLabel6;
    private XRLabel xrLabel7;
    private WebApp.Reports.en_EmployeesReports.Bullet bullet2;
    private XRLabel xrLabel3;
    private XRLabel xrLabel12;
    private WebApp.Reports.en_EmployeesReports.Bullet bullet5;
    private XRLabel xrLabel9;
    private XRLabel xrLabel16;
    private XRPageInfo xrPageInfo1;

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
    public en_EmployeesDetails()
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
            DevExpress.DataAccess.Sql.QueryParameter queryParameter9 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter10 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter11 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter12 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter13 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter14 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(en_EmployeesDetails));
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings1 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings2 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings1 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings2 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings3 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings3 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings4 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery2 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter15 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter16 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter17 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter18 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter19 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter20 = new DevExpress.DataAccess.Sql.QueryParameter();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell68 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell77 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell81 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell94 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell86 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell89 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell95 = new DevExpress.XtraReports.UI.XRTableCell();
            this.bullet1 = new WebApp.Reports.en_EmployeesReports.Bullet();
            this.Active = new DevExpress.XtraReports.UI.FormattingRule();
            this.NotEmployment = new DevExpress.XtraReports.UI.FormattingRule();
            this.Suspend = new DevExpress.XtraReports.UI.FormattingRule();
            this.Terminated = new DevExpress.XtraReports.UI.FormattingRule();
            this.Invisible = new DevExpress.XtraReports.UI.FormattingRule();
            this.xrTableCell92 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell93 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.EmpEndDateDetails = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.AssignEndDateEmp = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.CompanyName = new DevExpress.XtraReports.Parameters.Parameter();
            this.companyLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.User = new DevExpress.XtraReports.Parameters.Parameter();
            this.bullet4 = new WebApp.Reports.en_EmployeesReports.Bullet();
            this.bullet3 = new WebApp.Reports.en_EmployeesReports.Bullet();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.bullet2 = new WebApp.Reports.en_EmployeesReports.Bullet();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.bullet5 = new WebApp.Reports.en_EmployeesReports.Bullet();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.formattingRule2 = new DevExpress.XtraReports.UI.FormattingRule();
            this.Odding = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupCaption3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle4 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_header_style = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_even_detail = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_odd_detail = new DevExpress.XtraReports.UI.XRControlStyle();
            this.التأمين = new DevExpress.XtraReports.UI.CalculatedField();
            this.Gender = new DevExpress.XtraReports.Parameters.Parameter();
            this.FromAge = new DevExpress.XtraReports.Parameters.Parameter();
            this.ToAge = new DevExpress.XtraReports.Parameters.Parameter();
            this.WorkingPeriodFrom = new DevExpress.XtraReports.Parameters.Parameter();
            this.WorkingPeriodTo = new DevExpress.XtraReports.Parameters.Parameter();
            this.insured = new DevExpress.XtraReports.Parameters.Parameter();
            this.Nationality = new DevExpress.XtraReports.Parameters.Parameter();
            this.deptName = new DevExpress.XtraReports.Parameters.Parameter();
            this.Job = new DevExpress.XtraReports.Parameters.Parameter();
            this.Culture = new DevExpress.XtraReports.Parameters.Parameter();
            this.CompanyId = new DevExpress.XtraReports.Parameters.Parameter();
            this.mappingGender = new DevExpress.XtraReports.Parameters.Parameter();
            this.mappingContractType = new DevExpress.XtraReports.Parameters.Parameter();
            this.mappingJob = new DevExpress.XtraReports.Parameters.Parameter();
            this.mappingNationality = new DevExpress.XtraReports.Parameters.Parameter();
            this.mappingDeptName = new DevExpress.XtraReports.Parameters.Parameter();
            this.ContractType = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingInsured = new DevExpress.XtraReports.Parameters.Parameter();
            this.sqlDataSource2 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "HrContext";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "EmployeeDataDetailsReport";
            queryParameter1.Name = "@ContractType";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingContractType]", typeof(string));
            queryParameter2.Name = "@Gender";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingGender]", typeof(string));
            queryParameter3.Name = "@FromAge";
            queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter3.Value = new DevExpress.DataAccess.Expression("[Parameters.FromAge]", typeof(int));
            queryParameter4.Name = "@ToAge";
            queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter4.Value = new DevExpress.DataAccess.Expression("[Parameters.ToAge]", typeof(int));
            queryParameter5.Name = "@FromWorkAge";
            queryParameter5.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter5.Value = new DevExpress.DataAccess.Expression("[Parameters.WorkingPeriodFrom]", typeof(float));
            queryParameter6.Name = "@ToWorkAge";
            queryParameter6.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter6.Value = new DevExpress.DataAccess.Expression("[Parameters.WorkingPeriodTo]", typeof(float));
            queryParameter7.Name = "@Insured";
            queryParameter7.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter7.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingInsured]", typeof(string));
            queryParameter8.Name = "@EmploymentEDate";
            queryParameter8.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter8.Value = new DevExpress.DataAccess.Expression("[Parameters.EmpEndDateDetails]", typeof(System.DateTime));
            queryParameter9.Name = "@AssignEDate";
            queryParameter9.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter9.Value = new DevExpress.DataAccess.Expression("[Parameters.AssignEndDateEmp]", typeof(System.DateTime));
            queryParameter10.Name = "@Nationality";
            queryParameter10.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter10.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingNationality]", typeof(string));
            queryParameter11.Name = "@Job";
            queryParameter11.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter11.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingJob]", typeof(string));
            queryParameter12.Name = "@Culture";
            queryParameter12.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter12.Value = new DevExpress.DataAccess.Expression("[Parameters.Culture]", typeof(string));
            queryParameter13.Name = "@LoginCompanyId";
            queryParameter13.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter13.Value = new DevExpress.DataAccess.Expression("[Parameters.CompanyId]", typeof(int));
            queryParameter14.Name = "@DeptName";
            queryParameter14.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter14.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingDeptName]", typeof(string));
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.Parameters.Add(queryParameter5);
            storedProcQuery1.Parameters.Add(queryParameter6);
            storedProcQuery1.Parameters.Add(queryParameter7);
            storedProcQuery1.Parameters.Add(queryParameter8);
            storedProcQuery1.Parameters.Add(queryParameter9);
            storedProcQuery1.Parameters.Add(queryParameter10);
            storedProcQuery1.Parameters.Add(queryParameter11);
            storedProcQuery1.Parameters.Add(queryParameter12);
            storedProcQuery1.Parameters.Add(queryParameter13);
            storedProcQuery1.Parameters.Add(queryParameter14);
            storedProcQuery1.StoredProcName = "EmployeeDataDetailsReport";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("DeptName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("DeptName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("VarSubAmt", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("VarSubAmt", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("WorkingPeriod", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("WorkingPeriod", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("التأمين", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("التأمين", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("SubscripDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("SubscripDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("UnivName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("UnivName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("StartDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("StartDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("Religion", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Religion", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("QualName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("QualName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("QualGrade", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("QualGrade", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("Profession", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Profession", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("PositionName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("PositionName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("NationalityAr", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("NationalityAr", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("MilStatDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("MilStatDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("Militarystatus", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Militarystatus", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("MangerName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("MangerName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("LocationName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("LocationName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("JoinDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("JoinDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("JobName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("JobName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("IssuePlace", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("IssuePlace", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("IssueDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("IssueDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("GradYear", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("GradYear", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("Gender", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Gender", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("ExpiryDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ExpiryDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("EmpStatus", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("EmpStatus", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("EmpName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("EmpName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("ContractEndDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ContractEndDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("Age", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Age", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("BasicSubAmt", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("BasicSubAmt", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("ContractType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ContractType", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("AssignEndDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("AssignEndDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("AssignDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("AssignDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("AssignStatus", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("AssignStatus", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("MaritalStat", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("MaritalStat", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("CountryName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("CountryName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("CityName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("CityName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("District", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("District", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderColor = System.Drawing.Color.Silver;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.EvenStyleName = "tbl_odd_detail";
            this.xrTable2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(1.716614E-05F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.OddStyleName = "tbl_even_detail";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(4612F, 25F);
            this.xrTable2.StylePriority.UseBorderColor = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell47,
            this.xrTableCell48,
            this.xrTableCell49,
            this.xrTableCell50,
            this.xrTableCell51,
            this.xrTableCell52,
            this.xrTableCell53,
            this.xrTableCell54,
            this.xrTableCell55,
            this.xrTableCell56,
            this.xrTableCell57,
            this.xrTableCell58,
            this.xrTableCell59,
            this.xrTableCell60,
            this.xrTableCell61,
            this.xrTableCell62,
            this.xrTableCell63,
            this.xrTableCell64,
            this.xrTableCell65,
            this.xrTableCell66,
            this.xrTableCell67,
            this.xrTableCell68,
            this.xrTableCell69,
            this.xrTableCell70,
            this.xrTableCell71,
            this.xrTableCell72,
            this.xrTableCell73,
            this.xrTableCell74,
            this.xrTableCell75,
            this.xrTableCell76,
            this.xrTableCell77,
            this.xrTableCell78,
            this.xrTableCell79,
            this.xrTableCell80,
            this.xrTableCell81,
            this.xrTableCell82,
            this.xrTableCell94,
            this.xrTableCell83,
            this.xrTableCell84,
            this.xrTableCell85,
            this.xrTableCell86,
            this.xrTableCell87,
            this.xrTableCell88,
            this.xrTableCell89,
            this.xrTableCell90,
            this.xrTableCell91,
            this.xrTableCell95,
            this.xrTableCell92});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 11.5D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.IssuePlace")});
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.StylePriority.UseTextAlignment = false;
            this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell47.Weight = 4.4485782362143755D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.ExpiryDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.StylePriority.UseTextAlignment = false;
            this.xrTableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell48.Weight = 4.2199968532710059D;
            // 
            // xrTableCell49
            // 
            this.xrTableCell49.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.IssueDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell49.Name = "xrTableCell49";
            this.xrTableCell49.Weight = 3.8455524068276024D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.PassportNo")});
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.StylePriority.UseTextAlignment = false;
            this.xrTableCell50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell50.Weight = 3.9019469007017871D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.MilStatDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.StylePriority.UseTextAlignment = false;
            this.xrTableCell51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell51.Weight = 4.7866570083174649D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.MilitaryNo")});
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.Weight = 3.934036636450803D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.Militarystatus")});
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.Weight = 4.5142017741822915D;
            // 
            // xrTableCell54
            // 
            this.xrTableCell54.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.SubscripDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell54.Name = "xrTableCell54";
            this.xrTableCell54.StylePriority.UseTextAlignment = false;
            this.xrTableCell54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell54.Weight = 4.4513690389268668D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.Ssn")});
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.StylePriority.UseTextAlignment = false;
            this.xrTableCell55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell55.Weight = 4.818736735006758D;
            // 
            // xrTableCell56
            // 
            this.xrTableCell56.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.التأمين")});
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.StylePriority.UseTextAlignment = false;
            this.xrTableCell56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell56.Weight = 4.5988221616127722D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.VarSubAmt")});
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.StylePriority.UseTextAlignment = false;
            this.xrTableCell57.Text = "xrTableCell57";
            this.xrTableCell57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell57.Weight = 4.2017758777569165D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.BasicSubAmt")});
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.StylePriority.UseTextAlignment = false;
            this.xrTableCell58.Text = "xrTableCell58";
            this.xrTableCell58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell58.Weight = 4.2881019470200421D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.WorkingPeriod")});
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.StylePriority.UseTextAlignment = false;
            this.xrTableCell59.Text = "xrTableCell59";
            this.xrTableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell59.Weight = 4.146776784863988D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.ContractEndDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.StylePriority.UseTextAlignment = false;
            this.xrTableCell60.Text = "xrTableCell60";
            this.xrTableCell60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell60.Weight = 5.057803126468988D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.StartDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.StylePriority.UseTextAlignment = false;
            this.xrTableCell61.Text = "xrTableCell61";
            this.xrTableCell61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell61.Weight = 4.2723418196376279D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.ContractType")});
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.Text = "xrTableCell62";
            this.xrTableCell62.Weight = 4.1780713180750837D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.AssignEndDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.StylePriority.UseTextAlignment = false;
            this.xrTableCell63.Text = "xrTableCell63";
            this.xrTableCell63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell63.Weight = 4.983180754280907D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.AssignDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.StylePriority.UseTextAlignment = false;
            this.xrTableCell64.Text = "xrTableCell64";
            this.xrTableCell64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell64.Weight = 4.3863809044312845D;
            // 
            // xrTableCell65
            // 
            this.xrTableCell65.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.AssignStatus")});
            this.xrTableCell65.Name = "xrTableCell65";
            this.xrTableCell65.Text = "xrTableCell65";
            this.xrTableCell65.Weight = 4.4687561562241065D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.JoinDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.StylePriority.UseTextAlignment = false;
            this.xrTableCell66.Text = "xrTableCell66";
            this.xrTableCell66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell66.Weight = 3.791592187124742D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.GradYear")});
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.Weight = 4.4860521547449661D;
            // 
            // xrTableCell68
            // 
            this.xrTableCell68.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.QualGrade")});
            this.xrTableCell68.Name = "xrTableCell68";
            this.xrTableCell68.Weight = 3.5182023120766077D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.QualName")});
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.Weight = 5.0106145507796587D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.UnivName")});
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.Weight = 4.3901915958450237D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.OtherEmail")});
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.Weight = 7.0435396563013812D;
            // 
            // xrTableCell72
            // 
            this.xrTableCell72.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.WorkEmail")});
            this.xrTableCell72.Name = "xrTableCell72";
            this.xrTableCell72.Weight = 6.0055904989920963D;
            // 
            // xrTableCell73
            // 
            this.xrTableCell73.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.HomeTel")});
            this.xrTableCell73.Name = "xrTableCell73";
            this.xrTableCell73.StylePriority.UseTextAlignment = false;
            this.xrTableCell73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell73.Weight = 4.4669835116015157D;
            // 
            // xrTableCell74
            // 
            this.xrTableCell74.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.Mobile")});
            this.xrTableCell74.Name = "xrTableCell74";
            this.xrTableCell74.StylePriority.UseTextAlignment = false;
            this.xrTableCell74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell74.Weight = 4.507461886889117D;
            // 
            // xrTableCell75
            // 
            this.xrTableCell75.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.CountryName")});
            this.xrTableCell75.Name = "xrTableCell75";
            this.xrTableCell75.StylePriority.UseTextAlignment = false;
            this.xrTableCell75.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell75.Weight = 4.3720786477163891D;
            // 
            // xrTableCell76
            // 
            this.xrTableCell76.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.CityName")});
            this.xrTableCell76.Name = "xrTableCell76";
            this.xrTableCell76.StylePriority.UseTextAlignment = false;
            this.xrTableCell76.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell76.Weight = 4.6233296893509941D;
            // 
            // xrTableCell77
            // 
            this.xrTableCell77.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.District")});
            this.xrTableCell77.Name = "xrTableCell77";
            this.xrTableCell77.Weight = 3.6547288188471176D;
            // 
            // xrTableCell78
            // 
            this.xrTableCell78.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.EmpAddress")});
            this.xrTableCell78.Name = "xrTableCell78";
            this.xrTableCell78.Weight = 5.5713548648471223D;
            // 
            // xrTableCell79
            // 
            this.xrTableCell79.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.NationalId")});
            this.xrTableCell79.Name = "xrTableCell79";
            this.xrTableCell79.Weight = 5.48784720787816D;
            // 
            // xrTableCell80
            // 
            this.xrTableCell80.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.Nationality")});
            this.xrTableCell80.Name = "xrTableCell80";
            this.xrTableCell80.Weight = 4.1585888561400575D;
            // 
            // xrTableCell81
            // 
            this.xrTableCell81.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.Religion")});
            this.xrTableCell81.Name = "xrTableCell81";
            this.xrTableCell81.Weight = 3.8248246857119668D;
            // 
            // xrTableCell82
            // 
            this.xrTableCell82.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.MaritalStat")});
            this.xrTableCell82.Name = "xrTableCell82";
            this.xrTableCell82.StylePriority.UseTextAlignment = false;
            this.xrTableCell82.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell82.Weight = 4.4467393300463778D;
            // 
            // xrTableCell94
            // 
            this.xrTableCell94.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.Age")});
            this.xrTableCell94.Name = "xrTableCell94";
            this.xrTableCell94.StylePriority.UseTextAlignment = false;
            this.xrTableCell94.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell94.Weight = 2.7959431049694654D;
            // 
            // xrTableCell83
            // 
            this.xrTableCell83.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.BirthDate", "{0:yyyy-MM-dd}")});
            this.xrTableCell83.Name = "xrTableCell83";
            this.xrTableCell83.Weight = 5.0750712404894838D;
            // 
            // xrTableCell84
            // 
            this.xrTableCell84.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.Gender")});
            this.xrTableCell84.Name = "xrTableCell84";
            this.xrTableCell84.Weight = 3.0816492321218218D;
            // 
            // xrTableCell85
            // 
            this.xrTableCell85.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.LocationName")});
            this.xrTableCell85.Name = "xrTableCell85";
            this.xrTableCell85.Weight = 4.3942228314273324D;
            // 
            // xrTableCell86
            // 
            this.xrTableCell86.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.MangerName")});
            this.xrTableCell86.Name = "xrTableCell86";
            this.xrTableCell86.Weight = 4.58668393219603D;
            // 
            // xrTableCell87
            // 
            this.xrTableCell87.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.PositionName")});
            this.xrTableCell87.Name = "xrTableCell87";
            this.xrTableCell87.StylePriority.UseTextAlignment = false;
            this.xrTableCell87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell87.Weight = 4.3568703996742553D;
            // 
            // xrTableCell88
            // 
            this.xrTableCell88.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.Profession")});
            this.xrTableCell88.Name = "xrTableCell88";
            this.xrTableCell88.StylePriority.UseTextAlignment = false;
            this.xrTableCell88.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell88.Weight = 3.9504558049502716D;
            // 
            // xrTableCell89
            // 
            this.xrTableCell89.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.JobName")});
            this.xrTableCell89.Name = "xrTableCell89";
            this.xrTableCell89.Weight = 3.7259878009033143D;
            // 
            // xrTableCell90
            // 
            this.xrTableCell90.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.DeptName")});
            this.xrTableCell90.Name = "xrTableCell90";
            this.xrTableCell90.StylePriority.UseTextAlignment = false;
            this.xrTableCell90.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell90.Weight = 5.2273576345596657D;
            // 
            // xrTableCell91
            // 
            this.xrTableCell91.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell91.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.EmpName")});
            this.xrTableCell91.Name = "xrTableCell91";
            this.xrTableCell91.StylePriority.UseBorders = false;
            this.xrTableCell91.Weight = 5.5134042268799863D;
            // 
            // xrTableCell95
            // 
            this.xrTableCell95.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell95.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.bullet1});
            this.xrTableCell95.Name = "xrTableCell95";
            this.xrTableCell95.StylePriority.UseBorders = false;
            this.xrTableCell95.Weight = 0.848171883127878D;
            // 
            // bullet1
            // 
            this.bullet1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.bullet1.BulletBackColor = WebApp.Reports.en_EmployeesReports.BulletColor.green;
            this.bullet1.ForeColor = System.Drawing.Color.Green;
            this.bullet1.FormattingRules.Add(this.Active);
            this.bullet1.FormattingRules.Add(this.NotEmployment);
            this.bullet1.FormattingRules.Add(this.Suspend);
            this.bullet1.FormattingRules.Add(this.Terminated);
            this.bullet1.FormattingRules.Add(this.Invisible);
            this.bullet1.LocationFloat = new DevExpress.Utils.PointFloat(1F, 4.694445F);
            this.bullet1.Name = "bullet1";
            this.bullet1.SizeF = new System.Drawing.SizeF(15.27771F, 14.30554F);
            this.bullet1.StylePriority.UseBorders = false;
            // 
            // Active
            // 
            this.Active.Condition = "[EmpStatus]==1 And  [EmpStatus]!=2  And  [EmpStatus]!=0 And  [EmpStatus]!=3 And  " +
    "[EmpStatus]!=12";
            this.Active.DataMember = "EmployeeDataDetailsReport";
            this.Active.Formatting.ForeColor = System.Drawing.Color.Green;
            this.Active.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.Active.Name = "Active";
            // 
            // NotEmployment
            // 
            this.NotEmployment.Condition = "[EmpStatus]==0 And  [EmpStatus]!=2  And  [EmpStatus]!=1 And  [EmpStatus]!=3 And  " +
    "[EmpStatus]!=12";
            this.NotEmployment.DataMember = "EmployeeDataDetailsReport";
            this.NotEmployment.Formatting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.NotEmployment.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.NotEmployment.Name = "NotEmployment";
            // 
            // Suspend
            // 
            this.Suspend.Condition = "[EmpStatus]==2 And  [EmpStatus]!=1  And  [EmpStatus]!=0 And  [EmpStatus]!=3 And  " +
    "[EmpStatus]!=12";
            this.Suspend.DataMember = "EmployeeDataDetailsReport";
            this.Suspend.Formatting.ForeColor = System.Drawing.Color.Yellow;
            this.Suspend.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.Suspend.Name = "Suspend";
            // 
            // Terminated
            // 
            this.Terminated.Condition = "[EmpStatus]==3 And  [EmpStatus]!=2  And  [EmpStatus]!=0 And  [EmpStatus]!=1 And  " +
    "[EmpStatus]!=12";
            this.Terminated.DataMember = "EmployeeDataDetailsReport";
            this.Terminated.Formatting.ForeColor = System.Drawing.Color.Red;
            this.Terminated.Formatting.Visible = DevExpress.Utils.DefaultBoolean.True;
            this.Terminated.Name = "Terminated";
            // 
            // Invisible
            // 
            this.Invisible.Condition = "[DataSource.RowCount]==0";
            this.Invisible.DataMember = "EmployeeDataDetailsReport";
            this.Invisible.Formatting.Visible = DevExpress.Utils.DefaultBoolean.False;
            this.Invisible.Name = "Invisible";
            // 
            // xrTableCell92
            // 
            this.xrTableCell92.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmployeeDataDetailsReport.Code")});
            this.xrTableCell92.Name = "xrTableCell92";
            this.xrTableCell92.StylePriority.UseTextAlignment = false;
            this.xrTableCell92.Text = "xrTableCell92";
            this.xrTableCell92.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell92.Weight = 2.2199015244262679D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.StylePriority.UseTextAlignment = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.HeightF = 30.7002F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.StyleName = "tbl_header_style";
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTable1.BorderColor = System.Drawing.Color.Silver;
            this.xrTable1.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(1.907349E-06F, 5.700196F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(4612F, 25F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorderColor = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell39,
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell40,
            this.xrTableCell8,
            this.xrTableCell28,
            this.xrTableCell41,
            this.xrTableCell27,
            this.xrTableCell29,
            this.xrTableCell25,
            this.xrTableCell22,
            this.xrTableCell17,
            this.xrTableCell45,
            this.xrTableCell16,
            this.xrTableCell24,
            this.xrTableCell14,
            this.xrTableCell7,
            this.xrTableCell1,
            this.xrTableCell6,
            this.xrTableCell15,
            this.xrTableCell9,
            this.xrTableCell13,
            this.xrTableCell26,
            this.xrTableCell46,
            this.xrTableCell10,
            this.xrTableCell21,
            this.xrTableCell34,
            this.xrTableCell33,
            this.xrTableCell36,
            this.xrTableCell35,
            this.xrTableCell23,
            this.xrTableCell5,
            this.xrTableCell4,
            this.xrTableCell3,
            this.xrTableCell12,
            this.xrTableCell43,
            this.xrTableCell93,
            this.xrTableCell42,
            this.xrTableCell30,
            this.xrTableCell31,
            this.xrTableCell44,
            this.xrTableCell20,
            this.xrTableCell19,
            this.xrTableCell18,
            this.xrTableCell32,
            this.xrTableCell11,
            this.xrTableCell2});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 11.5D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.InteractiveSorting.FieldName = "IssuePlace";
            this.xrTableCell39.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.Text = "Issue Place";
            this.xrTableCell39.Weight = 0.00096227093425740168D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.InteractiveSorting.FieldName = "ExpiryDate";
            this.xrTableCell37.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.Text = "Expiry Date";
            this.xrTableCell37.Weight = 0.00091282652562216335D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.InteractiveSorting.FieldName = "IssueDate";
            this.xrTableCell38.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.Text = "Issue Date";
            this.xrTableCell38.Weight = 0.00083183061162706126D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.Text = ".Passport No";
            this.xrTableCell40.Weight = 0.0008440294790395212D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.InteractiveSorting.FieldName = "MilStatDate";
            this.xrTableCell8.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "Military Date";
            this.xrTableCell8.Weight = 0.0010354005376153057D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.Text = ".Military No";
            this.xrTableCell28.Weight = 0.00085097054530556984D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.InteractiveSorting.FieldName = "Militarystatus";
            this.xrTableCell41.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.Text = "Military Status";
            this.xrTableCell41.Weight = 0.00097646653064932737D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.InteractiveSorting.FieldName = "SubscripDate";
            this.xrTableCell27.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Text = "Subscribe Date";
            this.xrTableCell27.Weight = 0.00096287512229762393D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.Text = "SSN";
            this.xrTableCell29.Weight = 0.0010423391714025742D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.InteractiveSorting.FieldName = "التأمين";
            this.xrTableCell25.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Text = "Insured";
            this.xrTableCell25.Weight = 0.00099477008947931054D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.InteractiveSorting.FieldName = "VarSubAmt";
            this.xrTableCell22.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Text = "Var. Salary";
            this.xrTableCell22.Weight = 0.000908886277209354D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.InteractiveSorting.FieldName = "BasicSubAmt";
            this.xrTableCell17.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Text = "Basic Salary";
            this.xrTableCell17.Weight = 0.00092755352182384023D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.InteractiveSorting.FieldName = "WorkingPeriod";
            this.xrTableCell45.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.Text = "Working Period";
            this.xrTableCell45.Weight = 0.000896990696843116D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.InteractiveSorting.FieldName = "ContractEndDate";
            this.xrTableCell16.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Text = "Contract End Date";
            this.xrTableCell16.Weight = 0.0010940533078361429D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.InteractiveSorting.FieldName = "StartDate";
            this.xrTableCell24.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.Text = "Contract Date";
            this.xrTableCell24.Weight = 0.00092414678420641165D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.InteractiveSorting.FieldName = "ContractType";
            this.xrTableCell14.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = "Contract Type";
            this.xrTableCell14.Weight = 0.00090376120233667551D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.InteractiveSorting.FieldName = "AssignEndDate";
            this.xrTableCell7.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Text = "Assignment End Date";
            this.xrTableCell7.Weight = 0.001077913047287849D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.InteractiveSorting.FieldName = "AssignDate";
            this.xrTableCell1.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "Assignment Date";
            this.xrTableCell1.Weight = 0.00094881591056788649D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.InteractiveSorting.FieldName = "AssignStatus";
            this.xrTableCell6.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Text = "Assignment Status";
            this.xrTableCell6.Weight = 0.00096663447022026731D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.InteractiveSorting.FieldName = "JoinDate";
            this.xrTableCell15.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Text = "Join Date";
            this.xrTableCell15.Weight = 0.00082016190725818288D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.InteractiveSorting.FieldName = "GradYear";
            this.xrTableCell9.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Text = "Graduation Year";
            this.xrTableCell9.Weight = 0.00097037209704028092D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.InteractiveSorting.FieldName = "QualGrade";
            this.xrTableCell13.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Text = "Grade";
            this.xrTableCell13.Weight = 0.00076102150692381485D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.InteractiveSorting.FieldName = "QualName";
            this.xrTableCell26.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Text = "Qualification";
            this.xrTableCell26.Weight = 0.0010838426065576537D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.InteractiveSorting.FieldName = "UnivName";
            this.xrTableCell46.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.Text = "University";
            this.xrTableCell46.Weight = 0.00094964847780864723D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "Other Emails";
            this.xrTableCell10.Weight = 0.0015235815637826432D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.Text = "Work Email";
            this.xrTableCell21.Weight = 0.0012990678192925521D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.Text = ".Home Tel";
            this.xrTableCell34.Weight = 0.00096625222079217D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Text = "Mobile";
            this.xrTableCell33.Weight = 0.00097500800969348123D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.InteractiveSorting.FieldName = "CountryName";
            this.xrTableCell36.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Text = "Country";
            this.xrTableCell36.Weight = 0.00094571852485018492D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.InteractiveSorting.FieldName = "CityName";
            this.xrTableCell35.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Text = "City";
            this.xrTableCell35.Weight = 0.0010000737062563717D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.InteractiveSorting.FieldName = "District";
            this.xrTableCell23.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Text = "District";
            this.xrTableCell23.Weight = 0.00079055588847033189D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = "Address";
            this.xrTableCell5.Weight = 0.0012051336066886251D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "National ID";
            this.xrTableCell4.Weight = 0.0011870797102158668D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.InteractiveSorting.FieldName = "NationalityAr";
            this.xrTableCell3.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "Nationality";
            this.xrTableCell3.Weight = 0.00089954337909388979D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.InteractiveSorting.FieldName = "Religion";
            this.xrTableCell12.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "Religion";
            this.xrTableCell12.Weight = 0.00082734693053563083D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.InteractiveSorting.FieldName = "MaritalStat";
            this.xrTableCell43.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.Text = "Marital Status";
            this.xrTableCell43.Weight = 0.00096187193927676725D;
            // 
            // xrTableCell93
            // 
            this.xrTableCell93.InteractiveSorting.FieldName = "Age";
            this.xrTableCell93.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell93.Name = "xrTableCell93";
            this.xrTableCell93.Text = "Age";
            this.xrTableCell93.Weight = 0.00060479695565695711D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.Text = "Birth Date";
            this.xrTableCell42.Weight = 0.0010977837823745761D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.InteractiveSorting.FieldName = "Gender";
            this.xrTableCell30.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Text = "Gender";
            this.xrTableCell30.Weight = 0.0006665820175825464D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.InteractiveSorting.FieldName = "LocationName";
            this.xrTableCell31.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.Text = "Location";
            this.xrTableCell31.Weight = 0.00095051807896483318D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.InteractiveSorting.FieldName = "MangerName";
            this.xrTableCell44.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.Text = "Manager";
            this.xrTableCell44.Weight = 0.00099213734874754227D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.InteractiveSorting.FieldName = "PositionName";
            this.xrTableCell20.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.Text = "Position";
            this.xrTableCell20.Weight = 0.000942438384472272D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.InteractiveSorting.FieldName = "Profession";
            this.xrTableCell19.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.Text = "Profession";
            this.xrTableCell19.Weight = 0.00085452212652014487D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.InteractiveSorting.FieldName = "JobName";
            this.xrTableCell18.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.Text = "Job";
            this.xrTableCell18.Weight = 0.00080596750198169123D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.InteractiveSorting.FieldName = "DeptName";
            this.xrTableCell32.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.Text = "Department";
            this.xrTableCell32.Weight = 0.0011307272891439319D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.InteractiveSorting.FieldName = "EmpName";
            this.xrTableCell11.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = "Employee Name";
            this.xrTableCell11.Weight = 0.0013760757609456976D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = "Code";
            this.xrTableCell2.Weight = 0.00048018521253579116D;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel8,
            this.xrLabel5,
            this.xrLabel1,
            this.xrLabel10,
            this.xrLabel11,
            this.xrLabel2,
            this.companyLogo});
            this.ReportHeader.HeightF = 127.7299F;
            this.ReportHeader.Name = "ReportHeader";
            this.ReportHeader.StylePriority.UseBorders = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel4.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(1539.955F, 31.47222F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(463.8893F, 50.26794F);
            this.xrLabel4.StylePriority.UseBorderColor = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseForeColor = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Basic Employees Data";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel8
            // 
            this.xrLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(4493.66F, 67.97218F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(128.3398F, 23F);
            this.xrLabel8.StylePriority.UseBackColor = false;
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseForeColor = false;
            this.xrLabel8.Text = ":Contracts End Date";
            this.xrLabel8.Visible = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.AutoWidth = true;
            this.xrLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.xrLabel5.CanShrink = true;
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_ReportTitle.Column1")});
            this.xrLabel5.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(2281.221F, 90.97218F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
            this.xrLabel5.SizeF = new System.Drawing.SizeF(2340.779F, 36.75772F);
            this.xrLabel5.StylePriority.UseBackColor = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseForeColor = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel1
            // 
            this.xrLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.EmpEndDateDetails, "Text", "{0:yyyy-MM-dd}")});
            this.xrLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(4393.66F, 67.97218F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.Visible = false;
            // 
            // EmpEndDateDetails
            // 
            this.EmpEndDateDetails.Description = "Ended Contracts Before";
            this.EmpEndDateDetails.Name = "EmpEndDateDetails";
            this.EmpEndDateDetails.Type = typeof(System.DateTime);
            // 
            // xrLabel10
            // 
            this.xrLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(4108.452F, 67.97218F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(150.3618F, 23F);
            this.xrLabel10.StylePriority.UseBackColor = false;
            this.xrLabel10.StylePriority.UseBorders = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseForeColor = false;
            this.xrLabel10.Text = ": Assignments End Date";
            this.xrLabel10.Visible = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.AssignEndDateEmp, "Text", "{0:yyyy-MM-dd}")});
            this.xrLabel11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(4008.452F, 67.97218F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel11.StylePriority.UseBackColor = false;
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseForeColor = false;
            this.xrLabel11.Text = "xrLabel11";
            this.xrLabel11.Visible = false;
            // 
            // AssignEndDateEmp
            // 
            this.AssignEndDateEmp.Description = "Ended Assignments Before";
            this.AssignEndDateEmp.Name = "AssignEndDateEmp";
            this.AssignEndDateEmp.Type = typeof(System.DateTime);
            // 
            // xrLabel2
            // 
            this.xrLabel2.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.CompanyName, "Text", "")});
            this.xrLabel2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(4268.558F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(353.4419F, 38.17184F);
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
            this.companyLogo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.companyLogo.BorderWidth = 0F;
            this.companyLogo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", null, "GetCompanyLogo.file_stream")});
            this.companyLogo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.companyLogo.Name = "companyLogo";
            this.companyLogo.SizeF = new System.Drawing.SizeF(157.4803F, 78.74016F);
            this.companyLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.companyLogo.StylePriority.UseBorders = false;
            this.companyLogo.StylePriority.UseBorderWidth = false;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrLabel9,
            this.xrLabel16,
            this.bullet4,
            this.bullet3,
            this.xrLabel6,
            this.xrLabel7,
            this.bullet2,
            this.xrLabel3,
            this.xrLabel12,
            this.bullet5,
            this.xrPageInfo2});
            this.PageFooter.HeightF = 54.94444F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.ForeColor = System.Drawing.Color.Black;
            this.xrPageInfo1.Format = "Page{0} of {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(4522F, 31.86111F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseForeColor = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel9
            // 
            this.xrLabel9.ForeColor = System.Drawing.Color.Black;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(822.859F, 31.86111F);
            this.xrLabel9.Multiline = true;
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(37.5F, 23F);
            this.xrLabel9.StylePriority.UseForeColor = false;
            this.xrLabel9.Text = ": User\r\n";
            // 
            // xrLabel16
            // 
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.User, "Text", "")});
            this.xrLabel16.ForeColor = System.Drawing.Color.Black;
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(725.9701F, 31.86111F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(99.99994F, 23F);
            this.xrLabel16.StylePriority.UseForeColor = false;
            // 
            // User
            // 
            this.User.Description = "User";
            this.User.Name = "User";
            this.User.Visible = false;
            // 
            // bullet4
            // 
            this.bullet4.BackColor = System.Drawing.Color.Transparent;
            this.bullet4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.bullet4.BulletBackColor = WebApp.Reports.en_EmployeesReports.BulletColor.green;
            this.bullet4.EvenStyleName = "tbl_odd_detail";
            this.bullet4.ForeColor = System.Drawing.Color.Red;
            this.bullet4.LocationFloat = new DevExpress.Utils.PointFloat(2529.055F, 6.785838F);
            this.bullet4.Name = "bullet4";
            this.bullet4.OddStyleName = "tbl_even_detail";
            this.bullet4.SizeF = new System.Drawing.SizeF(12.35107F, 12.5F);
            this.bullet4.StylePriority.UseBackColor = false;
            this.bullet4.StylePriority.UseBorders = false;
            this.bullet4.StylePriority.UseForeColor = false;
            // 
            // bullet3
            // 
            this.bullet3.BackColor = System.Drawing.Color.Transparent;
            this.bullet3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.bullet3.BulletBackColor = WebApp.Reports.en_EmployeesReports.BulletColor.green;
            this.bullet3.EvenStyleName = "tbl_odd_detail";
            this.bullet3.ForeColor = System.Drawing.Color.Green;
            this.bullet3.LocationFloat = new DevExpress.Utils.PointFloat(2083.913F, 6.785838F);
            this.bullet3.Name = "bullet3";
            this.bullet3.OddStyleName = "tbl_even_detail";
            this.bullet3.SizeF = new System.Drawing.SizeF(12.3512F, 12.5F);
            this.bullet3.StylePriority.UseBackColor = false;
            this.bullet3.StylePriority.UseBorders = false;
            this.bullet3.StylePriority.UseForeColor = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.ForeColor = System.Drawing.Color.Black;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(2007.116F, 0F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.No;
            this.xrLabel6.SizeF = new System.Drawing.SizeF(76.79675F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseForeColor = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Active";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.ForeColor = System.Drawing.Color.Black;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(2416.39F, 0F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.No;
            this.xrLabel7.SizeF = new System.Drawing.SizeF(112.6643F, 23F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseForeColor = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Terminated";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // bullet2
            // 
            this.bullet2.BackColor = System.Drawing.Color.Transparent;
            this.bullet2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.bullet2.BulletBackColor = WebApp.Reports.en_EmployeesReports.BulletColor.green;
            this.bullet2.EvenStyleName = "tbl_odd_detail";
            this.bullet2.ForeColor = System.Drawing.Color.Yellow;
            this.bullet2.LocationFloat = new DevExpress.Utils.PointFloat(2315.089F, 8.360672F);
            this.bullet2.Name = "bullet2";
            this.bullet2.OddStyleName = "tbl_even_detail";
            this.bullet2.SizeF = new System.Drawing.SizeF(12.35107F, 12.5F);
            this.bullet2.StylePriority.UseBackColor = false;
            this.bullet2.StylePriority.UseBorders = false;
            this.bullet2.StylePriority.UseForeColor = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.ForeColor = System.Drawing.Color.Black;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(2191.837F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.No;
            this.xrLabel3.SizeF = new System.Drawing.SizeF(123.2515F, 23F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Suspend";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.ForeColor = System.Drawing.Color.Black;
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(2641.541F, 0F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.No;
            this.xrLabel12.SizeF = new System.Drawing.SizeF(112.6641F, 23F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseForeColor = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "Not employed";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // bullet5
            // 
            this.bullet5.BackColor = System.Drawing.Color.Transparent;
            this.bullet5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.bullet5.BulletBackColor = WebApp.Reports.en_EmployeesReports.BulletColor.green;
            this.bullet5.EvenStyleName = "tbl_odd_detail";
            this.bullet5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.bullet5.LocationFloat = new DevExpress.Utils.PointFloat(2754.205F, 6.785838F);
            this.bullet5.Name = "bullet5";
            this.bullet5.OddStyleName = "tbl_even_detail";
            this.bullet5.SizeF = new System.Drawing.SizeF(12.35083F, 12.5F);
            this.bullet5.StylePriority.UseBackColor = false;
            this.bullet5.StylePriority.UseBorders = false;
            this.bullet5.StylePriority.UseForeColor = false;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.ForeColor = System.Drawing.Color.Black;
            this.xrPageInfo2.Format = "{0:yyyy-MM-dd h:mm tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 31.86111F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.No;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(127.7293F, 23F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseForeColor = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // formattingRule2
            // 
            this.formattingRule2.Condition = "Iif([Insured] == 1, true ,false )";
            this.formattingRule2.DataMember = "EmployeeDataDetailsReport";
            this.formattingRule2.Name = "formattingRule2";
            // 
            // Odding
            // 
            this.Odding.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.Odding.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.Odding.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Odding.Name = "Odding";
            this.Odding.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
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
            this.GroupCaption3.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 2, 0, 0, 100F);
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
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
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
            this.xrControlStyle4.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrControlStyle2
            // 
            this.xrControlStyle2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrControlStyle2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrControlStyle2.Name = "xrControlStyle2";
            this.xrControlStyle2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // tbl_header_style
            // 
            this.tbl_header_style.BackColor = System.Drawing.Color.Silver;
            this.tbl_header_style.BorderColor = System.Drawing.Color.Black;
            this.tbl_header_style.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.tbl_header_style.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_header_style.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_header_style.ForeColor = System.Drawing.Color.Black;
            this.tbl_header_style.Name = "tbl_header_style";
            this.tbl_header_style.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.tbl_header_style.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // tbl_even_detail
            // 
            this.tbl_even_detail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tbl_even_detail.BorderColor = System.Drawing.Color.Black;
            this.tbl_even_detail.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.tbl_even_detail.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_even_detail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_even_detail.ForeColor = System.Drawing.Color.Black;
            this.tbl_even_detail.Name = "tbl_even_detail";
            this.tbl_even_detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 1, 1, 100F);
            this.tbl_even_detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // tbl_odd_detail
            // 
            this.tbl_odd_detail.BackColor = System.Drawing.Color.White;
            this.tbl_odd_detail.BorderColor = System.Drawing.Color.Black;
            this.tbl_odd_detail.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.tbl_odd_detail.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_odd_detail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_odd_detail.ForeColor = System.Drawing.Color.Black;
            this.tbl_odd_detail.Name = "tbl_odd_detail";
            this.tbl_odd_detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 1, 1, 100F);
            this.tbl_odd_detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // التأمين
            // 
            this.التأمين.DataMember = "EmployeeDataDetailsReport";
            this.التأمين.Expression = "Iif([Insured]==1,\'Insured\',\'Not Insured\')";
            this.التأمين.Name = "التأمين";
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
            // FromAge
            // 
            this.FromAge.Description = "Age From";
            this.FromAge.Name = "FromAge";
            this.FromAge.Type = typeof(int);
            this.FromAge.ValueInfo = "0";
            // 
            // ToAge
            // 
            this.ToAge.Description = "Age To";
            this.ToAge.Name = "ToAge";
            this.ToAge.Type = typeof(int);
            this.ToAge.ValueInfo = "0";
            // 
            // WorkingPeriodFrom
            // 
            this.WorkingPeriodFrom.Description = "Working Period From";
            this.WorkingPeriodFrom.Name = "WorkingPeriodFrom";
            this.WorkingPeriodFrom.Type = typeof(float);
            this.WorkingPeriodFrom.ValueInfo = "0";
            // 
            // WorkingPeriodTo
            // 
            this.WorkingPeriodTo.Description = "Working Period To";
            this.WorkingPeriodTo.Name = "WorkingPeriodTo";
            this.WorkingPeriodTo.Type = typeof(float);
            this.WorkingPeriodTo.ValueInfo = "0";
            // 
            // insured
            // 
            this.insured.Description = "Insured";
            staticListLookUpSettings2.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(0, "Not Insured"));
            staticListLookUpSettings2.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(1, "Insured"));
            this.insured.LookUpSettings = staticListLookUpSettings2;
            this.insured.MultiValue = true;
            this.insured.Name = "insured";
            this.insured.Type = typeof(int);
            // 
            // Nationality
            // 
            this.Nationality.Description = "Nationality";
            dynamicListLookUpSettings1.DataAdapter = null;
            dynamicListLookUpSettings1.DataMember = "EmployeeDataDetailsReport";
            dynamicListLookUpSettings1.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings1.DisplayMember = "Nationality";
            dynamicListLookUpSettings1.ValueMember = "NationaltyId";
            this.Nationality.LookUpSettings = dynamicListLookUpSettings1;
            this.Nationality.MultiValue = true;
            this.Nationality.Name = "Nationality";
            this.Nationality.Type = typeof(int);
            // 
            // deptName
            // 
            this.deptName.Description = "Department";
            dynamicListLookUpSettings2.DataAdapter = null;
            dynamicListLookUpSettings2.DataMember = "EmployeeDataDetailsReport";
            dynamicListLookUpSettings2.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings2.DisplayMember = "DeptName";
            dynamicListLookUpSettings2.ValueMember = "DeptId";
            this.deptName.LookUpSettings = dynamicListLookUpSettings2;
            this.deptName.MultiValue = true;
            this.deptName.Name = "deptName";
            this.deptName.Type = typeof(int);
            // 
            // Job
            // 
            this.Job.Description = "Job";
            dynamicListLookUpSettings3.DataAdapter = null;
            dynamicListLookUpSettings3.DataMember = "EmployeeDataDetailsReport";
            dynamicListLookUpSettings3.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings3.DisplayMember = "JobName";
            dynamicListLookUpSettings3.ValueMember = "JobId";
            this.Job.LookUpSettings = dynamicListLookUpSettings3;
            this.Job.MultiValue = true;
            this.Job.Name = "Job";
            this.Job.Type = typeof(int);
            // 
            // Culture
            // 
            this.Culture.Description = "Culture";
            staticListLookUpSettings3.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("ar-EG", "Arabic"));
            staticListLookUpSettings3.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("en-GB", "English"));
            this.Culture.LookUpSettings = staticListLookUpSettings3;
            this.Culture.Name = "Culture";
            this.Culture.ValueInfo = "En-GB";
            this.Culture.Visible = false;
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
            // mappingContractType
            // 
            this.mappingContractType.Description = "mappingContractType";
            this.mappingContractType.Name = "mappingContractType";
            this.mappingContractType.Visible = false;
            // 
            // mappingJob
            // 
            this.mappingJob.Description = "mappingJob";
            this.mappingJob.Name = "mappingJob";
            this.mappingJob.Visible = false;
            // 
            // mappingNationality
            // 
            this.mappingNationality.Description = "mappingNationality";
            this.mappingNationality.Name = "mappingNationality";
            this.mappingNationality.Visible = false;
            // 
            // mappingDeptName
            // 
            this.mappingDeptName.Description = "mappingDeptName";
            this.mappingDeptName.Name = "mappingDeptName";
            this.mappingDeptName.Visible = false;
            // 
            // ContractType
            // 
            this.ContractType.Description = "Contract Type";
            dynamicListLookUpSettings4.DataAdapter = null;
            dynamicListLookUpSettings4.DataMember = "EmployeeDataDetailsReport";
            dynamicListLookUpSettings4.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings4.DisplayMember = "ContractType";
            dynamicListLookUpSettings4.ValueMember = "ContractType";
            this.ContractType.LookUpSettings = dynamicListLookUpSettings4;
            this.ContractType.MultiValue = true;
            this.ContractType.Name = "ContractType";
            // 
            // MappingInsured
            // 
            this.MappingInsured.Description = "MappingInsured";
            this.MappingInsured.Name = "MappingInsured";
            this.MappingInsured.Visible = false;
            // 
            // sqlDataSource2
            // 
            this.sqlDataSource2.ConnectionName = "HrContext";
            this.sqlDataSource2.Name = "sqlDataSource2";
            storedProcQuery2.Name = "sp_ReportTitle";
            queryParameter15.Name = "@EmpIds";
            queryParameter15.Type = typeof(string);
            queryParameter16.Name = "@JobIds";
            queryParameter16.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter16.Value = new DevExpress.DataAccess.Expression("", typeof(string));
            queryParameter17.Name = "@deptIds";
            queryParameter17.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter17.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingDeptName]", typeof(string));
            queryParameter18.Name = "@culture";
            queryParameter18.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter18.Value = new DevExpress.DataAccess.Expression("[Parameters.Culture]", typeof(string));
            queryParameter19.Name = "@NationIds";
            queryParameter19.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter19.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingNationality]", typeof(string));
            queryParameter20.Name = "@ContractsIds";
            queryParameter20.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter20.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingContractType]", typeof(string));
            storedProcQuery2.Parameters.Add(queryParameter15);
            storedProcQuery2.Parameters.Add(queryParameter16);
            storedProcQuery2.Parameters.Add(queryParameter17);
            storedProcQuery2.Parameters.Add(queryParameter18);
            storedProcQuery2.Parameters.Add(queryParameter19);
            storedProcQuery2.Parameters.Add(queryParameter20);
            storedProcQuery2.StoredProcName = "sp_ReportTitle";
            this.sqlDataSource2.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery2});
            this.sqlDataSource2.ResultSchemaSerializable = "PERhdGFTZXQgTmFtZT0ic3FsRGF0YVNvdXJjZTIiPjxWaWV3IE5hbWU9InNwX1JlcG9ydFRpdGxlIj48R" +
    "mllbGQgTmFtZT0iQ29sdW1uMSIgVHlwZT0iU3RyaW5nIiAvPjwvVmlldz48L0RhdGFTZXQ+";
            // 
            // en_EmployeesDetails
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.ReportHeader,
            this.PageFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.التأمين});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1,
            this.sqlDataSource2});
            this.DataMember = "EmployeeDataDetailsReport";
            this.DataSource = this.sqlDataSource1;
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.Invisible,
            this.NotEmployment,
            this.Active,
            this.Suspend,
            this.Terminated,
            this.formattingRule2});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(1, 3, 0, 0);
            this.PageHeight = 4626;
            this.PageWidth = 4626;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Gender,
            this.FromAge,
            this.ToAge,
            this.WorkingPeriodFrom,
            this.WorkingPeriodTo,
            this.insured,
            this.EmpEndDateDetails,
            this.AssignEndDateEmp,
            this.Nationality,
            this.deptName,
            this.Job,
            this.Culture,
            this.CompanyId,
            this.mappingGender,
            this.mappingContractType,
            this.mappingJob,
            this.mappingNationality,
            this.mappingDeptName,
            this.User,
            this.ContractType,
            this.CompanyName,
            this.MappingInsured});
            this.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
            this.RightToLeftLayout = DevExpress.XtraReports.UI.RightToLeftLayout.Yes;
            this.RollPaper = true;
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
