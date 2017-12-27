namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyTablesCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CareerPaths",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        Reference = c.Guid(),
                        Description = c.String(maxLength: 500),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CareerPathJobs",
                c => new
                    {
                        CareerId = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                        MinYears = c.Byte(),
                        Performance = c.Byte(),
                        FormulaId = c.Int(),
                    })
                .PrimaryKey(t => new { t.CareerId, t.JobId })
                .ForeignKey("dbo.CareerPaths", t => t.CareerId, cascadeDelete: true)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .Index(t => t.CareerId)
                .Index(t => t.JobId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        Reference = c.Guid(),
                        NameInInsurance = c.String(maxLength: 100),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        DefaultGradeId = c.Int(),
                        PlanCount = c.Short(),
                        PlanTurnOverRate = c.Single(),
                        PrimaryRole = c.Boolean(nullable: false),
                        ContractEndNotifyDays = c.Short(),
                        ProbationPeriod = c.Short(),
                        DescInRecruitment = c.String(),
                        WorkHours = c.Short(),
                        Frequency = c.Byte(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        ReplacementRequired = c.Boolean(nullable: false),
                        NotifyIfAbsent = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                        CustInt1 = c.Int(),
                        CustInt2 = c.Int(),
                        CustDecimal3 = c.Decimal(precision: 18, scale: 2),
                        CustDecimal4 = c.Decimal(precision: 18, scale: 2),
                        CustDate1 = c.DateTime(),
                        CustDate2 = c.DateTime(),
                        CustDate3 = c.DateTime(),
                        CustDate4 = c.DateTime(),
                        CustText1 = c.String(maxLength: 250),
                        CustText2 = c.String(maxLength: 250),
                        CustText3 = c.String(maxLength: 250),
                        CustText4 = c.String(maxLength: 250),
                        CustText5 = c.String(maxLength: 250),
                        CustText6 = c.String(maxLength: 250),
                        CustText7 = c.String(maxLength: 250),
                        CustText8 = c.String(maxLength: 250),
                        PayrollGrade_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.PayrollGrades", t => t.PayrollGrade_Id)
                .Index(t => t.Code, unique: true, name: "IX_JobCode")
                .Index(t => t.CompanyId)
                .Index(t => t.PayrollGrade_Id);
            
            CreateTable(
                "dbo.PayrollGrades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 30),
                        Name = c.String(nullable: false, maxLength: 50),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        StartPoint = c.Int(),
                        EndPoint = c.Int(),
                        Group = c.Short(),
                        Grade = c.Short(),
                        SubGrade = c.Short(),
                        Step = c.Short(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        HasExpiryDate = c.Boolean(nullable: false),
                        NotifyNofDays = c.Byte(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_DocType");
            
            CreateTable(
                "dbo.DocTypeAttrs",
                c => new
                    {
                        TypeId = c.Int(nullable: false),
                        Attribute = c.String(nullable: false, maxLength: 100),
                        InputType = c.Byte(nullable: false),
                        CodeName = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => new { t.TypeId, t.Attribute })
                .ForeignKey("dbo.DocTypes", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.JobPayrollGrades",
                c => new
                    {
                        JobId = c.Int(nullable: false),
                        PayrollGradeId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndtDate = c.DateTime(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.JobId, t.PayrollGradeId })
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .ForeignKey("dbo.PayrollGrades", t => t.PayrollGradeId, cascadeDelete: true)
                .Index(t => t.JobId)
                .Index(t => t.PayrollGradeId);
            
            AddColumn("dbo.Companies", "Reference", c => c.Guid());
            AddColumn("dbo.CompanyDocuments", "Reference", c => c.Guid());
            AddColumn("dbo.CompanyDocuments", "TypeId", c => c.Int());
            AddColumn("dbo.CompanyStructures", "Reference", c => c.Guid());
            AddColumn("dbo.FormColumns", "CodeName", c => c.String(maxLength: 20));
            AddColumn("dbo.GridColumns", "CodeName", c => c.String(maxLength: 20));
            AddColumn("dbo.JobClasses", "Job_Id", c => c.Int());
            AddColumn("dbo.Personnels", "Frequency", c => c.Byte());
            CreateIndex("dbo.JobClasses", "Job_Id");
            CreateIndex("dbo.CompanyDocuments", "TypeId");
            AddForeignKey("dbo.JobClasses", "Job_Id", "dbo.Jobs", "Id");
            AddForeignKey("dbo.CompanyDocuments", "TypeId", "dbo.DocTypes", "Id");
            DropColumn("dbo.CompanyDocuments", "DocType");
            DropColumn("dbo.Personnels", "Rate");  
        }
        
        public override void Down()
        {
            AddColumn("dbo.Personnels", "Rate", c => c.Byte());
            AddColumn("dbo.CompanyDocsViews", "DocType", c => c.Byte());
            DropForeignKey("dbo.JobPayrollGrades", "PayrollGradeId", "dbo.PayrollGrades");
            DropForeignKey("dbo.JobPayrollGrades", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.DocTypeAttrs", "TypeId", "dbo.DocTypes");
            DropForeignKey("dbo.CompanyDocsViews", "TypeId", "dbo.DocTypes");
            DropForeignKey("dbo.CareerPathJobs", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "PayrollGrade_Id", "dbo.PayrollGrades");
            DropForeignKey("dbo.JobClasses", "Job_Id", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CareerPathJobs", "CareerId", "dbo.CareerPaths");
            DropIndex("dbo.JobPayrollGrades", new[] { "PayrollGradeId" });
            DropIndex("dbo.JobPayrollGrades", new[] { "JobId" });
            DropIndex("dbo.DocTypeAttrs", new[] { "TypeId" });
            DropIndex("dbo.DocTypes", "IX_DocType");
            DropIndex("dbo.CompanyDocsViews", new[] { "TypeId" });
            DropIndex("dbo.JobClasses", new[] { "Job_Id" });
            DropIndex("dbo.Jobs", new[] { "PayrollGrade_Id" });
            DropIndex("dbo.Jobs", new[] { "CompanyId" });
            DropIndex("dbo.Jobs", "IX_JobCode");
            DropIndex("dbo.CareerPathJobs", new[] { "JobId" });
            DropIndex("dbo.CareerPathJobs", new[] { "CareerId" });
            DropColumn("dbo.Personnels", "Frequency");
            DropColumn("dbo.JobClasses", "Job_Id");
            DropColumn("dbo.GridColumns", "CodeName");
            DropColumn("dbo.FormColumns", "CodeName");
            DropColumn("dbo.CompanyStructures", "Reference");
            DropColumn("dbo.CompanyDocsViews", "TypeId");
            DropColumn("dbo.CompanyDocsViews", "Reference");
            DropColumn("dbo.Companies", "Reference");
            DropTable("dbo.JobPayrollGrades");
            DropTable("dbo.DocTypeAttrs");
            DropTable("dbo.DocTypes");
            DropTable("dbo.PayrollGrades");
            DropTable("dbo.Jobs");
            DropTable("dbo.CareerPathJobs");
            DropTable("dbo.CareerPaths");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
