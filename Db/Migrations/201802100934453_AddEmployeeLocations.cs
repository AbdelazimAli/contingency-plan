namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeLocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationEmps",
                c => new
                    {
                        LocationId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LocationId, t.EmpId })
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.EmpId);
            
            AddColumn("dbo.Locations", "Address1", c => c.String(maxLength: 250));
            AddColumn("dbo.Locations", "Address2", c => c.String(maxLength: 100));
            AddColumn("dbo.Locations", "Address3", c => c.String(maxLength: 100));
            AddColumn("dbo.Locations", "CountryId", c => c.Int());
            AddColumn("dbo.Locations", "CityId", c => c.Int());
            AddColumn("dbo.Locations", "DistrictId", c => c.Int());
            AddColumn("dbo.Locations", "Telephone", c => c.String(maxLength: 50));
            AddColumn("dbo.Locations", "PostalCode", c => c.String(maxLength: 15));
            AddColumn("dbo.Locations", "Email", c => c.String(maxLength: 50));
            AddColumn("dbo.Locations", "ContactPerson", c => c.String(maxLength: 100));
            AlterColumn("dbo.Locations", "TimeZone", c => c.String(maxLength: 50));
            CreateIndex("dbo.Locations", "CountryId");
            CreateIndex("dbo.Locations", "CityId");
            CreateIndex("dbo.Locations", "DistrictId");
            AddForeignKey("dbo.Locations", "CityId", "dbo.Cities", "Id");
            AddForeignKey("dbo.Locations", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.Locations", "DistrictId", "dbo.Districts", "Id");
            DropColumn("dbo.Locations", "CostCenter");
            DropColumn("dbo.Locations", "Address");
            Sql("DROP TRIGGER [dbo].[AfterDELETELocations]");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "Address", c => c.String(maxLength: 250));
            AddColumn("dbo.Locations", "CostCenter", c => c.String());
            DropForeignKey("dbo.LocationEmps", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.LocationEmps", "EmpId", "dbo.People");
            DropForeignKey("dbo.Locations", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Locations", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Locations", "CityId", "dbo.Cities");
            DropIndex("dbo.Locations", new[] { "DistrictId" });
            DropIndex("dbo.Locations", new[] { "CityId" });
            DropIndex("dbo.Locations", new[] { "CountryId" });
            DropIndex("dbo.LocationEmps", new[] { "EmpId" });
            DropIndex("dbo.LocationEmps", new[] { "LocationId" });
            AlterColumn("dbo.Locations", "TimeZone", c => c.String(maxLength: 10));
            DropColumn("dbo.Locations", "ContactPerson");
            DropColumn("dbo.Locations", "Email");
            DropColumn("dbo.Locations", "PostalCode");
            DropColumn("dbo.Locations", "Telephone");
            DropColumn("dbo.Locations", "DistrictId");
            DropColumn("dbo.Locations", "CityId");
            DropColumn("dbo.Locations", "CountryId");
            DropColumn("dbo.Locations", "Address3");
            DropColumn("dbo.Locations", "Address2");
            DropColumn("dbo.Locations", "Address1");
            DropTable("dbo.LocationEmps");
        }
    }
}
