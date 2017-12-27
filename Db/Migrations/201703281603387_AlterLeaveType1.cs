namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterLeaveType1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LeaveTypes", "CasualLeaveId", "dbo.LeaveTypes");
            DropForeignKey("dbo.LeaveRequests", "PeriodId", "dbo.LeavePeriods");
            DropForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.LeavePeriods");
            DropIndex("dbo.Periods", "IX_Period");
            DropIndex("dbo.LeaveTypes", new[] { "CasualLeaveId" });
            DropIndex("dbo.LeaveRequests", "IX_LeaveRequest");
            DropIndex("dbo.LeaveTrans", "IX_LeaveTrans");
            DropIndex("dbo.SubPeriods", "IX_SubPeriod");
            RenameColumn("dbo.LeaveTypes", "MaxDaysInLife", "MaxDaysInPeriod");
            RenameColumn("dbo.LeaveTypes", "MustCheckBal", "HasAccrualPlan");
            //AddColumn("dbo.LeaveTypes", "MaxDaysInPeriod", c => c.Short());
            AddColumn("dbo.LeaveTypes", "CalendarId", c => c.Int(nullable: false));
            //AddColumn("dbo.LeaveTypes", "HasAccrualPlan", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveTrans", "AbsenceType", c => c.Short(nullable: false));
            AlterColumn("dbo.LeaveRequests", "PeriodId", c => c.Int(nullable: false));
            AlterColumn("dbo.LeaveTrans", "PeriodId", c => c.Int(nullable: false));
            CreateIndex("dbo.Periods", new[] { "CalendarId", "StartDate", "EndDate" }, unique: true, name: "IX_Period");
            CreateIndex("dbo.LeaveTypes", "CalendarId");
            CreateIndex("dbo.LeaveRequests", new[] { "TypeId", "PeriodId", "EmpId", "ApprovalStatus" }, name: "IX_LeaveRequest");
            CreateIndex("dbo.LeaveTrans", new[] { "TypeId", "PeriodId", "EmpId" }, name: "IX_LeaveTrans");
            CreateIndex("dbo.LeaveTrans", new[] { "AbsenceType", "PeriodId", "EmpId" }, name: "IX_LeaveAbsenceType");
            CreateIndex("dbo.SubPeriods", new[] { "PeriodId", "StartDate", "EndDate" }, unique: true, name: "IX_SubPeriod");
            AddForeignKey("dbo.LeaveTypes", "CalendarId", "dbo.PeriodNames", "Id", cascadeDelete: false);
            AddForeignKey("dbo.LeaveRequests", "PeriodId", "dbo.Periods", "Id", cascadeDelete: false);
            AddForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.Periods", "Id", cascadeDelete: false);
            DropColumn("dbo.LeaveTypes", "MaxDaysInYear");
           // DropColumn("dbo.LeaveTypes", "MaxDaysInLife");
           // DropColumn("dbo.LeaveTypes", "MustCheckBal");
            DropColumn("dbo.LeaveTypes", "CasualLeaveId");
            DropColumn("dbo.LeaveTypes", "FirstMonth");
            DropColumn("dbo.LeaveTypes", "FirstYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveTypes", "FirstYear", c => c.Short());
            AddColumn("dbo.LeaveTypes", "FirstMonth", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "CasualLeaveId", c => c.Int());
            AddColumn("dbo.LeaveTypes", "MustCheckBal", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveTypes", "MaxDaysInLife", c => c.Short());
            AddColumn("dbo.LeaveTypes", "MaxDaysInYear", c => c.Byte());
            DropForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.LeaveRequests", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.LeaveTypes", "CalendarId", "dbo.PeriodNames");
            DropIndex("dbo.SubPeriods", "IX_SubPeriod");
            DropIndex("dbo.LeaveTrans", "IX_LeaveAbsenceType");
            DropIndex("dbo.LeaveTrans", "IX_LeaveTrans");
            DropIndex("dbo.LeaveRequests", "IX_LeaveRequest");
            DropIndex("dbo.LeaveTypes", new[] { "CalendarId" });
            DropIndex("dbo.Periods", "IX_Period");
            AlterColumn("dbo.LeaveTrans", "PeriodId", c => c.Int());
            AlterColumn("dbo.LeaveRequests", "PeriodId", c => c.Int());
            DropColumn("dbo.LeaveTrans", "AbsenceType");
            DropColumn("dbo.LeaveTypes", "HasAccrualPlan");
            DropColumn("dbo.LeaveTypes", "CalendarId");
            DropColumn("dbo.LeaveTypes", "MaxDaysInPeriod");
            CreateIndex("dbo.SubPeriods", new[] { "PeriodId", "SubPeriodNo" }, unique: true, name: "IX_SubPeriod");
            CreateIndex("dbo.LeaveTrans", new[] { "TypeId", "PeriodId", "EmpId" }, name: "IX_LeaveTrans");
            CreateIndex("dbo.LeaveRequests", new[] { "TypeId", "PeriodId", "EmpId", "ApprovalStatus" }, name: "IX_LeaveRequest");
            CreateIndex("dbo.LeaveTypes", "CasualLeaveId");
            CreateIndex("dbo.Periods", new[] { "CalendarId", "PeriodNo" }, unique: true, name: "IX_Period");
            AddForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.LeavePeriods", "Id");
            AddForeignKey("dbo.LeaveRequests", "PeriodId", "dbo.LeavePeriods", "Id");
            AddForeignKey("dbo.LeaveTypes", "CasualLeaveId", "dbo.LeaveTypes", "Id");
        }
    }
}
