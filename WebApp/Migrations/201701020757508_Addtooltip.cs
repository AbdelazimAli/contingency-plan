namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addtooltip : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LogTooltip", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "LogTooltip");
        }
    }
}
