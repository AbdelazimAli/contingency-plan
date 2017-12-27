namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCalendar : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Hosiptals", newName: "Providers");
            DropIndex("dbo.Periods", "IX_Period");
            RenameColumn(table: "dbo.People", name: "HosiptalId", newName: "ProviderId");
            RenameIndex(table: "dbo.People", name: "IX_HosiptalId", newName: "IX_ProviderId");
            CreateTable(
                "dbo.PeriodNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        PeriodLength = c.Byte(nullable: false),
                        SubPeriodCount = c.Byte(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.Name, unique: true, name: "IX_CalendarName")
                .Index(t => t.CompanyId);
            
            AddColumn("dbo.Providers", "ProviderType", c => c.Short(nullable: false));
            AddColumn("dbo.Periods", "CalendarId", c => c.Int(nullable: false));
            AddColumn("dbo.Periods", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Periods", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Periods", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Periods", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.Periods", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.SubPeriods", "Status", c => c.Byte(nullable: false));
            AddColumn("dbo.SubPeriods", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.SubPeriods", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.SubPeriods", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.SubPeriods", "ModifiedTime", c => c.DateTime());
            CreateIndex("dbo.Periods", new[] { "CalendarId", "PeriodNo" }, unique: true, name: "IX_Period");
            CreateIndex("dbo.Periods", new[] { "CalendarId", "Name" }, unique: true, name: "IX_PeriodName");
            CreateIndex("dbo.SubPeriods", new[] { "PeriodId", "Name" }, unique: true, name: "IX_SubPeriodName");
            AddForeignKey("dbo.Periods", "CalendarId", "dbo.PeriodNames", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Periods", "CalendarId", "dbo.PeriodNames");
            DropForeignKey("dbo.PeriodNames", "CompanyId", "dbo.Companies");
            DropIndex("dbo.SubPeriods", "IX_SubPeriodName");
            DropIndex("dbo.PeriodNames", new[] { "CompanyId" });
            DropIndex("dbo.PeriodNames", "IX_CalendarName");
            DropIndex("dbo.Periods", "IX_PeriodName");
            DropIndex("dbo.Periods", "IX_Period");
            DropColumn("dbo.SubPeriods", "ModifiedTime");
            DropColumn("dbo.SubPeriods", "CreatedTime");
            DropColumn("dbo.SubPeriods", "ModifiedUser");
            DropColumn("dbo.SubPeriods", "CreatedUser");
            DropColumn("dbo.SubPeriods", "Status");
            DropColumn("dbo.Periods", "ModifiedTime");
            DropColumn("dbo.Periods", "CreatedTime");
            DropColumn("dbo.Periods", "ModifiedUser");
            DropColumn("dbo.Periods", "CreatedUser");
            DropColumn("dbo.Periods", "Status");
            DropColumn("dbo.Periods", "CalendarId");
            DropColumn("dbo.Providers", "ProviderType");
            DropTable("dbo.PeriodNames");
            RenameIndex(table: "dbo.People", name: "IX_ProviderId", newName: "IX_HosiptalId");
            RenameColumn(table: "dbo.People", name: "ProviderId", newName: "HosiptalId");
            CreateIndex("dbo.Periods", "PeriodNo", unique: true, name: "IX_Period");
            RenameTable(name: "dbo.Providers", newName: "Hosiptals");
        }
    }
}
