namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalAlterNotificationDb : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Notifications", "IX_Notification");
            AddColumn("dbo.Notifications", "RefEmpId", c => c.Int());
            CreateIndex("dbo.Notifications", "CompanyId", name: "IX_Notification");
            CreateIndex("dbo.NotifyConditions", new[] { "CompanyId", "Event", "AlertMeUntil" }, name: "IX_NotifyCondition2");
            DropColumn("dbo.Notifications", "ReadOK");
            DropColumn("dbo.Notifications", "DueTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "DueTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notifications", "ReadOK", c => c.Boolean(nullable: false));
            DropIndex("dbo.NotifyConditions", "IX_NotifyCondition2");
            DropIndex("dbo.Notifications", "IX_Notification");
            DropColumn("dbo.Notifications", "RefEmpId");
            CreateIndex("dbo.Notifications", new[] { "CompanyId", "ReadOK", "DueTime" }, name: "IX_Notification");
        }
    }
}
