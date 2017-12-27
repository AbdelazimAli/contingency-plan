namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQualification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QualGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true, name: "IX_QualGroup");
            
            CreateTable(
                "dbo.Qualifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        QualGroupId = c.Int(),
                        Rank = c.Byte(),
                        Category = c.Byte(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QualGroups", t => t.QualGroupId)
                .Index(t => t.Code, unique: true, name: "IX_Qualification")
                .Index(t => t.QualGroupId);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        SchoolType = c.Byte(),
                        Classification = c.Byte(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Qualifications", "QualGroupId", "dbo.QualGroups");
            DropIndex("dbo.Qualifications", new[] { "QualGroupId" });
            DropIndex("dbo.Qualifications", "IX_Qualification");
            DropIndex("dbo.QualGroups", "IX_QualGroup");
            DropTable("dbo.Schools");
            DropTable("dbo.Qualifications");
            DropTable("dbo.QualGroups");
        }
    }
}
