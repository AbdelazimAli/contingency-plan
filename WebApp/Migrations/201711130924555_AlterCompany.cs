namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCompany : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "Code", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "Code", c => c.String(maxLength: 20));
        }
    }
}
