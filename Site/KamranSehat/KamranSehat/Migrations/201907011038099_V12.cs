namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Consultations", "DayOfWeek", c => c.String());
            AddColumn("dbo.Consultations", "Time", c => c.String());
            AddColumn("dbo.Consultations", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Consultations", "Type");
            DropColumn("dbo.Consultations", "Time");
            DropColumn("dbo.Consultations", "DayOfWeek");
        }
    }
}
