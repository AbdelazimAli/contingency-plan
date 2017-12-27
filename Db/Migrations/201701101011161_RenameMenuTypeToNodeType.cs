namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameMenuTypeToNodeType : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.CompanyStructures", "StructureType", "NodeType");
            RenameColumn("dbo.Menus", "MenuType", "NodeType");
        }
        
        public override void Down()
        {
           
        }
    }
}
