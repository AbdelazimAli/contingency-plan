namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterWFlow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComplainRequests", "WFlowId", c => c.Int());
            AddColumn("dbo.LeaveRequests", "WFlowId", c => c.Int());
            AddColumn("dbo.Terminations", "WFlowId", c => c.Int());
            //DropColumn("dbo.V_WF_TRANS", "Source");
        }
        
        public override void Down()
        {
            AddColumn("dbo.V_WF_TRANS", "Source", c => c.String());
            DropColumn("dbo.Terminations", "WFlowId");
            DropColumn("dbo.LeaveRequests", "WFlowId");
            DropColumn("dbo.ComplainRequests", "WFlowId");
        }
    }
}
