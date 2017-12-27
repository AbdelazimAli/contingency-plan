namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCheckList2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChecklistTasks", "ExpectDur", c => c.Short());
            DropColumn("dbo.ChecklistTasks", "ExpectTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChecklistTasks", "ExpectTime", c => c.Short());
            DropColumn("dbo.ChecklistTasks", "ExpectDur");
        }
    }
}
