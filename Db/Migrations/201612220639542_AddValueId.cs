namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddValueId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyDocAttrs", "ValueId", c => c.Int());
            AddColumn("dbo.FlexData", "ValueId", c => c.Int());
            AlterColumn("dbo.CompanyDocAttrs", "Value", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CompanyDocAttrs", "Value", c => c.String(maxLength: 100));
            DropColumn("dbo.FlexData", "ValueId");
            DropColumn("dbo.CompanyDocAttrs", "ValueId");
        }
    }
}
