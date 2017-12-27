namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterJobs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobClasses", "Job_Id", "dbo.Jobs");
            DropIndex("dbo.JobClasses", new[] { "Job_Id" });
            DropColumn("dbo.Jobs", "DefaultGradeId");
            RenameColumn(table: "dbo.Jobs", name: "PayrollGrade_Id", newName: "DefaultGradeId");
            RenameIndex(table: "dbo.Jobs", name: "IX_PayrollGrade_Id", newName: "IX_DefaultGradeId");
            CreateTable(
                "dbo.JobClassJobs",
                c => new
                    {
                        JobClass_Id = c.Int(nullable: false),
                        Job_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.JobClass_Id, t.Job_Id })
                .ForeignKey("dbo.JobClasses", t => t.JobClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.Jobs", t => t.Job_Id, cascadeDelete: true)
                .Index(t => t.JobClass_Id)
                .Index(t => t.Job_Id);
            
            DropColumn("dbo.JobClasses", "Job_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobClasses", "Job_Id", c => c.Int());
            DropForeignKey("dbo.JobClassJobs", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.JobClassJobs", "JobClass_Id", "dbo.JobClasses");
            DropIndex("dbo.JobClassJobs", new[] { "Job_Id" });
            DropIndex("dbo.JobClassJobs", new[] { "JobClass_Id" });
            DropTable("dbo.JobClassJobs");
            RenameIndex(table: "dbo.Jobs", name: "IX_DefaultGradeId", newName: "IX_PayrollGrade_Id");
            RenameColumn(table: "dbo.Jobs", name: "DefaultGradeId", newName: "PayrollGrade_Id");
            AddColumn("dbo.Jobs", "DefaultGradeId", c => c.Int());
            CreateIndex("dbo.JobClasses", "Job_Id");
            AddForeignKey("dbo.JobClasses", "Job_Id", "dbo.Jobs", "Id");
        }
    }
}
