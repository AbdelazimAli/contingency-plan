namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeAlters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "Performance", c => c.Short());
            AddColumn("dbo.PersonSetup", "ExceedPlanLimit", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PersonSetup", "ExceedPlanLimit");
            DropColumn("dbo.Assignments", "Performance");
        }
    }
}
