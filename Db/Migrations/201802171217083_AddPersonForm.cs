namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        Question = c.String(maxLength: 250),
                        FormColumnId = c.Int(nullable: false),
                        Answer = c.String(maxLength: 500),
                        OtherText = c.String(maxLength: 500),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.FlexForms", t => t.FormId, cascadeDelete: false)
                .ForeignKey("dbo.FlexFormColumns", t => t.FormColumnId, cascadeDelete: false)
                .Index(t => t.FormId)
                .Index(t => t.EmpId)
                .Index(t => t.FormColumnId);
            
            AlterColumn("dbo.FlexFormColumns", "InputType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonForms", "FormColumnId", "dbo.FlexFormColumns");
            DropForeignKey("dbo.PersonForms", "FormId", "dbo.FlexForms");
            DropForeignKey("dbo.PersonForms", "EmpId", "dbo.People");
            DropIndex("dbo.PersonForms", new[] { "FormColumnId" });
            DropIndex("dbo.PersonForms", new[] { "EmpId" });
            DropIndex("dbo.PersonForms", new[] { "FormId" });
            AlterColumn("dbo.FlexFormColumns", "InputType", c => c.Byte(nullable: false));
            DropTable("dbo.PersonForms");
        }
    }
}
