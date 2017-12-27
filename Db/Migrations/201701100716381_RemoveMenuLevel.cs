namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMenuLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisplinPeriods", "Frequency", c => c.Byte(nullable: false));
            AddColumn("dbo.DisplinPeriods", "Times", c => c.Short(nullable: false));
            DropColumn("dbo.DisplinPeriods", "PeriodSDate");
            DropColumn("dbo.Menus", "MenuLevel");
            DropColumn("dbo.LeaveTypes", "FirstMonth");
            DropColumn("dbo.LeaveTypes", "FirstYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveTypes", "FirstYear", c => c.Short());
            AddColumn("dbo.LeaveTypes", "FirstMonth", c => c.Byte());
            AddColumn("dbo.Menus", "MenuLevel", c => c.Byte(nullable: false));
            AddColumn("dbo.DisplinPeriods", "PeriodSDate", c => c.DateTime());
            DropColumn("dbo.DisplinPeriods", "Times");
            DropColumn("dbo.DisplinPeriods", "Frequency");
        }
    }
}
