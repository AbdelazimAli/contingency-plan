namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Covenant : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PeopleQuals", newName: "PeopleQuals");
            CreateTable(
                "dbo.Custody",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 20),
                        Name = c.String(maxLength: 150),
                        Description = c.String(maxLength: 250),
                        CovenantCat = c.Byte(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        SerialNo = c.String(maxLength: 20),
                        PurchaseDate = c.DateTime(),
                        Keyword = c.String(maxLength: 100),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmpCustodies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpId = c.Int(nullable: false),
                        RecvDate = c.DateTime(),
                        CovenantStat = c.Byte(),
                        delvryDate = c.DateTime(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .Index(t => t.EmpId);
            
            AddColumn("dbo.Addresses", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Addresses", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Addresses", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.Addresses", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.Countries", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Countries", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Countries", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.Countries", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.Locations", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Locations", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Locations", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.Locations", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.LeaveTrans", "TransDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Payrolls", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Payrolls", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Payrolls", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.Payrolls", "ModifiedTime", c => c.DateTime());
            DropColumn("dbo.LeaveTrans", "TransTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveTrans", "TransTime", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.EmpCustodies", "EmpId", "dbo.People");
            DropIndex("dbo.EmpCustodies", new[] { "EmpId" });
            DropColumn("dbo.Payrolls", "ModifiedTime");
            DropColumn("dbo.Payrolls", "CreatedTime");
            DropColumn("dbo.Payrolls", "ModifiedUser");
            DropColumn("dbo.Payrolls", "CreatedUser");
            DropColumn("dbo.LeaveTrans", "TransDate");
            DropColumn("dbo.Locations", "ModifiedTime");
            DropColumn("dbo.Locations", "CreatedTime");
            DropColumn("dbo.Locations", "ModifiedUser");
            DropColumn("dbo.Locations", "CreatedUser");
            DropColumn("dbo.Countries", "ModifiedTime");
            DropColumn("dbo.Countries", "CreatedTime");
            DropColumn("dbo.Countries", "ModifiedUser");
            DropColumn("dbo.Countries", "CreatedUser");
            DropColumn("dbo.Addresses", "ModifiedTime");
            DropColumn("dbo.Addresses", "CreatedTime");
            DropColumn("dbo.Addresses", "ModifiedUser");
            DropColumn("dbo.Addresses", "CreatedUser");
            DropTable("dbo.EmpCustodies");
            DropTable("dbo.Custody");
            RenameTable(name: "dbo.PeopleQuals", newName: "PeopleQuals");
        }
    }
}
