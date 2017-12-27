namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotifyLetter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotifyLetters", "Sent", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyLetters", "ReadTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotifyLetters", "ReadTime");
            DropColumn("dbo.NotifyLetters", "Sent");
        }
    }
}
