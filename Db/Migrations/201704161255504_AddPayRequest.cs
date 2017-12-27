namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPayRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PayRequestDets",
                c => new
                    {
                        RequestId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        BankId = c.Int(),
                        EmpAccountNo = c.String(maxLength: 50),
                        PayAmount = c.Decimal(nullable: false, precision: 18, scale: 3),
                    })
                .PrimaryKey(t => new { t.RequestId, t.EmpId })
                .ForeignKey("dbo.Providers", t => t.BankId)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.PayRequests", t => t.RequestId, cascadeDelete: true)
                .Index(t => t.RequestId)
                .Index(t => t.EmpId)
                .Index(t => t.BankId);
            
            CreateTable(
                "dbo.PayRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        RequestNo = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false, storeType: "date"),
                        PayType = c.Byte(nullable: false),
                        ApprovalStatus = c.Byte(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        PayDate = c.DateTime(storeType: "date"),
                        RejectReason = c.Short(),
                        RejectDesc = c.String(maxLength: 250),
                        CancelReason = c.Short(),
                        CancelDesc = c.String(maxLength: 250),
                        WFlowId = c.Int(),
                        Departments = c.String(maxLength: 100),
                        Employees = c.String(maxLength: 100),
                        PayrollGroup = c.Short(),
                        PayrollId = c.Int(),
                        SalaryItems = c.String(maxLength: 100),
                        PayPercent = c.Single(nullable: false),
                        FormulaId = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payrolls", t => t.PayrollId)
                .Index(t => new { t.CompanyId, t.ApprovalStatus }, name: "IX_PayRequest")
                .Index(t => new { t.CompanyId, t.RequestNo }, name: "IX_RequestNo")
                .Index(t => t.PayrollId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PayRequestDets", "RequestId", "dbo.PayRequests");
            DropForeignKey("dbo.PayRequests", "PayrollId", "dbo.Payrolls");
            DropForeignKey("dbo.PayRequestDets", "EmpId", "dbo.People");
            DropForeignKey("dbo.PayRequestDets", "BankId", "dbo.Providers");
            DropIndex("dbo.PayRequests", new[] { "PayrollId" });
            DropIndex("dbo.PayRequests", "IX_RequestNo");
            DropIndex("dbo.PayRequests", "IX_PayRequest");
            DropIndex("dbo.PayRequestDets", new[] { "BankId" });
            DropIndex("dbo.PayRequestDets", new[] { "EmpId" });
            DropIndex("dbo.PayRequestDets", new[] { "RequestId" });
            DropTable("dbo.PayRequests");
            DropTable("dbo.PayRequestDets");
        }
    }
}
