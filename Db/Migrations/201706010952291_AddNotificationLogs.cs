namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationLogs : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Conditions", newName: "Filters");
            DropForeignKey("dbo.Consignees", "Employee_Id", "dbo.People");
            DropForeignKey("dbo.Consignees", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.MsgTemplangs", "EmailId", "dbo.EmailAccounts");
            DropForeignKey("dbo.MsgTemplates", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.MsgTemplangs", "TemplateId", "dbo.MsgTemplates");
            DropIndex("dbo.Consignees", new[] { "NotificationId" });
            DropIndex("dbo.Consignees", new[] { "Employee_Id" });
            DropIndex("dbo.MsgTemplangs", new[] { "TemplateId" });
            DropIndex("dbo.MsgTemplangs", new[] { "EmailId" });
            DropIndex("dbo.MsgTemplates", new[] { "CompanyId" });
            CreateTable(
                "dbo.EmailLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificatId = c.Int(nullable: false),
                        SentToUser = c.String(maxLength: 20),
                        SentOk = c.Boolean(nullable: false),
                        SentTime = c.DateTime(nullable: false),
                        Error = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notifications", t => t.NotificatId, cascadeDelete: true)
                .Index(t => t.NotificatId)
                .Index(t => t.SentToUser, name: "IX_EmailLog");
            
            CreateTable(
                "dbo.SmsLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificatId = c.Int(nullable: false),
                        SentToUser = c.String(maxLength: 20),
                        SentOk = c.Boolean(nullable: false),
                        SentTime = c.DateTime(nullable: false),
                        Error = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notifications", t => t.NotificatId, cascadeDelete: true)
                .Index(t => t.NotificatId)
                .Index(t => t.SentToUser, name: "IX_SmsLog");
            
            CreateTable(
                "dbo.WebMobLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificatId = c.Int(nullable: false),
                        Subject = c.String(maxLength: 100),
                        Message = c.String(maxLength: 1000),
                        SentToUser = c.String(maxLength: 20),
                        SentTime = c.DateTime(nullable: false),
                        MarkAsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notifications", t => t.NotificatId, cascadeDelete: true)
                .Index(t => t.NotificatId)
                .Index(t => t.SentToUser, name: "IX_WebMobLog");
            
            AddColumn("dbo.NotifyConditions", "Event", c => c.Byte(nullable: false));
            AddColumn("dbo.NotifyConditions", "AlertMeFor", c => c.Byte(nullable: false));
            AddColumn("dbo.NotifyConditions", "filter", c => c.String(maxLength: 500));
            AddColumn("dbo.NotifyConditions", "Message", c => c.String(maxLength: 1000));
            AddColumn("dbo.NotifyConditions", "CustEmail", c => c.String(maxLength: 50));
            AddColumn("dbo.SchedualTasks", "PeriodInMinutes", c => c.Int(nullable: false));
            AddColumn("dbo.SchedualTasks", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.SchedualTasks", "ModifiedUser", c => c.String());
            AddColumn("dbo.SchedualTasks", "ModifiedTime", c => c.DateTime());
            AlterColumn("dbo.Notifications", "Message", c => c.String(maxLength: 1000));
            DropColumn("dbo.NotifyConditions", "TransType");
            DropColumn("dbo.NotifyConditions", "Condition");
            DropColumn("dbo.NotifyConditions", "Body");
            DropTable("dbo.Consignees");
            DropTable("dbo.MsgTemplangs");
            DropTable("dbo.MsgTemplates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MsgTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        CompanyId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MsgTemplangs",
                c => new
                    {
                        TemplateId = c.Int(nullable: false),
                        Culture = c.String(nullable: false, maxLength: 15, unicode: false),
                        Subject = c.String(maxLength: 150),
                        Body = c.String(maxLength: 1000),
                        Bcc = c.String(maxLength: 100),
                        EmailId = c.Int(),
                    })
                .PrimaryKey(t => new { t.TemplateId, t.Culture });
            
            CreateTable(
                "dbo.Consignees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        Sent = c.Boolean(nullable: false),
                        ByEmail = c.Boolean(nullable: false),
                        MailSent = c.Boolean(nullable: false),
                        BySms = c.Boolean(nullable: false),
                        SmsSent = c.Boolean(nullable: false),
                        Read = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        SentTime = c.DateTime(),
                        ReadTime = c.DateTime(),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.NotifyConditions", "Body", c => c.String(maxLength: 1000));
            AddColumn("dbo.NotifyConditions", "Condition", c => c.String(maxLength: 500));
            AddColumn("dbo.NotifyConditions", "TransType", c => c.Byte(nullable: false));
            DropForeignKey("dbo.WebMobLogs", "NotificatId", "dbo.Notifications");
            DropForeignKey("dbo.SmsLogs", "NotificatId", "dbo.Notifications");
            DropForeignKey("dbo.EmailLogs", "NotificatId", "dbo.Notifications");
            DropIndex("dbo.WebMobLogs", "IX_WebMobLog");
            DropIndex("dbo.WebMobLogs", new[] { "NotificatId" });
            DropIndex("dbo.SmsLogs", "IX_SmsLog");
            DropIndex("dbo.SmsLogs", new[] { "NotificatId" });
            DropIndex("dbo.EmailLogs", "IX_EmailLog");
            DropIndex("dbo.EmailLogs", new[] { "NotificatId" });
            AlterColumn("dbo.Notifications", "Message", c => c.String(maxLength: 500));
            DropColumn("dbo.SchedualTasks", "ModifiedTime");
            DropColumn("dbo.SchedualTasks", "ModifiedUser");
            DropColumn("dbo.SchedualTasks", "CompanyId");
            DropColumn("dbo.SchedualTasks", "PeriodInMinutes");
            DropColumn("dbo.NotifyConditions", "CustEmail");
            DropColumn("dbo.NotifyConditions", "Message");
            DropColumn("dbo.NotifyConditions", "filter");
            DropColumn("dbo.NotifyConditions", "AlertMeFor");
            DropColumn("dbo.NotifyConditions", "Event");
            DropTable("dbo.WebMobLogs");
            DropTable("dbo.SmsLogs");
            DropTable("dbo.EmailLogs");
            CreateIndex("dbo.MsgTemplates", "CompanyId");
            CreateIndex("dbo.MsgTemplangs", "EmailId");
            CreateIndex("dbo.MsgTemplangs", "TemplateId");
            CreateIndex("dbo.Consignees", "Employee_Id");
            CreateIndex("dbo.Consignees", "NotificationId");
            AddForeignKey("dbo.MsgTemplangs", "TemplateId", "dbo.MsgTemplates", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MsgTemplates", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MsgTemplangs", "EmailId", "dbo.EmailAccounts", "Id");
            AddForeignKey("dbo.Consignees", "NotificationId", "dbo.Notifications", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Consignees", "Employee_Id", "dbo.People", "Id");
            RenameTable(name: "dbo.Filters", newName: "Conditions");
        }
    }
}
