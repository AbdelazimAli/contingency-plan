namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSectorId : DbMigration
    {
        public override void Up()
        {
           // DropColumn("dbo.V_WF_TRANS", "SectorId");
            DropColumn("dbo.WfTrans", "SectorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WfTrans", "SectorId", c => c.Int());
            AddColumn("dbo.V_WF_TRANS", "SectorId", c => c.Int());
        }
    }
}
