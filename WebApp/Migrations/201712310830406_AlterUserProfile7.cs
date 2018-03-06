namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUserProfile7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "DefferedEmp", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DefferedEmp");
            DropColumn("dbo.AspNetUsers", "IsAvailable");
        }
    }
}
