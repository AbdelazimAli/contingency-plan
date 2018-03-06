namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPersonSetup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonSetup", "WaitingInMinutes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonSetup", "WaitingInMinutes");
        }
    }
}
