namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyTablesCreate1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CareerPathJobs", new[] { "CareerId" });
            DropIndex("dbo.CareerPathJobs", new[] { "JobId" });
            RenameColumn(table: "dbo.CompanyDocAttrs", name: "TypeId", newName: "AttributeId");
            RenameIndex(table: "dbo.CompanyDocAttrs", name: "IX_TypeId", newName: "IX_AttributeId");
            DropPrimaryKey("dbo.CareerPathJobs");
            CreateTable(
                "dbo.BudgetItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BudgetId = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .Index(t => new { t.BudgetId, t.Name }, unique: true, name: "IX_BudgetItem");
            
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Source = c.Byte(),
                        PeriodId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .Index(t => t.Name, unique: true, name: "IX_Budget")
                .Index(t => t.PeriodId);
            
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeriodNo = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        StartDate = c.String(),
                        EndDate = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.PeriodNo, unique: true, name: "IX_Period");
            
            CreateTable(
                "dbo.Payrolls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_Payroll");
            
            CreateTable(
                "dbo.PeopleGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(nullable: false),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true, name: "IX_PeopleGroup");
            
            CreateTable(
                "dbo.PositionBudgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PositionId = c.Int(nullable: false),
                        SubPeriodId = c.Int(nullable: false),
                        BudgetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Positions", t => t.PositionId, cascadeDelete: true)
                .ForeignKey("dbo.SubPeriods", t => t.SubPeriodId, cascadeDelete: true)
                .Index(t => t.PositionId)
                .Index(t => t.SubPeriodId);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        Reference = c.Guid(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        PositionType = c.Byte(nullable: false),
                        Seasonal = c.Boolean(nullable: false),
                        SeasonDay = c.Byte(),
                        SeasonMonth = c.Byte(),
                        JobId = c.Int(nullable: false),
                        DeptId = c.Int(nullable: false),
                        HiringStatus = c.Byte(nullable: false),
                        Supervisor = c.Int(),
                        Relief = c.Int(),
                        Successor = c.Int(),
                        BudgetId = c.Int(),
                        BudgetItemId = c.Int(),
                        BudgetAmount = c.Decimal(precision: 18, scale: 2),
                        ProbationPeriod = c.Byte(),
                        OverlapPeriod = c.Short(),
                        PayrollId = c.Int(),
                        SalaryBasis = c.Byte(),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyStructures", t => t.DeptId, cascadeDelete: true)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .ForeignKey("dbo.Payrolls", t => t.PayrollId)
                .Index(t => t.Code, unique: true, name: "IX_PositionCode")
                .Index(t => t.JobId)
                .Index(t => t.DeptId)
                .Index(t => t.PayrollId);
            
            CreateTable(
                "dbo.SubPeriods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeriodId = c.Int(nullable: false),
                        SubPeriodNo = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        StartDate = c.String(),
                        EndDate = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .Index(t => new { t.PeriodId, t.SubPeriodNo }, unique: true, name: "IX_SubPeriod");
            
            AddColumn("dbo.CareerPathJobs", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.CareerPathJobs", "Id");
            CreateIndex("dbo.CareerPathJobs", new[] { "CareerId", "JobId" }, unique: true, name: "IX_CareerPathJobs");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PositionBudgets", "SubPeriodId", "dbo.SubPeriods");
            DropForeignKey("dbo.SubPeriods", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.PositionBudgets", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.Positions", "PayrollId", "dbo.Payrolls");
            DropForeignKey("dbo.Positions", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.Positions", "DeptId", "dbo.CompanyStructures");
            DropForeignKey("dbo.BudgetItems", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.Budgets", "PeriodId", "dbo.Periods");
            DropIndex("dbo.SubPeriods", "IX_SubPeriod");
            DropIndex("dbo.Positions", new[] { "PayrollId" });
            DropIndex("dbo.Positions", new[] { "DeptId" });
            DropIndex("dbo.Positions", new[] { "JobId" });
            DropIndex("dbo.Positions", "IX_PositionCode");
            DropIndex("dbo.PositionBudgets", new[] { "SubPeriodId" });
            DropIndex("dbo.PositionBudgets", new[] { "PositionId" });
            DropIndex("dbo.PeopleGroups", "IX_PeopleGroup");
            DropIndex("dbo.Payrolls", "IX_Payroll");
            DropIndex("dbo.CareerPathJobs", "IX_CareerPathJobs");
            DropIndex("dbo.Periods", "IX_Period");
            DropIndex("dbo.Budgets", new[] { "PeriodId" });
            DropIndex("dbo.Budgets", "IX_Budget");
            DropIndex("dbo.BudgetItems", "IX_BudgetItem");
            DropPrimaryKey("dbo.CareerPathJobs");
            DropColumn("dbo.CareerPathJobs", "Id");
            DropTable("dbo.SubPeriods");
            DropTable("dbo.Positions");
            DropTable("dbo.PositionBudgets");
            DropTable("dbo.PeopleGroups");
            DropTable("dbo.Payrolls");
            DropTable("dbo.Periods");
            DropTable("dbo.Budgets");
            DropTable("dbo.BudgetItems");
            AddPrimaryKey("dbo.CareerPathJobs", new[] { "CareerId", "JobId" });
            RenameIndex(table: "dbo.CompanyDocAttrs", name: "IX_AttributeId", newName: "IX_TypeId");
            RenameColumn(table: "dbo.CompanyDocAttrs", name: "AttributeId", newName: "TypeId");
            CreateIndex("dbo.CareerPathJobs", "JobId");
            CreateIndex("dbo.CareerPathJobs", "CareerId");
        }
    }
}
