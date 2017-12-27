namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCheckList1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmpTasks", "ExpectDur", c => c.Short());
            AddColumn("dbo.EmpTasks", "EndTime", c => c.DateTime());
            DropColumn("dbo.EmpTasks", "ExpectDur");
            DropColumn("dbo.EmpTasks", "DoneTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmpTasks", "DoneTime", c => c.DateTime());
            AddColumn("dbo.EmpTasks", "ExpectDur", c => c.Short());
            DropColumn("dbo.EmpTasks", "EndTime");
            DropColumn("dbo.EmpTasks", "ExpectDur");
        }
    }
}
