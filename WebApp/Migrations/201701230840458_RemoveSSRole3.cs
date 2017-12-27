namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSSRole3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetRoles", "SSRole");
            DropColumn("dbo.AspNetRoles", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetRoles", "SSRole", c => c.Boolean());
        }
    }
}
