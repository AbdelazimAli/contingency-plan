namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSSRole2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "SSRole", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "SSRole");
        }
    }
}
