namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Businesses", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Businesses", "UserId");
            AddForeignKey("dbo.Businesses", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Businesses", "UserId", "dbo.Users");
            DropIndex("dbo.Businesses", new[] { "UserId" });
            DropColumn("dbo.Businesses", "UserId");
        }
    }
}
