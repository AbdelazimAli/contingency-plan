namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifyDaysDoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocTypes", "NotifyDays", c => c.Int(nullable: false));
            AlterColumn("dbo.People", "Status", c => c.Int(nullable: false));
            Sql("update people set [status] = (case [dbo].[fn_GetEmpStatus](id) when 12 then 2 when 0 then 1 else 4 end)");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "Status", c => c.Boolean(nullable: false));
            DropColumn("dbo.DocTypes", "NotifyDays");
        }
    }
}
