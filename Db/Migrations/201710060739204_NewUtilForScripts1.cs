namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewUtilForScripts1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.People", "IX_PersonName, 1");
            DropIndex("dbo.People", "IX_PersonName, 2");
            DropIndex("dbo.People", "IX_PersonName, 3");
            DropIndex("dbo.People", "IX_PersonName, 4");
            AddColumn("dbo.LeaveAdjusts", "PayDone", c => c.Boolean(nullable: false));
            //AddColumn("dbo.V_WF_TRANS", "Message", c => c.String());
            AlterColumn("dbo.People", "FirstName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.People", "Fathername", c => c.String(maxLength: 100));
            AlterColumn("dbo.People", "GFathername", c => c.String(maxLength: 100));
            AlterColumn("dbo.People", "Familyname", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.People", "FirstName", name: "IX_PersonName, 1");
            CreateIndex("dbo.People", "Fathername", name: "IX_PersonName, 2");
            CreateIndex("dbo.People", "GFathername", name: "IX_PersonName, 3");
            CreateIndex("dbo.People", "Familyname", name: "IX_PersonName, 4");
        }
        
        public override void Down()
        {
            DropIndex("dbo.People", "IX_PersonName, 4");
            DropIndex("dbo.People", "IX_PersonName, 3");
            DropIndex("dbo.People", "IX_PersonName, 2");
            DropIndex("dbo.People", "IX_PersonName, 1");
            AlterColumn("dbo.People", "Familyname", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.People", "GFathername", c => c.String(maxLength: 50));
            AlterColumn("dbo.People", "Fathername", c => c.String(maxLength: 50));
            AlterColumn("dbo.People", "FirstName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.V_WF_TRANS", "Message");
            DropColumn("dbo.LeaveAdjusts", "PayDone");
            CreateIndex("dbo.People", "Familyname", name: "IX_PersonName, 4");
            CreateIndex("dbo.People", "GFathername", name: "IX_PersonName, 3");
            CreateIndex("dbo.People", "Fathername", name: "IX_PersonName, 2");
            CreateIndex("dbo.People", "FirstName", name: "IX_PersonName, 1");
        }
    }
}
