namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 250),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsLocal = c.Boolean(nullable: false),
                        Default = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        ListType = c.Byte(nullable: false),
                        Duration = c.Short(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.ChecklistTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ListId = c.Int(nullable: false),
                        TaskNo = c.Short(nullable: false),
                        TaskCat = c.Byte(),
                        Description = c.String(maxLength: 250),
                        Priority = c.Byte(nullable: false),
                        ExpectTime = c.Short(),
                        Unit = c.Byte(),
                        EmpId = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CheckLists", t => t.ListId, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.EmpId)
                .Index(t => new { t.ListId, t.TaskNo }, unique: true, name: "IX_ChecklistTask")
                .Index(t => t.EmpId);
            
            CreateTable(
                "dbo.EmpChkLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 250),
                        EmpId = c.Int(),
                        ListStartDate = c.DateTime(nullable: false),
                        ListEndDate = c.DateTime(),
                        ListType = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId)
                .Index(t => t.EmpId);
            
            CreateTable(
                "dbo.EmpTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpListId = c.Int(),
                        TaskNo = c.Short(),
                        TaskCat = c.Byte(),
                        Description = c.String(maxLength: 250),
                        Priority = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                        Required = c.Boolean(nullable: false),
                        ExpectTime = c.Short(),
                        Unit = c.Byte(),
                        EmpId = c.Int(),
                        ManagerId = c.Int(),
                        AssignedTime = c.DateTime(),
                        StartTime = c.DateTime(),
                        DoneTime = c.DateTime(),
                        Duration = c.Short(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmpChkLists", t => t.EmpListId)
                .ForeignKey("dbo.People", t => t.EmpId)
                .ForeignKey("dbo.People", t => t.ManagerId)
                .Index(t => t.EmpListId)
                .Index(t => t.EmpId)
                .Index(t => t.ManagerId);
            
            AddColumn("dbo.Menus", "Title", c => c.String(maxLength: 50));
            AddColumn("dbo.Personnels", "AutoEmployment", c => c.Boolean(nullable: false));
            AddColumn("dbo.Personnels", "AutoTermiation", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmpTasks", "ManagerId", "dbo.People");
            DropForeignKey("dbo.EmpTasks", "EmpId", "dbo.People");
            DropForeignKey("dbo.EmpTasks", "EmpListId", "dbo.EmpChkLists");
            DropForeignKey("dbo.EmpChkLists", "EmpId", "dbo.People");
            DropForeignKey("dbo.ChecklistTasks", "EmpId", "dbo.People");
            DropForeignKey("dbo.ChecklistTasks", "ListId", "dbo.CheckLists");
            DropForeignKey("dbo.CheckLists", "CompanyId", "dbo.Companies");
            DropIndex("dbo.EmpTasks", new[] { "ManagerId" });
            DropIndex("dbo.EmpTasks", new[] { "EmpId" });
            DropIndex("dbo.EmpTasks", new[] { "EmpListId" });
            DropIndex("dbo.EmpChkLists", new[] { "EmpId" });
            DropIndex("dbo.ChecklistTasks", new[] { "EmpId" });
            DropIndex("dbo.ChecklistTasks", "IX_ChecklistTask");
            DropIndex("dbo.CheckLists", new[] { "CompanyId" });
            DropColumn("dbo.Personnels", "AutoTermiation");
            DropColumn("dbo.Personnels", "AutoEmployment");
            DropColumn("dbo.Menus", "Title");
            DropTable("dbo.EmpTasks");
            DropTable("dbo.EmpChkLists");
            DropTable("dbo.ChecklistTasks");
            DropTable("dbo.CheckLists");
        }
    }
}
