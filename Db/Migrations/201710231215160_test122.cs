namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test122 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LeaveRequests", "StartTolerance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveRequests", "StartTolerance", c => c.Short());
        }
    }
}
