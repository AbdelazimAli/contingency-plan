namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MedicalRequest : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PeopleTrain", "IX_PeopleTrainCompany");
            CreateTable(
                "dbo.BenefitServs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        BenefitId = c.Int(nullable: false),
                        Code = c.String(maxLength: 20),
                        Name = c.String(maxLength: 200),
                        IsGroup = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 3),
                        Curr = c.String(maxLength: 3, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Benefits", t => t.BenefitId, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.Curr)
                .ForeignKey("dbo.BenefitServs", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.BenefitId)
                .Index(t => t.Curr);
            
            CreateTable(
                "dbo.MedicalRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        ServiceId = c.Int(nullable: false),
                        ProviderId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        BendefId = c.Int(),
                        Description = c.String(maxLength: 500),
                        ApprovalStatus = c.Byte(nullable: false),
                        RejectReason = c.Short(),
                        RejectDesc = c.String(maxLength: 250),
                        CancelReason = c.Short(),
                        CancelDesc = c.String(maxLength: 250),
                        WFlowId = c.Int(),
                        ServCost = c.Decimal(nullable: false, precision: 18, scale: 3),
                        Curr = c.String(maxLength: 3, fixedLength: true, unicode: false),
                        CurrRate = c.Single(),
                        EmpCost = c.Decimal(nullable: false, precision: 18, scale: 3),
                        CompanyCost = c.Decimal(nullable: false, precision: 18, scale: 3),
                        IssueDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        ServStartDate = c.DateTime(nullable: false),
                        ServEndDate = c.DateTime(nullable: false),
                        PeriodId = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmpRelatives", t => t.BendefId)
                .ForeignKey("dbo.Currencies", t => t.Curr)
                .ForeignKey("dbo.Periods", t => t.PeriodId)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.Providers", t => t.ProviderId, cascadeDelete: false)
                .ForeignKey("dbo.BenefitServs", t => t.ServiceId, cascadeDelete: false)
                .Index(t => new { t.CompanyId, t.ApprovalStatus }, name: "IX_MedicalReqStatus")
                .Index(t => t.ServiceId)
                .Index(t => t.ProviderId)
                .Index(t => t.EmpId)
                .Index(t => t.BendefId)
                .Index(t => t.Curr)
                .Index(t => t.PeriodId);
            
            CreateTable(
                "dbo.TrainEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        CourseId = c.Int(nullable: false),
                        StartBookDate = c.DateTime(nullable: false),
                        EndBookDate = c.DateTime(nullable: false),
                        Internal = c.Boolean(nullable: false),
                        MaxBookCount = c.Short(),
                        MinBookCount = c.Short(),
                        AllowCandidate = c.Boolean(nullable: false),
                        AllowEmpBook = c.Boolean(nullable: false),
                        TrainStartDate = c.DateTime(),
                        TrainEndDate = c.DateTime(),
                        PeriodId = c.Int(),
                        CenterId = c.Int(),
                        Cost = c.Decimal(precision: 18, scale: 3),
                        Adwarding = c.Byte(),
                        Curr = c.String(maxLength: 3, fixedLength: true, unicode: false),
                        CurrRate = c.Single(),
                        CostFlag = c.Byte(nullable: false),
                        ResponsbleId = c.Int(),
                        Description = c.String(maxLength: 500),
                        Notes = c.String(maxLength: 500),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Providers", t => t.CenterId)
                .ForeignKey("dbo.TrainCourses", t => t.CourseId, cascadeDelete: false)
                .ForeignKey("dbo.Currencies", t => t.Curr)
                .ForeignKey("dbo.Periods", t => t.PeriodId)
                .ForeignKey("dbo.People", t => t.ResponsbleId)
                .Index(t => t.CourseId)
                .Index(t => t.PeriodId)
                .Index(t => t.CenterId)
                .Index(t => t.Curr)
                .Index(t => t.ResponsbleId);
            
            AddColumn("dbo.PeriodNames", "Default", c => c.Boolean(nullable: false));
            AddColumn("dbo.PeopleTrain", "EventId", c => c.Int());
            AddColumn("dbo.PeopleTrain", "RequestDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PeopleTrain", "ApprovalStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.PeopleTrain", "Curr", c => c.String(maxLength: 3, fixedLength: true, unicode: false));
            AddColumn("dbo.PeopleTrain", "CurrRate", c => c.Single());
            AddColumn("dbo.PeopleTrain", "WFlowId", c => c.Int());
            AddColumn("dbo.PeopleTrain", "RejectReason", c => c.Short());
            AddColumn("dbo.PeopleTrain", "RejectDesc", c => c.String(maxLength: 250));
            AddColumn("dbo.PeopleTrain", "CancelReason", c => c.Short());
            AddColumn("dbo.PeopleTrain", "CancelDesc", c => c.String(maxLength: 250));
            CreateIndex("dbo.PeopleTrain", new[] { "EventId", "PersonId" }, name: "IX_PeopleEvent");
            CreateIndex("dbo.PeopleTrain", new[] { "CompanyId", "ApprovalStatus" }, name: "IX_TrainingReqStatus");
            CreateIndex("dbo.PeopleTrain", "Curr");
            AddForeignKey("dbo.PeopleTrain", "Curr", "dbo.Currencies", "Code");
            AddForeignKey("dbo.PeopleTrain", "EventId", "dbo.TrainEvents", "Id");
            DropColumn("dbo.EmpBenefits", "CompPercent");
            DropColumn("dbo.EmpBenefits", "CompAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmpBenefits", "CompAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.EmpBenefits", "CompPercent", c => c.Single());
            DropForeignKey("dbo.PeopleTrain", "EventId", "dbo.TrainEvents");
            DropForeignKey("dbo.TrainEvents", "ResponsbleId", "dbo.People");
            DropForeignKey("dbo.TrainEvents", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.TrainEvents", "Curr", "dbo.Currencies");
            DropForeignKey("dbo.TrainEvents", "CourseId", "dbo.TrainCourses");
            DropForeignKey("dbo.TrainEvents", "CenterId", "dbo.Providers");
            DropForeignKey("dbo.PeopleTrain", "Curr", "dbo.Currencies");
            DropForeignKey("dbo.MedicalRequests", "ServiceId", "dbo.BenefitServs");
            DropForeignKey("dbo.MedicalRequests", "ProviderId", "dbo.Providers");
            DropForeignKey("dbo.MedicalRequests", "EmpId", "dbo.People");
            DropForeignKey("dbo.MedicalRequests", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.MedicalRequests", "Curr", "dbo.Currencies");
            DropForeignKey("dbo.MedicalRequests", "BendefId", "dbo.EmpRelatives");
            DropForeignKey("dbo.BenefitServs", "ParentId", "dbo.BenefitServs");
            DropForeignKey("dbo.BenefitServs", "Curr", "dbo.Currencies");
            DropForeignKey("dbo.BenefitServs", "BenefitId", "dbo.Benefits");
            DropIndex("dbo.TrainEvents", new[] { "ResponsbleId" });
            DropIndex("dbo.TrainEvents", new[] { "Curr" });
            DropIndex("dbo.TrainEvents", new[] { "CenterId" });
            DropIndex("dbo.TrainEvents", new[] { "PeriodId" });
            DropIndex("dbo.TrainEvents", new[] { "CourseId" });
            DropIndex("dbo.PeopleTrain", new[] { "Curr" });
            DropIndex("dbo.PeopleTrain", "IX_TrainingReqStatus");
            DropIndex("dbo.PeopleTrain", "IX_PeopleEvent");
            DropIndex("dbo.MedicalRequests", new[] { "PeriodId" });
            DropIndex("dbo.MedicalRequests", new[] { "Curr" });
            DropIndex("dbo.MedicalRequests", new[] { "BendefId" });
            DropIndex("dbo.MedicalRequests", new[] { "EmpId" });
            DropIndex("dbo.MedicalRequests", new[] { "ProviderId" });
            DropIndex("dbo.MedicalRequests", new[] { "ServiceId" });
            DropIndex("dbo.MedicalRequests", "IX_MedicalReqStatus");
            DropIndex("dbo.BenefitServs", new[] { "Curr" });
            DropIndex("dbo.BenefitServs", new[] { "BenefitId" });
            DropIndex("dbo.BenefitServs", new[] { "ParentId" });
            DropColumn("dbo.PeopleTrain", "CancelDesc");
            DropColumn("dbo.PeopleTrain", "CancelReason");
            DropColumn("dbo.PeopleTrain", "RejectDesc");
            DropColumn("dbo.PeopleTrain", "RejectReason");
            DropColumn("dbo.PeopleTrain", "WFlowId");
            DropColumn("dbo.PeopleTrain", "CurrRate");
            DropColumn("dbo.PeopleTrain", "Curr");
            DropColumn("dbo.PeopleTrain", "ApprovalStatus");
            DropColumn("dbo.PeopleTrain", "RequestDate");
            DropColumn("dbo.PeopleTrain", "EventId");
            DropColumn("dbo.PeriodNames", "Default");
            DropTable("dbo.TrainEvents");
            DropTable("dbo.MedicalRequests");
            DropTable("dbo.BenefitServs");
            CreateIndex("dbo.PeopleTrain", "CompanyId", name: "IX_PeopleTrainCompany");
        }
    }
}
