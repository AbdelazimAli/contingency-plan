namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnDefaultValues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormColumns", "DefaultValue", c => c.String(maxLength: 50));
            AddColumn("dbo.GridColumns", "DefaultValue", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GridColumns", "DefaultValue");
            DropColumn("dbo.FormColumns", "DefaultValue");
        }
    }
}
