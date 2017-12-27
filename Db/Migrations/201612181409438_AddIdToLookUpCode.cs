namespace Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdToLookUpCode : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" }, "dbo.SystemCodes");
            DropIndex("dbo.LookUpCode", "IX_Name");
            DropIndex("dbo.LookUpUserCodes", "IX_UserCodeName");
            DropIndex("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" });
            DropPrimaryKey("dbo.LookUpCode");
            DropPrimaryKey("dbo.LookUpUserCodes");
            AddColumn("dbo.LookUpCode", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.LookUpCode", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.LookUpCode", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.LookUpCode", "CreatedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LookUpCode", "ModifiedTime", c => c.DateTime());
            AddColumn("dbo.LookUpUserCodes", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.LookUpUserCodes", "CreatedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.LookUpUserCodes", "ModifiedUser", c => c.String(maxLength: 20));
            AddColumn("dbo.LookUpUserCodes", "CreatedTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.LookUpUserCodes", "ModifiedTime", c => c.DateTime());
            AlterColumn("dbo.LookUpCode", "CodeName", c => c.String(maxLength: 20));
            AlterColumn("dbo.LookUpUserCodes", "CodeName", c => c.String(maxLength: 20));
            AddPrimaryKey("dbo.LookUpCode", "Id");
            AddPrimaryKey("dbo.LookUpUserCodes", "Id");
            CreateIndex("dbo.LookUpCode", new[] { "CodeName", "CodeId" }, unique: true, name: "IX_LookUpCode");
            CreateIndex("dbo.LookUpCode", new[] { "CodeName", "Name" }, unique: true, name: "IX_Name");
            CreateIndex("dbo.LookUpUserCodes", new[] { "CodeName", "CodeId" }, unique: true, name: "IX_LookUpUserCode");
            CreateIndex("dbo.LookUpUserCodes", new[] { "CodeName", "Name" }, unique: true, name: "IX_UserCodeName");
            CreateIndex("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" });
            AddForeignKey("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" }, "dbo.SystemCodes", new[] { "CodeName", "SysCodeId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" }, "dbo.SystemCodes");
            DropIndex("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" });
            DropIndex("dbo.LookUpUserCodes", "IX_UserCodeName");
            DropIndex("dbo.LookUpUserCodes", "IX_LookUpUserCode");
            DropIndex("dbo.LookUpCode", "IX_Name");
            DropIndex("dbo.LookUpCode", "IX_LookUpCode");
            DropPrimaryKey("dbo.LookUpUserCodes");
            DropPrimaryKey("dbo.LookUpCode");
            AlterColumn("dbo.LookUpUserCodes", "CodeName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.LookUpCode", "CodeName", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.LookUpUserCodes", "ModifiedTime");
            DropColumn("dbo.LookUpUserCodes", "CreatedTime");
            DropColumn("dbo.LookUpUserCodes", "ModifiedUser");
            DropColumn("dbo.LookUpUserCodes", "CreatedUser");
            DropColumn("dbo.LookUpUserCodes", "Id");
            DropColumn("dbo.LookUpCode", "ModifiedTime");
            DropColumn("dbo.LookUpCode", "CreatedTime");
            DropColumn("dbo.LookUpCode", "ModifiedUser");
            DropColumn("dbo.LookUpCode", "CreatedUser");
            DropColumn("dbo.LookUpCode", "Id");
            AddPrimaryKey("dbo.LookUpUserCodes", new[] { "CodeName", "CodeId" });
            AddPrimaryKey("dbo.LookUpCode", new[] { "CodeName", "CodeId" });
            CreateIndex("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" });
            CreateIndex("dbo.LookUpUserCodes", new[] { "CodeName", "Name" }, unique: true, name: "IX_UserCodeName");
            CreateIndex("dbo.LookUpCode", new[] { "CodeName", "Name" }, unique: true, name: "IX_Name");
            AddForeignKey("dbo.LookUpUserCodes", new[] { "CodeName", "SysCodeId" }, "dbo.SystemCodes", new[] { "CodeName", "SysCodeId" }, cascadeDelete: true);
        }
    }
}
