namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLanguage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Language", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Language");
        }
    }
}
