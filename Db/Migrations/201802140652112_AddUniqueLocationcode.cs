namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueLocationcode : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Locations", "Code", unique: true, name: "IX_LocationCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Locations", "IX_LocationCode");
        }
    }
}
