namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHolidays : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyDocuments", "SourceId", c => c.Int());
            AddColumn("dbo.Personnels", "Weekend1", c => c.Byte());
            AddColumn("dbo.Personnels", "Weekend2", c => c.Byte());
            AlterColumn("dbo.People", "UpdtCVDate", c => c.DateTime(storeType: "date")); 
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "UpdtCVDate", c => c.DateTime(nullable: false, storeType: "date"));
            DropColumn("dbo.Personnels", "Weekend2");
            DropColumn("dbo.Personnels", "Weekend1");
            DropColumn("dbo.CompanyDocsViews", "SourceId");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
