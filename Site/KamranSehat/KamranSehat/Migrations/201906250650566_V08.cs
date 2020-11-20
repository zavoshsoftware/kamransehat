namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V08 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Consultations", "CellNumber", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Consultations", "Email", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Consultations", "Email", c => c.String());
            AlterColumn("dbo.Consultations", "CellNumber", c => c.String(nullable: false));
        }
    }
}
