namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCustody : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "SysAssignStatus", c => c.Short(nullable: false));
            Sql("Update Assignments Set SysAssignStatus = C.SysCodeId from LookUpUserCodes C where C.CodeName = 'Assignment' and C.CodeId = Assignments.AssignStatus");
            AddColumn("dbo.Custody", "CurrencyRate", c => c.Single(nullable: false));
            AddColumn("dbo.Employements", "AutoRenew", c => c.Boolean(nullable: false));
            AddColumn("dbo.Employements", "RemindarDays", c => c.Int());
            AlterColumn("dbo.BudgetItems", "Code", c => c.Int());
            CreateIndex("dbo.AudiTrails", "SourceId", name: "IX_AudiTrailSourceId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AudiTrails", "IX_AudiTrailSourceId");
            AlterColumn("dbo.BudgetItems", "Code", c => c.Int(nullable: false));
            DropColumn("dbo.Employements", "RemindarDays");
            DropColumn("dbo.Employements", "AutoRenew");
            DropColumn("dbo.Custody", "CurrencyRate");
            DropColumn("dbo.Assignments", "SysAssignStatus");
        }
    }
}
