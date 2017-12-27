namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHasImageToEmp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "HasImage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "HasImage");
        }
    }
}
