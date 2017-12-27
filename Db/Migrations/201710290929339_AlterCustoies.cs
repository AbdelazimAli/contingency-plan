namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCustoies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Custody", "PurchaseAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Custody", "InUse", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "Status", c => c.Byte(nullable: false));
            AddColumn("dbo.Custody", "Qty", c => c.Single(nullable: false));
            AddColumn("dbo.Custody", "Disposal", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "LocationId", c => c.Int());
            AddColumn("dbo.EmpCustodies", "Status", c => c.Byte(nullable: false));
            AddColumn("dbo.EmpCustodies", "Qty", c => c.Single(nullable: false));
            AddColumn("dbo.EmpCustodies", "Description", c => c.String(maxLength: 250));
            AddColumn("dbo.EmpCustodies", "LocationId", c => c.Int());
            AddColumn("dbo.Employements", "Renew", c => c.Boolean(nullable: false));
            AddColumn("dbo.RenewRequests", "RejectionRes", c => c.String(maxLength: 250));
            AlterColumn("dbo.EmpCustodies", "RecvDate", c => c.DateTime(nullable: false, storeType: "date"));
            CreateIndex("dbo.Custody", "LocationId");
            CreateIndex("dbo.EmpCustodies", "LocationId");
            AddForeignKey("dbo.Custody", "LocationId", "dbo.Locations", "Id");
            AddForeignKey("dbo.EmpCustodies", "LocationId", "dbo.Locations", "Id");
            DropColumn("dbo.Custody", "CurrentAmount");
            DropColumn("dbo.Custody", "Keyword");
            DropColumn("dbo.Custody", "Available");
            DropColumn("dbo.Custody", "IsNew");
            DropColumn("dbo.Custody", "LinkToInvent");
            DropColumn("dbo.Custody", "LinkToGL");
            DropColumn("dbo.Custody", "AccountCode");
            DropColumn("dbo.EmpCustodies", "CurrentAmount");
            DropColumn("dbo.EmpCustodies", "CustodyStat");
            DropColumn("dbo.EmpCustodies", "ExpdelvryDate");
            DropColumn("dbo.EmpCustodies", "deliveryStatus");
            DropColumn("dbo.EmpCustodies", "deliveryCause");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmpCustodies", "deliveryCause", c => c.String(maxLength: 250));
            AddColumn("dbo.EmpCustodies", "deliveryStatus", c => c.Short());
            AddColumn("dbo.EmpCustodies", "ExpdelvryDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.EmpCustodies", "CustodyStat", c => c.Short());
            AddColumn("dbo.EmpCustodies", "CurrentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Custody", "AccountCode", c => c.String(maxLength: 100));
            AddColumn("dbo.Custody", "LinkToGL", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "LinkToInvent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "IsNew", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "Available", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "Keyword", c => c.String(maxLength: 100));
            AddColumn("dbo.Custody", "CurrentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.EmpCustodies", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Custody", "LocationId", "dbo.Locations");
            DropIndex("dbo.EmpCustodies", new[] { "LocationId" });
            DropIndex("dbo.Custody", new[] { "LocationId" });
            AlterColumn("dbo.EmpCustodies", "RecvDate", c => c.DateTime(storeType: "date"));
            DropColumn("dbo.RenewRequests", "RejectionRes");
            DropColumn("dbo.Employements", "Renew");
            DropColumn("dbo.EmpCustodies", "LocationId");
            DropColumn("dbo.EmpCustodies", "Description");
            DropColumn("dbo.EmpCustodies", "Qty");
            DropColumn("dbo.EmpCustodies", "Status");
            DropColumn("dbo.Custody", "LocationId");
            DropColumn("dbo.Custody", "Disposal");
            DropColumn("dbo.Custody", "Qty");
            DropColumn("dbo.Custody", "Status");
            DropColumn("dbo.Custody", "InUse");
            DropColumn("dbo.Custody", "PurchaseAmount");
        }
    }
}
