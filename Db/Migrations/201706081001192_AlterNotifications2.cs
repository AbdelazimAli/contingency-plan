namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotifications2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "Immediately", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Immediately");
        }
    }
}
