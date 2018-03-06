namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterErrandRequest1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ErrandRequests", new[] { "EmpId" });
            AddColumn("dbo.ErrandRequests", "ApprovalStatus", c => c.Byte(nullable: false));
            CreateIndex("dbo.ErrandRequests", new[] { "EmpId", "StartDate", "EndDate", "ApprovalStatus" }, name: "IX_ErrandRequest");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ErrandRequests", "IX_ErrandRequest");
            DropColumn("dbo.ErrandRequests", "ApprovalStatus");
            CreateIndex("dbo.ErrandRequests", "EmpId");
        }
    }
}
