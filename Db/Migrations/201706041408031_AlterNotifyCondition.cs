namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotifyCondition : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.NotifyConditions", new[] { "CompanyId" });
            AddColumn("dbo.NotifyConditions", "EventValue", c => c.String(maxLength: 30));
            AddColumn("dbo.NotifyConditions", "ObjectName", c => c.String(maxLength: 30, unicode: false));
            CreateIndex("dbo.NotifyConditions", new[] { "CompanyId", "ObjectName" }, name: "IX_NotifyCondition");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NotifyConditions", "IX_NotifyCondition");
            DropColumn("dbo.NotifyConditions", "ObjectName");
            DropColumn("dbo.NotifyConditions", "EventValue");
            CreateIndex("dbo.NotifyConditions", "CompanyId");
        }
    }
}
