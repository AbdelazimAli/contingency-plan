namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterHReports1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.HReports", "IX_HReport");
            CreateIndex("dbo.HReports", new[] { "ReportTitle", "Language" }, unique: true, name: "IX_HReport");
        }
        
        public override void Down()
        {
            DropIndex("dbo.HReports", "IX_HReport");
            CreateIndex("dbo.HReports", new[] { "ReportName", "Language" }, unique: true, name: "IX_HReport");
        }
    }
}
