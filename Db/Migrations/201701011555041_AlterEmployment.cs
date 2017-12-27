namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmployment : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FlexData", "IX_FlexData");
            AddColumn("dbo.Employements", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.Employements", "Sequence", c => c.Int());
            AlterColumn("dbo.Employements", "Code", c => c.String(maxLength: 20));
            CreateIndex("dbo.Employements", "CompanyId");
            CreateIndex("dbo.FlexData", new[] { "PageId", "ColumnName", "SourceId" }, unique: true, name: "IX_FlexData");
            AddForeignKey("dbo.Employements", "CompanyId", "dbo.Companies", "Id", cascadeDelete: false);
            DropColumn("dbo.FlexData", "Source");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FlexData", "Source", c => c.String(nullable: false, maxLength: 10));
            DropForeignKey("dbo.Employements", "CompanyId", "dbo.Companies");
            DropIndex("dbo.FlexData", "IX_FlexData");
            DropIndex("dbo.Employements", new[] { "CompanyId" });
            AlterColumn("dbo.Employements", "Code", c => c.String(maxLength: 15));
            DropColumn("dbo.Employements", "Sequence");
            DropColumn("dbo.Employements", "CompanyId");
            CreateIndex("dbo.FlexData", new[] { "PageId", "Source", "SourceId", "ColumnName" }, unique: true, name: "IX_FlexData");
        }
    }
}
