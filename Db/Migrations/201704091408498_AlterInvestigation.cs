namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterInvestigation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Investigations", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Investigations", "Name");
        }
    }
}
