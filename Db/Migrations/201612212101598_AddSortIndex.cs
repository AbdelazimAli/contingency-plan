namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSortIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CompanyStructures", "Sort", unique: true, name: "IX_StructureSort");
            CreateIndex("dbo.Menus", "Sort", unique: true, name: "IX_MenuSort");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Menus", "IX_MenuSort");
            DropIndex("dbo.CompanyStructures", "IX_StructureSort");
        }
    }
}
