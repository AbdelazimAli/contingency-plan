namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLeaveRange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeaveRanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeaveTypeId = c.Int(nullable: false),
                        FromYear = c.Short(nullable: false),
                        ToYear = c.Short(),
                        NofDays = c.Byte(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LeaveTypes", t => t.LeaveTypeId, cascadeDelete: true)
                .Index(t => t.LeaveTypeId);
            
            AddColumn("dbo.Companies", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Companies", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Companies", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.Companies", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.CompanyBranches", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.CompanyBranches", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.CompanyBranches", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.CompanyBranches", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.CompanyPartners", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.CompanyPartners", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.CompanyPartners", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.CompanyPartners", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.Menus", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Menus", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Menus", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.Menus", "ModifiedTime", c => c.DateTime());
            AlterColumn("dbo.LeaveTypes", "ExWorkService", c => c.Boolean());
            AlterColumn("dbo.LeaveTypes", "AccBalDays", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeaveRanges", "LeaveTypeId", "dbo.LeaveTypes");
            DropIndex("dbo.LeaveRanges", new[] { "LeaveTypeId" });
            AlterColumn("dbo.LeaveTypes", "AccBalDays", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "ExWorkService", c => c.Byte());
            DropColumn("dbo.Menus", "ModifiedTime");
            DropColumn("dbo.Menus", "CreatedTime");
            DropColumn("dbo.Menus", "ModifiedUser");
            DropColumn("dbo.Menus", "CreatedUser");
            DropColumn("dbo.CompanyPartners", "ModifiedTime");
            DropColumn("dbo.CompanyPartners", "CreatedTime");
            DropColumn("dbo.CompanyPartners", "ModifiedUser");
            DropColumn("dbo.CompanyPartners", "CreatedUser");
            DropColumn("dbo.CompanyBranches", "ModifiedTime");
            DropColumn("dbo.CompanyBranches", "CreatedTime");
            DropColumn("dbo.CompanyBranches", "ModifiedUser");
            DropColumn("dbo.CompanyBranches", "CreatedUser");
            DropColumn("dbo.Companies", "ModifiedTime");
            DropColumn("dbo.Companies", "CreatedTime");
            DropColumn("dbo.Companies", "ModifiedUser");
            DropColumn("dbo.Companies", "CreatedUser");
            DropTable("dbo.LeaveRanges");
        }
    }
}
