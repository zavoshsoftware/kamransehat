namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "VideoPosterImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "VideoPosterImageUrl");
        }
    }
}
