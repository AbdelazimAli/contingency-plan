namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MsgTemplates", "Company_Id", "dbo.Companies");
            DropIndex("dbo.MsgTemplates", new[] { "Company_Id" });
            RenameColumn(table: "dbo.MsgTemplates", name: "Company_Id", newName: "CompanyId");
            AddColumn("dbo.Notifications", "CompanyId", c => c.Int(nullable: false));
            AlterColumn("dbo.MsgTemplates", "CompanyId", c => c.Int(nullable: false));
            CreateIndex("dbo.MsgTemplates", "CompanyId");
            CreateIndex("dbo.Notifications", new[] { "CompanyId", "Read" }, name: "IX_Notification");
            AddForeignKey("dbo.MsgTemplates", "CompanyId", "dbo.Companies", "Id", cascadeDelete: false);
            DropColumn("dbo.MsgTemplates", "ComyanyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MsgTemplates", "ComyanyId", c => c.Int(nullable: false));
            DropForeignKey("dbo.MsgTemplates", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Notifications", "IX_Notification");
            DropIndex("dbo.MsgTemplates", new[] { "CompanyId" });
            AlterColumn("dbo.MsgTemplates", "CompanyId", c => c.Int());
            DropColumn("dbo.Notifications", "CompanyId");
            RenameColumn(table: "dbo.MsgTemplates", name: "CompanyId", newName: "Company_Id");
            CreateIndex("dbo.MsgTemplates", "Company_Id");
            AddForeignKey("dbo.MsgTemplates", "Company_Id", "dbo.Companies", "Id");
        }
    }
}
