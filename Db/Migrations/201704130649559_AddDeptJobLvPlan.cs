namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeptJobLvPlan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeptJobLvPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeptPlanId = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                        MinAllowPercent = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeptLeavePlans", t => t.DeptPlanId, cascadeDelete: true)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .Index(t => t.DeptPlanId)
                .Index(t => t.JobId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeptJobLvPlans", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.DeptJobLvPlans", "DeptPlanId", "dbo.DeptLeavePlans");
            DropIndex("dbo.DeptJobLvPlans", new[] { "JobId" });
            DropIndex("dbo.DeptJobLvPlans", new[] { "DeptPlanId" });
            DropTable("dbo.DeptJobLvPlans");
        }
    }
}
