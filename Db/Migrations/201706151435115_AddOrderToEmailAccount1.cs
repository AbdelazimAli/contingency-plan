namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderToEmailAccount1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailAccounts", "SendOrder", c => c.Int(nullable: false));
            DropColumn("dbo.EmailAccounts", "Order");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmailAccounts", "Order", c => c.Int(nullable: false));
            DropColumn("dbo.EmailAccounts", "SendOrder");
        }
    }
}
