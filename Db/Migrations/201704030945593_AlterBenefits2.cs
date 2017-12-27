namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterBenefits2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.EmpBenefits", "IX_EmpBenefit");
            DropIndex("dbo.EmpBenefits", new[] { "BeneficiaryId" });
            AddColumn("dbo.BenefitServs", "EmpPercent", c => c.Single());
            AddColumn("dbo.BenefitServs", "CompPercent", c => c.Single());
            CreateIndex("dbo.EmpBenefits", new[] { "BenefitId", "EmpId", "BeneficiaryId" }, unique: true, name: "IX_EmpBenefit");
        }
        
        public override void Down()
        {
            DropIndex("dbo.EmpBenefits", "IX_EmpBenefit");
            DropColumn("dbo.BenefitServs", "CompPercent");
            DropColumn("dbo.BenefitServs", "EmpPercent");
            CreateIndex("dbo.EmpBenefits", "BeneficiaryId");
            CreateIndex("dbo.EmpBenefits", new[] { "BenefitId", "EmpId" }, unique: true, name: "IX_EmpBenefit");
        }
    }
}
