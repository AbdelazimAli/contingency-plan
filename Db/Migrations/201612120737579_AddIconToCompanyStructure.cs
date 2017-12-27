namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIconToCompanyStructure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyStructures", "Icon", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyStructures", "Icon");
        }
    }
}
