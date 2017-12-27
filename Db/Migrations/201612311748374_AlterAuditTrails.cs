namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterAuditTrails : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AudiTrails", "IX_AudiTrail");
            CreateIndex("dbo.AudiTrails", new[] { "CompanyId", "ObjectName", "Version" }, name: "IX_AudiTrail");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AudiTrails", "IX_AudiTrail");
            CreateIndex("dbo.AudiTrails", new[] { "CompanyId", "ObjectName", "Version", "ColumnName" }, unique: true, name: "IX_AudiTrail");
        }
    }
}
