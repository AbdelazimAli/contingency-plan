namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotificationDb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PeopleTrain", "Person_Id", "dbo.People");
            DropIndex("dbo.PeopleTrain", "IX_PeopleEvent");
            DropIndex("dbo.PeopleTrain", "IX_PeopleTrain");
            DropIndex("dbo.PeopleTrain", new[] { "Person_Id" });
            DropColumn("dbo.PeopleTrain", "EmpId");
            RenameColumn(table: "dbo.PeopleTrain", name: "Person_Id", newName: "EmpId");
            RenameColumn("dbo.People", "Religon", "Religion");
            RenameColumn("dbo.People", "RecmndReson", "RecommenReson");
            AddColumn("dbo.NotifyConditions", "EncodedMsg", c => c.String(maxLength: 500));
            AddColumn("dbo.NotifyConditions", "Fields", c => c.String(maxLength: 100));
            AlterColumn("dbo.PeopleTrain", "EmpId", c => c.Int(nullable: false));
            CreateIndex("dbo.PeopleTrain", new[] { "EventId", "EmpId" }, name: "IX_PeopleEvent");
            CreateIndex("dbo.PeopleTrain", new[] { "EmpId", "CourseId" }, unique: true, name: "IX_PeopleTrain");
            AddForeignKey("dbo.PeopleTrain", "EmpId", "dbo.People", "Id", cascadeDelete: true);
            DropColumn("dbo.Notifications", "CheckSql");
            DropColumn("dbo.Notifications", "Immediately");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notifications", "Immediately", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notifications", "CheckSql", c => c.String(maxLength: 100));
            AddColumn("dbo.People", "RecmndReson", c => c.Short());
            AddColumn("dbo.People", "Religon", c => c.Short());
            DropForeignKey("dbo.PeopleTrain", "EmpId", "dbo.People");
            DropIndex("dbo.PeopleTrain", "IX_PeopleTrain");
            DropIndex("dbo.PeopleTrain", "IX_PeopleEvent");
            AlterColumn("dbo.PeopleTrain", "EmpId", c => c.Int());
            DropColumn("dbo.NotifyConditions", "Fields");
            DropColumn("dbo.NotifyConditions", "EncodedMsg");
            DropColumn("dbo.People", "RecommenReson");
            DropColumn("dbo.People", "Religion");
            RenameColumn(table: "dbo.PeopleTrain", name: "EmpId", newName: "Person_Id");
            AddColumn("dbo.PeopleTrain", "EmpId", c => c.Int(nullable: false));
            CreateIndex("dbo.PeopleTrain", "Person_Id");
            CreateIndex("dbo.PeopleTrain", new[] { "EmpId", "CourseId" }, unique: true, name: "IX_PeopleTrain");
            CreateIndex("dbo.PeopleTrain", new[] { "EventId", "EmpId" }, name: "IX_PeopleEvent");
            AddForeignKey("dbo.PeopleTrain", "Person_Id", "dbo.People", "Id");
        }
    }
}
