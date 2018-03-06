namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPersonAddress : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.People", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.People", "HoAddressId", "dbo.Addresses");
            DropIndex("dbo.People", new[] { "AddressId" });
            DropIndex("dbo.People", new[] { "HoAddressId" });
            AddColumn("dbo.People", "Address1", c => c.String(maxLength: 500));
            AddColumn("dbo.People", "CountryId", c => c.Int());
            AddColumn("dbo.People", "CityId", c => c.Int());
            AddColumn("dbo.People", "DistrictId", c => c.Int());
            AddColumn("dbo.People", "Latitude", c => c.Double());
            AddColumn("dbo.People", "Longitude", c => c.Double());
            AddColumn("dbo.People", "HoAddress", c => c.String(maxLength: 500));
            CreateIndex("dbo.People", "CountryId");
            CreateIndex("dbo.People", "CityId");
            CreateIndex("dbo.People", "DistrictId");
            AddForeignKey("dbo.People", "CityId", "dbo.Cities", "Id");
            AddForeignKey("dbo.People", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.People", "DistrictId", "dbo.Districts", "Id");
            DropColumn("dbo.People", "AddressId");
            DropColumn("dbo.People", "HoAddressId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "HoAddressId", c => c.Int());
            AddColumn("dbo.People", "AddressId", c => c.Int());
            DropForeignKey("dbo.People", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.People", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.People", "CityId", "dbo.Cities");
            DropIndex("dbo.People", new[] { "DistrictId" });
            DropIndex("dbo.People", new[] { "CityId" });
            DropIndex("dbo.People", new[] { "CountryId" });
            DropColumn("dbo.People", "HoAddress");
            DropColumn("dbo.People", "Longitude");
            DropColumn("dbo.People", "Latitude");
            DropColumn("dbo.People", "DistrictId");
            DropColumn("dbo.People", "CityId");
            DropColumn("dbo.People", "CountryId");
            DropColumn("dbo.People", "Address1");
            CreateIndex("dbo.People", "HoAddressId");
            CreateIndex("dbo.People", "AddressId");
            AddForeignKey("dbo.People", "HoAddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.People", "AddressId", "dbo.Addresses", "Id");
        }
    }
}
