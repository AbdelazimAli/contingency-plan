namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Region", c => c.String(maxLength: 50));
            AddColumn("dbo.Companies", "Office", c => c.String(maxLength: 50));
            AddColumn("dbo.Companies", "Responsible", c => c.String(maxLength: 50));
            AddColumn("dbo.Companies", "LegalForm", c => c.Short());
            AddColumn("dbo.Companies", "AddressId", c => c.Int());
            CreateIndex("dbo.Companies", "AddressId");
            AddForeignKey("dbo.Companies", "AddressId", "dbo.Addresses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "AddressId", "dbo.Addresses");
            DropIndex("dbo.Companies", new[] { "AddressId" });
            DropColumn("dbo.Companies", "AddressId");
            DropColumn("dbo.Companies", "LegalForm");
            DropColumn("dbo.Companies", "Responsible");
            DropColumn("dbo.Companies", "Office");
            DropColumn("dbo.Companies", "Region");
        }
    }
}
