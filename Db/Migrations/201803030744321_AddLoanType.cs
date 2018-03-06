namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoanType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoanTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, name: "IX_LoanType");
            
            AddColumn("dbo.People", "PaperStatus", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropIndex("dbo.LoanTypes", "IX_LoanType");
            DropColumn("dbo.People", "PaperStatus");
            DropTable("dbo.LoanTypes");
        }
    }
}
