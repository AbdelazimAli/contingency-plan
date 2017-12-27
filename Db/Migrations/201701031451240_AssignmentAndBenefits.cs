namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssignmentAndBenefits : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        AssignDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Code = c.String(nullable: false, maxLength: 20),
                        AssignStatus = c.Byte(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        IsDepManager = c.Boolean(nullable: false),
                        JobId = c.Int(nullable: false),
                        PositionId = c.Int(),
                        LocationId = c.Int(),
                        GroupId = c.Int(),
                        PayrollId = c.Int(),
                        SalaryBasis = c.Byte(),
                        PayGradeId = c.Int(),
                        CareerId = c.Int(),
                        ManagerId = c.Int(),
                        ProbationPrd = c.Byte(),
                        NoticePrd = c.Byte(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CareerPaths", t => t.CareerId)
                .ForeignKey("dbo.CompanyStructures", t => t.DepartmentId, cascadeDelete: false)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.Payrolls", t => t.PayrollId)
                .ForeignKey("dbo.PayrollGrades", t => t.PayGradeId)
                .ForeignKey("dbo.PeopleGroups", t => t.GroupId)
                .ForeignKey("dbo.Positions", t => t.PositionId)
                .Index(t => new { t.EmpId, t.Active }, name: "IX_Assignment")
                .Index(t => new { t.EmpId, t.AssignDate }, unique: true, name: "IX_EmpAssignment")
                .Index(t => t.DepartmentId)
                .Index(t => t.JobId);
            
            CreateTable(
                "dbo.BenefitPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BenefitId = c.Int(nullable: false),
                        PlanName = c.String(maxLength: 100),
                        EmpPercent = c.Single(),
                        EmpAmount = c.Decimal(precision: 18, scale: 2),
                        CompPercent = c.Single(),
                        CompAmount = c.Decimal(precision: 18, scale: 2),
                        CoverAmount = c.Decimal(precision: 18, scale: 2),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId, cascadeDelete: true)
                .Index(t => new { t.BenefitId, t.PlanName }, unique: true, name: "IX_BenefitPlan");
            
            CreateTable(
                "dbo.Benefits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 20),
                        Name = c.String(maxLength: 100),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(maxLength: 250),
                        MonthFees = c.Decimal(precision: 18, scale: 2),
                        Coverage = c.Byte(nullable: false),
                        MaxFamilyCnt = c.Byte(),
                        EmpAccural = c.Byte(nullable: false),
                        WaitMonth = c.Byte(),
                        PeopleGroups = c.String(maxLength: 50),
                        Payrolls = c.String(maxLength: 50),
                        Jobs = c.String(maxLength: 50),
                        Employments = c.String(maxLength: 50),
                        CompanyStuctures = c.String(maxLength: 50),
                        Positions = c.String(maxLength: 50),
                        PayrollGrades = c.String(maxLength: 50),
                        Locations = c.String(maxLength: 50),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.Code, unique: true, name: "IX_Benefit")
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.EmpBenefits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BenefitId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        BenPlanId = c.Int(nullable: false),
                        Coverage = c.Byte(nullable: false),
                        MaxFamilyCnt = c.Byte(),
                        EmpPercent = c.Single(),
                        EmpAmount = c.Decimal(precision: 18, scale: 2),
                        CompPercent = c.Single(),
                        CompAmount = c.Decimal(precision: 18, scale: 2),
                        CoverAmount = c.Decimal(precision: 18, scale: 2),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId, cascadeDelete: false)
                .ForeignKey("dbo.BenefitPlans", t => t.BenPlanId, cascadeDelete: false)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: true)
                .Index(t => new { t.BenefitId, t.EmpId }, unique: true, name: "IX_EmpBenefit")
                .Index(t => t.BenPlanId);
            
            CreateTable(
                "dbo.EmpRelatives",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpId = c.Int(nullable: false),
                        Name = c.String(maxLength: 150),
                        Relation = c.Byte(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        NationalId = c.String(maxLength: 20),
                        Telephone = c.String(maxLength: 20),
                        PassportNo = c.String(maxLength: 20),
                        ExpiryDate = c.DateTime(nullable: false),
                        GateIn = c.String(maxLength: 30),
                        Entry = c.String(maxLength: 20),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: true)
                .Index(t => new { t.EmpId, t.Name }, unique: true, name: "IX_EmpRelative");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmpRelatives", "EmpId", "dbo.People");
            DropForeignKey("dbo.EmpBenefits", "EmpId", "dbo.People");
            DropForeignKey("dbo.EmpBenefits", "BenPlanId", "dbo.BenefitPlans");
            DropForeignKey("dbo.EmpBenefits", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.BenefitPlans", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.Benefits", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Assignments", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.Assignments", "GroupId", "dbo.PeopleGroups");
            DropForeignKey("dbo.Assignments", "PayGradeId", "dbo.PayrollGrades");
            DropForeignKey("dbo.Assignments", "PayrollId", "dbo.Payrolls");
            DropForeignKey("dbo.Assignments", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Assignments", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.Assignments", "EmpId", "dbo.People");
            DropForeignKey("dbo.Assignments", "DepartmentId", "dbo.CompanyStructures");
            DropForeignKey("dbo.Assignments", "CareerId", "dbo.CareerPaths");
            DropIndex("dbo.EmpRelatives", "IX_EmpRelative");
            DropIndex("dbo.EmpBenefits", new[] { "BenPlanId" });
            DropIndex("dbo.EmpBenefits", "IX_EmpBenefit");
            DropIndex("dbo.Benefits", new[] { "CompanyId" });
            DropIndex("dbo.Benefits", "IX_Benefit");
            DropIndex("dbo.BenefitPlans", "IX_BenefitPlan");
            DropIndex("dbo.Assignments", new[] { "CareerId" });
            DropIndex("dbo.Assignments", new[] { "PayGradeId" });
            DropIndex("dbo.Assignments", new[] { "PayrollId" });
            DropIndex("dbo.Assignments", new[] { "GroupId" });
            DropIndex("dbo.Assignments", new[] { "LocationId" });
            DropIndex("dbo.Assignments", new[] { "PositionId" });
            DropIndex("dbo.Assignments", new[] { "JobId" });
            DropIndex("dbo.Assignments", new[] { "DepartmentId" });
            DropIndex("dbo.Assignments", "IX_EmpAssignment");
            DropIndex("dbo.Assignments", "IX_Assignment");
            DropTable("dbo.EmpRelatives");
            DropTable("dbo.EmpBenefits");
            DropTable("dbo.Benefits");
            DropTable("dbo.BenefitPlans");
            DropTable("dbo.Assignments");
        }
    }
}
