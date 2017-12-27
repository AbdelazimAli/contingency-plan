namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPositionHier : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestWf", "Hierarchy", c => c.Int());
            CreateIndex("dbo.RequestWf", "Hierarchy");
            AddForeignKey("dbo.RequestWf", "Hierarchy", "dbo.Diagrams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestWf", "Hierarchy", "dbo.Diagrams");
            DropIndex("dbo.RequestWf", new[] { "Hierarchy" });
            AlterColumn("dbo.RequestWf", "Hierarchy", c => c.Byte());
        }
    }
}
