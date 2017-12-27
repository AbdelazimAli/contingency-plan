namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTemplate : DbMigration
    {
        public override void Up()
        {
           // RenameTable(name: "dbo.BenefitServBenefitPlans", newName: "BenefitServPlans");
           // DropPrimaryKey("dbo.BenefitServPlans");
            AddColumn("dbo.Personnels", "ContractTempl", c => c.String(maxLength: 50));
           // AddPrimaryKey("dbo.BenefitServPlans", new[] { "BenefitPlan_Id", "BenefitServ_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.BenefitServPlans");
            DropColumn("dbo.Personnels", "ContractTempl");
            AddPrimaryKey("dbo.BenefitServPlans", new[] { "BenefitServ_Id", "BenefitPlan_Id" });
            RenameTable(name: "dbo.BenefitServPlans", newName: "BenefitServBenefitPlans");
        }
    }
}
