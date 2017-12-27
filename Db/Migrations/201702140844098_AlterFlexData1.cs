namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFlexData1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.FlexData", "IX_TableColumnData");
            CreateIndex("dbo.FlexData", new[] { "TableName", "ColumnName", "SourceId" }, unique: true, name: "IX_TableColumnData");
        }
        
        public override void Down()
        {
            DropIndex("dbo.FlexData", "IX_TableColumnData");
            CreateIndex("dbo.FlexData", new[] { "TableName", "ColumnName" }, unique: true, name: "IX_TableColumnData");
        }
    }
}
