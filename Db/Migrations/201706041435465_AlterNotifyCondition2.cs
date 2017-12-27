namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotifyCondition2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.NotifyConditions", "IX_NotifyCondition");
            CreateIndex("dbo.NotifyConditions", new[] { "CompanyId", "ObjectName", "Version" }, name: "IX_NotifyCondition");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NotifyConditions", "IX_NotifyCondition");
            CreateIndex("dbo.NotifyConditions", new[] { "CompanyId", "ObjectName" }, name: "IX_NotifyCondition");
        }
    }
}
