namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUserProfile4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Culture", c => c.String(maxLength: 15));
            DropColumn("dbo.AspNetUsers", "Culture");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Culture", c => c.String(maxLength: 15));
            DropColumn("dbo.AspNetUsers", "Culture");
        }
    }
}
