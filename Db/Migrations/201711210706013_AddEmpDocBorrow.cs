namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmpDocBorrow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmpDocBorrows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        DocId = c.Int(nullable: false),
                        RecvDate = c.DateTime(nullable: false, storeType: "date"),
                        delvryDate = c.DateTime(storeType: "date"),
                        delvryStatus = c.Byte(),
                        Purpose = c.String(maxLength: 250),
                        Notes = c.String(maxLength: 250),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocTypes", t => t.DocId, cascadeDelete: false)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .Index(t => new { t.CompanyId, t.EmpId }, name: "IX_EmpDocBorrow")
                .Index(t => t.DocId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmpDocBorrows", "EmpId", "dbo.People");
            DropForeignKey("dbo.EmpDocBorrows", "DocId", "dbo.DocTypes");
            DropIndex("dbo.EmpDocBorrows", new[] { "DocId" });
            DropIndex("dbo.EmpDocBorrows", "IX_EmpDocBorrow");
            DropTable("dbo.EmpDocBorrows");
        }
    }
}
