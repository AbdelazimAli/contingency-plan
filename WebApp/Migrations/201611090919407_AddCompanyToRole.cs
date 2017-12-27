namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyToRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SuperUser", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetRoles", "CompanyId", c => c.Int());
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AspNetRoles", "CompanyId");
            AddForeignKey("dbo.AspNetRoles", "CompanyId", "dbo.Companies", "Id");
            DropColumn("dbo.AspNetUsers", "LoginToAllComp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LoginToAllComp", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.AspNetRoles", "CompanyId", "dbo.Companies");
            DropIndex("dbo.AspNetRoles", new[] { "CompanyId" });
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.AspNetRoles", "CompanyId");
            DropColumn("dbo.AspNetUsers", "SuperUser");
        }
    }
}
