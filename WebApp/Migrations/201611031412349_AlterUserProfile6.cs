namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUserProfile6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LoginToAllComp", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "AutoSave", c => c.Boolean(nullable: false));
            AlterColumn("dbo.AspNetUsers", "TimeZone", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "TimeZone", c => c.String(maxLength: 5));
            DropColumn("dbo.AspNetUsers", "AutoSave");
            DropColumn("dbo.AspNetUsers", "LoginToAllComp");
        }
    }
}
