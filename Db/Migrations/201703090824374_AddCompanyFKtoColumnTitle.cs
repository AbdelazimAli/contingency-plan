namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyFKtoColumnTitle : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ColumnTitles", "CompanyId");
            AddForeignKey("dbo.ColumnTitles", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ColumnTitles", "CompanyId", "dbo.Companies");
            DropIndex("dbo.ColumnTitles", new[] { "CompanyId" });
        }
    }
}
