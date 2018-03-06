namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoanRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoanRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        RequestNo = c.Int(),
                        RequestDate = c.DateTime(nullable: false, storeType: "date"),
                        LoanTypeId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InstallCnt = c.Short(nullable: false),
                        InstallAmt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartPayDate = c.DateTime(nullable: false, storeType: "date"),
                        PaidDate = c.DateTime(storeType: "date"),
                        LoanCause = c.String(maxLength: 500),
                        ApprovalStatus = c.Byte(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.LoanTypes", t => t.LoanTypeId, cascadeDelete: false)
                .Index(t => t.EmpId)
                .Index(t => t.LoanTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoanRequests", "LoanTypeId", "dbo.LoanTypes");
            DropForeignKey("dbo.LoanRequests", "EmpId", "dbo.People");
            DropIndex("dbo.LoanRequests", new[] { "LoanTypeId" });
            DropIndex("dbo.LoanRequests", new[] { "EmpId" });
            DropTable("dbo.LoanRequests");
        }
    }
}
