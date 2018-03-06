namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterDocTypeAttr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocTypeAttrs", "IsRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocTypeAttrs", "IsRequired");
        }
    }
}
