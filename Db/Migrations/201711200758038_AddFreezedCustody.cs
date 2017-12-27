namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFreezedCustody : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Custody", "Freeze", c => c.Boolean(nullable: false));
            AddColumn("dbo.PagePrints", "ForceUpload", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PagePrints", "ForceUpload");
            DropColumn("dbo.Custody", "Freeze");
        }
    }
}
