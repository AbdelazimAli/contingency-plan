namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotification2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Notifications", "IX_Notification");
            AddColumn("dbo.Notifications", "ReadOK", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Notifications", new[] { "CompanyId", "ReadOK" }, name: "IX_Notification");
            DropColumn("dbo.Notifications", "Read");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "Read", c => c.Boolean(nullable: false));
            DropIndex("dbo.Notifications", "IX_Notification");
            DropColumn("dbo.Notifications", "ReadOK");
            CreateIndex("dbo.Notifications", new[] { "CompanyId", "Read" }, name: "IX_Notification");
        }
    }
}
