namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustUserCompany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserCompanies", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserCompanies", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.AspNetUsers", "UserRoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUsers", new[] { "UserRoleId" });
            DropIndex("dbo.ApplicationUserCompanies", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserCompanies", new[] { "Company_Id" });
            CreateTable(
                "dbo.UserCompanyRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        CompanyId = c.Int(nullable: false),
                        RoleId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => new { t.UserId, t.CompanyId, t.RoleId }, unique: true, name: "IX_UserRole");

            Sql("insert into UserCompanyRoles select Id, DefaultCompany, 'e90efd54-ee33-4406-974d-8b8f0dd4567c' from AspNetUsers");
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.AspNetUsers", "UserRoleId");
            DropTable("dbo.ApplicationUserCompanies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserCompanies",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Company_Id });
            
            AddColumn("dbo.AspNetUsers", "UserRoleId", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.UserCompanyRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCompanyRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.UserCompanyRoles", "CompanyId", "dbo.Companies");
            DropIndex("dbo.UserCompanyRoles", "IX_UserRole");
            DropTable("dbo.UserCompanyRoles");
            CreateIndex("dbo.ApplicationUserCompanies", "Company_Id");
            CreateIndex("dbo.ApplicationUserCompanies", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "UserRoleId");
            AddForeignKey("dbo.AspNetUsers", "UserRoleId", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.ApplicationUserCompanies", "Company_Id", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserCompanies", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
