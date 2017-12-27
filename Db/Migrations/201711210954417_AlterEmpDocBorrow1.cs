namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmpDocBorrow1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmpDocBorrows", "DocId", "dbo.DocTypes");
            DropIndex("dbo.EmpDocBorrows", new[] { "DocId" });
            CreateTable(
                "dbo.DocBorrowLists",
                c => new
                    {
                        DocBorrowId = c.Int(nullable: false),
                        DocId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DocBorrowId, t.DocId })
                .ForeignKey("dbo.DocTypes", t => t.DocId, cascadeDelete: false)
                .ForeignKey("dbo.EmpDocBorrows", t => t.DocBorrowId, cascadeDelete: true)
                .Index(t => t.DocBorrowId)
                .Index(t => t.DocId);
            
            //AddColumn("dbo.CompanyDocsViews", "thumbs", c => c.Binary());
            AddColumn("dbo.EmpDocBorrows", "Site", c => c.String(maxLength: 200));
            AlterColumn("dbo.EmpDocBorrows", "Purpose", c => c.String(maxLength: 200));
            DropColumn("dbo.CompanyDocuments", "Reference");
            DropColumn("dbo.EmpDocBorrows", "DocId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmpDocBorrows", "DocId", c => c.Int(nullable: false));
            AddColumn("dbo.CompanyDocsViews", "Reference", c => c.Guid());
            DropForeignKey("dbo.DocBorrowLists", "DocBorrowId", "dbo.EmpDocBorrows");
            DropForeignKey("dbo.DocBorrowLists", "DocId", "dbo.DocTypes");
            DropIndex("dbo.DocBorrowLists", new[] { "DocId" });
            DropIndex("dbo.DocBorrowLists", new[] { "DocBorrowId" });
            AlterColumn("dbo.EmpDocBorrows", "Purpose", c => c.String(maxLength: 250));
            DropColumn("dbo.EmpDocBorrows", "Site");
            DropColumn("dbo.CompanyDocsViews", "thumbs");
            DropTable("dbo.DocBorrowLists");
            CreateIndex("dbo.EmpDocBorrows", "DocId");
            AddForeignKey("dbo.EmpDocBorrows", "DocId", "dbo.DocTypes", "Id", cascadeDelete: true);
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
