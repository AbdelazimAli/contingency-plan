﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using WebApp.Reports.EmployeesReports;

/// <summary>
/// Summary description for InsuranceEmployeesReports
/// </summary>
public class InsuranceEmployeesReport : DevExpress.XtraReports.UI.XtraReport,IBasicInfoReport
{
    public DetailBand Detail;
    public XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell30;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private PageHeaderBand PageHeader;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel1;
    private XRPictureBox companyLogo;
    private XRLabel xrLabel4;
    private PageFooterBand PageFooter;
    private XRControlStyle Odding;
    private XRControlStyle GroupCaption3;
    private XRControlStyle Title;
    private XRControlStyle xrControlStyle4;
    private XRControlStyle xrControlStyle2;
    private XRControlStyle tbl_header_style;
    private XRControlStyle tbl_even_detail;
    private XRControlStyle tbl_odd_detail;
    private CalculatedField BasicComp;
    private CalculatedField BasicEmp;
    private CalculatedField VarComp;
    private CalculatedField VarEmp;
    private DevExpress.XtraReports.Parameters.Parameter Gender;
    private DevExpress.XtraReports.Parameters.Parameter MappingEmpIds;
    private DevExpress.XtraReports.Parameters.Parameter Nationality;
    private DevExpress.XtraReports.Parameters.Parameter JobIds;
    private DevExpress.XtraReports.Parameters.Parameter mappingNationality;
    private DevExpress.XtraReports.Parameters.Parameter MappingJobIds;
    private DevExpress.XtraReports.Parameters.Parameter mappingGender;
    private DevExpress.XtraReports.Parameters.Parameter CompanyId;
    private DevExpress.XtraReports.Parameters.Parameter Culture;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell34;
    private CalculatedField Sum;
    private CalculatedField SumEmp;
    private CalculatedField SumComp;
    private XRTableCell xrTableCell5;
    private DevExpress.XtraReports.Parameters.Parameter User;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell15;
    public XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell8;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell26;
    private DevExpress.XtraReports.Parameters.Parameter MappingDeptIds;
    private DevExpress.XtraReports.Parameters.Parameter DeptIds;
    public GroupHeaderBand GroupHeader1;
    private XRPageInfo xrPageInfo2;
    private XRPageInfo xrPageInfo1;
    private XRLabel xrLabel3;
    private XRLabel xrLabel2;
    private XRLabel xrLabel17;
    private XRLabel xrLabel18;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell41;
    private XRLabel xrLabel9;
    private XRLabel xrLabel12;
    private DevExpress.XtraReports.Parameters.Parameter CompanyName;
    public XRTable xrTable3;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell44;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell47;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell49;
    private XRTableCell xrTableCell50;
    private XRTableCell xrTableCell51;
    private XRTableRow xrTableRow5;
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
    public XRTable xrTable4;
    private XRTableRow xrTableRow6;
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
    private XRTableCell xrTableCell88;
    private XRTableCell xrTableCell86;
    private XRTableCell xrTableCell89;
    private XRTableCell xrTableCell87;
    private XRTableCell xrTableCell84;
    private XRTableCell xrTableCell82;
    private XRTableCell xrTableCell85;
    private XRTableCell xrTableCell83;
    private XRTableCell xrTableCell93;
    private XRTableCell xrTableCell91;
    private XRTableCell xrTableCell92;
    private XRTableCell xrTableCell90;
    private DevExpress.XtraReports.Parameters.Parameter from;
    private DevExpress.XtraReports.Parameters.Parameter to;
    private DevExpress.XtraReports.Parameters.Parameter GroupingBy;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private DevExpress.XtraReports.Parameters.Parameter EmpIds;
    private DevExpress.XtraReports.Parameters.Parameter insured;
    private DevExpress.XtraReports.Parameters.Parameter MappingInsured;

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

