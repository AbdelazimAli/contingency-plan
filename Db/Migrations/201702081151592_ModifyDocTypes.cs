namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyDocTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocTypeJobs",
                c => new
                    {
                        DocType_Id = c.Int(nullable: false),
                        Job_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DocType_Id, t.Job_Id })
                .ForeignKey("dbo.DocTypes", t => t.DocType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Jobs", t => t.Job_Id, cascadeDelete: true)
                .Index(t => t.DocType_Id)
                .Index(t => t.Job_Id);
            
            AddColumn("dbo.DocTypes", "IsLocal", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocTypes", "CompanyId", c => c.Int());
            AddColumn("dbo.DocTypes", "RequiredOpt", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocTypeJobs", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.DocTypeJobs", "DocType_Id", "dbo.DocTypes");
            DropIndex("dbo.DocTypeJobs", new[] { "Job_Id" });
            DropIndex("dbo.DocTypeJobs", new[] { "DocType_Id" });
            DropColumn("dbo.DocTypes", "RequiredOpt");
            DropColumn("dbo.DocTypes", "CompanyId");
            DropColumn("dbo.DocTypes", "IsLocal");
            DropTable("dbo.DocTypeJobs");
        }
    }
}
