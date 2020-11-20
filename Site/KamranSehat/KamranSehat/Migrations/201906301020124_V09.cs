namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V09 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Title = c.String(),
                        ImageUrl = c.String(),
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
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Title = c.String(),
                        ImageUrl = c.String(),
                        CustomerGroupId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.Guid(),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        DeleteUserId = c.Guid(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerGroups", t => t.CustomerGroupId, cascadeDelete: true)
                .Index(t => t.CustomerGroupId);
            
            CreateTable(
                "dbo.ServiceGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Order = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        Body = c.String(),
                        Name = c.String(),
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
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Order = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        Body = c.String(),
                        Name = c.String(),
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
            
            CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        FullName = c.String(),
                        Post = c.String(),
                        LinkedinUrl = c.String(),
                        InstagramUrl = c.String(),
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
            DropForeignKey("dbo.Customers", "CustomerGroupId", "dbo.CustomerGroups");
            DropIndex("dbo.Customers", new[] { "CustomerGroupId" });
            DropTable("dbo.TeamMembers");
            DropTable("dbo.Services");
            DropTable("dbo.ServiceGroups");
            DropTable("dbo.Customers");
            DropTable("dbo.CustomerGroups");
        }
    }
}
