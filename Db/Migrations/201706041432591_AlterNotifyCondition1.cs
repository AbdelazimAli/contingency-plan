namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotifyCondition1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotifyConditions", "AlertMeUntil", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotifyConditions", "AlertMeUntil");
        }
    }
}
