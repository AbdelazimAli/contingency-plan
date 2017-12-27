namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIconToReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HReports", "Icon", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HReports", "Icon");
        }
    }
}
