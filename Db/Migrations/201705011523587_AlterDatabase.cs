namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Spiltter", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AddColumn("dbo.InfoTables", "IJob", c => c.Boolean(nullable: false));
            AddColumn("dbo.PayRequests", "EmpSelect", c => c.Byte(nullable: false));
            AddColumn("dbo.PayRequests", "PaySelect", c => c.Byte(nullable: false));
            AddColumn("dbo.PayrollSetup", "Spiltter", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AlterColumn("dbo.PayRequests", "RequestDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PayRequests", "RequestDate", c => c.DateTime(nullable: false, storeType: "date"));
            DropColumn("dbo.PayrollSetup", "Spiltter");
            DropColumn("dbo.PayRequests", "PaySelect");
            DropColumn("dbo.PayRequests", "EmpSelect");
            DropColumn("dbo.InfoTables", "IJob");
            DropColumn("dbo.Accounts", "Spiltter");
        }
    }
}
