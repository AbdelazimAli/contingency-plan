namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSourceId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisplinRepeats", "Description", c => c.String(maxLength: 250));
            AddColumn("dbo.Menus", "Config", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AudiTrails", "SourceId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Notifications", "SourceId", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "SourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.AudiTrails", "SourceId", c => c.Int(nullable: false));
            DropColumn("dbo.Menus", "Config");
            DropColumn("dbo.DisplinRepeats", "Description");
        }
    }
}