    public InsuranceEmployeesReport()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsuranceEmployeesReport));
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings1 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings1 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings2 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings2 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings3 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings3 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings4 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings4 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
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
            this.xrTableCell93 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell81 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell92 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell86 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
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
            this.xrTableCell89 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.companyLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.CompanyName = new DevExpress.XtraReports.Parameters.Parameter();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.User = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.Odding = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupCaption3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle4 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_header_style = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_even_detail = new DevExpress.XtraReports.UI.XRControlStyle();
            this.tbl_odd_detail = new DevExpress.XtraReports.UI.XRControlStyle();
            this.BasicComp = new DevExpress.XtraReports.UI.CalculatedField();
            this.BasicEmp = new DevExpress.XtraReports.UI.CalculatedField();
            this.VarComp = new DevExpress.XtraReports.UI.CalculatedField();
            this.VarEmp = new DevExpress.XtraReports.UI.CalculatedField();
            this.Gender = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingEmpIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.Nationality = new DevExpress.XtraReports.Parameters.Parameter();
            this.JobIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.mappingNationality = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingJobIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.mappingGender = new DevExpress.XtraReports.Parameters.Parameter();
            this.CompanyId = new DevExpress.XtraReports.Parameters.Parameter();
            this.Culture = new DevExpress.XtraReports.Parameters.Parameter();
            this.Sum = new DevExpress.XtraReports.UI.CalculatedField();
            this.SumEmp = new DevExpress.XtraReports.UI.CalculatedField();
            this.SumComp = new DevExpress.XtraReports.UI.CalculatedField();
            this.MappingDeptIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.DeptIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.from = new DevExpress.XtraReports.Parameters.Parameter();
            this.to = new DevExpress.XtraReports.Parameters.Parameter();
            this.GroupingBy = new DevExpress.XtraReports.Parameters.Parameter();
            this.EmpIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.insured = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingInsured = new DevExpress.XtraReports.Parameters.Parameter();
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
            storedProcQuery1.Name = "SP_InsuranceEmployeeReport";
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
            queryParameter7.Name = "@Nationality";
            queryParameter7.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter7.Value = new DevExpress.DataAccess.Expression("[Parameters.mappingNationality]", typeof(string));
            queryParameter8.Name = "@Insured";
            queryParameter8.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter8.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingInsured]", typeof(string));
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.Parameters.Add(queryParameter5);
            storedProcQuery1.Parameters.Add(queryParameter6);
            storedProcQuery1.Parameters.Add(queryParameter7);
            storedProcQuery1.Parameters.Add(queryParameter8);
            storedProcQuery1.StoredProcName = "SP_InsuranceEmployeeReport";
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
            this.Detail.EvenStyleName = "tbl_even_detail";
            this.Detail.HeightF = 70.55556F;
            this.Detail.Name = "Detail";
            this.Detail.OddStyleName = "tbl_odd_detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("EmpName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("DeptName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("AssigmentStatus", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("EmpStatus", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("SubscripDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Sum", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("SumEmp", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("VarEmp", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("VarComp", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("BasicEmp", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("BasicComp", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("SumComp", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("VarSubAmt", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("BasicSubAmt", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("VarSubAmt", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("BasicSubAmt", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending),
            new DevExpress.XtraReports.UI.GroupField("EmpName", DevExpress.XtraReports.UI.XRColumnSortOrder.Descending)});
            this.Detail.StylePriority.UseBorderColor = false;
            this.Detail.StylePriority.UseFont = false;
            this.Detail.StylePriority.UseForeColor = false;
            this.Detail.StylePriority.UseTextAlignment = false;
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
            this.xrTable4.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.OddStyleName = "tbl_even_detail";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable4.SizeF = new System.Drawing.SizeF(2964F, 70.55556F);
            this.xrTable4.StylePriority.UseBorderColor = false;
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseBorderWidth = false;
            this.xrTable4.StylePriority.UseFont = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
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
            this.xrTableCell93,
            this.xrTableCell78,
            this.xrTableCell79,
            this.xrTableCell91,
            this.xrTableCell80,
            this.xrTableCell81});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 11.5D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.Sum")});
            this.xrTableCell67.Dpi = 254F;
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.StylePriority.UseTextAlignment = false;
            this.xrTableCell67.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell67.Weight = 0.010449494791098843D;
            // 
            // xrTableCell68
            // 
            this.xrTableCell68.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.SumComp")});
            this.xrTableCell68.Dpi = 254F;
            this.xrTableCell68.Name = "xrTableCell68";
            this.xrTableCell68.StylePriority.UseTextAlignment = false;
            this.xrTableCell68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell68.Weight = 0.0106037952388231D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.VarComp")});
            this.xrTableCell69.Dpi = 254F;
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.StylePriority.UseTextAlignment = false;
            this.xrTableCell69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell69.Weight = 0.010018854077106426D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.BasicComp")});
            this.xrTableCell70.Dpi = 254F;
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.StylePriority.UseTextAlignment = false;
            this.xrTableCell70.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell70.Weight = 0.0086408693909782817D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.SumEmp")});
            this.xrTableCell71.Dpi = 254F;
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.StylePriority.UseTextAlignment = false;
            this.xrTableCell71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell71.Weight = 0.010897569189218621D;
            // 
            // xrTableCell72
            // 
            this.xrTableCell72.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.VarEmp")});
            this.xrTableCell72.Dpi = 254F;
            this.xrTableCell72.Name = "xrTableCell72";
            this.xrTableCell72.StylePriority.UseTextAlignment = false;
            this.xrTableCell72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell72.Weight = 0.008788429495290068D;
            // 
            // xrTableCell73
            // 
            this.xrTableCell73.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.BasicEmp")});
            this.xrTableCell73.Dpi = 254F;
            this.xrTableCell73.Name = "xrTableCell73";
            this.xrTableCell73.StylePriority.UseTextAlignment = false;
            this.xrTableCell73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell73.Weight = 0.0107135683156184D;
            // 
            // xrTableCell74
            // 
            this.xrTableCell74.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.VarSubAmt")});
            this.xrTableCell74.Dpi = 254F;
            this.xrTableCell74.Name = "xrTableCell74";
            this.xrTableCell74.StylePriority.UseTextAlignment = false;
            this.xrTableCell74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell74.Weight = 0.010194125834680538D;
            // 
            // xrTableCell75
            // 
            this.xrTableCell75.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.BasicSubAmt")});
            this.xrTableCell75.Dpi = 254F;
            this.xrTableCell75.Name = "xrTableCell75";
            this.xrTableCell75.StylePriority.UseTextAlignment = false;
            this.xrTableCell75.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell75.Weight = 0.009625192791798522D;
            // 
            // xrTableCell76
            // 
            this.xrTableCell76.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.SubscripDate", "{0:MM/dd/yyyy}")});
            this.xrTableCell76.Dpi = 254F;
            this.xrTableCell76.Name = "xrTableCell76";
            this.xrTableCell76.StylePriority.UseTextAlignment = false;
            this.xrTableCell76.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell76.Weight = 0.017423464959323055D;
            // 
            // xrTableCell77
            // 
            this.xrTableCell77.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.Ssn")});
            this.xrTableCell77.Dpi = 254F;
            this.xrTableCell77.Name = "xrTableCell77";
            this.xrTableCell77.StylePriority.UseTextAlignment = false;
            this.xrTableCell77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell77.Weight = 0.016202571824828541D;
            // 
            // xrTableCell93
            // 
            this.xrTableCell93.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.EmpStartDate", "{0:MM/dd/yyyy}")});
            this.xrTableCell93.Dpi = 254F;
            this.xrTableCell93.Name = "xrTableCell93";
            this.xrTableCell93.StylePriority.UseTextAlignment = false;
            this.xrTableCell93.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell93.Weight = 0.014000672601243453D;
            // 
            // xrTableCell78
            // 
            this.xrTableCell78.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.JobName")});
            this.xrTableCell78.Dpi = 254F;
            this.xrTableCell78.Name = "xrTableCell78";
            this.xrTableCell78.StylePriority.UseTextAlignment = false;
            this.xrTableCell78.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell78.Weight = 0.013019402948118118D;
            // 
            // xrTableCell79
            // 
            this.xrTableCell79.BookmarkParent = this.xrTableCell7;
            this.xrTableCell79.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Bookmark", null, "EmployeeDetailsReport.DeptName"),
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.DeptName")});
            this.xrTableCell79.Dpi = 254F;
            this.xrTableCell79.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell79.Name = "xrTableCell79";
            this.xrTableCell79.StylePriority.UseTextAlignment = false;
            this.xrTableCell79.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell79.Weight = 0.013369443737807707D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell7.InteractiveSorting.FieldName = "DeptName";
            this.xrTableCell7.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.RowSpan = 2;
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.Text = "الوظيفة";
            this.xrTableCell7.Weight = 0.018330883615780287D;
            // 
            // xrTableCell91
            // 
            this.xrTableCell91.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.NationalityAr")});
            this.xrTableCell91.Dpi = 254F;
            this.xrTableCell91.Name = "xrTableCell91";
            this.xrTableCell91.StylePriority.UseTextAlignment = false;
            this.xrTableCell91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell91.Weight = 0.011427790896245222D;
            // 
            // xrTableCell80
            // 
            this.xrTableCell80.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.EmpName")});
            this.xrTableCell80.Dpi = 254F;
            this.xrTableCell80.Name = "xrTableCell80";
            this.xrTableCell80.StylePriority.UseTextAlignment = false;
            this.xrTableCell80.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell80.Weight = 0.016720413538255124D;
            // 
            // xrTableCell81
            // 
            this.xrTableCell81.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.EmpCode")});
            this.xrTableCell81.Dpi = 254F;
            this.xrTableCell81.Name = "xrTableCell81";
            this.xrTableCell81.StylePriority.UseTextAlignment = false;
            this.xrTableCell81.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell81.Weight = 0.0059686510311291047D;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderColor = System.Drawing.Color.Silver;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Dpi = 254F;
            this.xrTable2.EvenStyleName = "tbl_odd_detail";
            this.xrTable2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.OddStyleName = "tbl_even_detail";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(2964F, 70.55556F);
            this.xrTable2.StylePriority.UseBorderColor = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseBorderWidth = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrTableCell39,
            this.xrTableCell30,
            this.xrTableCell11,
            this.xrTableCell40,
            this.xrTableCell31,
            this.xrTableCell12,
            this.xrTableCell24,
            this.xrTableCell15,
            this.xrTableCell34,
            this.xrTableCell13,
            this.xrTableCell92,
            this.xrTableCell25,
            this.xrTableCell90,
            this.xrTableCell17,
            this.xrTableCell18});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 11.5D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.Sum")});
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell5.Weight = 0.010449492771240567D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.SumComp")});
            this.xrTableCell39.Dpi = 254F;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.StylePriority.UseTextAlignment = false;
            this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell39.Weight = 0.010603795245537872D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.VarComp")});
            this.xrTableCell30.Dpi = 254F;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.StylePriority.UseTextAlignment = false;
            this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell30.Weight = 0.010018857123452797D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.BasicComp")});
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell11.Weight = 0.0086408510486538637D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.SumEmp")});
            this.xrTableCell40.Dpi = 254F;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.StylePriority.UseTextAlignment = false;
            this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell40.Weight = 0.010897578890994583D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.VarEmp")});
            this.xrTableCell31.Dpi = 254F;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.StylePriority.UseTextAlignment = false;
            this.xrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell31.Weight = 0.0087884362557919191D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.BasicEmp")});
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell12.Weight = 0.010713563083894538D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.VarSubAmt")});
            this.xrTableCell24.Dpi = 254F;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell24.Weight = 0.010194119719952774D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.BasicSubAmt")});
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell15.Weight = 0.0096252000287184742D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.SubscripDate", "{0:MM/dd/yyyy}")});
            this.xrTableCell34.Dpi = 254F;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.StylePriority.UseTextAlignment = false;
            this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell34.Weight = 0.019642529876538362D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.Ssn")});
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell13.Weight = 0.01905185587131775D;
            // 
            // xrTableCell92
            // 
            this.xrTableCell92.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.EmpStartDate", "{0:MM/dd/yyyy}")});
            this.xrTableCell92.Dpi = 254F;
            this.xrTableCell92.Name = "xrTableCell92";
            this.xrTableCell92.StylePriority.UseTextAlignment = false;
            this.xrTableCell92.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell92.Weight = 0.01466933197098521D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.JobName")});
            this.xrTableCell25.Dpi = 254F;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell25.Weight = 0.018330877546634684D;
            // 
            // xrTableCell90
            // 
            this.xrTableCell90.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.NationalityAr")});
            this.xrTableCell90.Dpi = 254F;
            this.xrTableCell90.Name = "xrTableCell90";
            this.xrTableCell90.StylePriority.UseTextAlignment = false;
            this.xrTableCell90.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell90.Weight = 0.00991275527282014D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.EmpName")});
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell17.Weight = 0.020556407121643416D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.EmpCode")});
            this.xrTableCell18.Dpi = 254F;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell18.Weight = 0.0059686473064515895D;
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
            this.PageHeader.HeightF = 129.1313F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.StyleName = "tbl_header_style";
            this.PageHeader.StylePriority.UseTextAlignment = false;
            // 
            // xrTable3
            // 
            this.xrTable3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTable3.BorderColor = System.Drawing.Color.Silver;
            this.xrTable3.Dpi = 254F;
            this.xrTable3.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2.13133F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4,
            this.xrTableRow5});
            this.xrTable3.SizeF = new System.Drawing.SizeF(2964F, 127F);
            this.xrTable3.StyleName = "tbl_header_style";
            this.xrTable3.StylePriority.UseBackColor = false;
            this.xrTable3.StylePriority.UseBorderColor = false;
            this.xrTable3.StylePriority.UseFont = false;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell27,
            this.xrTableCell42,
            this.xrTableCell43,
            this.xrTableCell44,
            this.xrTableCell45,
            this.xrTableCell46,
            this.xrTableCell47,
            this.xrTableCell88,
            this.xrTableCell48,
            this.xrTableCell49,
            this.xrTableCell86,
            this.xrTableCell50,
            this.xrTableCell51});
            this.xrTableRow4.Dpi = 254F;
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 11.5D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell9.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.InteractiveSorting.FieldName = "Sum";
            this.xrTableCell9.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.RowSpan = 2;
            this.xrTableCell9.StyleName = "tbl_header_style";
            this.xrTableCell9.StylePriority.UseBackColor = false;
            this.xrTableCell9.StylePriority.UseBorderColor = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.Text = "الإجمالى";
            this.xrTableCell9.Weight = 0.010449495538269613D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell27.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell27.Dpi = 254F;
            this.xrTableCell27.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell27.InteractiveSorting.FieldName = "SumComp";
            this.xrTableCell27.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.RowSpan = 2;
            this.xrTableCell27.StylePriority.UseBackColor = false;
            this.xrTableCell27.StylePriority.UseBorderColor = false;
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.Text = "إجمالى حصة الشركة";
            this.xrTableCell27.Weight = 0.010603801695900408D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell42.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell42.Dpi = 254F;
            this.xrTableCell42.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.StyleName = "tbl_header_style";
            this.xrTableCell42.StylePriority.UseBackColor = false;
            this.xrTableCell42.StylePriority.UseBorderColor = false;
            this.xrTableCell42.StylePriority.UseFont = false;
            this.xrTableCell42.Text = "حصة الشركة";
            this.xrTableCell42.Weight = 0.018659724303444807D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.Dpi = 254F;
            this.xrTableCell43.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell43.InteractiveSorting.FieldName = "SumEmp";
            this.xrTableCell43.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.RowSpan = 2;
            this.xrTableCell43.StylePriority.UseFont = false;
            this.xrTableCell43.Text = "إجمالى حصة العامل";
            this.xrTableCell43.Weight = 0.010897571753378745D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.Dpi = 254F;
            this.xrTableCell44.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.StylePriority.UseFont = false;
            this.xrTableCell44.Text = "حصة العامل";
            this.xrTableCell44.Weight = 0.019502006938758497D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.Dpi = 254F;
            this.xrTableCell45.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.StylePriority.UseFont = false;
            this.xrTableCell45.Text = "الراتب التأميني";
            this.xrTableCell45.Weight = 0.019819325878597235D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.Dpi = 254F;
            this.xrTableCell46.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell46.InteractiveSorting.FieldName = "SubscripDate";
            this.xrTableCell46.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.RowSpan = 2;
            this.xrTableCell46.StylePriority.UseFont = false;
            this.xrTableCell46.Text = "تاريخ الاشتراك";
            this.xrTableCell46.Weight = 0.017423461493951006D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.Dpi = 254F;
            this.xrTableCell47.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.RowSpan = 2;
            this.xrTableCell47.StylePriority.UseFont = false;
            this.xrTableCell47.StylePriority.UseTextAlignment = false;
            this.xrTableCell47.Text = "الرقم التأمينى";
            this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell47.Weight = 0.016202574946482882D;
            // 
            // xrTableCell88
            // 
            this.xrTableCell88.Dpi = 254F;
            this.xrTableCell88.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell88.Name = "xrTableCell88";
            this.xrTableCell88.RowSpan = 2;
            this.xrTableCell88.StylePriority.UseFont = false;
            this.xrTableCell88.Text = "ت.التعيين";
            this.xrTableCell88.Weight = 0.014000675219753133D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.Dpi = 254F;
            this.xrTableCell48.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell48.InteractiveSorting.FieldName = "JobName";
            this.xrTableCell48.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.RowSpan = 2;
            this.xrTableCell48.StylePriority.UseFont = false;
            this.xrTableCell48.Text = "الوظيفة";
            this.xrTableCell48.Weight = 0.01301940637896447D;
            // 
            // xrTableCell49
            // 
            this.xrTableCell49.Dpi = 254F;
            this.xrTableCell49.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell49.InteractiveSorting.FieldName = "DeptName";
            this.xrTableCell49.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell49.Name = "xrTableCell49";
            this.xrTableCell49.RowSpan = 2;
            this.xrTableCell49.StylePriority.UseFont = false;
            this.xrTableCell49.Text = "الإدارة";
            this.xrTableCell49.Weight = 0.013369446441074131D;
            // 
            // xrTableCell86
            // 
            this.xrTableCell86.Dpi = 254F;
            this.xrTableCell86.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell86.Name = "xrTableCell86";
            this.xrTableCell86.RowSpan = 2;
            this.xrTableCell86.StylePriority.UseFont = false;
            this.xrTableCell86.Text = "الجنسية";
            this.xrTableCell86.Weight = 0.011427799273345021D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.Dpi = 254F;
            this.xrTableCell50.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell50.InteractiveSorting.FieldName = "EmpName";
            this.xrTableCell50.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.RowSpan = 2;
            this.xrTableCell50.StylePriority.UseFont = false;
            this.xrTableCell50.Text = "اسم الموظف";
            this.xrTableCell50.Weight = 0.016720417935277256D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell51.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell51.Dpi = 254F;
            this.xrTableCell51.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.RowSpan = 2;
            this.xrTableCell51.StylePriority.UseBackColor = false;
            this.xrTableCell51.StylePriority.UseBorderColor = false;
            this.xrTableCell51.StylePriority.UseFont = false;
            this.xrTableCell51.Text = "الكود";
            this.xrTableCell51.Weight = 0.0059686447081212859D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
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
            this.xrTableCell89,
            this.xrTableCell63,
            this.xrTableCell64,
            this.xrTableCell87,
            this.xrTableCell65,
            this.xrTableCell66});
            this.xrTableRow5.Dpi = 254F;
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 11.5D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell52.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell52.Dpi = 254F;
            this.xrTableCell52.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.StylePriority.UseBackColor = false;
            this.xrTableCell52.StylePriority.UseBorderColor = false;
            this.xrTableCell52.StylePriority.UseFont = false;
            this.xrTableCell52.Text = "xrTableCell38";
            this.xrTableCell52.Weight = 0.01232997429009474D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell53.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell53.Dpi = 254F;
            this.xrTableCell53.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell53.InteractiveSorting.FieldName = "SubscripDate";
            this.xrTableCell53.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.StylePriority.UseBackColor = false;
            this.xrTableCell53.StylePriority.UseBorderColor = false;
            this.xrTableCell53.StylePriority.UseFont = false;
            this.xrTableCell53.Weight = 0.012512052073699686D;
            // 
            // xrTableCell54
            // 
            this.xrTableCell54.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell54.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell54.Dpi = 254F;
            this.xrTableCell54.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell54.InteractiveSorting.FieldName = "VarComp";
            this.xrTableCell54.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell54.Name = "xrTableCell54";
            this.xrTableCell54.StylePriority.UseBackColor = false;
            this.xrTableCell54.StylePriority.UseBorderColor = false;
            this.xrTableCell54.StylePriority.UseFont = false;
            this.xrTableCell54.Text = "متغير";
            this.xrTableCell54.Weight = 0.011821834508843549D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell55.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell55.Dpi = 254F;
            this.xrTableCell55.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell55.InteractiveSorting.FieldName = "BasicComp";
            this.xrTableCell55.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.StylePriority.UseBackColor = false;
            this.xrTableCell55.StylePriority.UseBorderColor = false;
            this.xrTableCell55.StylePriority.UseFont = false;
            this.xrTableCell55.Text = "ثابت";
            this.xrTableCell55.Weight = 0.010195870093805577D;
            // 
            // xrTableCell56
            // 
            this.xrTableCell56.Dpi = 254F;
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.Text = "xrTableCell36";
            this.xrTableCell56.Weight = 0.012858685909782765D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.Dpi = 254F;
            this.xrTableCell57.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell57.InteractiveSorting.FieldName = "VarEmp";
            this.xrTableCell57.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.StylePriority.UseFont = false;
            this.xrTableCell57.Text = "متغير";
            this.xrTableCell57.Weight = 0.010369987254023562D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.Dpi = 254F;
            this.xrTableCell58.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell58.InteractiveSorting.FieldName = "BasicEmp";
            this.xrTableCell58.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.StylePriority.UseFont = false;
            this.xrTableCell58.Text = "ثابت";
            this.xrTableCell58.Weight = 0.012641572452544787D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.Dpi = 254F;
            this.xrTableCell59.InteractiveSorting.FieldName = "VarSubAmt";
            this.xrTableCell59.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.Text = "متغير";
            this.xrTableCell59.Weight = 0.012028646413525841D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.Dpi = 254F;
            this.xrTableCell60.InteractiveSorting.FieldName = "BasicSubAmt";
            this.xrTableCell60.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.Text = "ثابت";
            this.xrTableCell60.Weight = 0.011357351602699175D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.Dpi = 254F;
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.Weight = 0.020558959914624659D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.Dpi = 254F;
            this.xrTableCell62.InteractiveSorting.FieldName = "EmpStatus";
            this.xrTableCell62.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.Weight = 0.019118369352813365D;
            // 
            // xrTableCell89
            // 
            this.xrTableCell89.Dpi = 254F;
            this.xrTableCell89.Name = "xrTableCell89";
            this.xrTableCell89.Text = "xrTableCell89";
            this.xrTableCell89.Weight = 0.016520219784154928D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.Dpi = 254F;
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.Text = "xrTableCell43";
            this.xrTableCell63.Weight = 0.015362362998685089D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.Dpi = 254F;
            this.xrTableCell64.InteractiveSorting.FieldName = "DeptName";
            this.xrTableCell64.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.Weight = 0.015775395196357664D;
            // 
            // xrTableCell87
            // 
            this.xrTableCell87.Dpi = 254F;
            this.xrTableCell87.Name = "xrTableCell87";
            this.xrTableCell87.Text = "xrTableCell87";
            this.xrTableCell87.Weight = 0.013484331920765593D;
            // 
            // xrTableCell65
            // 
            this.xrTableCell65.Dpi = 254F;
            this.xrTableCell65.InteractiveSorting.FieldName = "EmpName";
            this.xrTableCell65.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell65.Name = "xrTableCell65";
            this.xrTableCell65.Weight = 0.019729384536607539D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell66.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell66.Dpi = 254F;
            this.xrTableCell66.InteractiveSorting.FieldName = "Code";
            this.xrTableCell66.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.StylePriority.UseBackColor = false;
            this.xrTableCell66.StylePriority.UseBorderColor = false;
            this.xrTableCell66.Weight = 0.0070427736910922134D;
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTable1.BorderColor = System.Drawing.Color.Silver;
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2.13133F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow3});
            this.xrTable1.SizeF = new System.Drawing.SizeF(2964F, 127F);
            this.xrTable1.StyleName = "tbl_header_style";
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorderColor = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell37,
            this.xrTableCell2,
            this.xrTableCell35,
            this.xrTableCell3,
            this.xrTableCell6,
            this.xrTableCell32,
            this.xrTableCell4,
            this.xrTableCell84,
            this.xrTableCell7,
            this.xrTableCell82,
            this.xrTableCell8,
            this.xrTableCell33});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 11.5D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell1.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.InteractiveSorting.FieldName = "Sum";
            this.xrTableCell1.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.RowSpan = 2;
            this.xrTableCell1.StyleName = "tbl_header_style";
            this.xrTableCell1.StylePriority.UseBackColor = false;
            this.xrTableCell1.StylePriority.UseBorderColor = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.Text = "الإجمالى";
            this.xrTableCell1.Weight = 0.009742279159671784D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell37.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell37.Dpi = 254F;
            this.xrTableCell37.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell37.InteractiveSorting.FieldName = "SumComp";
            this.xrTableCell37.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.RowSpan = 2;
            this.xrTableCell37.StylePriority.UseBackColor = false;
            this.xrTableCell37.StylePriority.UseBorderColor = false;
            this.xrTableCell37.StylePriority.UseFont = false;
            this.xrTableCell37.Text = "إجمالى حصة الشركة";
            this.xrTableCell37.Weight = 0.01036807038010213D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell2.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StyleName = "tbl_header_style";
            this.xrTableCell2.StylePriority.UseBackColor = false;
            this.xrTableCell2.StylePriority.UseBorderColor = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.Text = "حصة الشركة";
            this.xrTableCell2.Weight = 0.019602684203670055D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.Dpi = 254F;
            this.xrTableCell35.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell35.InteractiveSorting.FieldName = "SumEmp";
            this.xrTableCell35.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.RowSpan = 2;
            this.xrTableCell35.StylePriority.UseFont = false;
            this.xrTableCell35.Text = "إجمالى حصة العامل";
            this.xrTableCell35.Weight = 0.010897566636886019D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.Text = "حصة العامل";
            this.xrTableCell3.Weight = 0.019501997762796743D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.Text = "الراتب التأميني";
            this.xrTableCell6.Weight = 0.019819325750999765D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.Dpi = 254F;
            this.xrTableCell32.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell32.InteractiveSorting.FieldName = "SubscripDate";
            this.xrTableCell32.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.RowSpan = 2;
            this.xrTableCell32.StylePriority.UseFont = false;
            this.xrTableCell32.Text = "تاريخ الاشتراك";
            this.xrTableCell32.Weight = 0.019642536565994368D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.RowSpan = 2;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "الرقم التأمينى";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.019051861396605505D;
            // 
            // xrTableCell84
            // 
            this.xrTableCell84.Dpi = 254F;
            this.xrTableCell84.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell84.Name = "xrTableCell84";
            this.xrTableCell84.RowSpan = 2;
            this.xrTableCell84.StylePriority.UseFont = false;
            this.xrTableCell84.Text = "ت.التعيين";
            this.xrTableCell84.Weight = 0.014669336788369129D;
            // 
            // xrTableCell82
            // 
            this.xrTableCell82.Dpi = 254F;
            this.xrTableCell82.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell82.Name = "xrTableCell82";
            this.xrTableCell82.RowSpan = 2;
            this.xrTableCell82.StylePriority.UseFont = false;
            this.xrTableCell82.Text = "الجنسية";
            this.xrTableCell82.Weight = 0.0099127591195533778D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell8.InteractiveSorting.FieldName = "EmpName";
            this.xrTableCell8.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.RowSpan = 2;
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.Text = "اسم الموظف";
            this.xrTableCell8.Weight = 0.020556398292799309D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Dpi = 254F;
            this.xrTableCell33.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.RowSpan = 2;
            this.xrTableCell33.StylePriority.UseFont = false;
            this.xrTableCell33.Text = "الكود";
            this.xrTableCell33.Weight = 0.0059686671730524825D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell38,
            this.xrTableCell19,
            this.xrTableCell28,
            this.xrTableCell20,
            this.xrTableCell36,
            this.xrTableCell29,
            this.xrTableCell21,
            this.xrTableCell14,
            this.xrTableCell10,
            this.xrTableCell22,
            this.xrTableCell23,
            this.xrTableCell85,
            this.xrTableCell16,
            this.xrTableCell83,
            this.xrTableCell26,
            this.xrTableCell41});
            this.xrTableRow3.Dpi = 254F;
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 11.5D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell38.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell38.Dpi = 254F;
            this.xrTableCell38.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.StylePriority.UseBackColor = false;
            this.xrTableCell38.StylePriority.UseBorderColor = false;
            this.xrTableCell38.StylePriority.UseFont = false;
            this.xrTableCell38.Text = "xrTableCell38";
            this.xrTableCell38.Weight = 0.011495484715024262D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell19.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell19.Dpi = 254F;
            this.xrTableCell19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell19.InteractiveSorting.FieldName = "SubscripDate";
            this.xrTableCell19.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseBackColor = false;
            this.xrTableCell19.StylePriority.UseBorderColor = false;
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.Weight = 0.0122338940970397D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell28.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell28.Dpi = 254F;
            this.xrTableCell28.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell28.InteractiveSorting.FieldName = "VarComp";
            this.xrTableCell28.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseBackColor = false;
            this.xrTableCell28.StylePriority.UseBorderColor = false;
            this.xrTableCell28.StylePriority.UseFont = false;
            this.xrTableCell28.Text = "متغير";
            this.xrTableCell28.Weight = 0.01293447725032848D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.BackColor = System.Drawing.SystemColors.ControlLight;
            this.xrTableCell20.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell20.Dpi = 254F;
            this.xrTableCell20.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell20.InteractiveSorting.FieldName = "BasicComp";
            this.xrTableCell20.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseBackColor = false;
            this.xrTableCell20.StylePriority.UseBorderColor = false;
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.Text = "ثابت";
            this.xrTableCell20.Weight = 0.010195864080030728D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.Dpi = 254F;
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Text = "~";
            this.xrTableCell36.Weight = 0.012858683506083728D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Dpi = 254F;
            this.xrTableCell29.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell29.InteractiveSorting.FieldName = "VarEmp";
            this.xrTableCell29.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.Text = "متغير";
            this.xrTableCell29.Weight = 0.010369993872186515D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Dpi = 254F;
            this.xrTableCell21.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell21.InteractiveSorting.FieldName = "BasicEmp";
            this.xrTableCell21.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.Text = "ثابت";
            this.xrTableCell21.Weight = 0.012641564631128661D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.InteractiveSorting.FieldName = "VarSubAmt";
            this.xrTableCell14.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = "متغير";
            this.xrTableCell14.Weight = 0.012028642804646779D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.InteractiveSorting.FieldName = "BasicSubAmt";
            this.xrTableCell10.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "ثابت";
            this.xrTableCell10.Weight = 0.011357340995985923D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Dpi = 254F;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Weight = 0.023177379808360349D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Dpi = 254F;
            this.xrTableCell23.InteractiveSorting.FieldName = "EmpStatus";
            this.xrTableCell23.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Weight = 0.022480408080361343D;
            // 
            // xrTableCell85
            // 
            this.xrTableCell85.Dpi = 254F;
            this.xrTableCell85.Name = "xrTableCell85";
            this.xrTableCell85.Text = "xrTableCell85";
            this.xrTableCell85.Weight = 0.017309209842431228D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Text = "xrTableCell16";
            this.xrTableCell16.Weight = 0.021629683401644027D;
            // 
            // xrTableCell83
            // 
            this.xrTableCell83.Dpi = 254F;
            this.xrTableCell83.Name = "xrTableCell83";
            this.xrTableCell83.Text = "xrTableCell83";
            this.xrTableCell83.Weight = 0.011696645177037718D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Dpi = 254F;
            this.xrTableCell26.InteractiveSorting.FieldName = "EmpName";
            this.xrTableCell26.InteractiveSorting.TargetBand = this.Detail;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Weight = 0.02425569843768495D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.Dpi = 254F;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.Text = "xrTableCell41";
            this.xrTableCell41.Weight = 0.0070427764126341722D;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.companyLogo,
            this.xrLabel4});
            this.ReportHeader.Dpi = 254F;
            this.ReportHeader.HeightF = 217.4468F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel1
            // 
            this.xrLabel1.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(788.5695F, 38.99767F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(1178.278F, 161.0023F);
            this.xrLabel1.StylePriority.UseBorderColor = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "تأمينات الموظفين";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // companyLogo
            // 
            this.companyLogo.Dpi = 254F;
            this.companyLogo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.companyLogo.Name = "companyLogo";
            this.companyLogo.SizeF = new System.Drawing.SizeF(400F, 200F);
            this.companyLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            // 
            // xrLabel4
            // 
            this.xrLabel4.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.CompanyName, "Text", "")});
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(2058.542F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(905.458F, 91.66479F);
            this.xrLabel4.StylePriority.UseBorderColor = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseForeColor = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
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
            this.xrLabel9,
            this.xrLabel12,
            this.xrPageInfo2,
            this.xrPageInfo1});
            this.PageFooter.Dpi = 254F;
            this.PageFooter.HeightF = 72.95444F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(960.1806F, 14.53443F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(137.5833F, 58.41999F);
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "المستخدم";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel12
            // 
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.User, "Text", "")});
            this.xrLabel12.Dpi = 254F;
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(706.1806F, 14.53443F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "xrLabel10";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // User
            // 
            this.User.Description = "User";
            this.User.Name = "User";
            this.User.Visible = false;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Dpi = 254F;
            this.xrPageInfo2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.Format = "{0:yyyy-MM-dd h:mm tt}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 14.53443F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(664.6506F, 58.42F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Dpi = 254F;
            this.xrPageInfo1.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.Format = "صفحة {0} من {1}";
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(2704.403F, 14.53443F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(259.5969F, 58.42F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
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
            this.tbl_header_style.BackColor = System.Drawing.Color.Silver;
            this.tbl_header_style.BorderColor = System.Drawing.Color.Black;
            this.tbl_header_style.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.tbl_header_style.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_header_style.BorderWidth = 1F;
            this.tbl_header_style.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_header_style.ForeColor = System.Drawing.Color.Black;
            this.tbl_header_style.Name = "tbl_header_style";
            this.tbl_header_style.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 5, 5, 254F);
            this.tbl_header_style.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // tbl_even_detail
            // 
            this.tbl_even_detail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tbl_even_detail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.tbl_even_detail.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.tbl_even_detail.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_even_detail.BorderWidth = 1F;
            this.tbl_even_detail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_even_detail.ForeColor = System.Drawing.Color.Black;
            this.tbl_even_detail.Name = "tbl_even_detail";
            this.tbl_even_detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 13, 13, 13, 254F);
            this.tbl_even_detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // tbl_odd_detail
            // 
            this.tbl_odd_detail.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tbl_odd_detail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.tbl_odd_detail.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.tbl_odd_detail.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tbl_odd_detail.BorderWidth = 1F;
            this.tbl_odd_detail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbl_odd_detail.ForeColor = System.Drawing.Color.Black;
            this.tbl_odd_detail.Name = "tbl_odd_detail";
            this.tbl_odd_detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 13, 13, 13, 254F);
            this.tbl_odd_detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // BasicComp
            // 
            this.BasicComp.DataMember = "SP_InsuranceEmployeeReport";
            this.BasicComp.Expression = "[BasicSubAmt]*0.26";
            this.BasicComp.FieldType = DevExpress.XtraReports.UI.FieldType.Decimal;
            this.BasicComp.Name = "BasicComp";
            // 
            // BasicEmp
            // 
            this.BasicEmp.DataMember = "SP_InsuranceEmployeeReport";
            this.BasicEmp.Expression = "[BasicSubAmt]*0.14";
            this.BasicEmp.FieldType = DevExpress.XtraReports.UI.FieldType.Decimal;
            this.BasicEmp.Name = "BasicEmp";
            // 
            // VarComp
            // 
            this.VarComp.DataMember = "SP_InsuranceEmployeeReport";
            this.VarComp.Expression = "[VarSubAmt]*0.24";
            this.VarComp.FieldType = DevExpress.XtraReports.UI.FieldType.Decimal;
            this.VarComp.Name = "VarComp";
            // 
            // VarEmp
            // 
            this.VarEmp.DataMember = "SP_InsuranceEmployeeReport";
            this.VarEmp.Expression = "[VarSubAmt]*0.11";
            this.VarEmp.FieldType = DevExpress.XtraReports.UI.FieldType.Decimal;
            this.VarEmp.Name = "VarEmp";
            // 
            // Gender
            // 
            this.Gender.Description = "النوع";
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(1, "Male"));
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(2, "Female"));
            this.Gender.LookUpSettings = staticListLookUpSettings1;
            this.Gender.MultiValue = true;
            this.Gender.Name = "Gender";
            this.Gender.Type = typeof(int);
            // 
            // MappingEmpIds
            // 
            this.MappingEmpIds.Description = "MappingEmpIds";
            this.MappingEmpIds.Name = "MappingEmpIds";
            this.MappingEmpIds.Visible = false;
            // 
            // Nationality
            // 
            this.Nationality.Description = "الجنسية";
            dynamicListLookUpSettings1.DataAdapter = null;
            dynamicListLookUpSettings1.DataMember = "SP_InsuranceEmployeeReport";
            dynamicListLookUpSettings1.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings1.DisplayMember = "NationalityAr";
            dynamicListLookUpSettings1.ValueMember = "NationalityId";
            this.Nationality.LookUpSettings = dynamicListLookUpSettings1;
            this.Nationality.MultiValue = true;
            this.Nationality.Name = "Nationality";
            this.Nationality.Type = typeof(int);
            // 
            // JobIds
            // 
            this.JobIds.Description = "الوظيفة";
            dynamicListLookUpSettings2.DataAdapter = null;
            dynamicListLookUpSettings2.DataMember = "SP_InsuranceEmployeeReport";
            dynamicListLookUpSettings2.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings2.DisplayMember = "JobName";
            dynamicListLookUpSettings2.ValueMember = "JobId";
            this.JobIds.LookUpSettings = dynamicListLookUpSettings2;
            this.JobIds.MultiValue = true;
            this.JobIds.Name = "JobIds";
            this.JobIds.Type = typeof(int);
            // 
            // mappingNationality
            // 
            this.mappingNationality.Description = "mappingNationality";
            this.mappingNationality.Name = "mappingNationality";
            this.mappingNationality.Visible = false;
            // 
            // MappingJobIds
            // 
            this.MappingJobIds.Description = "mappingJob";
            this.MappingJobIds.Name = "MappingJobIds";
            this.MappingJobIds.Visible = false;
            // 
            // mappingGender
            // 
            this.mappingGender.Description = "mappingGender";
            this.mappingGender.Name = "mappingGender";
            this.mappingGender.Visible = false;
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
            staticListLookUpSettings2.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("ar-EG", "Arabic"));
            staticListLookUpSettings2.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("en-GB", "English"));
            this.Culture.LookUpSettings = staticListLookUpSettings2;
            this.Culture.Name = "Culture";
            this.Culture.ValueInfo = "ar-EG";
            this.Culture.Visible = false;
            // 
            // Sum
            // 
            this.Sum.DataMember = "SP_InsuranceEmployeeReport";
            this.Sum.Expression = "[BasicComp]+[VarComp]+[BasicEmp]+[VarEmp]";
            this.Sum.FieldType = DevExpress.XtraReports.UI.FieldType.Decimal;
            this.Sum.Name = "Sum";
            // 
            // SumEmp
            // 
            this.SumEmp.DataMember = "SP_InsuranceEmployeeReport";
            this.SumEmp.Expression = "[VarEmp]+[BasicEmp]";
            this.SumEmp.Name = "SumEmp";
            // 
            // SumComp
            // 
            this.SumComp.DataMember = "SP_InsuranceEmployeeReport";
            this.SumComp.Expression = "[BasicComp]+[VarComp]";
            this.SumComp.Name = "SumComp";
            // 
            // MappingDeptIds
            // 
            this.MappingDeptIds.Description = "mappingDeptIds";
            this.MappingDeptIds.Name = "MappingDeptIds";
            this.MappingDeptIds.Visible = false;
            // 
            // DeptIds
            // 
            this.DeptIds.Description = " الإدارة";
            dynamicListLookUpSettings3.DataAdapter = null;
            dynamicListLookUpSettings3.DataMember = "SP_InsuranceEmployeeReport";
            dynamicListLookUpSettings3.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings3.DisplayMember = "DeptName";
            dynamicListLookUpSettings3.ValueMember = "DeptId";
            this.DeptIds.LookUpSettings = dynamicListLookUpSettings3;
            this.DeptIds.MultiValue = true;
            this.DeptIds.Name = "DeptIds";
            this.DeptIds.Type = typeof(int);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel17,
            this.xrLabel18});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("DeptName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 91.72222F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.Silver;
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.DeptName")});
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.ForeColor = System.Drawing.Color.Navy;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(2534.582F, 16.65112F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(410.4177F, 58.41999F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SP_InsuranceEmployeeReport.EmpName")});
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(2253.375F, 16.65112F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(54.68061F, 58.41999F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0}    ";
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Count;
            xrSummary1.IgnoreNullValues = true;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel2.Summary = xrSummary1;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Dpi = 254F;
            this.xrLabel17.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(2308.055F, 16.65112F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(31.74976F, 58.41999F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = ":";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrLabel18
            // 
            this.xrLabel18.Dpi = 254F;
            this.xrLabel18.Font = new System.Drawing.Font("Frutiger LT Arabic 55 Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(2329.055F, 16.65112F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(205.5271F, 58.41999F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "عدد الموظفين";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // from
            // 
            this.from.Description = "تاريخ الاشتراك من";
            this.from.Name = "from";
            this.from.Type = typeof(System.DateTime);
            this.from.ValueInfo = "1900-01-01";
            // 
            // to
            // 
            this.to.Description = "إلى";
            this.to.Name = "to";
            this.to.Type = typeof(System.DateTime);
            this.to.ValueInfo = "2099-01-01";
            // 
            // GroupingBy
            // 
            this.GroupingBy.Description = "تجميع بـ";
            staticListLookUpSettings3.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("Default", "عرض افتراضى"));
            staticListLookUpSettings3.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue("Dept", "الإدارة"));
            this.GroupingBy.LookUpSettings = staticListLookUpSettings3;
            this.GroupingBy.Name = "GroupingBy";
            this.GroupingBy.ValueInfo = "Default";
            // 
            // EmpIds
            // 
            this.EmpIds.Description = "اسم الموظف";
            dynamicListLookUpSettings4.DataAdapter = null;
            dynamicListLookUpSettings4.DataMember = "SP_InsuranceEmployeeReport";
            dynamicListLookUpSettings4.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings4.DisplayMember = "EmpName";
            dynamicListLookUpSettings4.ValueMember = "Id";
            this.EmpIds.LookUpSettings = dynamicListLookUpSettings4;
            this.EmpIds.MultiValue = true;
            this.EmpIds.Name = "EmpIds";
            this.EmpIds.Type = typeof(int);
            // 
            // insured
            // 
            this.insured.Description = "التأمين";
            staticListLookUpSettings4.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(0, "غير مؤمّن عليه"));
            staticListLookUpSettings4.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue(1, "مؤمّن عليه"));
            this.insured.LookUpSettings = staticListLookUpSettings4;
            this.insured.MultiValue = true;
            this.insured.Name = "insured";
            this.insured.Type = typeof(int);
            // 
            // MappingInsured
            // 
            this.MappingInsured.Description = "MappingInsured";
            this.MappingInsured.Name = "MappingInsured";
            this.MappingInsured.Visible = false;
            // 
            // InsuranceEmployeesReport
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
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.BasicComp,
            this.BasicEmp,
            this.VarComp,
            this.VarEmp,
            this.Sum,
            this.SumEmp,
            this.SumComp});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.DataMember = "SP_InsuranceEmployeeReport";
            this.DataSource = this.sqlDataSource1;
            this.Dpi = 254F;
            this.FilterString = "[SubscripDate] Between(?from, ?to)";
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(4, 0, 0, 0);
            this.PageHeight = 2100;
            this.PageWidth = 2970;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.EmpIds,
            this.Gender,
            this.MappingEmpIds,
            this.Nationality,
            this.DeptIds,
            this.JobIds,
            this.mappingNationality,
            this.MappingJobIds,
            this.mappingGender,
            this.insured,
            this.CompanyId,
            this.Culture,
            this.User,
            this.MappingDeptIds,
            this.CompanyName,
            this.from,
            this.to,
            this.GroupingBy,
            this.MappingInsured});
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.SnapGridSize = 25F;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Odding,
            this.GroupCaption3,
            this.Title,
            this.xrControlStyle4,
            this.xrControlStyle2,
            this.tbl_header_style,
            this.tbl_even_detail,
            this.tbl_odd_detail});
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    
}
