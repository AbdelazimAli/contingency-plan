namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTimeZone : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "TimeZone", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "TimeZone", c => c.String(maxLength: 10));
        }
    }
}
