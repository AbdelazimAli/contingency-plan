namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterWorkFlow : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WfTrans", "IX_WfTrans");
            //DropPrimaryKey("dbo.V_WF_TRANS");
            //AddColumn("dbo.V_WF_TRANS", "Source", c => c.String(nullable: false, maxLength: 128));
            //AddColumn("dbo.V_WF_TRANS", "SourceId", c => c.Int(nullable: false));
            //AddColumn("dbo.V_WF_TRANS", "CompanyId", c => c.Int(nullable: false));
            //AddColumn("dbo.V_WF_TRANS", "BranchId", c => c.Int());
            //AddColumn("dbo.V_WF_TRANS", "SectorId", c => c.Int());
            //AddColumn("dbo.V_WF_TRANS", "AuthBranch", c => c.Int());
            //AddColumn("dbo.V_WF_TRANS", "AuthDept", c => c.Int());
            //AddColumn("dbo.V_WF_TRANS", "AuthPosition", c => c.Int());
            //AddColumn("dbo.V_WF_TRANS", "AuthEmp", c => c.Int());
            AddColumn("dbo.WfTrans", "Source", c => c.String(maxLength: 20));
            AddColumn("dbo.WfTrans", "SourceId", c => c.Int(nullable: false));
            AddColumn("dbo.WfTrans", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.WfTrans", "BranchId", c => c.Int());
            AddColumn("dbo.WfTrans", "SectorId", c => c.Int());
            AddColumn("dbo.WfTrans", "AuthBranch", c => c.Int());
            AddColumn("dbo.WfTrans", "AuthDept", c => c.Int());
            AddColumn("dbo.WfTrans", "AuthPosition", c => c.Int());
            AddColumn("dbo.WfTrans", "AuthEmp", c => c.Int());
           // AddPrimaryKey("dbo.V_WF_TRANS", new[] { "Source", "SourceId", "DocumentId" });
            CreateIndex("dbo.Assignments", "BranchId", name: "IX_AssignmentBranch");
            CreateIndex("dbo.Assignments", "CompanyId", name: "IX_AssignmentCompany");
            CreateIndex("dbo.WfTrans", new[] { "Source", "SourceId", "DocumentId", "Sequence" }, unique: true, name: "IX_WfTrans");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WfTrans", "IX_WfTrans");
            DropIndex("dbo.Assignments", "IX_AssignmentCompany");
            DropIndex("dbo.Assignments", "IX_AssignmentBranch");
            DropPrimaryKey("dbo.V_WF_TRANS");
            DropColumn("dbo.WfTrans", "AuthEmp");
            DropColumn("dbo.WfTrans", "AuthPosition");
            DropColumn("dbo.WfTrans", "AuthDept");
            DropColumn("dbo.WfTrans", "AuthBranch");
            DropColumn("dbo.WfTrans", "SectorId");
            DropColumn("dbo.WfTrans", "BranchId");
            DropColumn("dbo.WfTrans", "CompanyId");
            DropColumn("dbo.WfTrans", "SourceId");
            DropColumn("dbo.WfTrans", "Source");
            DropColumn("dbo.V_WF_TRANS", "AuthEmp");
            DropColumn("dbo.V_WF_TRANS", "AuthPosition");
            DropColumn("dbo.V_WF_TRANS", "AuthDept");
            DropColumn("dbo.V_WF_TRANS", "AuthBranch");
            DropColumn("dbo.V_WF_TRANS", "SectorId");
            DropColumn("dbo.V_WF_TRANS", "BranchId");
            DropColumn("dbo.V_WF_TRANS", "CompanyId");
            DropColumn("dbo.V_WF_TRANS", "SourceId");
            DropColumn("dbo.V_WF_TRANS", "Source");
            AddPrimaryKey("dbo.V_WF_TRANS", new[] { "WFlowId", "DocumentId" });
            CreateIndex("dbo.WfTrans", new[] { "WFlowId", "DocumentId", "Sequence" }, unique: true, name: "IX_WfTrans");
        }
    }
}
