namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterBenefits3 : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.BenefitServs", name: "IX_ParentId", newName: "IX_BenefitServ");
            CreateIndex("dbo.EmpBenefits", new[] { "BenefitClass", "EmpId", "BeneficiaryId", "StartDate", "EndDate" }, name: "IX_Benefit");
        }
        
        public override void Down()
        {
            DropIndex("dbo.EmpBenefits", "IX_Benefit");
            RenameIndex(table: "dbo.BenefitServs", name: "IX_BenefitServ", newName: "IX_ParentId");
        }
    }
}
