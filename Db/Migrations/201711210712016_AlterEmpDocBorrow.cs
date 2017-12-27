namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmpDocBorrow : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EmpDocBorrows", "delvryStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmpDocBorrows", "delvryStatus", c => c.Byte());
        }
    }
}
