namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterJobClassCompanyStr : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.JobClasses");
            AddColumn("dbo.CompanyStructures", "StructureType", c => c.Byte(nullable: false));
            AddColumn("dbo.CompanyStructures", "IsVisible", c => c.Boolean(nullable: false));
            AddColumn("dbo.CompanyStructures", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.JobClasses", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.JobClasses", "CompanyId", c => c.Int());
            AddPrimaryKey("dbo.JobClasses", "Id");
            CreateIndex("dbo.CompanyStructures", "CompanyId");
            CreateIndex("dbo.JobClasses", "JobClassCode", unique: true);
            CreateIndex("dbo.JobClasses", "CompanyId");
            AddForeignKey("dbo.CompanyStructures", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.JobClasses", "CompanyId", "dbo.Companies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobClasses", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyStructures", "CompanyId", "dbo.Companies");
            DropIndex("dbo.JobClasses", new[] { "CompanyId" });
            DropIndex("dbo.JobClasses", new[] { "JobClassCode" });
            DropIndex("dbo.CompanyStructures", new[] { "CompanyId" });
            DropPrimaryKey("dbo.JobClasses");
            DropColumn("dbo.JobClasses", "CompanyId");
            DropColumn("dbo.JobClasses", "Id");
            DropColumn("dbo.CompanyStructures", "CompanyId");
            DropColumn("dbo.CompanyStructures", "IsVisible");
            DropColumn("dbo.CompanyStructures", "StructureType");
            AddPrimaryKey("dbo.JobClasses", "JobClassCode");
        }
    }
}
