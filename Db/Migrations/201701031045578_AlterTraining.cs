namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTraining : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TrainCourses", "QualGroupId", "dbo.QualGroups");
            DropIndex("dbo.TrainCourses", new[] { "QualGroupId" });
            RenameIndex(table: "dbo.TrainCourses", name: "IX_LeaveType", newName: "IX_TrainCourse");
            AddColumn("dbo.PeopleTrain", "Internal", c => c.Boolean(nullable: false));
            AddColumn("dbo.TrainCourses", "CourseCat", c => c.Byte());
            DropColumn("dbo.QualGroups", "Flag");
            DropColumn("dbo.TrainCourses", "QualGroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TrainCourses", "QualGroupId", c => c.Int());
            AddColumn("dbo.QualGroups", "Flag", c => c.Boolean(nullable: false));
            DropColumn("dbo.TrainCourses", "CourseCat");
            DropColumn("dbo.PeopleTrain", "Internal");
            RenameIndex(table: "dbo.TrainCourses", name: "IX_TrainCourse", newName: "IX_LeaveType");
            CreateIndex("dbo.TrainCourses", "QualGroupId");
            AddForeignKey("dbo.TrainCourses", "QualGroupId", "dbo.QualGroups", "Id");
        }
    }
}
