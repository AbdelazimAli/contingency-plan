namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonnelAndGetcolumnTitle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Personnels",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        LocalCurrCode = c.String(maxLength: 3, fixedLength: true, unicode: false),
                        MaxAge = c.Byte(),
                        MinAge = c.Byte(),
                        CodeReuse = c.Boolean(nullable: false),
                        GenEmpCode = c.Byte(nullable: false),
                        GenWorkCode = c.Byte(nullable: false),
                        GenAppCode = c.Byte(nullable: false),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        WorkHours = c.Short(),
                        Rate = c.Byte(),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Currencies", t => t.LocalCurrCode)
                .Index(t => t.CompanyId)
                .Index(t => t.LocalCurrCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Personnels", "LocalCurrCode", "dbo.Currencies");
            DropForeignKey("dbo.Personnels", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Personnels", new[] { "LocalCurrCode" });
            DropIndex("dbo.Personnels", new[] { "CompanyId" });
            DropTable("dbo.Personnels");
        }
    }
}
