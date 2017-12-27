namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MsgEmployees", "MessageId", c => c.Int(nullable: false));
            AlterColumn("dbo.Messages", "Body", c => c.String(maxLength: 2000));
            CreateIndex("dbo.MsgEmployees", "MessageId");
            AddForeignKey("dbo.MsgEmployees", "MessageId", "dbo.Messages", "Id");
            DropColumn("dbo.MsgEmployees", "Title");
            DropColumn("dbo.MsgEmployees", "Body");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MsgEmployees", "Body", c => c.String(maxLength: 500));
            AddColumn("dbo.MsgEmployees", "Title", c => c.String(maxLength: 50));
            DropForeignKey("dbo.MsgEmployees", "MessageId", "dbo.Messages");
            DropIndex("dbo.MsgEmployees", new[] { "MessageId" });
            AlterColumn("dbo.Messages", "Body", c => c.String(maxLength: 500));
            DropColumn("dbo.MsgEmployees", "MessageId");
        }
    }
}
