namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotifyLetter1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.NotifyLetters", "IX_NotifyLetter");
            AlterColumn("dbo.NotifyLetters", "SourceId", c => c.String(maxLength: 128));
            CreateIndex("dbo.NotifyLetters", new[] { "SourceId", "NotifySource" }, name: "IX_NotifyLetter");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NotifyLetters", "IX_NotifyLetter");
            AlterColumn("dbo.NotifyLetters", "SourceId", c => c.Int(nullable: false));
            CreateIndex("dbo.NotifyLetters", new[] { "SourceId", "NotifySource" }, name: "IX_NotifyLetter");
        }
    }
}
