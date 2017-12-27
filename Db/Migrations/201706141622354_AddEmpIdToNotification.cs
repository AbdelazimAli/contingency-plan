namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmpIdToNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "EmpId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "EmpId");
        }
    }
}
