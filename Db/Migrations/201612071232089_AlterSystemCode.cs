namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSystemCode : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.SystemCodes");
            AddColumn("dbo.SystemCodes", "CodeName", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.SystemCodes", new[] { "CodeName", "SysCodeId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.SystemCodes");
            DropColumn("dbo.SystemCodes", "CodeName");
            AddPrimaryKey("dbo.SystemCodes", "SysCodeId");
        }
    }
}
