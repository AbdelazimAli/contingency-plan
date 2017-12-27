namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpId = c.Int(nullable: false),
                        PersonType = c.Byte(nullable: false),
                        Status = c.Byte(nullable: false),
                        TerminationId = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: true)
                .Index(t => t.EmpId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employements", "EmpId", "dbo.People");
            DropIndex("dbo.Employements", new[] { "EmpId" });
            DropTable("dbo.Employements");
        }
    }
}
