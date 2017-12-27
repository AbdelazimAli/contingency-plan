namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Messages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        FromEmpId = c.Int(nullable: false),
                        Sent = c.Boolean(nullable: false),
                        Title = c.String(maxLength: 50),
                        Body = c.String(maxLength: 500),
                        All = c.Boolean(nullable: false),
                        Jobs = c.String(maxLength: 50),
                        Depts = c.String(maxLength: 50),
                        PeopleGroups = c.String(maxLength: 50),
                        Employees = c.String(maxLength: 100),
                        CreatedTime = c.DateTime(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .ForeignKey("dbo.People", t => t.FromEmpId, cascadeDelete: false)
                .Index(t => t.CompanyId)
                .Index(t => t.FromEmpId);
            
            CreateTable(
                "dbo.MsgEmployees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromEmpId = c.Int(nullable: false),
                        ToEmpId = c.Int(nullable: false),
                        Title = c.String(maxLength: 50),
                        Body = c.String(maxLength: 500),
                        Read = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.ToEmpId, cascadeDelete: false)
                .Index(t => t.ToEmpId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MsgEmployees", "ToEmpId", "dbo.People");
            DropForeignKey("dbo.Messages", "FromEmpId", "dbo.People");
            DropForeignKey("dbo.Messages", "CompanyId", "dbo.Companies");
            DropIndex("dbo.MsgEmployees", new[] { "ToEmpId" });
            DropIndex("dbo.Messages", new[] { "FromEmpId" });
            DropIndex("dbo.Messages", new[] { "CompanyId" });
            DropTable("dbo.MsgEmployees");
            DropTable("dbo.Messages");
        }
    }
}
