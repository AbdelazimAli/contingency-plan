namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSalaryItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountSetup", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.AccountSetup", "Spiltter", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AlterColumn("dbo.SalaryItems", "PrimaryClass", c => c.Byte());
            AlterColumn("dbo.SalaryItems", "SecondaryClass", c => c.Short());
            AlterColumn("dbo.SalaryItems", "UoMeasure", c => c.Byte());
            AlterColumn("dbo.SalaryItems", "ItemType", c => c.Byte());
            AlterColumn("dbo.SalaryItems", "Basis", c => c.Byte());
            CreateIndex("dbo.AccountSetup", "CompanyId");
            AddForeignKey("dbo.AccountSetup", "CompanyId", "dbo.Companies", "Id", cascadeDelete: false);
            DropColumn("dbo.Accounts", "Spiltter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "Spiltter", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            DropForeignKey("dbo.AccountSetup", "CompanyId", "dbo.Companies");
            DropIndex("dbo.AccountSetup", new[] { "CompanyId" });
            AlterColumn("dbo.SalaryItems", "Basis", c => c.Byte(nullable: false));
            AlterColumn("dbo.SalaryItems", "ItemType", c => c.Byte(nullable: false));
            AlterColumn("dbo.SalaryItems", "UoMeasure", c => c.Byte(nullable: false));
            AlterColumn("dbo.SalaryItems", "SecondaryClass", c => c.Short(nullable: false));
            AlterColumn("dbo.SalaryItems", "PrimaryClass", c => c.Byte(nullable: false));
            DropColumn("dbo.AccountSetup", "Spiltter");
            DropColumn("dbo.AccountSetup", "CompanyId");
        }
    }
}
