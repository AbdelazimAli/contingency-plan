namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPagePrint : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PagePrints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        Version = c.Byte(nullable: false),
                        ObjectName = c.String(nullable: false, maxLength: 30, unicode: false),
                        LetterTempl = c.String(maxLength: 100),
                        ModifiedUser = c.String(maxLength: 20),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .Index(t => t.CompanyId);
            
            AddColumn("dbo.LeaveTypes", "ExLeavePlan", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PagePrints", "CompanyId", "dbo.Companies");
            DropIndex("dbo.PagePrints", new[] { "CompanyId" });
            DropColumn("dbo.LeaveTypes", "ExLeavePlan");
            DropTable("dbo.PagePrints");
        }
    }
}
