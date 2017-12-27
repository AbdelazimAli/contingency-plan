namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Iteration1_1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Qualifications", "IX_Qualification");
            DropIndex("dbo.BenefitRequests", new[] { "BenefitId" });
            DropIndex("dbo.BenefitRequests", new[] { "EmpId" });
            DropIndex("dbo.BenefitRequests", new[] { "BeneficiaryId" });
            DropIndex("dbo.BenefitRequests", new[] { "SubPeriodId" });
            RenameIndex(table: "dbo.BenefitRequests", name: "IX_MedicalReqStatus", newName: "IX_BenefitReqStatus");
            AddColumn("dbo.People", "WorkTel", c => c.String(maxLength: 20));
            AddColumn("dbo.Employements", "Profession", c => c.String(maxLength: 30));
            AddColumn("dbo.Employements", "DurInMonths", c => c.Byte(nullable: false));
            AlterColumn("dbo.Qualifications", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.EmpRelatives", "BirthDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.EmpRelatives", "ExpiryDate", c => c.DateTime(storeType: "date"));
            CreateIndex("dbo.Qualifications", "Code", name: "IX_Qualification");
            CreateIndex("dbo.BenefitRequests", new[] { "SubPeriodId", "EmpId", "BeneficiaryId", "BenefitId" }, name: "IX_BenefitRequest");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BenefitRequests", "IX_BenefitRequest");
            DropIndex("dbo.Qualifications", "IX_Qualification");
            AlterColumn("dbo.EmpRelatives", "ExpiryDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.EmpRelatives", "BirthDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Qualifications", "Code", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.Employements", "DurInMonths");
            DropColumn("dbo.Employements", "Profession");
            DropColumn("dbo.People", "WorkTel");
            RenameIndex(table: "dbo.BenefitRequests", name: "IX_BenefitReqStatus", newName: "IX_MedicalReqStatus");
            CreateIndex("dbo.BenefitRequests", "SubPeriodId");
            CreateIndex("dbo.BenefitRequests", "BeneficiaryId");
            CreateIndex("dbo.BenefitRequests", "EmpId");
            CreateIndex("dbo.BenefitRequests", "BenefitId");
            CreateIndex("dbo.Qualifications", "Code", unique: true, name: "IX_Qualification");
        }
    }
}
