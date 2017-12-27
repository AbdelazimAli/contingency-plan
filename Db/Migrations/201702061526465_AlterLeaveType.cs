namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterLeaveType : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PostingLeaves", newName: "LeavePostings");
            AddColumn("dbo.LeaveTypes", "PercentOfActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.LeaveTypes", "YearStartDt");
            DropColumn("dbo.LeaveTypes", "StartDay");
            DropColumn("dbo.LeaveTypes", "StartMonth");
            DropColumn("dbo.LeaveTypes", "FractionsOpt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveTypes", "FractionsOpt", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "StartMonth", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "StartDay", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "YearStartDt", c => c.Byte());
            DropColumn("dbo.LeaveTypes", "PercentOfActive");
            RenameTable(name: "dbo.LeavePostings", newName: "PostingLeaves");
        }
    }
}
