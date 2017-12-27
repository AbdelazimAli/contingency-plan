namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMenuinPageDiv : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PageDivs", "Menu_Id", "dbo.Menus");
            DropIndex("dbo.PageDivs", new[] { "Menu_Id" });
            RenameColumn(table: "dbo.PageDivs", name: "Menu_Id", newName: "MenuId");
            AlterColumn("dbo.Employements", "Code", c => c.String(nullable: false, maxLength: 20));
            Sql("UPDATE dbo.PageDivs Set MenuId = 2");
            AlterColumn("dbo.PageDivs", "MenuId", c => c.Int(nullable: false));
            CreateIndex("dbo.PageDivs", "MenuId");
            AddForeignKey("dbo.PageDivs", "MenuId", "dbo.Menus", "Id", cascadeDelete: false);
            DropColumn("dbo.PageDivs", "MenuName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PageDivs", "MenuName", c => c.String(maxLength: 50));
            DropForeignKey("dbo.PageDivs", "MenuId", "dbo.Menus");
            DropIndex("dbo.PageDivs", new[] { "MenuId" });
            AlterColumn("dbo.PageDivs", "MenuId", c => c.Int());
            AlterColumn("dbo.Employements", "Code", c => c.String(maxLength: 20));
            RenameColumn(table: "dbo.PageDivs", name: "MenuId", newName: "Menu_Id");
            CreateIndex("dbo.PageDivs", "Menu_Id");
            AddForeignKey("dbo.PageDivs", "Menu_Id", "dbo.Menus", "Id");
        }
    }
}
