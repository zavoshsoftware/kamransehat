namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactForms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Message = c.String(storeType: "ntext"),
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
            DropTable("dbo.ContactForms");
        }
    }
}
