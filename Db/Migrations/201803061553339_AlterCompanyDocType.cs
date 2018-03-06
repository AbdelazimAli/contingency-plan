namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCompanyDocType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "Address", c => c.String(maxLength: 500));
            AlterColumn("dbo.DocTypes", "RequiredOpt", c => c.Byte());
            DropColumn("dbo.Companies", "AddressId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "AddressId", c => c.Int());
            AlterColumn("dbo.DocTypes", "RequiredOpt", c => c.Byte(nullable: false));
            DropColumn("dbo.Companies", "Address");
        }
    }
}
