namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Locations", "AddressId", "dbo.Addresses");
            DropIndex("dbo.Locations", new[] { "AddressId" });
            CreateTable(
                "dbo.HRLetterLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LetterId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        IssueDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.HRLetters", t => t.LetterId, cascadeDelete: false)
                .Index(t => new { t.LetterId, t.EmpId }, name: "IX_HRLetterLog");
            
            CreateTable(
                "dbo.HRLetters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        LetterTempl = c.String(maxLength: 100),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Locations", "CostCenter", c => c.String());
            AddColumn("dbo.Locations", "Address", c => c.String(maxLength: 250));
            AddColumn("dbo.Locations", "Latitude", c => c.Double());
            AddColumn("dbo.Locations", "Longitude", c => c.Double());
            AddColumn("dbo.BenefitServs", "BenefitClass", c => c.Short(nullable: false));
            AddColumn("dbo.Custody", "JobId", c => c.Int());
            AlterColumn("dbo.Locations", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.Jobs", "ContractTempl", c => c.String(maxLength: 100));
            AlterColumn("dbo.Personnels", "ContractTempl", c => c.String(maxLength: 100));
            CreateIndex("dbo.BenefitServs", new[] { "BenefitClass", "StartDate", "EndDate" }, name: "IX_BenefitServClass");
            CreateIndex("dbo.Custody", "JobId");
            AddForeignKey("dbo.Custody", "JobId", "dbo.Jobs", "Id");
            DropColumn("dbo.Locations", "AddressId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "AddressId", c => c.Int());
            DropForeignKey("dbo.HRLetterLogs", "LetterId", "dbo.HRLetters");
            DropForeignKey("dbo.HRLetterLogs", "EmpId", "dbo.People");
            DropForeignKey("dbo.Custody", "JobId", "dbo.Jobs");
            DropIndex("dbo.HRLetterLogs", "IX_HRLetterLog");
            DropIndex("dbo.Custody", new[] { "JobId" });
            DropIndex("dbo.BenefitServs", "IX_BenefitServClass");
            AlterColumn("dbo.Personnels", "ContractTempl", c => c.String(maxLength: 50));
            AlterColumn("dbo.Jobs", "ContractTempl", c => c.String(maxLength: 50));
            AlterColumn("dbo.Locations", "Code", c => c.String());
            DropColumn("dbo.Custody", "JobId");
            DropColumn("dbo.BenefitServs", "BenefitClass");
            DropColumn("dbo.Locations", "Longitude");
            DropColumn("dbo.Locations", "Latitude");
            DropColumn("dbo.Locations", "Address");
            DropColumn("dbo.Locations", "CostCenter");
            DropTable("dbo.HRLetters");
            DropTable("dbo.HRLetterLogs");
            CreateIndex("dbo.Locations", "AddressId");
            AddForeignKey("dbo.Locations", "AddressId", "dbo.Addresses", "Id");
        }
    }
}
