namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstIteration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PositionBudgets", newName: "PosBudgets");
            DropForeignKey("dbo.BudgetItems", "BudgetId", "dbo.Budgets");
            DropIndex("dbo.BenefitServs", "IX_BenefitServClass");
            DropIndex("dbo.BudgetItems", "IX_BudgetItem");
            DropIndex("dbo.PeopleTrain", "IX_PeopleEvent");
            DropIndex("dbo.PeopleTrain", "IX_PeopleTrain");
            RenameIndex(table: "dbo.BenefitServs", name: "IX_BenefitServ", newName: "IX_ParentId");
            CreateTable(
                "dbo.BudgetItemAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BudgetId = c.Int(nullable: false),
                        BudgetItemId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 3),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .ForeignKey("dbo.BudgetItems", t => t.BudgetItemId, cascadeDelete: false)
                .Index(t => new { t.BudgetId, t.BudgetItemId }, unique: true, name: "IX_BudgetItemAmount");
            
            CreateTable(
                "dbo.DeptBudgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeptId = c.Int(nullable: false),
                        SubPeriodId = c.Int(nullable: false),
                        BudgetAmount = c.Decimal(nullable: false, precision: 18, scale: 3),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompanyStructures", t => t.DeptId, cascadeDelete: false)
                .ForeignKey("dbo.SubPeriods", t => t.SubPeriodId, cascadeDelete: false)
                .Index(t => t.DeptId)
                .Index(t => t.SubPeriodId);
            
            AddColumn("dbo.People", "MilResDate", c => c.DateTime());
            AddColumn("dbo.People", "Rank", c => c.Short());
            AddColumn("dbo.People", "MilCertGrade", c => c.Short());
            AddColumn("dbo.Benefits", "BudgetItemId", c => c.Int());
            AddColumn("dbo.BenefitRequests", "BenefitId", c => c.Int(nullable: false));
            AddColumn("dbo.BenefitServs", "BenefitId", c => c.Int(nullable: false));
            AddColumn("dbo.BudgetItems", "Code", c => c.String(maxLength: 20));
            AddColumn("dbo.BudgetItems", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.BudgetItems", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.BudgetItems", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.BudgetItems", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.Budgets", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Budgets", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.Budgets", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.Budgets", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.CareerPathJobs", "Sequence", c => c.Int());
            AddColumn("dbo.Custody", "Available", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "IsNew", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "LinkToInvent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "ItemCode", c => c.String(maxLength: 30));
            AddColumn("dbo.Custody", "LinkToGL", c => c.Boolean(nullable: false));
            AddColumn("dbo.Custody", "AccountCode", c => c.String(maxLength: 30));
            AddColumn("dbo.EmpTasks", "CompanyId", c => c.Int(nullable: false));
            AddColumn("dbo.TrainCourses", "BudgetItemId", c => c.Int());
            AddColumn("dbo.PersonSetup", "ExceedBudgetLimit", c => c.Byte(nullable: false));
            AlterColumn("dbo.BudgetItems", "Name", c => c.String(maxLength: 250));
            CreateIndex("dbo.Benefits", "BudgetItemId");
            CreateIndex("dbo.BenefitRequests", "BenefitId");
            CreateIndex("dbo.BenefitServs", new[] { "BenefitId", "StartDate", "EndDate" }, name: "IX_BenefitServ");
            CreateIndex("dbo.PeopleTrain", new[] { "EventId", "EmpId" }, unique: true, name: "IX_PeopleEvent");
            CreateIndex("dbo.PeopleTrain", new[] { "EmpId", "CourseId" }, name: "IX_PeopleTrain");
            CreateIndex("dbo.TrainCourses", "BudgetItemId");
            AddForeignKey("dbo.Benefits", "BudgetItemId", "dbo.BudgetItems", "Id");
            AddForeignKey("dbo.BenefitRequests", "BenefitId", "dbo.Benefits", "Id", cascadeDelete: false);
            AddForeignKey("dbo.BenefitServs", "BenefitId", "dbo.Benefits", "Id", cascadeDelete: false);
            AddForeignKey("dbo.TrainCourses", "BudgetItemId", "dbo.BudgetItems", "Id");
            DropColumn("dbo.Jobs", "ContractEndNotifyDays");
            DropColumn("dbo.DocTypes", "NotifyNofDays");
            DropColumn("dbo.Positions", "BudgetId");
            DropColumn("dbo.Positions", "BudgetItemId");
            DropColumn("dbo.Positions", "BudgetAmount");
            DropColumn("dbo.BenefitRequests", "BenefitClass");
            DropColumn("dbo.BenefitServs", "BenefitClass");
            DropColumn("dbo.BudgetItems", "BudgetId");
            DropColumn("dbo.BudgetItems", "Amount");
            DropColumn("dbo.Budgets", "IsLocal");
            DropColumn("dbo.Budgets", "Source");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Budgets", "Source", c => c.Byte());
            AddColumn("dbo.Budgets", "IsLocal", c => c.Boolean(nullable: false));
            AddColumn("dbo.BudgetItems", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BudgetItems", "BudgetId", c => c.Int(nullable: false));
            AddColumn("dbo.BenefitServs", "BenefitClass", c => c.Short(nullable: false));
            AddColumn("dbo.BenefitRequests", "BenefitClass", c => c.Short(nullable: false));
            AddColumn("dbo.Positions", "BudgetAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Positions", "BudgetItemId", c => c.Int());
            AddColumn("dbo.Positions", "BudgetId", c => c.Int());
            AddColumn("dbo.DocTypes", "NotifyNofDays", c => c.Byte());
            AddColumn("dbo.Jobs", "ContractEndNotifyDays", c => c.Short());
            DropForeignKey("dbo.TrainCourses", "BudgetItemId", "dbo.BudgetItems");
            DropForeignKey("dbo.DeptBudgets", "SubPeriodId", "dbo.SubPeriods");
            DropForeignKey("dbo.DeptBudgets", "DeptId", "dbo.CompanyStructures");
            DropForeignKey("dbo.BudgetItemAmounts", "BudgetItemId", "dbo.BudgetItems");
            DropForeignKey("dbo.BudgetItemAmounts", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.BenefitServs", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.BenefitRequests", "BenefitId", "dbo.Benefits");
            DropForeignKey("dbo.Benefits", "BudgetItemId", "dbo.BudgetItems");
            DropIndex("dbo.TrainCourses", new[] { "BudgetItemId" });
            DropIndex("dbo.PeopleTrain", "IX_PeopleTrain");
            DropIndex("dbo.PeopleTrain", "IX_PeopleEvent");
            DropIndex("dbo.DeptBudgets", new[] { "SubPeriodId" });
            DropIndex("dbo.DeptBudgets", new[] { "DeptId" });
            DropIndex("dbo.BudgetItemAmounts", "IX_BudgetItemAmount");
            DropIndex("dbo.BenefitServs", "IX_BenefitServ");
            DropIndex("dbo.BenefitRequests", new[] { "BenefitId" });
            DropIndex("dbo.Benefits", new[] { "BudgetItemId" });
            AlterColumn("dbo.BudgetItems", "Name", c => c.String(maxLength: 100));
            DropColumn("dbo.PersonSetup", "ExceedBudgetLimit");
            DropColumn("dbo.TrainCourses", "BudgetItemId");
            DropColumn("dbo.EmpTasks", "CompanyId");
            DropColumn("dbo.Custody", "AccountCode");
            DropColumn("dbo.Custody", "LinkToGL");
            DropColumn("dbo.Custody", "ItemCode");
            DropColumn("dbo.Custody", "LinkToInvent");
            DropColumn("dbo.Custody", "IsNew");
            DropColumn("dbo.Custody", "Available");
            DropColumn("dbo.CareerPathJobs", "Sequence");
            DropColumn("dbo.Budgets", "ModifiedTime");
            DropColumn("dbo.Budgets", "CreatedTime");
            DropColumn("dbo.Budgets", "ModifiedUser");
            DropColumn("dbo.Budgets", "CreatedUser");
            DropColumn("dbo.BudgetItems", "ModifiedTime");
            DropColumn("dbo.BudgetItems", "CreatedTime");
            DropColumn("dbo.BudgetItems", "ModifiedUser");
            DropColumn("dbo.BudgetItems", "CreatedUser");
            DropColumn("dbo.BudgetItems", "Code");
            DropColumn("dbo.BenefitServs", "BenefitId");
            DropColumn("dbo.BenefitRequests", "BenefitId");
            DropColumn("dbo.Benefits", "BudgetItemId");
            DropColumn("dbo.People", "MilCertGrade");
            DropColumn("dbo.People", "Rank");
            DropColumn("dbo.People", "MilResDate");
            DropTable("dbo.DeptBudgets");
            DropTable("dbo.BudgetItemAmounts");
            RenameIndex(table: "dbo.BenefitServs", name: "IX_ParentId", newName: "IX_BenefitServ");
            CreateIndex("dbo.PeopleTrain", new[] { "EmpId", "CourseId" }, unique: true, name: "IX_PeopleTrain");
            CreateIndex("dbo.PeopleTrain", new[] { "EventId", "EmpId" }, name: "IX_PeopleEvent");
            CreateIndex("dbo.BudgetItems", new[] { "BudgetId", "Name" }, unique: true, name: "IX_BudgetItem");
            CreateIndex("dbo.BenefitServs", new[] { "BenefitClass", "StartDate", "EndDate" }, name: "IX_BenefitServClass");
            AddForeignKey("dbo.BudgetItems", "BudgetId", "dbo.Budgets", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.PosBudgets", newName: "PositionBudgets");
        }
    }
}
