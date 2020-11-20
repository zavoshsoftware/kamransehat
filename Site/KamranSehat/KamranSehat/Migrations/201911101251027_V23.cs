namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductComments", "ResponseDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ProductComments", "ResponseName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductComments", "ResponseName");
            DropColumn("dbo.ProductComments", "ResponseDate");
        }
    }
}
