namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "PeopleGroups", c => c.String(maxLength: 50));
            AddColumn("dbo.Assignments", "Payrolls", c => c.String(maxLength: 50));
            AddColumn("dbo.Assignments", "Jobs", c => c.String(maxLength: 50));
            AddColumn("dbo.Assignments", "Employments", c => c.String(maxLength: 50));
            AddColumn("dbo.Assignments", "CompanyStuctures", c => c.String(maxLength: 50));
            AddColumn("dbo.Assignments", "Positions", c => c.String(maxLength: 50));
            AddColumn("dbo.Assignments", "PayrollGrades", c => c.String(maxLength: 50));
            AddColumn("dbo.Assignments", "Locations", c => c.String(maxLength: 50));
            AddColumn("dbo.People", "ImageUrl", c => c.String(maxLength: 50));
            AddColumn("dbo.Menus", "Sequence", c => c.Short(nullable: false));
            AddColumn("dbo.LeaveTypes", "CasualLeaveId", c => c.Int());
            AlterColumn("dbo.People", "WorkEmail", c => c.String(maxLength: 30));
            AlterColumn("dbo.People", "OtherEmail", c => c.String(maxLength: 30));
            CreateIndex("dbo.LeaveTypes", "CasualLeaveId");
            AddForeignKey("dbo.LeaveTypes", "CasualLeaveId", "dbo.LeaveTypes", "Id");
            DropColumn("dbo.People", "HasCV");
            DropColumn("dbo.People", "UpdtCVDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "UpdtCVDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.People", "HasCV", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.LeaveTypes", "CasualLeaveId", "dbo.LeaveTypes");
            DropIndex("dbo.LeaveTypes", new[] { "CasualLeaveId" });
            AlterColumn("dbo.People", "OtherEmail", c => c.String(maxLength: 20));
            AlterColumn("dbo.People", "WorkEmail", c => c.String(maxLength: 20));
            DropColumn("dbo.LeaveTypes", "CasualLeaveId");
            DropColumn("dbo.Menus", "Sequence");
            DropColumn("dbo.People", "ImageUrl");
            DropColumn("dbo.Assignments", "Locations");
            DropColumn("dbo.Assignments", "PayrollGrades");
            DropColumn("dbo.Assignments", "Positions");
            DropColumn("dbo.Assignments", "CompanyStuctures");
            DropColumn("dbo.Assignments", "Employments");
            DropColumn("dbo.Assignments", "Jobs");
            DropColumn("dbo.Assignments", "Payrolls");
            DropColumn("dbo.Assignments", "PeopleGroups");
        }
    }
}
