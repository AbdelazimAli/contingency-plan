namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPagePrint : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PagePrints", new[] { "CompanyId" });
            DropPrimaryKey("dbo.PagePrints");
            AddColumn("dbo.Languages", "IsEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.PagePrints", "Culture", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.Languages", "LanguageCulture", c => c.String(maxLength: 15));
            AlterColumn("dbo.PagePrints", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.PagePrints", "Culture");
            CreateIndex("dbo.PagePrints", new[] { "CompanyId", "ObjectName", "Version", "Culture" }, unique: true, name: "IX_PagePrint");
            CreateIndex("dbo.PagePrints", "LetterTempl", unique: true);
            DropColumn("dbo.Languages", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Languages", "Active", c => c.Boolean(nullable: false));
            DropIndex("dbo.PagePrints", new[] { "LetterTempl" });
            DropIndex("dbo.PagePrints", "IX_PagePrint");
            DropPrimaryKey("dbo.PagePrints");
            AlterColumn("dbo.PagePrints", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Languages", "LanguageCulture", c => c.String(maxLength: 20));
            DropColumn("dbo.PagePrints", "Culture");
            DropColumn("dbo.Languages", "IsEnabled");
            AddPrimaryKey("dbo.PagePrints", "Id");
            CreateIndex("dbo.PagePrints", "CompanyId");
        }
    }
}
