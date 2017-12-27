namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSSRole : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetRoles", "SSRole");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "SSRole", c => c.Boolean());
        }
    }
}
