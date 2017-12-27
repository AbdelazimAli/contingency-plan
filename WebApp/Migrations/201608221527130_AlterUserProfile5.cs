namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUserProfile5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Messages", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Messages");
        }
    }
}
