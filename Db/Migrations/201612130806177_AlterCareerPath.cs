namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCareerPath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CareerPaths", "IsLocal", c => c.Boolean(nullable: false));
            AddColumn("dbo.CareerPaths", "CompanyId", c => c.Int());
            AddColumn("dbo.CareerPathJobs", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.CareerPathJobs", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.CareerPathJobs", "CreatedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.CareerPathJobs", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.CompanyStructures", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.CompanyStructures", "EndDate", c => c.DateTime());
            AlterColumn("dbo.CareerPaths", "Code", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.CareerPaths", "Code", unique: true, name: "IX_CareerPathCode");
            CreateIndex("dbo.CareerPaths", "CompanyId");
            AddForeignKey("dbo.CareerPaths", "CompanyId", "dbo.Companies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CareerPaths", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CareerPaths", new[] { "CompanyId" });
            DropIndex("dbo.CareerPaths", "IX_CareerPathCode");
            AlterColumn("dbo.CareerPaths", "Code", c => c.String(maxLength: 20));
            DropColumn("dbo.CompanyStructures", "EndDate");
            DropColumn("dbo.CompanyStructures", "StartDate");
            DropColumn("dbo.CareerPathJobs", "ModifiedTime");
            DropColumn("dbo.CareerPathJobs", "CreatedTime");
            DropColumn("dbo.CareerPathJobs", "ModifiedUser");
            DropColumn("dbo.CareerPathJobs", "CreatedUser");
            DropColumn("dbo.CareerPaths", "CompanyId");
            DropColumn("dbo.CareerPaths", "IsLocal");
        }
    }
}
