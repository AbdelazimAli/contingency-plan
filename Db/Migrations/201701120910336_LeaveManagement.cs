namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeaveManagement : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.LeavePeriods");
            DropIndex("dbo.LeaveAdjusts", "IX_LeaveAction");
            DropIndex("dbo.RequestWf", "IX_RequestWf");
            AddColumn("dbo.DisplinPeriods", "MaxDaysDeduction", c => c.Short(nullable: false));
            AddColumn("dbo.LeaveAdjusts", "ActionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.LeavePeriods", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveRequests", "CancelReason", c => c.Byte());
            AddColumn("dbo.LeaveRequests", "CancelDesc", c => c.String(maxLength: 250));
            AddColumn("dbo.LeaveRequests", "BalanceBefore", c => c.Single());
            AddColumn("dbo.LookUpCode", "Protected", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ColumnTitles", "Title", c => c.String(maxLength: 150));
            AlterColumn("dbo.CompanyDocuments", "Source", c => c.String(maxLength: 20));
            AlterColumn("dbo.LeaveAdjusts", "PeriodId", c => c.Int(nullable: false));
            AlterColumn("dbo.RequestWf", "Source", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.LeaveAdjusts", new[] { "TypeId", "PeriodId", "EmpId" }, name: "IX_LeaveAction");
            CreateIndex("dbo.RequestWf", new[] { "Source", "SourceId", "Order" }, unique: true, name: "IX_RequestWf");
            AddForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.LeavePeriods", "Id", cascadeDelete: false);
            DropColumn("dbo.DisplinRepeats", "MaxDeduction"); 
        }
        
        public override void Down()
        {
            AddColumn("dbo.DisplinRepeats", "MaxDeduction", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.LeavePeriods");
            DropIndex("dbo.RequestWf", "IX_RequestWf");
            DropIndex("dbo.LeaveAdjusts", "IX_LeaveAction");
            AlterColumn("dbo.RequestWf", "Source", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.LeaveAdjusts", "PeriodId", c => c.Int());
            AlterColumn("dbo.CompanyDocsViews", "Source", c => c.String(maxLength: 10));
            AlterColumn("dbo.ColumnTitles", "Title", c => c.String(maxLength: 50));
            DropColumn("dbo.LookUpCode", "Protected");
            DropColumn("dbo.LeaveRequests", "BalanceBefore");
            DropColumn("dbo.LeaveRequests", "CancelDesc");
            DropColumn("dbo.LeaveRequests", "CancelReason");
            DropColumn("dbo.LeavePeriods", "Status");
            DropColumn("dbo.LeaveAdjusts", "ActionDate");
            DropColumn("dbo.DisplinPeriods", "MaxDaysDeduction");
            CreateIndex("dbo.RequestWf", new[] { "Source", "SourceId", "Order" }, unique: true, name: "IX_RequestWf");
            CreateIndex("dbo.LeaveAdjusts", new[] { "TypeId", "PeriodId", "EmpId" }, name: "IX_LeaveAction");
            AddForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.LeavePeriods", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
