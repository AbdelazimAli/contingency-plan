namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLeavePeriod : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("LeavePostings", "PeriodId", "dbo.LeavePeriods");
            DropIndex("dbo.LeavePostings", "IX_PeriodId");
            DropForeignKey("dbo.LeaveRequests", "PeriodId", "dbo.LeavePeriods");
            DropForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.LeavePeriods");
            DropForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.LeavePeriods");
            DropForeignKey("dbo.LeavePeriods", "LeaveTypeId", "dbo.LeaveTypes");
            DropIndex("dbo.LeavePeriods", "IX_LeaveMonthYear");
            DropIndex("dbo.LeavePeriods", "IX_LeavePeriod");
            DropTable("dbo.LeavePeriods");
           // AddForeignKey("dbo.Employements", "SuggestJobId", "dbo.Jobs", "Id");
            AddForeignKey("dbo.LeavePostings", "PeriodId", "dbo.Periods", "Id");
           // AddForeignKey("dbo.LeaveRequests", "PeriodId", "dbo.Periods", "Id");
            AddForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.Periods", "Id");
         //   AddForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.Periods", "Id");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LeavePeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeaveTypeId = c.Int(nullable: false),
                        PeriodNo = c.Int(nullable: false),
                        PeriodMonth = c.Byte(nullable: false),
                        PeriodYear = c.Short(nullable: false),
                        Posted = c.Boolean(nullable: false),
                        Opened = c.Boolean(nullable: false),
                        PostDate = c.DateTime(),
                        CreatedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.LeavePeriods", new[] { "LeaveTypeId", "PeriodNo" }, unique: true, name: "IX_LeavePeriod");
            CreateIndex("dbo.LeavePeriods", new[] { "LeaveTypeId", "PeriodMonth", "PeriodYear" }, unique: true, name: "IX_LeaveMonthYear");
            AddForeignKey("dbo.LeavePeriods", "LeaveTypeId", "dbo.LeaveTypes", "Id", cascadeDelete: true);
        }
    }
}
