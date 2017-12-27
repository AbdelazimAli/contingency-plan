namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameBenefServ : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MedicalRequests", newName: "BenefitRequests");
            DropForeignKey("dbo.DeptLeavePlans", "DeptId", "dbo.CompanyStructures");
            DropIndex("dbo.DeptLeavePlans", new[] { "DeptId" });
            AddColumn("dbo.Benefits", "PlanLimit", c => c.Byte(nullable: false));
            AddColumn("dbo.DeptLeavePlans", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.DeptLeavePlans", "LeaveYear", c => c.Short(nullable: false));
            AddColumn("dbo.BenefitRequests", "BenefitClass", c => c.Short(nullable: false));
            AlterColumn("dbo.DeptJobLvPlans", "MinAllowPercent", c => c.Single());
            AlterColumn("dbo.DeptLeavePlans", "DeptId", c => c.Int());
            AlterColumn("dbo.DeptLeavePlans", "FromDate", c => c.DateTime());
            AlterColumn("dbo.DeptLeavePlans", "ToDate", c => c.DateTime());
            CreateIndex("dbo.DeptLeavePlans", new[] { "DeptId", "FromDate", "ToDate" }, name: "IX_DeptLeavePlan");
            AddForeignKey("dbo.DeptLeavePlans", "DeptId", "dbo.CompanyStructures", "Id");
            DropColumn("dbo.Personnels", "EmpStars");
            DropColumn("dbo.Personnels", "MinAllowPercent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Personnels", "MinAllowPercent", c => c.Single());
            AddColumn("dbo.Personnels", "EmpStars", c => c.Byte(nullable: false));
            DropForeignKey("dbo.DeptLeavePlans", "DeptId", "dbo.CompanyStructures");
            DropIndex("dbo.DeptLeavePlans", "IX_DeptLeavePlan");
            AlterColumn("dbo.DeptLeavePlans", "ToDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DeptLeavePlans", "FromDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DeptLeavePlans", "DeptId", c => c.Int(nullable: false));
            AlterColumn("dbo.DeptJobLvPlans", "MinAllowPercent", c => c.Single(nullable: false));
            DropColumn("dbo.BenefitRequests", "BenefitClass");
            DropColumn("dbo.DeptLeavePlans", "LeaveYear");
            DropColumn("dbo.DeptLeavePlans", "CompanyId");
            DropColumn("dbo.Benefits", "PlanLimit");
            CreateIndex("dbo.DeptLeavePlans", "DeptId");
            AddForeignKey("dbo.DeptLeavePlans", "DeptId", "dbo.CompanyStructures", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.BenefitRequests", newName: "MedicalRequests");
        }
    }
}
