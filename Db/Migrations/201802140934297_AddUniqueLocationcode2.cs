namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueLocationcode2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Locations", "IX_LocationCode");
            AddColumn("dbo.Locations", "SiteType", c => c.Short(nullable: false));
            CreateIndex("dbo.Locations", new[] { "SiteType", "Code" }, unique: true, name: "IX_LocationCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Locations", "IX_LocationCode");
            DropColumn("dbo.Locations", "SiteType");
            CreateIndex("dbo.Locations", "Code", unique: true, name: "IX_LocationCode");
        }
    }
}
