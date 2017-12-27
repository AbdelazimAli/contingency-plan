namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSSMenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "SSMenu", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menus", "SSMenu");
        }
    }
}
