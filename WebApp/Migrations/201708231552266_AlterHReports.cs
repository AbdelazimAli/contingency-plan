namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterHReports : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.HReports", "IX_HReportMenu");
            AddColumn("dbo.HReports", "MenuName", c => c.String(maxLength: 50));
            CreateIndex("dbo.HReports", "MenuName", name: "IX_HReportMenu");
            DropColumn("dbo.HReports", "CompanyId");
            DropColumn("dbo.HReports", "MenuId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HReports", "MenuId", c => c.Int(nullable: false));
            AddColumn("dbo.HReports", "CompanyId", c => c.Int(nullable: false));
            DropIndex("dbo.HReports", "IX_HReportMenu");
            DropColumn("dbo.HReports", "MenuName");
            CreateIndex("dbo.HReports", "MenuId", name: "IX_HReportMenu");
        }
    }
}
