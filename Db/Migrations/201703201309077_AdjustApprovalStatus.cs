namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustApprovalStatus : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LeaveRequests", "IX_LeaveRequestCompany");
            DropIndex("dbo.LeaveRequests", "IX_LeaveReqStatus");
            CreateIndex("dbo.ComplainRequests", new[] { "CompanyId", "ApprovalStatus" }, name: "IX_ComplainReqStatus");
            CreateIndex("dbo.LeaveRequests", new[] { "CompanyId", "ApprovalStatus" }, name: "IX_LeaveReqStatus");
            CreateIndex("dbo.LeaveRequests", new[] { "EmpId", "ApprovalStatus" }, name: "IX_LeaveEmpRequest");
            CreateIndex("dbo.Terminations", new[] { "CompanyId", "ApprovalStatus" }, name: "IX_TermReqStatus");
            DropColumn("dbo.ComplainRequests", "IsDeleted");
            DropColumn("dbo.LeaveRequests", "IsDeleted");
            DropColumn("dbo.Terminations", "IsDeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Terminations", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveRequests", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ComplainRequests", "IsDeleted", c => c.Boolean(nullable: false));
            DropIndex("dbo.Terminations", "IX_TermReqStatus");
            DropIndex("dbo.LeaveRequests", "IX_LeaveEmpRequest");
            DropIndex("dbo.LeaveRequests", "IX_LeaveReqStatus");
            DropIndex("dbo.ComplainRequests", "IX_ComplainReqStatus");
            CreateIndex("dbo.LeaveRequests", new[] { "EmpId", "ReqStatus", "ActualStartDate", "ActualEndDate" }, name: "IX_LeaveReqStatus");
            CreateIndex("dbo.LeaveRequests", "CompanyId", name: "IX_LeaveRequestCompany");
        }
    }
}
