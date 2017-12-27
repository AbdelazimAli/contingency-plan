namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmpTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "EmpTasks", c => c.Byte());
            AddColumn("dbo.EmpChkLists", "ManagerId", c => c.Int());
            AddColumn("dbo.EmpTasks", "SubPeriodId", c => c.Int());
            AddColumn("dbo.Personnels", "TaskCalendarId", c => c.Int());
            CreateIndex("dbo.EmpChkLists", "ManagerId");
            CreateIndex("dbo.EmpTasks", "SubPeriodId");
            CreateIndex("dbo.Personnels", "TaskCalendarId");
            AddForeignKey("dbo.EmpChkLists", "ManagerId", "dbo.People", "Id");
            AddForeignKey("dbo.EmpTasks", "SubPeriodId", "dbo.SubPeriods", "Id");
            AddForeignKey("dbo.Personnels", "TaskCalendarId", "dbo.PeriodNames", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Personnels", "TaskCalendarId", "dbo.PeriodNames");
            DropForeignKey("dbo.EmpTasks", "SubPeriodId", "dbo.SubPeriods");
            DropForeignKey("dbo.EmpChkLists", "ManagerId", "dbo.People");
            DropIndex("dbo.Personnels", new[] { "TaskCalendarId" });
            DropIndex("dbo.EmpTasks", new[] { "SubPeriodId" });
            DropIndex("dbo.EmpChkLists", new[] { "ManagerId" });
            DropColumn("dbo.Personnels", "TaskCalendarId");
            DropColumn("dbo.EmpTasks", "SubPeriodId");
            DropColumn("dbo.EmpChkLists", "ManagerId");
            DropColumn("dbo.Assignments", "EmpTasks");
        }
    }
}
