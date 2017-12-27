namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMeeting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeetAttendees",
                c => new
                    {
                        MeetingId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MeetingId, t.EmpId })
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.Meetings", t => t.MeetingId, cascadeDelete: true)
                .Index(t => t.MeetingId)
                .Index(t => t.EmpId);
            
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectDesc = c.String(maxLength: 500),
                        MeetSubject = c.Short(nullable: false),
                        MeetDate = c.DateTime(nullable: false, storeType: "date"),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        Location = c.String(maxLength: 250),
                        EmpId = c.Int(nullable: false),
                        Status = c.Byte(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .Index(t => t.EmpId);
            
            CreateTable(
                "dbo.MeetScheduals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MeetingId = c.Int(nullable: false),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        Description = c.String(maxLength: 500),
                        EmpId = c.Int(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meetings", t => t.MeetingId, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .Index(t => t.MeetingId)
                .Index(t => t.EmpId);
            
            AddColumn("dbo.NotifyLetters", "read", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyLetters", "EventDate", c => c.DateTime(nullable: false));
            Sql("update dbo.NotifyLetters set EventDate = GetDate()");
            AddColumn("dbo.NotifyLetters", "Description", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeetScheduals", "EmpId", "dbo.People");
            DropForeignKey("dbo.MeetScheduals", "MeetingId", "dbo.Meetings");
            DropForeignKey("dbo.MeetAttendees", "MeetingId", "dbo.Meetings");
            DropForeignKey("dbo.Meetings", "EmpId", "dbo.People");
            DropForeignKey("dbo.MeetAttendees", "EmpId", "dbo.People");
            DropIndex("dbo.MeetScheduals", new[] { "EmpId" });
            DropIndex("dbo.MeetScheduals", new[] { "MeetingId" });
            DropIndex("dbo.Meetings", new[] { "EmpId" });
            DropIndex("dbo.MeetAttendees", new[] { "EmpId" });
            DropIndex("dbo.MeetAttendees", new[] { "MeetingId" });
            DropColumn("dbo.NotifyLetters", "Description");
            DropColumn("dbo.NotifyLetters", "EventDate");
            DropColumn("dbo.NotifyLetters", "read");
            DropTable("dbo.MeetScheduals");
            DropTable("dbo.Meetings");
            DropTable("dbo.MeetAttendees");
        }
    }
}
