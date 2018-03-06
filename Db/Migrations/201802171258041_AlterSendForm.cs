namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSendForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SendForms", "CompanyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SendForms", "CompanyId");
        }
    }
}
