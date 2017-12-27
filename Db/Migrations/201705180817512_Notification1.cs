namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notification1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotifyConditions", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.NotifyConditions", "CompanyId");
            AddForeignKey("dbo.NotifyConditions", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotifyConditions", "CompanyId", "dbo.Companies");
            DropIndex("dbo.NotifyConditions", new[] { "CompanyId" });
            DropColumn("dbo.NotifyConditions", "CompanyId");
        }
    }
}
