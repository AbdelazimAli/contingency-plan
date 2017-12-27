using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for TestPivotGrid
/// </summary>
public class TestPivotGrid : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private DevExpress.XtraReports.Parameters.Parameter parameter1;
    private DevExpress.XtraReports.Parameters.Parameter parameter2;
    private DevExpress.XtraReports.Parameters.Parameter Culture;
    private DevExpress.XtraReports.Parameters.Parameter parameter3;
    private DevExpress.XtraReports.Parameters.Parameter CompanyName;
    private DevExpress.XtraReports.Parameters.Parameter User;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource2;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField field9;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField field8;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField field7;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField field6;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField field5;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField field4;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField field3;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField field1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField field;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField field2;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldEmpName1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldGender1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldReligion1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCode1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldAssignDate1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldEndDate1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldJobName1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldJobId;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldDeptName1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldDeptId1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField2;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField3;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField4;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField5;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField6;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField7;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField8;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField9;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField10;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField11;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField12;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField13;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField14;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField15;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField16;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField17;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField18;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField19;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField20;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField21;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField22;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField23;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField24;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField25;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField26;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField27;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField28;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField29;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField30;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField31;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField32;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField33;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField34;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField35;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField36;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField37;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField38;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField39;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField40;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField41;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField42;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField43;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField44;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField45;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField46;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField47;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField48;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField49;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField50;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField51;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField52;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField53;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField54;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField55;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField56;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField57;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField58;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField59;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField60;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField61;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField62;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField63;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField64;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField65;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField66;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField67;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField68;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField69;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField70;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField71;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField72;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField73;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField74;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField75;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField76;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField77;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField78;
    private XRPivotGrid xrPivotGrid1;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource3;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField84;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField85;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldEmpId1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField86;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldjobId1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField87;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField88;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldReceiveDate1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldDeliveryDate1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fielddeliveryStatus1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldQtyOfRecievedItem1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCustodyCode1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCustodyName1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCustodyEnteringDate1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCustodyTimeOutDate1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCustodySerialNo1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCustodyPurchaseDate1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCustodyCat1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldPurchaseAmount1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldInUse1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldReceiveStatus1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldInitialCustodyAmount1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldDisposal1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCustodyCat12;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField79;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField80;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField81;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField82;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField xrPivotGridField83;


    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public TestPivotGrid()
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
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery2 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter5 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter6 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter7 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter8 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter9 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestPivotGrid));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.sqlDataSource2 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.parameter1 = new DevExpress.XtraReports.Parameters.Parameter();
            this.parameter2 = new DevExpress.XtraReports.Parameters.Parameter();
            this.Culture = new DevExpress.XtraReports.Parameters.Parameter();
            this.parameter3 = new DevExpress.XtraReports.Parameters.Parameter();
            this.CompanyName = new DevExpress.XtraReports.Parameters.Parameter();
            this.User = new DevExpress.XtraReports.Parameters.Parameter();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.field9 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.field8 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.field7 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.field6 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.field5 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.field4 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.field3 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.field1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.field = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.field2 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldEmpName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldGender1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldReligion1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCode1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldAssignDate1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldEndDate1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldJobName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldJobId = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldDeptName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldDeptId1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField2 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField3 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField4 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField5 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField6 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField7 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField8 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField9 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField10 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField11 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField12 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField13 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField14 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField15 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField16 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField17 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField18 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField19 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField20 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField21 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField22 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField23 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField24 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField25 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField26 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField27 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField28 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField29 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField30 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField31 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField32 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField33 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField34 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField35 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField36 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField37 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField38 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField39 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField40 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField41 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField42 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField43 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField44 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField45 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField46 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField47 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField48 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField49 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField50 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField51 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField52 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField53 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField54 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField55 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField56 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField57 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField58 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField59 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField60 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField61 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField62 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField63 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField64 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField65 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField66 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField67 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField68 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField69 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField70 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField71 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField72 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField73 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField74 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField75 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField76 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField77 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField78 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGrid1 = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.sqlDataSource3 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.xrPivotGridField79 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField80 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldEmpId1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField81 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldjobId1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField82 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField83 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldReceiveDate1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldDeliveryDate1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fielddeliveryStatus1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldQtyOfRecievedItem1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCustodyCode1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCustodyName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCustodyEnteringDate1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCustodyTimeOutDate1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCustodySerialNo1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCustodyPurchaseDate1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCustodyCat1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldPurchaseAmount1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldInUse1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldReceiveStatus1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldInitialCustodyAmount1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldDisposal1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCustodyCat12 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField84 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField85 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField86 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField87 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrPivotGridField88 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivotGrid1});
            this.Detail.HeightF = 220.4167F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // sqlDataSource2
            // 
            this.sqlDataSource2.ConnectionName = "HrContext";
            this.sqlDataSource2.Name = "sqlDataSource2";
            storedProcQuery2.Name = "SP_EmployeeCard";
            queryParameter5.Name = "@Culture";
            queryParameter5.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter5.Value = new DevExpress.DataAccess.Expression("[Parameters.Culture]", typeof(string));
            queryParameter6.Name = "@LoginedCompany";
            queryParameter6.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter6.Value = new DevExpress.DataAccess.Expression("[Parameters.parameter3]", typeof(int));
            queryParameter7.Name = "@EmpIds";
            queryParameter7.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter7.Value = new DevExpress.DataAccess.Expression("[Parameters.parameter1]", typeof(string));
            queryParameter8.Name = "@DeptIds";
            queryParameter8.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter8.Value = new DevExpress.DataAccess.Expression("[Parameters.parameter2]", typeof(string));
            queryParameter9.Name = "@JobId";
            queryParameter9.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter9.Value = new DevExpress.DataAccess.Expression("[Parameters.parameter1]", typeof(string));
            storedProcQuery2.Parameters.Add(queryParameter5);
            storedProcQuery2.Parameters.Add(queryParameter6);
            storedProcQuery2.Parameters.Add(queryParameter7);
            storedProcQuery2.Parameters.Add(queryParameter8);
            storedProcQuery2.Parameters.Add(queryParameter9);
            storedProcQuery2.StoredProcName = "SP_EmployeeCard";
            this.sqlDataSource2.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery2});
            this.sqlDataSource2.ResultSchemaSerializable = resources.GetString("sqlDataSource2.ResultSchemaSerializable");
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 100F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // parameter1
            // 
            this.parameter1.Description = "Parameter1";
            this.parameter1.Name = "parameter1";
            this.parameter1.Visible = false;
            // 
            // parameter2
            // 
            this.parameter2.Description = "Parameter2";
            this.parameter2.Name = "parameter2";
            this.parameter2.Visible = false;
            // 
            // Culture
            // 
            this.Culture.Description = "Culture";
            this.Culture.Name = "Culture";
            this.Culture.ValueInfo = "ar-EG";
            // 
            // parameter3
            // 
            this.parameter3.Description = "Parameter3";
            this.parameter3.Name = "parameter3";
            this.parameter3.Type = typeof(int);
            this.parameter3.ValueInfo = "0";
            // 
            // CompanyName
            // 
            this.CompanyName.Description = "Parameter4";
            this.CompanyName.Name = "CompanyName";
            this.CompanyName.Visible = false;
            // 
            // User
            // 
            this.User.Description = "Parameter4";
            this.User.Name = "User";
            this.User.Visible = false;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "HrContext";
            this.sqlDataSource1.Name = "sqlDataSource1";
            customSqlQuery1.Name = "Query";
            customSqlQuery1.Sql = resources.GetString("customSqlQuery1.Sql");
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // field9
            // 
            this.field9.AreaIndex = 9;
            this.field9.Name = "field9";
            this.field9.Options.AllowDrag = DevExpress.Utils.DefaultBoolean.True;
            this.field9.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.True;
            this.field9.Options.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.field9.Options.ShowCustomTotals = false;
            this.field9.Options.ShowGrandTotal = false;
            this.field9.Options.ShowTotals = false;
            // 
            // field8
            // 
            this.field8.AreaIndex = 8;
            this.field8.Name = "field8";
            // 
            // field7
            // 
            this.field7.AreaIndex = 7;
            this.field7.Name = "field7";
            this.field7.Options.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.field7.Options.AllowSortBySummary = DevExpress.Utils.DefaultBoolean.False;
            this.field7.Options.ShowTotals = false;
            // 
            // field6
            // 
            this.field6.AreaIndex = 6;
            this.field6.Name = "field6";
            // 
            // field5
            // 
            this.field5.AreaIndex = 5;
            this.field5.Name = "field5";
            this.field5.Options.ShowGrandTotal = false;
            this.field5.Options.ShowTotals = false;
            // 
            // field4
            // 
            this.field4.AreaIndex = 4;
            this.field4.Name = "field4";
            // 
            // field3
            // 
            this.field3.AreaIndex = 3;
            this.field3.Name = "field3";
            this.field3.Options.ShowTotals = false;
            // 
            // field1
            // 
            this.field1.AreaIndex = 1;
            this.field1.Name = "field1";
            this.field1.Options.ShowTotals = false;
            // 
            // field
            // 
            this.field.AreaIndex = 0;
            this.field.Name = "field";
            // 
            // field2
            // 
            this.field2.AreaIndex = 2;
            this.field2.Name = "field2";
            // 
            // fieldEmpName1
            // 
            this.fieldEmpName1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldEmpName1.AreaIndex = 0;
            this.fieldEmpName1.AutoPopulatedProperties = new string[0];
            this.fieldEmpName1.FieldName = "EmpName";
            this.fieldEmpName1.KPIGraphic = DevExpress.XtraPivotGrid.PivotKPIGraphic.Shapes;
            this.fieldEmpName1.Name = "fieldEmpName1";
            this.fieldEmpName1.Options.AllowDrag = DevExpress.Utils.DefaultBoolean.True;
            this.fieldEmpName1.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.True;
            this.fieldEmpName1.Options.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.fieldEmpName1.Options.ShowCustomTotals = false;
            this.fieldEmpName1.Options.ShowGrandTotal = false;
            this.fieldEmpName1.Options.ShowTotals = false;
            this.fieldEmpName1.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Count;
            this.fieldEmpName1.Tag = "here";
            this.fieldEmpName1.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.fieldEmpName1.Width = 313;
            // 
            // fieldGender1
            // 
            this.fieldGender1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldGender1.AreaIndex = 4;
            this.fieldGender1.AutoPopulatedProperties = new string[0];
            this.fieldGender1.FieldName = "Gender";
            this.fieldGender1.KPIGraphic = DevExpress.XtraPivotGrid.PivotKPIGraphic.Shapes;
            this.fieldGender1.Name = "fieldGender1";
            this.fieldGender1.Tag = true;
            this.fieldGender1.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldGender1.UnboundExpression = "Iif([Religion] = 1, \'male\', \'female\')";
            this.fieldGender1.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.fieldGender1.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // fieldReligion1
            // 
            this.fieldReligion1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldReligion1.AreaIndex = 1;
            this.fieldReligion1.FieldName = "Religion";
            this.fieldReligion1.KPIGraphic = DevExpress.XtraPivotGrid.PivotKPIGraphic.RoadSigns;
            this.fieldReligion1.Name = "fieldReligion1";
            this.fieldReligion1.Options.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.fieldReligion1.Options.AllowSortBySummary = DevExpress.Utils.DefaultBoolean.False;
            this.fieldReligion1.Options.ShowTotals = false;
            this.fieldReligion1.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            // 
            // fieldCode1
            // 
            this.fieldCode1.AreaIndex = 0;
            this.fieldCode1.FieldName = "Code";
            this.fieldCode1.Name = "fieldCode1";
            this.fieldCode1.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            // 
            // fieldAssignDate1
            // 
            this.fieldAssignDate1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldAssignDate1.AreaIndex = 2;
            this.fieldAssignDate1.FieldName = "AssignDate";
            this.fieldAssignDate1.Name = "fieldAssignDate1";
            this.fieldAssignDate1.Options.ShowGrandTotal = false;
            this.fieldAssignDate1.Options.ShowTotals = false;
            this.fieldAssignDate1.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.fieldAssignDate1.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            // 
            // fieldEndDate1
            // 
            this.fieldEndDate1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldEndDate1.AreaIndex = 3;
            this.fieldEndDate1.FieldName = "EndDate";
            this.fieldEndDate1.Name = "fieldEndDate1";
            this.fieldEndDate1.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            // 
            // fieldJobName1
            // 
            this.fieldJobName1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldJobName1.AreaIndex = 0;
            this.fieldJobName1.AutoPopulatedProperties = new string[0];
            this.fieldJobName1.FieldName = "JobName";
            this.fieldJobName1.Name = "fieldJobName1";
            this.fieldJobName1.Options.ShowTotals = false;
            this.fieldJobName1.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldJobName1.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.fieldJobName1.UseNativeFormat = DevExpress.Utils.DefaultBoolean.True;
            this.fieldJobName1.Width = 168;
            // 
            // fieldJobId
            // 
            this.fieldJobId.AreaIndex = 3;
            this.fieldJobId.FieldName = "JobId";
            this.fieldJobId.Name = "fieldJobId";
            this.fieldJobId.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Count;
            this.fieldJobId.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldJobId.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            // 
            // fieldDeptName1
            // 
            this.fieldDeptName1.AreaIndex = 2;
            this.fieldDeptName1.FieldName = "DeptName";
            this.fieldDeptName1.Name = "fieldDeptName1";
            this.fieldDeptName1.Options.ShowTotals = false;
            this.fieldDeptName1.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            // 
            // fieldDeptId1
            // 
            this.fieldDeptId1.AreaIndex = 1;
            this.fieldDeptId1.FieldName = "DeptId";
            this.fieldDeptId1.Name = "fieldDeptId1";
            this.fieldDeptId1.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            // 
            // xrPivotGridField1
            // 
            this.xrPivotGridField1.AreaIndex = 0;
            this.xrPivotGridField1.FieldName = "EmpName";
            this.xrPivotGridField1.Name = "xrPivotGridField1";
            this.xrPivotGridField1.Options.ShowCustomTotals = false;
            this.xrPivotGridField1.Options.ShowGrandTotal = false;
            this.xrPivotGridField1.Options.ShowTotals = false;
            this.xrPivotGridField1.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField1.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            // 
            // xrPivotGridField2
            // 
            this.xrPivotGridField2.AreaIndex = 1;
            this.xrPivotGridField2.FieldName = "Gender";
            this.xrPivotGridField2.Name = "xrPivotGridField2";
            // 
            // xrPivotGridField3
            // 
            this.xrPivotGridField3.AreaIndex = 2;
            this.xrPivotGridField3.FieldName = "Religion";
            this.xrPivotGridField3.Name = "xrPivotGridField3";
            // 
            // xrPivotGridField4
            // 
            this.xrPivotGridField4.AreaIndex = 3;
            this.xrPivotGridField4.FieldName = "Code";
            this.xrPivotGridField4.Name = "xrPivotGridField4";
            // 
            // xrPivotGridField5
            // 
            this.xrPivotGridField5.AreaIndex = 4;
            this.xrPivotGridField5.FieldName = "AssignDate";
            this.xrPivotGridField5.Name = "xrPivotGridField5";
            // 
            // xrPivotGridField6
            // 
            this.xrPivotGridField6.AreaIndex = 5;
            this.xrPivotGridField6.FieldName = "EndDate";
            this.xrPivotGridField6.Name = "xrPivotGridField6";
            // 
            // xrPivotGridField7
            // 
            this.xrPivotGridField7.AreaIndex = 6;
            this.xrPivotGridField7.FieldName = "JobName";
            this.xrPivotGridField7.Name = "xrPivotGridField7";
            // 
            // xrPivotGridField8
            // 
            this.xrPivotGridField8.AreaIndex = 8;
            this.xrPivotGridField8.FieldName = "DeptName";
            this.xrPivotGridField8.Name = "xrPivotGridField8";
            // 
            // xrPivotGridField9
            // 
            this.xrPivotGridField9.AreaIndex = 9;
            this.xrPivotGridField9.FieldName = "DeptId";
            this.xrPivotGridField9.Name = "xrPivotGridField9";
            // 
            // xrPivotGridField10
            // 
            this.xrPivotGridField10.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField10.AreaIndex = 0;
            this.xrPivotGridField10.FieldName = "EmpName";
            this.xrPivotGridField10.Name = "xrPivotGridField10";
            this.xrPivotGridField10.Options.ShowCustomTotals = false;
            this.xrPivotGridField10.Options.ShowGrandTotal = false;
            this.xrPivotGridField10.Options.ShowTotals = false;
            this.xrPivotGridField10.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField10.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.xrPivotGridField10.Width = 200;
            // 
            // xrPivotGridField11
            // 
            this.xrPivotGridField11.AreaIndex = 0;
            this.xrPivotGridField11.FieldName = "Gender";
            this.xrPivotGridField11.Name = "xrPivotGridField11";
            // 
            // xrPivotGridField12
            // 
            this.xrPivotGridField12.AreaIndex = 1;
            this.xrPivotGridField12.FieldName = "Religion";
            this.xrPivotGridField12.Name = "xrPivotGridField12";
            // 
            // xrPivotGridField13
            // 
            this.xrPivotGridField13.AreaIndex = 2;
            this.xrPivotGridField13.FieldName = "Code";
            this.xrPivotGridField13.Name = "xrPivotGridField13";
            // 
            // xrPivotGridField14
            // 
            this.xrPivotGridField14.AreaIndex = 3;
            this.xrPivotGridField14.FieldName = "AssignDate";
            this.xrPivotGridField14.Name = "xrPivotGridField14";
            // 
            // xrPivotGridField15
            // 
            this.xrPivotGridField15.AreaIndex = 4;
            this.xrPivotGridField15.FieldName = "EndDate";
            this.xrPivotGridField15.Name = "xrPivotGridField15";
            // 
            // xrPivotGridField16
            // 
            this.xrPivotGridField16.AreaIndex = 5;
            this.xrPivotGridField16.FieldName = "JobName";
            this.xrPivotGridField16.Name = "xrPivotGridField16";
            // 
            // xrPivotGridField17
            // 
            this.xrPivotGridField17.AreaIndex = 7;
            this.xrPivotGridField17.FieldName = "DeptName";
            this.xrPivotGridField17.Name = "xrPivotGridField17";
            // 
            // xrPivotGridField18
            // 
            this.xrPivotGridField18.AreaIndex = 8;
            this.xrPivotGridField18.FieldName = "DeptId";
            this.xrPivotGridField18.Name = "xrPivotGridField18";
            // 
            // xrPivotGridField19
            // 
            this.xrPivotGridField19.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField19.AreaIndex = 0;
            this.xrPivotGridField19.FieldName = "EmpName";
            this.xrPivotGridField19.Name = "xrPivotGridField19";
            this.xrPivotGridField19.Options.ShowCustomTotals = false;
            this.xrPivotGridField19.Options.ShowGrandTotal = false;
            this.xrPivotGridField19.Options.ShowTotals = false;
            this.xrPivotGridField19.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField19.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.xrPivotGridField19.Width = 200;
            // 
            // xrPivotGridField20
            // 
            this.xrPivotGridField20.AreaIndex = 0;
            this.xrPivotGridField20.FieldName = "Gender";
            this.xrPivotGridField20.Name = "xrPivotGridField20";
            // 
            // xrPivotGridField21
            // 
            this.xrPivotGridField21.AreaIndex = 1;
            this.xrPivotGridField21.FieldName = "Religion";
            this.xrPivotGridField21.Name = "xrPivotGridField21";
            // 
            // xrPivotGridField22
            // 
            this.xrPivotGridField22.AreaIndex = 2;
            this.xrPivotGridField22.FieldName = "Code";
            this.xrPivotGridField22.Name = "xrPivotGridField22";
            // 
            // xrPivotGridField23
            // 
            this.xrPivotGridField23.AreaIndex = 3;
            this.xrPivotGridField23.FieldName = "AssignDate";
            this.xrPivotGridField23.Name = "xrPivotGridField23";
            // 
            // xrPivotGridField24
            // 
            this.xrPivotGridField24.AreaIndex = 4;
            this.xrPivotGridField24.FieldName = "EndDate";
            this.xrPivotGridField24.Name = "xrPivotGridField24";
            // 
            // xrPivotGridField25
            // 
            this.xrPivotGridField25.AreaIndex = 5;
            this.xrPivotGridField25.FieldName = "JobName";
            this.xrPivotGridField25.Name = "xrPivotGridField25";
            this.xrPivotGridField25.Options.ShowCustomTotals = false;
            this.xrPivotGridField25.Options.ShowGrandTotal = false;
            this.xrPivotGridField25.Options.ShowTotals = false;
            this.xrPivotGridField25.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField25.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            // 
            // xrPivotGridField26
            // 
            this.xrPivotGridField26.AreaIndex = 7;
            this.xrPivotGridField26.FieldName = "DeptName";
            this.xrPivotGridField26.Name = "xrPivotGridField26";
            // 
            // xrPivotGridField27
            // 
            this.xrPivotGridField27.AreaIndex = 8;
            this.xrPivotGridField27.FieldName = "DeptId";
            this.xrPivotGridField27.Name = "xrPivotGridField27";
            // 
            // xrPivotGridField28
            // 
            this.xrPivotGridField28.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField28.AreaIndex = 0;
            this.xrPivotGridField28.FieldName = "EmpName";
            this.xrPivotGridField28.Name = "xrPivotGridField28";
            this.xrPivotGridField28.Options.ShowCustomTotals = false;
            this.xrPivotGridField28.Options.ShowGrandTotal = false;
            this.xrPivotGridField28.Options.ShowTotals = false;
            this.xrPivotGridField28.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField28.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.xrPivotGridField28.Width = 200;
            // 
            // xrPivotGridField29
            // 
            this.xrPivotGridField29.AreaIndex = 7;
            this.xrPivotGridField29.FieldName = "Gender";
            this.xrPivotGridField29.Name = "xrPivotGridField29";
            // 
            // xrPivotGridField30
            // 
            this.xrPivotGridField30.AreaIndex = 0;
            this.xrPivotGridField30.FieldName = "Religion";
            this.xrPivotGridField30.Name = "xrPivotGridField30";
            // 
            // xrPivotGridField31
            // 
            this.xrPivotGridField31.AreaIndex = 1;
            this.xrPivotGridField31.FieldName = "Code";
            this.xrPivotGridField31.Name = "xrPivotGridField31";
            // 
            // xrPivotGridField32
            // 
            this.xrPivotGridField32.AreaIndex = 6;
            this.xrPivotGridField32.FieldName = "AssignDate";
            this.xrPivotGridField32.Name = "xrPivotGridField32";
            // 
            // xrPivotGridField33
            // 
            this.xrPivotGridField33.AreaIndex = 2;
            this.xrPivotGridField33.FieldName = "EndDate";
            this.xrPivotGridField33.Name = "xrPivotGridField33";
            // 
            // xrPivotGridField34
            // 
            this.xrPivotGridField34.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField34.AreaIndex = 0;
            this.xrPivotGridField34.FieldName = "JobName";
            this.xrPivotGridField34.Name = "xrPivotGridField34";
            this.xrPivotGridField34.Options.ShowCustomTotals = false;
            this.xrPivotGridField34.Options.ShowGrandTotal = false;
            this.xrPivotGridField34.Options.ShowTotals = false;
            this.xrPivotGridField34.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField34.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.xrPivotGridField34.Width = 193;
            // 
            // xrPivotGridField35
            // 
            this.xrPivotGridField35.AreaIndex = 4;
            this.xrPivotGridField35.FieldName = "DeptName";
            this.xrPivotGridField35.Name = "xrPivotGridField35";
            // 
            // xrPivotGridField36
            // 
            this.xrPivotGridField36.AreaIndex = 5;
            this.xrPivotGridField36.FieldName = "DeptId";
            this.xrPivotGridField36.Name = "xrPivotGridField36";
            // 
            // xrPivotGridField37
            // 
            this.xrPivotGridField37.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField37.AreaIndex = 0;
            this.xrPivotGridField37.FieldName = "EmpName";
            this.xrPivotGridField37.Name = "xrPivotGridField37";
            this.xrPivotGridField37.Options.ShowCustomTotals = false;
            this.xrPivotGridField37.Options.ShowGrandTotal = false;
            this.xrPivotGridField37.Options.ShowTotals = false;
            this.xrPivotGridField37.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField37.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.xrPivotGridField37.Width = 200;
            // 
            // xrPivotGridField38
            // 
            this.xrPivotGridField38.AreaIndex = 5;
            this.xrPivotGridField38.FieldName = "Gender";
            this.xrPivotGridField38.Name = "xrPivotGridField38";
            // 
            // xrPivotGridField39
            // 
            this.xrPivotGridField39.AreaIndex = 0;
            this.xrPivotGridField39.FieldName = "Religion";
            this.xrPivotGridField39.Name = "xrPivotGridField39";
            // 
            // xrPivotGridField40
            // 
            this.xrPivotGridField40.AreaIndex = 1;
            this.xrPivotGridField40.FieldName = "Code";
            this.xrPivotGridField40.Name = "xrPivotGridField40";
            // 
            // xrPivotGridField41
            // 
            this.xrPivotGridField41.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField41.AreaIndex = 1;
            this.xrPivotGridField41.FieldName = "AssignDate";
            this.xrPivotGridField41.Name = "xrPivotGridField41";
            // 
            // xrPivotGridField42
            // 
            this.xrPivotGridField42.AreaIndex = 2;
            this.xrPivotGridField42.FieldName = "EndDate";
            this.xrPivotGridField42.Name = "xrPivotGridField42";
            // 
            // xrPivotGridField43
            // 
            this.xrPivotGridField43.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField43.AreaIndex = 0;
            this.xrPivotGridField43.FieldName = "JobName";
            this.xrPivotGridField43.Name = "xrPivotGridField43";
            this.xrPivotGridField43.Options.ShowCustomTotals = false;
            this.xrPivotGridField43.Options.ShowGrandTotal = false;
            this.xrPivotGridField43.Options.ShowTotals = false;
            this.xrPivotGridField43.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField43.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.xrPivotGridField43.Width = 193;
            // 
            // xrPivotGridField44
            // 
            this.xrPivotGridField44.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField44.AreaIndex = 0;
            this.xrPivotGridField44.FieldName = "DeptName";
            this.xrPivotGridField44.Name = "xrPivotGridField44";
            // 
            // xrPivotGridField45
            // 
            this.xrPivotGridField45.AreaIndex = 4;
            this.xrPivotGridField45.FieldName = "DeptId";
            this.xrPivotGridField45.Name = "xrPivotGridField45";
            // 
            // xrPivotGridField46
            // 
            this.xrPivotGridField46.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField46.AreaIndex = 0;
            this.xrPivotGridField46.FieldName = "EmpName";
            this.xrPivotGridField46.Name = "xrPivotGridField46";
            this.xrPivotGridField46.Options.ShowCustomTotals = false;
            this.xrPivotGridField46.Options.ShowGrandTotal = false;
            this.xrPivotGridField46.Options.ShowTotals = false;
            this.xrPivotGridField46.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField46.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.xrPivotGridField46.Width = 200;
            // 
            // xrPivotGridField47
            // 
            this.xrPivotGridField47.AreaIndex = 5;
            this.xrPivotGridField47.FieldName = "Gender";
            this.xrPivotGridField47.Name = "xrPivotGridField47";
            // 
            // xrPivotGridField48
            // 
            this.xrPivotGridField48.AreaIndex = 0;
            this.xrPivotGridField48.FieldName = "Religion";
            this.xrPivotGridField48.Name = "xrPivotGridField48";
            // 
            // xrPivotGridField49
            // 
            this.xrPivotGridField49.AreaIndex = 1;
            this.xrPivotGridField49.FieldName = "Code";
            this.xrPivotGridField49.Name = "xrPivotGridField49";
            // 
            // xrPivotGridField50
            // 
            this.xrPivotGridField50.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField50.AreaIndex = 1;
            this.xrPivotGridField50.FieldName = "AssignDate";
            this.xrPivotGridField50.Name = "xrPivotGridField50";
            // 
            // xrPivotGridField51
            // 
            this.xrPivotGridField51.AreaIndex = 2;
            this.xrPivotGridField51.FieldName = "EndDate";
            this.xrPivotGridField51.Name = "xrPivotGridField51";
            // 
            // xrPivotGridField52
            // 
            this.xrPivotGridField52.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField52.AreaIndex = 0;
            this.xrPivotGridField52.FieldName = "JobName";
            this.xrPivotGridField52.Name = "xrPivotGridField52";
            this.xrPivotGridField52.Options.ShowCustomTotals = false;
            this.xrPivotGridField52.Options.ShowGrandTotal = false;
            this.xrPivotGridField52.Options.ShowTotals = false;
            this.xrPivotGridField52.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField52.UnboundExpressionMode = DevExpress.XtraPivotGrid.UnboundExpressionMode.DataSource;
            this.xrPivotGridField52.Width = 193;
            // 
            // xrPivotGridField53
            // 
            this.xrPivotGridField53.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField53.AreaIndex = 0;
            this.xrPivotGridField53.FieldName = "DeptName";
            this.xrPivotGridField53.Name = "xrPivotGridField53";
            // 
            // xrPivotGridField54
            // 
            this.xrPivotGridField54.AreaIndex = 4;
            this.xrPivotGridField54.FieldName = "DeptId";
            this.xrPivotGridField54.Name = "xrPivotGridField54";
            // 
            // xrPivotGridField55
            // 
            this.xrPivotGridField55.AreaIndex = 0;
            this.xrPivotGridField55.FieldName = "EmpName";
            this.xrPivotGridField55.Name = "xrPivotGridField55";
            // 
            // xrPivotGridField56
            // 
            this.xrPivotGridField56.AreaIndex = 1;
            this.xrPivotGridField56.FieldName = "Gender";
            this.xrPivotGridField56.Name = "xrPivotGridField56";
            // 
            // xrPivotGridField57
            // 
            this.xrPivotGridField57.AreaIndex = 8;
            this.xrPivotGridField57.FieldName = "Code";
            this.xrPivotGridField57.Name = "xrPivotGridField57";
            // 
            // xrPivotGridField58
            // 
            this.xrPivotGridField58.AreaIndex = 11;
            this.xrPivotGridField58.FieldName = "JobName";
            this.xrPivotGridField58.Name = "xrPivotGridField58";
            // 
            // xrPivotGridField59
            // 
            this.xrPivotGridField59.AreaIndex = 13;
            this.xrPivotGridField59.FieldName = "deptName";
            this.xrPivotGridField59.Name = "xrPivotGridField59";
            // 
            // xrPivotGridField60
            // 
            this.xrPivotGridField60.AreaIndex = 14;
            this.xrPivotGridField60.FieldName = "deptId";
            this.xrPivotGridField60.Name = "xrPivotGridField60";
            // 
            // xrPivotGridField61
            // 
            this.xrPivotGridField61.AreaIndex = 0;
            this.xrPivotGridField61.FieldName = "EmpName";
            this.xrPivotGridField61.Name = "xrPivotGridField61";
            this.xrPivotGridField61.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField61.Visible = false;
            // 
            // xrPivotGridField62
            // 
            this.xrPivotGridField62.AreaIndex = 0;
            this.xrPivotGridField62.FieldName = "Gender";
            this.xrPivotGridField62.Name = "xrPivotGridField62";
            this.xrPivotGridField62.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField62.Visible = false;
            // 
            // xrPivotGridField63
            // 
            this.xrPivotGridField63.AreaIndex = 0;
            this.xrPivotGridField63.FieldName = "Code";
            this.xrPivotGridField63.Name = "xrPivotGridField63";
            this.xrPivotGridField63.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField63.Visible = false;
            // 
            // xrPivotGridField64
            // 
            this.xrPivotGridField64.AreaIndex = 0;
            this.xrPivotGridField64.FieldName = "JobName";
            this.xrPivotGridField64.Name = "xrPivotGridField64";
            this.xrPivotGridField64.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField64.Visible = false;
            // 
            // xrPivotGridField65
            // 
            this.xrPivotGridField65.AreaIndex = 0;
            this.xrPivotGridField65.FieldName = "deptName";
            this.xrPivotGridField65.Name = "xrPivotGridField65";
            this.xrPivotGridField65.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField65.Visible = false;
            // 
            // xrPivotGridField66
            // 
            this.xrPivotGridField66.AreaIndex = 0;
            this.xrPivotGridField66.FieldName = "deptId";
            this.xrPivotGridField66.Name = "xrPivotGridField66";
            this.xrPivotGridField66.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField66.Visible = false;
            // 
            // xrPivotGridField67
            // 
            this.xrPivotGridField67.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField67.AreaIndex = 2;
            this.xrPivotGridField67.FieldName = "EmpName";
            this.xrPivotGridField67.Name = "xrPivotGridField67";
            this.xrPivotGridField67.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField67.Width = 92;
            // 
            // xrPivotGridField68
            // 
            this.xrPivotGridField68.AreaIndex = 8;
            this.xrPivotGridField68.FieldName = "Gender";
            this.xrPivotGridField68.Name = "xrPivotGridField68";
            this.xrPivotGridField68.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGridField69
            // 
            this.xrPivotGridField69.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField69.AreaIndex = 1;
            this.xrPivotGridField69.FieldName = "Code";
            this.xrPivotGridField69.Name = "xrPivotGridField69";
            this.xrPivotGridField69.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGridField70
            // 
            this.xrPivotGridField70.AreaIndex = 9;
            this.xrPivotGridField70.FieldName = "JobName";
            this.xrPivotGridField70.Name = "xrPivotGridField70";
            this.xrPivotGridField70.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGridField71
            // 
            this.xrPivotGridField71.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField71.AreaIndex = 0;
            this.xrPivotGridField71.FieldName = "deptName";
            this.xrPivotGridField71.Name = "xrPivotGridField71";
            this.xrPivotGridField71.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGridField72
            // 
            this.xrPivotGridField72.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField72.AreaIndex = 0;
            this.xrPivotGridField72.FieldName = "deptId";
            this.xrPivotGridField72.Name = "xrPivotGridField72";
            this.xrPivotGridField72.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGridField73
            // 
            this.xrPivotGridField73.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField73.AreaIndex = 2;
            this.xrPivotGridField73.FieldName = "EmpName";
            this.xrPivotGridField73.Name = "xrPivotGridField73";
            this.xrPivotGridField73.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.xrPivotGridField73.Width = 92;
            // 
            // xrPivotGridField74
            // 
            this.xrPivotGridField74.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField74.AreaIndex = 2;
            this.xrPivotGridField74.FieldName = "Gender";
            this.xrPivotGridField74.KPIGraphic = DevExpress.XtraPivotGrid.PivotKPIGraphic.Shapes;
            this.xrPivotGridField74.Name = "xrPivotGridField74";
            this.xrPivotGridField74.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGridField75
            // 
            this.xrPivotGridField75.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.xrPivotGridField75.AreaIndex = 1;
            this.xrPivotGridField75.FieldName = "Code";
            this.xrPivotGridField75.Name = "xrPivotGridField75";
            this.xrPivotGridField75.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGridField76
            // 
            this.xrPivotGridField76.AreaIndex = 8;
            this.xrPivotGridField76.FieldName = "JobName";
            this.xrPivotGridField76.Name = "xrPivotGridField76";
            this.xrPivotGridField76.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGridField77
            // 
            this.xrPivotGridField77.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField77.AreaIndex = 0;
            this.xrPivotGridField77.FieldName = "deptName";
            this.xrPivotGridField77.Name = "xrPivotGridField77";
            this.xrPivotGridField77.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGridField78
            // 
            this.xrPivotGridField78.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.xrPivotGridField78.AreaIndex = 0;
            this.xrPivotGridField78.FieldName = "deptId";
            this.xrPivotGridField78.Name = "xrPivotGridField78";
            this.xrPivotGridField78.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGrid1
            // 
            this.xrPivotGrid1.Appearance.Cell.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrPivotGrid1.Appearance.CustomTotalCell.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrPivotGrid1.Appearance.FieldHeader.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrPivotGrid1.Appearance.FieldValue.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrPivotGrid1.Appearance.FieldValueGrandTotal.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrPivotGrid1.Appearance.FieldValueTotal.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrPivotGrid1.Appearance.GrandTotalCell.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrPivotGrid1.Appearance.Lines.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrPivotGrid1.Appearance.TotalCell.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrPivotGrid1.DataMember = "SP_EmployeeCustody";
            this.xrPivotGrid1.DataSource = this.sqlDataSource3;
            this.xrPivotGrid1.Fields.AddRange(new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField[] {
            this.xrPivotGridField84,
            this.xrPivotGridField85,
            this.fieldEmpId1,
            this.xrPivotGridField86,
            this.fieldjobId1,
            this.xrPivotGridField87,
            this.xrPivotGridField88,
            this.fieldReceiveDate1,
            this.fieldDeliveryDate1,
            this.fielddeliveryStatus1,
            this.fieldQtyOfRecievedItem1,
            this.fieldCustodyCode1,
            this.fieldCustodyName1,
            this.fieldCustodyEnteringDate1,
            this.fieldCustodyTimeOutDate1,
            this.fieldCustodySerialNo1,
            this.fieldCustodyPurchaseDate1,
            this.fieldCustodyCat1,
            this.fieldPurchaseAmount1,
            this.fieldInUse1,
            this.fieldReceiveStatus1,
            this.fieldInitialCustodyAmount1,
            this.fieldDisposal1,
            this.fieldCustodyCat12});
            this.xrPivotGrid1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPivotGrid1.Name = "xrPivotGrid1";
            this.xrPivotGrid1.OLAPConnectionString = "";
            this.xrPivotGrid1.OptionsPrint.FilterSeparatorBarPadding = 3;
            this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(569.1666F, 190.2778F);
            this.xrPivotGrid1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrPivotGrid1_BeforePrint);
            // 
            // sqlDataSource3
            // 
            this.sqlDataSource3.ConnectionName = "HrContext";
            this.sqlDataSource3.Name = "sqlDataSource3";
            storedProcQuery1.Name = "SP_EmployeeCustody";
            queryParameter1.Name = "@Culture";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("ar-EG", typeof(string));
            queryParameter2.Name = "@LoginCompanyId";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("0", typeof(string));
            queryParameter3.Name = "@deptName";
            queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter3.Value = new DevExpress.DataAccess.Expression("[Parameters.parameter1]", typeof(string));
            queryParameter4.Name = "@empIds";
            queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter4.Value = new DevExpress.DataAccess.Expression("[Parameters.parameter2]", typeof(string));
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.StoredProcName = "SP_EmployeeCustody";
            this.sqlDataSource3.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.sqlDataSource3.ResultSchemaSerializable = resources.GetString("sqlDataSource3.ResultSchemaSerializable");
            // 
            // xrPivotGridField79
            // 
            this.xrPivotGridField79.AreaIndex = 0;
            this.xrPivotGridField79.FieldName = "EmpName";
            this.xrPivotGridField79.Name = "xrPivotGridField79";
            // 
            // xrPivotGridField80
            // 
            this.xrPivotGridField80.AreaIndex = 1;
            this.xrPivotGridField80.FieldName = "Code";
            this.xrPivotGridField80.Name = "xrPivotGridField80";
            this.xrPivotGridField80.Options.ShowCustomTotals = false;
            this.xrPivotGridField80.Options.ShowGrandTotal = false;
            this.xrPivotGridField80.Options.ShowTotals = false;
            this.xrPivotGridField80.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // fieldEmpId1
            // 
            this.fieldEmpId1.AreaIndex = 0;
            this.fieldEmpId1.FieldName = "EmpId";
            this.fieldEmpId1.Name = "fieldEmpId1";
            // 
            // xrPivotGridField81
            // 
            this.xrPivotGridField81.AreaIndex = 3;
            this.xrPivotGridField81.FieldName = "jobName";
            this.xrPivotGridField81.Name = "xrPivotGridField81";
            // 
            // fieldjobId1
            // 
            this.fieldjobId1.AreaIndex = 1;
            this.fieldjobId1.FieldName = "jobId";
            this.fieldjobId1.Name = "fieldjobId1";
            // 
            // xrPivotGridField82
            // 
            this.xrPivotGridField82.AreaIndex = 5;
            this.xrPivotGridField82.FieldName = "deptName";
            this.xrPivotGridField82.Name = "xrPivotGridField82";
            // 
            // xrPivotGridField83
            // 
            this.xrPivotGridField83.AreaIndex = 6;
            this.xrPivotGridField83.FieldName = "deptId";
            this.xrPivotGridField83.Name = "xrPivotGridField83";
            // 
            // fieldReceiveDate1
            // 
            this.fieldReceiveDate1.AreaIndex = 4;
            this.fieldReceiveDate1.FieldName = "ReceiveDate";
            this.fieldReceiveDate1.Name = "fieldReceiveDate1";
            // 
            // fieldDeliveryDate1
            // 
            this.fieldDeliveryDate1.AreaIndex = 5;
            this.fieldDeliveryDate1.FieldName = "DeliveryDate";
            this.fieldDeliveryDate1.Name = "fieldDeliveryDate1";
            // 
            // fielddeliveryStatus1
            // 
            this.fielddeliveryStatus1.AreaIndex = 6;
            this.fielddeliveryStatus1.FieldName = "deliveryStatus";
            this.fielddeliveryStatus1.Name = "fielddeliveryStatus1";
            // 
            // fieldQtyOfRecievedItem1
            // 
            this.fieldQtyOfRecievedItem1.AreaIndex = 7;
            this.fieldQtyOfRecievedItem1.FieldName = "QtyOfRecievedItem";
            this.fieldQtyOfRecievedItem1.Name = "fieldQtyOfRecievedItem1";
            // 
            // fieldCustodyCode1
            // 
            this.fieldCustodyCode1.AreaIndex = 8;
            this.fieldCustodyCode1.FieldName = "CustodyCode";
            this.fieldCustodyCode1.Name = "fieldCustodyCode1";
            // 
            // fieldCustodyName1
            // 
            this.fieldCustodyName1.AreaIndex = 9;
            this.fieldCustodyName1.FieldName = "CustodyName";
            this.fieldCustodyName1.Name = "fieldCustodyName1";
            // 
            // fieldCustodyEnteringDate1
            // 
            this.fieldCustodyEnteringDate1.AreaIndex = 10;
            this.fieldCustodyEnteringDate1.FieldName = "CustodyEnteringDate";
            this.fieldCustodyEnteringDate1.Name = "fieldCustodyEnteringDate1";
            // 
            // fieldCustodyTimeOutDate1
            // 
            this.fieldCustodyTimeOutDate1.AreaIndex = 11;
            this.fieldCustodyTimeOutDate1.FieldName = "CustodyTimeOutDate";
            this.fieldCustodyTimeOutDate1.Name = "fieldCustodyTimeOutDate1";
            // 
            // fieldCustodySerialNo1
            // 
            this.fieldCustodySerialNo1.AreaIndex = 12;
            this.fieldCustodySerialNo1.FieldName = "CustodySerialNo";
            this.fieldCustodySerialNo1.Name = "fieldCustodySerialNo1";
            // 
            // fieldCustodyPurchaseDate1
            // 
            this.fieldCustodyPurchaseDate1.AreaIndex = 13;
            this.fieldCustodyPurchaseDate1.FieldName = "CustodyPurchaseDate";
            this.fieldCustodyPurchaseDate1.Name = "fieldCustodyPurchaseDate1";
            // 
            // fieldCustodyCat1
            // 
            this.fieldCustodyCat1.AreaIndex = 14;
            this.fieldCustodyCat1.FieldName = "CustodyCat";
            this.fieldCustodyCat1.Name = "fieldCustodyCat1";
            // 
            // fieldPurchaseAmount1
            // 
            this.fieldPurchaseAmount1.AreaIndex = 15;
            this.fieldPurchaseAmount1.FieldName = "PurchaseAmount";
            this.fieldPurchaseAmount1.Name = "fieldPurchaseAmount1";
            // 
            // fieldInUse1
            // 
            this.fieldInUse1.AreaIndex = 16;
            this.fieldInUse1.FieldName = "InUse";
            this.fieldInUse1.Name = "fieldInUse1";
            // 
            // fieldReceiveStatus1
            // 
            this.fieldReceiveStatus1.AreaIndex = 17;
            this.fieldReceiveStatus1.FieldName = "ReceiveStatus";
            this.fieldReceiveStatus1.Name = "fieldReceiveStatus1";
            // 
            // fieldInitialCustodyAmount1
            // 
            this.fieldInitialCustodyAmount1.AreaIndex = 18;
            this.fieldInitialCustodyAmount1.FieldName = "InitialCustodyAmount";
            this.fieldInitialCustodyAmount1.Name = "fieldInitialCustodyAmount1";
            // 
            // fieldDisposal1
            // 
            this.fieldDisposal1.AreaIndex = 19;
            this.fieldDisposal1.FieldName = "Disposal";
            this.fieldDisposal1.Name = "fieldDisposal1";
            // 
            // fieldCustodyCat12
            // 
            this.fieldCustodyCat12.AreaIndex = 20;
            this.fieldCustodyCat12.FieldName = "CustodyCat_1";
            this.fieldCustodyCat12.Name = "fieldCustodyCat12";
            // 
            // xrPivotGridField84
            // 
            this.xrPivotGridField84.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField84.AreaIndex = 2;
            this.xrPivotGridField84.FieldName = "EmpName";
            this.xrPivotGridField84.Name = "xrPivotGridField84";
            // 
            // xrPivotGridField85
            // 
            this.xrPivotGridField85.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField85.AreaIndex = 1;
            this.xrPivotGridField85.FieldName = "Code";
            this.xrPivotGridField85.Name = "xrPivotGridField85";
            this.xrPivotGridField85.Options.ShowCustomTotals = false;
            this.xrPivotGridField85.Options.ShowGrandTotal = false;
            this.xrPivotGridField85.Options.ShowTotals = false;
            this.xrPivotGridField85.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // xrPivotGridField86
            // 
            this.xrPivotGridField86.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.xrPivotGridField86.AreaIndex = 0;
            this.xrPivotGridField86.FieldName = "jobName";
            this.xrPivotGridField86.Name = "xrPivotGridField86";
            // 
            // xrPivotGridField87
            // 
            this.xrPivotGridField87.AreaIndex = 2;
            this.xrPivotGridField87.FieldName = "deptName";
            this.xrPivotGridField87.Name = "xrPivotGridField87";
            // 
            // xrPivotGridField88
            // 
            this.xrPivotGridField88.AreaIndex = 3;
            this.xrPivotGridField88.FieldName = "deptId";
            this.xrPivotGridField88.Name = "xrPivotGridField88";
            // 
            // TestPivotGrid
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1,
            this.sqlDataSource2,
            this.sqlDataSource3});
            this.DataMember = "SP_EmployeeCustody";
            this.DataSource = this.sqlDataSource3;
            this.Margins = new System.Drawing.Printing.Margins(12, 15, 100, 100);
            this.PageHeight = 1752;
            this.PageWidth = 1268;
            this.PaperKind = System.Drawing.Printing.PaperKind.A3Extra;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.parameter1,
            this.parameter2,
            this.Culture,
            this.parameter3,
            this.CompanyName,
            this.User});
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void xrPivotGrid1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }
}
