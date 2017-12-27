namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterMenuAddTitle : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Menus", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Menus", "Title", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Menus", "Name", name: "IX_MenuName");
            CreateIndex("dbo.Menus", "Title", name: "IX_MenuTitle");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Menus", "IX_MenuTitle");
            DropIndex("dbo.Menus", "IX_MenuName");
            AlterColumn("dbo.Menus", "Title", c => c.String(maxLength: 50));
            AlterColumn("dbo.Menus", "Name", c => c.String(maxLength: 50));
        }
    }
}
