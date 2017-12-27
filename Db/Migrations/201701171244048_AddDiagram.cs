namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiagram : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WfTrans", "IX_WfTrans");
            CreateTable(
                "dbo.DiagramNodes",
                c => new
                    {
                        DiagramId = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DiagramId, t.ParentId, t.ChildId })
                .ForeignKey("dbo.Diagrams", t => t.DiagramId, cascadeDelete: true)
                .Index(t => t.DiagramId);
            
            CreateTable(
                "dbo.Diagrams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Color = c.String(maxLength: 20),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .Index(t => t.CompanyId);
            
            AddColumn("dbo.WfTrans", "Sequence", c => c.Short(nullable: false));
            AddColumn("dbo.WfTrans", "DeptId", c => c.Int());
            AlterColumn("dbo.RequestWf", "Hierarchy", c => c.Byte());
            CreateIndex("dbo.WfTrans", new[] { "WFlowId", "DocumentId", "Sequence" }, unique: true, name: "IX_WfTrans");
            DropTable("dbo.PositionHeirs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PositionHeirs",
                c => new
                    {
                        Hierarchy = c.Byte(nullable: false),
                        ParentId = c.Int(nullable: false),
                        ChildId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hierarchy, t.ParentId, t.ChildId });
            
            DropForeignKey("dbo.DiagramNodes", "DiagramId", "dbo.Diagrams");
            DropForeignKey("dbo.Diagrams", "CompanyId", "dbo.Companies");
            DropIndex("dbo.WfTrans", "IX_WfTrans");
            DropIndex("dbo.Diagrams", new[] { "CompanyId" });
            DropIndex("dbo.DiagramNodes", new[] { "DiagramId" });
            AlterColumn("dbo.RequestWf", "Hierarchy", c => c.Byte(nullable: false));
            DropColumn("dbo.WfTrans", "DeptId");
            DropColumn("dbo.WfTrans", "Sequence");
            DropTable("dbo.Diagrams");
            DropTable("dbo.DiagramNodes");
            CreateIndex("dbo.WfTrans", new[] { "WFlowId", "DocumentId", "Order" }, unique: true, name: "IX_WfTrans");
        }
    }
}
