namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeQualification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PeopleQuals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmpId = c.Int(nullable: false),
                        QualId = c.Int(nullable: false),
                        Title = c.String(maxLength: 100),
                        Establishment = c.String(maxLength: 100),
                        StartDate = c.DateTime(),
                        FinishDate = c.DateTime(),
                        Status = c.Byte(nullable: false),
                        GradYear = c.Short(),
                        Grade = c.Byte(),
                        Score = c.Decimal(precision: 18, scale: 2),
                        Notes = c.String(maxLength: 250),
                        SerialNo = c.String(maxLength: 20),
                        GainDate = c.DateTime(),
                        ExpiryDate = c.DateTime(),
                        Cost = c.Decimal(precision: 18, scale: 2),
                        Awarding = c.Byte(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                        Employee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Employee_Id)
                .ForeignKey("dbo.Qualifications", t => t.QualId, cascadeDelete: true)
                .Index(t => t.QualId)
                .Index(t => t.Employee_Id);
            
            AddColumn("dbo.Employements", "Code", c => c.String(maxLength: 15));
            AddColumn("dbo.LeaveRequests", "RejectReason", c => c.Byte());
            AddColumn("dbo.LeaveRequests", "RejectDesc", c => c.String(maxLength: 250));
            AlterColumn("dbo.Periods", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Periods", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveRequests", "ActualStartDate", c => c.DateTime());
            AlterColumn("dbo.LeaveRequests", "ActualEndDate", c => c.DateTime());
            AlterColumn("dbo.LeaveRequests", "ReturnDate", c => c.DateTime());
            AlterColumn("dbo.SubPeriods", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SubPeriods", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PeopleQuals", "QualId", "dbo.Qualifications");
            DropForeignKey("dbo.PeopleQuals", "Employee_Id", "dbo.People");
            DropIndex("dbo.PeopleQuals", new[] { "Employee_Id" });
            DropIndex("dbo.PeopleQuals", new[] { "QualId" });
            AlterColumn("dbo.SubPeriods", "EndDate", c => c.String());
            AlterColumn("dbo.SubPeriods", "StartDate", c => c.String());
            AlterColumn("dbo.LeaveRequests", "ReturnDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveRequests", "ActualEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.LeaveRequests", "ActualStartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Periods", "EndDate", c => c.String());
            AlterColumn("dbo.Periods", "StartDate", c => c.String());
            DropColumn("dbo.LeaveRequests", "RejectDesc");
            DropColumn("dbo.LeaveRequests", "RejectReason");
            DropColumn("dbo.Employements", "Code");
            DropTable("dbo.PeopleQuals");
        }
    }
}
