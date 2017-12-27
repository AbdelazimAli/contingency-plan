namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterMeeting3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meetings", "LocationText", c => c.String(maxLength: 250));
            AddColumn("dbo.Meetings", "LocationType", c => c.Byte(nullable: false));
            AddColumn("dbo.Meetings", "LocationId", c => c.Int());
            AlterColumn("dbo.Meetings", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Meetings", "EndTime", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Meetings", "LocationId");
            AddForeignKey("dbo.Meetings", "LocationId", "dbo.Locations", "Id");
            DropColumn("dbo.Meetings", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Meetings", "Location", c => c.String(maxLength: 250));
            DropForeignKey("dbo.Meetings", "LocationId", "dbo.Locations");
            DropIndex("dbo.Meetings", new[] { "LocationId" });
            AlterColumn("dbo.Meetings", "EndTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Meetings", "StartTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Meetings", "LocationId");
            DropColumn("dbo.Meetings", "LocationType");
            DropColumn("dbo.Meetings", "LocationText");
        }
    }
}
