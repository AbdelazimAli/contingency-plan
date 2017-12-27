namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMsgTemplate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MsgTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Body = c.String(maxLength: 1000),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotifyConsigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConditionId = c.Int(nullable: false),
                        SendTo = c.Byte(nullable: false),
                        EmpId = c.Int(),
                        JobId = c.Int(),
                        PosId = c.Int(),
                        CustMail = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotifyConditions", t => t.ConditionId, cascadeDelete: true)
                .Index(t => t.ConditionId);
            
            AddColumn("dbo.NotifyConditions", "TemplateId", c => c.Int(nullable: false));
            AddColumn("dbo.NotifyConditions", "Comment", c => c.String(maxLength: 100));
            AddColumn("dbo.NotifyConditions", "WebOrMob", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyConditions", "Sms", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyConditions", "Email", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyConditions", "NotifyRef", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotifyConditions", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.NotifyConditions", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.NotifyConditions", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.NotifyConditions", "ModifiedTime", c => c.DateTime());
            CreateIndex("dbo.NotifyConditions", "TemplateId");
            AddForeignKey("dbo.NotifyConditions", "TemplateId", "dbo.MsgTemplates", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotifyConsigns", "ConditionId", "dbo.NotifyConditions");
            DropForeignKey("dbo.NotifyConditions", "TemplateId", "dbo.MsgTemplates");
            DropIndex("dbo.NotifyConsigns", new[] { "ConditionId" });
            DropIndex("dbo.NotifyConditions", new[] { "TemplateId" });
            DropColumn("dbo.NotifyConditions", "ModifiedTime");
            DropColumn("dbo.NotifyConditions", "CreatedTime");
            DropColumn("dbo.NotifyConditions", "ModifiedUser");
            DropColumn("dbo.NotifyConditions", "CreatedUser");
            DropColumn("dbo.NotifyConditions", "NotifyRef");
            DropColumn("dbo.NotifyConditions", "Email");
            DropColumn("dbo.NotifyConditions", "Sms");
            DropColumn("dbo.NotifyConditions", "WebOrMob");
            DropColumn("dbo.NotifyConditions", "Comment");
            DropColumn("dbo.NotifyConditions", "TemplateId");
            DropTable("dbo.NotifyConsigns");
            DropTable("dbo.MsgTemplates");
        }
    }
}
