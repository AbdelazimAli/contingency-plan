namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUserProfile1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Culture", c => c.String(maxLength: 5, fixedLength: true, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Culture", c => c.String(maxLength: 2, fixedLength: true, unicode: false));
        }
    }
}
