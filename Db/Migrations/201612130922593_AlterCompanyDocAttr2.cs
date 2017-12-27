namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCompanyDocAttr2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CompanyDocAttrs", "StreamId", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompanyDocAttrs", "StreamId", c => c.String());
        }
    }
}
