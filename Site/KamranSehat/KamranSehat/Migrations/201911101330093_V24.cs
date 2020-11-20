namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V24 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductComments", "ResponseDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductComments", "ResponseDate", c => c.DateTime(nullable: false));
        }
    }
}
