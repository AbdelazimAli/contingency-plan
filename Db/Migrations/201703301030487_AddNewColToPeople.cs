namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewColToPeople : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "IdIssueDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.People", "VisaNo", c => c.String(maxLength: 20));
            AddColumn("dbo.Employements", "ContIssueDate", c => c.DateTime(nullable: false, storeType: "date"));
            AddColumn("dbo.Employements", "DurInYears", c => c.Byte(nullable: false));
            AddColumn("dbo.Employements", "Curr", c => c.String(maxLength: 3, fixedLength: true, unicode: false));
            AddColumn("dbo.Employements", "VacationDur", c => c.Byte());
            AlterColumn("dbo.People", "JoinDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.People", "StartExpDate", c => c.DateTime(storeType: "date"));
            CreateIndex("dbo.Employements", "Curr");
            AddForeignKey("dbo.Employements", "Curr", "dbo.Currencies", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employements", "Curr", "dbo.Currencies");
            DropIndex("dbo.Employements", new[] { "Curr" });
            AlterColumn("dbo.People", "StartExpDate", c => c.DateTime());
            AlterColumn("dbo.People", "JoinDate", c => c.DateTime());
            DropColumn("dbo.Employements", "VacationDur");
            DropColumn("dbo.Employements", "Curr");
            DropColumn("dbo.Employements", "DurInYears");
            DropColumn("dbo.Employements", "ContIssueDate");
            DropColumn("dbo.People", "VisaNo");
            DropColumn("dbo.People", "IdIssueDate");
        }
    }
}
