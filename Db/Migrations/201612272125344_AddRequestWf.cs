namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestWf : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestWf",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.String(nullable: false, maxLength: 10),
                        SourceId = c.Int(nullable: false),
                        Order = c.Byte(nullable: false),
                        RoleId = c.Guid(),
                        CodeId = c.Byte(),
                        WaitDays = c.Byte(),
                        WaitAction = c.Byte(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Source, t.SourceId, t.Order }, unique: true, name: "IX_RequestWf");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RequestWf", "IX_RequestWf");
            DropTable("dbo.RequestWf");
        }
    }
}
