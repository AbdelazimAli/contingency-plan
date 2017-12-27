namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterDocTypes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DocTypeJobs", newName: "JobDocTypes");
            DropPrimaryKey("dbo.JobDocTypes");
            CreateTable(
                "dbo.DocTypeCountries",
                c => new
                    {
                        DocType_Id = c.Int(nullable: false),
                        Country_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DocType_Id, t.Country_Id })
                .ForeignKey("dbo.DocTypes", t => t.DocType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.Country_Id, cascadeDelete: true)
                .Index(t => t.DocType_Id)
                .Index(t => t.Country_Id);
            
            AddColumn("dbo.DocTypes", "Gender", c => c.Short(nullable: false));
            AddColumn("dbo.PersonSetup", "MaxPassTrials", c => c.Byte(nullable: false));
            AddPrimaryKey("dbo.JobDocTypes", new[] { "Job_Id", "DocType_Id" });
            DropColumn("dbo.PersonSetup", "MaxAge");
            DropColumn("dbo.PersonSetup", "MinAge");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PersonSetup", "MinAge", c => c.Byte());
            AddColumn("dbo.PersonSetup", "MaxAge", c => c.Byte());
            DropForeignKey("dbo.DocTypeCountries", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.DocTypeCountries", "DocType_Id", "dbo.DocTypes");
            DropIndex("dbo.DocTypeCountries", new[] { "Country_Id" });
            DropIndex("dbo.DocTypeCountries", new[] { "DocType_Id" });
            DropPrimaryKey("dbo.JobDocTypes");
            DropColumn("dbo.PersonSetup", "MaxPassTrials");
            DropColumn("dbo.DocTypes", "Gender");
            DropTable("dbo.DocTypeCountries");
            AddPrimaryKey("dbo.JobDocTypes", new[] { "DocType_Id", "Job_Id" });
            RenameTable(name: "dbo.JobDocTypes", newName: "DocTypeJobs");
        }
    }
}
