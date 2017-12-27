namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTraining : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PeopleTrain",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        CourseTitle = c.String(maxLength: 150),
                        CourseSDate = c.DateTime(),
                        CourseEDate = c.DateTime(),
                        ActualHours = c.Int(),
                        Cost = c.Decimal(precision: 18, scale: 2),
                        Adwarding = c.Byte(),
                        CantLeave = c.DateTime(),
                        Status = c.Byte(nullable: false),
                        Notes = c.String(maxLength: 250),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TrainCourses", t => t.CourseId, cascadeDelete: false)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: false)
                .Index(t => new { t.PersonId, t.CourseId }, unique: true, name: "IX_PeopleTrain");
            
            CreateTable(
                "dbo.TrainCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        QualGroupId = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Summary = c.String(maxLength: 250),
                        Whom = c.String(maxLength: 250),
                        Requirements = c.String(maxLength: 250),
                        PlannedHours = c.Short(),
                        PeopleGroups = c.String(maxLength: 50),
                        Payrolls = c.String(maxLength: 50),
                        Jobs = c.String(maxLength: 50),
                        Employments = c.String(maxLength: 50),
                        CompanyStuctures = c.String(maxLength: 50),
                        Positions = c.String(maxLength: 50),
                        PayrollGrades = c.String(maxLength: 50),
                        Locations = c.String(maxLength: 50),
                        PrevCourses = c.String(),
                        Qualification = c.Int(),
                        QualRank = c.Byte(),
                        YearServ = c.Byte(),
                        Age = c.Byte(),
                        Performance = c.Byte(),
                        Formula = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QualGroups", t => t.QualGroupId)
                .Index(t => t.Code, unique: true, name: "IX_LeaveType");
            
            CreateTable(
                "dbo.TrainPaths",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Summary = c.String(maxLength: 250),
                        PeopleGroups = c.String(maxLength: 50),
                        Payrolls = c.String(maxLength: 50),
                        Jobs = c.String(maxLength: 50),
                        Employments = c.String(maxLength: 50),
                        CompanyStuctures = c.String(maxLength: 50),
                        Positions = c.String(maxLength: 50),
                        PayrollGrades = c.String(maxLength: 50),
                        Locations = c.String(maxLength: 50),
                        Qualification = c.Int(),
                        QualRank = c.Byte(),
                        YearServ = c.Byte(),
                        Age = c.Byte(),
                        Performance = c.Byte(),
                        Formula = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_TrainPath");
            
            CreateTable(
                "dbo.TrainPathCourses",
                c => new
                    {
                        TrainCourse_Id = c.Int(nullable: false),
                        TrainPath_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TrainCourse_Id, t.TrainPath_Id })
                .ForeignKey("dbo.TrainCourses", t => t.TrainCourse_Id, cascadeDelete: true)
                .ForeignKey("dbo.TrainPaths", t => t.TrainPath_Id, cascadeDelete: true)
                .Index(t => t.TrainCourse_Id)
                .Index(t => t.TrainPath_Id);
            
            AddColumn("dbo.QualGroups", "Flag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PeopleTrain", "PersonId", "dbo.People");
            DropForeignKey("dbo.PeopleTrain", "CourseId", "dbo.TrainCourses");
            DropForeignKey("dbo.TrainCourses", "QualGroupId", "dbo.QualGroups");
            DropForeignKey("dbo.TrainPathCourses", "TrainPath_Id", "dbo.TrainPaths");
            DropForeignKey("dbo.TrainPathCourses", "TrainCourse_Id", "dbo.TrainCourses");
            DropIndex("dbo.TrainPathCourses", new[] { "TrainPath_Id" });
            DropIndex("dbo.TrainPathCourses", new[] { "TrainCourse_Id" });
            DropIndex("dbo.TrainPaths", "IX_TrainPath");
            DropIndex("dbo.TrainCourses", new[] { "QualGroupId" });
            DropIndex("dbo.TrainCourses", "IX_LeaveType");
            DropIndex("dbo.PeopleTrain", "IX_PeopleTrain");
            DropColumn("dbo.QualGroups", "Flag");
            DropTable("dbo.TrainPathCourses");
            DropTable("dbo.TrainPaths");
            DropTable("dbo.TrainCourses");
            DropTable("dbo.PeopleTrain");
        }
    }
}
