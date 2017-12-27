namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyIdToPosition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Positions", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Positions", "CompanyId");
            AddForeignKey("dbo.Positions", "CompanyId", "dbo.Companies", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Positions", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Positions", new[] { "CompanyId" });
            DropColumn("dbo.Positions", "CompanyId");
        }
    }
}
