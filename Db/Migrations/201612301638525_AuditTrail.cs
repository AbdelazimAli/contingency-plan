namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditTrail : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PageDivs", "IX_Grid");
            DropIndex("dbo.LeavePeriods", new[] { "LeaveTypeId" });
            DropIndex("dbo.LeavePeriods", "IX_LeavePeriod");
            DropPrimaryKey("dbo.ColumnTitles");
            DropPrimaryKey("dbo.RoleColumns");
            CreateTable(
                "dbo.AudiTrails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ObjectName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Version = c.Byte(nullable: false),
                        ColumnName = c.String(nullable: false, maxLength: 50, unicode: false),
                        SourceId = c.Int(nullable: false),
                        ValueBefore = c.String(maxLength: 250),
                        ValueAfter = c.String(maxLength: 250),
                        ModifiedUser = c.String(maxLength: 20),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.CompanyId, t.ObjectName, t.Version, t.ColumnName }, unique: true, name: "IX_AudiTrail");
            
            AlterColumn("dbo.ColumnTitles", "Version", c => c.Byte(nullable: false));
            AlterColumn("dbo.PageDivs", "Version", c => c.Byte(nullable: false));
            AlterColumn("dbo.Menus", "Version", c => c.Byte(nullable: false));
            AlterColumn("dbo.RoleColumns", "Version", c => c.Byte(nullable: false));
            AddPrimaryKey("dbo.ColumnTitles", new[] { "CompanyId", "Culture", "ObjectName", "Version", "ColumnName" });
            AddPrimaryKey("dbo.RoleColumns", new[] { "CompanyId", "ObjectName", "Version", "RoleId", "ColumnName" });
            CreateIndex("dbo.PageDivs", new[] { "CompanyId", "ObjectName", "Version" }, unique: true, name: "IX_Grid");
            CreateIndex("dbo.LeavePeriods", new[] { "LeaveTypeId", "PeriodNo" }, unique: true, name: "IX_LeavePeriod");
        }
        
        public override void Down()
        {
            DropIndex("dbo.LeavePeriods", "IX_LeavePeriod");
            DropIndex("dbo.PageDivs", "IX_Grid");
            DropIndex("dbo.AudiTrails", "IX_AudiTrail");
            DropPrimaryKey("dbo.RoleColumns");
            DropPrimaryKey("dbo.ColumnTitles");
            AlterColumn("dbo.RoleColumns", "Version", c => c.Int(nullable: false));
            AlterColumn("dbo.Menus", "Version", c => c.Int(nullable: false));
            AlterColumn("dbo.PageDivs", "Version", c => c.Int(nullable: false));
            AlterColumn("dbo.ColumnTitles", "Version", c => c.Int(nullable: false));
            DropTable("dbo.AudiTrails");
            AddPrimaryKey("dbo.RoleColumns", new[] { "CompanyId", "ObjectName", "Version", "RoleId", "ColumnName" });
            AddPrimaryKey("dbo.ColumnTitles", new[] { "CompanyId", "Culture", "ObjectName", "Version", "ColumnName" });
            CreateIndex("dbo.LeavePeriods", "PeriodNo", unique: true, name: "IX_LeavePeriod");
            CreateIndex("dbo.LeavePeriods", "LeaveTypeId");
            CreateIndex("dbo.PageDivs", new[] { "CompanyId", "ObjectName", "Version" }, unique: true, name: "IX_Grid");
        }
    }
}
