namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCompanyfromRoles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetRoles", "CompanyId", "dbo.Companies");
            DropIndex("dbo.AspNetRoles", new[] { "CompanyId" });
            DropColumn("dbo.AspNetRoles", "CompanyId");
            DropColumn("dbo.AspNetRoles", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetRoles", "CompanyId", c => c.Int());
            CreateIndex("dbo.AspNetRoles", "CompanyId");
            AddForeignKey("dbo.AspNetRoles", "CompanyId", "dbo.Companies", "Id");
        }
    }
}
