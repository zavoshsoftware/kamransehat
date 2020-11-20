namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "ServiceGroupId", c => c.Guid(nullable: false));
            AlterColumn("dbo.ServiceGroups", "Body", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Services", "Body", c => c.String(storeType: "ntext"));
            CreateIndex("dbo.Services", "ServiceGroupId");
            AddForeignKey("dbo.Services", "ServiceGroupId", "dbo.ServiceGroups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "ServiceGroupId", "dbo.ServiceGroups");
            DropIndex("dbo.Services", new[] { "ServiceGroupId" });
            AlterColumn("dbo.Services", "Body", c => c.String());
            AlterColumn("dbo.ServiceGroups", "Body", c => c.String());
            DropColumn("dbo.Services", "ServiceGroupId");
        }
    }
}
