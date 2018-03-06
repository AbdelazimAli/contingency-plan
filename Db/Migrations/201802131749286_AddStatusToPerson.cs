namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusToPerson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Status", c => c.Boolean(nullable: false));
            Sql("update people set status = 1");
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "Status");
        }
    }
}
