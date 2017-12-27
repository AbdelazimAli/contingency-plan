namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvestigations : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.BenefitServPlans", "BenefitPlan_Id", "dbo.BenefitPlans");
            //DropForeignKey("dbo.BenefitServPlans", "BenefitServ_Id", "dbo.BenefitServs");
            DropTable("dbo.BenefitServPlans");
            DropForeignKey("dbo.EmpBenefits", "BenefitId", "dbo.Benefits");
            DropIndex("dbo.EmpBenefits", "IX_Benefit");
            DropIndex("dbo.EmpBenefits", "IX_EmpBenefit");
            DropIndex("dbo.EmpBenefits", new[] { "BenPlanId" });
            DropIndex("dbo.BenefitServPlans", new[] { "BenefitPlan_Id" });
            DropIndex("dbo.BenefitServPlans", new[] { "BenefitServ_Id" });
            RenameColumn(table: "dbo.EmpBenefits", name: "BenPlanId", newName: "BenefitPlanId");
            RenameColumn(table: "dbo.MedicalRequests", name: "BendefId", newName: "BeneficiaryId");
            RenameIndex(table: "dbo.MedicalRequests", name: "IX_BendefId", newName: "IX_BeneficiaryId");
            CreateTable(
                "dbo.BenefitServPlans",
                c => new
                    {
                        BenefitPlanId = c.Int(nullable: false),
                        BenefitServId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BenefitPlanId, t.BenefitServId })
                .ForeignKey("dbo.BenefitPlans", t => t.BenefitPlanId, cascadeDelete: true)
                .ForeignKey("dbo.BenefitServs", t => t.BenefitServId, cascadeDelete: true)
                .Index(t => t.BenefitPlanId)
                .Index(t => t.BenefitServId);
            
            CreateTable(
                "dbo.Investigations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        InvestDate = c.DateTime(nullable: false, storeType: "date"),
                        Accident = c.String(maxLength: 1000),
                        Defense = c.String(maxLength: 1000),
                        ViolationId = c.Int(),
                        InvestResult = c.String(maxLength: 1000),
                        Notes = c.String(maxLength: 500),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Disciplines", t => t.ViolationId)
                .Index(t => t.CompanyId)
                .Index(t => t.ViolationId);
            
            CreateTable(
                "dbo.InvestigatEmps",
                c => new
                    {
                        InvestigatId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        EmpType = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => new { t.InvestigatId, t.EmpId })
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: true)
                .ForeignKey("dbo.Investigations", t => t.InvestigatId, cascadeDelete: true)
                .Index(t => t.InvestigatId)
                .Index(t => t.EmpId);
            
            AddColumn("dbo.EmpDisciplines", "InvestigatId", c => c.Int());
            AddColumn("dbo.MedicalRequests", "BenefitPlanId", c => c.Int(nullable: false));
            CreateIndex("dbo.EmpBenefits", new[] { "BenefitPlanId", "EmpId", "BeneficiaryId", "StartDate", "EndDate" }, name: "IX_Benefit");
            CreateIndex("dbo.EmpBenefits", new[] { "BenefitPlanId", "EmpId", "BeneficiaryId" }, unique: true, name: "IX_EmpBenefit");
            CreateIndex("dbo.EmpDisciplines", "InvestigatId");
            CreateIndex("dbo.MedicalRequests", "BenefitPlanId");
            AddForeignKey("dbo.EmpDisciplines", "InvestigatId", "dbo.Investigations", "Id");
            AddForeignKey("dbo.MedicalRequests", "BenefitPlanId", "dbo.BenefitPlans", "Id", cascadeDelete: false);
            DropColumn("dbo.EmpBenefits", "BenefitClass");
            DropColumn("dbo.EmpBenefits", "CoverAmount");
            //DropTable("dbo.BenefitServPlans");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BenefitServPlans",
                c => new
                    {
                        BenefitPlan_Id = c.Int(nullable: false),
                        BenefitServ_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BenefitPlan_Id, t.BenefitServ_Id });
            
            AddColumn("dbo.EmpBenefits", "CoverAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.EmpBenefits", "BenefitClass", c => c.Short(nullable: false));
            DropForeignKey("dbo.MedicalRequests", "BenefitPlanId", "dbo.BenefitPlans");
            DropForeignKey("dbo.InvestigatEmps", "InvestigatId", "dbo.Investigations");
            DropForeignKey("dbo.InvestigatEmps", "EmpId", "dbo.People");
            DropForeignKey("dbo.EmpDisciplines", "InvestigatId", "dbo.Investigations");
            DropForeignKey("dbo.Investigations", "ViolationId", "dbo.Disciplines");
            DropForeignKey("dbo.Investigations", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.BenefitServPlans", "BenefitServId", "dbo.BenefitServs");
            DropForeignKey("dbo.BenefitServPlans", "BenefitPlanId", "dbo.BenefitPlans");
            DropIndex("dbo.MedicalRequests", new[] { "BenefitPlanId" });
            DropIndex("dbo.InvestigatEmps", new[] { "EmpId" });
            DropIndex("dbo.InvestigatEmps", new[] { "InvestigatId" });
            DropIndex("dbo.Investigations", new[] { "ViolationId" });
            DropIndex("dbo.Investigations", new[] { "CompanyId" });
            DropIndex("dbo.EmpDisciplines", new[] { "InvestigatId" });
            DropIndex("dbo.EmpBenefits", "IX_EmpBenefit");
            DropIndex("dbo.EmpBenefits", "IX_Benefit");
            DropIndex("dbo.BenefitServPlans", new[] { "BenefitServId" });
            DropIndex("dbo.BenefitServPlans", new[] { "BenefitPlanId" });
            DropColumn("dbo.MedicalRequests", "BenefitPlanId");
            DropColumn("dbo.EmpDisciplines", "InvestigatId");
            DropTable("dbo.InvestigatEmps");
            DropTable("dbo.Investigations");
            DropTable("dbo.BenefitServPlans");
            RenameIndex(table: "dbo.MedicalRequests", name: "IX_BeneficiaryId", newName: "IX_BendefId");
            RenameColumn(table: "dbo.MedicalRequests", name: "BeneficiaryId", newName: "BendefId");
            RenameColumn(table: "dbo.EmpBenefits", name: "BenefitPlanId", newName: "BenPlanId");
            CreateIndex("dbo.BenefitServPlans", "BenefitServ_Id");
            CreateIndex("dbo.BenefitServPlans", "BenefitPlan_Id");
            CreateIndex("dbo.EmpBenefits", "BenPlanId");
            CreateIndex("dbo.EmpBenefits", new[] { "BenefitId", "EmpId", "BeneficiaryId" }, unique: true, name: "IX_EmpBenefit");
            CreateIndex("dbo.EmpBenefits", new[] { "BenefitClass", "EmpId", "BeneficiaryId", "StartDate", "EndDate" }, name: "IX_Benefit");
            AddForeignKey("dbo.EmpBenefits", "BenefitId", "dbo.Benefits", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BenefitServPlans", "BenefitServ_Id", "dbo.BenefitServs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BenefitServPlans", "BenefitPlan_Id", "dbo.BenefitPlans", "Id", cascadeDelete: true);
        }
    }
}
