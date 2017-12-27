namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotificationCondition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NotifyConditions", "TemplateId", "dbo.MsgTemplates");
            DropForeignKey("dbo.NotifyConsigns", "Employee_Id", "dbo.People");
            DropForeignKey("dbo.NotifyConsigns", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.NotifyConsigns", "NotifyCondId", "dbo.NotifyConditions");
            DropForeignKey("dbo.NotifyConsigns", "PositionId", "dbo.Positions");
            DropIndex("dbo.NotifyConditions", new[] { "TemplateId" });
            DropIndex("dbo.NotifyConsigns", new[] { "NotifyCondId" });
            DropIndex("dbo.NotifyConsigns", new[] { "JobId" });
            DropIndex("dbo.NotifyConsigns", new[] { "PositionId" });
            DropIndex("dbo.NotifyConsigns", new[] { "Employee_Id" });
            AddColumn("dbo.NotifyConditions", "Subject", c => c.String(maxLength: 100));
            AddColumn("dbo.NotifyConditions", "Body", c => c.String(maxLength: 1000));
            AddColumn("dbo.NotifyConditions", "Users", c => c.String(maxLength: 250));
            DropColumn("dbo.NotifyConditions", "TemplateId");
            DropColumn("dbo.NotifyConditions", "Comment");
            DropTable("dbo.NotifyConsigns");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.NotifyConsigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotifyCondId = c.Int(nullable: false),
                        SendTo = c.Byte(nullable: false),
                        EmpId = c.Int(),
                        JobId = c.Int(),
                        PositionId = c.Int(),
                        CustMail = c.String(maxLength: 30),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.NotifyConditions", "Comment", c => c.String(maxLength: 100));
            AddColumn("dbo.NotifyConditions", "TemplateId", c => c.Int(nullable: false));
            DropColumn("dbo.NotifyConditions", "Users");
            DropColumn("dbo.NotifyConditions", "Body");
            DropColumn("dbo.NotifyConditions", "Subject");
            CreateIndex("dbo.NotifyConsigns", "Employee_Id");
            CreateIndex("dbo.NotifyConsigns", "PositionId");
            CreateIndex("dbo.NotifyConsigns", "JobId");
            CreateIndex("dbo.NotifyConsigns", "NotifyCondId");
            CreateIndex("dbo.NotifyConditions", "TemplateId");
            AddForeignKey("dbo.NotifyConsigns", "PositionId", "dbo.Positions", "Id");
            AddForeignKey("dbo.NotifyConsigns", "NotifyCondId", "dbo.NotifyConditions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotifyConsigns", "JobId", "dbo.Jobs", "Id");
            AddForeignKey("dbo.NotifyConsigns", "Employee_Id", "dbo.People", "Id");
            AddForeignKey("dbo.NotifyConditions", "TemplateId", "dbo.MsgTemplates", "Id", cascadeDelete: true);
        }
    }
}
