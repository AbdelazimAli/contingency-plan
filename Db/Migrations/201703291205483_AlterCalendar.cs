namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCalendar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Periods", "Status", c => c.Byte(nullable: false));
            DropColumn("dbo.Periods", "Opened");
            DropColumn("dbo.LeaveTypes", "AccrualBal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveTypes", "AccrualBal", c => c.Byte());
            AddColumn("dbo.Periods", "Opened", c => c.Boolean(nullable: false));
            DropColumn("dbo.Periods", "Status");
        }
    }
}
