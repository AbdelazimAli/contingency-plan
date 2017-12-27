namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCompanyDocAttr : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyDocAttrs", "CompanyDocument_stream_id", "dbo.CompanyDocsViews");
            DropIndex("dbo.CompanyDocAttrs", new[] { "CompanyDocument_stream_id" });
            DropColumn("dbo.CompanyDocAttrs", "CompanyDocument_stream_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CompanyDocAttrs", "CompanyDocument_stream_id", c => c.Guid());
            CreateIndex("dbo.CompanyDocAttrs", "CompanyDocument_stream_id");
            AddForeignKey("dbo.CompanyDocAttrs", "CompanyDocument_stream_id", "dbo.CompanyDocsViews", "stream_id");
        }
    }
}
