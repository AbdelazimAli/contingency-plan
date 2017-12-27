namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComplainRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComplainRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        ComplainType = c.Byte(nullable: false),
                        EmpId = c.Int(nullable: false),
                        ApprovalStatus = c.Byte(nullable: false),
                        Description = c.String(maxLength: 500),
                        Against = c.Byte(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RejectReason = c.Byte(),
                        RejectDesc = c.String(maxLength: 250),
                        CancelReason = c.Byte(),
                        CancelDesc = c.String(maxLength: 250),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .Index(t => new { t.ComplainType, t.EmpId, t.ApprovalStatus }, name: "IX_ComplainRequest");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComplainRequests", "EmpId", "dbo.People");
            DropIndex("dbo.ComplainRequests", "IX_ComplainRequest");
            DropTable("dbo.ComplainRequests");
        }
    }
}
