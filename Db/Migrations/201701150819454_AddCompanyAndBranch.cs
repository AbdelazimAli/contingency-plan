namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyAndBranch : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.EmpCustodies", new[] { "EmpId" });
            AddColumn("dbo.Assignments", "BranchId", c => c.Int());
            AddColumn("dbo.Assignments", "SectorId", c => c.Int());
            AddColumn("dbo.Custody", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.EmpCustodies", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.EmpDisciplines", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.RoleMenus", "WhereClause", c => c.String(maxLength: 500));
            AddColumn("dbo.RoleMenus", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.RoleMenus", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.RoleMenus", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.RoleMenus", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.LeaveAdjusts", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.LeaveTypes", "IsLocal", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveTypes", "CompanyId", c => c.Int());
            AddColumn("dbo.LeaveRequests", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.LeaveTrans", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.PeopleTrain", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.Terminations", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Custody", "CompanyId", name: "IX_CustodyCompany");
            CreateIndex("dbo.EmpCustodies", new[] { "CompanyId", "EmpId" }, name: "IX_EmpCustodyCompany");
            CreateIndex("dbo.EmpDisciplines", "CompanyId", name: "IX_EmpDisCompany");
            CreateIndex("dbo.LeaveAdjusts", "CompanyId", name: "IX_LeaveActionCompany");
            CreateIndex("dbo.LeaveTypes", "CompanyId");
            CreateIndex("dbo.LeaveRequests", "CompanyId", name: "IX_LeaveRequestCompany");
            CreateIndex("dbo.LeaveTrans", "CompanyId", name: "IX_LeaveTransCompany");
            CreateIndex("dbo.PeopleTrain", "CompanyId", name: "IX_PeopleTrainCompany");
            AddForeignKey("dbo.LeaveTypes", "CompanyId", "dbo.Companies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeaveTypes", "CompanyId", "dbo.Companies");
            DropIndex("dbo.PeopleTrain", "IX_PeopleTrainCompany");
            DropIndex("dbo.LeaveTrans", "IX_LeaveTransCompany");
            DropIndex("dbo.LeaveRequests", "IX_LeaveRequestCompany");
            DropIndex("dbo.LeaveTypes", new[] { "CompanyId" });
            DropIndex("dbo.LeaveAdjusts", "IX_LeaveActionCompany");
            DropIndex("dbo.EmpDisciplines", "IX_EmpDisCompany");
            DropIndex("dbo.EmpCustodies", "IX_EmpCustodyCompany");
            DropIndex("dbo.Custody", "IX_CustodyCompany");
            DropColumn("dbo.Terminations", "CompanyId");
            DropColumn("dbo.PeopleTrain", "CompanyId");
            DropColumn("dbo.LeaveTrans", "CompanyId");
            DropColumn("dbo.LeaveRequests", "CompanyId");
            DropColumn("dbo.LeaveTypes", "CompanyId");
            DropColumn("dbo.LeaveTypes", "IsLocal");
            DropColumn("dbo.LeaveAdjusts", "CompanyId");
            DropColumn("dbo.RoleMenus", "ModifiedTime");
            DropColumn("dbo.RoleMenus", "CreatedTime");
            DropColumn("dbo.RoleMenus", "ModifiedUser");
            DropColumn("dbo.RoleMenus", "CreatedUser");
            DropColumn("dbo.RoleMenus", "WhereClause");
            DropColumn("dbo.EmpDisciplines", "CompanyId");
            DropColumn("dbo.EmpCustodies", "CompanyId");
            DropColumn("dbo.Custody", "CompanyId");
            DropColumn("dbo.Assignments", "SectorId");
            DropColumn("dbo.Assignments", "BranchId");
            CreateIndex("dbo.EmpCustodies", "EmpId");
        }
    }
}
