namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSalVar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Formulas", "RangeId", "dbo.RangeTables");
            DropIndex("dbo.Formulas", new[] { "RangeId" });
            CreateTable(
                "dbo.SalaryVars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayrollId = c.Int(nullable: false),
                        PayPeriodId = c.Int(nullable: false),
                        Status = c.Byte(nullable: false),
                        SalItemId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 3),
                        Curr = c.String(maxLength: 3, fixedLength: true, unicode: false),
                        Approvedby = c.String(maxLength: 20),
                        Reference = c.Guid(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.Curr)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.SubPeriods", t => t.PayPeriodId, cascadeDelete: false)
                .ForeignKey("dbo.Payrolls", t => t.PayrollId, cascadeDelete: false)
                .ForeignKey("dbo.SalaryItems", t => t.SalItemId, cascadeDelete: false)
                .Index(t => t.PayrollId)
                .Index(t => new { t.PayPeriodId, t.SalItemId, t.EmpId }, name: "SalaryVarPayEmp")
                .Index(t => t.Curr)
                .Index(t => t.Reference, name: "SalaryVarRef");
            
            RenameColumn("dbo.Formulas", "RangeId", "InfoId");
            AddColumn("dbo.PayrollSetup", "AutoApprovSalVal", c => c.Boolean(nullable: false));
            AddForeignKey("dbo.Formulas", "InfoId", "dbo.InfoTables", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Formulas", "RangeId", c => c.Int());
            DropForeignKey("dbo.SalaryVars", "SalItemId", "dbo.SalaryItems");
            DropForeignKey("dbo.SalaryVars", "PayrollId", "dbo.Payrolls");
            DropForeignKey("dbo.SalaryVars", "PayPeriodId", "dbo.SubPeriods");
            DropForeignKey("dbo.SalaryVars", "EmpId", "dbo.People");
            DropForeignKey("dbo.SalaryVars", "Curr", "dbo.Currencies");
            DropForeignKey("dbo.Formulas", "InfoId", "dbo.InfoTables");
            DropIndex("dbo.SalaryVars", "SalaryVarRef");
            DropIndex("dbo.SalaryVars", new[] { "Curr" });
            DropIndex("dbo.SalaryVars", "SalaryVarPayEmp");
            DropIndex("dbo.SalaryVars", new[] { "PayrollId" });
            DropIndex("dbo.Formulas", new[] { "InfoId" });
            DropColumn("dbo.PayrollSetup", "AutoApprovSalVal");
            DropColumn("dbo.Formulas", "InfoId");
            DropTable("dbo.SalaryVars");
            CreateIndex("dbo.Formulas", "RangeId");
            AddForeignKey("dbo.Formulas", "RangeId", "dbo.RangeTables", "Id");
        }
    }
}
