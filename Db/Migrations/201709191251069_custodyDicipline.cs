namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class custodyDicipline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmpCustodies", "deliveryStatus", c => c.Short());
            AddColumn("dbo.EmpCustodies", "deliveryCause", c => c.String(maxLength: 250));
            AddColumn("dbo.Investigations", "ViolDate", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.LeaveRequests", "StartTolerance", c => c.Short());
            AlterColumn("dbo.EmpDisciplines", "DescionNo", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EmpDisciplines", "DescionNo", c => c.String());
            DropColumn("dbo.LeaveRequests", "StartTolerance");
            DropColumn("dbo.Investigations", "ViolDate");
            DropColumn("dbo.EmpCustodies", "deliveryCause");
            DropColumn("dbo.EmpCustodies", "deliveryStatus");
        }
    }
}
