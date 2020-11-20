namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V14 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CourseDetails", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CourseDetails", "Title", c => c.String());
        }
    }
}
