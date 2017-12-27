namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmpId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmpId", c => c.Int());
            AddColumn("dbo.AspNetRoles", "SSRole", c => c.Boolean());
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.AspNetRoles", "SSRole");
            DropColumn("dbo.AspNetUsers", "EmpId");
        }
    }
}
