namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SampleVideoUrl", c => c.String());
            AddColumn("dbo.Products", "VideoUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "VideoUrl");
            DropColumn("dbo.Products", "SampleVideoUrl");
        }
    }
}
