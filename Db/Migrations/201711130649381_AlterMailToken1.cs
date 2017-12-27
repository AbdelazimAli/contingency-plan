namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterMailToken1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Mailtokens", "IX_Mailtoken");
            DropPrimaryKey("dbo.Mailtokens");
            AlterColumn("dbo.Mailtokens", "ObjectName", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AddPrimaryKey("dbo.Mailtokens", new[] { "ObjectName", "Culture", "Name" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Mailtokens");
            AlterColumn("dbo.Mailtokens", "ObjectName", c => c.String(maxLength: 30, unicode: false));
            AddPrimaryKey("dbo.Mailtokens", new[] { "Culture", "Name" });
            CreateIndex("dbo.Mailtokens", "ObjectName", name: "IX_Mailtoken");
        }
    }
}
