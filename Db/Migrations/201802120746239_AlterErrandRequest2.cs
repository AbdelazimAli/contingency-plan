namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterErrandRequest2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ErrandRequests", "ManagerId", c => c.Int());
            AddColumn("dbo.ErrandRequests", "MustCheckIn", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Locations", "Address1", c => c.String(maxLength: 500));
            CreateIndex("dbo.ErrandRequests", "ManagerId");
            AddForeignKey("dbo.ErrandRequests", "ManagerId", "dbo.People", "Id");
            DropColumn("dbo.Locations", "Address2");
            DropColumn("dbo.Locations", "Address3");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "Address3", c => c.String(maxLength: 100));
            AddColumn("dbo.Locations", "Address2", c => c.String(maxLength: 100));
            DropForeignKey("dbo.ErrandRequests", "ManagerId", "dbo.People");
            DropIndex("dbo.ErrandRequests", new[] { "ManagerId" });
            AlterColumn("dbo.Locations", "Address1", c => c.String(maxLength: 250));
            DropColumn("dbo.ErrandRequests", "MustCheckIn");
            DropColumn("dbo.ErrandRequests", "ManagerId");
        }
    }
}
