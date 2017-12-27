namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameToNameTbl : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MenuTitles", newName: "NameTbls");
            DropColumn("dbo.GridColumns", "DefaultTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GridColumns", "DefaultTitle", c => c.String(maxLength: 50));
            RenameTable(name: "dbo.NameTbls", newName: "MenuTitles");
        }
    }
}
