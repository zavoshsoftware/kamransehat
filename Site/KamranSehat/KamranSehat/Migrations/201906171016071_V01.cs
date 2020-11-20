namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogComments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 200),
                        Email = c.String(maxLength: 256),
                        Message = c.String(nullable: false),
                        BlogId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .Index(t => t.BlogId);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Summery = c.String(),
                        ImageUrl = c.String(),
                        UrlParam = c.String(),
                        Visit = c.Int(nullable: false),
                        Body = c.String(storeType: "ntext"),
                        BlogGroupId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.BlogGroups", t => t.BlogGroupId, cascadeDelete: true)
                .Index(t => t.BlogGroupId);
            
            CreateTable(
                "dbo.BlogGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Summery = c.String(),
                        ImageUrl = c.String(),
                        UrlParam = c.String(),
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
                "dbo.Cities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProvinceId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        IsCenter = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.Provinces", t => t.ProvinceId, cascadeDelete: true)
                .Index(t => t.ProvinceId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
                        UserId = c.Guid(),
                        Address = c.String(),
                        TotalAmount = c.Decimal(nullable: false, storeType: "money"),
                        OrderStatusId = c.Guid(nullable: false),
                        CityId = c.Guid(),
                        SaleReferenceId = c.Long(),
                        IsPaid = c.Boolean(nullable: false),
                        DiscountCodeId = c.Guid(),
                        ShippingAmount = c.Decimal(precision: 18, scale: 2),
                        SubTotal = c.Decimal(precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(precision: 18, scale: 2),
                        DeliverFullName = c.String(),
                        DeliverCellNumber = c.String(),
                        PostalCode = c.String(),
                        PaymentDate = c.DateTime(),
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
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.DiscountCodes", t => t.DiscountCodeId)
                .ForeignKey("dbo.OrderStatus", t => t.OrderStatusId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.OrderStatusId)
                .Index(t => t.CityId)
                .Index(t => t.DiscountCodeId);
            
            CreateTable(
                "dbo.DiscountCodes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10),
                        ExpireDate = c.DateTime(nullable: false),
                        IsPercent = c.Boolean(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsMultiUsing = c.Boolean(nullable: false),
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
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
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
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Code = c.String(maxLength: 15),
                        Title = c.String(nullable: false, maxLength: 256),
                        PageTitle = c.String(nullable: false, maxLength: 500),
                        PageDescription = c.String(maxLength: 1000),
                        ImageUrl = c.String(maxLength: 500),
                        Summery = c.String(),
                        Body = c.String(storeType: "ntext"),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsInPromotion = c.Boolean(nullable: false),
                        IsInHome = c.Boolean(nullable: false),
                        ProductGroupId = c.Guid(nullable: false),
                        ParentId = c.Guid(),
                        Stock = c.Int(nullable: false),
                        SeedStock = c.Int(nullable: false),
                        IsUpseller = c.Boolean(nullable: false),
                        WorsthRate = c.Int(nullable: false),
                        BestRate = c.Int(nullable: false),
                        AverageRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RateCount = c.Int(nullable: false),
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
                .ForeignKey("dbo.Products", t => t.ParentId)
                .ForeignKey("dbo.ProductGroups", t => t.ProductGroupId, cascadeDelete: true)
                .Index(t => t.ProductGroupId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.ProductComments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 200),
                        Email = c.String(maxLength: 256),
                        Message = c.String(nullable: false),
                        Response = c.String(),
                        ProductId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 256),
                        UrlParam = c.String(nullable: false, maxLength: 500),
                        PageTitle = c.String(nullable: false, maxLength: 500),
                        PageDescription = c.String(maxLength: 1000),
                        ImageUrl = c.String(maxLength: 500),
                        Summery = c.String(),
                        Body = c.String(storeType: "ntext"),
                        ParentId = c.Guid(),
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
                .ForeignKey("dbo.ProductGroups", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Value = c.Int(nullable: false),
                        ProductId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 30),
                        Code = c.Int(nullable: false),
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
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Password = c.String(maxLength: 150),
                        CellNum = c.String(nullable: false, maxLength: 20),
                        FullName = c.String(nullable: false, maxLength: 250),
                        Code = c.Int(),
                        BirthDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Address = c.String(maxLength: 500),
                        PostalCode = c.String(maxLength: 11),
                        AvatarImageUrl = c.String(),
                        Email = c.String(maxLength: 256),
                        GenderId = c.Guid(),
                        RoleId = c.Guid(nullable: false),
                        Token = c.String(),
                        RemainCredit = c.Decimal(precision: 18, scale: 2),
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
                .ForeignKey("dbo.Genders", t => t.GenderId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.GenderId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 10),
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
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
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
                "dbo.ZarinpallAuthorities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Authority = c.String(),
                        OrderId = c.Guid(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
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
            DropForeignKey("dbo.Cities", "ProvinceId", "dbo.Provinces");
            DropForeignKey("dbo.ZarinpallAuthorities", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "GenderId", "dbo.Genders");
            DropForeignKey("dbo.Orders", "OrderStatusId", "dbo.OrderStatus");
            DropForeignKey("dbo.Rates", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductGroupId", "dbo.ProductGroups");
            DropForeignKey("dbo.ProductGroups", "ParentId", "dbo.ProductGroups");
            DropForeignKey("dbo.ProductComments", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "ParentId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "DiscountCodeId", "dbo.DiscountCodes");
            DropForeignKey("dbo.Orders", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Blogs", "BlogGroupId", "dbo.BlogGroups");
            DropForeignKey("dbo.BlogComments", "BlogId", "dbo.Blogs");
            DropIndex("dbo.ZarinpallAuthorities", new[] { "OrderId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Users", new[] { "GenderId" });
            DropIndex("dbo.Rates", new[] { "ProductId" });
            DropIndex("dbo.ProductGroups", new[] { "ParentId" });
            DropIndex("dbo.ProductComments", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "ParentId" });
            DropIndex("dbo.Products", new[] { "ProductGroupId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "DiscountCodeId" });
            DropIndex("dbo.Orders", new[] { "CityId" });
            DropIndex("dbo.Orders", new[] { "OrderStatusId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Cities", new[] { "ProvinceId" });
            DropIndex("dbo.Blogs", new[] { "BlogGroupId" });
            DropIndex("dbo.BlogComments", new[] { "BlogId" });
            DropTable("dbo.Provinces");
            DropTable("dbo.ZarinpallAuthorities");
            DropTable("dbo.Roles");
            DropTable("dbo.Genders");
            DropTable("dbo.Users");
            DropTable("dbo.OrderStatus");
            DropTable("dbo.Rates");
            DropTable("dbo.ProductGroups");
            DropTable("dbo.ProductComments");
            DropTable("dbo.Products");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.DiscountCodes");
            DropTable("dbo.Orders");
            DropTable("dbo.Cities");
            DropTable("dbo.BlogGroups");
            DropTable("dbo.Blogs");
            DropTable("dbo.BlogComments");
        }
    }
}
