namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTimeFraction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Custody", "CurrentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AddColumn("dbo.EmpCustodies", "CurrentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AddColumn("dbo.EmpCustodies", "ExpdelvryDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.LeaveRequests", "DayFraction", c => c.Byte(nullable: false));
            AlterColumn("dbo.EmpCustodies", "delvryDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.LeaveRequests", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.LeaveRequests", "EndDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.LeaveRequests", "ActualStartDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.LeaveRequests", "ActualEndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.LeaveRequests", "ReturnDate", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LeaveRequests", "ReturnDate", c => c.DateTime());
            AlterColumn("dbo.LeaveRequests", "ActualEndDate", c => c.DateTime());
            AlterColumn("dbo.LeaveRequests", "ActualStartDate", c => c.DateTime());
            AlterColumn("dbo.LeaveRequests", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveRequests", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmpCustodies", "delvryDate", c => c.DateTime());
            DropColumn("dbo.LeaveRequests", "DayFraction");
            DropColumn("dbo.EmpCustodies", "ExpdelvryDate");
            DropColumn("dbo.EmpCustodies", "CurrentAmount");
            DropColumn("dbo.Custody", "CurrentAmount");
        }
    }
}
