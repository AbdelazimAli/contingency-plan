namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Consignees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        Sent = c.Boolean(nullable: false),
                        ByEmail = c.Boolean(nullable: false),
                        MailSent = c.Boolean(nullable: false),
                        BySms = c.Boolean(nullable: false),
                        SmsSent = c.Boolean(nullable: false),
                        Read = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        SentTime = c.DateTime(),
                        ReadTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Notifications", t => t.NotificationId, cascadeDelete: true)
                .Index(t => t.NotificationId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(maxLength: 500),
                        CreationTime = c.DateTime(nullable: false),
                        SentTime = c.DateTime(),
                        ConditionId = c.Int(),
                        Sent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotifyConditions", t => t.ConditionId)
                .Index(t => t.ConditionId);
            
            CreateTable(
                "dbo.NotifyConditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransType = c.Byte(nullable: false),
                        TableName = c.String(maxLength: 30),
                        ColumnName = c.String(maxLength: 30),
                        Condition = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Consignees", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.Notifications", "ConditionId", "dbo.NotifyConditions");
            DropForeignKey("dbo.Consignees", "EmployeeId", "dbo.People");
            DropIndex("dbo.Notifications", new[] { "ConditionId" });
            DropIndex("dbo.Consignees", new[] { "EmployeeId" });
            DropIndex("dbo.Consignees", new[] { "NotificationId" });
            DropTable("dbo.NotifyConditions");
            DropTable("dbo.Notifications");
            DropTable("dbo.Consignees");
        }
    }
}
