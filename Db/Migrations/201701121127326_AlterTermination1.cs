namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTermination1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.LookUpCode", "IX_Name");
            DropIndex("dbo.LookUpUserCodes", "IX_UserCodeName");
            AddColumn("dbo.Disciplines", "DeductPoint", c => c.Short());
            AddColumn("dbo.EmpDisciplines", "SuggNofDays", c => c.Single());
            AddColumn("dbo.EmpDisciplines", "ActualNofDays", c => c.Single());
            AddColumn("dbo.Terminations", "TermReason", c => c.Byte(nullable: false));
            AddColumn("dbo.Terminations", "ReasonDesc", c => c.String(maxLength: 250));
            AddColumn("dbo.Terminations", "RejectReason", c => c.Byte());
            AddColumn("dbo.Terminations", "RejectDesc", c => c.String(maxLength: 250));
            AddColumn("dbo.Terminations", "CancelReason", c => c.Byte());
            AddColumn("dbo.Terminations", "CancelDesc", c => c.String(maxLength: 250));
            AddColumn("dbo.Terminations", "FlowOrder", c => c.Byte());
            AlterColumn("dbo.DisplinRepeats", "NofDays", c => c.Single());
            AlterColumn("dbo.LookUpCode", "Name", c => c.String(maxLength: 150));
            AlterColumn("dbo.LookUpUserCodes", "Name", c => c.String(maxLength: 150));
            CreateIndex("dbo.LookUpCode", new[] { "CodeName", "Name" }, unique: true, name: "IX_Name");
            CreateIndex("dbo.LookUpUserCodes", new[] { "CodeName", "Name" }, unique: true, name: "IX_UserCodeName");
            DropColumn("dbo.Terminations", "TermCause");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Terminations", "TermCause", c => c.Byte(nullable: false));
            DropIndex("dbo.LookUpUserCodes", "IX_UserCodeName");
            DropIndex("dbo.LookUpCode", "IX_Name");
            AlterColumn("dbo.LookUpUserCodes", "Name", c => c.String(maxLength: 30));
            AlterColumn("dbo.LookUpCode", "Name", c => c.String(maxLength: 30));
            AlterColumn("dbo.DisplinRepeats", "NofDays", c => c.Byte());
            DropColumn("dbo.Terminations", "FlowOrder");
            DropColumn("dbo.Terminations", "CancelDesc");
            DropColumn("dbo.Terminations", "CancelReason");
            DropColumn("dbo.Terminations", "RejectDesc");
            DropColumn("dbo.Terminations", "RejectReason");
            DropColumn("dbo.Terminations", "ReasonDesc");
            DropColumn("dbo.Terminations", "TermReason");
            DropColumn("dbo.EmpDisciplines", "ActualNofDays");
            DropColumn("dbo.EmpDisciplines", "SuggNofDays");
            DropColumn("dbo.Disciplines", "DeductPoint");
            CreateIndex("dbo.LookUpUserCodes", new[] { "CodeName", "Name" }, unique: true, name: "IX_UserCodeName");
            CreateIndex("dbo.LookUpCode", new[] { "CodeName", "Name" }, unique: true, name: "IX_Name");
        }
    }
}
