namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSendForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SendForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false, storeType: "date"),
                        Employees = c.String(maxLength: 250),
                        Jobs = c.String(maxLength: 250),
                        Departments = c.String(maxLength: 250),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FlexForms", t => t.FormId, cascadeDelete: false)
                .Index(t => t.FormId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SendForms", "FormId", "dbo.FlexForms");
            DropIndex("dbo.SendForms", new[] { "FormId" });
            DropTable("dbo.SendForms");
        }
    }
}
