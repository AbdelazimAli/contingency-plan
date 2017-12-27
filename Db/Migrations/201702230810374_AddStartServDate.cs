namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStartServDate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LeaveRequests", "IX_LeaveReqStatus");
            AddColumn("dbo.Terminations", "ServStartDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.LeaveRequests", new[] { "EmpId", "ReqStatus", "ActualStartDate", "ActualEndDate" }, name: "IX_LeaveReqStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LeaveRequests", "IX_LeaveReqStatus");
            DropColumn("dbo.Terminations", "ServStartDate");
            CreateIndex("dbo.LeaveRequests", new[] { "EmpId", "ReqStatus" }, name: "IX_LeaveReqStatus");
        }
    }
}
