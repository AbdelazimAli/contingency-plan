namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSchedualTasks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SchedualTasks",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: false),
                        EventName = c.String(maxLength: 30),
                        EventUrl = c.String(maxLength: 50),
                        Enabled = c.Boolean(nullable: false, defaultValue: true),
                        StopOnError = c.Boolean(nullable: false),
                        LastStartDate = c.DateTime(),
                        LastEndDate = c.DateTime(),
                        LastSuccessDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EventId);
            
            AddColumn("dbo.AudiTrails", "Transtype", c => c.Byte(nullable: false, defaultValue: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AudiTrails", "Transtype");
            DropTable("dbo.SchedualTasks");
        }
    }
}
