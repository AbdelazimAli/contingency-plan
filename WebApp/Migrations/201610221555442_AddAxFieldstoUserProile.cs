namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAxFieldstoUserProile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NetworkDomain", c => c.String(maxLength: 15));
            AddColumn("dbo.AspNetUsers", "LastLogin", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "DefaultCountry", c => c.Int());
            AddColumn("dbo.AspNetUsers", "DefaultCompany", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Infolog", c => c.Byte());
            AddColumn("dbo.AspNetUsers", "ShutdownInMin", c => c.Byte());
            AddColumn("dbo.AspNetUsers", "TimeZone", c => c.String(maxLength: 5));
            AddColumn("dbo.AspNetUsers", "UploadDocs", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "ExportExcel", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "ExportTo", c => c.Byte());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ExportTo");
            DropColumn("dbo.AspNetUsers", "ExportExcel");
            DropColumn("dbo.AspNetUsers", "UploadDocs");
            DropColumn("dbo.AspNetUsers", "TimeZone");
            DropColumn("dbo.AspNetUsers", "ShutdownInMin");
            DropColumn("dbo.AspNetUsers", "Infolog");
            DropColumn("dbo.AspNetUsers", "DefaultCompany");
            DropColumn("dbo.AspNetUsers", "DefaultCountry");
            DropColumn("dbo.AspNetUsers", "LastLogin");
            DropColumn("dbo.AspNetUsers", "NetworkDomain");
        }
    }
}
