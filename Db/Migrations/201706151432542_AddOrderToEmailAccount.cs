namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderToEmailAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailAccounts", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmailAccounts", "Order");
        }
    }
}
