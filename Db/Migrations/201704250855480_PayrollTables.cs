namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayrollTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountSetup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccType = c.Byte(nullable: false),
                        Seq = c.Byte(nullable: false),
                        Segment = c.String(maxLength: 20),
                        SegLength = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.AccType, t.Seq }, unique: true, name: "IX_AccountSetup");
            
            CreateTable(
                "dbo.PayrollDues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayrollId = c.Int(nullable: false),
                        Name = c.String(maxLength: 30),
                        DayNo = c.Byte(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payrolls", t => t.PayrollId, cascadeDelete: true)
                .Index(t => t.PayrollId)
                .Index(t => t.Name, unique: true, name: "IX_PayrollDue");
            
            CreateTable(
                "dbo.Formulas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ShortName = c.String(nullable: false, maxLength: 30),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        Basis = c.Byte(nullable: false),
                        Result = c.Byte(nullable: false),
                        Curr = c.String(maxLength: 3),
                        FormType = c.Byte(nullable: false),
                        FormText = c.String(maxLength: 1000),
                        StoredName = c.String(maxLength: 100),
                        RangeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.RangeTables", t => t.RangeId)
                .Index(t => t.CompanyId)
                .Index(t => t.RangeId);
            
            CreateTable(
                "dbo.RangeTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GenTableId = c.Int(nullable: false),
                        TableType = c.Byte(),
                        FormValue = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ToValue = c.Decimal(nullable: false, precision: 18, scale: 3),
                        RangeValue = c.Double(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InfoTables", t => t.GenTableId, cascadeDelete: true)
                .Index(t => t.GenTableId);
            
            CreateTable(
                "dbo.InfoTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        Basis = c.Byte(nullable: false),
                        Purpose = c.Byte(nullable: false),
                        SalItemId = c.Int(),
                        IDepartment = c.Boolean(nullable: false),
                        IPeopleGroup = c.Boolean(nullable: false),
                        IPayrollGrade = c.Boolean(nullable: false),
                        IGrade = c.Boolean(nullable: false),
                        ISubGrade = c.Boolean(nullable: false),
                        IPayroll = c.Boolean(nullable: false),
                        ILocation = c.Boolean(nullable: false),
                        IPersonType = c.Boolean(nullable: false),
                        IPerformance = c.Boolean(nullable: false),
                        CellValue = c.Byte(nullable: false),
                        MinValue = c.Decimal(precision: 18, scale: 3),
                        MaxValue = c.Decimal(precision: 18, scale: 3),
                        FormulaId = c.Int(),
                        FillOptns = c.Byte(),
                        TableType = c.Byte(),
                        Y_N_Formula = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Formulas", t => t.FormulaId)
                .ForeignKey("dbo.SalaryItems", t => t.SalItemId)
                .ForeignKey("dbo.Formulas", t => t.Y_N_Formula)
                .Index(t => t.CompanyId)
                .Index(t => t.SalItemId)
                .Index(t => t.FormulaId)
                .Index(t => t.Y_N_Formula);
            
            CreateTable(
                "dbo.SalaryItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ShortName = c.String(nullable: false, maxLength: 30),
                        IsSalaryItem = c.Boolean(nullable: false),
                        Order = c.Short(),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        PrimaryClass = c.Byte(nullable: false),
                        SecondaryClass = c.Short(nullable: false),
                        UoMeasure = c.Byte(nullable: false),
                        ItemType = c.Byte(nullable: false),
                        Multiple = c.Boolean(nullable: false),
                        Basis = c.Byte(nullable: false),
                        InputCurr = c.String(maxLength: 3),
                        ValueType = c.Byte(nullable: false),
                        TstFormula = c.Int(),
                        MinValue = c.Decimal(precision: 18, scale: 2),
                        MaxValue = c.Decimal(precision: 18, scale: 2),
                        InValidValue = c.Byte(),
                        FormulaId = c.Int(),
                        BatchCurr = c.String(maxLength: 3),
                        DebitGlAccT = c.Int(),
                        CreditGlAccT = c.Int(),
                        Freezed = c.Boolean(nullable: false),
                        Termination = c.Byte(nullable: false),
                        MinAgeInYears = c.Byte(),
                        MinServInYears = c.Byte(),
                        AnnualSettl = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => t.CreditGlAccT)
                .ForeignKey("dbo.Accounts", t => t.DebitGlAccT)
                .ForeignKey("dbo.Formulas", t => t.FormulaId)
                .ForeignKey("dbo.Formulas", t => t.TstFormula)
                .Index(t => t.CompanyId)
                .Index(t => t.TstFormula)
                .Index(t => t.FormulaId)
                .Index(t => t.DebitGlAccT)
                .Index(t => t.CreditGlAccT);
            
            CreateTable(
                "dbo.LinkTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GenTableId = c.Int(nullable: false),
                        ShortName = c.String(maxLength: 30),
                        Basis = c.Byte(nullable: false),
                        SalItemId = c.Int(),
                        DeptId = c.Int(),
                        GroupId = c.Int(),
                        JobId = c.Int(),
                        GradeId = c.Int(),
                        Grade = c.String(maxLength: 20),
                        SubGrade = c.String(maxLength: 20),
                        PayDueId = c.Int(),
                        LocationId = c.Int(),
                        PersonType = c.Short(),
                        Performance = c.Short(),
                        CellValue = c.Decimal(precision: 18, scale: 3),
                        FormulaId = c.Int(),
                        YesNoForm = c.Int(),
                        DebitGlAccT = c.Int(),
                        CreditGlAccT = c.Int(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                        Payroll_Id = c.Int(),
                        PayrollGrade_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.CreditGlAccT)
                .ForeignKey("dbo.Accounts", t => t.DebitGlAccT)
                .ForeignKey("dbo.CompanyStructures", t => t.DeptId)
                .ForeignKey("dbo.Formulas", t => t.FormulaId)
                .ForeignKey("dbo.InfoTables", t => t.GenTableId, cascadeDelete: true)
                .ForeignKey("dbo.PeopleGroups", t => t.GroupId)
                .ForeignKey("dbo.Jobs", t => t.JobId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.PayrollDues", t => t.Payroll_Id)
                .ForeignKey("dbo.PayrollGrades", t => t.PayrollGrade_Id)
                .ForeignKey("dbo.SalaryItems", t => t.SalItemId)
                .Index(t => t.GenTableId)
                .Index(t => t.ShortName, name: "IX_GenTableName")
                .Index(t => t.SalItemId)
                .Index(t => t.DeptId)
                .Index(t => t.GroupId)
                .Index(t => t.JobId)
                .Index(t => t.LocationId)
                .Index(t => t.FormulaId)
                .Index(t => t.DebitGlAccT)
                .Index(t => t.CreditGlAccT)
                .Index(t => t.Payroll_Id)
                .Index(t => t.PayrollGrade_Id);
            
            CreateTable(
                "dbo.PayrollSetup",
                c => new
                    {
                        CompanyId = c.Int(nullable: false),
                        MultiCurr = c.Boolean(nullable: false),
                        DiffDebitAcct = c.Int(),
                        DiffCreditAcct = c.Int(),
                        DebitSettAcct = c.Int(),
                        CreditSettAcct = c.Int(),
                        GradeName = c.Byte(nullable: false),
                        Group = c.String(maxLength: 100),
                        Grade = c.String(maxLength: 100),
                        SubGrade = c.String(maxLength: 100),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyId)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.Accounts", t => t.CreditSettAcct)
                .ForeignKey("dbo.Accounts", t => t.DiffCreditAcct)
                .ForeignKey("dbo.Accounts", t => t.DiffDebitAcct)
                .ForeignKey("dbo.Accounts", t => t.DebitSettAcct)
                .Index(t => t.CompanyId)
                .Index(t => t.DiffDebitAcct)
                .Index(t => t.DiffCreditAcct)
                .Index(t => t.DebitSettAcct)
                .Index(t => t.CreditSettAcct);
            
            AddColumn("dbo.Payrolls", "PayMethod", c => c.Byte(nullable: false));
            AddColumn("dbo.Payrolls", "BankId", c => c.Int());
            AddColumn("dbo.Payrolls", "PeriodId", c => c.Int());
            AddColumn("dbo.SubPeriods", "PayDueId", c => c.Int());
            AddColumn("dbo.SubPeriods", "CalcSalaryDate", c => c.DateTime());
            AddColumn("dbo.SubPeriods", "PaySalaryDate", c => c.DateTime());
            AddColumn("dbo.PayRequestDets", "Stopped", c => c.Boolean(nullable: false));
            RenameColumn("dbo.PayRequests", "PayType", "PayMethod");
            CreateIndex("dbo.Payrolls", "BankId");
            CreateIndex("dbo.Payrolls", "PeriodId");
            CreateIndex("dbo.SubPeriods", "PayDueId");
            CreateIndex("dbo.PayRequests", "FormulaId");
            AddForeignKey("dbo.Payrolls", "BankId", "dbo.Providers", "Id");
            AddForeignKey("dbo.Payrolls", "PeriodId", "dbo.PeriodNames", "Id");
            AddForeignKey("dbo.SubPeriods", "PayDueId", "dbo.PayrollDues", "Id");
            AddForeignKey("dbo.PayRequests", "FormulaId", "dbo.Formulas", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PayRequests", "PayType", c => c.Byte(nullable: false));
            DropForeignKey("dbo.PayrollSetup", "DebitSettAcct", "dbo.Accounts");
            DropForeignKey("dbo.PayrollSetup", "DiffDebitAcct", "dbo.Accounts");
            DropForeignKey("dbo.PayrollSetup", "DiffCreditAcct", "dbo.Accounts");
            DropForeignKey("dbo.PayrollSetup", "CreditSettAcct", "dbo.Accounts");
            DropForeignKey("dbo.PayrollSetup", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.PayRequests", "FormulaId", "dbo.Formulas");
            DropForeignKey("dbo.LinkTables", "SalItemId", "dbo.SalaryItems");
            DropForeignKey("dbo.LinkTables", "PayrollGrade_Id", "dbo.PayrollGrades");
            DropForeignKey("dbo.LinkTables", "Payroll_Id", "dbo.PayrollDues");
            DropForeignKey("dbo.LinkTables", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.LinkTables", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.LinkTables", "GroupId", "dbo.PeopleGroups");
            DropForeignKey("dbo.LinkTables", "GenTableId", "dbo.InfoTables");
            DropForeignKey("dbo.LinkTables", "FormulaId", "dbo.Formulas");
            DropForeignKey("dbo.LinkTables", "DeptId", "dbo.CompanyStructures");
            DropForeignKey("dbo.LinkTables", "DebitGlAccT", "dbo.Accounts");
            DropForeignKey("dbo.LinkTables", "CreditGlAccT", "dbo.Accounts");
            DropForeignKey("dbo.Formulas", "RangeId", "dbo.RangeTables");
            DropForeignKey("dbo.RangeTables", "GenTableId", "dbo.InfoTables");
            DropForeignKey("dbo.InfoTables", "Y_N_Formula", "dbo.Formulas");
            DropForeignKey("dbo.InfoTables", "SalItemId", "dbo.SalaryItems");
            DropForeignKey("dbo.SalaryItems", "TstFormula", "dbo.Formulas");
            DropForeignKey("dbo.SalaryItems", "FormulaId", "dbo.Formulas");
            DropForeignKey("dbo.SalaryItems", "DebitGlAccT", "dbo.Accounts");
            DropForeignKey("dbo.SalaryItems", "CreditGlAccT", "dbo.Accounts");
            DropForeignKey("dbo.SalaryItems", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.InfoTables", "FormulaId", "dbo.Formulas");
            DropForeignKey("dbo.InfoTables", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Formulas", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.SubPeriods", "PayDueId", "dbo.PayrollDues");
            DropForeignKey("dbo.PayrollDues", "PayrollId", "dbo.Payrolls");
            DropForeignKey("dbo.Payrolls", "PeriodId", "dbo.PeriodNames");
            DropForeignKey("dbo.Payrolls", "BankId", "dbo.Providers");
            DropIndex("dbo.PayrollSetup", new[] { "CreditSettAcct" });
            DropIndex("dbo.PayrollSetup", new[] { "DebitSettAcct" });
            DropIndex("dbo.PayrollSetup", new[] { "DiffCreditAcct" });
            DropIndex("dbo.PayrollSetup", new[] { "DiffDebitAcct" });
            DropIndex("dbo.PayrollSetup", new[] { "CompanyId" });
            DropIndex("dbo.PayRequests", new[] { "FormulaId" });
            DropIndex("dbo.LinkTables", new[] { "PayrollGrade_Id" });
            DropIndex("dbo.LinkTables", new[] { "Payroll_Id" });
            DropIndex("dbo.LinkTables", new[] { "CreditGlAccT" });
            DropIndex("dbo.LinkTables", new[] { "DebitGlAccT" });
            DropIndex("dbo.LinkTables", new[] { "FormulaId" });
            DropIndex("dbo.LinkTables", new[] { "LocationId" });
            DropIndex("dbo.LinkTables", new[] { "JobId" });
            DropIndex("dbo.LinkTables", new[] { "GroupId" });
            DropIndex("dbo.LinkTables", new[] { "DeptId" });
            DropIndex("dbo.LinkTables", new[] { "SalItemId" });
            DropIndex("dbo.LinkTables", "IX_GenTableName");
            DropIndex("dbo.LinkTables", new[] { "GenTableId" });
            DropIndex("dbo.SalaryItems", new[] { "CreditGlAccT" });
            DropIndex("dbo.SalaryItems", new[] { "DebitGlAccT" });
            DropIndex("dbo.SalaryItems", new[] { "FormulaId" });
            DropIndex("dbo.SalaryItems", new[] { "TstFormula" });
            DropIndex("dbo.SalaryItems", new[] { "CompanyId" });
            DropIndex("dbo.InfoTables", new[] { "Y_N_Formula" });
            DropIndex("dbo.InfoTables", new[] { "FormulaId" });
            DropIndex("dbo.InfoTables", new[] { "SalItemId" });
            DropIndex("dbo.InfoTables", new[] { "CompanyId" });
            DropIndex("dbo.RangeTables", new[] { "GenTableId" });
            DropIndex("dbo.Formulas", new[] { "RangeId" });
            DropIndex("dbo.Formulas", new[] { "CompanyId" });
            DropIndex("dbo.PayrollDues", "IX_PayrollDue");
            DropIndex("dbo.PayrollDues", new[] { "PayrollId" });
            DropIndex("dbo.SubPeriods", new[] { "PayDueId" });
            DropIndex("dbo.Payrolls", new[] { "PeriodId" });
            DropIndex("dbo.Payrolls", new[] { "BankId" });
            DropIndex("dbo.AccountSetup", "IX_AccountSetup");
            DropColumn("dbo.PayRequests", "PayMethod");
            DropColumn("dbo.PayRequestDets", "Stopped");
            DropColumn("dbo.SubPeriods", "PaySalaryDate");
            DropColumn("dbo.SubPeriods", "CalcSalaryDate");
            DropColumn("dbo.SubPeriods", "PayDueId");
            DropColumn("dbo.Payrolls", "PeriodId");
            DropColumn("dbo.Payrolls", "BankId");
            DropColumn("dbo.Payrolls", "PayMethod");
            DropTable("dbo.PayrollSetup");
            DropTable("dbo.LinkTables");
            DropTable("dbo.SalaryItems");
            DropTable("dbo.InfoTables");
            DropTable("dbo.RangeTables");
            DropTable("dbo.Formulas");
            DropTable("dbo.PayrollDues");
            DropTable("dbo.AccountSetup");
        }
    }
}
