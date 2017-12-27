namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCustodies : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.PagePrints");
            AlterColumn("dbo.PagePrints", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.PagePrints", "Id");
            DropColumn("dbo.Custody", "StartDate");
            DropColumn("dbo.Custody", "EndDate");
            DropColumn("dbo.DeptJobLeavePlans", "LeaveYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeptJobLeavePlans", "LeaveYear", c => c.Short(nullable: false));
            AddColumn("dbo.Custody", "EndDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Custody", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            DropPrimaryKey("dbo.PagePrints");
            AlterColumn("dbo.PagePrints", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.PagePrints", "Culture");
        }
    }
}
