namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStars : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeptLeavePlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeptId = c.Int(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        MinAllowPercent = c.Single(),
                        Stars = c.Byte(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyStructures", t => t.DeptId, cascadeDelete: true)
                .Index(t => t.DeptId);
            
            AddColumn("dbo.CompanyStructures", "MinAllowPercent", c => c.Single());
            AddColumn("dbo.LeaveRequests", "Stars", c => c.Byte());
            AddColumn("dbo.Personnels", "EmpStars", c => c.Byte(nullable: false));
            AddColumn("dbo.Personnels", "MinAllowPercent", c => c.Single());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeptLeavePlans", "DeptId", "dbo.CompanyStructures");
            DropIndex("dbo.DeptLeavePlans", new[] { "DeptId" });
            DropColumn("dbo.Personnels", "MinAllowPercent");
            DropColumn("dbo.Personnels", "EmpStars");
            DropColumn("dbo.LeaveRequests", "Stars");
            DropColumn("dbo.CompanyStructures", "MinAllowPercent");
            DropTable("dbo.DeptLeavePlans");
        }
    }
}
