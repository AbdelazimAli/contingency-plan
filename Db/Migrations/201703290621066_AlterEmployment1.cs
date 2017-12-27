namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmployment1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "ContractTempl", c => c.String(maxLength: 50));
            AddColumn("dbo.Employements", "SuggestJobId", c => c.Int());
            AlterColumn("dbo.TrainEvents", "CostFlag", c => c.Byte());
            CreateIndex("dbo.Employements", "SuggestJobId");
            AddForeignKey("dbo.Employements", "SuggestJobId", "dbo.Jobs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employements", "SuggestJobId", "dbo.Jobs");
            DropIndex("dbo.Employements", new[] { "SuggestJobId" });
            AlterColumn("dbo.TrainEvents", "CostFlag", c => c.Byte(nullable: false));
            DropColumn("dbo.Employements", "SuggestJobId");
            DropColumn("dbo.Jobs", "ContractTempl");
        }
    }
}
