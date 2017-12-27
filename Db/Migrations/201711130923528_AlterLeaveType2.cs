namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterLeaveType2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeaveTypes", "IncLeavePlan", c => c.Boolean(nullable: false));
            DropColumn("dbo.LeaveTypes", "ExLeavePlan");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveTypes", "ExLeavePlan", c => c.Boolean(nullable: false));
            DropColumn("dbo.LeaveTypes", "IncLeavePlan");
        }
    }
}
