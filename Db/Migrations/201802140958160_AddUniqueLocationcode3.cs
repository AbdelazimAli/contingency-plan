namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueLocationcode3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Locations", "IX_LocationCode");
            AlterColumn("dbo.Locations", "Code", c => c.Int());
            CreateIndex("dbo.Locations", new[] { "SiteType", "Code" }, unique: true, name: "IX_LocationCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Locations", "IX_LocationCode");
            AlterColumn("dbo.Locations", "Code", c => c.Int(nullable: false));
            CreateIndex("dbo.Locations", new[] { "SiteType", "Code" }, unique: true, name: "IX_LocationCode");
        }
    }
}
