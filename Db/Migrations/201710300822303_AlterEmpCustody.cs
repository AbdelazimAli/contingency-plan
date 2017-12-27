namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmpCustody : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmpCustodies", "RecvStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.EmpCustodies", "delvryStatus", c => c.Byte());
            AddColumn("dbo.RenewRequests", "RequestDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.RenewRequests", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.RenewRequests", "CreatedTime", c => c.DateTime());
            DropColumn("dbo.EmpCustodies", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmpCustodies", "Status", c => c.Byte(nullable: false));
            DropColumn("dbo.RenewRequests", "CreatedTime");
            DropColumn("dbo.RenewRequests", "CreatedUser");
            DropColumn("dbo.RenewRequests", "RequestDate");
            DropColumn("dbo.EmpCustodies", "delvryStatus");
            DropColumn("dbo.EmpCustodies", "RecvStatus");
        }
    }
}
