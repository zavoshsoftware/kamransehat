namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V15 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "Body", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "Body", c => c.String());
        }
    }
}
