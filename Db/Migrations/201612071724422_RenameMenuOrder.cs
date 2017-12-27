namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameMenuOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Order", c => c.Int(nullable: false));
            DropColumn("dbo.Menus", "MenuOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "MenuOrder", c => c.Int(nullable: false));
            DropColumn("dbo.Menus", "Order");
        }
    }
}
