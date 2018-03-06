namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddErrandRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ErrandRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpId = c.Int(nullable: false),
                        LocationId = c.Int(),
                        Subject = c.String(maxLength: 500),
                        MultiDays = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .Index(t => t.EmpId)
                .Index(t => t.LocationId);
            
            AlterColumn("dbo.LeaveRequests", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveRequests", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveRequests", "ActualStartDate", c => c.DateTime());
            AlterColumn("dbo.LeaveRequests", "ActualEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ErrandRequests", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.ErrandRequests", "EmpId", "dbo.People");
            DropIndex("dbo.ErrandRequests", new[] { "LocationId" });
            DropIndex("dbo.ErrandRequests", new[] { "EmpId" });
            AlterColumn("dbo.LeaveRequests", "ActualEndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.LeaveRequests", "ActualStartDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.LeaveRequests", "EndDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.LeaveRequests", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            DropTable("dbo.ErrandRequests");
        }
    }
}
