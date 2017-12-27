namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Wftables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmpDisciplines", "ActDispline", "dbo.DisplinRepeats");
            DropIndex("dbo.EmpDisciplines", new[] { "ActDispline" });
            DropIndex("dbo.RequestWf", "IX_RequestWf");
            CreateTable(
                "dbo.DisplinRanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisPeriodId = c.Int(nullable: false),
                        FromPoint = c.Int(nullable: false),
                        ToPoint = c.Int(nullable: false),
                        Percentage = c.Single(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DisplinPeriods", t => t.DisPeriodId, cascadeDelete: true)
                .Index(t => new { t.DisPeriodId, t.FromPoint }, unique: true, name: "IX_DisplinRange");
            
            CreateTable(
                "dbo.EmpPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeriodId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        Balance = c.Int(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.DisPeriodNo", t => t.PeriodId, cascadeDelete: false)
                .Index(t => t.PeriodId)
                .Index(t => t.EmpId);
            
            CreateTable(
                "dbo.PositionHeirs",
                c => new
                    {
                        Hierarchy = c.Byte(nullable: false),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hierarchy, t.ParentId, t.ChildId });
            
            CreateTable(
                "dbo.WfRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WFlowId = c.Int(nullable: false),
                        Order = c.Byte(nullable: false),
                        RoleId = c.Guid(),
                        CodeId = c.Byte(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestWf", t => t.WFlowId, cascadeDelete: true)
                .Index(t => new { t.WFlowId, t.Order }, unique: true, name: "IX_WfRole");
            
            CreateTable(
                "dbo.WfTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WFlowId = c.Int(nullable: false),
                        DocumentId = c.Int(nullable: false),
                        Order = c.Byte(nullable: false),
                        RoleId = c.Guid(),
                        CodeId = c.Byte(),
                        PositionId = c.Int(),
                        EmpId = c.Int(),
                        ApprovalStatus = c.Byte(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.WFlowId, t.DocumentId, t.Order }, unique: true, name: "IX_WfTrans");
            
            AddColumn("dbo.Positions", "Headcount", c => c.Byte());
            AddColumn("dbo.Positions", "SysResponse", c => c.Byte());
            AddColumn("dbo.EmpDisciplines", "PeriodId", c => c.Int());
            AddColumn("dbo.LookUpCode", "Protected", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestWf", "HeirType", c => c.Byte(nullable: false));
            AddColumn("dbo.RequestWf", "Hierarchy", c => c.Byte(nullable: false));
            AddColumn("dbo.RequestWf", "NofApprovals", c => c.Byte());
            AddColumn("dbo.RequestWf", "TimeOutDays", c => c.Byte());
            AddColumn("dbo.RequestWf", "TimeOutAction", c => c.Byte());
            AlterColumn("dbo.DisplinPeriods", "MaxDaysDeduction", c => c.Short());
            AlterColumn("dbo.EmpDisciplines", "ActDispline", c => c.Int());
            CreateIndex("dbo.EmpDisciplines", "ActDispline");
            CreateIndex("dbo.RequestWf", new[] { "Source", "SourceId" }, unique: true, name: "IX_RequestWf");
            AddForeignKey("dbo.EmpDisciplines", "ActDispline", "dbo.DisplinRepeats", "Id");
            DropColumn("dbo.Positions", "PositionType");
            DropColumn("dbo.LookUpCode", "IsHidden");
            DropColumn("dbo.RequestWf", "Order");
            DropColumn("dbo.RequestWf", "RoleId");
            DropColumn("dbo.RequestWf", "CodeId");
            DropColumn("dbo.RequestWf", "WaitDays");
            DropColumn("dbo.RequestWf", "WaitAction");
            DropColumn("dbo.Terminations", "FlowOrder");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Terminations", "FlowOrder", c => c.Byte());
            AddColumn("dbo.RequestWf", "WaitAction", c => c.Byte());
            AddColumn("dbo.RequestWf", "WaitDays", c => c.Byte());
            AddColumn("dbo.RequestWf", "CodeId", c => c.Byte());
            AddColumn("dbo.RequestWf", "RoleId", c => c.Guid());
            AddColumn("dbo.RequestWf", "Order", c => c.Byte(nullable: false));
            AddColumn("dbo.LookUpCode", "IsHidden", c => c.Boolean(nullable: false));
            AddColumn("dbo.Positions", "PositionType", c => c.Byte(nullable: false));
            DropForeignKey("dbo.EmpDisciplines", "ActDispline", "dbo.DisplinRepeats");
            DropForeignKey("dbo.WfRoles", "WFlowId", "dbo.RequestWf");
            DropForeignKey("dbo.EmpPoints", "PeriodId", "dbo.DisPeriodNo");
            DropForeignKey("dbo.EmpPoints", "EmpId", "dbo.People");
            DropForeignKey("dbo.DisplinRanges", "DisPeriodId", "dbo.DisplinPeriods");
            DropIndex("dbo.WfTrans", "IX_WfTrans");
            DropIndex("dbo.WfRoles", "IX_WfRole");
            DropIndex("dbo.RequestWf", "IX_RequestWf");
            DropIndex("dbo.EmpPoints", new[] { "EmpId" });
            DropIndex("dbo.EmpPoints", new[] { "PeriodId" });
            DropIndex("dbo.EmpDisciplines", new[] { "ActDispline" });
            DropIndex("dbo.DisplinRanges", "IX_DisplinRange");
            AlterColumn("dbo.EmpDisciplines", "ActDispline", c => c.Int(nullable: false));
            AlterColumn("dbo.DisplinPeriods", "MaxDaysDeduction", c => c.Short(nullable: false));
            DropColumn("dbo.RequestWf", "TimeOutAction");
            DropColumn("dbo.RequestWf", "TimeOutDays");
            DropColumn("dbo.RequestWf", "NofApprovals");
            DropColumn("dbo.RequestWf", "Hierarchy");
            DropColumn("dbo.RequestWf", "HeirType");
            DropColumn("dbo.LookUpCode", "Protected");
            DropColumn("dbo.EmpDisciplines", "PeriodId");
            DropColumn("dbo.Positions", "SysResponse");
            DropColumn("dbo.Positions", "Headcount");
            DropTable("dbo.WfTrans");
            DropTable("dbo.WfRoles");
            DropTable("dbo.PositionHeirs");
            DropTable("dbo.EmpPoints");
            DropTable("dbo.DisplinRanges");
            CreateIndex("dbo.RequestWf", new[] { "Source", "SourceId", "Order" }, unique: true, name: "IX_RequestWf");
            CreateIndex("dbo.EmpDisciplines", "ActDispline");
            AddForeignKey("dbo.EmpDisciplines", "ActDispline", "dbo.DisplinRepeats", "Id", cascadeDelete: true);
        }
    }
}
