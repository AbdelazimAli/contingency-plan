namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RestrictPosition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Positions", "DeptId", "dbo.CompanyStructures");
            DropIndex("dbo.Positions", new[] { "DeptId" });
            AddColumn("dbo.CompanyDocuments", "Source", c => c.String(maxLength: 10));
            AlterColumn("dbo.Positions", "DeptId", c => c.Int());
            CreateIndex("dbo.Positions", "DeptId");
            AddForeignKey("dbo.Positions", "DeptId", "dbo.CompanyStructures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Positions", "DeptId", "dbo.CompanyStructures");
            DropIndex("dbo.Positions", new[] { "DeptId" });
            AlterColumn("dbo.Positions", "DeptId", c => c.Int(nullable: false));
            DropColumn("dbo.CompanyDocsViews", "Source");
            CreateIndex("dbo.Positions", "DeptId");
            AddForeignKey("dbo.Positions", "DeptId", "dbo.CompanyStructures", "Id", cascadeDelete: true);
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
