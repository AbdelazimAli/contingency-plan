namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyDocAttr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyDocAttrs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StreamId = c.String(),
                        TypeId = c.Int(nullable: false),
                        Attribute = c.String(nullable: false, maxLength: 100),
                        Value = c.String(maxLength: 100),
                        CompanyDocument_stream_id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocTypeAttrs", t => new { t.TypeId, t.Attribute }, cascadeDelete: true)
                .Index(t => new { t.TypeId, t.Attribute })
                .Index(t => t.CompanyDocument_stream_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyDocAttrs", new[] { "TypeId", "Attribute" }, "dbo.DocTypeAttrs");
            DropForeignKey("dbo.CompanyDocAttrs", "CompanyDocument_stream_id", "dbo.CompanyDocsViews");
            DropIndex("dbo.CompanyDocAttrs", new[] { "CompanyDocument_stream_id" });
            DropIndex("dbo.CompanyDocAttrs", new[] { "TypeId", "Attribute" });
            DropTable("dbo.CompanyDocAttrs");
        }
    }
}
