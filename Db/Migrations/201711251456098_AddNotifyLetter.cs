namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifyLetter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotifyLetters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        NotifyDate = c.DateTime(nullable: false),
                        EmpId = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                        NotifySource = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .Index(t => t.EmpId)
                .Index(t => new { t.SourceId, t.NotifySource }, name: "IX_NotifyLetter");

            Sql("update dbo.Employements set DurInYears = 0 where DurInYears is null");
            AlterColumn("dbo.Employements", "DurInYears", c => c.Byte(nullable: false));
            Sql("update dbo.Employements set DurInMonths = 0 where DurInMonths is null");
            AlterColumn("dbo.Employements", "DurInMonths", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotifyLetters", "EmpId", "dbo.People");
            DropIndex("dbo.NotifyLetters", "IX_NotifyLetter");
            DropIndex("dbo.NotifyLetters", new[] { "EmpId" });
            AlterColumn("dbo.Employements", "DurInMonths", c => c.Byte());
            AlterColumn("dbo.Employements", "DurInYears", c => c.Byte());
            DropTable("dbo.NotifyLetters");
        }
    }
}
