namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPayroll : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payrolls", "PrdNameId", "dbo.PeriodNames");
            DropIndex("dbo.Payrolls", new[] { "PrdNameId" });
            DropColumn("dbo.Payrolls", "PrdNameId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payrolls", "PrdNameId", c => c.Int(nullable: false));
            CreateIndex("dbo.Payrolls", "PrdNameId");
            AddForeignKey("dbo.Payrolls", "PrdNameId", "dbo.PeriodNames", "Id", cascadeDelete: true);
        }
    }
}
