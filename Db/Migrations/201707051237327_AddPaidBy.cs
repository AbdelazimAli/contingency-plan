namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaidBy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BenefitRequests", "PaidBy", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BenefitRequests", "PaidBy");
        }
    }
}
