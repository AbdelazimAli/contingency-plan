namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterMeeting2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MeetScheduals", "EmpId", "dbo.People");
            DropIndex("dbo.MeetScheduals", new[] { "EmpId" });
            AddColumn("dbo.Meetings", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.Meetings", "IsUploaded", c => c.Boolean(nullable: false));
            AlterColumn("dbo.MeetScheduals", "EmpId", c => c.Int());
            CreateIndex("dbo.Meetings", "CompanyId");
            CreateIndex("dbo.MeetScheduals", "EmpId");
            AddForeignKey("dbo.Meetings", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MeetScheduals", "EmpId", "dbo.People", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeetScheduals", "EmpId", "dbo.People");
            DropForeignKey("dbo.Meetings", "CompanyId", "dbo.Companies");
            DropIndex("dbo.MeetScheduals", new[] { "EmpId" });
            DropIndex("dbo.Meetings", new[] { "CompanyId" });
            AlterColumn("dbo.MeetScheduals", "EmpId", c => c.Int(nullable: false));
            DropColumn("dbo.Meetings", "IsUploaded");
            DropColumn("dbo.Meetings", "CompanyId");
            CreateIndex("dbo.MeetScheduals", "EmpId");
            AddForeignKey("dbo.MeetScheduals", "EmpId", "dbo.People", "Id", cascadeDelete: true);
        }
    }
}
