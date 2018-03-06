namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPersonAddress2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.People", "LocationId", "dbo.Locations");
            DropIndex("dbo.People", new[] { "LocationId" });
            DropColumn("dbo.People", "LocationId");
            DropColumn("dbo.People", "RoomNo");
            DropColumn("dbo.People", "ImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "ImageUrl", c => c.String(maxLength: 50));
            AddColumn("dbo.People", "RoomNo", c => c.String(maxLength: 10));
            AddColumn("dbo.People", "LocationId", c => c.Int());
            CreateIndex("dbo.People", "LocationId");
            AddForeignKey("dbo.People", "LocationId", "dbo.Locations", "Id");
        }
    }
}
