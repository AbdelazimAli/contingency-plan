namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmpQual : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PeopleQuals", "SchoolId", c => c.Int());
            CreateIndex("dbo.PeopleQuals", "SchoolId");
            AddForeignKey("dbo.PeopleQuals", "SchoolId", "dbo.Schools", "Id");
            DropColumn("dbo.PeopleQuals", "Establishment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PeopleQuals", "Establishment", c => c.String(maxLength: 100));
            DropForeignKey("dbo.PeopleQuals", "SchoolId", "dbo.Schools");
            DropIndex("dbo.PeopleQuals", new[] { "SchoolId" });
            DropColumn("dbo.PeopleQuals", "SchoolId");
        }
    }
}
