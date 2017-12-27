namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterWorkFlow1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Workflow");
            AlterColumn("dbo.Workflow", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Workflow", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Workflow");
            AlterColumn("dbo.Workflow", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Workflow", "Id");
        }
    }
}
