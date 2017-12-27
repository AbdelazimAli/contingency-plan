namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeByteToShortInCodes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Menus", "IX_MenuTitle");
            DropIndex("dbo.LookUpCode", "IX_LookUpCode");
            DropIndex("dbo.LookUpUserCodes", "IX_LookUpUserCode");
            DropPrimaryKey("dbo.LookUpTitles");
            AlterColumn("dbo.Assignments", "AssignStatus", c => c.Short(nullable: false));
            AlterColumn("dbo.Assignments", "SalaryBasis", c => c.Short());
            AlterColumn("dbo.Companies", "Purpose", c => c.Short());
            AlterColumn("dbo.Companies", "TaxAuthority", c => c.Short());
            AlterColumn("dbo.People", "Gender", c => c.Short(nullable: false));
            AlterColumn("dbo.People", "MaritalStat", c => c.Short());
            AlterColumn("dbo.People", "Religon", c => c.Short());
            AlterColumn("dbo.People", "MedicalStat", c => c.Short());
            AlterColumn("dbo.People", "BloodClass", c => c.Short());
            AlterColumn("dbo.People", "RecmndReson", c => c.Short());
            AlterColumn("dbo.Qualifications", "Rank", c => c.Short());
            AlterColumn("dbo.Qualifications", "Category", c => c.Short(nullable: false));
            AlterColumn("dbo.DocTypes", "DocumenType", c => c.Short(nullable: false));
            AlterColumn("dbo.Positions", "SalaryBasis", c => c.Short());
            AlterColumn("dbo.Benefits", "BenefitClass", c => c.Short(nullable: false));
            AlterColumn("dbo.CareerPathJobs", "Performance", c => c.Short());
            AlterColumn("dbo.ChecklistTasks", "TaskCat", c => c.Short());
            AlterColumn("dbo.ComplainRequests", "RejectReason", c => c.Short());
            AlterColumn("dbo.ComplainRequests", "CancelReason", c => c.Short());
            AlterColumn("dbo.Custody", "CustodyCat", c => c.Short());
            AlterColumn("dbo.Disciplines", "DisciplineClass", c => c.Short(nullable: false));
            AlterColumn("dbo.DisplinRepeats", "DisplinType", c => c.Short(nullable: false));
            AlterColumn("dbo.EmpCustodies", "CustodyStat", c => c.Short());
            AlterColumn("dbo.Employements", "PersonType", c => c.Short(nullable: false));
            AlterColumn("dbo.PeopleQuals", "Grade", c => c.Short());
            AlterColumn("dbo.PeopleQuals", "Awarding", c => c.Short());
            AlterColumn("dbo.Schools", "SchoolType", c => c.Short());
            AlterColumn("dbo.Schools", "Classification", c => c.Short());
            AlterColumn("dbo.EmpRelatives", "Relation", c => c.Short(nullable: false));
            AlterColumn("dbo.EmpTasks", "TaskCat", c => c.Short());
            AlterColumn("dbo.LeaveAdjusts", "Event", c => c.Short(nullable: false));
            AlterColumn("dbo.LeaveTypes", "AbsenceType", c => c.Short(nullable: false));
            AlterColumn("dbo.LeaveTypes", "Gender", c => c.Short());
            AlterColumn("dbo.LeaveTypes", "Religion", c => c.Short());
            AlterColumn("dbo.LeaveTypes", "MaritalStat", c => c.Short());
            AlterColumn("dbo.LeaveTypes", "Nationality", c => c.Int());
            AlterColumn("dbo.LeaveTypes", "MilitaryStat", c => c.Short());
            AlterColumn("dbo.LeaveTypes", "AssignStatus", c => c.Short());
            AlterColumn("dbo.LeaveRequests", "ReqReason", c => c.Short());
            AlterColumn("dbo.LeaveRequests", "RejectReason", c => c.Short());
            AlterColumn("dbo.LeaveRequests", "CancelReason", c => c.Short());
            AlterColumn("dbo.LeaveTrans", "Event", c => c.Short(nullable: false));
            AlterColumn("dbo.LookUpCode", "CodeId", c => c.Short(nullable: false));
            AlterColumn("dbo.LookUpTitles", "CodeId", c => c.Short(nullable: false));
            AlterColumn("dbo.LookUpUserCodes", "CodeId", c => c.Short(nullable: false));
            AlterColumn("dbo.TrainCourses", "CourseCat", c => c.Short());
            AlterColumn("dbo.TrainCourses", "QualRank", c => c.Short());
            AlterColumn("dbo.TrainCourses", "Performance", c => c.Short());
            AlterColumn("dbo.TrainPaths", "QualRank", c => c.Short());
            AlterColumn("dbo.TrainPaths", "Performance", c => c.Short());
            AlterColumn("dbo.Terminations", "TermReason", c => c.Short(nullable: false));
            AlterColumn("dbo.Terminations", "RejectReason", c => c.Short());
            AlterColumn("dbo.Terminations", "CancelReason", c => c.Short());
            AlterColumn("dbo.Terminations", "AssignStatus", c => c.Short(nullable: false));
            AlterColumn("dbo.Terminations", "PersonType", c => c.Short(nullable: false));
          //  AlterColumn("dbo.V_WF_TRANS", "CodeId", c => c.Short());
            AlterColumn("dbo.WfRoles", "CodeId", c => c.Short());
            AlterColumn("dbo.WfTrans", "CodeId", c => c.Short());
            AddPrimaryKey("dbo.LookUpTitles", new[] { "Culture", "CodeName", "CodeId" });
            CreateIndex("dbo.LookUpCode", new[] { "CodeName", "CodeId" }, unique: true, name: "IX_LookUpCode");
            CreateIndex("dbo.LookUpUserCodes", new[] { "CodeName", "CodeId" }, unique: true, name: "IX_LookUpUserCode");
            DropColumn("dbo.Menus", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "Title", c => c.String(nullable: false, maxLength: 50));
            DropIndex("dbo.LookUpUserCodes", "IX_LookUpUserCode");
            DropIndex("dbo.LookUpCode", "IX_LookUpCode");
            DropPrimaryKey("dbo.LookUpTitles");
            AlterColumn("dbo.WfTrans", "CodeId", c => c.Byte());
            AlterColumn("dbo.WfRoles", "CodeId", c => c.Byte());
            //AlterColumn("dbo.V_WF_TRANS", "CodeId", c => c.Byte());
            AlterColumn("dbo.Terminations", "PersonType", c => c.Byte(nullable: false));
            AlterColumn("dbo.Terminations", "AssignStatus", c => c.Byte(nullable: false));
            AlterColumn("dbo.Terminations", "CancelReason", c => c.Byte());
            AlterColumn("dbo.Terminations", "RejectReason", c => c.Byte());
            AlterColumn("dbo.Terminations", "TermReason", c => c.Byte(nullable: false));
            AlterColumn("dbo.TrainPaths", "Performance", c => c.Byte());
            AlterColumn("dbo.TrainPaths", "QualRank", c => c.Byte());
            AlterColumn("dbo.TrainCourses", "Performance", c => c.Byte());
            AlterColumn("dbo.TrainCourses", "QualRank", c => c.Byte());
            AlterColumn("dbo.TrainCourses", "CourseCat", c => c.Byte());
            AlterColumn("dbo.LookUpUserCodes", "CodeId", c => c.Byte(nullable: false));
            AlterColumn("dbo.LookUpTitles", "CodeId", c => c.Byte(nullable: false));
            AlterColumn("dbo.LookUpCode", "CodeId", c => c.Byte(nullable: false));
            AlterColumn("dbo.LeaveTrans", "Event", c => c.Byte(nullable: false));
            AlterColumn("dbo.LeaveRequests", "CancelReason", c => c.Byte());
            AlterColumn("dbo.LeaveRequests", "RejectReason", c => c.Byte());
            AlterColumn("dbo.LeaveRequests", "ReqReason", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "AssignStatus", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "MilitaryStat", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "Nationality", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "MaritalStat", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "Religion", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "Gender", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "AbsenceType", c => c.Byte(nullable: false));
            AlterColumn("dbo.LeaveAdjusts", "Event", c => c.Byte(nullable: false));
            AlterColumn("dbo.EmpTasks", "TaskCat", c => c.Byte());
            AlterColumn("dbo.EmpRelatives", "Relation", c => c.Byte(nullable: false));
            AlterColumn("dbo.Schools", "Classification", c => c.Byte());
            AlterColumn("dbo.Schools", "SchoolType", c => c.Byte());
            AlterColumn("dbo.PeopleQuals", "Awarding", c => c.Byte());
            AlterColumn("dbo.PeopleQuals", "Grade", c => c.Byte());
            AlterColumn("dbo.Employements", "PersonType", c => c.Byte(nullable: false));
            AlterColumn("dbo.EmpCustodies", "CustodyStat", c => c.Byte());
            AlterColumn("dbo.DisplinRepeats", "DisplinType", c => c.Byte(nullable: false));
            AlterColumn("dbo.Disciplines", "DisciplineClass", c => c.Byte(nullable: false));
            AlterColumn("dbo.Custody", "CustodyCat", c => c.Byte());
            AlterColumn("dbo.ComplainRequests", "CancelReason", c => c.Byte());
            AlterColumn("dbo.ComplainRequests", "RejectReason", c => c.Byte());
            AlterColumn("dbo.ChecklistTasks", "TaskCat", c => c.Byte());
            AlterColumn("dbo.CareerPathJobs", "Performance", c => c.Byte());
            AlterColumn("dbo.Benefits", "BenefitClass", c => c.Byte(nullable: false));
            AlterColumn("dbo.Positions", "SalaryBasis", c => c.Byte());
            AlterColumn("dbo.DocTypes", "DocumenType", c => c.Byte(nullable: false));
            AlterColumn("dbo.Qualifications", "Category", c => c.Byte(nullable: false));
            AlterColumn("dbo.Qualifications", "Rank", c => c.Byte());
            AlterColumn("dbo.People", "RecmndReson", c => c.Byte());
            AlterColumn("dbo.People", "BloodClass", c => c.Byte());
            AlterColumn("dbo.People", "MedicalStat", c => c.Byte());
            AlterColumn("dbo.People", "Religon", c => c.Byte());
            AlterColumn("dbo.People", "MaritalStat", c => c.Byte());
            AlterColumn("dbo.People", "Gender", c => c.Byte(nullable: false));
            AlterColumn("dbo.Companies", "TaxAuthority", c => c.Byte());
            AlterColumn("dbo.Companies", "Purpose", c => c.Byte());
            AlterColumn("dbo.Assignments", "SalaryBasis", c => c.Byte());
            AlterColumn("dbo.Assignments", "AssignStatus", c => c.Byte(nullable: false));
            AddPrimaryKey("dbo.LookUpTitles", new[] { "Culture", "CodeName", "CodeId" });
            CreateIndex("dbo.LookUpUserCodes", new[] { "CodeName", "CodeId" }, unique: true, name: "IX_LookUpUserCode");
            CreateIndex("dbo.LookUpCode", new[] { "CodeName", "CodeId" }, unique: true, name: "IX_LookUpCode");
            CreateIndex("dbo.Menus", "Title", name: "IX_MenuTitle");
        }
    }
}
