namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSort : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CompanyStructures", "IX_StructureSort");
            DropIndex("dbo.CompanyStructures", new[] { "CompanyId" });
            DropIndex("dbo.Menus", new[] { "CompanyId" });
            DropIndex("dbo.Menus", "IX_MenuSort");
            CreateIndex("dbo.CompanyStructures", new[] { "CompanyId", "Sort" }, unique: true, name: "IX_StructureSort");
            CreateIndex("dbo.Menus", new[] { "CompanyId", "Sort" }, unique: true, name: "IX_MenuSort");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Menus", "IX_MenuSort");
            DropIndex("dbo.CompanyStructures", "IX_StructureSort");
            CreateIndex("dbo.Menus", "Sort", unique: true, name: "IX_MenuSort");
            CreateIndex("dbo.Menus", "CompanyId");
            CreateIndex("dbo.CompanyStructures", "CompanyId");
            CreateIndex("dbo.CompanyStructures", "Sort", unique: true, name: "IX_StructureSort");
        }
    }
}
