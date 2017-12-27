namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLeaveActions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LeaveTypes", "PayrollId", "dbo.Payrolls");
            DropIndex("dbo.LeaveTypes", new[] { "PayrollId" });
            CreateTable(
                "dbo.LeaveAdjusts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeId = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        TransType = c.Byte(nullable: false),
                        NofDays = c.Short(nullable: false),
                        Posted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.LeavePeriods", t => t.PeriodId, cascadeDelete: false)
                .ForeignKey("dbo.LeaveTypes", t => t.TypeId, cascadeDelete: false)
                .Index(t => new { t.TypeId, t.PeriodId, t.EmpId }, name: "IX_LeaveAction");
            
            CreateTable(
                "dbo.LeaveTrans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeId = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        TransType = c.Byte(nullable: false),
                        TransTime = c.DateTime(nullable: false),
                        TransFlag = c.Short(nullable: false),
                        TransQty = c.Short(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.LeavePeriods", t => t.PeriodId, cascadeDelete: false)
                .ForeignKey("dbo.LeaveTypes", t => t.TypeId, cascadeDelete: false)
                .Index(t => new { t.TypeId, t.PeriodId, t.EmpId }, name: "IX_LeaveTrans");
            
            AddColumn("dbo.LeaveTypes", "HasAccrualPlan", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveRequests", "ActualNofDays", c => c.Short());
            DropColumn("dbo.LeaveTypes", "ExWorkService");
            DropColumn("dbo.LeaveTypes", "EffectOnPayroll");
            DropColumn("dbo.LeaveTypes", "PayBefore");
            DropColumn("dbo.LeaveTypes", "PayrollId");
            DropColumn("dbo.LeaveTypes", "Batch");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveTypes", "Batch", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "PayrollId", c => c.Int());
            AddColumn("dbo.LeaveTypes", "PayBefore", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveTypes", "EffectOnPayroll", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveTypes", "ExWorkService", c => c.Boolean());
            DropForeignKey("dbo.LeaveTrans", "TypeId", "dbo.LeaveTypes");
            DropForeignKey("dbo.LeaveTrans", "PeriodId", "dbo.LeavePeriods");
            DropForeignKey("dbo.LeaveTrans", "EmpId", "dbo.People");
            DropForeignKey("dbo.LeaveAdjusts", "TypeId", "dbo.LeaveTypes");
            DropForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.LeavePeriods");
            DropForeignKey("dbo.LeaveAdjusts", "EmpId", "dbo.People");
            DropIndex("dbo.LeaveTrans", "IX_LeaveTrans");
            DropIndex("dbo.LeaveAdjusts", "IX_LeaveAction");
            DropColumn("dbo.LeaveRequests", "ActualNofDays");
            DropColumn("dbo.LeaveTypes", "HasAccrualPlan");
            DropTable("dbo.LeaveTrans");
            DropTable("dbo.LeaveAdjusts");
            CreateIndex("dbo.LeaveTypes", "PayrollId");
            AddForeignKey("dbo.LeaveTypes", "PayrollId", "dbo.Payrolls", "Id");
        }
    }
}
