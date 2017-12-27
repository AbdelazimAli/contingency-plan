namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUserProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Culture", c => c.String(maxLength: 2, fixedLength: true, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Culture");
        }
    }
}
