namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCompanyDocAttr3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyDocAttrs", new[] { "TypeId", "Attribute" }, "dbo.DocTypeAttrs");
            DropIndex("dbo.CompanyDocAttrs", new[] { "TypeId", "Attribute" });
            DropIndex("dbo.DocTypeAttrs", new[] { "TypeId" });
            DropPrimaryKey("dbo.DocTypeAttrs");
            AddColumn("dbo.DocTypeAttrs", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.CompanyDocAttrs", "StreamId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.DocTypeAttrs", "Id");
            CreateIndex("dbo.CompanyDocAttrs", "StreamId");
            CreateIndex("dbo.CompanyDocAttrs", "TypeId");
            CreateIndex("dbo.DocTypeAttrs", new[] { "TypeId", "Attribute" }, unique: true, name: "IX_DocTypeAttr");
            AddForeignKey("dbo.CompanyDocAttrs", "StreamId", "dbo.CompanyDocuments", "stream_id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyDocAttrs", "TypeId", "dbo.DocTypeAttrs", "Id", cascadeDelete: true);
            DropColumn("dbo.CompanyDocAttrs", "Attribute");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanyDocAttrs", "Attribute", c => c.String(nullable: false, maxLength: 100));
            DropForeignKey("dbo.CompanyDocAttrs", "TypeId", "dbo.DocTypeAttrs");
            DropForeignKey("dbo.CompanyDocAttrs", "StreamId", "dbo.CompanyDocsViews");
            DropIndex("dbo.DocTypeAttrs", "IX_DocTypeAttr");
            DropIndex("dbo.CompanyDocAttrs", new[] { "TypeId" });
            DropIndex("dbo.CompanyDocAttrs", new[] { "StreamId" });
            DropPrimaryKey("dbo.DocTypeAttrs");
            AlterColumn("dbo.CompanyDocAttrs", "StreamId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.DocTypeAttrs", "Id");
            AddPrimaryKey("dbo.DocTypeAttrs", new[] { "TypeId", "Attribute" });
            CreateIndex("dbo.DocTypeAttrs", "TypeId");
            CreateIndex("dbo.CompanyDocAttrs", new[] { "TypeId", "Attribute" });
            AddForeignKey("dbo.CompanyDocAttrs", new[] { "TypeId", "Attribute" }, "dbo.DocTypeAttrs", new[] { "TypeId", "Attribute" }, cascadeDelete: true);
        }
    }
}
