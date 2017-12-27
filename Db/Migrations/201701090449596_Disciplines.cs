namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Disciplines : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Disciplines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 250),
                        Description = c.String(maxLength: 250),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        DisciplineClass = c.Byte(nullable: false),
                        PeriodId = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.DisplinPeriods", t => t.PeriodId)
                .Index(t => t.Code, unique: true, name: "IX_Discipline")
                .Index(t => t.CompanyId)
                .Index(t => t.PeriodId);
            
            CreateTable(
                "dbo.DisplinPeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        SysType = c.Byte(nullable: false),
                        MinPoints = c.Int(),
                        PointsAdd = c.Int(),
                        StartPeriod = c.Byte(),
                        SpecifDate = c.DateTime(),
                        SpecifMonth = c.Byte(),
                        SpecifYear = c.Short(),
                        Frequency = c.Byte(),
                        Times = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DisPeriodNo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeriodId = c.Int(nullable: false),
                        PeriodNo = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        PeriodSDate = c.DateTime(nullable: false),
                        PeriodEDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DisplinPeriods", t => t.PeriodId, cascadeDelete: true)
                .Index(t => new { t.PeriodId, t.PeriodNo }, unique: true, name: "IX_DisPeriodNo");
            
            CreateTable(
                "dbo.DisplinRepeats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplinId = c.Int(nullable: false),
                        RepNo = c.Byte(nullable: false),
                        DisplinType = c.Byte(nullable: false),
                        NofDays = c.Byte(),
                        MaxDeduction = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DenyPeriod = c.Byte(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disciplines", t => t.DisplinId, cascadeDelete: true)
                .Index(t => t.DisplinId);
            
            CreateTable(
                "dbo.EmpDisciplines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpId = c.Int(nullable: false),
                        DiscplinId = c.Int(nullable: false),
                        ViolDate = c.DateTime(nullable: false),
                        SuggDispline = c.Int(),
                        SuggPeriod = c.Byte(),
                        ActDispline = c.Int(nullable: false),
                        ActualPeriod = c.Byte(),
                        Description = c.String(maxLength: 500),
                        Witness = c.String(maxLength: 30),
                        Summary = c.String(maxLength: 500),
                        Defense = c.String(maxLength: 500),
                        DeductPoint = c.Short(),
                        EffectEDate = c.DateTime(),
                        DescionNo = c.String(),
                        DescionDate = c.DateTime(),
                        Manager = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DisplinRepeats", t => t.ActDispline, cascadeDelete: false)
                .ForeignKey("dbo.Disciplines", t => t.DiscplinId, cascadeDelete: false)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .Index(t => t.EmpId)
                .Index(t => t.DiscplinId)
                .Index(t => t.ActDispline);
            
            AlterColumn("dbo.Assignments", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.CareerPaths", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.CompanyStructures", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.People", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.Hosiptals", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.Kafeel", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.Qualifications", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.QualGroups", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.Jobs", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.JobClasses", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.PeopleGroups", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.Positions", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.CareerPathJobs", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.DocTypes", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.Schools", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.Holidays", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.JobPayrollGrades", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.LeavePeriods", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.LeaveTypes", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.LeaveRanges", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.LeaveRequests", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.LookUpCode", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.LookUpUserCodes", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.PeopleTrain", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.TrainCourses", "CreatedTime", c => c.DateTime());
            AlterColumn("dbo.TrainPaths", "CreatedTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmpDisciplines", "EmpId", "dbo.People");
            DropForeignKey("dbo.EmpDisciplines", "DiscplinId", "dbo.Disciplines");
            DropForeignKey("dbo.EmpDisciplines", "ActDispline", "dbo.DisplinRepeats");
            DropForeignKey("dbo.DisplinRepeats", "DisplinId", "dbo.Disciplines");
            DropForeignKey("dbo.DisPeriodNo", "PeriodId", "dbo.DisplinPeriods");
            DropForeignKey("dbo.Disciplines", "PeriodId", "dbo.DisplinPeriods");
            DropForeignKey("dbo.Disciplines", "CompanyId", "dbo.Companies");
            DropIndex("dbo.EmpDisciplines", new[] { "ActDispline" });
            DropIndex("dbo.EmpDisciplines", new[] { "DiscplinId" });
            DropIndex("dbo.EmpDisciplines", new[] { "EmpId" });
            DropIndex("dbo.DisplinRepeats", new[] { "DisplinId" });
            DropIndex("dbo.DisPeriodNo", "IX_DisPeriodNo");
            DropIndex("dbo.Disciplines", new[] { "PeriodId" });
            DropIndex("dbo.Disciplines", new[] { "CompanyId" });
            DropIndex("dbo.Disciplines", "IX_Discipline");
            AlterColumn("dbo.TrainPaths", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TrainCourses", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PeopleTrain", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LookUpUserCodes", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LookUpCode", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveRequests", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveRanges", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveTypes", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeavePeriods", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobPayrollGrades", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Holidays", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Schools", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DocTypes", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CareerPathJobs", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Positions", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PeopleGroups", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobClasses", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.QualGroups", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Qualifications", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Kafeel", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Hosiptals", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.People", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CompanyStructures", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CareerPaths", "CreatedTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Assignments", "CreatedTime", c => c.DateTime(nullable: false));
            DropTable("dbo.EmpDisciplines");
            DropTable("dbo.DisplinRepeats");
            DropTable("dbo.DisPeriodNo");
            DropTable("dbo.DisplinPeriods");
            DropTable("dbo.Disciplines");
        }
    }
}
