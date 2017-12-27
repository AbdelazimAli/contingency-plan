namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustLeave : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.LeavePeriods");
            DropForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.LeavePeriods");
            DropIndex("dbo.Employements", new[] { "EmpId" });
            DropIndex("dbo.LeaveAdjusts", "IX_LeaveAction");
            DropIndex("dbo.LeaveRequests", new[] { "EmpId" });
            DropIndex("dbo.LeaveRequests", new[] { "TypeId" });
            DropIndex("dbo.LeaveTrans", "IX_LeaveTrans");
            AddColumn("dbo.LeaveTypes", "MaxDaysInPeriod", c => c.Short());
            AddColumn("dbo.LeaveTypes", "AllowNegBal", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveTypes", "Percentage", c => c.Single());
            AddColumn("dbo.LeaveTypes", "FirstMonth", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "FirstYear", c => c.Short());
            AddColumn("dbo.LeaveRequests", "PeriodId", c => c.Int());
            AlterColumn("dbo.LeaveAdjusts", "PeriodId", c => c.Int());
            AlterColumn("dbo.LeaveAdjusts", "NofDays", c => c.Single(nullable: false));
            AlterColumn("dbo.LeaveTypes", "AccBalDays", c => c.Single());
            AlterColumn("dbo.LeaveRequests", "NofDays", c => c.Single(nullable: false));
            AlterColumn("dbo.LeaveTrans", "PeriodId", c => c.Int());
            AlterColumn("dbo.LeaveTrans", "TransQty", c => c.Single(nullable: false));
            CreateIndex("dbo.Employements", new[] { "EmpId", "Status" }, name: "IX_Employement");
            CreateIndex("dbo.Employements", new[] { "EmpId", "StartDate" }, unique: true, name: "IX_EmployementDate");
            CreateIndex("dbo.LeaveAdjusts", new[] { "TypeId", "PeriodId", "EmpId" }, name: "IX_LeaveAction");
            CreateIndex("dbo.LeavePeriods", new[] { "LeaveTypeId", "PeriodMonth", "PeriodYear" }, unique: true, name: "IX_LeaveMonthYear");
            CreateIndex("dbo.LeaveRequests", new[] { "TypeId", "PeriodId", "EmpId", "ApprovalStatus" }, name: "IX_LeaveRequest");
            CreateIndex("dbo.LeaveTrans", new[] { "TypeId", "PeriodId", "EmpId" }, name: "IX_LeaveTrans");
            AddForeignKey("dbo.LeaveRequests", "PeriodId", "dbo.LeavePeriods", "Id");
            AddForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.LeavePeriods", "Id");
            AddForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.LeavePeriods", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.LeavePeriods");
            DropForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.LeavePeriods");
            DropForeignKey("dbo.LeaveRequests", "PeriodId", "dbo.LeavePeriods");
            DropIndex("dbo.LeaveTrans", "IX_LeaveTrans");
            DropIndex("dbo.LeaveRequests", "IX_LeaveRequest");
            DropIndex("dbo.LeavePeriods", "IX_LeaveMonthYear");
            DropIndex("dbo.LeaveAdjusts", "IX_LeaveAction");
            DropIndex("dbo.Employements", "IX_EmployementDate");
            DropIndex("dbo.Employements", "IX_Employement");
            AlterColumn("dbo.LeaveTrans", "TransQty", c => c.Short(nullable: false));
            AlterColumn("dbo.LeaveTrans", "PeriodId", c => c.Int(nullable: false));
            AlterColumn("dbo.LeaveRequests", "NofDays", c => c.Short(nullable: false));
            AlterColumn("dbo.LeaveTypes", "AccBalDays", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LeaveAdjusts", "NofDays", c => c.Short(nullable: false));
            AlterColumn("dbo.LeaveAdjusts", "PeriodId", c => c.Int(nullable: false));
            DropColumn("dbo.LeaveRequests", "PeriodId");
            DropColumn("dbo.LeaveTypes", "FirstYear");
            DropColumn("dbo.LeaveTypes", "FirstMonth");
            DropColumn("dbo.LeaveTypes", "Percentage");
            DropColumn("dbo.LeaveTypes", "AllowNegBal");
            DropColumn("dbo.LeaveTypes", "MaxDaysInPeriod");
            CreateIndex("dbo.LeaveTrans", new[] { "TypeId", "PeriodId", "EmpId" }, name: "IX_LeaveTrans");
            CreateIndex("dbo.LeaveRequests", "TypeId");
            CreateIndex("dbo.LeaveRequests", "EmpId");
            CreateIndex("dbo.LeaveAdjusts", new[] { "TypeId", "PeriodId", "EmpId" }, name: "IX_LeaveAction");
            CreateIndex("dbo.Employements", "EmpId");
            AddForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.LeavePeriods", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.LeavePeriods", "Id", cascadeDelete: true);
        }
    }
}
