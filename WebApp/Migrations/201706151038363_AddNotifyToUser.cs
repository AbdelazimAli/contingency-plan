namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifyToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "WebNotify", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "EmailNotify", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "SmsNotify", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SmsNotify");
            DropColumn("dbo.AspNetUsers", "EmailNotify");
            DropColumn("dbo.AspNetUsers", "WebNotify");
        }
    }
}
