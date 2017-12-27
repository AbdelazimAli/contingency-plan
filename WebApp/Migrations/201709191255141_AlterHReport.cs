namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterHReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HReports", "ReportTitle", c => c.String(maxLength: 50));
            AddColumn("dbo.HReports", "ReportParm", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HReports", "ReportParm");
            DropColumn("dbo.HReports", "ReportTitle");
        }
    }
}
