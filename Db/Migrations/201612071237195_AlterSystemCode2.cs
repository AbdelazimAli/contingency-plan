namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSystemCode2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" });
            AddForeignKey("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" }, "dbo.SystemCodes", new[] { "CodeName", "SysCodeId" }, cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" }, "dbo.SystemCodes");
            DropIndex("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" });
        }
    }
}
