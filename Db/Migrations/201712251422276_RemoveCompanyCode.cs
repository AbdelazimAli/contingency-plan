namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCompanyCode : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Companies", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "Code", c => c.Int(nullable: false));
        }
    }
}
