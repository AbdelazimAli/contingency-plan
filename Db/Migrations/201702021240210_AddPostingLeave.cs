namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostingLeave : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostingLeaves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeriodId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        Posted = c.Boolean(nullable: false),
                        Reason = c.String(maxLength: 100),
                        CreatedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.LeavePeriods", t => t.PeriodId, cascadeDelete: true)
                .Index(t => t.PeriodId)
                .Index(t => t.EmpId);
            
            AddColumn("dbo.DocTypes", "AccessLevel", c => c.Byte(nullable: false));
            AddColumn("dbo.Employements", "StopReason", c => c.Byte(nullable: false));
            AddColumn("dbo.LeaveTypes", "MaxDaysInYear", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "WorkServMethod", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "MaxPercent", c => c.Single());
            AddColumn("dbo.LeaveTypes", "MinLeaveDays", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "MinContinous", c => c.Byte());
            AddColumn("dbo.LeaveRanges", "FromPeriod", c => c.Short(nullable: false));
            AddColumn("dbo.LeaveRanges", "ToPeriod", c => c.Short());
            AddColumn("dbo.Terminations", "IsDeleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Benefits", "EndDate", c => c.DateTime());
            AlterColumn("dbo.DisplinPeriods", "EndDate", c => c.DateTime());
            AlterColumn("dbo.EmpBenefits", "EndDate", c => c.DateTime());
            AlterColumn("dbo.LeaveRanges", "NofDays", c => c.Single(nullable: false));
            DropColumn("dbo.LeaveTypes", "MinDays");
            DropColumn("dbo.LeaveRanges", "FromYear");
            DropColumn("dbo.LeaveRanges", "ToYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveRanges", "ToYear", c => c.Single());
            AddColumn("dbo.LeaveRanges", "FromYear", c => c.Single(nullable: false));
            AddColumn("dbo.LeaveTypes", "MinDays", c => c.Byte());
            DropForeignKey("dbo.PostingLeaves", "PeriodId", "dbo.LeavePeriods");
            DropForeignKey("dbo.PostingLeaves", "EmpId", "dbo.People");
            DropIndex("dbo.PostingLeaves", new[] { "EmpId" });
            DropIndex("dbo.PostingLeaves", new[] { "PeriodId" });
            AlterColumn("dbo.LeaveRanges", "NofDays", c => c.Byte(nullable: false));
            AlterColumn("dbo.EmpBenefits", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DisplinPeriods", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Benefits", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Terminations", "IsDeleted");
            DropColumn("dbo.LeaveRanges", "ToPeriod");
            DropColumn("dbo.LeaveRanges", "FromPeriod");
            DropColumn("dbo.LeaveTypes", "MinContinous");
            DropColumn("dbo.LeaveTypes", "MinLeaveDays");
            DropColumn("dbo.LeaveTypes", "MaxPercent");
            DropColumn("dbo.LeaveTypes", "WorkServMethod");
            DropColumn("dbo.LeaveTypes", "MaxDaysInYear");
            DropColumn("dbo.Employements", "StopReason");
            DropColumn("dbo.DocTypes", "AccessLevel");
            DropTable("dbo.PostingLeaves");
        }
    }
}
