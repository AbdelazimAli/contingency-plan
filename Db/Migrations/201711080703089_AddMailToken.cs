namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMailToken : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Custody", "IX_CustodyCode");
            CreateTable(
                "dbo.CustodyCats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Disposal = c.Boolean(nullable: false),
                        Prefix = c.String(maxLength: 15),
                        CodeLength = c.Byte(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_CustodyCat");
            
            CreateTable(
                "dbo.Mailtokens",
                c => new
                    {
                        Culture = c.String(nullable: false, maxLength: 15, unicode: false),
                        Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        Meaning = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => new { t.Culture, t.Name });
            
            AddColumn("dbo.Custody", "Curr", c => c.String(maxLength: 3, fixedLength: true, unicode: false));
            AddColumn("dbo.Custody", "CustodyCatId", c => c.Int(nullable: false));
            AddColumn("dbo.Custody", "Sequence", c => c.Int(nullable: false));
            AddColumn("dbo.EmpCustodies", "Notes", c => c.String(maxLength: 250));
            AlterColumn("dbo.Custody", "Code", c => c.String(maxLength: 30));
            CreateIndex("dbo.Custody", new[] { "CompanyId", "Code" }, name: "IX_CustodyCode");
            CreateIndex("dbo.Custody", "Curr");
            CreateIndex("dbo.Custody", "CustodyCatId");
            AddForeignKey("dbo.Custody", "Curr", "dbo.Currencies", "Code");
            AddForeignKey("dbo.Custody", "CustodyCatId", "dbo.CustodyCats", "Id", cascadeDelete: false);
            DropColumn("dbo.Custody", "CustodyCat");
            DropColumn("dbo.Custody", "Disposal");
            DropColumn("dbo.EmpCustodies", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmpCustodies", "Description", c => c.String(maxLength: 250));
            AddColumn("dbo.Custody", "Disposal", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "CustodyCat", c => c.Short());
            DropForeignKey("dbo.Custody", "CustodyCatId", "dbo.CustodyCats");
            DropForeignKey("dbo.Custody", "Curr", "dbo.Currencies");
            DropIndex("dbo.CustodyCats", "IX_CustodyCat");
            DropIndex("dbo.Custody", new[] { "CustodyCatId" });
            DropIndex("dbo.Custody", new[] { "Curr" });
            DropIndex("dbo.Custody", "IX_CustodyCode");
            AlterColumn("dbo.Custody", "Code", c => c.String(maxLength: 20));
            DropColumn("dbo.EmpCustodies", "Notes");
            DropColumn("dbo.Custody", "Sequence");
            DropColumn("dbo.Custody", "CustodyCatId");
            DropColumn("dbo.Custody", "Curr");
            DropTable("dbo.Mailtokens");
            DropTable("dbo.CustodyCats");
            CreateIndex("dbo.Custody", new[] { "CompanyId", "Code" }, unique: true, name: "IX_CustodyCode");
        }
    }
}
