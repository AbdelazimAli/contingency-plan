namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyToBudget : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Budgets", "IsLocal", c => c.Boolean(nullable: false));
            AddColumn("dbo.Budgets", "CompanyId", c => c.Int());
            CreateIndex("dbo.Budgets", "CompanyId");
            AddForeignKey("dbo.Budgets", "CompanyId", "dbo.Companies", "Id");
            RenameColumn("dbo.LookUpCode", "Protectet", "Protected");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LookUpCode", "Protectet", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Budgets", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Budgets", new[] { "CompanyId" });
            DropColumn("dbo.LookUpCode", "Protected");
            DropColumn("dbo.Budgets", "CompanyId");
            DropColumn("dbo.Budgets", "IsLocal");
        }
    }
}
