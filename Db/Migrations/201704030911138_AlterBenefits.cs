namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterBenefits : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Benefits", "BenefitClass", c => c.Short(nullable: false));
            AddColumn("dbo.Benefits", "CalenderId", c => c.Int());
            AddColumn("dbo.EmpBenefits", "BeneficiaryId", c => c.Int());
            AddColumn("dbo.EmpBenefits", "BenefitClass", c => c.Short(nullable: false));
            CreateIndex("dbo.Benefits", "CalenderId");
            CreateIndex("dbo.EmpBenefits", "BeneficiaryId");
            AddForeignKey("dbo.Benefits", "CalenderId", "dbo.PeriodNames", "Id");
            AddForeignKey("dbo.EmpBenefits", "BeneficiaryId", "dbo.EmpRelatives", "Id");
            DropColumn("dbo.EmpBenefits", "Coverage");
            DropColumn("dbo.EmpBenefits", "MaxFamilyCnt");
            DropColumn("dbo.EmpBenefits", "EmpPercent");
            DropColumn("dbo.EmpBenefits", "EmpAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmpBenefits", "EmpAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.EmpBenefits", "EmpPercent", c => c.Single());
            AddColumn("dbo.EmpBenefits", "MaxFamilyCnt", c => c.Byte());
            AddColumn("dbo.EmpBenefits", "Coverage", c => c.Byte(nullable: false));
            DropForeignKey("dbo.EmpBenefits", "BeneficiaryId", "dbo.EmpRelatives");
            DropForeignKey("dbo.Benefits", "CalenderId", "dbo.PeriodNames");
            DropIndex("dbo.EmpBenefits", new[] { "BeneficiaryId" });
            DropIndex("dbo.Benefits", new[] { "CalenderId" });
            DropColumn("dbo.EmpBenefits", "BenefitClass");
            DropColumn("dbo.EmpBenefits", "BeneficiaryId");
            DropColumn("dbo.Benefits", "CalenderId");
            DropColumn("dbo.Benefits", "BenefitClass");
        }
    }
}
