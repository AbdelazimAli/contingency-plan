namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterActualNofDays : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LeaveRequests", "ActualNofDays", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LeaveRequests", "ActualNofDays", c => c.Short());
        }
    }
}
