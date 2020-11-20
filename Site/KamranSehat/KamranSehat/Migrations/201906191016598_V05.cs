namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V05 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Duration", c => c.String());
            AddColumn("dbo.Products", "Size", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Size");
            DropColumn("dbo.Products", "Duration");
        }
    }
}
