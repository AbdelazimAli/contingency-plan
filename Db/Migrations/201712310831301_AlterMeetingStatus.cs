namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterMeetingStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meetings", "IsActivate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Meetings", "IsActivate");
        }
    }
}
