namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWaringOrErrorMsg : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FlexColumns", "IX_TableColumn");
            DropIndex("dbo.FlexData", "IX_TableColumnData");
            AddColumn("dbo.Personnels", "EmploymentDoc", c => c.Byte(nullable: false));
            AddColumn("dbo.Personnels", "JobDoc", c => c.Byte(nullable: false));
            AddColumn("dbo.Personnels", "AssignFlex", c => c.Byte(nullable: false));
            AlterColumn("dbo.FlexColumns", "TableName", c => c.String(nullable: false, maxLength: 30, unicode: false));
            AlterColumn("dbo.FlexData", "TableName", c => c.String(nullable: false, maxLength: 30, unicode: false));
            CreateIndex("dbo.FlexColumns", new[] { "TableName", "ColumnName" }, unique: true, name: "IX_TableColumn");
            CreateIndex("dbo.FlexData", new[] { "TableName", "ColumnName" }, unique: true, name: "IX_TableColumnData");
        }
        
        public override void Down()
        {
            DropIndex("dbo.FlexData", "IX_TableColumnData");
            DropIndex("dbo.FlexColumns", "IX_TableColumn");
            AlterColumn("dbo.FlexData", "TableName", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.FlexColumns", "TableName", c => c.String(nullable: false, maxLength: 50, unicode: false));
            DropColumn("dbo.Personnels", "AssignFlex");
            DropColumn("dbo.Personnels", "JobDoc");
            DropColumn("dbo.Personnels", "EmploymentDoc");
            CreateIndex("dbo.FlexData", new[] { "TableName", "ColumnName" }, unique: true, name: "IX_TableColumnData");
            CreateIndex("dbo.FlexColumns", new[] { "TableName", "ColumnName" }, unique: true, name: "IX_TableColumn");
        }
    }
}
