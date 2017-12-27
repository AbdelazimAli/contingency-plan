namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCodes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Jobs", "IX_JobCode");
            DropIndex("dbo.CareerPaths", "IX_CareerPathCode");
            DropIndex("dbo.CompanyStructures", "IX_StructureCode");
            DropIndex("dbo.Qualifications", "IX_Qualification");
            DropIndex("dbo.QualGroups", "IX_QualGroup");
            DropIndex("dbo.Payrolls", "IX_PayrollCode");
            DropIndex("dbo.PeopleGroups", "IX_PeopleGroup");
            DropIndex("dbo.Positions", "IX_PositionCode");
            DropIndex("dbo.LeaveTypes", "IX_LeaveTypeCode");
            DropIndex("dbo.Benefits", "IX_Benefit");
            DropIndex("dbo.Disciplines", "IX_DisciplineCode");
            DropIndex("dbo.TrainCourses", "IX_TrainCourse");
            Sql("alter table dbo.companies drop constraint DF__Companies__Code__6B79F03D");
            Sql("update dbo.Companies set Code = '0'");
            AlterColumn("dbo.Companies", "Code", c => c.Int(nullable: false));
            AlterColumn("dbo.Jobs", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.CareerPaths", "Code", c => c.String(maxLength: 20));
            Sql("update dbo.CompanyStructures set Code = '0'");
            AlterColumn("dbo.CompanyStructures", "Code", c => c.Int(nullable: false));
            Sql("update dbo.Kafeel set Code = '0'");
            AlterColumn("dbo.Kafeel", "Code", c => c.Int(nullable: false));
            Sql("update dbo.Locations set Code = '0'");
            AlterColumn("dbo.Locations", "Code", c => c.Int(nullable: false));
            Sql("update dbo.Providers set Code = '0'");
            AlterColumn("dbo.Providers", "Code", c => c.Int(nullable: false));
            Sql("update dbo.Qualifications set Code = '0'");
            AlterColumn("dbo.Qualifications", "Code", c => c.Int(nullable: false));
            Sql("update dbo.QualGroups set Code = '0'");
            AlterColumn("dbo.QualGroups", "Code", c => c.Int(nullable: false));
            Sql("alter table dbo.Payrolls drop constraint DF__Payrolls__Code__4B02FF0A");
            Sql("update dbo.Payrolls set Code = '0'");
            AlterColumn("dbo.Payrolls", "Code", c => c.Int(nullable: false));
            Sql("update dbo.PeopleGroups set Code = '0'");
            AlterColumn("dbo.PeopleGroups", "Code", c => c.Int(nullable: false));
            Sql("update dbo.Positions set Code = '0'");
            AlterColumn("dbo.Positions", "Code", c => c.Int(nullable: false));
            Sql("update dbo.LeaveTypes set Code = '0'");
            AlterColumn("dbo.LeaveTypes", "Code", c => c.Int(nullable: false));
            Sql("update dbo.Benefits set Code = '0'");
            AlterColumn("dbo.Benefits", "Code", c => c.Int(nullable: false));
            Sql("update dbo.BudgetItems set Code = '0'");
            AlterColumn("dbo.BudgetItems", "Code", c => c.Int(nullable: false));
            Sql("update dbo.BenefitServs set Code = '0'");
            AlterColumn("dbo.BenefitServs", "Code", c => c.Int(nullable: false));
            Sql("update dbo.Disciplines set Code = '0'");
            AlterColumn("dbo.Disciplines", "Code", c => c.Int(nullable: false));
            Sql("update dbo.TrainCourses set Code = '0'");
            AlterColumn("dbo.TrainCourses", "Code", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "Code", name: "IX_JobCode");
            CreateIndex("dbo.CareerPaths", "Code", name: "IX_CareerPathCode");
            CreateIndex("dbo.CompanyStructures", "Code", name: "IX_StructureCode");
            CreateIndex("dbo.Qualifications", "Code", name: "IX_Qualification");
            CreateIndex("dbo.QualGroups", "Code", name: "IX_QualGroup");
            CreateIndex("dbo.Payrolls", "Code", name: "IX_PayrollCode");
            CreateIndex("dbo.PeopleGroups", "Code", name: "IX_PeopleGroup");
            CreateIndex("dbo.Positions", "Code", name: "IX_PositionCode");
            CreateIndex("dbo.LeaveTypes", "Code", name: "IX_LeaveTypeCode");
            CreateIndex("dbo.Benefits", "Code", name: "IX_Benefit");
            CreateIndex("dbo.Disciplines", "Code", name: "IX_DisciplineCode");
            CreateIndex("dbo.TrainCourses", "Code", name: "IX_TrainCourse");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TrainCourses", "IX_TrainCourse");
            DropIndex("dbo.Disciplines", "IX_DisciplineCode");
            DropIndex("dbo.Benefits", "IX_Benefit");
            DropIndex("dbo.LeaveTypes", "IX_LeaveTypeCode");
            DropIndex("dbo.Positions", "IX_PositionCode");
            DropIndex("dbo.PeopleGroups", "IX_PeopleGroup");
            DropIndex("dbo.Payrolls", "IX_PayrollCode");
            DropIndex("dbo.QualGroups", "IX_QualGroup");
            DropIndex("dbo.Qualifications", "IX_Qualification");
            DropIndex("dbo.CompanyStructures", "IX_StructureCode");
            DropIndex("dbo.CareerPaths", "IX_CareerPathCode");
            DropIndex("dbo.Jobs", "IX_JobCode");
            AlterColumn("dbo.TrainCourses", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.Disciplines", "Code", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.BenefitServs", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.BudgetItems", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.Benefits", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.LeaveTypes", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.Positions", "Code", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.PeopleGroups", "Code", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Payrolls", "Code", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.QualGroups", "Code", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Qualifications", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.Providers", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.Locations", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.Kafeel", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.CompanyStructures", "Code", c => c.String(maxLength: 20));
            AlterColumn("dbo.CareerPaths", "Code", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Jobs", "Code", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Companies", "Code", c => c.String(maxLength: 20));
            CreateIndex("dbo.TrainCourses", "Code", unique: true, name: "IX_TrainCourse");
            CreateIndex("dbo.Disciplines", "Code", name: "IX_DisciplineCode");
            CreateIndex("dbo.Benefits", "Code", unique: true, name: "IX_Benefit");
            CreateIndex("dbo.LeaveTypes", "Code", unique: true, name: "IX_LeaveTypeCode");
            CreateIndex("dbo.Positions", "Code", name: "IX_PositionCode");
            CreateIndex("dbo.PeopleGroups", "Code", unique: true, name: "IX_PeopleGroup");
            CreateIndex("dbo.Payrolls", "Code", unique: true, name: "IX_PayrollCode");
            CreateIndex("dbo.QualGroups", "Code", unique: true, name: "IX_QualGroup");
            CreateIndex("dbo.Qualifications", "Code", name: "IX_Qualification");
            CreateIndex("dbo.CompanyStructures", "Code", name: "IX_StructureCode");
            CreateIndex("dbo.CareerPaths", "Code", name: "IX_CareerPathCode");
            CreateIndex("dbo.Jobs", "Code", name: "IX_JobCode");
        }
    }
}
