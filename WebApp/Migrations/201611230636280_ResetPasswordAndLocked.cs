namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResetPasswordAndLocked : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ResetPassword", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Locked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Locked");
            DropColumn("dbo.AspNetUsers", "ResetPassword");
        }
    }
}
