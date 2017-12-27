namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDateTimeToDate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Assignments", "IX_EmpAssignment");
            DropIndex("dbo.Periods", "IX_Period");
            DropIndex("dbo.Employements", "IX_EmployementDate");
            DropIndex("dbo.SubPeriods", "IX_SubPeriod");
            AlterColumn("dbo.Assignments", "AssignDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Assignments", "EndDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.CareerPaths", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.CareerPaths", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.CompanyStructures", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.CompanyStructures", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.People", "MilStatDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Locations", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Locations", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Jobs", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Jobs", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.DocTypes", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.DocTypes", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.PayrollGrades", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.PayrollGrades", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Positions", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Positions", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Benefits", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Benefits", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.BenefitServs", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.BenefitServs", "EndDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Periods", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Periods", "EndDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.PeriodNames", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.PeriodNames", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.CheckLists", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.CheckLists", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Custody", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Custody", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Diagrams", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Diagrams", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Disciplines", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Disciplines", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.DisplinPeriods", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.DisplinPeriods", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.DisPeriodNo", "PeriodSDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.DisPeriodNo", "PeriodEDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.EmpBenefits", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.EmpBenefits", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.EmpChkLists", "ListStartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.EmpChkLists", "ListEndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.EmpCustodies", "RecvDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.EmpDisciplines", "ViolDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.EmpDisciplines", "EffectEDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.EmpDisciplines", "DescionDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Employements", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Employements", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.EmpRelatives", "BirthDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.EmpRelatives", "ExpiryDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Holidays", "HoliDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.JobPayrollGrades", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.JobPayrollGrades", "EndtDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.LeaveAdjusts", "ActionDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.LeaveTypes", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.LeaveTypes", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.MedicalRequests", "IssueDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.MedicalRequests", "ExpiryDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.MedicalRequests", "ServStartDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.MedicalRequests", "ServEndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.PeopleQuals", "StartDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.PeopleQuals", "FinishDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.PeopleQuals", "GainDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.PeopleQuals", "ExpiryDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.PeopleTrain", "CourseSDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.PeopleTrain", "CourseEDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.PeopleTrain", "CantLeave", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.TrainCourses", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.TrainCourses", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.TrainPaths", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.TrainPaths", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.TrainEvents", "StartBookDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.TrainEvents", "EndBookDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.TrainEvents", "TrainStartDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.TrainEvents", "TrainEndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.SubPeriods", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.SubPeriods", "EndDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Terminations", "ServStartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Terminations", "RequestDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Terminations", "PlanedDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Terminations", "ActualDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Terminations", "LastAccDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Terminations", "LastAdjustDate", c => c.DateTime(storeType: "date"));
            CreateIndex("dbo.Assignments", new[] { "EmpId", "AssignDate", "EndDate" }, unique: true, name: "IX_EmpAssignment");
            CreateIndex("dbo.Periods", new[] { "CalendarId", "StartDate", "EndDate" }, unique: true, name: "IX_Period");
            CreateIndex("dbo.Employements", new[] { "EmpId", "StartDate" }, unique: true, name: "IX_EmployementDate");
            CreateIndex("dbo.SubPeriods", new[] { "PeriodId", "StartDate", "EndDate" }, unique: true, name: "IX_SubPeriod");
        }
        
        public override void Down()
        {
            DropIndex("dbo.SubPeriods", "IX_SubPeriod");
            DropIndex("dbo.Employements", "IX_EmployementDate");
            DropIndex("dbo.Periods", "IX_Period");
            DropIndex("dbo.Assignments", "IX_EmpAssignment");
            AlterColumn("dbo.Terminations", "LastAdjustDate", c => c.DateTime());
            AlterColumn("dbo.Terminations", "LastAccDate", c => c.DateTime());
            AlterColumn("dbo.Terminations", "ActualDate", c => c.DateTime());
            AlterColumn("dbo.Terminations", "PlanedDate", c => c.DateTime());
            AlterColumn("dbo.Terminations", "RequestDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Terminations", "ServStartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SubPeriods", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SubPeriods", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TrainEvents", "TrainEndDate", c => c.DateTime());
            AlterColumn("dbo.TrainEvents", "TrainStartDate", c => c.DateTime());
            AlterColumn("dbo.TrainEvents", "EndBookDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TrainEvents", "StartBookDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TrainPaths", "EndDate", c => c.DateTime());
            AlterColumn("dbo.TrainPaths", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TrainCourses", "EndDate", c => c.DateTime());
            AlterColumn("dbo.TrainCourses", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PeopleTrain", "CantLeave", c => c.DateTime());
            AlterColumn("dbo.PeopleTrain", "CourseEDate", c => c.DateTime());
            AlterColumn("dbo.PeopleTrain", "CourseSDate", c => c.DateTime());
            AlterColumn("dbo.PeopleQuals", "ExpiryDate", c => c.DateTime());
            AlterColumn("dbo.PeopleQuals", "GainDate", c => c.DateTime());
            AlterColumn("dbo.PeopleQuals", "FinishDate", c => c.DateTime());
            AlterColumn("dbo.PeopleQuals", "StartDate", c => c.DateTime());
            AlterColumn("dbo.MedicalRequests", "ServEndDate", c => c.DateTime());
            AlterColumn("dbo.MedicalRequests", "ServStartDate", c => c.DateTime());
            AlterColumn("dbo.MedicalRequests", "ExpiryDate", c => c.DateTime());
            AlterColumn("dbo.MedicalRequests", "IssueDate", c => c.DateTime());
            AlterColumn("dbo.LeaveTypes", "EndDate", c => c.DateTime());
            AlterColumn("dbo.LeaveTypes", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveAdjusts", "ActionDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobPayrollGrades", "EndtDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobPayrollGrades", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Holidays", "HoliDate", c => c.DateTime());
            AlterColumn("dbo.EmpRelatives", "ExpiryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmpRelatives", "BirthDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Employements", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Employements", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmpDisciplines", "DescionDate", c => c.DateTime());
            AlterColumn("dbo.EmpDisciplines", "EffectEDate", c => c.DateTime());
            AlterColumn("dbo.EmpDisciplines", "ViolDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmpCustodies", "RecvDate", c => c.DateTime());
            AlterColumn("dbo.EmpChkLists", "ListEndDate", c => c.DateTime());
            AlterColumn("dbo.EmpChkLists", "ListStartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.EmpBenefits", "EndDate", c => c.DateTime());
            AlterColumn("dbo.EmpBenefits", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DisPeriodNo", "PeriodEDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DisPeriodNo", "PeriodSDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DisplinPeriods", "EndDate", c => c.DateTime());
            AlterColumn("dbo.DisplinPeriods", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Disciplines", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Disciplines", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Diagrams", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Diagrams", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Custody", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Custody", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CheckLists", "EndDate", c => c.DateTime());
            AlterColumn("dbo.CheckLists", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PeriodNames", "EndDate", c => c.DateTime());
            AlterColumn("dbo.PeriodNames", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Periods", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Periods", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BenefitServs", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.BenefitServs", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Benefits", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Benefits", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Positions", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Positions", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PayrollGrades", "EndDate", c => c.DateTime());
            AlterColumn("dbo.PayrollGrades", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DocTypes", "EndDate", c => c.DateTime());
            AlterColumn("dbo.DocTypes", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Jobs", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Jobs", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Locations", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Locations", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.People", "MilStatDate", c => c.DateTime());
            AlterColumn("dbo.CompanyStructures", "EndDate", c => c.DateTime());
            AlterColumn("dbo.CompanyStructures", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CareerPaths", "EndDate", c => c.DateTime());
            AlterColumn("dbo.CareerPaths", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Assignments", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Assignments", "AssignDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.SubPeriods", new[] { "PeriodId", "StartDate", "EndDate" }, unique: true, name: "IX_SubPeriod");
            CreateIndex("dbo.Employements", new[] { "EmpId", "StartDate" }, unique: true, name: "IX_EmployementDate");
            CreateIndex("dbo.Periods", new[] { "CalendarId", "StartDate", "EndDate" }, unique: true, name: "IX_Period");
            CreateIndex("dbo.Assignments", new[] { "EmpId", "AssignDate", "EndDate" }, unique: true, name: "IX_EmpAssignment");
        }
    }
}
