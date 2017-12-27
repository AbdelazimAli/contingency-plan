namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPersonInsurance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "SubscripDate", c => c.DateTime());
            AddColumn("dbo.People", "BasicSubAmt", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.People", "VarSubAmt", c => c.Decimal(precision: 18, scale: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "VarSubAmt");
            DropColumn("dbo.People", "BasicSubAmt");
            DropColumn("dbo.People", "SubscripDate");
        }
    }
}
