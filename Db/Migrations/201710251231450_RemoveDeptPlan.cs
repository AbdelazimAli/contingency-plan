namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDeptPlan : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeptLeavePlans", "DeptId", "dbo.CompanyStructures");
            DropForeignKey("dbo.DeptJobLvPlans", "DeptPlanId", "dbo.DeptLeavePlans");
            DropForeignKey("dbo.DeptJobLvPlans", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.AssignOrders", "LeaveTypeId", "dbo.LeaveTypes");
            DropIndex("dbo.AssignOrders", new[] { "LeaveTypeId" });
            DropIndex("dbo.DeptJobLvPlans", new[] { "DeptPlanId" });
            DropIndex("dbo.DeptJobLvPlans", new[] { "JobId" });
            DropIndex("dbo.DeptLeavePlans", "IX_DeptLeavePlan");
            CreateTable(
                "dbo.DeptJobLeavePlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        DeptId = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                        LeaveYear = c.Short(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        MinAllowPercent = c.Single(nullable: false),
                        Stars = c.Byte(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyStructures", t => t.DeptId, cascadeDelete: false)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: false)
                .Index(t => new { t.DeptId, t.JobId, t.FromDate, t.ToDate }, unique: true, name: "IX_DeptLeavePlan");
            
            AddColumn("dbo.AssignOrders", "CompanyId", c => c.Int(nullable: false));
            AlterColumn("dbo.AssignOrders", "LeaveTypeId", c => c.Int());
            AlterColumn("dbo.AssignOrders", "ExpiryDate", c => c.DateTime(storeType: "date"));
            CreateIndex("dbo.AssignOrders", "LeaveTypeId");
            AddForeignKey("dbo.AssignOrders", "LeaveTypeId", "dbo.LeaveTypes", "Id");
            DropTable("dbo.DeptJobLvPlans");
            DropTable("dbo.DeptLeavePlans");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DeptLeavePlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        DeptId = c.Int(),
                        LeaveYear = c.Short(nullable: false),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        MinAllowPercent = c.Single(),
                        Stars = c.Byte(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeptJobLvPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeptPlanId = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                        MinAllowPercent = c.Single(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.AssignOrders", "LeaveTypeId", "dbo.LeaveTypes");
            DropForeignKey("dbo.DeptJobLeavePlans", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.DeptJobLeavePlans", "DeptId", "dbo.CompanyStructures");
            DropIndex("dbo.DeptJobLeavePlans", "IX_DeptLeavePlan");
            DropIndex("dbo.AssignOrders", new[] { "LeaveTypeId" });
            AlterColumn("dbo.AssignOrders", "ExpiryDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.AssignOrders", "LeaveTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.AssignOrders", "CompanyId");
            DropTable("dbo.DeptJobLeavePlans");
            CreateIndex("dbo.DeptLeavePlans", new[] { "DeptId", "FromDate", "ToDate" }, name: "IX_DeptLeavePlan");
            CreateIndex("dbo.DeptJobLvPlans", "JobId");
            CreateIndex("dbo.DeptJobLvPlans", "DeptPlanId");
            CreateIndex("dbo.AssignOrders", "LeaveTypeId");
            AddForeignKey("dbo.AssignOrders", "LeaveTypeId", "dbo.LeaveTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DeptJobLvPlans", "JobId", "dbo.Jobs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DeptJobLvPlans", "DeptPlanId", "dbo.DeptLeavePlans", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DeptLeavePlans", "DeptId", "dbo.CompanyStructures", "Id");
        }
    }
}
