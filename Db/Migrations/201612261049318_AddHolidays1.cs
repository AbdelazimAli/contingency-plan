namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHolidays1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Holidays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        Standard = c.Boolean(nullable: false),
                        SDay = c.Byte(),
                        SMonth = c.Byte(),
                        HoliDate = c.DateTime(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Holidays", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Holidays", new[] { "CompanyId" });
            DropTable("dbo.Holidays");
        }
    }
}
