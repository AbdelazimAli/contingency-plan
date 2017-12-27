namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterMedicalRequest : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MedicalRequests", "ServCost", c => c.Decimal(precision: 18, scale: 3));
            AlterColumn("dbo.MedicalRequests", "EmpCost", c => c.Decimal(precision: 18, scale: 3));
            AlterColumn("dbo.MedicalRequests", "CompanyCost", c => c.Decimal(precision: 18, scale: 3));
            AlterColumn("dbo.MedicalRequests", "IssueDate", c => c.DateTime());
            AlterColumn("dbo.MedicalRequests", "ExpiryDate", c => c.DateTime());
            AlterColumn("dbo.MedicalRequests", "ServStartDate", c => c.DateTime());
            AlterColumn("dbo.MedicalRequests", "ServEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MedicalRequests", "ServEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MedicalRequests", "ServStartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MedicalRequests", "ExpiryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MedicalRequests", "IssueDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MedicalRequests", "CompanyCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MedicalRequests", "EmpCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.MedicalRequests", "ServCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
