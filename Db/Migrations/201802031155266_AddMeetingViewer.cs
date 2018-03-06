namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMeetingViewer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeetViewers",
                c => new
                    {
                        MeetingId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MeetingId, t.EmpId })
                .ForeignKey("dbo.Meetings", t => t.MeetingId, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.EmpId)
                .Index(t => t.MeetingId)
                .Index(t => t.EmpId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeetViewers", "EmpId", "dbo.People");
            DropForeignKey("dbo.MeetViewers", "MeetingId", "dbo.Meetings");
            DropIndex("dbo.MeetViewers", new[] { "EmpId" });
            DropIndex("dbo.MeetViewers", new[] { "MeetingId" });
            DropTable("dbo.MeetViewers");
        }
    }
}
