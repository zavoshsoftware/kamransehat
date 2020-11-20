namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "OrderId", c => c.Guid());
            CreateIndex("dbo.Questions", "OrderId");
            AddForeignKey("dbo.Questions", "OrderId", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "OrderId", "dbo.Orders");
            DropIndex("dbo.Questions", new[] { "OrderId" });
            DropColumn("dbo.Questions", "OrderId");
        }
    }
}
