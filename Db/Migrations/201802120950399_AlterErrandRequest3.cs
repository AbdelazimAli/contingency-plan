namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterErrandRequest3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ErrandRequests", "ErrandType", c => c.Byte(nullable: false));
            AddColumn("dbo.ErrandRequests", "Reason", c => c.String(maxLength: 500));
            DropColumn("dbo.ErrandRequests", "MustCheckIn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ErrandRequests", "MustCheckIn", c => c.Boolean(nullable: false));
            DropColumn("dbo.ErrandRequests", "Reason");
            DropColumn("dbo.ErrandRequests", "ErrandType");
        }
    }
}
