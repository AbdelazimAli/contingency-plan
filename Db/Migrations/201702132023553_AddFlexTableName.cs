namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlexTableName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmpChkLists", "CompanyId", c => c.Int());
            AddColumn("dbo.FlexColumns", "TableName", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddColumn("dbo.FlexData", "TableName", c => c.String(nullable: false, maxLength: 50, unicode: false));
            CreateIndex("dbo.FlexColumns", new[] { "TableName", "ColumnName" }, unique: true, name: "IX_TableColumn");
            CreateIndex("dbo.FlexData", new[] { "TableName", "ColumnName" }, unique: true, name: "IX_TableColumnData");
        }
        
        public override void Down()
        {
            DropIndex("dbo.FlexData", "IX_TableColumnData");
            DropIndex("dbo.FlexColumns", "IX_TableColumn");
            DropColumn("dbo.FlexData", "TableName");
            DropColumn("dbo.FlexColumns", "TableName");
            DropColumn("dbo.EmpChkLists", "CompanyId");
        }
    }
}
