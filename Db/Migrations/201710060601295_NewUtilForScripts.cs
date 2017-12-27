namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewUtilForScripts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LeaveAdjusts", "EmpId", "dbo.People");
            DropForeignKey("dbo.LeaveAdjusts", "TypeId", "dbo.LeaveTypes");
            DropForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.Periods");
            DropIndex("dbo.People", "IX_PersonName, 1");
            DropIndex("dbo.People", "IX_PersonName, 2");
            DropIndex("dbo.People", "IX_PersonName, 3");
            DropIndex("dbo.People", "IX_PersonName, 4");
            DropIndex("dbo.LeaveTypes", "IX_LeaveTypeCode");
            DropIndex("dbo.LeaveTypes", "IX_LeaveTypeName");
            DropIndex("dbo.LeaveAdjusts", "IX_LeaveActionCompany");
            DropIndex("dbo.LeaveAdjusts", "IX_LeaveAction");
            CreateTable(
                "dbo.LeaveAdjusts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        AdjustDate = c.DateTime(nullable: false, storeType: "date"),
                        TransType = c.Short(nullable: false),
                        NofDays = c.Single(nullable: false),
                        WorkingDate = c.DateTime(storeType: "date"),
                        ExpiryDate = c.DateTime(storeType: "date"),
                        Description = c.String(maxLength: 250),
                        CreatedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.EmpId, cascadeDelete: false)
                .ForeignKey("dbo.LeaveTypes", t => t.TypeId, cascadeDelete: false)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: false)
                .Index(t => t.CompanyId, name: "IX_LeaveAdjustCompany")
                .Index(t => new { t.TypeId, t.PeriodId, t.EmpId }, name: "IX_LeaveAdjust");
            
            CreateTable(
                "dbo.MenuFunctions",
                c => new
                    {
                        MenuId = c.Int(nullable: false),
                        FunctionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MenuId, t.FunctionId })
                .ForeignKey("dbo.Functions", t => t.FunctionId, cascadeDelete: false)
                .ForeignKey("dbo.Menus", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.MenuId)
                .Index(t => t.FunctionId);
            
            AddColumn("dbo.LeaveTrans", "ExpiryDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.MsgTbl", "Logged", c => c.Boolean(nullable: false));
            AddColumn("dbo.WfTrans", "Message", c => c.String(maxLength: 500));
            AlterColumn("dbo.People", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.People", "Fathername", c => c.String(maxLength: 50));
            AlterColumn("dbo.People", "GFathername", c => c.String(maxLength: 50));
            AlterColumn("dbo.People", "Familyname", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.People", "SubscripDate", c => c.DateTime(storeType: "date"));
            CreateIndex("dbo.People", "FirstName", name: "IX_PersonName, 1");
            CreateIndex("dbo.People", "Fathername", name: "IX_PersonName, 2");
            CreateIndex("dbo.People", "GFathername", name: "IX_PersonName, 3");
            CreateIndex("dbo.People", "Familyname", name: "IX_PersonName, 4");
           // CreateIndex("dbo.LeaveTypes", "Code", unique: true, name: "IX_LeaveTypeCode");
            CreateIndex("dbo.LeaveTypes", "Name", unique: true, name: "IX_LeaveTypeName");
            DropTable("dbo.LeaveAdjusts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LeaveAdjusts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                        EmpId = c.Int(nullable: false),
                        ActionDate = c.DateTime(nullable: false, storeType: "date"),
                        TransType = c.Short(nullable: false),
                        NofDays = c.Single(nullable: false),
                        Posted = c.Boolean(nullable: false),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.MenuFunctions", "MenuId", "dbo.Menus");
            DropForeignKey("dbo.MenuFunctions", "FunctionId", "dbo.Functions");
            DropForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.LeaveAdjusts", "TypeId", "dbo.LeaveTypes");
            DropForeignKey("dbo.LeaveAdjusts", "EmpId", "dbo.People");
            DropIndex("dbo.MenuFunctions", new[] { "FunctionId" });
            DropIndex("dbo.MenuFunctions", new[] { "MenuId" });
            DropIndex("dbo.LeaveAdjusts", "IX_LeaveAdjust");
            DropIndex("dbo.LeaveAdjusts", "IX_LeaveAdjustCompany");
            DropIndex("dbo.LeaveTypes", "IX_LeaveTypeName");
            DropIndex("dbo.LeaveTypes", "IX_LeaveTypeCode");
            DropIndex("dbo.People", "IX_PersonName, 4");
            DropIndex("dbo.People", "IX_PersonName, 3");
            DropIndex("dbo.People", "IX_PersonName, 2");
            DropIndex("dbo.People", "IX_PersonName, 1");
            AlterColumn("dbo.People", "SubscripDate", c => c.DateTime());
            AlterColumn("dbo.People", "Familyname", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.People", "GFathername", c => c.String(maxLength: 20));
            AlterColumn("dbo.People", "Fathername", c => c.String(maxLength: 20));
            AlterColumn("dbo.People", "FirstName", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.WfTrans", "Message");
            DropColumn("dbo.MsgTbl", "Logged");
            DropColumn("dbo.LeaveTrans", "ExpiryDate");
            DropTable("dbo.MenuFunctions");
            DropTable("dbo.LeaveAdjusts");
            CreateIndex("dbo.LeaveAdjusts", new[] { "TypeId", "PeriodId", "EmpId" }, name: "IX_LeaveAction");
            CreateIndex("dbo.LeaveAdjusts", "CompanyId", name: "IX_LeaveActionCompany");
            CreateIndex("dbo.LeaveTypes", "Name", name: "IX_LeaveTypeName");
            CreateIndex("dbo.LeaveTypes", "Code", name: "IX_LeaveTypeCode");
            CreateIndex("dbo.People", "Familyname", name: "IX_PersonName, 4");
            CreateIndex("dbo.People", "GFathername", name: "IX_PersonName, 3");
            CreateIndex("dbo.People", "Fathername", name: "IX_PersonName, 2");
            CreateIndex("dbo.People", "FirstName", name: "IX_PersonName, 1");
            AddForeignKey("dbo.LeaveAdjusts", "PeriodId", "dbo.Periods", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LeaveAdjusts", "TypeId", "dbo.LeaveTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LeaveAdjusts", "EmpId", "dbo.People", "Id", cascadeDelete: true);
        }
    }
}
