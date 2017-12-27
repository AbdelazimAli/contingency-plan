namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFunctions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Functions", "MenuId", "dbo.Menus");
            DropIndex("dbo.Functions", "IX_FunctionName");
            DropIndex("dbo.Functions", new[] { "MenuId" });
            AlterColumn("dbo.Functions", "MenuId", c => c.Int(nullable: false));
            CreateIndex("dbo.Functions", new[] { "MenuId", "Name" }, unique: true, name: "IX_FunctionName");
            AddForeignKey("dbo.Functions", "MenuId", "dbo.Menus", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Functions", "MenuId", "dbo.Menus");
            DropIndex("dbo.Functions", "IX_FunctionName");
            AlterColumn("dbo.Functions", "MenuId", c => c.Int());
            CreateIndex("dbo.Functions", "MenuId");
            CreateIndex("dbo.Functions", "Name", unique: true, name: "IX_FunctionName");
            AddForeignKey("dbo.Functions", "MenuId", "dbo.Menus", "Id");
        }
    }
}
