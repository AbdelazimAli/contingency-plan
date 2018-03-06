namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MajorChanges : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Locations", newName: "Sites");
            DropForeignKey("dbo.Addresses", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Addresses", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Addresses", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Companies", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.CompanyBranches", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.CompanyPartners", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Kafeel", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Providers", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.LocationEmps", "EmpId", "dbo.People");
            DropForeignKey("dbo.Locations", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.LocationEmps", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Assignments", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.CompanyBranches", "CompanyId", "dbo.Companies"); 
            DropForeignKey("dbo.Custody", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.EmpCustodies", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.LinkTables", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.ErrandRequests", "LocationId", "dbo.Locations");
            DropIndex("dbo.Companies", new[] { "AddressId" });
            DropIndex("dbo.Addresses", "IX_Address");
            DropIndex("dbo.Addresses", new[] { "CountryId" });
            DropIndex("dbo.Addresses", new[] { "CityId" });
            DropIndex("dbo.Addresses", new[] { "DistrictId" });
            DropIndex("dbo.LocationEmps", new[] { "LocationId" });
            DropIndex("dbo.LocationEmps", new[] { "EmpId" });
            DropIndex("dbo.Kafeel", new[] { "AddressId" });
            DropIndex("dbo.Providers", new[] { "AddressId" });
            DropIndex("dbo.Sites", new[] { "CompanyId" });
            DropIndex("dbo.Assignments", "IX_AssignmentBranch");
            DropIndex("dbo.Assignments", new[] { "LocationId" });
            DropIndex("dbo.CompanyBranches", new[] { "CompanyId" });
            DropIndex("dbo.CompanyBranches", new[] { "AddressId" });
            DropIndex("dbo.CompanyPartners", new[] { "AddressId" });
            DropIndex("dbo.Custody", new[] { "LocationId" });
            DropIndex("dbo.EmpCustodies", new[] { "LocationId" });
            DropIndex("dbo.ErrandRequests", new[] { "LocationId" });
            DropIndex("dbo.LinkTables", new[] { "LocationId" });
            RenameColumn(table: "dbo.ErrandRequests", name: "LocationId", newName: "SiteId");
            RenameColumn(table: "dbo.Meetings", name: "LocationId", newName: "SiteId");
            RenameIndex(table: "dbo.Sites", name: "IX_LocationCode", newName: "IX_SiteCode");
            RenameIndex(table: "dbo.Sites", name: "IX_LocationName", newName: "IX_SiteName");
            RenameIndex(table: "dbo.Meetings", name: "IX_LocationId", newName: "IX_SiteId");
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        Code = c.Int(),
                        Name = c.String(nullable: false, maxLength: 50),
                        Address1 = c.String(maxLength: 500),
                        CountryId = c.Int(),
                        CityId = c.Int(),
                        DistrictId = c.Int(),
                        Telephone = c.String(maxLength: 50),
                        Latitude = c.Double(),
                        Longitude = c.Double(),
                        TimeZone = c.String(maxLength: 50),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .Index(t => new { t.CompanyId, t.Code }, unique: true, name: "IX_BranchCode")
                .Index(t => new { t.CompanyId, t.Name }, unique: true, name: "IX_BranchName")
                .Index(t => t.CountryId)
                .Index(t => t.CityId)
                .Index(t => t.DistrictId);
            
            CreateTable(
                "dbo.SiteToEmps",
                c => new
                    {
                        SiteId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SiteId, t.EmpId })
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: true)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId)
                .Index(t => t.EmpId);
            

            AddColumn("dbo.Assignments", "Branches", c => c.String(maxLength: 50));
            AddColumn("dbo.LeaveTypes", "Branches", c => c.String(maxLength: 50));
            AddColumn("dbo.Benefits", "Branches", c => c.String(maxLength: 50));
            AddColumn("dbo.Custody", "BranchId", c => c.Int());
            AddColumn("dbo.EmpCustodies", "BranchId", c => c.Int());
            AddColumn("dbo.InfoTables", "IBranch", c => c.Boolean(nullable: false));
            AddColumn("dbo.LinkTables", "BranchId", c => c.Int());
            AddColumn("dbo.Meetings", "BranchId", c => c.Int());
            AddColumn("dbo.TrainCourses", "Branches", c => c.String(maxLength: 50));
            AddColumn("dbo.TrainPaths", "Branches", c => c.String(maxLength: 50));
            Sql("delete from dbo.Assignments");
            AlterColumn("dbo.Assignments", "BranchId", c => c.Int(nullable: false));
            AlterColumn("dbo.ErrandRequests", "SiteId", c => c.Int(nullable: false));
            CreateIndex("dbo.Assignments", "BranchId", name: "IX_AssignmentBranch");
            CreateIndex("dbo.Custody", "BranchId");
            CreateIndex("dbo.EmpCustodies", "BranchId");
            CreateIndex("dbo.ErrandRequests", "SiteId");
            CreateIndex("dbo.LinkTables", "BranchId");
            CreateIndex("dbo.Meetings", "BranchId");
            AddForeignKey("dbo.Assignments", "BranchId", "dbo.Branches", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Custody", "BranchId", "dbo.Branches", "Id");
            AddForeignKey("dbo.EmpCustodies", "BranchId", "dbo.Branches", "Id");
            AddForeignKey("dbo.LinkTables", "BranchId", "dbo.Branches", "Id");
            AddForeignKey("dbo.Meetings", "BranchId", "dbo.Branches", "Id");
            AddForeignKey("dbo.ErrandRequests", "SiteId", "dbo.Sites", "Id", cascadeDelete: false);
            DropColumn("dbo.Sites", "IsLocal");
            DropColumn("dbo.Sites", "CompanyId");
            DropColumn("dbo.Sites", "IsInternal");
            DropColumn("dbo.Sites", "DaylightSaving");
            DropColumn("dbo.Assignments", "SectorId");
            DropColumn("dbo.Assignments", "LocationId");
            DropColumn("dbo.Assignments", "Locations");
            DropColumn("dbo.LeaveTypes", "Locations");
            DropColumn("dbo.Benefits", "Locations");
            DropColumn("dbo.Custody", "LocationId");
            DropColumn("dbo.EmpCustodies", "LocationId");
            DropColumn("dbo.InfoTables", "ILocation");
            DropColumn("dbo.LinkTables", "LocationId");
            DropColumn("dbo.TrainCourses", "Locations");
            DropColumn("dbo.TrainPaths", "Locations");
            //DropTable("dbo.Addresses");
            DropTable("dbo.LocationEmps");
            DropTable("dbo.CompanyBranches");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CompanyBranches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        BranchNo = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        AddressId = c.Int(),
                        Telephone = c.String(maxLength: 100),
                        Email = c.String(maxLength: 50),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocationEmps",
                c => new
                    {
                        LocationId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LocationId, t.EmpId });
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address1 = c.String(nullable: false, maxLength: 500),
                        Address2 = c.String(maxLength: 100),
                        Address3 = c.String(maxLength: 100),
                        CountryId = c.Int(),
                        CityId = c.Int(),
                        DistrictId = c.Int(),
                        PostalCode = c.String(maxLength: 15),
                        Telephone = c.String(maxLength: 50),
                        Latitude = c.Double(),
                        Longitude = c.Double(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TrainPaths", "Locations", c => c.String(maxLength: 50));
            AddColumn("dbo.TrainCourses", "Locations", c => c.String(maxLength: 50));
            AddColumn("dbo.LinkTables", "LocationId", c => c.Int());
            AddColumn("dbo.InfoTables", "ILocation", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmpCustodies", "LocationId", c => c.Int());
            AddColumn("dbo.Custody", "LocationId", c => c.Int());
            AddColumn("dbo.Benefits", "Locations", c => c.String(maxLength: 50));
            AddColumn("dbo.LeaveTypes", "Locations", c => c.String(maxLength: 50));
            AddColumn("dbo.Assignments", "Locations", c => c.String(maxLength: 50));
            AddColumn("dbo.Assignments", "LocationId", c => c.Int());
            AddColumn("dbo.Assignments", "SectorId", c => c.Int());
            AddColumn("dbo.Sites", "DaylightSaving", c => c.Boolean());
            AddColumn("dbo.Sites", "IsInternal", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sites", "CompanyId", c => c.Int());
            AddColumn("dbo.Sites", "IsLocal", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.ErrandRequests", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.SiteToEmps", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.SiteToEmps", "EmpId", "dbo.People");
            DropForeignKey("dbo.Meetings", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.LinkTables", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.EmpCustodies", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Custody", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Assignments", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Branches", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Branches", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Branches", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Branches", "CityId", "dbo.Cities");
            DropIndex("dbo.SiteToEmps", new[] { "EmpId" });
            DropIndex("dbo.SiteToEmps", new[] { "SiteId" });
            DropIndex("dbo.Meetings", new[] { "BranchId" });
            DropIndex("dbo.LinkTables", new[] { "BranchId" });
            DropIndex("dbo.ErrandRequests", new[] { "SiteId" });
            DropIndex("dbo.EmpCustodies", new[] { "BranchId" });
            DropIndex("dbo.Custody", new[] { "BranchId" });
            DropIndex("dbo.Branches", new[] { "DistrictId" });
            DropIndex("dbo.Branches", new[] { "CityId" });
            DropIndex("dbo.Branches", new[] { "CountryId" });
            DropIndex("dbo.Branches", "IX_BranchName");
            DropIndex("dbo.Branches", "IX_BranchCode");
            DropIndex("dbo.Assignments", "IX_AssignmentBranch");
            AlterColumn("dbo.ErrandRequests", "SiteId", c => c.Int());
            AlterColumn("dbo.Assignments", "BranchId", c => c.Int());
            DropColumn("dbo.TrainPaths", "Branches");
            DropColumn("dbo.TrainCourses", "Branches");
            DropColumn("dbo.Meetings", "BranchId");
            DropColumn("dbo.LinkTables", "BranchId");
            DropColumn("dbo.InfoTables", "IBranch");
            DropColumn("dbo.EmpCustodies", "BranchId");
            DropColumn("dbo.Custody", "BranchId");
            DropColumn("dbo.Benefits", "Branches");
            DropColumn("dbo.LeaveTypes", "Branches");
            DropColumn("dbo.Assignments", "Branches");
            DropTable("dbo.SiteToEmps");
            DropTable("dbo.Branches");
            RenameIndex(table: "dbo.Meetings", name: "IX_SiteId", newName: "IX_LocationId");
            RenameIndex(table: "dbo.Sites", name: "IX_SiteName", newName: "IX_LocationName");
            RenameIndex(table: "dbo.Sites", name: "IX_SiteCode", newName: "IX_LocationCode");
            RenameColumn(table: "dbo.Meetings", name: "SiteId", newName: "LocationId");
            RenameColumn(table: "dbo.ErrandRequests", name: "SiteId", newName: "LocationId");
            CreateIndex("dbo.LinkTables", "LocationId");
            CreateIndex("dbo.ErrandRequests", "LocationId");
            CreateIndex("dbo.EmpCustodies", "LocationId");
            CreateIndex("dbo.Custody", "LocationId");
            CreateIndex("dbo.CompanyPartners", "AddressId");
            CreateIndex("dbo.CompanyBranches", "AddressId");
            CreateIndex("dbo.CompanyBranches", "CompanyId");
            CreateIndex("dbo.Assignments", "LocationId");
            CreateIndex("dbo.Assignments", "BranchId", name: "IX_AssignmentBranch");
            CreateIndex("dbo.Sites", "CompanyId");
            CreateIndex("dbo.Providers", "AddressId");
            CreateIndex("dbo.Kafeel", "AddressId");
            CreateIndex("dbo.LocationEmps", "EmpId");
            CreateIndex("dbo.LocationEmps", "LocationId");
            CreateIndex("dbo.Addresses", "DistrictId");
            CreateIndex("dbo.Addresses", "CityId");
            CreateIndex("dbo.Addresses", "CountryId");
            CreateIndex("dbo.Addresses", new[] { "Address1", "Address2", "Address3" }, unique: true, name: "IX_Address");
            CreateIndex("dbo.Companies", "AddressId");
            AddForeignKey("dbo.ErrandRequests", "LocationId", "dbo.Locations", "Id");
            AddForeignKey("dbo.LinkTables", "LocationId", "dbo.Locations", "Id");
            AddForeignKey("dbo.EmpCustodies", "LocationId", "dbo.Locations", "Id");
            AddForeignKey("dbo.Custody", "LocationId", "dbo.Locations", "Id");
            AddForeignKey("dbo.CompanyPartners", "AddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.CompanyBranches", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyBranches", "AddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.Assignments", "LocationId", "dbo.Locations", "Id");
            AddForeignKey("dbo.LocationEmps", "LocationId", "dbo.Locations", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Locations", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.LocationEmps", "EmpId", "dbo.People", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Providers", "AddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.Kafeel", "AddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.Companies", "AddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.Addresses", "DistrictId", "dbo.Districts", "Id");
            AddForeignKey("dbo.Addresses", "CountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.Addresses", "CityId", "dbo.Cities", "Id");
            RenameTable(name: "dbo.Sites", newName: "Locations");
        }
    }
}
