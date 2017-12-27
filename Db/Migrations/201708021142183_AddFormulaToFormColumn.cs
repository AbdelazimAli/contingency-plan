namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFormulaToFormColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormColumns", "Formula", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormColumns", "Formula");
        }
    }
}
