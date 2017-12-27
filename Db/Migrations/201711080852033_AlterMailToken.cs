namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterMailToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Mailtokens", "ObjectName", c => c.String(maxLength: 30, unicode: false));
            CreateIndex("dbo.Mailtokens", "ObjectName", name: "IX_Mailtoken");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mailtokens", "IX_Mailtoken");
            DropColumn("dbo.Mailtokens", "ObjectName");
        }
    }
}
