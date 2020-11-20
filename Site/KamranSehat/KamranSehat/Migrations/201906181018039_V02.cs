namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V02 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "ParentId", "dbo.Products");
            DropIndex("dbo.Products", new[] { "ParentId" });
            DropColumn("dbo.Products", "ParentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ParentId", c => c.Guid());
            CreateIndex("dbo.Products", "ParentId");
            AddForeignKey("dbo.Products", "ParentId", "dbo.Products", "Id");
        }
    }
}
