namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpId = c.Int(nullable: false),
                        ManagerId = c.Int(nullable: false),
                        Duration = c.Byte(nullable: false),
                        AssignDate = c.DateTime(nullable: false, storeType: "date"),
                        CalcMethod = c.Byte(nullable: false),
                        LeaveTypeId = c.Int(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false, storeType: "date"),
                        TaskDesc = c.String(maxLength: 500),
                        ApprovalStatus = c.Byte(nullable: false),
                        WFlowId = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.LeaveTypes", t => t.LeaveTypeId, cascadeDelete: false)
                .ForeignKey("dbo.People", t => t.ManagerId, cascadeDelete: false)
                .Index(t => t.EmpId)
                .Index(t => t.ManagerId)
                .Index(t => t.LeaveTypeId);
            
            CreateTable(
                "dbo.AssignOrderTasks",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                        TaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderId, t.TaskId })
                .ForeignKey("dbo.AssignOrders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.EmpTasks", t => t.TaskId, cascadeDelete: false)
                .Index(t => t.OrderId)
                .Index(t => t.TaskId);
            
            CreateTable(
                "dbo.RenewRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpId = c.Int(nullable: false),
                        ColumnName = c.String(nullable: false, maxLength: 50, unicode: false),
                        OldValue = c.String(maxLength: 250),
                        NewValue = c.String(maxLength: 250),
                        ApprovalStatus = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .Index(t => t.EmpId);
            
            AddColumn("dbo.LeaveTypes", "MonthOrYear", c => c.Byte(nullable: false));
            AlterColumn("dbo.LeaveTypes", "MaxNofDays", c => c.Single());
            AlterColumn("dbo.LeaveTypes", "MinLeaveDays", c => c.Single());
            DropColumn("dbo.LeaveRanges", "MonthOrYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveRanges", "MonthOrYear", c => c.Byte(nullable: false));
            DropForeignKey("dbo.RenewRequests", "EmpId", "dbo.People");
            DropForeignKey("dbo.AssignOrderTasks", "TaskId", "dbo.EmpTasks");
            DropForeignKey("dbo.AssignOrderTasks", "OrderId", "dbo.AssignOrders");
            DropForeignKey("dbo.AssignOrders", "ManagerId", "dbo.People");
            DropForeignKey("dbo.AssignOrders", "LeaveTypeId", "dbo.LeaveTypes");
            DropForeignKey("dbo.AssignOrders", "EmpId", "dbo.People");
            DropIndex("dbo.RenewRequests", new[] { "EmpId" });
            DropIndex("dbo.AssignOrderTasks", new[] { "TaskId" });
            DropIndex("dbo.AssignOrderTasks", new[] { "OrderId" });
            DropIndex("dbo.AssignOrders", new[] { "LeaveTypeId" });
            DropIndex("dbo.AssignOrders", new[] { "ManagerId" });
            DropIndex("dbo.AssignOrders", new[] { "EmpId" });
            AlterColumn("dbo.LeaveTypes", "MinLeaveDays", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "MaxNofDays", c => c.Byte());
            DropColumn("dbo.LeaveTypes", "MonthOrYear");
            DropTable("dbo.RenewRequests");
            DropTable("dbo.AssignOrderTasks");
            DropTable("dbo.AssignOrders");
        }
    }
}
