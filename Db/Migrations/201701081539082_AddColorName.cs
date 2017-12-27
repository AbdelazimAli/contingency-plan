namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColorName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyStructures", "ColorName", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyStructures", "ColorName");
        }
    }
}
