namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContracts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employements", "Salary", c => c.Int());
            AddColumn("dbo.Employements", "Allowances", c => c.Int());
            AddColumn("dbo.Employements", "TicketCnt", c => c.Byte());
            AddColumn("dbo.Employements", "TicketAmt", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Employements", "FromCountry", c => c.Int());
            AddColumn("dbo.Employements", "ToCountry", c => c.Int());
            AddColumn("dbo.Employements", "JobDesc", c => c.String(maxLength: 500));
            AddColumn("dbo.Employements", "BenefitDesc", c => c.String(maxLength: 500));
            AddColumn("dbo.Employements", "SpecialCond", c => c.String(maxLength: 500));
            DropColumn("dbo.Employements", "SupenseReason");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employements", "SupenseReason", c => c.Byte());
            DropColumn("dbo.Employements", "SpecialCond");
            DropColumn("dbo.Employements", "BenefitDesc");
            DropColumn("dbo.Employements", "JobDesc");
            DropColumn("dbo.Employements", "ToCountry");
            DropColumn("dbo.Employements", "FromCountry");
            DropColumn("dbo.Employements", "TicketAmt");
            DropColumn("dbo.Employements", "TicketCnt");
            DropColumn("dbo.Employements", "Allowances");
            DropColumn("dbo.Employements", "Salary");
        }
    }
}
