namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLeaveType1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeaveRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestDate = c.DateTime(nullable: false),
                        EmpId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        ReqStatus = c.Byte(nullable: false),
                        ApprovalStatus = c.Byte(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ReqReason = c.Byte(),
                        ReasonDesc = c.String(maxLength: 250),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        ActualStartDate = c.DateTime(nullable: false),
                        ActualEndDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                        NofDays = c.Short(nullable: false),
                        ReplaceEmpId = c.Int(),
                        AuthbyEmpId = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LeaveTypes", t => t.TypeId, cascadeDelete: false)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .Index(t => t.EmpId)
                .Index(t => t.TypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeaveRequests", "EmpId", "dbo.People");
            DropForeignKey("dbo.LeaveRequests", "TypeId", "dbo.LeaveTypes");
            DropIndex("dbo.LeaveRequests", new[] { "TypeId" });
            DropIndex("dbo.LeaveRequests", new[] { "EmpId" });
            DropTable("dbo.LeaveRequests");
        }
    }
}
