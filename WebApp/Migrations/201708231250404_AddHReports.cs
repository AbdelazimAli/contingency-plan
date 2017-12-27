namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHReports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        MenuId = c.Int(nullable: false),
                        OrgReportId = c.Int(),
                        ReportName = c.String(maxLength: 50),
                        ReportData = c.Binary(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HReports", t => t.OrgReportId)
                .Index(t => t.MenuId, name: "IX_HReportMenu")
                .Index(t => t.OrgReportId, name: "IX_HReportOrgRepor");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HReports", "OrgReportId", "dbo.HReports");
            DropIndex("dbo.HReports", "IX_HReportOrgRepor");
            DropIndex("dbo.HReports", "IX_HReportMenu");
            DropTable("dbo.HReports");
        }
    }
}
