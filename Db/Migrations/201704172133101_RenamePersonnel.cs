namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamePersonnel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Personnels", newName: "PersonSetup");
            DropIndex("dbo.Payrolls", "IX_Payroll");
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsLocal = c.Boolean(nullable: false),
                        CompanyId = c.Int(),
                        StartDate = c.DateTime(nullable: false, storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        AccType = c.Byte(nullable: false),
                        Segment1 = c.String(maxLength: 10),
                        Segment2 = c.String(maxLength: 10),
                        Segment3 = c.String(maxLength: 10),
                        Segment4 = c.String(maxLength: 10),
                        Segment5 = c.String(maxLength: 10),
                        Segment6 = c.String(maxLength: 10),
                        Segment7 = c.String(maxLength: 10),
                        Segment8 = c.String(maxLength: 10),
                        Segment9 = c.String(maxLength: 10),
                        Segment10 = c.String(maxLength: 10),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.Code, unique: true, name: "IX_AccountCode")
                .Index(t => t.CompanyId);
            
            AddColumn("dbo.PayrollGrades", "IsLocal", c => c.Boolean(nullable: false));
            AddColumn("dbo.PayrollGrades", "CompanyId", c => c.Int());
            AddColumn("dbo.PayrollGrades", "Points", c => c.String(maxLength: 100));
            AddColumn("dbo.PayrollGrades", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.PayrollGrades", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.PayrollGrades", "CreatedTime", c => c.DateTime());
            AddColumn("dbo.PayrollGrades", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.Payrolls", "Code", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Payrolls", "IsLocal", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payrolls", "CompanyId", c => c.Int());
            AddColumn("dbo.Payrolls", "StartDate", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.Payrolls", "EndDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Payrolls", "PrdNameId", c => c.Int(nullable: false));
            AddColumn("dbo.Payrolls", "FirstCloseDate", c => c.DateTime());
            AddColumn("dbo.Payrolls", "CalcOfstDays", c => c.Short(nullable: false));
            AddColumn("dbo.Payrolls", "PayOfstDays", c => c.Short(nullable: false));
            AddColumn("dbo.Payrolls", "PayrollGroup", c => c.Short());
            AddColumn("dbo.Payrolls", "AllowNegSalary", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payrolls", "AccrualSalAcct", c => c.Int());
            AddColumn("dbo.Payrolls", "DistPercent", c => c.Byte(nullable: false));
            AlterColumn("dbo.PayrollGrades", "Code", c => c.Int(nullable: false));
            AlterColumn("dbo.PayrollGrades", "Name", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.PayrollGrades", "Group", c => c.String(maxLength: 20));
            AlterColumn("dbo.PayrollGrades", "Grade", c => c.String(maxLength: 20));
            AlterColumn("dbo.PayrollGrades", "SubGrade", c => c.String(maxLength: 20));
            AlterColumn("dbo.Payrolls", "Name", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.PayrollGrades", "Code", unique: true, name: "IX_PayrollGrade");
            CreateIndex("dbo.PayrollGrades", "CompanyId");
            CreateIndex("dbo.Payrolls", "Code", unique: true, name: "IX_PayrollCode");
            CreateIndex("dbo.Payrolls", "CompanyId");
            CreateIndex("dbo.Payrolls", "PrdNameId");
            CreateIndex("dbo.Payrolls", "AccrualSalAcct");
            AddForeignKey("dbo.PayrollGrades", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.Payrolls", "AccrualSalAcct", "dbo.Accounts", "Id");
            AddForeignKey("dbo.Payrolls", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.Payrolls", "PrdNameId", "dbo.PeriodNames", "Id", cascadeDelete: false);
            DropColumn("dbo.PayrollGrades", "StartPoint");
            DropColumn("dbo.PayrollGrades", "EndPoint");
            DropColumn("dbo.PayrollGrades", "Step");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PayrollGrades", "Step", c => c.Short());
            AddColumn("dbo.PayrollGrades", "EndPoint", c => c.Int());
            AddColumn("dbo.PayrollGrades", "StartPoint", c => c.Int());
            DropForeignKey("dbo.Payrolls", "PrdNameId", "dbo.PeriodNames");
            DropForeignKey("dbo.Payrolls", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Payrolls", "AccrualSalAcct", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.PayrollGrades", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Accounts", new[] { "CompanyId" });
            DropIndex("dbo.Accounts", "IX_AccountCode");
            DropIndex("dbo.Payrolls", new[] { "AccrualSalAcct" });
            DropIndex("dbo.Payrolls", new[] { "PrdNameId" });
            DropIndex("dbo.Payrolls", new[] { "CompanyId" });
            DropIndex("dbo.Payrolls", "IX_PayrollCode");
            DropIndex("dbo.PayrollGrades", new[] { "CompanyId" });
            DropIndex("dbo.PayrollGrades", "IX_PayrollGrade");
            AlterColumn("dbo.Payrolls", "Name", c => c.String(maxLength: 100));
            AlterColumn("dbo.PayrollGrades", "SubGrade", c => c.Short());
            AlterColumn("dbo.PayrollGrades", "Grade", c => c.Short());
            AlterColumn("dbo.PayrollGrades", "Group", c => c.Short());
            AlterColumn("dbo.PayrollGrades", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.PayrollGrades", "Code", c => c.String(maxLength: 30));
            DropColumn("dbo.Payrolls", "DistPercent");
            DropColumn("dbo.Payrolls", "AccrualSalAcct");
            DropColumn("dbo.Payrolls", "AllowNegSalary");
            DropColumn("dbo.Payrolls", "PayrollGroup");
            DropColumn("dbo.Payrolls", "PayOfstDays");
            DropColumn("dbo.Payrolls", "CalcOfstDays");
            DropColumn("dbo.Payrolls", "FirstCloseDate");
            DropColumn("dbo.Payrolls", "PrdNameId");
            DropColumn("dbo.Payrolls", "EndDate");
            DropColumn("dbo.Payrolls", "StartDate");
            DropColumn("dbo.Payrolls", "CompanyId");
            DropColumn("dbo.Payrolls", "IsLocal");
            DropColumn("dbo.Payrolls", "Code");
            DropColumn("dbo.PayrollGrades", "ModifiedTime");
            DropColumn("dbo.PayrollGrades", "CreatedTime");
            DropColumn("dbo.PayrollGrades", "ModifiedUser");
            DropColumn("dbo.PayrollGrades", "CreatedUser");
            DropColumn("dbo.PayrollGrades", "Points");
            DropColumn("dbo.PayrollGrades", "CompanyId");
            DropColumn("dbo.PayrollGrades", "IsLocal");
            DropTable("dbo.Accounts");
            CreateIndex("dbo.Payrolls", "Name", unique: true, name: "IX_Payroll");
            RenameTable(name: "dbo.PersonSetup", newName: "Personnels");
        }
    }
}
