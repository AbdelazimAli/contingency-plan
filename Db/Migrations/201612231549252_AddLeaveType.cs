namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLeaveType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeavePeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeaveTypeId = c.Int(nullable: false),
                        PeriodNo = c.Int(nullable: false),
                        PeriodMonth = c.Byte(nullable: false),
                        PeriodYear = c.Short(nullable: false),
                        Posted = c.Boolean(nullable: false),
                        PostDate = c.DateTime(),
                        CreatedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LeaveTypes", t => t.LeaveTypeId, cascadeDelete: true)
                .Index(t => t.LeaveTypeId)
                .Index(t => t.PeriodNo, unique: true, name: "IX_LeavePeriod");
            
            CreateTable(
                "dbo.LeaveTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        AbsenceType = c.Byte(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        MustAddCause = c.Boolean(nullable: false),
                        ExDayOff = c.Boolean(nullable: false),
                        ExHolidays = c.Boolean(nullable: false),
                        AllowFraction = c.Boolean(nullable: false),
                        VerifyFraction = c.Boolean(nullable: false),
                        WaitingMonth = c.Byte(),
                        MinDays = c.Byte(),
                        MaxDays = c.Byte(),
                        ExWorkService = c.Byte(),
                        EffectOnPayroll = c.Boolean(nullable: false),
                        PeopleGroups = c.String(maxLength: 50),
                        Payrolls = c.String(maxLength: 50),
                        Jobs = c.String(maxLength: 50),
                        Employments = c.String(maxLength: 50),
                        CompanyStuctures = c.String(maxLength: 50),
                        Positions = c.String(maxLength: 50),
                        PayrollGrades = c.String(maxLength: 50),
                        Locations = c.String(maxLength: 50),
                        Gender = c.Byte(),
                        Religion = c.Byte(),
                        MaritalStat = c.Byte(),
                        Nationality = c.Byte(),
                        MilitaryStat = c.Byte(),
                        YearStartDt = c.Byte(),
                        StartDay = c.Byte(),
                        StartMonth = c.Byte(),
                        AccrualBal = c.Byte(),
                        AccBalDays = c.Byte(),
                        NofDays = c.Byte(),
                        Balanace50 = c.Boolean(nullable: false),
                        Age50 = c.Byte(),
                        NofDays50 = c.Byte(),
                        FractionsOpt = c.Byte(),
                        PostOpt = c.Byte(),
                        MaxNofDays = c.Byte(),
                        DiffDaysOpt = c.Byte(),
                        PayBefore = c.Boolean(nullable: false),
                        PayrollId = c.Int(),
                        Batch = c.Byte(),
                        FirstMonth = c.Byte(),
                        FirstYear = c.Short(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payrolls", t => t.PayrollId)
                .Index(t => t.Name, unique: true, name: "IX_LeaveType");
            
            AddColumn("dbo.DocTypes", "DocumenType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeavePeriods", "LeaveTypeId", "dbo.LeaveTypes");
            DropForeignKey("dbo.LeaveTypes", "PayrollId", "dbo.Payrolls");
            DropIndex("dbo.LeaveTypes", new[] { "PayrollId" });
            DropIndex("dbo.LeaveTypes", "IX_LeaveType");
            DropIndex("dbo.LeavePeriods", "IX_LeavePeriod");
            DropIndex("dbo.LeavePeriods", new[] { "LeaveTypeId" });
            DropColumn("dbo.DocTypes", "DocumenType");
            DropTable("dbo.LeaveTypes");
            DropTable("dbo.LeavePeriods");
        }
    }
}
