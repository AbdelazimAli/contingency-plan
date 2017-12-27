namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Disciplines1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Covenants", newName: "Custody");
            RenameTable(name: "dbo.EmpCovenants", newName: "EmpCustodies");
            CreateTable(
                "dbo.Terminations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpId = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        PlanedDate = c.DateTime(),
                        ActualDate = c.DateTime(),
                        LastAccDate = c.DateTime(),
                        LastAdjustDate = c.DateTime(),
                        ServYear = c.Byte(nullable: false),
                        ServMonth = c.Byte(nullable: false),
                        TermCause = c.Byte(nullable: false),
                        AssignStatus = c.Byte(nullable: false),
                        PersonType = c.Byte(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .Index(t => t.EmpId);
            
            AddColumn("dbo.Custody", "CustodyCat", c => c.Byte());
            AddColumn("dbo.DisplinPeriods", "MaxPoints", c => c.Int());
            AddColumn("dbo.DisplinPeriods", "PeriodSDate", c => c.DateTime());
            AddColumn("dbo.DisPeriodNo", "Posted", c => c.Boolean(nullable: false));
            AddColumn("dbo.DisPeriodNo", "PostDate", c => c.DateTime());
            AddColumn("dbo.EmpCustodies", "CustodyId", c => c.Int(nullable: false));
            AddColumn("dbo.EmpCustodies", "CustodyStat", c => c.Byte());
            CreateIndex("dbo.EmpCustodies", "CustodyId");
            AddForeignKey("dbo.EmpCustodies", "CustodyId", "dbo.Custody", "Id", cascadeDelete: false);
            DropColumn("dbo.Custody", "CovenantCat");
            DropColumn("dbo.DisplinPeriods", "MinPoints");
            DropColumn("dbo.DisplinPeriods", "StartPeriod");
            DropColumn("dbo.DisplinPeriods", "SpecifDate");
            DropColumn("dbo.DisplinPeriods", "SpecifMonth");
            DropColumn("dbo.DisplinPeriods", "SpecifYear");
            DropColumn("dbo.DisplinPeriods", "Frequency");
            DropColumn("dbo.DisplinPeriods", "Times");
            DropColumn("dbo.EmpCustodies", "CovenantStat");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmpCustodies", "CovenantStat", c => c.Byte());
            AddColumn("dbo.DisplinPeriods", "Times", c => c.Int());
            AddColumn("dbo.DisplinPeriods", "Frequency", c => c.Byte());
            AddColumn("dbo.DisplinPeriods", "SpecifYear", c => c.Short());
            AddColumn("dbo.DisplinPeriods", "SpecifMonth", c => c.Byte());
            AddColumn("dbo.DisplinPeriods", "SpecifDate", c => c.DateTime());
            AddColumn("dbo.DisplinPeriods", "StartPeriod", c => c.Byte());
            AddColumn("dbo.DisplinPeriods", "MinPoints", c => c.Int());
            AddColumn("dbo.Custody", "CovenantCat", c => c.Byte());
            DropForeignKey("dbo.Terminations", "EmpId", "dbo.People");
            DropForeignKey("dbo.EmpCustodies", "CustodyId", "dbo.Custody");
            DropIndex("dbo.Terminations", new[] { "EmpId" });
            DropIndex("dbo.EmpCustodies", new[] { "CustodyId" });
            DropColumn("dbo.EmpCustodies", "CustodyStat");
            DropColumn("dbo.EmpCustodies", "CustodyId");
            DropColumn("dbo.DisPeriodNo", "PostDate");
            DropColumn("dbo.DisPeriodNo", "Posted");
            DropColumn("dbo.DisplinPeriods", "PeriodSDate");
            DropColumn("dbo.DisplinPeriods", "MaxPoints");
            DropColumn("dbo.Custody", "CustodyCat");
            DropTable("dbo.Terminations");
            RenameTable(name: "dbo.EmpCustodies", newName: "EmpCovenants");
            RenameTable(name: "dbo.Custody", newName: "Covenants");
        }
    }
}
