namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterLeaveRange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assignments", "EndDate", c => c.DateTime());
            AlterColumn("dbo.LeaveRanges", "FromYear", c => c.Single(nullable: false));
            AlterColumn("dbo.LeaveRanges", "ToYear", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LeaveRanges", "ToYear", c => c.Short());
            AlterColumn("dbo.LeaveRanges", "FromYear", c => c.Short(nullable: false));
            AlterColumn("dbo.Assignments", "EndDate", c => c.DateTime(nullable: false));
        }
    }
}
