namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "RelatedProductCodes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "RelatedProductCodes");
        }
    }
}
