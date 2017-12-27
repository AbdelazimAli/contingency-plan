namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyToUserLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLogs", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.UserLogs", "CompanyId");
            AddForeignKey("dbo.UserLogs", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLogs", "CompanyId", "dbo.Companies");
            DropIndex("dbo.UserLogs", new[] { "CompanyId" });
            DropColumn("dbo.UserLogs", "CompanyId");
        }
    }
}
