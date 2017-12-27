namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSuspeseReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employements", "SupenseReason", c => c.Byte());
            DropColumn("dbo.Employements", "StopReason");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employements", "StopReason", c => c.Byte(nullable: false));
            DropColumn("dbo.Employements", "SupenseReason");
        }
    }
}
