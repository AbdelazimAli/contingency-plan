namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailAccount : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.NotifyConsigns", name: "ConditionId", newName: "NotifyCondId");
            RenameIndex(table: "dbo.NotifyConsigns", name: "IX_ConditionId", newName: "IX_NotifyCondId");
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotifyCondId = c.Int(nullable: false),
                        ObjectName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Version = c.Byte(nullable: false),
                        ColumnName = c.String(nullable: false, maxLength: 50, unicode: false),
                        ColumnType = c.String(maxLength: 20),
                        Operator = c.String(maxLength: 10),
                        Value = c.String(maxLength: 50),
                        AndOr = c.String(maxLength: 3),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotifyConditions", t => t.NotifyCondId, cascadeDelete: true)
                .Index(t => t.NotifyCondId)
                .Index(t => new { t.ObjectName, t.Version, t.ColumnName }, name: "IX_Condition");
            
            CreateTable(
                "dbo.EmailAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 100),
                        DisplayName = c.String(maxLength: 100),
                        Host = c.String(maxLength: 150),
                        Port = c.Int(nullable: false),
                        Username = c.String(maxLength: 100),
                        Password = c.String(maxLength: 100),
                        EnableSsl = c.Boolean(nullable: false),
                        UseDefaultCredentials = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        LanguageCulture = c.String(maxLength: 20),
                        UniqueSeoCode = c.String(maxLength: 2),
                        FlagImageFileName = c.String(maxLength: 20),
                        Rtl = c.Boolean(nullable: false),
                        DefaultCurrencyId = c.Int(),
                        Active = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(),
                        DefaultCurrency_Code = c.String(maxLength: 3, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.DefaultCurrency_Code)
                .Index(t => t.DefaultCurrency_Code);
            
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
                .PrimaryKey(t => new { t.TemplateId, t.Culture })
                .ForeignKey("dbo.EmailAccounts", t => t.EmailId)
                .ForeignKey("dbo.MsgTemplates", t => t.TemplateId, cascadeDelete: true)
                .Index(t => t.TemplateId)
                .Index(t => t.EmailId);
            
            AddColumn("dbo.NotifyConditions", "Version", c => c.Byte(nullable: false));
            AddColumn("dbo.MsgTemplates", "ComyanyId", c => c.Int(nullable: false));
            AddColumn("dbo.MsgTemplates", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.MsgTemplates", "EndDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.MsgTemplates", "Company_Id", c => c.Int());
            RenameColumn("dbo.NotifyConsigns", "EmpId", "EmployeeId");
            RenameColumn("dbo.NotifyConsigns", "PosId", "PositionId");
            CreateIndex("dbo.MsgTemplates", "Company_Id");
            CreateIndex("dbo.NotifyConsigns", "EmployeeId");
            CreateIndex("dbo.NotifyConsigns", "JobId");
            CreateIndex("dbo.NotifyConsigns", "PositionId");
            AddForeignKey("dbo.MsgTemplates", "Company_Id", "dbo.Companies", "Id");
            AddForeignKey("dbo.NotifyConsigns", "EmployeeId", "dbo.People", "Id");
            AddForeignKey("dbo.NotifyConsigns", "JobId", "dbo.Jobs", "Id");
            AddForeignKey("dbo.NotifyConsigns", "PositionId", "dbo.Positions", "Id");
            DropColumn("dbo.MsgTemplates", "Body");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NotifyConsigns", "PosId", c => c.Int());
            AddColumn("dbo.NotifyConsigns", "EmpId", c => c.Int());
            AddColumn("dbo.MsgTemplates", "Body", c => c.String(maxLength: 1000));
            DropForeignKey("dbo.NotifyConsigns", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.NotifyConsigns", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.NotifyConsigns", "EmployeeId", "dbo.People");
            DropForeignKey("dbo.MsgTemplangs", "TemplateId", "dbo.MsgTemplates");
            DropForeignKey("dbo.MsgTemplangs", "EmailId", "dbo.EmailAccounts");
            DropForeignKey("dbo.Languages", "DefaultCurrency_Code", "dbo.Currencies");
            DropForeignKey("dbo.Conditions", "NotifyCondId", "dbo.NotifyConditions");
            DropForeignKey("dbo.MsgTemplates", "Company_Id", "dbo.Companies");
            DropIndex("dbo.NotifyConsigns", new[] { "PositionId" });
            DropIndex("dbo.NotifyConsigns", new[] { "JobId" });
            DropIndex("dbo.NotifyConsigns", new[] { "EmployeeId" });
            DropIndex("dbo.MsgTemplangs", new[] { "EmailId" });
            DropIndex("dbo.MsgTemplangs", new[] { "TemplateId" });
            DropIndex("dbo.Languages", new[] { "DefaultCurrency_Code" });
            DropIndex("dbo.MsgTemplates", new[] { "Company_Id" });
            DropIndex("dbo.Conditions", "IX_Condition");
            DropIndex("dbo.Conditions", new[] { "NotifyCondId" });
            DropColumn("dbo.NotifyConsigns", "PositionId");
            DropColumn("dbo.NotifyConsigns", "EmployeeId");
            DropColumn("dbo.MsgTemplates", "Company_Id");
            DropColumn("dbo.MsgTemplates", "EndDate");
            DropColumn("dbo.MsgTemplates", "StartDate");
            DropColumn("dbo.MsgTemplates", "ComyanyId");
            DropColumn("dbo.NotifyConditions", "Version");
            DropTable("dbo.MsgTemplangs");
            DropTable("dbo.Languages");
            DropTable("dbo.EmailAccounts");
            DropTable("dbo.Conditions");
            RenameIndex(table: "dbo.NotifyConsigns", name: "IX_NotifyCondId", newName: "IX_ConditionId");
            RenameColumn(table: "dbo.NotifyConsigns", name: "NotifyCondId", newName: "ConditionId");
        }
    }
}
