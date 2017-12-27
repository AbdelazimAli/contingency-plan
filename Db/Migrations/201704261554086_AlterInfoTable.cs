namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterInfoTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LinkTables", "IX_GenTableName");
            AddColumn("dbo.InfoTables", "IPoints", c => c.Boolean(nullable: false));
            AddColumn("dbo.LinkTables", "Point", c => c.String(maxLength: 20));
            AddColumn("dbo.LinkTables", "MinValue", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LinkTables", "MaxValue", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.InfoTables", "FillOptns");
            DropColumn("dbo.LinkTables", "ShortName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LinkTables", "ShortName", c => c.String(maxLength: 30));
            AddColumn("dbo.InfoTables", "FillOptns", c => c.Byte());
            DropColumn("dbo.LinkTables", "MaxValue");
            DropColumn("dbo.LinkTables", "MinValue");
            DropColumn("dbo.LinkTables", "Point");
            DropColumn("dbo.InfoTables", "IPoints");
            CreateIndex("dbo.LinkTables", "ShortName", name: "IX_GenTableName");
        }
    }
}
