namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlexForm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlexFormColumns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FlexFSId = c.Int(nullable: false),
                        Name = c.String(maxLength: 250),
                        ColumnOrder = c.Byte(nullable: false),
                        InputType = c.Byte(nullable: false),
                        ShowTextBox = c.Boolean(nullable: false),
                        Selections = c.String(maxLength: 1000),
                        ShowHint = c.Boolean(nullable: false),
                        Hint = c.String(maxLength: 100),
                        Answer = c.String(maxLength: 100),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FlexFormFS", t => t.FlexFSId, cascadeDelete: true)
                .Index(t => t.FlexFSId);
            
            CreateTable(
                "dbo.FlexFormFS",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FlexformId = c.Int(nullable: false),
                        Description = c.String(maxLength: 500),
                        FSOrder = c.Byte(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FlexForms", t => t.FlexformId, cascadeDelete: true)
                .Index(t => t.FlexformId);
            
            CreateTable(
                "dbo.FlexForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Purpose = c.String(maxLength: 250),
                        DesignedBy = c.Int(nullable: false),
                        FormType = c.Byte(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.DesignedBy, cascadeDelete: true)
                .Index(t => t.DesignedBy);
            
            AddColumn("dbo.EmpDocBorrows", "ExpdelvryDate", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlexFormColumns", "FlexFSId", "dbo.FlexFormFS");
            DropForeignKey("dbo.FlexFormFS", "FlexformId", "dbo.FlexForms");
            DropForeignKey("dbo.FlexForms", "DesignedBy", "dbo.People");
            DropIndex("dbo.FlexForms", new[] { "DesignedBy" });
            DropIndex("dbo.FlexFormFS", new[] { "FlexformId" });
            DropIndex("dbo.FlexFormColumns", new[] { "FlexFSId" });
            DropColumn("dbo.EmpDocBorrows", "ExpdelvryDate");
            DropTable("dbo.FlexForms");
            DropTable("dbo.FlexFormFS");
            DropTable("dbo.FlexFormColumns");
        }
    }
}
