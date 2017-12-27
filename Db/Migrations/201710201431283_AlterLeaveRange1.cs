namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterLeaveRange1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.LeaveRanges", "FromMonth", "FromPeriod");
            RenameColumn("dbo.LeaveRanges", "ToMonth", "ToPeriod");
            AddColumn("dbo.LeaveRanges", "MonthOrYear", c => c.Byte(nullable: false));
            Sql("update LeaveRanges set MonthOrYear = 1 where MonthOrYear = 0");
            AddColumn("dbo.PeopleTrain", "CourseGrade", c => c.Short());
            AlterColumn("dbo.People", "Title", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveRanges", "ToMonth", c => c.Short(nullable: false));
            AddColumn("dbo.LeaveRanges", "FromMonth", c => c.Short(nullable: false));
            AlterColumn("dbo.People", "Title", c => c.String(maxLength: 20));
            DropColumn("dbo.PeopleTrain", "CourseGrade");
            DropColumn("dbo.LeaveRanges", "MonthOrYear");
            DropColumn("dbo.LeaveRanges", "ToPeriod");
            DropColumn("dbo.LeaveRanges", "FromPeriod");
        }
    }
}
