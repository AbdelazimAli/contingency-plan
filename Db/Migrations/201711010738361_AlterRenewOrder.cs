namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterRenewOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RenewRequests", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.RenewRequests", "OldValueId", c => c.Int());
            AddColumn("dbo.RenewRequests", "NewValueId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RenewRequests", "NewValueId");
            DropColumn("dbo.RenewRequests", "OldValueId");
            DropColumn("dbo.RenewRequests", "CompanyId");
        }
    }
}
