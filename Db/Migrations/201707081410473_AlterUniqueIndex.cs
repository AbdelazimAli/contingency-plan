namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUniqueIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CareerPaths", "IX_CareerPathCode");
            DropIndex("dbo.Jobs", "IX_JobCode");
            DropIndex("dbo.Positions", "IX_PositionCode");
            DropIndex("dbo.Disciplines", "IX_Discipline");
            DropIndex("dbo.LeaveTypes", "IX_LeaveType");
            CreateIndex("dbo.CareerPaths", "Code", name: "IX_CareerPathCode");
            CreateIndex("dbo.CareerPaths", "Name", name: "IX_CareerPathName");
            CreateIndex("dbo.CompanyStructures", "Code", name: "IX_StructureCode");
            CreateIndex("dbo.CompanyStructures", "Name", name: "IX_StructureName");
            CreateIndex("dbo.Jobs", "Code", name: "IX_JobCode");
            CreateIndex("dbo.Jobs", "Name", name: "IX_JobName");
            CreateIndex("dbo.Positions", "Code", name: "IX_PositionCode");
            CreateIndex("dbo.Positions", "Name", name: "IX_PositionName");
            CreateIndex("dbo.Custody", "Code", name: "IX_CustodyCode");
            CreateIndex("dbo.Custody", "Name", name: "IX_CustodyName");
            CreateIndex("dbo.Disciplines", "Code", name: "IX_DisciplineCode");
            CreateIndex("dbo.Disciplines", "Name", name: "IX_DisciplineName");
            CreateIndex("dbo.LeaveTypes", "Code", name: "IX_LeaveTypeCode");
            CreateIndex("dbo.LeaveTypes", "Name", name: "IX_LeaveTypeName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LeaveTypes", "IX_LeaveTypeName");
            DropIndex("dbo.LeaveTypes", "IX_LeaveTypeCode");
            DropIndex("dbo.Disciplines", "IX_DisciplineName");
            DropIndex("dbo.Disciplines", "IX_DisciplineCode");
            DropIndex("dbo.Custody", "IX_CustodyName");
            DropIndex("dbo.Custody", "IX_CustodyCode");
            DropIndex("dbo.Positions", "IX_PositionName");
            DropIndex("dbo.Positions", "IX_PositionCode");
            DropIndex("dbo.Jobs", "IX_JobName");
            DropIndex("dbo.Jobs", "IX_JobCode");
            DropIndex("dbo.CompanyStructures", "IX_StructureName");
            DropIndex("dbo.CompanyStructures", "IX_StructureCode");
            DropIndex("dbo.CareerPaths", "IX_CareerPathName");
            DropIndex("dbo.CareerPaths", "IX_CareerPathCode");
            CreateIndex("dbo.LeaveTypes", "Name", unique: true, name: "IX_LeaveType");
            CreateIndex("dbo.Disciplines", "Code", unique: true, name: "IX_Discipline");
            CreateIndex("dbo.Positions", "Code", unique: true, name: "IX_PositionCode");
            CreateIndex("dbo.Jobs", "Code", unique: true, name: "IX_JobCode");
            CreateIndex("dbo.CareerPaths", "Code", unique: true, name: "IX_CareerPathCode");
        }
    }
}
