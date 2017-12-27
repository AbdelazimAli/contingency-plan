namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterHReport1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.HReports", "IX_HReportMenu");
            AddColumn("dbo.HReports", "Language", c => c.String(maxLength: 15));
            CreateIndex("dbo.HReports", new[] { "MenuName", "Language" }, name: "IX_HReportMenu");
            CreateIndex("dbo.HReports", new[] { "ReportName", "Language" }, unique: true, name: "IX_HReport");
            DropColumn("dbo.HReports", "ReportParm");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HReports", "ReportParm", c => c.String(maxLength: 50));
            DropIndex("dbo.HReports", "IX_HReport");
            DropIndex("dbo.HReports", "IX_HReportMenu");
            DropColumn("dbo.HReports", "Language");
            CreateIndex("dbo.HReports", "MenuName", name: "IX_HReportMenu");
        }
    }
}
