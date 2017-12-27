namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTermination : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TermDurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        TermSysCode = c.Byte(nullable: false),
                        WorkDuration = c.Byte(nullable: false),
                        FirstPeriod = c.Byte(nullable: false),
                        Percent1 = c.Single(nullable: false),
                        Percent2 = c.Single(),
                        CreatedUser = c.String(maxLength: 20),
                        ModifiedUser = c.String(maxLength: 20),
                        CreatedTime = c.DateTime(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => new { t.CompanyId, t.TermSysCode, t.WorkDuration }, unique: true, name: "IX_TermDuration");
            
            AddColumn("dbo.Personnels", "TermSysCode", c => c.Byte());
            AddColumn("dbo.Personnels", "WorkServMethod", c => c.Byte());
            AddColumn("dbo.Terminations", "BonusInMonths", c => c.Single(nullable: false));
            AlterColumn("dbo.Terminations", "ServYear", c => c.Single(nullable: false));
            DropColumn("dbo.Terminations", "ServMonth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Terminations", "ServMonth", c => c.Byte(nullable: false));
            DropForeignKey("dbo.TermDurations", "CompanyId", "dbo.Companies");
            DropIndex("dbo.TermDurations", "IX_TermDuration");
            AlterColumn("dbo.Terminations", "ServYear", c => c.Byte(nullable: false));
            DropColumn("dbo.Terminations", "BonusInMonths");
            DropColumn("dbo.Personnels", "WorkServMethod");
            DropColumn("dbo.Personnels", "TermSysCode");
            DropTable("dbo.TermDurations");
        }
    }
}
