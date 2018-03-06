namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CanCustomize", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Developer", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "UserRoleId", c => c.String(maxLength: 128));
            Sql("update AspNetUsers set CanCustomize=1, Developer=1, UserRoleId='e90efd54-ee33-4406-974d-8b8f0dd4567c'");
            Sql("delete from AspNetRoles where Name in ('Developer','Admin','Configuration')");
            CreateIndex("dbo.AspNetUsers", "UserRoleId");
            AddForeignKey("dbo.AspNetUsers", "UserRoleId", "dbo.AspNetRoles", "Id");
            DropColumn("dbo.AspNetRoles", "SSRole");
            DropColumn("dbo.AspNetUsers", "Locked");
            DropColumn("dbo.AspNetUsers", "SSUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "SSUser", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Locked", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetRoles", "SSRole", c => c.Boolean());
            DropForeignKey("dbo.AspNetUsers", "UserRoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUsers", new[] { "UserRoleId" });
            DropColumn("dbo.AspNetUsers", "UserRoleId");
            DropColumn("dbo.AspNetUsers", "Developer");
            DropColumn("dbo.AspNetUsers", "CanCustomize");
        }
    }
}
