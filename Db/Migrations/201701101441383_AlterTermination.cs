namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTermination : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Terminations", "ApprovalStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.Terminations", "Terminated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Terminations", "Terminated");
            DropColumn("dbo.Terminations", "ApprovalStatus");
        }
    }
}
