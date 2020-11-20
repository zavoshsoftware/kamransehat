namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V19 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Duration = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            
            AddColumn("dbo.Businesses", "PackageId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Businesses", "PackageId");
            AddForeignKey("dbo.Businesses", "PackageId", "dbo.Packages", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Businesses", "PackageId", "dbo.Packages");
            DropIndex("dbo.Businesses", new[] { "PackageId" });
            DropColumn("dbo.Businesses", "PackageId");
            DropTable("dbo.Packages");
        }
    }
}
