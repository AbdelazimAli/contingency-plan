namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMenuFromFunctions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Functions", "MenuId", "dbo.Menus");
            DropIndex("dbo.Functions", "IX_FunctionName");
            CreateIndex("dbo.Functions", "Name", unique: true, name: "IX_FunctionName");
            DropColumn("dbo.Functions", "MenuId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Functions", "MenuId", c => c.Int(nullable: false));
            DropIndex("dbo.Functions", "IX_FunctionName");
            CreateIndex("dbo.Functions", new[] { "MenuId", "Name" }, unique: true, name: "IX_FunctionName");
            AddForeignKey("dbo.Functions", "MenuId", "dbo.Menus", "Id", cascadeDelete: true);
        }
    }
}
