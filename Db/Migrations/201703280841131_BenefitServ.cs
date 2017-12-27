namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BenefitServ : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BenefitServs", "BenefitId", "dbo.Benefits");
            DropIndex("dbo.BenefitServs", new[] { "BenefitId" });
            CreateTable(
                "dbo.BenefitServPlans",
                c => new
                    {
                        BenefitServ_Id = c.Int(nullable: false),
                        BenefitPlan_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BenefitServ_Id, t.BenefitPlan_Id })
                .ForeignKey("dbo.BenefitServs", t => t.BenefitServ_Id, cascadeDelete: true)
                .ForeignKey("dbo.BenefitPlans", t => t.BenefitPlan_Id, cascadeDelete: true)
                .Index(t => t.BenefitServ_Id)
                .Index(t => t.BenefitPlan_Id);
            
            DropColumn("dbo.BenefitServs", "BenefitId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BenefitServs", "BenefitId", c => c.Int(nullable: false));
            DropForeignKey("dbo.BenefitServBenefitPlans", "BenefitPlan_Id", "dbo.BenefitPlans");
            DropForeignKey("dbo.BenefitServBenefitPlans", "BenefitServ_Id", "dbo.BenefitServs");
            DropIndex("dbo.BenefitServBenefitPlans", new[] { "BenefitPlan_Id" });
            DropIndex("dbo.BenefitServBenefitPlans", new[] { "BenefitServ_Id" });
            DropTable("dbo.BenefitServBenefitPlans");
            CreateIndex("dbo.BenefitServs", "BenefitId");
            AddForeignKey("dbo.BenefitServs", "BenefitId", "dbo.Benefits", "Id", cascadeDelete: true);
        }
    }
}
