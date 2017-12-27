namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyStructure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyStructures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sort = c.String(nullable: false, maxLength: 150),
                        Order = c.Int(nullable: false),
                        Code = c.String(maxLength: 20),
                        Name = c.String(maxLength: 100),
                        ParentId = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                        PlannedCount = c.Int(),
                        CustInt1 = c.Int(),
                        CustInt2 = c.Int(),
                        CustDecimal3 = c.Decimal(precision: 18, scale: 2),
                        CustDecimal4 = c.Decimal(precision: 18, scale: 2),
                        CustDate1 = c.DateTime(),
                        CustDate2 = c.DateTime(),
                        CustDate3 = c.DateTime(),
                        CustDate4 = c.DateTime(),
                        CustText1 = c.String(maxLength: 250),
                        CustText2 = c.String(maxLength: 250),
                        CustText3 = c.String(maxLength: 250),
                        CustText4 = c.String(maxLength: 250),
                        CustText5 = c.String(maxLength: 250),
                        CustText6 = c.String(maxLength: 250),
                        CustText7 = c.String(maxLength: 250),
                        CustText8 = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyStructures", t => t.ParentId)
                .Index(t => t.ParentId);
            
            AddColumn("dbo.Personnels", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Personnels", "ModifiedTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyStructures", "ParentId", "dbo.CompanyStructures");
            DropIndex("dbo.CompanyStructures", new[] { "ParentId" });
            DropColumn("dbo.Personnels", "ModifiedTime");
            DropColumn("dbo.Personnels", "ModifiedUser");
            DropTable("dbo.CompanyStructures");
        }
    }
}
