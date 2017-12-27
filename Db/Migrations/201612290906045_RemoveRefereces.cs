namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRefereces : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PeopleQuals", "Employee_Id", "dbo.People");
            DropIndex("dbo.PeopleQuals", new[] { "Employee_Id" });
            DropIndex("dbo.FlexData", "IX_FlexData");
            DropColumn("dbo.PeopleQuals", "EmpId");
            RenameColumn(table: "dbo.PeopleQuals", name: "Employee_Id", newName: "EmpId");
            AddColumn("dbo.PeopleQuals", "QualCatSysCd", c => c.Byte(nullable: false));
            AddColumn("dbo.FlexData", "Source", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.FlexData", "SourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.Qualifications", "Category", c => c.Byte(nullable: false));
            AlterColumn("dbo.PeopleQuals", "EmpId", c => c.Int(nullable: false));
            CreateIndex("dbo.PeopleQuals", "EmpId");
            CreateIndex("dbo.FlexData", new[] { "PageId", "Source", "SourceId", "ColumnName" }, unique: true, name: "IX_FlexData");
            AddForeignKey("dbo.PeopleQuals", "EmpId", "dbo.People", "Id", cascadeDelete: true);
            DropColumn("dbo.CareerPaths", "Reference");
            DropColumn("dbo.Companies", "Reference");
            DropColumn("dbo.Jobs", "Reference");
            DropColumn("dbo.CompanyStructures", "Reference");
            DropColumn("dbo.People", "Reference");
            DropColumn("dbo.FlexData", "Reference");
            DropColumn("dbo.Positions", "Reference");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Positions", "Reference", c => c.Guid());
            AddColumn("dbo.FlexData", "Reference", c => c.Guid(nullable: false));
            AddColumn("dbo.People", "Reference", c => c.Guid());
            AddColumn("dbo.CompanyStructures", "Reference", c => c.Guid());
            AddColumn("dbo.Jobs", "Reference", c => c.Guid());
            AddColumn("dbo.Companies", "Reference", c => c.Guid());
            AddColumn("dbo.CareerPaths", "Reference", c => c.Guid());
            DropForeignKey("dbo.PeopleQuals", "EmpId", "dbo.People");
            DropIndex("dbo.FlexData", "IX_FlexData");
            DropIndex("dbo.PeopleQuals", new[] { "EmpId" });
            AlterColumn("dbo.PeopleQuals", "EmpId", c => c.Int());
            AlterColumn("dbo.Qualifications", "Category", c => c.Byte());
            DropColumn("dbo.FlexData", "SourceId");
            DropColumn("dbo.FlexData", "Source");
            DropColumn("dbo.PeopleQuals", "QualCatSysCd");
            RenameColumn(table: "dbo.PeopleQuals", name: "EmpId", newName: "Employee_Id");
            AddColumn("dbo.PeopleQuals", "EmpId", c => c.Int(nullable: false));
            CreateIndex("dbo.FlexData", new[] { "PageId", "ColumnName", "Reference" }, unique: true, name: "IX_FlexData");
            CreateIndex("dbo.PeopleQuals", "Employee_Id");
            AddForeignKey("dbo.PeopleQuals", "Employee_Id", "dbo.People", "Id");
        }
    }
}
