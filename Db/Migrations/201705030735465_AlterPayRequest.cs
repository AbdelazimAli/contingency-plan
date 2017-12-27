namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPayRequest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PayrollSetup", "CreditSettAcct", "dbo.Accounts");
            DropForeignKey("dbo.PayrollSetup", "DebitSettAcct", "dbo.Accounts");
            DropIndex("dbo.PayrollSetup", new[] { "DebitSettAcct" });
            DropIndex("dbo.PayrollSetup", new[] { "CreditSettAcct" });
            AddColumn("dbo.PayRequests", "Requester", c => c.Int(nullable: false));
            AddColumn("dbo.PayrollSetup", "DebitSettSal", c => c.Int());
            AddColumn("dbo.PayrollSetup", "CreditSettSal", c => c.Int());
            CreateIndex("dbo.PayRequests", "Requester");
            CreateIndex("dbo.PayrollSetup", "DebitSettSal");
            CreateIndex("dbo.PayrollSetup", "CreditSettSal");
            AddForeignKey("dbo.PayRequests", "Requester", "dbo.People", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PayrollSetup", "CreditSettSal", "dbo.SalaryItems", "Id");
            AddForeignKey("dbo.PayrollSetup", "DebitSettSal", "dbo.SalaryItems", "Id");
            DropColumn("dbo.PayrollSetup", "DebitSettAcct");
            DropColumn("dbo.PayrollSetup", "CreditSettAcct");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PayrollSetup", "CreditSettAcct", c => c.Int());
            AddColumn("dbo.PayrollSetup", "DebitSettAcct", c => c.Int());
            DropForeignKey("dbo.PayrollSetup", "DebitSettSal", "dbo.SalaryItems");
            DropForeignKey("dbo.PayrollSetup", "CreditSettSal", "dbo.SalaryItems");
            DropForeignKey("dbo.PayRequests", "Requester", "dbo.People");
            DropIndex("dbo.PayrollSetup", new[] { "CreditSettSal" });
            DropIndex("dbo.PayrollSetup", new[] { "DebitSettSal" });
            DropIndex("dbo.PayRequests", new[] { "Requester" });
            DropColumn("dbo.PayrollSetup", "CreditSettSal");
            DropColumn("dbo.PayrollSetup", "DebitSettSal");
            DropColumn("dbo.PayRequests", "Requester");
            CreateIndex("dbo.PayrollSetup", "CreditSettAcct");
            CreateIndex("dbo.PayrollSetup", "DebitSettAcct");
            AddForeignKey("dbo.PayrollSetup", "DebitSettAcct", "dbo.Accounts", "Id");
            AddForeignKey("dbo.PayrollSetup", "CreditSettAcct", "dbo.Accounts", "Id");
        }
    }
}
