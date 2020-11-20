namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V07 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Consultations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Fullname = c.String(nullable: false),
                        Organization = c.String(),
                        Position = c.String(),
                        Front = c.String(),
                        CellNumber = c.String(nullable: false),
                        Email = c.String(),
                        Website = c.String(),
                        Body = c.String(storeType: "ntext"),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.Guid(),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        DeleteUserId = c.Guid(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Consultations");
        }
    }
}
