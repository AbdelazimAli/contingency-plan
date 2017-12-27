namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddjobClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobClasses",
                c => new
                    {
                        JobClassCode = c.String(nullable: false, maxLength: 20),
                        IsLocal = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Notes = c.String(maxLength: 250),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobClassCode);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.JobClasses");
        }
    }
}
