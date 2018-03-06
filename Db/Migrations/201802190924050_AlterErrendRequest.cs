namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterErrendRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ErrandRequests", "Expenses", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ErrandRequests", "Notes", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ErrandRequests", "Notes");
            DropColumn("dbo.ErrandRequests", "Expenses");
        }
    }
}
