namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCheckList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChecklistTasks", "Required", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmpChkLists", "ListId", c => c.Int());
            AlterColumn("dbo.Custody", "Name", c => c.String(nullable: false, maxLength: 150));
            CreateIndex("dbo.EmpChkLists", "ListId");
            AddForeignKey("dbo.EmpChkLists", "ListId", "dbo.CheckLists", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmpChkLists", "ListId", "dbo.CheckLists");
            DropIndex("dbo.EmpChkLists", new[] { "ListId" });
            AlterColumn("dbo.Custody", "Name", c => c.String(maxLength: 150));
            DropColumn("dbo.EmpChkLists", "ListId");
            DropColumn("dbo.ChecklistTasks", "Required");
        }
    }
}
