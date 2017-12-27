namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkflow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Workflow",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        Source = c.String(maxLength: 20),
                        IsRequired = c.Boolean(nullable: false),
                        ModifiedUser = c.String(maxLength: 20),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Workflow", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Workflow", new[] { "CompanyId" });
            DropTable("dbo.Workflow");
        }
    }
}
