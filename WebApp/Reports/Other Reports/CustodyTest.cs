using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for CustodyTest
/// </summary>
public class CustodyTest : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRPivotGrid xrPivotGrid1;
    private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    private DevExpress.XtraReports.Parameters.Parameter Culture;
    private DevExpress.XtraReports.Parameters.Parameter CompanyId;
    private DevExpress.XtraReports.Parameters.Parameter CompanyName;
    private DevExpress.XtraReports.Parameters.Parameter User;
    private DevExpress.XtraReports.Parameters.Parameter MappingDeptIds;
    private DevExpress.XtraReports.Parameters.Parameter MappingEmpIds;
    private DevExpress.XtraReports.Parameters.Parameter DeptIds;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldEmpName1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCode1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldEmpId1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldjobName1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldjobId1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fielddeptName1;
    private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fielddeptId1;
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

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public CustodyTest()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustodyTest));
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings1 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPivotGrid1 = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Culture = new DevExpress.XtraReports.Parameters.Parameter();
            this.CompanyId = new DevExpress.XtraReports.Parameters.Parameter();
            this.CompanyName = new DevExpress.XtraReports.Parameters.Parameter();
            this.User = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingDeptIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.MappingEmpIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.DeptIds = new DevExpress.XtraReports.Parameters.Parameter();
            this.fieldCustodyCat1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldEmpName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCode1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldEmpId1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldjobName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldjobId1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fielddeptName1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fielddeptId1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
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
            this.fieldPurchaseAmount1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldInUse1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldReceiveStatus1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldInitialCustodyAmount1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldDisposal1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCustodyCat12 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivotGrid1});
            this.Detail.HeightF = 100F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 11.80556F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 9.722222F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
            this.xrPivotGrid1.DataSource = this.sqlDataSource1;
            this.xrPivotGrid1.Fields.AddRange(new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField[] {
            this.fieldEmpName1,
            this.fieldCode1,
            this.fieldEmpId1,
            this.fieldjobName1,
            this.fieldjobId1,
            this.fielddeptName1,
            this.fielddeptId1,
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
            this.xrPivotGrid1.OptionsPrint.FilterSeparatorBarPadding = 3;
            this.xrPivotGrid1.SizeF = new System.Drawing.SizeF(673.3333F, 100F);
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionName = "HrContext";
            this.sqlDataSource1.Name = "sqlDataSource1";
            storedProcQuery1.Name = "SP_EmployeeCustody";
            queryParameter1.Name = "@Culture";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("[Parameters.Culture]", typeof(string));
            queryParameter2.Name = "@LoginCompanyId";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("[Parameters.CompanyId]", typeof(int));
            queryParameter3.Name = "@deptName";
            queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter3.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingDeptIds]", typeof(string));
            queryParameter4.Name = "@empIds";
            queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter4.Value = new DevExpress.DataAccess.Expression("[Parameters.MappingEmpIds]", typeof(string));
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.StoredProcName = "SP_EmployeeCustody";
            this.sqlDataSource1.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1});
            this.sqlDataSource1.ResultSchemaSerializable = resources.GetString("sqlDataSource1.ResultSchemaSerializable");
            // 
            // Culture
            // 
            this.Culture.Description = "Culture";
            this.Culture.Name = "Culture";
            this.Culture.ValueInfo = "ar-EG";
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
            // CompanyName
            // 
            this.CompanyName.Description = "CompanyName";
            this.CompanyName.Name = "CompanyName";
            this.CompanyName.Visible = false;
            // 
            // User
            // 
            this.User.Description = "User";
            this.User.Name = "User";
            this.User.Visible = false;
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
            // DeptIds
            // 
            this.DeptIds.Description = "الإدارة";
            dynamicListLookUpSettings1.DataAdapter = null;
            dynamicListLookUpSettings1.DataMember = "SP_EmployeeCustody";
            dynamicListLookUpSettings1.DataSource = this.sqlDataSource1;
            dynamicListLookUpSettings1.DisplayMember = "deptName";
            dynamicListLookUpSettings1.ValueMember = "deptId";
            this.DeptIds.LookUpSettings = dynamicListLookUpSettings1;
            this.DeptIds.Name = "DeptIds";
            this.DeptIds.Type = typeof(int);
            this.DeptIds.ValueInfo = "0";
            // 
            // fieldCustodyCat1
            // 
            this.fieldCustodyCat1.AreaIndex = 16;
            this.fieldCustodyCat1.FieldName = "CustodyCat";
            this.fieldCustodyCat1.Name = "fieldCustodyCat1";
            // 
            // fieldEmpName1
            // 
            this.fieldEmpName1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldEmpName1.AreaIndex = 3;
            this.fieldEmpName1.FieldName = "EmpName";
            this.fieldEmpName1.MinWidth = 10;
            this.fieldEmpName1.Name = "fieldEmpName1";
            this.fieldEmpName1.Options.ShowCustomTotals = false;
            this.fieldEmpName1.Options.ShowGrandTotal = false;
            this.fieldEmpName1.Options.ShowTotals = false;
            this.fieldEmpName1.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // fieldCode1
            // 
            this.fieldCode1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldCode1.AreaIndex = 2;
            this.fieldCode1.FieldName = "Code";
            this.fieldCode1.Name = "fieldCode1";
            this.fieldCode1.Options.ShowCustomTotals = false;
            this.fieldCode1.Options.ShowGrandTotal = false;
            this.fieldCode1.Options.ShowTotals = false;
            this.fieldCode1.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // fieldEmpId1
            // 
            this.fieldEmpId1.AreaIndex = 0;
            this.fieldEmpId1.FieldName = "EmpId";
            this.fieldEmpId1.Name = "fieldEmpId1";
            // 
            // fieldjobName1
            // 
            this.fieldjobName1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldjobName1.AreaIndex = 1;
            this.fieldjobName1.FieldName = "jobName";
            this.fieldjobName1.Name = "fieldjobName1";
            this.fieldjobName1.Options.ShowCustomTotals = false;
            this.fieldjobName1.Options.ShowGrandTotal = false;
            this.fieldjobName1.Options.ShowTotals = false;
            this.fieldjobName1.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // fieldjobId1
            // 
            this.fieldjobId1.AreaIndex = 1;
            this.fieldjobId1.FieldName = "jobId";
            this.fieldjobId1.Name = "fieldjobId1";
            // 
            // fielddeptName1
            // 
            this.fielddeptName1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fielddeptName1.AreaIndex = 0;
            this.fielddeptName1.FieldName = "deptName";
            this.fielddeptName1.Name = "fielddeptName1";
            this.fielddeptName1.Options.ShowCustomTotals = false;
            this.fielddeptName1.Options.ShowGrandTotal = false;
            this.fielddeptName1.Options.ShowTotals = false;
            this.fielddeptName1.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // fielddeptId1
            // 
            this.fielddeptId1.AreaIndex = 2;
            this.fielddeptId1.FieldName = "deptId";
            this.fielddeptId1.Name = "fielddeptId1";
            // 
            // fieldReceiveDate1
            // 
            this.fieldReceiveDate1.AreaIndex = 3;
            this.fieldReceiveDate1.FieldName = "ReceiveDate";
            this.fieldReceiveDate1.Name = "fieldReceiveDate1";
            // 
            // fieldDeliveryDate1
            // 
            this.fieldDeliveryDate1.AreaIndex = 4;
            this.fieldDeliveryDate1.FieldName = "DeliveryDate";
            this.fieldDeliveryDate1.Name = "fieldDeliveryDate1";
            // 
            // fielddeliveryStatus1
            // 
            this.fielddeliveryStatus1.AreaIndex = 5;
            this.fielddeliveryStatus1.FieldName = "deliveryStatus";
            this.fielddeliveryStatus1.Name = "fielddeliveryStatus1";
            // 
            // fieldQtyOfRecievedItem1
            // 
            this.fieldQtyOfRecievedItem1.AreaIndex = 6;
            this.fieldQtyOfRecievedItem1.FieldName = "QtyOfRecievedItem";
            this.fieldQtyOfRecievedItem1.Name = "fieldQtyOfRecievedItem1";
            // 
            // fieldCustodyCode1
            // 
            this.fieldCustodyCode1.AreaIndex = 8;
            this.fieldCustodyCode1.FieldName = "CustodyCode";
            this.fieldCustodyCode1.KPIGraphic = DevExpress.XtraPivotGrid.PivotKPIGraphic.Shapes;
            this.fieldCustodyCode1.Name = "fieldCustodyCode1";
            this.fieldCustodyCode1.Options.ShowCustomTotals = false;
            this.fieldCustodyCode1.Options.ShowGrandTotal = false;
            this.fieldCustodyCode1.Options.ShowTotals = false;
            this.fieldCustodyCode1.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // fieldCustodyName1
            // 
            this.fieldCustodyName1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldCustodyName1.AreaIndex = 1;
            this.fieldCustodyName1.FieldName = "CustodyName";
            this.fieldCustodyName1.Name = "fieldCustodyName1";
            this.fieldCustodyName1.Options.ShowCustomTotals = false;
            this.fieldCustodyName1.Options.ShowGrandTotal = false;
            this.fieldCustodyName1.Options.ShowTotals = false;
            this.fieldCustodyName1.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // fieldCustodyEnteringDate1
            // 
            this.fieldCustodyEnteringDate1.AreaIndex = 7;
            this.fieldCustodyEnteringDate1.FieldName = "CustodyEnteringDate";
            this.fieldCustodyEnteringDate1.Name = "fieldCustodyEnteringDate1";
            // 
            // fieldCustodyTimeOutDate1
            // 
            this.fieldCustodyTimeOutDate1.AreaIndex = 9;
            this.fieldCustodyTimeOutDate1.FieldName = "CustodyTimeOutDate";
            this.fieldCustodyTimeOutDate1.Name = "fieldCustodyTimeOutDate1";
            // 
            // fieldCustodySerialNo1
            // 
            this.fieldCustodySerialNo1.AreaIndex = 10;
            this.fieldCustodySerialNo1.FieldName = "CustodySerialNo";
            this.fieldCustodySerialNo1.Name = "fieldCustodySerialNo1";
            // 
            // fieldCustodyPurchaseDate1
            // 
            this.fieldCustodyPurchaseDate1.AreaIndex = 11;
            this.fieldCustodyPurchaseDate1.FieldName = "CustodyPurchaseDate";
            this.fieldCustodyPurchaseDate1.Name = "fieldCustodyPurchaseDate1";
            // 
            // fieldPurchaseAmount1
            // 
            this.fieldPurchaseAmount1.AreaIndex = 12;
            this.fieldPurchaseAmount1.FieldName = "PurchaseAmount";
            this.fieldPurchaseAmount1.Name = "fieldPurchaseAmount1";
            // 
            // fieldInUse1
            // 
            this.fieldInUse1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldInUse1.AreaIndex = 0;
            this.fieldInUse1.FieldName = "InUse";
            this.fieldInUse1.KPIGraphic = DevExpress.XtraPivotGrid.PivotKPIGraphic.Shapes;
            this.fieldInUse1.Name = "fieldInUse1";
            this.fieldInUse1.Options.ShowCustomTotals = false;
            this.fieldInUse1.Options.ShowGrandTotal = false;
            this.fieldInUse1.Options.ShowTotals = false;
            this.fieldInUse1.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // fieldReceiveStatus1
            // 
            this.fieldReceiveStatus1.AreaIndex = 13;
            this.fieldReceiveStatus1.FieldName = "ReceiveStatus";
            this.fieldReceiveStatus1.Name = "fieldReceiveStatus1";
            // 
            // fieldInitialCustodyAmount1
            // 
            this.fieldInitialCustodyAmount1.AreaIndex = 14;
            this.fieldInitialCustodyAmount1.FieldName = "InitialCustodyAmount";
            this.fieldInitialCustodyAmount1.Name = "fieldInitialCustodyAmount1";
            // 
            // fieldDisposal1
            // 
            this.fieldDisposal1.AreaIndex = 15;
            this.fieldDisposal1.FieldName = "Disposal";
            this.fieldDisposal1.Name = "fieldDisposal1";
            // 
            // fieldCustodyCat12
            // 
            this.fieldCustodyCat12.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldCustodyCat12.AreaIndex = 0;
            this.fieldCustodyCat12.FieldName = "CustodyCat_1";
            this.fieldCustodyCat12.Name = "fieldCustodyCat12";
            this.fieldCustodyCat12.Options.ShowCustomTotals = false;
            this.fieldCustodyCat12.Options.ShowGrandTotal = false;
            this.fieldCustodyCat12.Options.ShowTotals = false;
            this.fieldCustodyCat12.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // CustodyTest
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource1});
            this.Margins = new System.Drawing.Printing.Margins(10, 17, 12, 10);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Culture,
            this.CompanyId,
            this.CompanyName,
            this.User,
            this.MappingDeptIds,
            this.MappingEmpIds,
            this.DeptIds});
            this.Version = "17.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
