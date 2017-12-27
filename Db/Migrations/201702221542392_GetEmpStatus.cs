namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GetEmpStatus : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.LeaveRequests", new[] { "EmpId", "ReqStatus" }, name: "IX_LeaveReqStatus");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LeaveRequests", "IX_LeaveReqStatus");
        }
    }
}
