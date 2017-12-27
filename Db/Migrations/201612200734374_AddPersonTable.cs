namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlexColumns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PageId = c.Int(nullable: false),
                        ColumnOrder = c.Byte(nullable: false),
                        ColumnName = c.String(nullable: false, maxLength: 100, unicode: false),
                        isVisible = c.Boolean(nullable: false),
                        InputType = c.Byte(nullable: false),
                        CodeName = c.String(maxLength: 20),
                        Required = c.Boolean(nullable: false),
                        Min = c.Int(),
                        Max = c.Int(),
                        Pattern = c.String(maxLength: 100),
                        PlaceHolder = c.String(maxLength: 100),
                        IsUnique = c.Boolean(nullable: false),
                        UniqueColumns = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PageDivs", t => t.PageId, cascadeDelete: true)
                .Index(t => new { t.PageId, t.ColumnName }, unique: true, name: "IX_FlexColumn");
            
            CreateTable(
                "dbo.FlexData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PageId = c.Int(nullable: false),
                        ColumnName = c.String(nullable: false, maxLength: 100, unicode: false),
                        Reference = c.Guid(nullable: false),
                        Value = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PageDivs", t => t.PageId, cascadeDelete: true)
                .Index(t => new { t.PageId, t.ColumnName, t.Reference }, unique: true, name: "IX_FlexData");
            
            CreateTable(
                "dbo.Hosiptals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        AddressId = c.Int(),
                        Manager = c.String(maxLength: 100),
                        Tel = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Kafeel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        AddressId = c.Int(),
                        Tel = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 20),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        Fathername = c.String(maxLength: 20),
                        GFathername = c.String(maxLength: 20),
                        Familyname = c.String(nullable: false, maxLength: 20),
                        Gender = c.Byte(nullable: false),
                        PersonType = c.Byte(nullable: false),
                        NationalId = c.String(maxLength: 20),
                        Ssn = c.String(maxLength: 20),
                        JoinDate = c.DateTime(),
                        StartExpDate = c.DateTime(),
                        QualificationId = c.Int(),
                        BirthDate = c.DateTime(nullable: false, storeType: "date"),
                        BirthCountry = c.Int(),
                        BirthCity = c.Int(),
                        BirthDstrct = c.Int(),
                        Nationality = c.Int(),
                        MaritalStat = c.Byte(),
                        TaxFamlyCnt = c.Byte(),
                        BnftFamlyCnt = c.Byte(),
                        Religon = c.Byte(),
                        AddressId = c.Int(),
                        HoAddressId = c.Int(),
                        Mobile = c.String(maxLength: 20),
                        HomeTel = c.String(maxLength: 20),
                        EmergencyTel = c.String(maxLength: 20),
                        WorkEmail = c.String(maxLength: 20),
                        OtherEmail = c.String(maxLength: 20),
                        MilitaryStat = c.Byte(),
                        MilStatDate = c.DateTime(),
                        MilitaryNo = c.String(maxLength: 20),
                        PassportNo = c.String(maxLength: 20),
                        IssueDate = c.DateTime(storeType: "date"),
                        ExpiryDate = c.DateTime(storeType: "date"),
                        IssuePlace = c.String(maxLength: 100),
                        Profession = c.String(maxLength: 50),
                        KafeelId = c.Int(),
                        MedicalStat = c.Byte(),
                        MedStatDate = c.DateTime(storeType: "date"),
                        InspectDate = c.DateTime(storeType: "date"),
                        HosiptalId = c.Int(),
                        BloodClass = c.Byte(),
                        Recommend = c.String(maxLength: 200),
                        RecmndReson = c.Byte(),
                        HasCV = c.Boolean(nullable: false),
                        UpdtCVDate = c.DateTime(nullable: false, storeType: "date"),
                        LocationId = c.Int(),
                        RoomNo = c.String(maxLength: 10),
                        Reference = c.Guid(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Addresses", t => t.HoAddressId)
                .ForeignKey("dbo.Hosiptals", t => t.HosiptalId)
                .ForeignKey("dbo.Kafeel", t => t.KafeelId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.Qualifications", t => t.QualificationId)
                .Index(t => t.FirstName, name: "IX_PersonName, 1")
                .Index(t => t.Fathername, name: "IX_PersonName, 2")
                .Index(t => t.GFathername, name: "IX_PersonName, 3")
                .Index(t => t.Familyname, name: "IX_PersonName, 4")
                .Index(t => t.QualificationId)
                .Index(t => t.AddressId)
                .Index(t => t.HoAddressId)
                .Index(t => t.KafeelId)
                .Index(t => t.HosiptalId)
                .Index(t => t.LocationId);
            
            DropColumn("dbo.Jobs", "CustInt1");
            DropColumn("dbo.Jobs", "CustInt2");
            DropColumn("dbo.Jobs", "CustDecimal3");
            DropColumn("dbo.Jobs", "CustDecimal4");
            DropColumn("dbo.Jobs", "CustDate1");
            DropColumn("dbo.Jobs", "CustDate2");
            DropColumn("dbo.Jobs", "CustDate3");
            DropColumn("dbo.Jobs", "CustDate4");
            DropColumn("dbo.Jobs", "CustText1");
            DropColumn("dbo.Jobs", "CustText2");
            DropColumn("dbo.Jobs", "CustText3");
            DropColumn("dbo.Jobs", "CustText4");
            DropColumn("dbo.Jobs", "CustText5");
            DropColumn("dbo.Jobs", "CustText6");
            DropColumn("dbo.Jobs", "CustText7");
            DropColumn("dbo.Jobs", "CustText8");
            DropColumn("dbo.CompanyStructures", "CustInt1");
            DropColumn("dbo.CompanyStructures", "CustInt2");
            DropColumn("dbo.CompanyStructures", "CustDecimal3");
            DropColumn("dbo.CompanyStructures", "CustDecimal4");
            DropColumn("dbo.CompanyStructures", "CustDate1");
            DropColumn("dbo.CompanyStructures", "CustDate2");
            DropColumn("dbo.CompanyStructures", "CustDate3");
            DropColumn("dbo.CompanyStructures", "CustDate4");
            DropColumn("dbo.CompanyStructures", "CustText1");
            DropColumn("dbo.CompanyStructures", "CustText2");
            DropColumn("dbo.CompanyStructures", "CustText3");
            DropColumn("dbo.CompanyStructures", "CustText4");
            DropColumn("dbo.CompanyStructures", "CustText5");
            DropColumn("dbo.CompanyStructures", "CustText6");
            DropColumn("dbo.CompanyStructures", "CustText7");
            DropColumn("dbo.CompanyStructures", "CustText8");
            DropColumn("dbo.Positions", "CustInt1");
            DropColumn("dbo.Positions", "CustInt2");
            DropColumn("dbo.Positions", "CustDecimal3");
            DropColumn("dbo.Positions", "CustDecimal4");
            DropColumn("dbo.Positions", "CustDate1");
            DropColumn("dbo.Positions", "CustDate2");
            DropColumn("dbo.Positions", "CustDate3");
            DropColumn("dbo.Positions", "CustDate4");
            DropColumn("dbo.Positions", "CustText1");
            DropColumn("dbo.Positions", "CustText2");
            DropColumn("dbo.Positions", "CustText3");
            DropColumn("dbo.Positions", "CustText4");
            DropColumn("dbo.Positions", "CustText5");
            DropColumn("dbo.Positions", "CustText6");
            DropColumn("dbo.Positions", "CustText7");
            DropColumn("dbo.Positions", "CustText8");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Positions", "CustText8", c => c.String(maxLength: 250));
            AddColumn("dbo.Positions", "CustText7", c => c.String(maxLength: 250));
            AddColumn("dbo.Positions", "CustText6", c => c.String(maxLength: 250));
            AddColumn("dbo.Positions", "CustText5", c => c.String(maxLength: 250));
            AddColumn("dbo.Positions", "CustText4", c => c.String(maxLength: 250));
            AddColumn("dbo.Positions", "CustText3", c => c.String(maxLength: 250));
            AddColumn("dbo.Positions", "CustText2", c => c.String(maxLength: 250));
            AddColumn("dbo.Positions", "CustText1", c => c.String(maxLength: 250));
            AddColumn("dbo.Positions", "CustDate4", c => c.DateTime());
            AddColumn("dbo.Positions", "CustDate3", c => c.DateTime());
            AddColumn("dbo.Positions", "CustDate2", c => c.DateTime());
            AddColumn("dbo.Positions", "CustDate1", c => c.DateTime());
            AddColumn("dbo.Positions", "CustDecimal4", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Positions", "CustDecimal3", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Positions", "CustInt2", c => c.Int());
            AddColumn("dbo.Positions", "CustInt1", c => c.Int());
            AddColumn("dbo.CompanyStructures", "CustText8", c => c.String(maxLength: 250));
            AddColumn("dbo.CompanyStructures", "CustText7", c => c.String(maxLength: 250));
            AddColumn("dbo.CompanyStructures", "CustText6", c => c.String(maxLength: 250));
            AddColumn("dbo.CompanyStructures", "CustText5", c => c.String(maxLength: 250));
            AddColumn("dbo.CompanyStructures", "CustText4", c => c.String(maxLength: 250));
            AddColumn("dbo.CompanyStructures", "CustText3", c => c.String(maxLength: 250));
            AddColumn("dbo.CompanyStructures", "CustText2", c => c.String(maxLength: 250));
            AddColumn("dbo.CompanyStructures", "CustText1", c => c.String(maxLength: 250));
            AddColumn("dbo.CompanyStructures", "CustDate4", c => c.DateTime());
            AddColumn("dbo.CompanyStructures", "CustDate3", c => c.DateTime());
            AddColumn("dbo.CompanyStructures", "CustDate2", c => c.DateTime());
            AddColumn("dbo.CompanyStructures", "CustDate1", c => c.DateTime());
            AddColumn("dbo.CompanyStructures", "CustDecimal4", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CompanyStructures", "CustDecimal3", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CompanyStructures", "CustInt2", c => c.Int());
            AddColumn("dbo.CompanyStructures", "CustInt1", c => c.Int());
            AddColumn("dbo.Jobs", "CustText8", c => c.String(maxLength: 250));
            AddColumn("dbo.Jobs", "CustText7", c => c.String(maxLength: 250));
            AddColumn("dbo.Jobs", "CustText6", c => c.String(maxLength: 250));
            AddColumn("dbo.Jobs", "CustText5", c => c.String(maxLength: 250));
            AddColumn("dbo.Jobs", "CustText4", c => c.String(maxLength: 250));
            AddColumn("dbo.Jobs", "CustText3", c => c.String(maxLength: 250));
            AddColumn("dbo.Jobs", "CustText2", c => c.String(maxLength: 250));
            AddColumn("dbo.Jobs", "CustText1", c => c.String(maxLength: 250));
            AddColumn("dbo.Jobs", "CustDate4", c => c.DateTime());
            AddColumn("dbo.Jobs", "CustDate3", c => c.DateTime());
            AddColumn("dbo.Jobs", "CustDate2", c => c.DateTime());
            AddColumn("dbo.Jobs", "CustDate1", c => c.DateTime());
            AddColumn("dbo.Jobs", "CustDecimal4", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Jobs", "CustDecimal3", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Jobs", "CustInt2", c => c.Int());
            AddColumn("dbo.Jobs", "CustInt1", c => c.Int());
            DropForeignKey("dbo.People", "QualificationId", "dbo.Qualifications");
            DropForeignKey("dbo.People", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.People", "KafeelId", "dbo.Kafeel");
            DropForeignKey("dbo.People", "HosiptalId", "dbo.Hosiptals");
            DropForeignKey("dbo.People", "HoAddressId", "dbo.Addresses");
            DropForeignKey("dbo.People", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Kafeel", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Hosiptals", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.FlexData", "PageId", "dbo.PageDivs");
            DropForeignKey("dbo.FlexColumns", "PageId", "dbo.PageDivs");
            DropIndex("dbo.People", new[] { "LocationId" });
            DropIndex("dbo.People", new[] { "HosiptalId" });
            DropIndex("dbo.People", new[] { "KafeelId" });
            DropIndex("dbo.People", new[] { "HoAddressId" });
            DropIndex("dbo.People", new[] { "AddressId" });
            DropIndex("dbo.People", new[] { "QualificationId" });
            DropIndex("dbo.People", "IX_PersonName, 4");
            DropIndex("dbo.People", "IX_PersonName, 3");
            DropIndex("dbo.People", "IX_PersonName, 2");
            DropIndex("dbo.People", "IX_PersonName, 1");
            DropIndex("dbo.Kafeel", new[] { "AddressId" });
            DropIndex("dbo.Hosiptals", new[] { "AddressId" });
            DropIndex("dbo.FlexData", "IX_FlexData");
            DropIndex("dbo.FlexColumns", "IX_FlexColumn");
            DropTable("dbo.People");
            DropTable("dbo.Kafeel");
            DropTable("dbo.Hosiptals");
            DropTable("dbo.FlexData");
            DropTable("dbo.FlexColumns");
        }
    }
}
