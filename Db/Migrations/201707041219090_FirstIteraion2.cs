namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstIteraion2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BenefitRequests", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.BudgetItemAmounts", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.BudgetItemAmounts", "BudgetItemId", "dbo.BudgetItems");
            DropIndex("dbo.BenefitRequests", new[] { "PeriodId" });
            DropIndex("dbo.BudgetItemAmounts", "IX_BudgetItemAmount");
            DropIndex("dbo.DeptBudgets", new[] { "DeptId" });
            DropIndex("dbo.DeptBudgets", new[] { "SubPeriodId" });
            DropIndex("dbo.PosBudgets", new[] { "PositionId" });
            DropIndex("dbo.PosBudgets", new[] { "SubPeriodId" });
            RenameColumn(table: "dbo.PersonSetup", name: "TaskCalendarId", newName: "BudgetPeriodId");
            RenameIndex(table: "dbo.PersonSetup", name: "IX_TaskCalendarId", newName: "IX_BudgetPeriodId");
            CreateTable(
                "dbo.CompanyBudgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                        SubPeriodId = c.Int(nullable: false),
                        BudgetItemId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetItems", t => t.BudgetItemId, cascadeDelete: false)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: false)
                .ForeignKey("dbo.SubPeriods", t => t.SubPeriodId, cascadeDelete: false)
                .Index(t => new { t.CompanyId, t.PeriodId, t.SubPeriodId, t.BudgetItemId }, unique: true, name: "IX_CompanyBudget");
            
            RenameColumn("dbo.BenefitRequests", "PeriodId", "SubPeriodId");
            AddColumn("dbo.DeptBudgets", "PeriodId", c => c.Int(nullable: false));
            AddColumn("dbo.DeptBudgets", "BudgetItemId", c => c.Int(nullable: false));
            AddColumn("dbo.PersonSetup", "TaskPeriodId", c => c.Int());
            AddColumn("dbo.PosBudgets", "PeriodId", c => c.Int(nullable: false));
            AddColumn("dbo.PosBudgets", "BudgetItemId", c => c.Int(nullable: false));
            AlterColumn("dbo.Custody", "AccountCode", c => c.String(maxLength: 100));
            CreateIndex("dbo.BenefitRequests", "SubPeriodId");
            CreateIndex("dbo.DeptBudgets", new[] { "DeptId", "PeriodId", "SubPeriodId", "BudgetItemId" }, unique: true, name: "IX_DeptBudget");
            CreateIndex("dbo.PersonSetup", "TaskPeriodId");
            CreateIndex("dbo.PosBudgets", new[] { "PositionId", "PeriodId", "SubPeriodId", "BudgetItemId" }, unique: true, name: "IX_PosBudget");
            AddForeignKey("dbo.BenefitRequests", "SubPeriodId", "dbo.SubPeriods", "Id");
            AddForeignKey("dbo.DeptBudgets", "BudgetItemId", "dbo.BudgetItems", "Id", cascadeDelete: false);
            AddForeignKey("dbo.DeptBudgets", "PeriodId", "dbo.Periods", "Id", cascadeDelete: false);
            AddForeignKey("dbo.PersonSetup", "TaskPeriodId", "dbo.PeriodNames", "Id");
            AddForeignKey("dbo.PosBudgets", "BudgetItemId", "dbo.BudgetItems", "Id", cascadeDelete: false);
            AddForeignKey("dbo.PosBudgets", "PeriodId", "dbo.Periods", "Id", cascadeDelete: false);
            DropTable("dbo.BudgetItemAmounts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BudgetItemAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BudgetId = c.Int(nullable: false),
                        BudgetItemId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BenefitRequests", "PeriodId", c => c.Int());
            DropForeignKey("dbo.PosBudgets", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.PosBudgets", "BudgetItemId", "dbo.BudgetItems");
            DropForeignKey("dbo.PersonSetup", "TaskPeriodId", "dbo.PeriodNames");
            DropForeignKey("dbo.DeptBudgets", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.DeptBudgets", "BudgetItemId", "dbo.BudgetItems");
            DropForeignKey("dbo.CompanyBudgets", "SubPeriodId", "dbo.SubPeriods");
            DropForeignKey("dbo.CompanyBudgets", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.CompanyBudgets", "BudgetItemId", "dbo.BudgetItems");
            DropForeignKey("dbo.BenefitRequests", "SubPeriodId", "dbo.SubPeriods");
            DropIndex("dbo.PosBudgets", "IX_PosBudget");
            DropIndex("dbo.PersonSetup", new[] { "TaskPeriodId" });
            DropIndex("dbo.DeptBudgets", "IX_DeptBudget");
            DropIndex("dbo.CompanyBudgets", "IX_CompanyBudget");
            DropIndex("dbo.BenefitRequests", new[] { "SubPeriodId" });
            AlterColumn("dbo.Custody", "AccountCode", c => c.String(maxLength: 30));
            DropColumn("dbo.PosBudgets", "BudgetItemId");
            DropColumn("dbo.PosBudgets", "PeriodId");
            DropColumn("dbo.PersonSetup", "TaskPeriodId");
            DropColumn("dbo.DeptBudgets", "BudgetItemId");
            DropColumn("dbo.DeptBudgets", "PeriodId");
            DropColumn("dbo.BenefitRequests", "SubPeriodId");
            DropTable("dbo.CompanyBudgets");
            RenameIndex(table: "dbo.PersonSetup", name: "IX_BudgetPeriodId", newName: "IX_TaskCalendarId");
            RenameColumn(table: "dbo.PersonSetup", name: "BudgetPeriodId", newName: "TaskCalendarId");
            CreateIndex("dbo.PosBudgets", "SubPeriodId");
            CreateIndex("dbo.PosBudgets", "PositionId");
            CreateIndex("dbo.DeptBudgets", "SubPeriodId");
            CreateIndex("dbo.DeptBudgets", "DeptId");
            CreateIndex("dbo.BudgetItemAmounts", new[] { "BudgetId", "BudgetItemId" }, unique: true, name: "IX_BudgetItemAmount");
            CreateIndex("dbo.BenefitRequests", "PeriodId");
            AddForeignKey("dbo.BudgetItemAmounts", "BudgetItemId", "dbo.BudgetItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BudgetItemAmounts", "BudgetId", "dbo.Budgets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BenefitRequests", "PeriodId", "dbo.Periods", "Id");
        }
    }
}
