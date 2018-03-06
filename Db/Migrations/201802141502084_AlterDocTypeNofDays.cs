namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterDocTypeNofDays : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DocTypes", "NotifyDays", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DocTypes", "NotifyDays", c => c.Int(nullable: false));
        }
    }
}
