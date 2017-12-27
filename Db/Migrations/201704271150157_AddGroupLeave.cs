namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupLeave : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupLeaveLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupLeaveId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        Success = c.Boolean(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        ReasonCode = c.Byte(),
                        Reason = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.GroupLeaves", t => t.GroupLeaveId, cascadeDelete: true)
                .Index(t => t.GroupLeaveId)
                .Index(t => t.EmpId);
            
            CreateTable(
                "dbo.GroupLeaves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        TypeId = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                        Departments = c.String(maxLength: 250),
                        ApprovalStatus = c.Byte(nullable: false),
                        ReqReason = c.Short(),
                        ReasonDesc = c.String(maxLength: 250),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(),
                        NofDays = c.Single(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .ForeignKey("dbo.LeaveTypes", t => t.TypeId, cascadeDelete: false)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: false)
                .Index(t => t.CompanyId)
                .Index(t => new { t.TypeId, t.RequestDate, t.ApprovalStatus }, name: "IX_LeaveRequest")
                .Index(t => t.PeriodId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupLeaveLogs", "GroupLeaveId", "dbo.GroupLeaves");
            DropForeignKey("dbo.GroupLeaves", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.GroupLeaves", "TypeId", "dbo.LeaveTypes");
            DropForeignKey("dbo.GroupLeaves", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.GroupLeaveLogs", "EmpId", "dbo.People");
            DropIndex("dbo.GroupLeaves", new[] { "PeriodId" });
            DropIndex("dbo.GroupLeaves", "IX_LeaveRequest");
            DropIndex("dbo.GroupLeaves", new[] { "CompanyId" });
            DropIndex("dbo.GroupLeaveLogs", new[] { "EmpId" });
            DropIndex("dbo.GroupLeaveLogs", new[] { "GroupLeaveId" });
            DropTable("dbo.GroupLeaves");
            DropTable("dbo.GroupLeaveLogs");
        }
    }
}
