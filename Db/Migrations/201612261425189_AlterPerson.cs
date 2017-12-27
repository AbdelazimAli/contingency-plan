namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPerson : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.People", "PersonType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "PersonType", c => c.Byte(nullable: false));
        }
    }
}
