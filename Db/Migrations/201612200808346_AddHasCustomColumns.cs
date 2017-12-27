namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHasCustomColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PageDivs", "HasCustCols", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PageDivs", "HasCustCols");
        }
    }
}
