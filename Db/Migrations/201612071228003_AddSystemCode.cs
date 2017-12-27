namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSystemCode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemCodes",
                c => new
                    {
                        SysCodeId = c.Byte(nullable: false),
                        SysCodeName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.SysCodeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemCodes");
        }
    }
}
