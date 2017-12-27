namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmailAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailAccounts", "Capacity", c => c.Int(nullable: false));
            AddColumn("dbo.EmailAccounts", "LastSentDate", c => c.DateTime());
            AddColumn("dbo.EmailAccounts", "TodayCount", c => c.Int(nullable: false));
            DropColumn("dbo.NotifyConditions", "WebOrMob");
            DropColumn("dbo.NotifyConditions", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NotifyConditions", "Email", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyConditions", "WebOrMob", c => c.Boolean(nullable: false));
            DropColumn("dbo.EmailAccounts", "TodayCount");
            DropColumn("dbo.EmailAccounts", "LastSentDate");
            DropColumn("dbo.EmailAccounts", "Capacity");
        }
    }
}
