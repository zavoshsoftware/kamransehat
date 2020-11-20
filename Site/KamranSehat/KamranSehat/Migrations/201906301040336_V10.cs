namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeamMembers", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamMembers", "ImageUrl");
        }
    }
}
