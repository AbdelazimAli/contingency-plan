namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterAddress : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Addresses", "IX_Address");
            AlterColumn("dbo.Addresses", "Address1", c => c.String(nullable: false, maxLength: 500));
            CreateIndex("dbo.Addresses", new[] { "Address1", "Address2", "Address3" }, unique: true, name: "IX_Address");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Addresses", "IX_Address");
            AlterColumn("dbo.Addresses", "Address1", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Addresses", new[] { "Address1", "Address2", "Address3" }, unique: true, name: "IX_Address");
        }
    }
}
