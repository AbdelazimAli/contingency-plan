namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameToNamesTbl : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.NameTbls", newName: "NamesTbl");
            DropPrimaryKey("dbo.NamesTbl");
            AlterColumn("dbo.NamesTbl", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.NamesTbl", "Title", c => c.String(maxLength: 100));
            AddPrimaryKey("dbo.NamesTbl", new[] { "Culture", "Name" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.NamesTbl");
            AlterColumn("dbo.NamesTbl", "Title", c => c.String(maxLength: 50));
            AlterColumn("dbo.NamesTbl", "Name", c => c.String(nullable: false, maxLength: 50));
            AddPrimaryKey("dbo.NamesTbl", new[] { "Culture", "Name" });
            RenameTable(name: "dbo.NamesTbl", newName: "NameTbls");
        }
    }
}
