namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        Code = c.String(),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 500),
                        IsInternal = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        AddressId = c.Int(),
                        TimeZone = c.String(maxLength: 10),
                        DaylightSaving = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId)
                .Index(t => t.Name, unique: true, name: "IX_LocationName")
                .Index(t => t.AddressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Locations", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Locations", "AddressId", "dbo.Addresses");
            DropIndex("dbo.Locations", new[] { "AddressId" });
            DropIndex("dbo.Locations", "IX_LocationName");
            DropIndex("dbo.Locations", new[] { "CompanyId" });
            DropTable("dbo.Locations");
        }
    }
}
