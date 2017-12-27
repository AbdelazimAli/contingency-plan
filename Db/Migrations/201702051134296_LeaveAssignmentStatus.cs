namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LeaveAssignmentStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LeaveTypes", "ChangAssignStat", c => c.Boolean(nullable: false));
            AddColumn("dbo.LeaveTypes", "AssignStatus", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "AutoChangStat", c => c.Byte());
            AddColumn("dbo.LeaveTypes", "IncludContinu", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "AccBalDays", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "NofDays", c => c.Single());
            AlterColumn("dbo.LeaveTypes", "NofDays50", c => c.Single());
            DropColumn("dbo.LeaveTypes", "MinContinous");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LeaveTypes", "MinContinous", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "NofDays50", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "NofDays", c => c.Byte());
            AlterColumn("dbo.LeaveTypes", "AccBalDays", c => c.Single());
            DropColumn("dbo.LeaveTypes", "IncludContinu");
            DropColumn("dbo.LeaveTypes", "AutoChangStat");
            DropColumn("dbo.LeaveTypes", "AssignStatus");
            DropColumn("dbo.LeaveTypes", "ChangAssignStat");
        }
    }
}
