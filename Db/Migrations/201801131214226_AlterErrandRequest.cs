namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterErrandRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ErrandRequests", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.ErrandRequests", "CompanyId");
            AddForeignKey("dbo.ErrandRequests", "CompanyId", "dbo.Companies", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ErrandRequests", "CompanyId", "dbo.Companies");
            DropIndex("dbo.ErrandRequests", new[] { "CompanyId" });
            DropColumn("dbo.ErrandRequests", "CompanyId");
        }
    }
}
