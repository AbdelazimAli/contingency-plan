namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameHRCalendar : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.HRCalendars", newName: "PeriodNames");
            CreateTable(
                "dbo.FiscalYears",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.PeriodNames", "SingleYear", c => c.Boolean(nullable: false));
            RenameColumn("dbo.PeriodNames", "Default","SingleYear");
            AddColumn("dbo.Periods", "YearId", c => c.Int());
            CreateIndex("dbo.Periods", "YearId");
            AddForeignKey("dbo.Periods", "YearId", "dbo.FiscalYears", "Id");
            //DropColumn("dbo.PeriodNames", "Default");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PeriodNames", "Default", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Periods", "YearId", "dbo.FiscalYears");
            DropIndex("dbo.Periods", new[] { "YearId" });
            DropColumn("dbo.Periods", "YearId");
            DropColumn("dbo.PeriodNames", "SingleYear");
            DropTable("dbo.FiscalYears");
            RenameTable(name: "dbo.PeriodNames", newName: "HRCalendars");
        }
    }
}
