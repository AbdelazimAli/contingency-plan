namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPersonForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SendFormPeoples",
                c => new
                    {
                        SendFormId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SendFormId, t.EmpId })
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.SendForms", t => t.SendFormId, cascadeDelete: true)
                .Index(t => t.SendFormId)
                .Index(t => t.EmpId);
            
            AddColumn("dbo.PersonForms", "SendFormId", c => c.Int(nullable: false));
            CreateIndex("dbo.PersonForms", "SendFormId");
            AddForeignKey("dbo.PersonForms", "SendFormId", "dbo.SendForms", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SendFormPeoples", "SendFormId", "dbo.SendForms");
            DropForeignKey("dbo.SendFormPeoples", "EmpId", "dbo.People");
            DropForeignKey("dbo.PersonForms", "SendFormId", "dbo.SendForms");
            DropIndex("dbo.SendFormPeoples", new[] { "EmpId" });
            DropIndex("dbo.SendFormPeoples", new[] { "SendFormId" });
            DropIndex("dbo.PersonForms", new[] { "SendFormId" });
            DropColumn("dbo.PersonForms", "SendFormId");
            DropTable("dbo.SendFormPeoples");
        }
    }
}
