namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterJob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonSetup", "QualGroupId", c => c.Int());
            AlterColumn("dbo.DocTypes", "Gender", c => c.Short());
            AlterColumn("dbo.Jobs", "ReplacementRequired", c => c.Boolean());
            AlterColumn("dbo.People", "WorkEmail", c => c.String(maxLength: 50));
            AlterColumn("dbo.People", "OtherEmail", c => c.String(maxLength: 50));
            CreateIndex("dbo.PersonSetup", "QualGroupId");
            AddForeignKey("dbo.PersonSetup", "QualGroupId", "dbo.QualGroups", "Id");
            DropColumn("dbo.Jobs", "NotifyIfAbsent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "NotifyIfAbsent", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.PersonSetup", "QualGroupId", "dbo.QualGroups");
            DropIndex("dbo.PersonSetup", new[] { "QualGroupId" });
            AlterColumn("dbo.People", "OtherEmail", c => c.String(maxLength: 30));
            AlterColumn("dbo.People", "WorkEmail", c => c.String(maxLength: 30));
            AlterColumn("dbo.Jobs", "ReplacementRequired", c => c.Boolean(nullable: false));
            AlterColumn("dbo.DocTypes", "Gender", c => c.Short(nullable: false));
            DropColumn("dbo.PersonSetup", "QualGroupId");
        }
    }
}
