namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUserCompany : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UserCompanyRoles", "IX_UserRole");
            CreateIndex("dbo.UserCompanyRoles", new[] { "UserId", "CompanyId" }, unique: true, name: "IX_UserRole");
            CreateIndex("dbo.UserCompanyRoles", "RoleId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserCompanyRoles", new[] { "RoleId" });
            DropIndex("dbo.UserCompanyRoles", "IX_UserRole");
            CreateIndex("dbo.UserCompanyRoles", new[] { "UserId", "CompanyId", "RoleId" }, unique: true, name: "IX_UserRole");
        }
    }
}
