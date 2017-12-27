namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotifications : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Notifications", "IX_Notification");
            DropIndex("dbo.NotifyConditions", "IX_NotifyCondition");
            RenameColumn(table: "dbo.PayrollSetup", name: "CompanyId", newName: "Id");
            RenameColumn(table: "dbo.PersonSetup", name: "CompanyId", newName: "Id");
            RenameIndex(table: "dbo.PayrollSetup", name: "IX_CompanyId", newName: "IX_Id");
            RenameIndex(table: "dbo.PersonSetup", name: "IX_CompanyId", newName: "IX_Id");
            AddColumn("dbo.Notifications", "DueTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notifications", "CheckSql", c => c.String(maxLength: 100));
            AddColumn("dbo.Notifications", "SourceId", c => c.Int(nullable: false));
            AddColumn("dbo.WebMobLogs", "CompanyId", c => c.Int(nullable: false));
            AlterColumn("dbo.NotifyConditions", "Event", c => c.Int(nullable: false));
            CreateIndex("dbo.Notifications", new[] { "CompanyId", "ReadOK", "DueTime" }, name: "IX_Notification");
            CreateIndex("dbo.NotifyConditions", new[] { "CompanyId", "ObjectName", "Version", "AlertMeUntil" }, name: "IX_NotifyCondition");
            CreateIndex("dbo.WebMobLogs", "CompanyId", name: "IX_WebMobCompany");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WebMobLogs", "IX_WebMobCompany");
            DropIndex("dbo.NotifyConditions", "IX_NotifyCondition");
            DropIndex("dbo.Notifications", "IX_Notification");
            AlterColumn("dbo.NotifyConditions", "Event", c => c.Byte(nullable: false));
            DropColumn("dbo.WebMobLogs", "CompanyId");
            DropColumn("dbo.Notifications", "SourceId");
            DropColumn("dbo.Notifications", "CheckSql");
            DropColumn("dbo.Notifications", "DueTime");
            RenameIndex(table: "dbo.PersonSetup", name: "IX_Id", newName: "IX_CompanyId");
            RenameIndex(table: "dbo.PayrollSetup", name: "IX_Id", newName: "IX_CompanyId");
            RenameColumn(table: "dbo.PersonSetup", name: "Id", newName: "CompanyId");
            RenameColumn(table: "dbo.PayrollSetup", name: "Id", newName: "CompanyId");
            CreateIndex("dbo.NotifyConditions", new[] { "CompanyId", "ObjectName", "Version" }, name: "IX_NotifyCondition");
            CreateIndex("dbo.Notifications", new[] { "CompanyId", "ReadOK" }, name: "IX_Notification");
        }
    }
}
