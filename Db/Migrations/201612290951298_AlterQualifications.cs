namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterQualifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PeopleQuals", "IsQualification", c => c.Boolean(nullable: false));
            DropColumn("dbo.PeopleQuals", "QualCatSysCd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PeopleQuals", "QualCatSysCd", c => c.Byte(nullable: false));
            DropColumn("dbo.PeopleQuals", "IsQualification");
        }
    }
}
