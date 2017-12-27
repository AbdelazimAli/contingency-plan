namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveActive : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Assignments", "IX_Assignment");
            DropIndex("dbo.Assignments", "IX_EmpAssignment");
            AlterColumn("dbo.Assignments", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveRanges", "ToPeriod", c => c.Short(nullable: false));
            CreateIndex("dbo.Assignments", new[] { "EmpId", "AssignDate", "EndDate" }, unique: true, name: "IX_EmpAssignment");
            DropColumn("dbo.Assignments", "Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assignments", "Active", c => c.Boolean(nullable: false));
            DropIndex("dbo.Assignments", "IX_EmpAssignment");
            AlterColumn("dbo.LeaveRanges", "ToPeriod", c => c.Short());
            AlterColumn("dbo.Assignments", "EndDate", c => c.DateTime());
            CreateIndex("dbo.Assignments", new[] { "EmpId", "AssignDate" }, unique: true, name: "IX_EmpAssignment");
            CreateIndex("dbo.Assignments", new[] { "EmpId", "Active" }, name: "IX_Assignment");
        }
    }
}
