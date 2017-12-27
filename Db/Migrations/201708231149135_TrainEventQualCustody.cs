namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrainEventQualCustody : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PeopleQuals", "QualId", "dbo.Qualifications");
            DropIndex("dbo.Custody", "IX_CustodyCompany");
            DropIndex("dbo.Custody", "IX_CustodyCode");
            DropIndex("dbo.Custody", "IX_CustodyName");
            DropIndex("dbo.PeopleQuals", new[] { "QualId" });
            DropIndex("dbo.PeopleTrain", "IX_PeopleEvent");
            // Create Index
            Sql(@"CREATE UNIQUE NONCLUSTERED INDEX
             [IX_PeopleEvent] ON [dbo].[PeopleTrain]
             (
                [EventId] ASC,
                [EmpId] ASC 
             )
             WHERE ([EventId] IS NOT NULL)");
            AddColumn("dbo.People", "TreatCardNo", c => c.String(maxLength: 20));
            AlterColumn("dbo.PeopleQuals", "QualId", c => c.Int());
            CreateIndex("dbo.Custody", new[] { "CompanyId", "Code" }, unique: true, name: "IX_CustodyCode");
            CreateIndex("dbo.Custody", new[] { "CompanyId", "Name" }, unique: true, name: "IX_CustodyName");
            CreateIndex("dbo.PeopleQuals", "QualId");
            AddForeignKey("dbo.PeopleQuals", "QualId", "dbo.Qualifications", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PeopleQuals", "QualId", "dbo.Qualifications");
            DropIndex("dbo.PeopleQuals", new[] { "QualId" });
            DropIndex("dbo.Custody", "IX_CustodyName");
            DropIndex("dbo.Custody", "IX_CustodyCode");
            AlterColumn("dbo.PeopleQuals", "QualId", c => c.Int(nullable: false));
            DropColumn("dbo.People", "TreatCardNo");
            RenameIndex(table: "dbo.PeopleTrain", name: "IX_TrainEvent", newName: "IX_PeopleEvent");
            CreateIndex("dbo.PeopleQuals", "QualId");
            CreateIndex("dbo.Custody", "Name", name: "IX_CustodyName");
            CreateIndex("dbo.Custody", "Code", name: "IX_CustodyCode");
            CreateIndex("dbo.Custody", "CompanyId", name: "IX_CustodyCompany");
            AddForeignKey("dbo.PeopleQuals", "QualId", "dbo.Qualifications", "Id", cascadeDelete: true);
        }
    }
}
