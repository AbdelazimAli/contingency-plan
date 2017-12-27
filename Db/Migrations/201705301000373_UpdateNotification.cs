namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNotification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Consignees", "EmployeeId", "dbo.People");
            DropForeignKey("dbo.PeopleTrain", "PersonId", "dbo.People");
            DropIndex("dbo.Consignees", new[] { "EmployeeId" });
            DropIndex("dbo.PeopleTrain", "IX_PeopleEvent");
            DropIndex("dbo.PeopleTrain", "IX_PeopleTrain");
            RenameColumn(table: "dbo.Consignees", name: "EmployeeId", newName: "Employee_Id");
            RenameColumn(table: "dbo.NotifyConsigns", name: "EmployeeId", newName: "Employee_Id");
            RenameColumn(table: "dbo.PeopleTrain", name: "PersonId", newName: "Person_Id");
            RenameIndex(table: "dbo.NotifyConsigns", name: "IX_EmployeeId", newName: "IX_Employee_Id");
            AddColumn("dbo.Consignees", "EmpId", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "Subject", c => c.String(maxLength: 100));
            AddColumn("dbo.Notifications", "read", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyConsigns", "EmpId", c => c.Int());
            AddColumn("dbo.PeopleTrain", "EmpId", c => c.Int(nullable: false));
            AlterColumn("dbo.Consignees", "Employee_Id", c => c.Int());
            AlterColumn("dbo.PeopleTrain", "Person_Id", c => c.Int());
            CreateIndex("dbo.Consignees", "Employee_Id");
            CreateIndex("dbo.PeopleTrain", new[] { "EventId", "EmpId" }, name: "IX_PeopleEvent");
            CreateIndex("dbo.PeopleTrain", new[] { "EmpId", "CourseId" }, unique: true, name: "IX_PeopleTrain");
            CreateIndex("dbo.PeopleTrain", "Person_Id");
            AddForeignKey("dbo.Consignees", "Employee_Id", "dbo.People", "Id");
            AddForeignKey("dbo.PeopleTrain", "Person_Id", "dbo.People", "Id");
            DropColumn("dbo.Notifications", "SentTime");
            DropColumn("dbo.Notifications", "Sent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "Sent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notifications", "SentTime", c => c.DateTime());
            DropForeignKey("dbo.PeopleTrain", "Person_Id", "dbo.People");
            DropForeignKey("dbo.Consignees", "Employee_Id", "dbo.People");
            DropIndex("dbo.PeopleTrain", new[] { "Person_Id" });
            DropIndex("dbo.PeopleTrain", "IX_PeopleTrain");
            DropIndex("dbo.PeopleTrain", "IX_PeopleEvent");
            DropIndex("dbo.Consignees", new[] { "Employee_Id" });
            AlterColumn("dbo.PeopleTrain", "Person_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Consignees", "Employee_Id", c => c.Int(nullable: false));
            DropColumn("dbo.PeopleTrain", "EmpId");
            DropColumn("dbo.NotifyConsigns", "EmpId");
            DropColumn("dbo.Notifications", "read");
            DropColumn("dbo.Notifications", "Subject");
            DropColumn("dbo.Consignees", "EmpId");
            RenameIndex(table: "dbo.NotifyConsigns", name: "IX_Employee_Id", newName: "IX_EmployeeId");
            RenameColumn(table: "dbo.PeopleTrain", name: "Person_Id", newName: "PersonId");
            RenameColumn(table: "dbo.NotifyConsigns", name: "Employee_Id", newName: "EmployeeId");
            RenameColumn(table: "dbo.Consignees", name: "Employee_Id", newName: "EmployeeId");
            CreateIndex("dbo.PeopleTrain", new[] { "PersonId", "CourseId" }, unique: true, name: "IX_PeopleTrain");
            CreateIndex("dbo.PeopleTrain", new[] { "EventId", "PersonId" }, name: "IX_PeopleEvent");
            CreateIndex("dbo.Consignees", "EmployeeId");
            AddForeignKey("dbo.PeopleTrain", "PersonId", "dbo.People", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Consignees", "EmployeeId", "dbo.People", "Id", cascadeDelete: true);
        }
    }
}
