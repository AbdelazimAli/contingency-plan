namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmployment2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeaveTypes", "ExWorkflow", c => c.Boolean(nullable: false));
            AddColumn("dbo.RequestWf", "ForceUpload", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Employements", "DurInYears", c => c.Byte());
            AlterColumn("dbo.Employements", "DurInMonths", c => c.Byte());
            DropColumn("dbo.PagePrints", "ForceUpload");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PagePrints", "ForceUpload", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Employements", "DurInMonths", c => c.Byte(nullable: false));
            AlterColumn("dbo.Employements", "DurInYears", c => c.Byte(nullable: false));
            DropColumn("dbo.RequestWf", "ForceUpload");
            DropColumn("dbo.LeaveTypes", "ExWorkflow");
        }
    }
}
